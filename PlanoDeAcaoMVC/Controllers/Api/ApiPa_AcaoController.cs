using ADOFactory;
using AutoMapper;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using PlanoAcaoCore.Acao;
using PlanoDeAcaoMVC.PaMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Acao")]
    public class ApiPa_AcaoController : BaseApiController
    {

        [HttpGet]
        [Route("List")]
        public IEnumerable<Pa_Acao> List()
        {
            return Pa_Acao.Listar();
        }

        [HttpGet]
        [Route("GET")]
        public Pa_Acao Get(int id)
        {
            return Pa_Acao.Get(id);
        }

        [HttpPost]
        [Route("Save")]
        public List<Pa_Acao> Save([FromBody] List<Pa_Acao> acao)
        {
            foreach (var i in acao)
                i.IsValid();

            foreach (var i in acao)
            {
                var acaoSaved = Mapper.Map<PlanoAcaoEF.Pa_Acao>(i);
                SalvarAcao(acaoSaved);
                CreateMail(i.Panejamento_Id, acaoSaved.Id, i.Quem_Id, Conn.TitileMailNovaAcao);
            }

            return acao;
        }

        [HttpPost]
        [Route("SaveAcompanhamento")]
        public Pa_Acompanhamento Acompanhamento(Pa_Acompanhamento obj)
        {
            var acompanhamento = Mapper.Map<PlanoAcaoEF.Pa_Acompanhamento>(obj);
            using (var db = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {
                SalvarAcompanhamento(db, acompanhamento);
                var acao = db.Pa_Acao.FirstOrDefault(r => r.Id == acompanhamento.Acao_Id);
                foreach (var i in obj.MailTo)
                {
                    Pa_AcompanhamentoXQuemVM obj2 = new Pa_AcompanhamentoXQuemVM();
                    var acomXQuem = Mapper.Map<PlanoAcaoEF.Pa_AcompanhamentoXQuem>(obj2);
                    acomXQuem.Acompanhamento_Id = acompanhamento.Id;
                    acomXQuem.Quem_Id = i;
                    SalvarAcompanhamentoXQuem(db, acomXQuem);
                    CreateMail(acao.Panejamento_Id.GetValueOrDefault(), acao.Id, acomXQuem.Quem_Id, Conn.TitileMailAcompanhamento, true);
                }
            }
            return obj;
        }

        [HttpPost]
        [Route("SaveFTA")]
        public FTA SaveFTA(FTA obj)
        {
            obj.ValidaFTA();
            obj.IsValid();
            var acao = Mapper.Map<PlanoAcaoEF.Pa_Acao>(obj);
            var fta = Mapper.Map<PlanoAcaoEF.Pa_FTA>(obj);
            SalvaFTA(fta);
            acao.Fta_Id = fta.Id;
            SalvarAcao(acao);
            CreateMail(obj.Panejamento_Id, acao.Id, obj.Quem_Id, Conn.TitileMailNovoFTA);
            return obj;
        }

        #region Auxiliares

        private void SalvarAcompanhamentoXQuem(PlanoAcaoEF.PlanoDeAcaoEntities db, PlanoAcaoEF.Pa_AcompanhamentoXQuem quem)
        {
            if (quem.Id > 0)
            {
                db.Pa_AcompanhamentoXQuem.Attach(quem);
                var entry = db.Entry(quem);
                entry.State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.Pa_AcompanhamentoXQuem.Add(quem);
                db.SaveChanges();
            }
        }

        private void SalvarAcompanhamento(PlanoAcaoEF.PlanoDeAcaoEntities db, PlanoAcaoEF.Pa_Acompanhamento acom)
        {
            if (acom.Id > 0)
            {
                acom.AlterDate = DateTime.Now;
                db.Pa_Acompanhamento.Attach(acom);
                var entry = db.Entry(acom);
                entry.State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Database.ExecuteSqlCommand("Update Pa_Acao set Status = " + acom.Status_Id + " where Id = " + acom.Acao_Id);
            }
            else
            {
                acom.AddDate = DateTime.Now;
                db.Pa_Acompanhamento.Add(acom);
                db.SaveChanges();
                db.Database.ExecuteSqlCommand("Update Pa_Acao set Status = " + acom.Status_Id + " where Id = " + acom.Acao_Id);
            }
        }

        private void SalvarAcao(PlanoAcaoEF.Pa_Acao acao)
        {
            //var acao = Mapper.Map<PlanoAcaoEF.Pa_Acao>(obj);

            GetLevelName(acao);

            GetUnidadeName(acao);

            GetRegionalName(acao);

            Salvar(acao);

        }

        private static void GetRegionalName(PlanoAcaoEF.Pa_Acao acao)
        {
            if (acao.Unidade_Id > 0)
            {
                //try
                //{
                using (var db = new Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
                {
                    var Regional = db.QueryNinjaADO("SELECT PS.Id as 'Id', PS.Name as 'Name' FROM ParStructure PS " +
                                    "Inner join ParCompanyXStructure PC on PC.ParStructure_Id = PS.Id " +
                                    "WHERE PC.ParCompany_ID =" + acao.Unidade_Id).FirstOrDefault();
                    if (Regional != null)
                    {
                        acao.Regional = Regional.GetValue("Name").Value<string>();
                    }
                }
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
            }
        }

        private void GetUnidadeName(PlanoAcaoEF.Pa_Acao acao)
        {

            if (acao.Unidade_Id > 0)
            {
                using (var db = new PlanoAcaoEF.PlanoDeAcaoEntities())
                {
                    acao.UnidadeName = QueryNinja(db, "SELECT * from PA_UNIDADE WHERE ID = " + acao.Unidade_Id).FirstOrDefault().GetValue("Description").Value<string>();
                }
            }
        }

        private void GetLevelName(PlanoAcaoEF.Pa_Acao acao)
        {
            using (var db = new Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
            {
                try
                {
                    if (acao.Level1Id > 0)
                    {
                        acao.Level1Name = db.QueryNinjaADO("SELECT Name FROM ParLevel1 WHERE ID = " + acao.Level1Id).FirstOrDefault().GetValue("Name").Value<string>();
                    }

                    if (acao.Level2Id > 0)
                    {
                        acao.Level2Name = db.QueryNinjaADO("SELECT Name FROM ParLevel2 WHERE ID = " + acao.Level2Id).FirstOrDefault().GetValue("Name").Value<string>();
                    }

                    if (acao.Level3Id > 0)
                    {
                        acao.Level3Name = db.QueryNinjaADO("SELECT Name FROM ParLevel3 WHERE ID = " + acao.Level3Id).FirstOrDefault().GetValue("Name").Value<string>();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        private static void Salvar(PlanoAcaoEF.Pa_Acao acao)
        {
            using (var db = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {
                if (acao.Id > 0)
                {
                    acao.AlterDate = DateTime.Now;
                    db.Pa_Acao.Attach(acao);
                    var entry = db.Entry(acao);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    entry.Property(e => e.AddDate).IsModified = false;
                    db.SaveChanges();
                }
                else
                {
                    acao.AddDate = DateTime.Now;
                    db.Pa_Acao.Add(acao);
                    db.SaveChanges();
                }
            }
        }

        private void SalvaFTA(PlanoAcaoEF.Pa_FTA fta)
        {
            // var fta = Mapper.Map<PlanoAcaoEF.Pa_FTA>(obj);

            using (var db = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {
                if (fta.Id > 0)
                {
                    fta.AddDate = DateTime.Now;
                    db.Pa_FTA.Attach(fta);
                    var entry = db.Entry(fta);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    fta.AddDate = DateTime.Now;
                    db.Pa_FTA.Add(fta);
                    db.SaveChanges();
                }
            }

        }

        #endregion

    }

}

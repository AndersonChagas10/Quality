using ADOFactory;
using AutoMapper;
using DTO;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using PlanoAcaoCore.Acao;
using PlanoAcaoCore.Enum;
using PlanoDeAcaoMVC.PaMail;
using PlanoDeAcaoMVC.SgqIntegracao;
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
            bool IsFTA = false;

            foreach (var i in acao)
                i.IsValid();

            foreach (var i in acao)
            {
                var acaoSaved = Mapper.Map<Dominio.Pa_Acao>(i);
                SalvarAcao(acaoSaved, IsFTA);
                CreateMail(i.Panejamento_Id, acaoSaved.Id, i.Quem_Id, Conn.TitileMailNovaAcao);
            }

            return acao;
        }

        [HttpPost]
        [Route("SaveAcompanhamento")]
        public Pa_Acompanhamento Acompanhamento(Pa_Acompanhamento obj)
        {
            var acompanhamento = Mapper.Map<Dominio.Pa_Acompanhamento>(obj);

            using (var db = new Dominio.SgqDbDevEntities())
            {
                var acao = db.Pa_Acao.FirstOrDefault(r => r.Id == acompanhamento.Acao_Id);

                var statusDaAcao = GetStatusAcao(acompanhamento, acao);

                SalvarAcompanhamento(db, acompanhamento, statusDaAcao);

                foreach (var i in obj.MailTo)
                {
                    Pa_AcompanhamentoXQuemVM obj2 = new Pa_AcompanhamentoXQuemVM();
                    var acomXQuem = Mapper.Map<Dominio.Pa_AcompanhamentoXQuem>(obj2);
                    acomXQuem.Acompanhamento_Id = acompanhamento.Id;
                    acomXQuem.Quem_Id = i;
                    SalvarAcompanhamentoXQuem(db, acomXQuem);
                    CreateMail(acao.Panejamento_Id.GetValueOrDefault(), acao.Id, acomXQuem.Quem_Id, Conn.TitileMailAcompanhamento, true);
                }
            }

            return obj;
        }


        private int GetStatusAcao(Dominio.Pa_Acompanhamento acompanhamento, Dominio.Pa_Acao acao)
        {
            switch (acompanhamento.Status_Id)
            {
                case (int)Enums.Status.Concluido:

                    if (DateTime.Now.Date > acao.QuandoFim)
                    {
                        return (int)Enums.Status.ConcluidoComAtraso;
                    }

                    return acompanhamento.Status_Id;

                case (int)Enums.Status.Aberto:

                    if (DateTime.Now.Date >= acao.QuandoInicio && DateTime.Now.Date <= acao.QuandoFim)
                    {
                        return (int)Enums.Status.EmAndamento;
                    }
                    else if (DateTime.Now.Date > acao.QuandoFim)
                    {
                        return (int)Enums.Status.Atrasado;
                    }
                    else if (DateTime.Now.Date < acao.QuandoInicio)
                    {
                        return (int)Enums.Status.NaoIniciado;
                    }

                    return acompanhamento.Status_Id;

                default:

                    return acompanhamento.Status_Id;

            }
        }

        [HttpPost]
        [Route("SaveFTA")]
        public FTA SaveFTA(FTA obj)
        {
            if (GlobalConfig.Brasil || GlobalConfig.SESMT)
                obj.Panejamento_Id = 128; //Mock do ID Tático Genérico que vinculas as Ações
            else
                obj.Panejamento_Id = 3;

            try
            {
                obj.ValidaFTA();
            }
            catch (Exception ex)
            {
                throw (new Exception("obj.ValidaFTA(); " + ex.StackTrace.ToString()));
            }

            try
            {
                obj.IsValid();
            }
            catch (Exception ex)
            {
                throw (new Exception("obj.IsValid(); " + ex.StackTrace.ToString()));
            }

            var acao = new Dominio.Pa_Acao();
            var fta = new Dominio.Pa_FTA();

            try
            {
                acao = Mapper.Map<Dominio.Pa_Acao>(obj);
                acao.IsActive = true;
            }
            catch (Exception ex)
            {
                throw (new Exception("acao = Mapper.Map<PlanoAcaoEF.Pa_Acao>(obj); " + ex.StackTrace.ToString()));
            }

            try
            {
                fta = Mapper.Map<Dominio.Pa_FTA>(obj);
            }
            catch (Exception ex)
            {
                throw (new Exception("fta = Mapper.Map<PlanoAcaoEF.Pa_FTA>(obj); " + ex.StackTrace.ToString()));
            }

            try
            {
                SalvaFTA(fta);
            }
            catch (Exception ex)
            {
                throw (new Exception("SalvaFTA(fta); " + ex.StackTrace.ToString()));
            }

            try
            {
                acao.Fta_Id = fta.Id;
            }
            catch (Exception ex)
            {
                throw (new Exception("acao.Fta " + ex.StackTrace.ToString()));
            }

            try
            {
                SalvarAcao(acao, obj.IsFTA);
            }
            catch (Exception ex)
            {
                throw (new Exception(" SalvarAcao(acao, obj.IsFTA); " + ex.StackTrace.ToString()));
            }

            try
            {
                CreateMail(obj.Panejamento_Id, acao.Id, obj.Quem_Id, Conn.TitileMailNovoFTA);
            }
            catch (Exception ex)
            {
                throw (new Exception("CreateMail(obj.Panejamento_Id, acao.Id, obj.Quem_Id, Conn.TitileMailNovoFTA); " + ex.StackTrace.ToString()));
            }

            return obj;
        }

        #region Auxiliares

        private void SalvarAcompanhamentoXQuem(Dominio.SgqDbDevEntities db, Dominio.Pa_AcompanhamentoXQuem quem)
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

        private void SalvarAcompanhamento(Dominio.SgqDbDevEntities db, Dominio.Pa_Acompanhamento acom, int statusDaAcao)
        {
            if (acom.Id > 0)
            {
                acom.AlterDate = DateTime.Now;
                db.Pa_Acompanhamento.Attach(acom);
                var entry = db.Entry(acom);
                entry.State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Database.ExecuteSqlCommand("Update Pa_Acao set Status = " + statusDaAcao + " where Id = " + acom.Acao_Id);
            }
            else
            {
                acom.AddDate = DateTime.Now;
                db.Pa_Acompanhamento.Add(acom);
                db.SaveChanges();
                db.Database.ExecuteSqlCommand("Update Pa_Acao set Status = " + statusDaAcao + " where Id = " + acom.Acao_Id);
            }
        }

        private void SalvarAcao(Dominio.Pa_Acao acao, bool IsFTA)
        {
            //var acao = Mapper.Map<PlanoAcaoEF.Pa_Acao>(obj);

            GetLevelName(acao);

            GetUnidadeName(acao, IsFTA);

            GetRegionalName(acao);

            Salvar(acao);

        }

        private static void GetRegionalName(Dominio.Pa_Acao acao)
        {
            if (acao.Unidade_Id > 0)
            {
                //try
                //{
                using (var dbSgq = new ConexaoSgq().db)
                {
                    var Regional = dbSgq.QueryNinjaADO("SELECT PS.Id as 'Id', PS.Name as 'Name' FROM ParStructure PS " +
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

        private void GetUnidadeName(Dominio.Pa_Acao acao, bool IsFTA)
        {
            if (acao.Unidade_Id > 0)
            {
                if (IsFTA)//Pelo FTA
                {
                    //GetName
                    using (var dbSgq = new ConexaoSgq().db)
                    {
                        acao.UnidadeName = dbSgq.QueryNinjaADO("SELECT Name FROM ParCompany WHERE ID = " + acao.Unidade_Id).FirstOrDefault().GetValue("Name").Value<string>();
                    }

                    //Atribui o Id da Unidade do PA na Acao
                    using (var dbPa = new Dominio.SgqDbDevEntities())
                    {
                        acao.Unidade_Id = QueryNinja(dbPa, "SELECT Id from PA_UNIDADE WHERE DESCRIPTION = '" + acao.UnidadeName + "'").FirstOrDefault().GetValue("Id").Value<int>();
                    }
                }
                else//Pelo PA
                {
                    using (var dbPa = new Dominio.SgqDbDevEntities())
                    {
                        acao.UnidadeName = QueryNinja(dbPa, "SELECT * from PA_UNIDADE WHERE ID = " + acao.Unidade_Id).FirstOrDefault().GetValue("Description").Value<string>();
                    }
                }

            }
        }

        private void GetLevelName(Dominio.Pa_Acao acao)
        {
            using (var dbSgq = new ConexaoSgq().db)
            {
                try
                {
                    if (acao.Level1Id > 0)
                    {
                        acao.Level1Name = dbSgq.QueryNinjaADO("SELECT Name FROM ParLevel1 WHERE ID = " + acao.Level1Id).FirstOrDefault().GetValue("Name").Value<string>();
                    }

                    if (acao.Level2Id > 0)
                    {
                        acao.Level2Name = dbSgq.QueryNinjaADO("SELECT Name FROM ParLevel2 WHERE ID = " + acao.Level2Id).FirstOrDefault().GetValue("Name").Value<string>();
                    }

                    if (acao.Level3Id > 0)
                    {
                        acao.Level3Name = dbSgq.QueryNinjaADO("SELECT Name FROM ParLevel3 WHERE ID = " + acao.Level3Id).FirstOrDefault().GetValue("Name").Value<string>();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        private static void Salvar(Dominio.Pa_Acao acao)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                try
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
                catch (Exception ex)
                {

                }

            }
        }

        private void SalvaFTA(Dominio.Pa_FTA fta)
        {
   
            using (var db = new Dominio.SgqDbDevEntities())
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

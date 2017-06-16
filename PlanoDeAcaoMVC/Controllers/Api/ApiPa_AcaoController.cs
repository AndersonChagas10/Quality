using ADOFactory;
using AutoMapper;
using DTO.Helpers;
using Helper;
using PlanoAcaoCore;
using PlanoAcaoCore.Acao;
using PlanoDeAcaoMVC.PaMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

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
                SalvarAcao(Mapper.Map<PlanoAcaoEF.Pa_Acao>(i));

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
                var acao = db.Pa_Acao.FirstOrDefault(r=>r.Id == acompanhamento.Acao_Id);

                foreach (var i in obj.MailTo)
                {
                    Pa_AcompanhamentoXQuemVM obj2 = new Pa_AcompanhamentoXQuemVM();

                    var acomXQuem = Mapper.Map<PlanoAcaoEF.Pa_AcompanhamentoXQuem>(obj2);

                    acomXQuem.Acompanhamento_Id = acompanhamento.Id;
                    acomXQuem.Quem_Id = i;
                    SalvarAcompanhamentoXQuem(db, acomXQuem);
                    
                    //Task.Run(() => CreateMail(acao.Panejamento_Id.GetValueOrDefault(), acao.Id, acomXQuem.Quem_Id, Request.RequestUri.Authority));
                    CreateMail(acao.Panejamento_Id.GetValueOrDefault(), acao.Id, "celsogea@hotmail.com", Request.RequestUri.Authority, "Atualização de acompanhamento do Plano de Ação.");
                    //CreateMail(acao.Panejamento_Id.GetValueOrDefault(), acao.Id, "celso.bernar@grtsolucoes.com.br", Request.RequestUri.Authority);

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

            Task.Run(() => CreateMail(obj.Panejamento_Id, acao.Id, obj.Quem_Id, Request.RequestUri.Authority, "Novo Relatório de Análise de Desvio criado."));
            //CreateMail(obj.Panejamento_Id, acao.Id, "celsogea@hotmail.com", Request.RequestUri.Authority, "Novo Relatório de Análise de Desvio criado.");

            return obj;
        }

        #region Auxiliares

        private void CreateMail(int idPlanejamento, int idAcao, int idQuem, string path, string title)
        {

            //using (var ct = new Pa_PlanejamentoController())
            //{
            //    var teste = ct.Details(idPlanejamento);
            //}
            var conteudoPlanejamento = GetExternalResponse("http:/" + path + "/Pa_Planejamento/Details?id=" + idPlanejamento);
            var conteudoAcao = GetExternalResponse("http:/" + path + "/Pa_Acao/Details?id=" + idAcao);

            var todoConteudo = conteudoPlanejamento.Result + conteudoAcao.Result;


            using (var db = new Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
            {
                var paUser = Pa_Quem.Get(idQuem);
                dynamic enviarPara = db.QueryNinjaADO("SELECT * FROM UserSgq WHERE Name  = '" + paUser.Name + "'").FirstOrDefault();

                var email = new PlanoAcaoEF.EmailContent()
                {
                    IsBodyHtml = true,
                    AddDate = DateTime.Now,
                    Subject = title,
                    Project = "Plano de Ação",
                    Body = todoConteudo,
                    To = enviarPara.Email,
                };

                PaAsyncServices.SendMailPATeste(email, "celso.bernar@grtsolucoes.com.br");

            }
        }


        private void CreateMail(int idPlanejamento, int idAcao, string emailTo, string path, string title)
        {

            //using (var ct = new Pa_PlanejamentoController())
            //{
            //    var teste = ct.Details(idPlanejamento);
            //}
            var conteudoPlanejamento = GetExternalResponse("http:/" + path + "/Pa_Planejamento/Details?id=" + idPlanejamento);
            var conteudoAcao = GetExternalResponse("http:/" + path + "/Pa_Acao/Details?id=" + idAcao);
            var todoConteudo = conteudoPlanejamento.Result + conteudoAcao.Result;
            //var todoConteudo = "teste";


            using (var db = new Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
            {
                //var paUser = Pa_Quem.Get(idQuem);
                //dynamic enviarPara = db.QueryNinjaADO("SELECT * FROM UserSgq WHERE Name  = '" + paUser.Name + "'").FirstOrDefault();
                var email = new PlanoAcaoEF.EmailContent()
                {
                    IsBodyHtml = true,
                    AddDate = DateTime.Now,
                    Subject = title,
                    Project = "Plano de Ação",
                    Body = todoConteudo,
                    To = emailTo,
                };

                PaAsyncServices.SendMailPATeste(email, "celso.bernar@grtsolucoes.com.br");

            }
        }

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

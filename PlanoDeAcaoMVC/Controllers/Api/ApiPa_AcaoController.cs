using AutoMapper;
using DTO.Helpers;
using Helper;
using PlanoAcaoCore;
using PlanoAcaoCore.Acao;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Acao")]
    public class ApiPa_AcaoController : ApiController
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

                foreach (var i in obj.MailTo)
                {
                    Pa_AcompanhamentoXQuemVM obj2 = new Pa_AcompanhamentoXQuemVM();

                    var acomXQuem = Mapper.Map<PlanoAcaoEF.Pa_AcompanhamentoXQuem>(obj2);

                    acomXQuem.Acompanhamento_Id = acompanhamento.Id;
                    acomXQuem.Quem_Id = i;
                    SalvarAcompanhamentoXQuem(db, acomXQuem);
                }
            }

            return obj;
        }

        public static void SalvarAcompanhamentoXQuem(PlanoAcaoEF.PlanoDeAcaoEntities db, PlanoAcaoEF.Pa_AcompanhamentoXQuem quem)
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

        public static void SalvarAcompanhamento(PlanoAcaoEF.PlanoDeAcaoEntities db, PlanoAcaoEF.Pa_Acompanhamento acom)
        {
            if (acom.Id > 0)
            {
                acom.AlterDate = DateTime.Now;
                db.Pa_Acompanhamento.Attach(acom);
                var entry = db.Entry(acom);
                entry.State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                acom.AddDate = DateTime.Now;
                db.Pa_Acompanhamento.Add(acom);
                db.SaveChanges();
            }
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

            return obj;
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
    }
}

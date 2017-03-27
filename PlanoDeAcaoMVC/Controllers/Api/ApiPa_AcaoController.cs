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

            using (var db = new PlanoAcaoEF.PlanoDeAcaoEntities())
                foreach (var i in acao)
                    SalvarAcao(db, Mapper.Map<PlanoAcaoEF.Pa_Acao>(i));

            return acao;
        }

       

        [HttpPost]
        [Route("SaveAcompanhamento")]
        public Pa_Acompanhamento Acompanhamento(Pa_Acompanhamento obj)
        {
            //Pa_BaseObject.SalvarGenerico(obj);
            //Pa_Acompanhamento.SalvarGenerico(obj);
            //var obj = Pa_Acao.Get(id);
            return Pa_BaseObject.SalvarGenerico(obj); 
        }

        [HttpPost]
        [Route("SaveFTA")]
        public FTA SaveFTA(FTA obj)
        {
            obj.ValidaFTA();

            obj.IsValid();

            var acao = Mapper.Map<PlanoAcaoEF.Pa_Acao>(obj);

            var fta = Mapper.Map<PlanoAcaoEF.Pa_FTA>(obj);
            
            using (var db = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {
                SalvaFTA(db, fta);
                acao.Fta_Id = fta.Id;
                SalvarAcao(db, acao);
            }

            return Pa_BaseObject.SalvarGenerico(obj);
        }

        private static void SalvarAcao(PlanoAcaoEF.PlanoDeAcaoEntities db, PlanoAcaoEF.Pa_Acao acao)
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

        private static void SalvaFTA(PlanoAcaoEF.PlanoDeAcaoEntities db, PlanoAcaoEF.Pa_FTA fta)
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

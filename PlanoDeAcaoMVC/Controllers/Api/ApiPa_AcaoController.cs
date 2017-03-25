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
            {
                foreach (var i in acao)
                {

                    //throw new Exception("treste");
                    if (i._QuantoCusta != null)
                        i.QuantoCusta = NumericExtensions.CustomParseDecimal(i._QuantoCusta).GetValueOrDefault();

                    if (i._QuandoInicio != null)
                        i.QuandoInicio = Guard.ParseDateToSqlV2(i._QuandoInicio);
                    else
                        i.QuandoInicio = DateTime.Now;

                    if (i._QuandoFim != null)
                        i.QuandoFim = Guard.ParseDateToSqlV2(i._QuandoFim);
                    else
                        i.QuandoFim = DateTime.Now;

                    i._QuandoInicio = null;
                    i._QuandoFim = null;

                    //Pa_BaseObject.SalvarGenerico(acao);
                    //Pa_BaseObject.SalvarGenerico(acao.CausaMedidasXAcao);

                    //Pa_BaseObject.SalvarGenerico(i);

                    var a = Mapper.Map<PlanoAcaoEF.Pa_Acao>(i);

                    if (a.Id > 0)
                    {
                        db.Pa_Acao.Attach(a);
                        var entry = db.Entry(a);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        //entry.Property(e => e.Email).IsModified = true;
                        // other changed properties
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Pa_Acao.Add(a);
                        db.SaveChanges();
                    }
                }
            }

            return acao;
        }


        [HttpPost]
        [Route("SaveAcompanhamento")]
        public Pa_Acompanhamento Acompanhamento(Pa_Acompanhamento obj)
        {
            //Pa_BaseObject.SalvarGenerico(obj);
            //Pa_Acompanhamento.SalvarGenerico(obj);
            //var obj = Pa_Acao.Get(id);
            //return Pa_BaseObject.SalvarGenerico(obj);

            using (var db = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {

                var a = Mapper.Map<PlanoAcaoEF.Pa_Acompanhamento>(obj);

                if (a.Id > 0)
                {
                    a.AlterDate = DateTime.Now;
                    db.Pa_Acompanhamento.Attach(a);
                    var entry = db.Entry(a);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    //entry.Property(e => e.Email).IsModified = true;
                    // other changed properties
                    db.SaveChanges();
                }
                else
                {
                    a.AddDate = DateTime.Now;
                    db.Pa_Acompanhamento.Add(a);
                    db.SaveChanges();
                }
            }

            return obj;
        }

    }
}

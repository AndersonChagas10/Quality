using DTO.Helpers;
using Helper;
using PlanoAcaoCore;
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

                Pa_BaseObject.SalvarGenerico(i);

                i.AddOrUpdate();
            }

            return acao;
        }

        
        //[HttpPost]
        //[Route("SaveAcompnhamento")]
        //public Pa_Acompanhamento Acompanhamento(Pa_Acompanhamento id)
        //{
        //    var obj = Pa_Acao.Get(id);
        //    return Pa_Acompanhamento
        //}


    }
}

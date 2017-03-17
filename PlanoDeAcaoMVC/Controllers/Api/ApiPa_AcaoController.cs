using DTO.Helpers;
using Helper;
using PlanoAcaoCore;
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
            foreach(var i in acao)
                i.IsValid();

            foreach (var i in acao)
            {
                //throw new Exception("treste");
                i.QuantoCusta = NumericExtensions.CustomParseDecimal(i._QuantoCusta).GetValueOrDefault();
                i.QuandoInicio = Guard.ParseDateToSqlV2(i._QuandoInicio);
                i.QuandoFim = Guard.ParseDateToSqlV2(i._QuandoFim);
                i._QuandoInicio = null;
                i._QuandoFim = null;
                
                //Pa_BaseObject.SalvarGenerico(acao);
                //Pa_BaseObject.SalvarGenerico(acao.CausaMedidasXAcao);

                i.AddOrUpdate();
            }

            return acao;
        }
    }
}

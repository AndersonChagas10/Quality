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
        public Pa_Acao Save([FromBody] Pa_Acao acao)
        {
            acao.QuantoCusta = NumericExtensions.CustomParseDecimal(acao._QuantoCusta).GetValueOrDefault();
            acao.QuandoInicio = Guard.ParseDateToSqlV2(acao._QuandoInicio);
            acao.QuandoFim = Guard.ParseDateToSqlV2(acao._QuandoFim);
            acao._QuandoInicio = null;
            acao._QuandoFim = null;
            //Pa_BaseObject.SalvarGenerico(acao);
            //Pa_BaseObject.SalvarGenerico(acao.CausaMedidasXAcao);

            acao.AddOrUpdate();
            return acao;
        }
    }
}

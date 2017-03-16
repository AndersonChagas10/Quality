using DTO.Helpers;
using Helper;
using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Planejamento")]
    public class ApiPa_PlanejamentoController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IEnumerable<Pa_Planejamento> List()
        {
            return Pa_Planejamento.Listar();
        }

        [HttpGet]
        [Route("GET")]
        public Pa_Planejamento Get(int id)
        {
            return Pa_Planejamento.Get(id);
        }

        [HttpPost]
        [Route("Save")]
        public Pa_Planejamento Save([FromBody]Pa_Planejamento planejamento)
        {
            planejamento.IsfiltrarAcao = null;
            if (planejamento.Estrategico_Id.GetValueOrDefault() > 0)
            {
                planejamento.ValorDe = NumericExtensions.CustomParseDecimal(planejamento._ValorDe).GetValueOrDefault();
                planejamento.ValorPara = NumericExtensions.CustomParseDecimal(planejamento._ValorPara).GetValueOrDefault();
                planejamento.DataInicio = Guard.ParseDateToSqlV2(planejamento._DataInicio);
                planejamento.DataFim = Guard.ParseDateToSqlV2(planejamento._DataFim);
            }
           
            Pa_BaseObject.SalvarGenerico(planejamento);
            return planejamento;
        }

        [HttpPost]
        [Route("GetPlanejamentoAcao")]
        public IEnumerable<Pa_Planejamento> GetPlanejamentoAcao()
        {
            var retorno = Pa_Planejamento.GetPlanejamentoAcao();
            return retorno;
        }

    }
}

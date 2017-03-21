using DTO.Helpers;
using Helper;
using PlanoAcaoCore;
using System.Collections.Generic;
using System.Linq;
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
            var planejamento = Pa_Planejamento.Get(id);
            if (planejamento.Estrategico_Id.GetValueOrDefault() > 0)
            {
                planejamento.Tatico_Id = Pa_BaseObject.ListarGenerico<Pa_Planejamento>("Select * from Pa_Planejamento where Estrategico_Id = " + planejamento.Estrategico_Id.GetValueOrDefault()).FirstOrDefault().Id;
            }
            return planejamento;
        }

        [HttpPost]
        [Route("Save")]
        public Pa_Planejamento Save([FromBody]Pa_Planejamento planejamento)
        {
            planejamento.IsValid();

            planejamento.IsfiltrarAcao = null;
            planejamento.Tatico_Id = null;
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
            foreach (var i in retorno)
            {
                var planejamento = i;
                if (planejamento.Estrategico_Id.GetValueOrDefault() > 0)
                {
                    planejamento.Tatico_Id = Pa_BaseObject.ListarGenerico<Pa_Planejamento>("Select * from Pa_Planejamento where Estrategico_Id = " + planejamento.Estrategico_Id.GetValueOrDefault()).FirstOrDefault().Id;
                }
            }
            return retorno;
        }

    }
}

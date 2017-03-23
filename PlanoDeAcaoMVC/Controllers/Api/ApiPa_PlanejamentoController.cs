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
                //planejamento.Tatico_Id = Pa_BaseObject.ListarGenerico<Pa_Planejamento>("Select * from Pa_Planejamento where Estrategico_Id = " + planejamento.Estrategico_Id.GetValueOrDefault()).FirstOrDefault().Id;
            }
            return planejamento;
        }

        [HttpPost]
        [Route("GetPlanejamentoAcao")]
        public IEnumerable<Pa_Planejamento> GetPlanejamentoAcao()
        {
            var retorno = Pa_Planejamento.GetPlanejamentoAcao();
            foreach (var i in retorno)
            {

                if (i.Estrategico_Id.GetValueOrDefault() > 0)
                {
                    //i.Tatico_Id = Pa_BaseObject.ListarGenerico<Pa_Planejamento>("Select * from Pa_Planejamento where Estrategico_Id = " + i.Tatico_Id.GetValueOrDefault()).FirstOrDefault().Id;
                }
            }
            return retorno;
        }

        [HttpPost]
        [Route("Save")]
        public Pa_Planejamento Save([FromBody]Pa_Planejamento planejamento)
        {
            planejamento.IsValid();

            planejamento.IsfiltrarAcao = null;

            if (planejamento.Estrategico_Id.GetValueOrDefault() > 0)
            {
                planejamento.ValorDe = NumericExtensions.CustomParseDecimal(planejamento._ValorDe).GetValueOrDefault();
                planejamento.ValorPara = NumericExtensions.CustomParseDecimal(planejamento._ValorPara).GetValueOrDefault();
                planejamento.DataInicio = Guard.ParseDateToSqlV2(planejamento._DataInicio);
                planejamento.DataFim = Guard.ParseDateToSqlV2(planejamento._DataFim);
            }

            if (!planejamento.IsTatico)
            {
                planejamento.Tatico_Id = null;
                planejamento.Gerencia_Id = 0;
                planejamento.Coordenacao_Id = 0;
                planejamento.Iniciativa_Id = 0;
                planejamento.ObjetivoGerencial_Id = 0;
                planejamento.Responsavel_Projeto = 0;
                planejamento.UnidadeDeMedida_Id = 0;
                planejamento.IndicadoresDeProjeto_Id = 0;
            }
            else if(planejamento.IsTatico && planejamento.Tatico_Id.GetValueOrDefault() > 0)
            {
                planejamento.Id = planejamento.Tatico_Id.GetValueOrDefault();
            }

            Pa_BaseObject.SalvarGenerico(planejamento);

            #region GAMBIARRA FDP

            if (planejamento.IsTatico)
            {
                planejamento.Tatico_Id = planejamento.Id;
                Pa_BaseObject.SalvarGenerico(planejamento);
            }

            #endregion

            return planejamento;
        }

       

    }
}

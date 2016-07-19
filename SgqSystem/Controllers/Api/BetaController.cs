using Application.Interface;
using AutoMapper;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BetaApiController : ApiController
    {

        private readonly IBetaAppService _betaAppService;

        public BetaApiController(IBetaAppService betaAppService)
        {
            _betaAppService = betaAppService;
        }

        #region Coleta de Dados

        [Route("api/Result/Salvar")]
        public GenericReturnViewModel<ResultOldViewModel> Post([FromBody] ResultOldViewModel data)
        {
            var queryResult = _betaAppService.Salvar(data);
            return Mapper.Map<GenericReturn<ResultOldDTO>, GenericReturnViewModel<ResultOldViewModel>>(queryResult);
        }

        [Route("api/Result/SalvarLista")]
        public GenericReturnViewModel<ResultOldViewModel> SalvarLista([FromBody] ResultOldViewModel data)
        {
            var queryResult = _betaAppService.SalvarLista(data.listaResultado);
            return Mapper.Map<GenericReturn<ResultOldDTO>, GenericReturnViewModel<ResultOldViewModel>>(queryResult);
        }

        #endregion

        #region Retorno de Dados

        [Route("api/RelatorioBetaApi/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorIndicador(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorIndicador(idIndicador, dateInit, dateEnd);
            return  Mapper.Map<GenericReturn<List<ResultOldDTO>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorMonitoramento")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorMonitoramento(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorMonitoramento(idIndicador, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ResultOldDTO>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorMonitoramentoJelsafa")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorMonitoramentoJelsafa(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorMonitoramentoJelsafa(idIndicador, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ResultOldDTO>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorTarefa")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorTarefa(int indicadorId, int idMonitoramento, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorTarefa(indicadorId, idMonitoramento, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ResultOldDTO>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
        }

        #endregion

    }
}

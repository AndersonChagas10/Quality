using Application.Interface;
using AutoMapper;
using Dominio.Entities;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RelatorioBetaApiController : ApiController
    {

        private readonly IRelatorioBetaAppService _relatorioBetaAppService;

        public RelatorioBetaApiController(IRelatorioBetaAppService relatorioBetaAppService)
        {
            _relatorioBetaAppService = relatorioBetaAppService;
        }

        [Route("api/RelatorioBetaApi/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorIndicador(int idIndicador, string dateInit, string dateEnd)
        {
            try
            {
                var queryResult = _relatorioBetaAppService.GetNcPorIndicador(idIndicador, dateInit, dateEnd);
                var result = Mapper.Map<GenericReturn<List<ResultOld>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
                return result;
            }
            catch (Exception ex)
            {
                return new GenericReturnViewModel<List<ResultOldViewModel>>(ex, "Não foi possível carregar o grafico.");
            }
        }

        [Route("api/RelatorioBetaApi/GetNcPorMonitoramento")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorMonitoramento(int idIndicador, string dateInit, string dateEnd)
        {
            try
            {
                var queryResult = _relatorioBetaAppService.GetNcPorMonitoramento(idIndicador, dateInit, dateEnd);
                var result = Mapper.Map<GenericReturn<List<ResultOld>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
                return result;
            }
            catch (Exception ex)
            {
                return new GenericReturnViewModel<List<ResultOldViewModel>>(ex, "Não foi possível carregar o grafico.");
            }
        }

        [Route("api/RelatorioBetaApi/GetNcPorMonitoramentoJelsafa")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorMonitoramentoJelsafa(int idIndicador, string dateInit, string dateEnd)
        {
            try
            {
                var queryResult = _relatorioBetaAppService.GetNcPorMonitoramentoJelsafa(idIndicador, dateInit, dateEnd);
                var result = Mapper.Map<GenericReturn<List<ResultOld>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
                return result;
            }
            catch (Exception ex)
            {
                return new GenericReturnViewModel<List<ResultOldViewModel>>(ex, "Não foi possível carregar o grafico.");
            }
        }

        [Route("api/RelatorioBetaApi/GetNcPorTarefa")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorTarefa(int indicadorId, int idMonitoramento, string dateInit, string dateEnd)
        {
            try
            {
                var queryResult = _relatorioBetaAppService.GetNcPorTarefa(indicadorId, idMonitoramento, dateInit, dateEnd);
                var result = Mapper.Map<GenericReturn<List<ResultOld>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
                return result;
            }
            catch (Exception ex)
            {
                return new GenericReturnViewModel<List<ResultOldViewModel>>(ex, "Não foi possível carregar o grafico.");
            }
        }
    }
}

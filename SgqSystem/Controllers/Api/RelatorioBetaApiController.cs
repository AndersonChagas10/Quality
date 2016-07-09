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

        [Route("api/RelatorioBeta/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorIndicador(int idIndicador)
        {
            try
            {
                var teste = _relatorioBetaAppService.GetNcPorIndicador(idIndicador);
                var result = Mapper.Map<GenericReturn<List<ResultOld>>, GenericReturnViewModel<List<ResultOldViewModel>>>(teste);
                return result;
            }
            catch (Exception ex)
            {
                return new GenericReturnViewModel<List<ResultOldViewModel>>(ex, "Não foi possível carregar o grafico.");
            }
        }

        [Route("api/RelatorioBeta/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorMonitoramento(int idIndicador)
        {
            //try
            //{
            //    var teste = _relatorioBetaAppService.GetNcPorMonitoramento(idIndicador);
            //    var result = Mapper.Map<GenericReturn<List<ResultOld>>, GenericReturnViewModel<List<ResultOldViewModel>>>(teste);
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    return new GenericReturnViewModel<List<ResultOldViewModel>>(ex, "Não foi possível carregar o grafico.");
            //}
        }

        [Route("api/RelatorioBeta/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorTarefa(int idIndicador)
        {
            //try
            //{
            //    var teste = _relatorioBetaAppService.GetNcPorTarefa(idIndicador);
            //    var result = Mapper.Map<GenericReturn<List<ResultOld>>, GenericReturnViewModel<List<ResultOldViewModel>>>(teste);
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    return new GenericReturnViewModel<List<ResultOldViewModel>>(ex, "Não foi possível carregar o grafico.");
            //}
        }
    }
}

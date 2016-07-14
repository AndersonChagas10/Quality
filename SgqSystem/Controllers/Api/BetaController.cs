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
    public class BetaApiController : ApiController
    {

        private readonly IBetaAppService _betaAppService;

        public BetaApiController(IBetaAppService betaAppService)
        {
            _betaAppService = betaAppService;
        }

        #region Coleta de Dados

        [Route("api/Result/Salvar")]
        public GenericReturnViewModel<ResultOldViewModel> Post([FromBody] ResultOldViewModel result)
        {
            try
            {
                var resultToSave = Mapper.Map<ResultOldViewModel, ResultOld>(result);
                var response = _betaAppService.Salvar(resultToSave);
                var responseViewModel = Mapper.Map<GenericReturn<ResultOld>, GenericReturnViewModel<ResultOldViewModel>>(response);
                return responseViewModel;
            }
            catch (Exception e)
            {
                return new GenericReturnViewModel<ResultOldViewModel>(e);
            }
        }

        [Route("api/Result/SalvarLista")]
        public GenericReturnViewModel<ResultOldViewModel> SalvarLista([FromBody] List<ResultOldViewModel> obj)
        {
            try
            {
                //List<ResultOld> objToSave;
                var objToSave = Mapper.Map<List<ResultOldViewModel>, List<ResultOld>>(obj);
                var result = _betaAppService.SalvarLista(objToSave);
                return Mapper.Map<GenericReturn<ResultOld>, GenericReturnViewModel<ResultOldViewModel>>(result);
            }
            catch (Exception e)
            {
                return new GenericReturnViewModel<ResultOldViewModel>(e, "Não foi possível inserir a coleta.");
            }
        }
        #endregion

        #region Retorno de Dados

        [Route("api/RelatorioBetaApi/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetNcPorIndicador(int idIndicador, string dateInit, string dateEnd)
        {
            try
            {
                var queryResult = _betaAppService.GetNcPorIndicador(idIndicador, dateInit, dateEnd);
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
                var queryResult = _betaAppService.GetNcPorMonitoramento(idIndicador, dateInit, dateEnd);
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
                var queryResult = _betaAppService.GetNcPorMonitoramentoJelsafa(idIndicador, dateInit, dateEnd);
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
                var queryResult = _betaAppService.GetNcPorTarefa(indicadorId, idMonitoramento, dateInit, dateEnd);
                var result = Mapper.Map<GenericReturn<List<ResultOld>>, GenericReturnViewModel<List<ResultOldViewModel>>>(queryResult);
                return result;
            }
            catch (Exception ex)
            {
                return new GenericReturnViewModel<List<ResultOldViewModel>>(ex, "Não foi possível carregar o grafico.");
            }
        } 
        #endregion

    }
}

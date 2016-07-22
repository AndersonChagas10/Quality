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
        public GenericReturnViewModel<ColetaViewModel> Post([FromBody] ColetaViewModel data)
        {
            var queryResult = _betaAppService.Salvar(data);
            return Mapper.Map<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>(queryResult);
        }

        [Route("api/Result/SalvarLista")]
        public GenericReturnViewModel<ColetaViewModel> SalvarLista([FromBody] ColetaViewModel data)
        {
            var queryResult = _betaAppService.SalvarLista(data.listaResultado);
            return Mapper.Map<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>(queryResult);
        }

        #endregion

        #region Retorno de Dados

        [Route("api/RelatorioBetaApi/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorIndicador(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorIndicador(idIndicador, dateInit, dateEnd);
            return  Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorLevel2")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorLevel2(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorLevel2(idIndicador, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorLevel2Jelsafa")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorLevel2Jelsafa(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorLevel2Jelsafa(idIndicador, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorLevel3")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorLevel3(int indicadorId, int idLevel2, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorLevel3(indicadorId, idLevel2, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
        }

        #endregion

        #region Retorno de dados, Tabelas Level1, 2 e 3

        //[Route("api/RelatorioBetaApi/GetNcPorIndicador")]
        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel1(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel1();
        //}


        //[Route("api/RelatorioBetaApi/GetDadosLevel2")]
        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel2(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel2();
        //}


        //[Route("api/RelatorioBetaApi/GetDadosLevel3")]
        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel3(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel3();
        //}

        #endregion

        #region Recebe Dados para Syncronizar o banco de dados

        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel1(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel1();
        //}

        #endregion

        #region Envia Dados Para Syncronizar a plataforma remota

        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel1(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel1();
        //}

        #endregion
    }
}

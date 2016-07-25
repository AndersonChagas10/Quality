using Application.Interface;
using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Linq;
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

        #region Coleta de Dados SP1 BEta

    

        #endregion

        #region Retorno de Dados

        [Route("api/RelatorioBetaApi/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorIndicador(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorIndicador(idIndicador, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
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

    }
}

using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Par
{
    [RoutePrefix("api/Params")]
    public class ParamsApiController : ApiController
    {

        #region Construtor para injeção de dependencia

        private IParamsDomain _parDomain;

        public ParamsApiController(IParamsDomain parDomain)
        {
            _parDomain = parDomain;
        }

        #endregion

        #region Metodos disponíveis na API

        [HttpPost]
        [Route("AddLevel01")]
        public ParamsDTO AddLevel01([FromBody] ParamsViewModel paramsViewModel)
        {
           return _parDomain.AddUpdateParLevel1(paramsViewModel);
        }

        #endregion
    }
}

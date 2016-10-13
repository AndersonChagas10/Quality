using Application.Interface;
using DTO.DTO;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Par
{
    [RoutePrefix("api/Params")]
    public class ParamsApiController : ApiController
    {

        #region Construtor para injeção de dependencia

        private IParamsApp _parApp;

        public ParamsApiController(IParamsApp parApp)
        {
            _parApp = parApp;
        }

        #endregion

        #region Metodos disponíveis na API

        [HttpPost]
        [Route("AddLevel01")]
        public ParamsDTO AddLevel01([FromBody] ParamsViewModel paramsViewModel)
        {
           return _parApp.AddUpdateParLevel1(paramsViewModel);
        }

        #endregion
    }
}

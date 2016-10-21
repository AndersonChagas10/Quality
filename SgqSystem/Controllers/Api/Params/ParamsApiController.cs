using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Params
{
    [HandleApi()]
    [RoutePrefix("api/ParamsApi")]
    public class ParamsApiController : ApiController
    {

        #region Constructor

        private IParamsDomain _paramdDomain;
        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;

        public ParamsApiController(IParamsDomain paramdDomain, IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1)
        {
            _baseParLevel1 = baseParLevel1;
            _paramdDomain = paramdDomain;
        }

        #endregion

        #region Metods

        [HttpPost]
        [Route("AddUpdateLevel1")]
        public ParamsViewModel AddUpdateLevel1([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel1(paramsViewModel.paramsDto);
            return paramsViewModel;
        }


        #endregion

    }
}

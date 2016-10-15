using Dominio.Interfaces.Services;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Params
{
    [RoutePrefix("api/Example")]
    public class ParamsApiController : ApiController
    {

        #region Constructor

        private IParamsDomain _paramdDomain;

        public ParamsApiController(IParamsDomain paramdDomain)
        {
            _paramdDomain = paramdDomain;
        }

        #endregion

        #region Metods

        public ParamsViewModel AddUpdateLevel1(ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel1(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        public ParamsViewModel AddUpdateLevel2(ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel2(paramsViewModel.paramsDto);
            return paramsViewModel;
        }


        public ParamsViewModel AddUpdateLevel3(ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel3(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        #endregion

    }
}

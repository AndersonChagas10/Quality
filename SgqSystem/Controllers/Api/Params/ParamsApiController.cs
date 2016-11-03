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
        [ValidateModel]
        [Route("AddUpdateLevel1")]
        public ParamsViewModel AddUpdateLevel1([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel1(paramsViewModel.paramsDto);
            return paramsViewModel;
        }

        [HttpPost]
        [Route("AddUpdateLevel2")]
        public ParamsViewModel AddUpdateLevel2([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel2(paramsViewModel.paramsDto);
            return paramsViewModel;
        }



        [HttpPost]
        [Route("AddUpdateLevel3")]
        public ParamsViewModel AddUpdateLevel3([FromBody] ParamsViewModel paramsViewModel)
        {
            paramsViewModel.paramsDto = _paramdDomain.AddUpdateLevel3(paramsViewModel.paramsDto);
            return paramsViewModel;
        }


        #endregion


        #region Vinculo Level3 com Level2

        [HttpGet]
        [Route("AddVinculoL3L2/{idLevel2}/{idLevel3}/{peso}")]
        public ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, int peso)
        {
            return _paramdDomain.AddVinculoL3L2(idLevel2, idLevel3, peso);
            //return paramsViewModel;
        }


        #endregion

        #region Vinculo Level1 com Level2

        [HttpGet]
        [Route("AddVinculoL1L2/{idLevel1}/{idLevel2}/{idLevel3}")]
        public ParLevel3Level2Level1DTO AddVinculoL1L2(int idLevel1, int idLevel2,int idLevel3)
        {
            return _paramdDomain.AddVinculoL1L2(idLevel1, idLevel2, idLevel3);
            //return paramsViewModel;
        }


        #endregion

    }
}

using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/ParLevel2ControlCompany")]
    public class ParLevel2ControlCompanyController : ApiController
    {

        private IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> _baseParLevel2ControlCompany;
        private IBaseDomain<UserSgq, UserDTO> _baseteste;
        public List<ParLevel2ControlCompanyDTO> _list;

        public ParLevel2ControlCompanyController(IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> baseParLevel2ControlCompany
            , IBaseDomain<UserSgq, UserDTO> baseteste)
        {
            _baseParLevel2ControlCompany = baseParLevel2ControlCompany;
            _baseteste = baseteste;
            _list = new List<ParLevel2ControlCompanyDTO>();
        }

        [HttpPost]
        [Route("Save")]
        public List<ParLevel2ControlCompanyDTO> Save([FromBody]  ParLevel1DTO parLevel1)
        {
            foreach (var level2 in parLevel1.listLevel2Corporativos)
                _list.Add(_baseParLevel2ControlCompany.AddOrUpdate(parLevel1.createParLevel2ControlCompany(level2, 1, 1)));

            return _list;
        }



    }
}

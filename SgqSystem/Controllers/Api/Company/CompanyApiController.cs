using Dominio.Interfaces.Services;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Company
{
    [RoutePrefix("api/Company")]
    [HandleApi()]
    public class  CompanyApiController : ApiController
    {

        #region Construtor para injeção de dependencia

        private ICompanyDomain _companyDomain;
        
        public CompanyApiController(ICompanyDomain companyDomain)
        {
            _companyDomain = companyDomain;
        }

        #endregion

        #region Metodos disponíveis na API

        [HttpPost]
        [HandleApi()]
        [Route("AddUpdateParCompany")]
        public void AddUpdateParCompany([FromBody] CompanyViewModel companyViewModel)
        {
            _companyDomain.AddUpdateParCompany(companyViewModel.parCompanyDTO);
        }

        [HttpPost]
        [HandleApi()]
        [Route("AddUpdateParStructure")]
        public void AddUpdateParStructure([FromBody] CompanyViewModel companyViewModel)
        {
            _companyDomain.AddUpdateParStructure(companyViewModel.parStructureDTO);
        }

        [HttpPost]
        [HandleApi()]
        [Route("AddUpdateParStructureGroup")]
        public void AddUpdateParStructureGroup([FromBody] CompanyViewModel companyViewModel)
        {
            _companyDomain.AddUpdateParStructureGroup(companyViewModel.parStructureGroupDTO);
        }

        #endregion
    }
}

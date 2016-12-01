using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Company
{
    [RoutePrefix("api/Company")]
    public class CompanyApiController : ApiController
    {

        #region Construtor para injeção de dependencia
        
        public CompanyApiController()
        {
            
        }

        #endregion

        #region Metodos disponíveis na API

        [HttpPost]
        [HandleApi()]
        [Route("AddUpdateParCompany")]
        public void AddUpdateParCompany([FromBody] ParCompanyDTO parCompanyDTO)
        {
           //return _companyDomain.AddUpdateParCompany(parCompanyDTO);
        }

        #endregion
    }
}

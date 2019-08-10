using Dominio;
using DTO.Interfaces.Services;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Api.Company
{
    public class CompanyApiController
    {

        #region Construtor para injeção de dependencia

        private ICompanyDomain _companyDomain;
        public IBaseDomain<ParCompany, ParCompanyDTO> _basedomain;

        public CompanyApiController(ICompanyDomain companyDomain, IBaseDomain<ParCompany, ParCompanyDTO> basedomain)
        {
            _companyDomain = companyDomain;
            _basedomain = basedomain;
        }

        #endregion

        #region Metodos disponíveis na API

        public ParCompanyDTO GETCompany(int id)
        {
            return _basedomain.GetById(id);
        }

        #endregion
    }
}

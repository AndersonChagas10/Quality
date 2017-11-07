using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Company
{
    [RoutePrefix("api/Company")]
    [HandleApi()]
    public class  CompanyApiController : ApiController
    {

        #region Construtor para injeção de dependencia

        private ICompanyDomain _companyDomain;
        public IBaseDomain<ParCompany, ParCompanyDTO> _basedomain;
        
        public CompanyApiController(ICompanyDomain companyDomain, IBaseDomain<ParCompany,ParCompanyDTO> basedomain)
        {
            _companyDomain = companyDomain;
            _basedomain = basedomain;
        }

        #endregion

        #region Metodos disponíveis na API

        [HttpPost]
        [HandleApi()]
        [Route("AddUpdateParCompany")]
        public HttpResponseMessage AddUpdateParCompany([FromBody] CompanyViewModel companyViewModel)
        {
            ParCompany parCompanySalvar = Mapper.Map<ParCompany>(companyViewModel.parCompanyDTO);
            List<ParCompanyCluster> parCompanyClusterSalvar = Mapper.Map<List<ParCompanyCluster>>(companyViewModel.parCompanyDTO.ListParCompanyCluster);
            List<ParCompanyXStructure> parCompanyXStructureSalvar = Mapper.Map<List<ParCompanyXStructure>>(companyViewModel.parCompanyDTO.ListParCompanyXStructure);

            var errors = new List<string>();

            try
            {
                _companyDomain.SaveParCompany(parCompanySalvar);
                _companyDomain.SaveParCompanyCluster(parCompanyClusterSalvar, parCompanySalvar);
                _companyDomain.SaveParCompanyXStructure(parCompanyXStructureSalvar, parCompanySalvar);
            }
            catch (System.Exception e)
            {
                errors.Add("Error: " + e.Message);
            }

            if (errors.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, new { errors });

            return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "", user = "" }); ;
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

        [HttpGet]
        [HandleApi()]
        [Route("getCompany")]
        public ParCompanyDTO GETCompany(int id)
        {
            return _basedomain.GetById(id);
        }
        #endregion
    }
}

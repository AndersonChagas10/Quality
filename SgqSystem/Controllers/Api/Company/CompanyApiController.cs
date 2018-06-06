using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Company
{
    [RoutePrefix("api/Company")]
    [HandleApi()]
    public class CompanyApiController : ApiController
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

        [HttpPost]
        [HandleApi()]
        [Route("AddUpdateParCompany")]
        public HttpResponseMessage AddUpdateParCompany([FromBody] CompanyViewModel companyViewModel)
        {
            ValidModelState(companyViewModel);

            var errors = new List<string>();
            if (ModelState.IsValid)
            {

                ParCompany parCompanySalvar = Mapper.Map<ParCompany>(companyViewModel.parCompanyDTO);
                List<ParCompanyCluster> parCompanyClusterSalvar = Mapper.Map<List<ParCompanyCluster>>(companyViewModel.parCompanyDTO.ListParCompanyCluster);
                List<ParCompanyXStructure> parCompanyXStructureSalvar = Mapper.Map<List<ParCompanyXStructure>>(companyViewModel.parCompanyDTO.ListParCompanyXStructure);


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

                return Request.CreateResponse(HttpStatusCode.OK, new { resposta = "", user = "" });

            }
            else
            {
                List<string> listaErros = null;

                listaErros = Util.PreencherMensagem(ModelState);
                return Request.CreateResponse(HttpStatusCode.OK, new { listaErros });
            }
        }

        private void ValidModelState(CompanyViewModel companyViewModel)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(companyViewModel.parCompanyDTO.Name))
                ModelState.AddModelError("parCompanyDTO.Name", Resources.Resource.required_field + " " + Resources.Resource.name);

            if (string.IsNullOrEmpty(companyViewModel.parCompanyDTO.Description))
                ModelState.AddModelError("parCompanyDTO.Description", Resources.Resource.required_field + " " + Resources.Resource.description);

            if (companyViewModel.parCompanyDTO.AddDate == null)
                ModelState.AddModelError("parCompanyDTO.Name", Resources.Resource.required_field + " " + Resources.Resource.add_date);
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
        [Route("AtivarOuDesativarParStructure")]
        public bool AtivarOuDesativarParStructure([FromBody]ParStructureDTO parStructureDTO)
        {
            _companyDomain.AtivarOuDesativarParStructure(parStructureDTO);

            return true;
        }

        [HttpPost]
        [HandleApi()]
        [Route("AddUpdateParStructureGroup")]
        public void AddUpdateParStructureGroup([FromBody] CompanyViewModel companyViewModel)
        {
            _companyDomain.AddUpdateParStructureGroup(companyViewModel.parStructureGroupDTO);
        }

        [HttpPost]
        [HandleApi()]
        [Route("AtivarOuDesativarParStructureGroup")]
        public bool AtivarOuDesativarParStructureGroup([FromBody]ParStructureGroupDTO parStructureGroupDTO)
        {
            _companyDomain.AtivarOuDesativarParStructureGroup(parStructureGroupDTO);

            return true;
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

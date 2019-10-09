using AutoMapper;
using Dominio;
using DTO.Interfaces.Services;
using DTO.DTO.Params;
using SgqService.Handlres;
using SgqService.Helpers;
using SgqService.ViewModels;
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
        SgqServiceBusiness.Api.Company.CompanyApiController business;
        
        #region Construtor para injeção de dependencia

        private ICompanyDomain _companyDomain;
        public IBaseDomain<ParCompany, ParCompanyDTO> _basedomain;

        public CompanyApiController(ICompanyDomain companyDomain, IBaseDomain<ParCompany, ParCompanyDTO> basedomain)
        {
            _companyDomain = companyDomain;
            _basedomain = basedomain;

            business = new SgqServiceBusiness.Api.Company.CompanyApiController(companyDomain,basedomain);
        }

        #endregion

        #region Metodos disponíveis na API

        [HttpGet]
        [HandleApi()]
        [Route("getCompany")]
        public ParCompanyDTO GETCompany(int id)
        {
            return business.GETCompany(id);
        }

        #endregion
    }
}

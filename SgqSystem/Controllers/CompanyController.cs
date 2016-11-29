using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class CompanyController : Controller
    {
        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;
        private IBaseDomain<ParCompanyXStructure, ParCompanyXStructureDTO> _baseDomainParCompanyXStructure;
        private IBaseDomain<ParStructure, ParStructureDTO> _baseDomainParStructure;
        private IBaseDomain<ParStructureGroup, ParStructureGroupDTO> _baseDomainParStructureGroup;

        public CompanyController(IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany,
                    IBaseDomain<ParCompanyXStructure, ParCompanyXStructureDTO> baseDomainParCompanyXStructure,
                    IBaseDomain<ParStructure, ParStructureDTO> baseDomainParStructure,
                    IBaseDomain<ParStructureGroup, ParStructureGroupDTO> baseDomainParStructureGroup)
        {
            _baseDomainParCompany = baseDomainParCompany;
            _baseDomainParCompanyXStructure = baseDomainParCompanyXStructure;
            _baseDomainParStructure = baseDomainParStructure;
            _baseDomainParStructureGroup = baseDomainParStructureGroup;

            ViewBag.listaParCompany = _baseDomainParCompany.GetAll();
            ViewBag.listaParCompanyStructure = _baseDomainParCompanyXStructure.GetAll();
            ViewBag.listaParStructure = _baseDomainParStructure.GetAll();
            ViewBag.listaParStructureGroup = _baseDomainParStructureGroup.GetAll();

        }

        // GET: CompanyStructure
        public ActionResult Index()
        {
            return View();
        }
    }
}
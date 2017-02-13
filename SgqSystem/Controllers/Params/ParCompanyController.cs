using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using Helper;
using SgqSystem.Secirity;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [HandleController()]
    [CustomAuthorize]
    public class ParCompanyController : BaseController
    {
        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;
        private IBaseDomain<ParCompanyXStructure, ParCompanyXStructureDTO> _baseDomainParCompanyXStructure;
        private IBaseDomain<ParStructure, ParStructureDTO> _baseDomainParStructure;
        private IBaseDomain<ParStructureGroup, ParStructureGroupDTO> _baseDomainParStructureGroup;
        private IBaseDomain<ParCluster, ParClusterDTO> _baseDomainParCluster;

        public ParCompanyController(IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany,
                    IBaseDomain<ParCompanyXStructure, ParCompanyXStructureDTO> baseDomainParCompanyXStructure,
                    IBaseDomain<ParStructure, ParStructureDTO> baseDomainParStructure,
                    IBaseDomain<ParStructureGroup, ParStructureGroupDTO> baseDomainParStructureGroup,
                    IBaseDomain<ParCluster, ParClusterDTO> baseDomainParCluster)
        {
            _baseDomainParCompany = baseDomainParCompany;
            _baseDomainParCompanyXStructure = baseDomainParCompanyXStructure;
            _baseDomainParStructure = baseDomainParStructure;
            _baseDomainParStructureGroup = baseDomainParStructureGroup;
            _baseDomainParCluster = baseDomainParCluster;

            ViewBag.listaParCompany = _baseDomainParCompany.GetAll();
            ViewBag.listaParCompanyStructure = _baseDomainParCompanyXStructure.GetAll();
            ViewBag.listaParStructure = _baseDomainParStructure.GetAll();
            ViewBag.listaParStructureGroup = _baseDomainParStructureGroup.GetAll();
            ViewBag.listaParCluster = _baseDomainParCluster.GetAll();

        }

        // GET: CompanyStructure
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult GetParCompany()
        {          
            return PartialView("ParCompany");//retorna partial ParCompany
        }

        public ActionResult GetParStructure()
        {
            return PartialView("ParStructure");//retorna partial ParStructure
        }

        public ActionResult GetParStructureGroup()
        {
            return PartialView("ParStructureGroup");//retorna partial ParStructureGroup
        }


    }
}
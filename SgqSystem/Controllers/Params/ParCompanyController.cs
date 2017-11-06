using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using Helper;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class ParCompanyController : BaseController
    {
        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;
        private IBaseDomain<ParCompanyXStructure, ParCompanyXStructureDTO> _baseDomainParCompanyXStructure;
        private IBaseDomain<ParStructure, ParStructureDTO> _baseDomainParStructure;
        private IBaseDomain<ParStructureGroup, ParStructureGroupDTO> _baseDomainParStructureGroup;
        private IBaseDomain<ParCluster, ParClusterDTO> _baseDomainParCluster;
        private IBaseDomain<ParCompanyCluster, ParCompanyClusterDTO> _baseDomainParCompanyCluster;
        private IBaseDomain<ParClusterGroup, ParClusterGroupDTO> _baseDomainParClusterGroup;

        public ParCompanyController(IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany,
                    IBaseDomain<ParCompanyXStructure, ParCompanyXStructureDTO> baseDomainParCompanyXStructure,
                    IBaseDomain<ParStructure, ParStructureDTO> baseDomainParStructure,
                    IBaseDomain<ParStructureGroup, ParStructureGroupDTO> baseDomainParStructureGroup,
                    IBaseDomain<ParCluster, ParClusterDTO> baseDomainParCluster,
                    IBaseDomain<ParCompanyCluster, ParCompanyClusterDTO> baseDomainParCompanyCluster,
                    IBaseDomain<ParClusterGroup, ParClusterGroupDTO> baseDomainParClusterGroup
                    )
        {
            _baseDomainParCompany = baseDomainParCompany;
            _baseDomainParCompanyXStructure = baseDomainParCompanyXStructure;
            _baseDomainParStructure = baseDomainParStructure;
            _baseDomainParStructureGroup = baseDomainParStructureGroup;
            _baseDomainParCluster = baseDomainParCluster;
            _baseDomainParCompanyCluster = baseDomainParCompanyCluster;
            _baseDomainParClusterGroup = baseDomainParClusterGroup;

            ViewBag.listaParCompany = _baseDomainParCompany.GetAll().Where(r => r.IsActive == true).ToList();
            ViewBag.listaParCompanyStructure = _baseDomainParCompanyXStructure.GetAllNoLazyLoad().Where(r => r.Active == true).ToList();
            ViewBag.listaParStructure = _baseDomainParStructure.GetAll().Where(r => r.IsActive == true).ToList();
            ViewBag.listaParStructureGroup = _baseDomainParStructureGroup.GetAll().Where(r => r.IsActive == true).ToList();
            ViewBag.listaParCluster = _baseDomainParCluster.GetAll().Where(r=>r.IsActive == true).ToList();
            ViewBag.listaParClusterGroup = _baseDomainParClusterGroup.GetAll().ToList();
            ViewBag.listaParStructureXCompany = _baseDomainParStructure.GetAll().Where(
                r=>r.ParStructureGroup_Id == _baseDomainParStructure.GetAllNoLazyLoad().Where(y => y.IsActive == true).Max(x => x.ParStructureGroup_Id) && r.IsActive == true);

            foreach(ParCompanyDTO company in ViewBag.listaParCompany)
            {
                company.ListParCompanyCluster = _baseDomainParCompanyCluster.GetAll().Where(
                    r => r.ParCompany_Id == company.Id && r.Active == true).ToList();

                company.ListParCompanyXStructure = _baseDomainParCompanyXStructure.GetAll().Where(
                    r => r.ParCompany_Id == company.Id && r.Active == true).ToList();
            }
            

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
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using Helper;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    [CustomAuthorize]
    public class Level1ControlController : Controller
    {

        private readonly IBaseDomain<ParLevel2Level1, ParLevel2Level1DTO> _baseRepoParLevel2Level1;
        //private readonly IBaseDomain<ParLevel1, ParLevel1DTO> _baseRepoParLevel1;
        //private readonly IBaseDomain<ParLevel2, ParLevel2DTO> _baseRepoParLevel2;
        private readonly IBaseDomain<ParCompany, ParCompanyDTO> _baseRepoParCompany;
        public Level1ControlController(IBaseDomain<ParLevel2Level1, ParLevel2Level1DTO> baseRepoParLevel2Level1,
            //IBaseDomain<ParLevel1, ParLevel1DTO> baseRepoParLevel1,
            //IBaseDomain<ParLevel2, ParLevel2DTO> baseRepoParLevel2,
            IBaseDomain<ParCompany, ParCompanyDTO> baseRepoParCompany)
        {
            _baseRepoParLevel2Level1 = baseRepoParLevel2Level1;
            //_baseRepoParLevel1 = baseRepoParLevel1;
            //_baseRepoParLevel2 = baseRepoParLevel2;
            _baseRepoParCompany = baseRepoParCompany;

            ViewBag.ParCompany = _baseRepoParCompany.GetAllNoLazyLoad();
            ViewBag.ParLevel2Level1 = _baseRepoParLevel2Level1.GetAllNoLazyLoad();
            //ViewBag.ParLevel1 = _baseRepoParLevel1.GetAllNoLazyLoad();
            //ViewBag.ParLevel2 = _baseRepoParLevel2.GetAllNoLazyLoad();
        }

        // GET: EspecificarMonitoramentos
        public ActionResult Index()
        {
            return View();
        }

        // GET: EspecificarMonitoramentos
        public ActionResult SelectLevel1(int companyId)
        {
            var company = _baseRepoParCompany.GetByIdNoLazyLoad(companyId);

            var allParLevel2Level1 = _baseRepoParLevel2Level1.GetAll();
            ViewBag.level2LevelSemCompany = allParLevel2Level1.Where(r => r.ParCompany_Id == null).ToList();
            ViewBag.level2LevelCompany = allParLevel2Level1.Where(r => r.ParCompany_Id != null).ToList();

            return PartialView("_ParLevel2OnParCompany");
        }

    }
}
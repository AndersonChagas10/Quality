using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ParLevel2ControlCompanyController : BaseController
    {
        private IBaseDomain<ParLevel3Level2Level1, ParLevel3Level2Level1DTO> _baseParLevel3Level2Level1;
        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseLevel1;
        private IBaseDomain<ParLevel2, ParLevel2DTO> _baseLevel2;
        private IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> _baseParLevel2ControlCompany;
        
        public ParLevel2ControlCompanyController(IBaseDomain<ParLevel3Level2Level1, ParLevel3Level2Level1DTO> baseParLevel3Level2Level1,
            IBaseDomain<ParLevel1, ParLevel1DTO> baseLevel1,
            IBaseDomain<ParLevel2, ParLevel2DTO> baseLevel2,
            IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> baseParLevel2ControlCompany
            )
        {
            _baseParLevel3Level2Level1 = baseParLevel3Level2Level1;
            _baseParLevel2ControlCompany = baseParLevel2ControlCompany;
            _baseLevel1 = baseLevel1;
            _baseLevel2 = baseLevel2;
            ViewBag.ParLevel1 = _baseLevel1.GetAll().Where(r=>r.Id == 21 || r.Id == 2);
            ViewBag.ParLevel2 = new List<ParLevel2DTO>();
            ViewBag.level2UserCompany = new List<ParLevel2ControlCompanyDTO>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeLevel2(int id)
        {
            ViewBag.ParLevel2 = _baseParLevel3Level2Level1.GetAll().Where(r => r.ParLevel1_Id == id).Select(r => r.ParLevel3Level2.ParLevel2);
            return PartialView("_ControlCompanySelectBoxLevel2");
        }

        public ActionResult ChangeLevel2UserCompany(int id)
        {
            ViewBag.level2UserCompany = _baseParLevel2ControlCompany.GetAll().Where(r => r.ParLevel1_Id == id).Select(r=>r.ParLevel2);
            return PartialView("_ControlCompanySelectBoxLevel2UserCompany");
        }
    }
}
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using SgqSystem.Secirity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    [FilterUnit(filtraUnidadeDoUsuario = true)]
    public class ParLevel2ControlCompanyController : BaseController
    {
        private IBaseDomain<ParLevel3Level2Level1, ParLevel3Level2Level1DTO> _baseParLevel3Level2Level1;
        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseLevel1;
        private IBaseDomain<ParLevel2, ParLevel2DTO> _baseLevel2;
        private IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> _baseParLevel2ControlCompany;
        private IBaseDomain<ParCompany, ParCompanyDTO> _baseParCompany;

        public ParLevel2ControlCompanyController(IBaseDomain<ParLevel3Level2Level1, ParLevel3Level2Level1DTO> baseParLevel3Level2Level1,
            IBaseDomain<ParLevel1, ParLevel1DTO> baseLevel1,
            IBaseDomain<ParLevel2, ParLevel2DTO> baseLevel2,
            IBaseDomain<ParCompany, ParCompanyDTO> baseParCompany,
            IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> baseParLevel2ControlCompany
            )
        {
            _baseParCompany = baseParCompany;
            _baseParLevel3Level2Level1 = baseParLevel3Level2Level1;
            _baseParLevel2ControlCompany = baseParLevel2ControlCompany;
            _baseLevel1 = baseLevel1;
            _baseLevel2 = baseLevel2;
            ViewBag.ParLevel1 = _baseLevel1.GetAll().Where(r => r.IsFixedEvaluetionNumber == true);
            ViewBag.ParLevel2Todos = new List<ParLevel2DTO>();
            ViewBag.level2Number = 0;

            ViewBag.level2Comporativo = new List<ParLevel2DTO>();
            ViewBag.level2DisponivelParaEmpresa = new List<ParLevel2DTO>();

            ViewBag.company = _baseParCompany.GetAll();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeLevel2(int id)
        {
            if (id > 0)
            {
                var allControlCompany = _baseParLevel2ControlCompany.GetAll().Where(r => r.ParLevel1_Id == id);
                var todosLevel321 = _baseParLevel3Level2Level1.GetAll().Where(r => r.ParLevel1_Id == id);

                var lastDate = allControlCompany.Where(r => r.ParCompany_Id == null).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2Comporativo = allControlCompany.Where(r => r.InitDate == lastDate).Select(r => r.ParLevel2);

                ViewBag.ParLevel2Todos = todosLevel321.Select(r => r.ParLevel3Level2.ParLevel2).GroupBy(r => r.Id)
                    .Select(group => group.First()).ToList();
                ViewBag.ParLevel2Ids = level2Comporativo?.Select(r=>r.Id);
                ViewBag.level2Number = _baseLevel1.GetById(id).level2Number;
            }
            else
            {
                ViewBag.ParLevel2 = new List<ParLevel2DTO>();
                ViewBag.level2Number = 0;
            }
            return PartialView("_ControlCompanySelectBoxLevel2");
        }

        public ActionResult ChangeLevel2UserCompany(int id, int companyId)
        {
            if (id > 0 && companyId > 0)
            {
                var allControlCompany = _baseParLevel2ControlCompany.GetAll().Where(r => r.ParLevel1_Id == id);

                var lastDate = allControlCompany.Where(r=>r.ParCompany_Id == null).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2Comporativo = allControlCompany.Where(r => r.InitDate == lastDate)?.Select(r => r.ParLevel2);

                var todosLevel321 = _baseParLevel3Level2Level1.GetAll().Where(r => r.ParLevel1_Id == id);
                var level2DisponivelParaEmpresa = todosLevel321.Where(r=> !level2Comporativo.Any(c=>c.Id == r.ParLevel3Level2.ParLevel2.Id)).Select(r => r.ParLevel3Level2.ParLevel2);

                var lastDateCompany = allControlCompany.Where(r => r.ParCompany_Id == companyId).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2SelecionadosParaEmpresa = allControlCompany.Where(r => r.ParCompany_Id == companyId && r.InitDate == lastDateCompany).Select(r => r.ParLevel2);

                ViewBag.level2Number = _baseLevel1.GetById(id).level2Number;
                ViewBag.level2Comporativo = level2Comporativo;
                ViewBag.level2ComporativoIds = level2Comporativo.Select(r => r.Id);
                ViewBag.level2DisponivelParaEmpresa = level2DisponivelParaEmpresa.GroupBy(r => r.Id)
                    .Select(group => group.First()).ToList(); 
                ViewBag.level2SelecionadosParaEmpresaIds = level2SelecionadosParaEmpresa?.Select(r => r.Id);
            }
            else
            {
                ViewBag.ParLevel2 = new List<ParLevel2DTO>();
                ViewBag.level2Number = 0;
            }
            return PartialView("_ControlCompanySelectBoxLevel2UserCompany");
        }
    }
}
using Dominio;
using DTO.DTO.Params;
using Helper;
using SgqSystem.Secirity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    /// <summary>
    /// Este controller gerencia o numero de Monitoramentos (level2)
    /// definidos por Coporação e Unidade (ParCompany)
    /// chamado na JBS Braisl de "Familia de monitoramentos".
    /// 
    /// Por ex:
    /// Na Figura: ..\Referencias\FamiliasExemplo.png
    /// O Indicador (level1) Cep Desossa está configurado para exibir
    /// Os "Monitoramentos da Empresa" Selecionados (e salvos) juntamente com 
    /// os "Monitoramentos Corporativos Ativos" selecionados.
    /// </summary>
    [CustomAuthorize]
    [FilterUnit(filtraUnidadeDoUsuario = true)]
    public class ParLevel2ControlCompanyController : BaseController
    {
        private SgqDbDevEntities db;
       
        public ParLevel2ControlCompanyController()
        {
            db = new SgqDbDevEntities();
            db.Configuration.LazyLoadingEnabled = false;

            ViewBag.ParLevel1 = db.ParLevel1.Where(r => r.IsFixedEvaluetionNumber == true);
            ViewBag.ParLevel2Todos = new List<ParLevel2DTO>();
            ViewBag.level2Number = 0;
            ViewBag.level2Comporativo = new List<ParLevel2DTO>();
            ViewBag.level2DisponivelParaEmpresa = new List<ParLevel2DTO>();
            ViewBag.company = db.ParCompany.ToList();
        }

        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult ChangeLevel2(int id)
        {
            if (id > 0)
            {
                var allControlCompany = db.ParLevel2ControlCompany.Include("ParLevel2").Where(r => r.ParLevel1_Id == id && r.IsActive == true);
                var lastDateDaControlCompany = allControlCompany.Where(r => r.ParCompany_Id == null).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2Comporativo = allControlCompany.Where(r => r.InitDate == lastDateDaControlCompany).Select(r => r.ParLevel2);
                var level2VinculadosAoLevel1Selecionado = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == id).Select(r => r.ParLevel3Level2.ParLevel2).Distinct().ToList();
               
                ViewBag.ParLevel2Todos = level2VinculadosAoLevel1Selecionado;
                ViewBag.ParLevel2Ids = level2Comporativo?.Select(r => r.Id);
                ViewBag.level2Number = db.ParLevel1.FirstOrDefault(r=>r.Id == id).Level2Number;
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
                var allControlCompany = db.ParLevel2ControlCompany.Include("ParLevel2").Where(r => r.ParLevel1_Id == id && r.IsActive == true);
                var lastDateDaControlCompany = allControlCompany.Where(r => r.ParCompany_Id == null).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2Comporativo = allControlCompany.Where(r => r.InitDate == lastDateDaControlCompany).Select(r => r.ParLevel2);

                var lastDateCompany = allControlCompany.Where(r => r.ParCompany_Id == companyId).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2SelecionadosParaEmpresa = allControlCompany.Where(r => r.ParCompany_Id == companyId && r.InitDate == lastDateCompany).Select(r => r.ParLevel2);
                var level2DisponivelParaEmpresa = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == id && !level2Comporativo.Any(c => c.Id == r.ParLevel3Level2.ParLevel2.Id)).Select(r => r.ParLevel3Level2.ParLevel2).Distinct().ToList();

                ViewBag.level2Number = db.ParLevel1.FirstOrDefault(r => r.Id == id).Level2Number;
                ViewBag.level2Comporativo = level2Comporativo;
                ViewBag.level2ComporativoIds = level2Comporativo.Select(r => r.Id);
                ViewBag.level2DisponivelParaEmpresa = level2DisponivelParaEmpresa;
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
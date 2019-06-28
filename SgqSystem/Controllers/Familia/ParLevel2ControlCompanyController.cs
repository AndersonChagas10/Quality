using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using DTO.Helpers;
using Helper;
using Newtonsoft.Json;
using SgqSystem.Secirity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

            ViewBag.ParLevel1 = db.ParLevel1.Where(r => r.IsFixedEvaluetionNumber == true).ToList();
            ViewBag.ParLevel2Todos = new List<ParLevel2DTO>();
            ViewBag.level2Number = 0;
            ViewBag.level2Comporativo = new List<ParLevel2DTO>();
            ViewBag.level2DisponivelParaEmpresa = new List<ParLevel2DTO>();
            ViewBag.company = db.ParCompany.ToList();
        }

        public ActionResult Index()
        {
            var allControlCompany = db.ParLevel2ControlCompany.Include("ParLevel2").Where(r => r.IsActive == true && r.InitDate <= DateTime.Now);
            var lastDateDaControlCompany = allControlCompany.Where(r => r.ParCompany_Id == null).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;

            if (lastDateDaControlCompany == null)
                lastDateDaControlCompany = DateTime.Now;

            ViewBag.dataInit = lastDateDaControlCompany;

            PreencheViewBagEmpresasVinculadas();

            return View();
        }

        public void PreencheViewBagEmpresasVinculadas()
        {
            var userId = 0;
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
            if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                int.TryParse(cookie.Values["userId"].ToString(), out userId);

            UserSgq userLogado = db.UserSgq.FirstOrDefault(r => r.Id == userId);

            ViewBag.listaFamilias = db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == userId).Select(r => r.ParCompany).ToList().OrderBy(r => r.Name).GroupBy(r => r.Id).Select(group => group.First()).ToList();
        }

        public ActionResult ChangeLevel2(int id, string dataInit)
        {
            var _dataInit = Guard.ParseDateToSqlV2(dataInit);
            if (id > 0)
            {
                //var allControlCompany = db.ParLevel2ControlCompany.Include("ParLevel2").Where(r => r.ParLevel1_Id == id && r.InitDate == _dataInit && r.IsActive == true);
                var allControlCompany = db.ParLevel2ControlCompany.Include("ParLevel2").Where(r => r.ParLevel1_Id == id && r.InitDate <= _dataInit && r.IsActive == true).ToList();
                var lastDateDaControlCompany = allControlCompany.Where(r => r.ParCompany_Id == null).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2Comporativo = allControlCompany.Where(r => r.InitDate == lastDateDaControlCompany && r.ParCompany_Id == null).Select(r => r.ParLevel2).ToList();
                var level2VinculadosAoLevel1Selecionado = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == id).Select(r => r.ParLevel3Level2.ParLevel2).Distinct().ToList();

                ViewBag.ParLevel2Todos = level2VinculadosAoLevel1Selecionado;
                ViewBag.ParLevel2Ids = level2Comporativo?.Select(r => r.Id);
                ViewBag.level2Number = db.ParLevel1.FirstOrDefault(r => r.Id == id).Level2Number;
            }
            else
            {
                ViewBag.ParLevel2 = new List<ParLevel2DTO>();
                ViewBag.level2Number = 0;
            }
            return PartialView("_ControlCompanySelectBoxLevel2");
        }

        public ActionResult ChangeLevel2UserCompany(int id, int companyId, string dataInit)
        {
            var _dataInit = Guard.ParseDateToSqlV2(dataInit);
            if (id > 0 && companyId > 0)
            {
                var allControlCompany = db.ParLevel2ControlCompany.Include("ParLevel2").Where(r => r.ParLevel1_Id == id && r.InitDate <= _dataInit && r.IsActive == true);
                //var allControlCompany = db.ParLevel2ControlCompany.Include("ParLevel2").Where(r => r.ParLevel1_Id == id && r.IsActive == true).ToList();
                var lastDateDaControlCompany = allControlCompany.Where(r => r.ParCompany_Id == null).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2Comporativo = allControlCompany.Where(r => r.InitDate == lastDateDaControlCompany && r.ParCompany_Id == null).Select(r => r.ParLevel2);

                var lastDateCompany = allControlCompany.Where(r => r.ParCompany_Id == companyId).OrderByDescending(r => r.InitDate).FirstOrDefault()?.InitDate;
                var level2SelecionadosParaEmpresa = allControlCompany.Where(r => r.ParCompany_Id == companyId && r.InitDate == lastDateCompany).Select(r => r.ParLevel2).ToList();
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


        public ActionResult List()
        {

            var sql = $@"SELECT TOP 200
            	IIF(PC.Name IS NULL, 'Sim', 'Não') AS IsCorporativo
               ,PC.Name AS ParCompany
               ,P1.Name AS ParLevel1
               ,P2CC.InitDate AS InitDate
               ,STUFF((SELECT DISTINCT
            			', ' + P2.Name
            		FROM ParLevel2ControlCompany P2CC2 WITH (NOLOCK)
            		LEFT JOIN ParLevel1 P1 WITH (NOLOCK) ON P1.Id = P2CC2.ParLevel1_Id
            		LEFT JOIN Parlevel2 P2 WITH (NOLOCK) ON P2.Id = P2CC2.ParLevel2_Id
            		LEFT JOIN ParCompany PC WITH (NOLOCK) ON PC.Id = P2CC2.ParCompany_Id
            		WHERE P2CC2.InitDate = P2CC.InitDate
            		FOR XML PATH (''))
            	, 1, 1, '') AS ParLevel2
            FROM ParLevel2ControlCompany P2CC WITH (NOLOCK)
            LEFT JOIN ParLevel1 P1 WITH (NOLOCK) ON P1.Id = P2CC.ParLevel1_Id
            LEFT JOIN Parlevel2 P2 WITH (NOLOCK) ON P2.Id = P2CC.ParLevel2_Id
            LEFT JOIN ParCompany PC WITH (NOLOCK) ON PC.Id = P2CC.ParCompany_Id
            GROUP BY P2CC.InitDate
            		,P1.Name
            		,PC.Name
            ORDER BY P2CC.InitDate DESC";

            var retorno = db.Database.SqlQuery<RetornoListaFamilias>(sql).ToList();

            var idUsuario = Guard.GetUsuarioLogado_Id(ControllerContext.HttpContext);

            var userSgq = db.UserSgq.Where(x => x.Id == idUsuario).FirstOrDefault();

            List<ParCompanyXUserSgq> parCompanyXUserSgq = db.ParCompanyXUserSgq.Where(x => x.UserSgq_Id == idUsuario).ToList();


            return View(retorno);
        }

    }


    public class RetornoListaFamilias
    {
        public string IsCorporativo { get; set; }
        public string ParCompany { get; set; }
        public string ParLevel1 { get; set; }
        public string ParLevel2 { get; set; }
        public DateTime InitDate { get; set; }

        public string EmpresaPadrao { get; set; }
    }

}
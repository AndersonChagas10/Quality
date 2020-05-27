using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.CadastrosGerais
{
    public class ParReportLayoutXReportXUserController : BaseController
    {
        public class EnumeradorLayoutLevel {
            public enum Level{
                Cabecalho, //0
                Linha,     //1
                Valor      //2
            }
        }

        public enum AcessoType
        {
            Admin = 1,
            PAGED = 2,
            GED = 3
        }

        // GET: ParReportLayoutXReportXUser
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int reportXUserId)
        {
            var teste = EnumeradorLayoutLevel.Level.Linha;

            var teste2 = new EnumeradorLayoutLevel.Level();

            //var oi = AcessoType;

            var enumeradorLayoutLevel = new List<EnumeradorLayoutLevel.Level>();

            ViewBag.ReportXUser_Id = reportXUserId;
            

            ViewBag.ItemMenu_Id = new SelectList(enumeradorLayoutLevel, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            return View();
        }
    }
}
using Dominio;
using DTO.DTO;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Recravacao
{
    [CustomAuthorize]
    public class RecravacaoController : BaseController
    {
        // GET: Recravacao
        public ActionResult Index()
        {
            return View();
        }

        // GET: Recravacao
        public ActionResult Print(int? indicadorId = 0, int? linhaId = 0)
        {
            ViewBag.IndicadorId = indicadorId;
            ViewBag.LinhaId = linhaId;
            return View();
        }

        public ActionResult Print3(int? indicadorId = 0, int? linhaId = 0)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                var itemMenu = (ItemMenuDTO)ViewBag.itemMenu;
                if (itemMenu != null)
                {
                    ViewBag.ReportXUserSgq = db.ReportXUserSgq.Include("Elaborador").Include("Aprovador")
                        .FirstOrDefault(r => r.ItemMenu_Id == itemMenu.Id && r.ParLevel1_Id == indicadorId);
                }
                ViewBag.IndicadorId = indicadorId;
                ViewBag.LinhaId = linhaId;
                return View();
            }
        }

        public ActionResult PrintAcaoCorretiva(int? indicadorId = 0, int? linhaId = 0)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                var itemMenu = (ItemMenuDTO)ViewBag.itemMenu;
                if (itemMenu != null)
                {
                    ViewBag.ReportXUserSgq = db.ReportXUserSgq.Include("Elaborador").Include("Aprovador")
                        .FirstOrDefault(r => r.ItemMenu_Id == itemMenu.Id && r.ParLevel1_Id == indicadorId);
                }
                ViewBag.IndicadorId = indicadorId;
                ViewBag.LinhaId = linhaId;
                return View();
            }
        }

    }
}
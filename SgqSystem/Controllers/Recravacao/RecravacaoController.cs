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

                var unidadeUsuario_Id = getUserUnitId();

                if (itemMenu != null)
                {
                    var reportXUserSgq = db.ReportXUserSgq
                        .Include("ItemMenu")
                        .OrderByDescending(r => r.ParCompany_Id)
                        .FirstOrDefault(r => r.ItemMenu_Id == itemMenu.Id && r.ParLevel1_Id == indicadorId && (r.ParCompany_Id == unidadeUsuario_Id || r.ParCompany_Id == null));

                    if (reportXUserSgq != null)
                        reportXUserSgq.CodigoRelatorio = reportXUserSgq.CodigoRelatorio?.Replace("[", "<").Replace("]", ">");

                    ViewBag.ReportXUserSgq = reportXUserSgq;
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

                var unidadeUsuario_Id = getUserUnitId();

                if (itemMenu != null)
                {
                    var reportXUserSgq = db.ReportXUserSgq
                        .Include("ItemMenu")
                        .OrderByDescending(r => r.ParCompany_Id)
                        .FirstOrDefault(r => r.ItemMenu_Id == itemMenu.Id && r.ParLevel1_Id == indicadorId && (r.ParCompany_Id == unidadeUsuario_Id || r.ParCompany_Id == null));

                    if (reportXUserSgq != null)
                        reportXUserSgq.CodigoRelatorio = reportXUserSgq.CodigoRelatorio?.Replace("[", "<").Replace("]", ">");
                    ViewBag.ReportXUserSgq = reportXUserSgq;

                    var outroRelatorio = db.ReportXUserSgq
                        .Include("ItemMenu")
                        .FirstOrDefault(r => r.ParLevel1_Id == indicadorId && r.Id != reportXUserSgq.Id);
                    outroRelatorio.AddDate = DateTime.Now.Date;
                    ViewBag.OutroRelatorio = outroRelatorio;
                }

                ViewBag.IndicadorId = indicadorId;
                ViewBag.LinhaId = linhaId;
                return View();
            }
        }

    }
}
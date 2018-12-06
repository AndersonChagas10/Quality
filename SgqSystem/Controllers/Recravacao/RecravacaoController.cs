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

        [Route("{id}")]
        public ActionResult Retroativo(int id)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                RecravacaoJson rj = db.RecravacaoJson.FirstOrDefault(x => x.Id == id);
                ViewBag.RecravacaoJsonId = rj.Id;
                ViewBag.RecravacaoJson_LinhaId = rj.Linha_Id;
            }
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

        //RECRAVAÇÃO RETROATIVA PRINT
        public ActionResult PrintRetroativa(int? indicadorId = 0, int? linhaId = 0, int? recravacaoJsonId = 0)
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
                        .FirstOrDefault(r => r.ItemMenu.Url.Contains("Print3") && r.ParLevel1_Id == indicadorId && (r.ParCompany_Id == unidadeUsuario_Id || r.ParCompany_Id == null));

                    if (reportXUserSgq != null)
                        reportXUserSgq.CodigoRelatorio = reportXUserSgq.CodigoRelatorio?.Replace("[", "<").Replace("]", ">");

                    ViewBag.ReportXUserSgq = reportXUserSgq;
                }

                RecravacaoJson rj = db.RecravacaoJson.FirstOrDefault(x => x.Id == recravacaoJsonId);
                ViewBag.RecravacaoJsonId = rj.Id;
                ViewBag.RecravacaoJson_LinhaId = rj.Linha_Id;

                ViewBag.IndicadorId = indicadorId;
                ViewBag.LinhaId = linhaId;
                return View();
            }
        }

        public ActionResult PrintAcaoCorretivaRetroativa(int? indicadorId = 0, int? linhaId = 0, int? recravacaoJsonId = 0)
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
                        .FirstOrDefault(r => r.ItemMenu.Url.Contains("PrintAcaoCorretiva") && r.ParLevel1_Id == indicadorId && (r.ParCompany_Id == unidadeUsuario_Id || r.ParCompany_Id == null));

                    if (reportXUserSgq != null)
                        reportXUserSgq.CodigoRelatorio = reportXUserSgq.CodigoRelatorio?.Replace("[", "<").Replace("]", ">");
                    ViewBag.ReportXUserSgq = reportXUserSgq;

                    var outroRelatorio = db.ReportXUserSgq
                        .Include("ItemMenu")
                        .FirstOrDefault(r => r.ParLevel1_Id == indicadorId && r.Id != reportXUserSgq.Id);
                    outroRelatorio.AddDate = DateTime.Now.Date;
                    ViewBag.OutroRelatorio = outroRelatorio;
                }

                RecravacaoJson rj = db.RecravacaoJson.FirstOrDefault(x => x.Id == recravacaoJsonId);
                ViewBag.RecravacaoJsonId = rj.Id;
                ViewBag.RecravacaoJson_LinhaId = rj.Linha_Id;

                ViewBag.IndicadorId = indicadorId;
                ViewBag.LinhaId = linhaId;
                return View("PrintAcaoCorretiva");
            }
        }

    }
}
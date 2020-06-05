using Dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.CadastrosGerais
{
    public class ParReportLayoutXReportXUserController : BaseController
    {

        private SgqDbDevEntities db = new SgqDbDevEntities();

        public enum LayoutLevelEnum
        {
            Cabecalho = 1,
            Linha = 2,
            Valor = 3
        }

        public class LayoutLevel{

            public int Id { get; set; }

            public string Name { get; set; }

        }

        List<LayoutLevel> NiveisLayout = new List<LayoutLevel>();

        // GET: ParReportLayoutXReportXUser
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int reportXUserId)
        {
            PreencheViewBags(reportXUserId);
           

            return View();
        }

        private void PreencheViewBags(int reportXUserId)
        {
            AdicionaItemNivelLayout(LayoutLevelEnum.Cabecalho.ToString(), (int)LayoutLevelEnum.Cabecalho);
            AdicionaItemNivelLayout(LayoutLevelEnum.Linha.ToString(), (int)LayoutLevelEnum.Linha);
            AdicionaItemNivelLayout(LayoutLevelEnum.Valor.ToString(), (int)LayoutLevelEnum.Valor);

            ViewBag.ReportXUser_Id = reportXUserId;

            ViewBag.NiveisLayout_Id = new SelectList(NiveisLayout.ToList(), "Id", "Name");

            ViewBag.ReportLayoutItens = new SelectList(db.ReportLayoutItens.Where(x => x.IsActive).ToList(), "Name", "Name");
        }

        private void AdicionaItemNivelLayout(string name, int id)
        {
            LayoutLevel niveisLayout = new LayoutLevel();

            niveisLayout.Id = id;
            niveisLayout.Name = name;

            NiveisLayout.Add(niveisLayout);
        }

        [HttpPost]
        public ActionResult Create(ParReportLayoutXReportXUser form)
        {

            if (ModelState.IsValid)
            {
                using (db)
                {
                    form.IsActive = true;
                    db.ParReportLayoutXReportXUser.Add(form);

                    db.SaveChanges();
                }
            }

            return RedirectToAction("Details/" + form.ReportXUserSgq_Id, "ReportXUserSgq");
        }

        // GET: ReportXUserSgqs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ParReportLayoutXReportXUser reportLayoutXReportXUser = db.ParReportLayoutXReportXUser.Find(id);

            if (reportLayoutXReportXUser == null)
            {
                return HttpNotFound();
            }
            PreencheViewBags(reportLayoutXReportXUser.ReportXUserSgq_Id);
            return View(reportLayoutXReportXUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ParReportLayoutXReportXUser reportLayoutXReportXUser)
        {
            if (ModelState.IsValid)
            {
                reportLayoutXReportXUser.AlterDate = DateTime.Now;
                reportLayoutXReportXUser.IsActive = true;
                db.Entry(reportLayoutXReportXUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + reportLayoutXReportXUser.ReportXUserSgq_Id, "ReportXUserSgq");
            }
            PreencheViewBags(reportLayoutXReportXUser.ReportXUserSgq_Id);
            return View(reportLayoutXReportXUser);
        }


        // GET: ParReportLayoutXReportXUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var reportLayoutXReportXUser = db.ParReportLayoutXReportXUser.Where(r => r.Id == id).FirstOrDefault();

            if (reportLayoutXReportXUser == null)
            {
                return HttpNotFound();
            }
            return View(reportLayoutXReportXUser);
        }

        // POST: ParReportLayoutXReportXUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParReportLayoutXReportXUser reportLayoutXReportXUser = db.ParReportLayoutXReportXUser.Find(id);

            reportLayoutXReportXUser.IsActive = false;
            db.SaveChanges();

            return RedirectToAction("Details/" + reportLayoutXReportXUser.ReportXUserSgq_Id, "ReportXUserSgq");
        }

    }
}
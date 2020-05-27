using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;

namespace SgqSystem.Controllers
{
    public class ReportXUserSgqController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ReportXUserSgqs
        public ActionResult Index()
        {
            var reportXUserSgq = db.ReportXUserSgq.Include(r => r.ItemMenu).Include(r => r.ParCompany).Include(r => r.ParLevel1);
            return View(reportXUserSgq.ToList());
        }

        // GET: ReportXUserSgqs/Create
        public ActionResult Create()
        {
            ViewBag.ItemMenu_Id = new SelectList(db.ItemMenu, "Id", "Name");
            ViewBag.ParCompany_Id = new SelectList(db.ParCompany, "Id", "Name");
            ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name");
            return View();
        }

        // POST: ReportXUserSgqs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AddDate,AlterData,NomeRelatorio,ItemMenu_Id,Elaborador,Aprovador,ParLevel1_Id,ParCompany_Id,IsActive,CodigoRelatorio")] ReportXUserSgq reportXUserSgq)
        {

            if (db.ReportXUserSgq.Any(r => r.ItemMenu_Id == reportXUserSgq.ItemMenu_Id && 
            r.ParLevel1_Id == reportXUserSgq.ParLevel1_Id && 
            r.ParCompany_Id == reportXUserSgq.ParCompany_Id))
            {
                ModelState.AddModelError("ItemMenu_Id", "Já existe elaborador e editor para este relatório nesta unidade");
            }

            if (ModelState.IsValid)
            {
                reportXUserSgq.AddDate = DateTime.Now;
                reportXUserSgq.IsActive = true;
                db.ReportXUserSgq.Add(reportXUserSgq);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemMenu_Id = new SelectList(db.ItemMenu, "Id", "Name", reportXUserSgq.ItemMenu_Id);
            ViewBag.ParCompany_Id = new SelectList(db.ParCompany, "Id", "Name", reportXUserSgq.ParCompany_Id);
            ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name", reportXUserSgq.ParLevel1_Id);
            return View(reportXUserSgq);
        }

        // GET: ParDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportXUserSgq reportXUserSgq = db.ReportXUserSgq.Where(x => x.Id == id).Include(x => x.ParCompany).Include(X => X.ParLevel1).FirstOrDefault();
            reportXUserSgq.parReportLayoutXReportXUser = db.ParReportLayoutXReportXUser.Where(x => x.ReportXUserSgq_Id == reportXUserSgq.Id).ToList();
            if (reportXUserSgq == null)
            {
                return HttpNotFound();
            }
            return View(reportXUserSgq);
        }

        // GET: ReportXUserSgqs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ReportXUserSgq reportXUserSgq = db.ReportXUserSgq.Find(id);

            if (reportXUserSgq == null)
            {
                return HttpNotFound();
            }

            ViewBag.ItemMenu_Id = new SelectList(db.ItemMenu, "Id", "Name", reportXUserSgq.ItemMenu_Id);
            ViewBag.ParCompany_Id = new SelectList(db.ParCompany, "Id", "Name", reportXUserSgq.ParCompany_Id);
            ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name", reportXUserSgq.ParLevel1_Id);
            return View(reportXUserSgq);
        }

        // POST: ReportXUserSgqs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AddDate,AlterData,NomeRelatorio,ItemMenu_Id,Elaborador,Aprovador,ParLevel1_Id,ParCompany_Id,IsActive,CodigoRelatorio")] ReportXUserSgq reportXUserSgq)
        {

            if (db.ReportXUserSgq.Any(r => r.ItemMenu_Id == reportXUserSgq.ItemMenu_Id &&
                r.ParLevel1_Id == reportXUserSgq.ParLevel1_Id &&
                r.ParCompany_Id == reportXUserSgq.ParCompany_Id && r.Id != reportXUserSgq.Id))
            {
                ModelState.AddModelError("ItemMenu_Id", "Já existe elaborador e editor para este relatório nesta unidade");
            }

            if (ModelState.IsValid)
            {
                reportXUserSgq.AlterDate = DateTime.Now;
                db.Entry(reportXUserSgq).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemMenu_Id = new SelectList(db.ItemMenu, "Id", "Name", reportXUserSgq.ItemMenu_Id);
            ViewBag.ParCompany_Id = new SelectList(db.ParCompany, "Id", "Name", reportXUserSgq.ParCompany_Id);
            ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name", reportXUserSgq.ParLevel1_Id);
            return View(reportXUserSgq);
        }

        // GET: ReportXUserSgqs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reportXUserSgq = db.ReportXUserSgq.Include(r => r.ItemMenu).Include(r => r.ParCompany).Include(r => r.ParLevel1).Where(r => r.Id == id).FirstOrDefault();
            if (reportXUserSgq == null)
            {
                return HttpNotFound();
            }
            return View(reportXUserSgq);
        }

        // POST: ReportXUserSgqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportXUserSgq reportXUserSgq = db.ReportXUserSgq.Find(id);
            db.ReportXUserSgq.Remove(reportXUserSgq);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

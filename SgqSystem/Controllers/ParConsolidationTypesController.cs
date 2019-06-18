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
    public class ParConsolidationTypesController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParConsolidationTypes
        public ActionResult Index()
        {
            return View(db.ParConsolidationType.ToList());
        }

        // GET: ParConsolidationTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParConsolidationType parConsolidationType = db.ParConsolidationType.Find(id);
            if (parConsolidationType == null)
            {
                return HttpNotFound();
            }
            return View(parConsolidationType);
        }

        // GET: ParConsolidationTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParConsolidationTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IsActive,AddDate,AlterDate")] ParConsolidationType parConsolidationType)
        {
            if (ModelState.IsValid)
            {
                db.ParConsolidationType.Add(parConsolidationType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parConsolidationType);
        }

        // GET: ParConsolidationTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParConsolidationType parConsolidationType = db.ParConsolidationType.Find(id);
            if (parConsolidationType == null)
            {
                return HttpNotFound();
            }
            return View(parConsolidationType);
        }

        // POST: ParConsolidationTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IsActive,AddDate,AlterDate")] ParConsolidationType parConsolidationType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parConsolidationType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parConsolidationType);
        }

        // GET: ParConsolidationTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParConsolidationType parConsolidationType = db.ParConsolidationType.Find(id);
            if (parConsolidationType == null)
            {
                return HttpNotFound();
            }
            return View(parConsolidationType);
        }

        // POST: ParConsolidationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParConsolidationType parConsolidationType = db.ParConsolidationType.Find(id);
            db.ParConsolidationType.Remove(parConsolidationType);
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

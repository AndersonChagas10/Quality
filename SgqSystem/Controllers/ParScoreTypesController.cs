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
    public class ParScoreTypesController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParScoreTypes
        public ActionResult Index()
        {
            return View(db.ParScoreType.ToList());
        }

        // GET: ParScoreTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParScoreType parScoreType = db.ParScoreType.Find(id);
            if (parScoreType == null)
            {
                return HttpNotFound();
            }
            return View(parScoreType);
        }

        // GET: ParScoreTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParScoreTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IsActive,AddDate,AlterDate")] ParScoreType parScoreType)
        {
            if (ModelState.IsValid)
            {
                db.ParScoreType.Add(parScoreType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parScoreType);
        }

        // GET: ParScoreTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParScoreType parScoreType = db.ParScoreType.Find(id);
            if (parScoreType == null)
            {
                return HttpNotFound();
            }
            return View(parScoreType);
        }

        // POST: ParScoreTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IsActive,AddDate,AlterDate")] ParScoreType parScoreType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parScoreType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parScoreType);
        }

        // GET: ParScoreTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParScoreType parScoreType = db.ParScoreType.Find(id);
            if (parScoreType == null)
            {
                return HttpNotFound();
            }
            return View(parScoreType);
        }

        // POST: ParScoreTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParScoreType parScoreType = db.ParScoreType.Find(id);
            db.ParScoreType.Remove(parScoreType);
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

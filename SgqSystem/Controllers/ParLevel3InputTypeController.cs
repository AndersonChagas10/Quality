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
    public class ParLevel3InputTypeController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParLevel3InputType
        public ActionResult Index()
        {
            return View(db.ParLevel3InputType.ToList());
        }

        // GET: ParLevel3InputType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel3InputType parLevel3InputType = db.ParLevel3InputType.Find(id);
            if (parLevel3InputType == null)
            {
                return HttpNotFound();
            }
            return View(parLevel3InputType);
        }

        // GET: ParLevel3InputType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParLevel3InputType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IsActive,Sampling,AddDate,AlterDate")] ParLevel3InputType parLevel3InputType)
        {
            if (ModelState.IsValid)
            {
                db.ParLevel3InputType.Add(parLevel3InputType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parLevel3InputType);
        }

        // GET: ParLevel3InputType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel3InputType parLevel3InputType = db.ParLevel3InputType.Find(id);
            if (parLevel3InputType == null)
            {
                return HttpNotFound();
            }
            return View(parLevel3InputType);
        }

        // POST: ParLevel3InputType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IsActive,Sampling,AddDate,AlterDate")] ParLevel3InputType parLevel3InputType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parLevel3InputType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parLevel3InputType);
        }

        // GET: ParLevel3InputType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel3InputType parLevel3InputType = db.ParLevel3InputType.Find(id);
            if (parLevel3InputType == null)
            {
                return HttpNotFound();
            }
            return View(parLevel3InputType);
        }

        // POST: ParLevel3InputType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParLevel3InputType parLevel3InputType = db.ParLevel3InputType.Find(id);
            db.ParLevel3InputType.Remove(parLevel3InputType);
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

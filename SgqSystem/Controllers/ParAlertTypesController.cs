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
    public class ParAlertTypesController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParAlertTypes
        public ActionResult Index()
        {
            return View(db.ParAlertType.ToList());
        }

        // GET: ParAlertTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParAlertType parAlertType = db.ParAlertType.Find(id);
            if (parAlertType == null)
            {
                return HttpNotFound();
            }
            return View(parAlertType);
        }

        // GET: ParAlertTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParAlertTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate")] ParAlertType parAlertType)
        {
            if (ModelState.IsValid)
            {
                db.ParAlertType.Add(parAlertType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parAlertType);
        }

        // GET: ParAlertTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParAlertType parAlertType = db.ParAlertType.Find(id);
            if (parAlertType == null)
            {
                return HttpNotFound();
            }
            return View(parAlertType);
        }

        // POST: ParAlertTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate")] ParAlertType parAlertType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parAlertType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parAlertType);
        }

        // GET: ParAlertTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParAlertType parAlertType = db.ParAlertType.Find(id);
            if (parAlertType == null)
            {
                return HttpNotFound();
            }
            return View(parAlertType);
        }

        // POST: ParAlertTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParAlertType parAlertType = db.ParAlertType.Find(id);
            db.ParAlertType.Remove(parAlertType);
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

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
    public class ParReasonTypesController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParReasonTypes
        public ActionResult Index()
        {
            return View(db.ParReasonType.ToList());
        }

        // GET: ParReasonTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParReasonType parReasonType = db.ParReasonType.Find(id);
            if (parReasonType == null)
            {
                return HttpNotFound();
            }
            return View(parReasonType);
        }

        // GET: ParReasonTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParReasonTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate")] ParReasonType parReasonType)
        {
            if (ModelState.IsValid)
            {
                db.ParReasonType.Add(parReasonType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parReasonType);
        }

        // GET: ParReasonTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParReasonType parReasonType = db.ParReasonType.Find(id);
            if (parReasonType == null)
            {
                return HttpNotFound();
            }
            return View(parReasonType);
        }

        // POST: ParReasonTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate")] ParReasonType parReasonType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parReasonType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parReasonType);
        }

        // GET: ParReasonTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParReasonType parReasonType = db.ParReasonType.Find(id);
            if (parReasonType == null)
            {
                return HttpNotFound();
            }
            return View(parReasonType);
        }

        // POST: ParReasonTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParReasonType parReasonType = db.ParReasonType.Find(id);
            db.ParReasonType.Remove(parReasonType);
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

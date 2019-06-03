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
    public class ResourceENsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ResourceENs
        public ActionResult Index()
        {
            return View(db.ResourceEN.OrderBy(x => x.Key).ToList());
        }

        // GET: ResourceENs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceEN resourceEN = db.ResourceEN.Find(id);
            if (resourceEN == null)
            {
                return HttpNotFound();
            }
            return View(resourceEN);
        }

        // GET: ResourceENs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResourceENs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Key,Value,AddDate,AlterDate")] ResourceEN resourceEN)
        {
            if (ModelState.IsValid)
            {
                db.ResourceEN.Add(resourceEN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(resourceEN);
        }

        // GET: ResourceENs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceEN resourceEN = db.ResourceEN.Find(id);
            if (resourceEN == null)
            {
                return HttpNotFound();
            }
            return View(resourceEN);
        }

        // POST: ResourceENs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Key,Value,AddDate,AlterDate")] ResourceEN resourceEN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resourceEN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resourceEN);
        }

        // GET: ResourceENs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceEN resourceEN = db.ResourceEN.Find(id);
            if (resourceEN == null)
            {
                return HttpNotFound();
            }
            return View(resourceEN);
        }

        // POST: ResourceENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResourceEN resourceEN = db.ResourceEN.Find(id);
            db.ResourceEN.Remove(resourceEN);
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

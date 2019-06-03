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
    public class ResourcePTsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ResourcePTs
        public ActionResult Index()
        {
            return View(db.ResourcePT.OrderBy(x => x.Key).ToList());
        }

        // GET: ResourcePTs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourcePT resourcePT = db.ResourcePT.Find(id);
            if (resourcePT == null)
            {
                return HttpNotFound();
            }
            return View(resourcePT);
        }

        // GET: ResourcePTs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResourcePTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Key,Value,AddDate,AlterDate")] ResourcePT resourcePT)
        {
            if (ModelState.IsValid)
            {
                db.ResourcePT.Add(resourcePT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(resourcePT);
        }

        // GET: ResourcePTs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourcePT resourcePT = db.ResourcePT.Find(id);
            if (resourcePT == null)
            {
                return HttpNotFound();
            }
            return View(resourcePT);
        }

        // POST: ResourcePTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Key,Value,AddDate,AlterDate")] ResourcePT resourcePT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resourcePT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resourcePT);
        }

        // GET: ResourcePTs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourcePT resourcePT = db.ResourcePT.Find(id);
            if (resourcePT == null)
            {
                return HttpNotFound();
            }
            return View(resourcePT);
        }

        // POST: ResourcePTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResourcePT resourcePT = db.ResourcePT.Find(id);
            db.ResourcePT.Remove(resourcePT);
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

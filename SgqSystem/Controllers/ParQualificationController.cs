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
    public class ParQualificationController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParQualification
        public ActionResult Index()
        {
            return View(db.ParQualification.ToList());
        }

        // GET: ParQualification/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParQualification parQualification = db.ParQualification.Find(id);
            if (parQualification == null)
            {
                return HttpNotFound();
            }
            return View(parQualification);
        }

        // GET: ParQualification/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParQualification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate")] ParQualification parQualification)
        {
            if (ModelState.IsValid)
            {
                var jaExiste = db.ParQualification.Where(
                    x => x.Name == parQualification.Name);
                if (jaExiste.Count() > 0)
                {
                    ViewBag.JaExiste = Resources.Resource.register_already_exist as string;
                }
                else
                {
                    db.ParQualification.Add(parQualification);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(parQualification);
        }

        // GET: ParQualification/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParQualification parQualification = db.ParQualification.Find(id);
            if (parQualification == null)
            {
                return HttpNotFound();
            }
            return View(parQualification);
        }

        // POST: ParQualification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate")] ParQualification parQualification)
        {
            if (ModelState.IsValid)
            {
                var jaExiste = db.ParQualification.Where(
                    x => x.Name == parQualification.Name
                    && x.Id != parQualification.Id);
                if (jaExiste.Count() > 0)
                {
                    ViewBag.JaExiste = Resources.Resource.register_already_exist as string;
                }
                else
                {
                    db.Entry(parQualification).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(parQualification);
        }

        // GET: ParQualification/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParQualification parQualification = db.ParQualification.Find(id);
            if (parQualification == null)
            {
                return HttpNotFound();
            }
            return View(parQualification);
        }

        // POST: ParQualification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParQualification parQualification = db.ParQualification.Find(id);
            db.ParQualification.Remove(parQualification);
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

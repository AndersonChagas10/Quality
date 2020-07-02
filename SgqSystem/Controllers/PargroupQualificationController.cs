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
    public class PargroupQualificationController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: PargroupQualification
        public ActionResult Index()
        {
            return View(db.PargroupQualification.ToList());
        }

        // GET: PargroupQualification/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PargroupQualification pargroupQualification = db.PargroupQualification.Find(id);
            if (pargroupQualification == null)
            {
                return HttpNotFound();
            }
            ViewBag.PargroupQualification_Id = id;
            pargroupQualification.PargroupQualificationXParQualification = db.PargroupQualificationXParQualification.Where(x => x.PargroupQualification_Id == id).ToList();
            return View(pargroupQualification);
        }

        // GET: PargroupQualification/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PargroupQualification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate")] PargroupQualification pargroupQualification)
        {
            if (ModelState.IsValid)
            {
                var jaExiste = db.PargroupQualification.Where(
                    x => x.Name == pargroupQualification.Name);
                if (jaExiste.Count() > 0)
                {
                    ViewBag.JaExiste = Resources.Resource.register_already_exist as string;
                }
                else
                {
                    db.PargroupQualification.Add(pargroupQualification);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(pargroupQualification);
        }

        // GET: PargroupQualification/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PargroupQualification pargroupQualification = db.PargroupQualification.Find(id);
            if (pargroupQualification == null)
            {
                return HttpNotFound();
            }
            return View(pargroupQualification);
        }

        // POST: PargroupQualification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate")] PargroupQualification pargroupQualification)
        {
            if (ModelState.IsValid)
            {
                var jaExiste = db.PargroupQualification.Where(
                    x => x.Name == pargroupQualification.Name
                    && x.Id != pargroupQualification.Id);
                if (jaExiste.Count() > 0)
                {
                    ViewBag.JaExiste = Resources.Resource.register_already_exist as string;
                }
                else
                {
                    db.Entry(pargroupQualification).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(pargroupQualification);
        }

        // GET: PargroupQualification/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PargroupQualification pargroupQualification = db.PargroupQualification.Find(id);
            if (pargroupQualification == null)
            {
                return HttpNotFound();
            }
            return View(pargroupQualification);
        }

        // POST: PargroupQualification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PargroupQualification pargroupQualification = db.PargroupQualification.Find(id);
            db.PargroupQualification.Remove(pargroupQualification);
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

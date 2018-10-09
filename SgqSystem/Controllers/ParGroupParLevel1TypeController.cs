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
    public class ParGroupParLevel1TypeController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParGroupParLevel1Type
        public ActionResult Index()
        {
            return View(db.ParGroupParLevel1Type.ToList());
        }

        // GET: ParGroupParLevel1Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1Type parGroupParLevel1Type = db.ParGroupParLevel1Type.Find(id);
            if (parGroupParLevel1Type == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1Type);
        }

        // GET: ParGroupParLevel1Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParGroupParLevel1Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive")] ParGroupParLevel1Type parGroupParLevel1Type)
        {
            ValidaTipoGrupo(parGroupParLevel1Type);
            if (ModelState.IsValid)
            {
                db.ParGroupParLevel1Type.Add(parGroupParLevel1Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parGroupParLevel1Type);
        }

        // GET: ParGroupParLevel1Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1Type parGroupParLevel1Type = db.ParGroupParLevel1Type.Find(id);
            if (parGroupParLevel1Type == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1Type);
        }

        // POST: ParGroupParLevel1Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive")] ParGroupParLevel1Type parGroupParLevel1Type)
        {
            ValidaTipoGrupo(parGroupParLevel1Type);
            if (ModelState.IsValid)
            {
                db.Entry(parGroupParLevel1Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parGroupParLevel1Type);
        }

        // GET: ParGroupParLevel1Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1Type parGroupParLevel1Type = db.ParGroupParLevel1Type.Find(id);
            if (parGroupParLevel1Type == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1Type);
        }

        // POST: ParGroupParLevel1Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParGroupParLevel1Type parGroupParLevel1Type = db.ParGroupParLevel1Type.Find(id);
            db.ParGroupParLevel1Type.Remove(parGroupParLevel1Type);
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

        private void ValidaTipoGrupo(ParGroupParLevel1Type parGroupParLevel1Type)
        {
            if (parGroupParLevel1Type.Name == null)
                ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);
        }
    }
}

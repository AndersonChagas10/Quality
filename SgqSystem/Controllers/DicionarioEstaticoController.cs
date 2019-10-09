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
    public class DicionarioEstaticoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: DicionarioEstatico
        public ActionResult Index()
        {
            return View(db.DicionarioEstatico.ToList());
        }

        // GET: DicionarioEstatico/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DicionarioEstatico dicionarioEstatico = db.DicionarioEstatico.Find(id);
            if (dicionarioEstatico == null)
            {
                return HttpNotFound();
            }
            return View(dicionarioEstatico);
        }

        // GET: DicionarioEstatico/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DicionarioEstatico/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Key,ControllerName,Value,Descricao")] DicionarioEstatico dicionarioEstatico)
        {
            if (ModelState.IsValid)
            {
                db.DicionarioEstatico.Add(dicionarioEstatico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dicionarioEstatico);
        }

        // GET: DicionarioEstatico/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DicionarioEstatico dicionarioEstatico = db.DicionarioEstatico.Find(id);
            if (dicionarioEstatico == null)
            {
                return HttpNotFound();
            }
            return View(dicionarioEstatico);
        }

        // POST: DicionarioEstatico/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Key,ControllerName,Value,Descricao")] DicionarioEstatico dicionarioEstatico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dicionarioEstatico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dicionarioEstatico);
        }

        // GET: DicionarioEstatico/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DicionarioEstatico dicionarioEstatico = db.DicionarioEstatico.Find(id);
            if (dicionarioEstatico == null)
            {
                return HttpNotFound();
            }
            return View(dicionarioEstatico);
        }

        // POST: DicionarioEstatico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DicionarioEstatico dicionarioEstatico = db.DicionarioEstatico.Find(id);
            db.DicionarioEstatico.Remove(dicionarioEstatico);
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

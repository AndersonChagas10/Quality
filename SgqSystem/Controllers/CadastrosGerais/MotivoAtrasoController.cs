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
    public class MotivoAtrasoController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: MotivoAtrasoes
        public ActionResult Index()
        {
            return View(db.MotivoAtraso.ToList());
        }

        // GET: MotivoAtrasoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotivoAtraso motivoAtraso = db.MotivoAtraso.Find(id);
            if (motivoAtraso == null)
            {
                return HttpNotFound();
            }
            return View(motivoAtraso);
        }

        // GET: MotivoAtrasoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MotivoAtrasoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Motivo,IsActive,AddDate,AlterDate")] MotivoAtraso motivoAtraso)
        {
            if (ModelState.IsValid)
            {
                motivoAtraso.AlterDate = null;
                motivoAtraso.AddDate = DateTime.Now;
                db.MotivoAtraso.Add(motivoAtraso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(motivoAtraso);
        }

        // GET: MotivoAtrasoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotivoAtraso motivoAtraso = db.MotivoAtraso.Find(id);
            if (motivoAtraso == null)
            {
                return HttpNotFound();
            }
            return View(motivoAtraso);
        }

        // POST: MotivoAtrasoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Motivo,IsActive,AddDate,AlterDate")] MotivoAtraso motivoAtraso)
        {
            if (ModelState.IsValid)
            {
                motivoAtraso.AlterDate = DateTime.Now;
                db.Entry(motivoAtraso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(motivoAtraso);
        }

        // GET: MotivoAtrasoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotivoAtraso motivoAtraso = db.MotivoAtraso.Find(id);
            if (motivoAtraso == null)
            {
                return HttpNotFound();
            }
            return View(motivoAtraso);
        }

        // POST: MotivoAtrasoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MotivoAtraso motivoAtraso = db.MotivoAtraso.Find(id);
            db.MotivoAtraso.Remove(motivoAtraso);
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

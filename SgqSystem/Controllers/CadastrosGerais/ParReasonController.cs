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
    public class ParReasonController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParReasones
        public ActionResult Index()
        {
            return View(db.ParReason.Include(x=>x.ParReasonType).ToList());
        }

        // GET: ParReasones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParReason ParReason = db.ParReason.Find(id);
            if (ParReason == null)
            {
                return HttpNotFound();
            }
            return View(ParReason);
        }

        // GET: ParReasones/Create
        public ActionResult Create()
        {
            ViewBag.ParReasonType_Id = new SelectList(db.ParReasonType.ToList(), "Id", "Name");
            return View();
        }

        // POST: ParReasones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Motivo,IsActive,AddDate,AlterDate,ParReasonType_Id")] ParReason ParReason)
        {
            if (ModelState.IsValid)
            {
                ParReason.AlterDate = null;
                ParReason.AddDate = DateTime.Now;
                db.ParReason.Add(ParReason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParReasonType_Id = new SelectList(db.ParReasonType.ToList(), "Id", "Name",ParReason.ParReasonType_Id);
            return View(ParReason);
        }

        // GET: ParReasones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParReason ParReason = db.ParReason.Find(id);
            if (ParReason == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParReasonType_Id = new SelectList(db.ParReasonType.ToList(), "Id", "Name", ParReason.ParReasonType_Id);
            return View(ParReason);
        }

        // POST: ParReasones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Motivo,IsActive,AddDate,AlterDate,ParReasonType_Id")] ParReason ParReason)
        {
            if (ModelState.IsValid)
            {
                ParReason.AlterDate = DateTime.Now;
                db.Entry(ParReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParReasonType_Id = new SelectList(db.ParReasonType.ToList(), "Id", "Name", ParReason.ParReasonType_Id);
            return View(ParReason);
        }

        // GET: ParReasones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParReason ParReason = db.ParReason.Find(id);
            if (ParReason == null)
            {
                return HttpNotFound();
            }
            return View(ParReason);
        }

        // POST: ParReasones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParReason ParReason = db.ParReason.Find(id);
            db.ParReason.Remove(ParReason);
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

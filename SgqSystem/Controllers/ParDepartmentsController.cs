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
    public class ParDepartmentsController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParDepartments
        public ActionResult Index()
        {
            return View(db.ParDepartment.ToList());
        }

        // GET: ParDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parDepartment);
        }

        // GET: ParDepartments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,AddDate,AlterDate,Active")] ParDepartment parDepartment)
        {
            if (ModelState.IsValid)
            {
                db.ParDepartment.Add(parDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parDepartment);
        }

        // GET: ParDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parDepartment);
        }

        // POST: ParDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,AddDate,AlterDate,Active")] ParDepartment parDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parDepartment);
        }

        // GET: ParDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            if (parDepartment == null)
            {
                return HttpNotFound();
            }
            return View(parDepartment);
        }

        // POST: ParDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParDepartment parDepartment = db.ParDepartment.Find(id);
            db.ParDepartment.Remove(parDepartment);
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

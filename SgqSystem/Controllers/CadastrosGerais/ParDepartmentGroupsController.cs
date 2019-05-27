using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;

namespace SgqSystem.Controllers.CadastrosGerais
{
    public class ParDepartmentGroupsController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParDepartmentGroups
        public ActionResult Index()
        {
            return View(db.ParDepartmentGroup.ToList());
        }

        // GET: ParDepartmentGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartmentGroup parDepartmentGroup = db.ParDepartmentGroup.Find(id);
            if (parDepartmentGroup == null)
            {
                return HttpNotFound();
            }
            return View(parDepartmentGroup);
        }

        // GET: ParDepartmentGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParDepartmentGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive,ParDepartment_Id")] ParDepartmentGroup parDepartmentGroup)
        {
            if (ModelState.IsValid)
            {
                db.ParDepartmentGroup.Add(parDepartmentGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parDepartmentGroup);
        }

        // GET: ParDepartmentGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartmentGroup parDepartmentGroup = db.ParDepartmentGroup.Find(id);
            if (parDepartmentGroup == null)
            {
                return HttpNotFound();
            }

            return View(parDepartmentGroup);
        }

        // POST: ParDepartmentGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive,ParDepartment_Id,AddDate,AlterDate")] ParDepartmentGroup parDepartmentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parDepartmentGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parDepartmentGroup);
        }

        // GET: ParDepartmentGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParDepartmentGroup parDepartmentGroup = db.ParDepartmentGroup.Find(id);
            if (parDepartmentGroup == null)
            {
                return HttpNotFound();
            }
            return View(parDepartmentGroup);
        }

        // POST: ParDepartmentGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParDepartmentGroup parDepartmentGroup = db.ParDepartmentGroup.Find(id);
            db.ParDepartmentGroup.Remove(parDepartmentGroup);
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

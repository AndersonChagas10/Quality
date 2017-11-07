using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;

namespace SgqSystem.Controllers.Params
{
    public class ParClusterGroupsController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParClusterGroups
        public ActionResult Index()
        {
            return View(db.ParClusterGroup.ToList());
        }

        // GET: ParClusterGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterGroup parClusterGroup = db.ParClusterGroup.Find(id);
            if (parClusterGroup == null)
            {
                return HttpNotFound();
            }
            return View(parClusterGroup);
        }

        // GET: ParClusterGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParClusterGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ParClusterGroupParent_Id,AddDate,AlterDate,IsActive")] ParClusterGroup parClusterGroup)
        {
            if (ModelState.IsValid)
            {
                parClusterGroup.IsActive = true;
                parClusterGroup.AddDate = DateTime.Now;
                db.ParClusterGroup.Add(parClusterGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parClusterGroup);
        }

        // GET: ParClusterGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterGroup parClusterGroup = db.ParClusterGroup.Find(id);
            if (parClusterGroup == null)
            {
                return HttpNotFound();
            }
            return View(parClusterGroup);
        }

        // POST: ParClusterGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ParClusterGroupParent_Id,AddDate,AlterDate,IsActive")] ParClusterGroup parClusterGroup)
        {
            if (ModelState.IsValid)
            {
                parClusterGroup.IsActive = true;
                parClusterGroup.AlterDate = DateTime.Now;               
                db.Entry(parClusterGroup).State = EntityState.Modified;
                db.Entry(parClusterGroup).Property(x => x.AddDate).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parClusterGroup);
        }

        // GET: ParClusterGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterGroup parClusterGroup = db.ParClusterGroup.Find(id);
            if (parClusterGroup == null)
            {
                return HttpNotFound();
            }
            return View(parClusterGroup);
        }

        // POST: ParClusterGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParClusterGroup parClusterGroup = db.ParClusterGroup.Find(id);
            db.ParClusterGroup.Remove(parClusterGroup);
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

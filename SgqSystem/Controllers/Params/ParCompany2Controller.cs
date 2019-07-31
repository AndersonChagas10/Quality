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
    public class ParCompany2Controller : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParCompany2
        public ActionResult Index()
        {
            return View(db.ParCompany.ToList());
        }

        // GET: ParCompany2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompany parCompany = db.ParCompany.Find(id);
            if (parCompany == null)
            {
                return HttpNotFound();
            }

            return View(parCompany);
        }

        // GET: ParCompany2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParCompany2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Name, Description, IsActive, Initials, SIF, CompanyNumber, IpServer, DBServer, IntegrationId, ParCompany_Id, Identification")] ParCompany parCompany)
        {
            ValidModelState(parCompany);
            if (ModelState.IsValid)
            {
                parCompany.AddDate = DateTime.Now;
                db.ParCompany.Add(parCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parCompany);
        }

        // GET: ParCompany2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompany parCompany = db.ParCompany.Find(id);
            if (parCompany == null)
            {
                return HttpNotFound();
            }
            return View(parCompany);
        }

        // POST: ParCompany2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, Description, IsActive, Initials, SIF, CompanyNumber, IpServer, DBServer, IntegrationId, ParCompany_Id, Identification")] ParCompany parCompany)
        {
            ValidModelState(parCompany);
            if (ModelState.IsValid)
            {
                parCompany.AlterDate = DateTime.Now;
                db.Entry(parCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parCompany);
        }

        // GET: ParCompany2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompany parCompany = db.ParCompany.Find(id);
            if (parCompany == null)
            {
                return HttpNotFound();
            }
            return View(parCompany);
        }

        // POST: ParCompany2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParCompany parCompany = db.ParCompany.Find(id);
            parCompany.AlterDate = DateTime.Now;
            parCompany.IsActive = false;
            db.Entry(parCompany).State = EntityState.Modified;
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

        private void ValidModelState(ParCompany parCompany)
        {
            if (parCompany.Identification.Length > 50) 
                ModelState.AddModelError("Identification", "O campo deve conter apenas 50 caracteres!");

            //if (totalDeVinculos > 0 && parCompany.IsActive == false) 
            //    ModelState.AddModelError("IsActive", Resources.Resource.module_link_indicator);

            //if (string.IsNullOrEmpty(parCompany.Name))
            //    ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);
        }
    }
}

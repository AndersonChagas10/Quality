﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;

namespace SgqSystem.Controllers
{
    public class ParModulesController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParModules
        public ActionResult Index()
        {
            return View(db.ParModule.ToList());
        }
    

        // GET: ParModules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParModule parModule = db.ParModule.Find(id);
            if (parModule == null)
            {
                return HttpNotFound();
            }
            parModule.ParModuleXModuleParent = db.ParModuleXModule.Where(x => x.ParModuleParent_Id == parModule.Id).ToList();
            ViewBag.EnableCreate = (db.ParModule.Count() - (parModule.ParModuleXModuleParent.Count()+1)) > 0;
            return View(parModule);
        }

        // GET: ParModules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,AddDate,AlterDate,IsActive")] ParModule parModule)
        {
            ValidModelState(parModule);
            if (ModelState.IsValid)
            {
                parModule.AddDate = DateTime.Now;
                db.ParModule.Add(parModule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parModule);
        }

        // GET: ParModules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParModule parModule = db.ParModule.Find(id);
            if (parModule == null)
            {
                return HttpNotFound();
            }
            return View(parModule);
        }

        // POST: ParModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,AddDate,AlterDate,IsActive")] ParModule parModule)
        {
            ValidModelState(parModule);
            if (ModelState.IsValid)
            {
                parModule.AlterDate = DateTime.Now;
                db.Entry(parModule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parModule);
        }

        // GET: ParModules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParModule parModule = db.ParModule.Find(id);
            if (parModule == null)
            {
                return HttpNotFound();
            }
            return View(parModule);
        }

        // POST: ParModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParModule parModule = db.ParModule.Find(id);
            db.ParModule.Remove(parModule);
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

        private void ValidModelState(ParModule parModule)
        {
            ModelState.Clear();

            var totalDeVinculos = db.ParLevel1XModule.Where(x => x.ParModule_Id == parModule.Id).Count();

            if (totalDeVinculos > 0 && parModule.IsActive == false) 
                ModelState.AddModelError("IsActive", Resources.Resource.module_link_indicator);

            if (string.IsNullOrEmpty(parModule.Name))
                ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);

            if (string.IsNullOrEmpty(parModule.Description))
                ModelState.AddModelError("Description", Resources.Resource.required_field + " " + Resources.Resource.description);
        }
    }
}

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
    public class IntegracaoSistemicasController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: IntegracaoSistemicas
        public ActionResult Index()
        {
            return View(db.IntegracaoSistemica.ToList());
        }

        // GET: IntegracaoSistemicas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IntegracaoSistemica integracaoSistemica = db.IntegracaoSistemica.Find(id);
            if (integracaoSistemica == null)
            {
                return HttpNotFound();
            }
            return View(integracaoSistemica);
        }

        // GET: IntegracaoSistemicas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IntegracaoSistemicas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Configuration,Script,TableName,Intervalo,IsActive,AddDate,AlterDate")] IntegracaoSistemica integracaoSistemica)
        {
            ValidaCamposObrigatorios(integracaoSistemica);
            if (ModelState.IsValid)
            {
                db.IntegracaoSistemica.Add(integracaoSistemica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(integracaoSistemica);
        }

        private void ValidaCamposObrigatorios(IntegracaoSistemica integracaoSistemica)
        {
            if (integracaoSistemica.Name == null)
                ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);

            if (integracaoSistemica.Configuration == null)
                ModelState.AddModelError("Configuration", Resources.Resource.required_field + " " + Resources.Resource.config);

            if (integracaoSistemica.Script == null)
                ModelState.AddModelError("Script", Resources.Resource.required_field + " Script");

            if (integracaoSistemica.TableName == null)
                ModelState.AddModelError("TableName", Resources.Resource.required_field + " " + Resources.Resource.table_name);

            if (integracaoSistemica.Intervalo <= 0)
                ModelState.AddModelError("Intervalo", Resources.Resource.required_field + " " + Resources.Resource.interval);
        }

        // GET: IntegracaoSistemicas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IntegracaoSistemica integracaoSistemica = db.IntegracaoSistemica.Find(id);
            if (integracaoSistemica == null)
            {
                return HttpNotFound();
            }
            return View(integracaoSistemica);
        }

        // POST: IntegracaoSistemicas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Configuration,Script,TableName,Intervalo,IsActive,AddDate,AlterDate")] IntegracaoSistemica integracaoSistemica)
        {
            ValidaCamposObrigatorios(integracaoSistemica);
            if (ModelState.IsValid)
            {
                db.Entry(integracaoSistemica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(integracaoSistemica);
        }

        // GET: IntegracaoSistemicas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IntegracaoSistemica integracaoSistemica = db.IntegracaoSistemica.Find(id);
            if (integracaoSistemica == null)
            {
                return HttpNotFound();
            }
            return View(integracaoSistemica);
        }

        // POST: IntegracaoSistemicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IntegracaoSistemica integracaoSistemica = db.IntegracaoSistemica.Find(id);
            db.IntegracaoSistemica.Remove(integracaoSistemica);
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

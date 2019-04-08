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
    public class ParColaboradorController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public ActionResult Index()
        {
            var parColaborador = db.ParColaborador.Include(p => p.ParCargo);
            return View(parColaborador.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.ParCargo_Id = new SelectList(db.ParCargo, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Documento,ParCargo_Id,IsActive,AddDate,AlterDate")] ParColaborador parColaborador)
        {
            if (ModelState.IsValid)
            {
                parColaborador.AddDate = DateTime.Now;
                db.ParColaborador.Add(parColaborador);

                db.ParColaboradorXCargo.Add(new ParColaboradorXCargo()
                {
                    AddDate = DateTime.Now,
                    IsActive = true,
                    ParCargo_Id = parColaborador.ParCargo_Id,
                    ParColaborador_Id = parColaborador.Id
                });

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParCargo_Id = new SelectList(db.ParCargo, "Id", "Name", parColaborador.ParCargo_Id);
            return View(parColaborador);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParColaborador parColaborador = db.ParColaborador.Find(id);
            if (parColaborador == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCargo_Id = new SelectList(db.ParCargo, "Id", "Name", parColaborador.ParCargo_Id);
            return View(parColaborador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Documento,ParCargo_Id,IsActive,AddDate,AlterDate")] ParColaborador parColaborador)
        {
            if (ModelState.IsValid)
            {
                parColaborador.AlterDate = DateTime.Now;
                db.Entry(parColaborador).State = EntityState.Modified;

                var parColaboradorXCargoAtual = db.ParColaboradorXCargo.Where(x => x.ParColaborador_Id == parColaborador.Id && x.IsActive).OrderByDescending(x => x.AddDate).FirstOrDefault();

                if(parColaboradorXCargoAtual != null)
                {
                    parColaboradorXCargoAtual.AlterDate = DateTime.Now;

                    if(parColaboradorXCargoAtual.ParCargo_Id != parColaborador.ParCargo_Id)
                    {
                        parColaboradorXCargoAtual.IsActive = false;
                    }              
                }

                if (parColaboradorXCargoAtual == null || parColaboradorXCargoAtual.ParCargo_Id != parColaborador.ParCargo_Id)
                {
                    db.ParColaboradorXCargo.Add(new ParColaboradorXCargo()
                    {
                        AddDate = DateTime.Now,
                        IsActive = true,
                        ParCargo_Id = parColaborador.ParCargo_Id,
                        ParColaborador_Id = parColaborador.Id
                    });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParCargo_Id = new SelectList(db.ParCargo, "Id", "Name", parColaborador.ParCargo_Id);
            return View(parColaborador);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParColaborador parColaborador = db.ParColaborador.Find(id);
            if (parColaborador == null)
            {
                return HttpNotFound();
            }
            return View(parColaborador);
        }

        // POST: ParColaborador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParColaborador parColaborador = db.ParColaborador.Find(id);
            db.ParColaborador.Remove(parColaborador);
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

using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class RotinaIntegracaoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: RotinaIntegracao
        public ActionResult Index()
        {
            return View(db.RotinaIntegracao.Where(x => x.IsActive).ToList());
        }

        // GET: RotinaIntegracao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RotinaIntegracao/Create
        [HttpPost]
        public ActionResult Create(RotinaIntegracao rotinaIntegracao)
        {
            try
            {
                ValidaItemMenu(rotinaIntegracao);

                if (!ModelState.IsValid)
                   return View(rotinaIntegracao);


                if (rotinaIntegracao.Id > 0)
                {
                    rotinaIntegracao.AlterDate = DateTime.Now;
                }

                db.RotinaIntegracao.AddOrUpdate(rotinaIntegracao);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: ItemMenu/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var rotinaIntegracao = db.RotinaIntegracao.Find(id);

            if (rotinaIntegracao == null)
            {
                return HttpNotFound();
            }

            return View("Edit", rotinaIntegracao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RotinaIntegracao rotinaIntegracao)
        {
            ValidaItemMenu(rotinaIntegracao);
           
            if (ModelState.IsValid)
            {
                db.Entry(rotinaIntegracao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rotinaIntegracao);
        }

        // GET: RotinaIntegracao/Delete/id
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RotinaIntegracao rotinaIntegracao = db.RotinaIntegracao.Find(id);
            if (rotinaIntegracao == null)
            {
                return HttpNotFound();
            }
            return View(rotinaIntegracao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RotinaIntegracao rotinaIntegracao = db.RotinaIntegracao.Find(id);
            rotinaIntegracao.IsActive = false;
            db.Entry(rotinaIntegracao).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void ValidaItemMenu(RotinaIntegracao rotinaIntegracao)
        {
            if (string.IsNullOrEmpty(rotinaIntegracao.Name))
            {
                ModelState.AddModelError("Name", Resources.Resource.name_can_not_be_empty);
            }
        }
    }
}
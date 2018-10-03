﻿using System;
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
    public class ParGroupParLevel1XParLevel3Controller : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParGroupParLevel1XParLevel3
        public ActionResult Index()
        {
            var parGroupParLevel1XParLevel3 = db.ParGroupParLevel1XParLevel3.Include(p => p.ParLevel1).Include(p => p.ParLevel3);
            return View(parGroupParLevel1XParLevel3.ToList());
        }

        // GET: ParGroupParLevel1XParLevel3/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParVinculoPeso parGroupParLevel1XParLevel3 = db.ParGroupParLevel1XParLevel3.Find(id);
            if (parGroupParLevel1XParLevel3 == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1XParLevel3);
        }

        // GET: ParGroupParLevel1XParLevel3/Create
        public ActionResult Create()
        {
            var listaTiposIndicadores = db.ParGroupParLevel1Type.Where(x => x.IsActive).ToList();
            listaTiposIndicadores.Add(new ParGroupParLevel1Type() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1Type_Id = new SelectList(listaTiposIndicadores, "Id", "Name", -1);

            var listaIndicador = db.ParLevel1.Where(x => x.IsActive).ToList();
            listaIndicador.Add(new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(listaIndicador, "Id", "Name", -1);

            var listaTarefa = db.ParLevel3.Where(x => x.IsActive).ToList();
            listaTarefa.Add(new ParLevel3() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel3_Id = new SelectList(listaTarefa, "Id", "Name", -1);

            var listaMonitoramento = db.ParLevel2.Where(x => x.IsActive).ToList();
            listaMonitoramento.Add(new ParLevel2() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel2_Id = new SelectList(listaMonitoramento, "Id", "Name", -1);

            var listaEmpresa = db.ParCompany.Where(x => x.IsActive).ToList();
            listaEmpresa.Add(new ParCompany() { Id = -1, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(listaEmpresa, "Id", "Name", -1);

            var listaDepartamento = db.ParDepartment.Where(x => x.Active).ToList();
            listaDepartamento.Add(new ParDepartment() { Id = -1, Name = "Selecione" });
            ViewBag.ParDepartment_Id = new SelectList(listaDepartamento, "Id", "Name", -1);

            var listaGrupoIndicadores = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGrupoIndicadores.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1_Id = new SelectList(listaGrupoIndicadores, "Id", "Name", -1);

            return View();
        }

        // POST: ParGroupParLevel1XParLevel3/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ParGroupParLevel1Type_Id,ParLevel3_Id,ParLevel1_Id,ParDepartment_Id,ParLevel2_Id,ParCompany_Id,Peso,IsActive,ParGroupParLevel1_Id")] ParVinculoPeso parGroupParLevel1XParLevel3)
        {
            if (ModelState.IsValid)
            {
                db.ParGroupParLevel1XParLevel3.Add(parGroupParLevel1XParLevel3);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ParGroupParLevel1Type_Id = new SelectList(db.ParGroupParLevel1Type, "Id", "Name", parGroupParLevel1XParLevel3.ParGroupParLevel1Type_Id);
            ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name", parGroupParLevel1XParLevel3.ParLevel1_Id);
            ViewBag.ParLevel3_Id = new SelectList(db.ParLevel3, "Id", "Name", parGroupParLevel1XParLevel3.ParLevel3_Id);
            return View(parGroupParLevel1XParLevel3);
        }

        // GET: ParGroupParLevel1XParLevel3/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParVinculoPeso parGroupParLevel1XParLevel3 = db.ParGroupParLevel1XParLevel3.Find(id);
            if (parGroupParLevel1XParLevel3 == null)
            {
                return HttpNotFound();
            }
            var listaTiposIndicadores = db.ParGroupParLevel1Type.Where(x => x.IsActive).ToList();
            listaTiposIndicadores.Add(new ParGroupParLevel1Type() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1Type_Id = new SelectList(listaTiposIndicadores, "Id", "Name", -1);

            var listaIndicador = db.ParLevel1.Where(x => x.IsActive).ToList();
            listaIndicador.Add(new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(listaIndicador, "Id", "Name", parGroupParLevel1XParLevel3.ParLevel1_Id);

            var listaTarefa = db.ParLevel3.Where(x => x.IsActive).ToList();
            listaTarefa.Add(new ParLevel3() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel3_Id = new SelectList(listaTarefa, "Id", "Name", parGroupParLevel1XParLevel3.ParLevel3_Id);

            var listaMonitoramento = db.ParLevel2.Where(x => x.IsActive).ToList();
            listaMonitoramento.Add(new ParLevel2() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel2_Id = new SelectList(listaMonitoramento, "Id", "Name", parGroupParLevel1XParLevel3.ParLevel2_Id);

            var listaEmpresa = db.ParCompany.Where(x => x.IsActive).ToList();
            listaEmpresa.Add(new ParCompany() { Id = -1, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(listaEmpresa, "Id", "Name", parGroupParLevel1XParLevel3.ParCompany_Id);

            var listaDepartamento = db.ParDepartment.Where(x => x.Active).ToList();
            listaDepartamento.Add(new ParDepartment() { Id = -1, Name = "Selecione" });
            ViewBag.ParDepartment_Id = new SelectList(listaDepartamento, "Id", "Name", parGroupParLevel1XParLevel3.ParDepartment_Id);

            var listaGrupoIndicadores = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGrupoIndicadores.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1_Id = new SelectList(listaGrupoIndicadores, "Id", "Name", parGroupParLevel1XParLevel3.ParGroupParLevel1_Id);

            return View(parGroupParLevel1XParLevel3);
        }
 
        // POST: ParGroupParLevel1XParLevel3/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParGroupParLevel1Type_Id,ParLevel3_Id,ParLevel1_Id,ParDepartment_Id,ParLevel2_Id,ParCompany_Id,Peso,IsActive,ParGroupParLevel1_Id")] ParVinculoPeso parGroupParLevel1XParLevel3)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parGroupParLevel1XParLevel3).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           // ViewBag.ParGroupParLevel1Type_Id = new SelectList(db.ParGroupParLevel1Type, "Id", "Name", parGroupParLevel1XParLevel3.ParGroupParLevel1Type_Id);
            ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name", parGroupParLevel1XParLevel3.ParLevel1_Id);
            ViewBag.ParLevel3_Id = new SelectList(db.ParLevel3, "Id", "Name", parGroupParLevel1XParLevel3.ParLevel3_Id);
            return View(parGroupParLevel1XParLevel3);
        }

        // GET: ParGroupParLevel1XParLevel3/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParVinculoPeso parGroupParLevel1XParLevel3 = db.ParGroupParLevel1XParLevel3.Find(id);
            if (parGroupParLevel1XParLevel3 == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1XParLevel3);
        }

        // POST: ParGroupParLevel1XParLevel3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParVinculoPeso parGroupParLevel1XParLevel3 = db.ParGroupParLevel1XParLevel3.Find(id);
            db.ParGroupParLevel1XParLevel3.Remove(parGroupParLevel1XParLevel3);
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

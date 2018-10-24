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
    public class ParGroupParLevel1XParLevel1Controller : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParGroupParLevel1XParLevel1
        public ActionResult Index()
        {
            var parGroupParLevel1XParLevel1 = db.ParGroupParLevel1XParLevel1.Include(p => p.ParGroupParLevel1).Include(p => p.ParLevel1);
            return View(parGroupParLevel1XParLevel1.ToList());
        }

        // GET: ParGroupParLevel1XParLevel1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1XParLevel1 parGroupParLevel1XParLevel1 = db.ParGroupParLevel1XParLevel1.Find(id);
            if (parGroupParLevel1XParLevel1 == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1XParLevel1);
        }

        // GET: ParGroupParLevel1XParLevel1/Create
        public ActionResult Create()
        {
           // ViewBag.ParGroupParLevel1_Id = new SelectList(db.ParGroupParLevel1, "Id", "Name");
           // ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name");

            var listaGrupos = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGrupos.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1_Id = new SelectList(listaGrupos, "Id", "Name", -1);

            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            listaIndicadores.Add(new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(listaIndicadores, "Id", "Name", -1);
            return View();
        }

        // POST: ParGroupParLevel1XParLevel1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParLevel1_Id,ParGroupParLevel1_Id,IsActive")] ParGroupParLevel1XParLevel1 parGroupParLevel1XParLevel1)
        {
            ValidaGrupoIndicadorXIndicador(parGroupParLevel1XParLevel1);
            if (ModelState.IsValid)
            {
                db.ParGroupParLevel1XParLevel1.Add(parGroupParLevel1XParLevel1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ParGroupParLevel1_Id = new SelectList(db.ParGroupParLevel1, "Id", "Name", parGroupParLevel1XParLevel1.ParGroupParLevel1_Id);
            //ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name", parGroupParLevel1XParLevel1.ParLevel1_Id);

            var listaGrupos = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGrupos.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1_Id = new SelectList(listaGrupos, "Id", "Name", -1);

            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            listaIndicadores.Add(new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(listaIndicadores, "Id", "Name", -1);

            return View(parGroupParLevel1XParLevel1);
        }

        private void ValidaGrupoIndicadorXIndicador(ParGroupParLevel1XParLevel1 parGroupParLevel1XParLevel1)
        {
            if (parGroupParLevel1XParLevel1.ParLevel1_Id <= 0)
                ModelState.AddModelError("ParLevel1_Id", Resources.Resource.required_field + " " + "Indicador");

            if (parGroupParLevel1XParLevel1.ParGroupParLevel1_Id <= 0)
                ModelState.AddModelError("ParGroupParLevel1_Id", Resources.Resource.required_field + " " + "Grupo Indicador");
        }

        // GET: ParGroupParLevel1XParLevel1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1XParLevel1 parGroupParLevel1XParLevel1 = db.ParGroupParLevel1XParLevel1.Find(id);
            if (parGroupParLevel1XParLevel1 == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ParGroupParLevel1_Id = new SelectList(db.ParGroupParLevel1, "Id", "Name", parGroupParLevel1XParLevel1.ParGroupParLevel1_Id);
            //ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name", parGroupParLevel1XParLevel1.ParLevel1_Id);


            var listaGrupos = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGrupos.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1_Id = new SelectList(listaGrupos, "Id", "Name", parGroupParLevel1XParLevel1.ParGroupParLevel1_Id);

            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            listaIndicadores.Add(new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(listaIndicadores, "Id", "Name", parGroupParLevel1XParLevel1.ParLevel1_Id);
            return View(parGroupParLevel1XParLevel1);
        }

        // POST: ParGroupParLevel1XParLevel1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParLevel1_Id,ParGroupParLevel1_Id,IsActive")] ParGroupParLevel1XParLevel1 parGroupParLevel1XParLevel1)
        {
            ValidaGrupoIndicadorXIndicador(parGroupParLevel1XParLevel1);
            if (ModelState.IsValid)
            {
                db.Entry(parGroupParLevel1XParLevel1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParGroupParLevel1_Id = new SelectList(db.ParGroupParLevel1, "Id", "Name", parGroupParLevel1XParLevel1.ParGroupParLevel1_Id);
            ViewBag.ParLevel1_Id = new SelectList(db.ParLevel1, "Id", "Name", parGroupParLevel1XParLevel1.ParLevel1_Id);
            return View(parGroupParLevel1XParLevel1);
        }

        // GET: ParGroupParLevel1XParLevel1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1XParLevel1 parGroupParLevel1XParLevel1 = db.ParGroupParLevel1XParLevel1.Find(id);
            if (parGroupParLevel1XParLevel1 == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1XParLevel1);
        }

        // POST: ParGroupParLevel1XParLevel1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParGroupParLevel1XParLevel1 parGroupParLevel1XParLevel1 = db.ParGroupParLevel1XParLevel1.Find(id);
            db.ParGroupParLevel1XParLevel1.Remove(parGroupParLevel1XParLevel1);
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

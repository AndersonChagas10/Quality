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
    public class ParGroupParLevel1Controller : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParGroupParLevel1
        public ActionResult Index()
        {
            var parGroupParLevel1 = db.ParGroupParLevel1.Include(p => p.ParGroupParLevel1Type);

            var listaFilhos = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaFilhos.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaFilhos, "Id", "Name", -1);

            return View(parGroupParLevel1.ToList());
        }

        // GET: ParGroupParLevel1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1 parGroupParLevel1 = db.ParGroupParLevel1.Find(id);
            if (parGroupParLevel1 == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1);
        }

        // GET: ParGroupParLevel1/Create
        public ActionResult Create()
        {
            var listaGrupos = db.ParGroupParLevel1Type.Where(x => x.IsActive).ToList();
            listaGrupos.Add(new ParGroupParLevel1Type() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1Type_Id = new SelectList(listaGrupos, "Id", "Name", -1);

            var listaGruposIndicador = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGruposIndicador.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaGruposIndicador, "Id", "Name", -1);

            var listaFilhos = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaFilhos.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaFilhos, "Id", "Name", -1);

            return View();
        }

        // POST: ParGroupParLevel1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ParGroupParLevel1Type_Id,IsActive,Parent_Id")] ParGroupParLevel1 parGroupParLevel1)
        {
            ValidaGrupoIndicadores(parGroupParLevel1);
            ValidaHash(parGroupParLevel1);
            if (ModelState.IsValid)
            {
                db.ParGroupParLevel1.Add(parGroupParLevel1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParGroupParLevel1Type_Id = new SelectList(db.ParGroupParLevel1Type, "Id", "Name", parGroupParLevel1.ParGroupParLevel1Type_Id);

            var listaFilhos = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaFilhos.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaFilhos, "Id", "Name", -1);

            return View(parGroupParLevel1);
        }

        private void ValidaHash(ParGroupParLevel1 parGroupParLevel1)
        {
            var pai = db.ParGroupParLevel1.AsNoTracking().Where(x => x.Parent_Id == parGroupParLevel1.Parent_Id).FirstOrDefault();

            if (pai != null)
            {
                if (pai.Hash != null)
                {
                    parGroupParLevel1.Hash = pai.Hash + "|" + pai.Parent_Id;
                }
                else
                {
                    parGroupParLevel1.Hash = pai.Parent_Id.ToString();
                }
            }
            else
            {
                parGroupParLevel1.Hash = null;
            }
        }

        // GET: ParGroupParLevel1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1 parGroupParLevel1 = db.ParGroupParLevel1.Find(id);
            if (parGroupParLevel1 == null)
            {
                return HttpNotFound();
            }
            var listaFilhos = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaFilhos.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaFilhos, "Id", "Name", parGroupParLevel1.Parent_Id);

            var listaGrupos = db.ParGroupParLevel1Type.Where(x => x.IsActive).ToList();
            listaGrupos.Add(new ParGroupParLevel1Type() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1Type_Id = new SelectList(listaGrupos, "Id", "Name", parGroupParLevel1.ParGroupParLevel1Type_Id);

            var listaGruposIndicador = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGruposIndicador.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaGruposIndicador, "Id", "Name", parGroupParLevel1.Parent_Id);
            //ViewBag.ParGroupParLevel1Type_Id = new SelectList(db.ParGroupParLevel1Type, "Id", "Name", parGroupParLevel1.ParGroupParLevel1Type_Id);
            return View(parGroupParLevel1);
        }

        // POST: ParGroupParLevel1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParGroupParLevel1Type_Id,IsActive,Parent_Id")] ParGroupParLevel1 parGroupParLevel1)
        {
            ValidaGrupoIndicadores(parGroupParLevel1);
            ValidaHash(parGroupParLevel1);
            if (ModelState.IsValid)
            {
                db.Entry(parGroupParLevel1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var listaGruposIndicador = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGruposIndicador.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.Parent_Id = new SelectList(listaGruposIndicador, "Id", "Name", parGroupParLevel1.Parent_Id);

            ViewBag.ParGroupParLevel1Type_Id = new SelectList(db.ParGroupParLevel1Type, "Id", "Name", parGroupParLevel1.ParGroupParLevel1Type_Id);
            return View(parGroupParLevel1);
        }

        // GET: ParGroupParLevel1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGroupParLevel1 parGroupParLevel1 = db.ParGroupParLevel1.Find(id);
            if (parGroupParLevel1 == null)
            {
                return HttpNotFound();
            }
            return View(parGroupParLevel1);
        }

        // POST: ParGroupParLevel1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParGroupParLevel1 parGroupParLevel1 = db.ParGroupParLevel1.Find(id);
            db.ParGroupParLevel1.Remove(parGroupParLevel1);
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

        private void ValidaGrupoIndicadores(ParGroupParLevel1 parGroupParLevel1)
        {
            if (parGroupParLevel1.Name == null)
                ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);

            if (parGroupParLevel1.ParGroupParLevel1Type_Id <= 0)
                ModelState.AddModelError("ParGroupParLevel1Type_Id", Resources.Resource.required_field + " " + "Grupo de Tipo de Indicador");
        }

    }
}

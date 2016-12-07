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
    public class VacuoGRDsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: VacuoGRDs
        public ActionResult Index()
        {
            var vacuoGRD = db.VacuoGRD.Include(v => v.ParCompany).Include(v => v.ParLevel1);
            return View(vacuoGRD.ToList());
        }

        // GET: VacuoGRDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacuoGRD vacuoGRD = db.VacuoGRD.Find(id);
            if (vacuoGRD == null)
            {
                return HttpNotFound();
            }
            return View(vacuoGRD);
        }

        // GET: VacuoGRDs/Create
        public ActionResult Create()
        {
            ViewBag.ParCompany_id = new SelectList(db.ParCompany, "Id", "Name");
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name");
            return View();
        }

        // POST: VacuoGRDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VacuoGRD vacuoGRD)
        {
            if (ModelState.IsValid)
            {
                db.VacuoGRD.Add(vacuoGRD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParCompany_id = new SelectList(db.ParCompany, "Id", "Name", vacuoGRD.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", vacuoGRD.ParLevel1_id);
            return View(vacuoGRD);
        }

        // GET: VacuoGRDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacuoGRD vacuoGRD = db.VacuoGRD.Find(id);
            if (vacuoGRD == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany, "Id", "Name", vacuoGRD.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", vacuoGRD.ParLevel1_id);
            return View(vacuoGRD);
        }

        // POST: VacuoGRDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,HorasTrabalhadasPorDia,AmostraPorDia,QtdadeFamiliaProduto,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VacuoGRD vacuoGRD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacuoGRD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany, "Id", "Name", vacuoGRD.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", vacuoGRD.ParLevel1_id);
            return View(vacuoGRD);
        }

        // GET: VacuoGRDs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VacuoGRD vacuoGRD = db.VacuoGRD.Find(id);
            if (vacuoGRD == null)
            {
                return HttpNotFound();
            }
            return View(vacuoGRD);
        }

        // POST: VacuoGRDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VacuoGRD vacuoGRD = db.VacuoGRD.Find(id);
            db.VacuoGRD.Remove(vacuoGRD);
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

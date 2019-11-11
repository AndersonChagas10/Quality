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
    public class ParVinculoPesoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParVinculoPeso
        public ActionResult Index()
        {
            var ParVinculoPeso = db.ParVinculoPeso.Include(p => p.ParLevel1).Include(p => p.ParLevel3);
            return View(ParVinculoPeso.ToList());
        }

        // GET: ParVinculoPeso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParVinculoPeso ParVinculoPeso = db.ParVinculoPeso.Find(id);
            if (ParVinculoPeso == null)
            {
                return HttpNotFound();
            }
            return View(ParVinculoPeso);
        }

        // GET: ParVinculoPeso/Create
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

        // POST: ParVinculoPeso/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ParGroupParLevel1Type_Id,ParLevel3_Id,ParLevel1_Id,ParDepartment_Id,ParLevel2_Id,ParCompany_Id,Peso,IsActive,EffectiveDateStart,EffectiveDateEnd,ParGroupParLevel1_Id")] ParVinculoPeso ParVinculoPeso)
        {
            ValidaGrupoIndicadorXTarefa(ParVinculoPeso);
            if (ModelState.IsValid)
            {
                db.ParVinculoPeso.Add(ParVinculoPeso);
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }

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

            return View(ParVinculoPeso);
        }

        private void ValidaGrupoIndicadorXTarefa(ParVinculoPeso ParVinculoPeso)
        {
            var existe = true;

            existe = db.ParVinculoPeso
               .Any(
               x => ((x.ParCompany_Id == ParVinculoPeso.ParCompany_Id &&
               x.ParDepartment_Id == ParVinculoPeso.ParDepartment_Id &&
               x.ParGroupParLevel1_Id == ParVinculoPeso.ParGroupParLevel1_Id &&
               x.ParLevel1_Id == ParVinculoPeso.ParLevel1_Id &&
               x.ParLevel2_Id == ParVinculoPeso.ParLevel2_Id &&
               x.ParLevel3_Id == ParVinculoPeso.ParLevel3_Id) ||
               x.Name == ParVinculoPeso.Name) && x.Id != ParVinculoPeso.Id);


            if (!existe)
            {
                if (ParVinculoPeso.Peso == 0 || ParVinculoPeso.Peso == null)
                    ModelState.AddModelError("Peso", Resources.Resource.required_field + " " + Resources.Resource.weight);

                if (ParVinculoPeso.Name == "" || ParVinculoPeso.Name == null)
                    ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);

                if (ParVinculoPeso.ParDepartment_Id <= 0)
                    ModelState.AddModelError("ParDepartment_Id", Resources.Resource.required_field + " " + Resources.Resource.department);

                if (ParVinculoPeso.ParCompany_Id <= 0)
                    ModelState.AddModelError("ParCompany_Id", Resources.Resource.required_field + " " + Resources.Resource.unit);

                if (ParVinculoPeso.ParLevel3_Id <= 0)
                    ModelState.AddModelError("ParLevel3_Id", Resources.Resource.required_field + " " + Resources.Resource.task);

                if (ParVinculoPeso.ParLevel2_Id <= 0)
                    ModelState.AddModelError("ParLevel2_Id", Resources.Resource.required_field + " " + Resources.Resource.monitoring);

                if (ParVinculoPeso.ParLevel1_Id <= 0)
                    ModelState.AddModelError("ParLevel1_Id", Resources.Resource.required_field + " " + "Indicador");

                if (ParVinculoPeso.ParGroupParLevel1_Id <= 0)
                    ModelState.AddModelError("ParGroupParLevel1_Id", Resources.Resource.required_field + " " + "Grupo Indicadores");
            }
            else
            {
                ModelState.AddModelError("ParLevel1_Id", Resources.Resource.link_alredy_used);
            }
        }

        // GET: ParVinculoPeso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParVinculoPeso ParVinculoPeso = db.ParVinculoPeso.Find(id);
            if (ParVinculoPeso == null)
            {
                return HttpNotFound();
            }
            var listaTiposIndicadores = db.ParGroupParLevel1Type.Where(x => x.IsActive).ToList();
            listaTiposIndicadores.Add(new ParGroupParLevel1Type() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1Type_Id = new SelectList(listaTiposIndicadores, "Id", "Name", -1);

            var listaIndicador = db.ParLevel1.Where(x => x.IsActive).ToList();
            listaIndicador.Add(new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(listaIndicador, "Id", "Name", ParVinculoPeso.ParLevel1_Id);

            var listaTarefa = db.ParLevel3.Where(x => x.IsActive).ToList();
            listaTarefa.Add(new ParLevel3() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel3_Id = new SelectList(listaTarefa, "Id", "Name", ParVinculoPeso.ParLevel3_Id);

            var listaMonitoramento = db.ParLevel2.Where(x => x.IsActive).ToList();
            listaMonitoramento.Add(new ParLevel2() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel2_Id = new SelectList(listaMonitoramento, "Id", "Name", ParVinculoPeso.ParLevel2_Id);

            var listaEmpresa = db.ParCompany.Where(x => x.IsActive).ToList();
            listaEmpresa.Add(new ParCompany() { Id = -1, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(listaEmpresa, "Id", "Name", ParVinculoPeso.ParCompany_Id);

            var listaDepartamento = db.ParDepartment.Where(x => x.Active).ToList();
            listaDepartamento.Add(new ParDepartment() { Id = -1, Name = "Selecione" });
            ViewBag.ParDepartment_Id = new SelectList(listaDepartamento, "Id", "Name", ParVinculoPeso.ParDepartment_Id);

            var listaGrupoIndicadores = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGrupoIndicadores.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1_Id = new SelectList(listaGrupoIndicadores, "Id", "Name", ParVinculoPeso.ParGroupParLevel1_Id);

            return View(ParVinculoPeso);
        }

        // POST: ParVinculoPeso/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParGroupParLevel1Type_Id,ParLevel3_Id,ParLevel1_Id,ParDepartment_Id,ParLevel2_Id,ParCompany_Id,Peso,EffectiveDateStart,EffectiveDateEnd,IsActive,ParGroupParLevel1_Id")] ParVinculoPeso ParVinculoPeso)
        {
            ValidaGrupoIndicadorXTarefa(ParVinculoPeso);
            if (ModelState.IsValid)
            {
                db.Entry(ParVinculoPeso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var listaTiposIndicadores = db.ParGroupParLevel1Type.Where(x => x.IsActive).ToList();
            listaTiposIndicadores.Add(new ParGroupParLevel1Type() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1Type_Id = new SelectList(listaTiposIndicadores, "Id", "Name", -1);

            var listaIndicador = db.ParLevel1.Where(x => x.IsActive).ToList();
            listaIndicador.Add(new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(listaIndicador, "Id", "Name", ParVinculoPeso.ParLevel1_Id);

            var listaTarefa = db.ParLevel3.Where(x => x.IsActive).ToList();
            listaTarefa.Add(new ParLevel3() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel3_Id = new SelectList(listaTarefa, "Id", "Name", ParVinculoPeso.ParLevel3_Id);

            var listaMonitoramento = db.ParLevel2.Where(x => x.IsActive).ToList();
            listaMonitoramento.Add(new ParLevel2() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel2_Id = new SelectList(listaMonitoramento, "Id", "Name", ParVinculoPeso.ParLevel2_Id);

            var listaEmpresa = db.ParCompany.Where(x => x.IsActive).ToList();
            listaEmpresa.Add(new ParCompany() { Id = -1, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(listaEmpresa, "Id", "Name", ParVinculoPeso.ParCompany_Id);

            var listaDepartamento = db.ParDepartment.Where(x => x.Active).ToList();
            listaDepartamento.Add(new ParDepartment() { Id = -1, Name = "Selecione" });
            ViewBag.ParDepartment_Id = new SelectList(listaDepartamento, "Id", "Name", ParVinculoPeso.ParDepartment_Id);

            var listaGrupoIndicadores = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
            listaGrupoIndicadores.Add(new ParGroupParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParGroupParLevel1_Id = new SelectList(listaGrupoIndicadores, "Id", "Name", ParVinculoPeso.ParGroupParLevel1_Id);
            return View(ParVinculoPeso);
        }

        // GET: ParVinculoPeso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParVinculoPeso ParVinculoPeso = db.ParVinculoPeso.Find(id);
            if (ParVinculoPeso == null)
            {
                return HttpNotFound();
            }
            return View(ParVinculoPeso);
        }

        // POST: ParVinculoPeso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParVinculoPeso ParVinculoPeso = db.ParVinculoPeso.Find(id);
            db.ParVinculoPeso.Remove(ParVinculoPeso);
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

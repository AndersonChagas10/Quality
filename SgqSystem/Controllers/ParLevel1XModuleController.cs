using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;
using DTO.Helpers;
using Microsoft.Ajax.Utilities;

namespace SgqSystem.Controllers
{
    public class ParLevel1XModuleController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParLevel1XModule
        public ActionResult Index()
        {
            return View(db.ParLevel1XModule.Where(x => x.IsActive).OrderBy(x => x.ParModule_Id).ToList());
        }

        // GET: ParLevel1XModule/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel1XModule parLevel1XModule = await db.ParLevel1XModule.FindAsync(id);
            if (parLevel1XModule == null)
            {
                return HttpNotFound();
            }
            return View(parLevel1XModule);
        }

        // GET: ParLevel1XModule/Create
        public ActionResult Create()
        {
            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            var listaModulos = db.ParModule.Where(x => x.IsActive).ToList();
            ViewBag.Indicadores = listaIndicadores;
            ViewBag.Modulos = listaModulos;
            return View();
        }

        // POST: ParLevel1XModule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Points,ParModule_Id,IsActive,EffectiveDateStart,EffectiveDateEnd,ParLevel1Helper,ParLevel1_IdHelper")] ParLevel1XModule parLevel1XModule)
        {
            parLevel1XModule.AddDate = DateTime.Now;
            parLevel1XModule.AlterDate = DateTime.Now;
            var quantSalvo = 0;
            if (parLevel1XModule.ParLevel1_IdHelper?.Count() > 0)
            {
                foreach (var item in parLevel1XModule.ParLevel1_IdHelper)
                {
                    parLevel1XModule.ParLevel1_Id = item;
                    ValidaIndicadoresxModulos(parLevel1XModule);
                    ValidaDataEntre(parLevel1XModule);
                    if (ModelState.IsValid)
                    {
                        db.ParLevel1XModule.Add(parLevel1XModule);
                        await db.SaveChangesAsync();
                        quantSalvo++;

                    }
                }
                if (quantSalvo == parLevel1XModule.ParLevel1_IdHelper.Count())
                    return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("ParLevel1_IdHelper", Resources.Resource.required_field + " " + Resources.Resource.parlevel1);
            }
            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            var listaModulos = db.ParModule.Where(x => x.IsActive).ToList();
            ViewBag.Indicadores = listaIndicadores;
            ViewBag.Modulos = listaModulos;
            return View(parLevel1XModule);
        }

        // GET: ParLevel1XModule/Edit/5
        public async Task<ActionResult> Edit(int? id, int? moduloId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel1XModule parLevel1XModule = await db.ParLevel1XModule.FindAsync(id);
            if (parLevel1XModule == null)
            {
                return HttpNotFound();
            }
            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            var listaModulos = db.ParModule.Where(x => x.IsActive).ToList();
            ViewBag.Indicadores = listaIndicadores;
            ViewBag.Modulos = listaModulos;
            return View(parLevel1XModule);
        }

        // POST: ParLevel1XModule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParLevel1_Id,ParModule_Id,Points,IsActive,EffectiveDateStart,EffectiveDateEnd,ParLevel1Helper")] ParLevel1XModule parLevel1XModule)
        {
            parLevel1XModule.AlterDate = DateTime.Now;
            ValidaIndicadoresxModulosEdicao(parLevel1XModule);
            ValidaDataEntre(parLevel1XModule);
            var indicadorxModuloEditado = db.ParLevel1XModule.Where(x => x.Id == parLevel1XModule.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                indicadorxModuloEditado.EffectiveDateEnd = parLevel1XModule.EffectiveDateEnd;
                indicadorxModuloEditado.EffectiveDateStart = parLevel1XModule.EffectiveDateStart;               
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            var listaModulos = db.ParModule.Where(x => x.IsActive).ToList();
            ViewBag.Indicadores = listaIndicadores;
            ViewBag.Modulos = listaModulos;
            return View(parLevel1XModule);
        }

        private void ValidaIndicadoresxModulosEdicao(ParLevel1XModule parLevel1XModule)
        {
            if (parLevel1XModule.ParLevel1_Id == 0)
                ModelState.AddModelError("ParLevel1_Id", Resources.Resource.required_field + " " + Resources.Resource.parlevel1);

            if (parLevel1XModule.ParModule_Id.IsNull())
                ModelState.AddModelError("ParModule_Id", Resources.Resource.required_field + " " + Resources.Resource.parModule);

            if (parLevel1XModule.EffectiveDateStart.IsNull())
                ModelState.AddModelError("EffectiveDateStart", Resources.Resource.required_field + " " + Resources.Resource.effective_date_start);

            if (parLevel1XModule.EffectiveDateStart > parLevel1XModule.EffectiveDateEnd)
                ModelState.AddModelError("EffectiveDateEnd", Resources.Resource.effective_date_start + " " + Resources.Resource.are_greater_than + " " + Resources.Resource.effective_date_end);
        }

        // GET: ParLevel1XModule/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel1XModule parLevel1XModule = await db.ParLevel1XModule.FindAsync(id);
            if (parLevel1XModule == null)
            {
                return HttpNotFound();
            }
            return View(parLevel1XModule);
        }

        // POST: ParLevel1XModule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParLevel1XModule parLevel1XModule = await db.ParLevel1XModule.FindAsync(id);
            db.ParLevel1XModule.Remove(parLevel1XModule);
            await db.SaveChangesAsync();
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

        private void ValidaIndicadoresxModulos(ParLevel1XModule parLevel1XModule)
        {
            //parLevel1XModule.ParLevel1 == null && 
            if (parLevel1XModule.ParLevel1_Id == 0)
                ModelState.AddModelError("ParLevel1_Id", Resources.Resource.required_field + " " + Resources.Resource.parlevel1);

            if (parLevel1XModule.ParModule_Id.IsNull())
                ModelState.AddModelError("ParModule_Id", Resources.Resource.required_field + " " + Resources.Resource.parModule);

            if (parLevel1XModule.ParLevel1_IdHelper.IsNull())
                ModelState.AddModelError("ParLevel1_IdHelper", Resources.Resource.required_field + " " + Resources.Resource.parlevel1);

            if (!parLevel1XModule.IsActive)
                ModelState.AddModelError("IsActive", Resources.Resource.required_field + " " + Resources.Resource.is_active);

            if (parLevel1XModule.EffectiveDateStart.IsNull())
                ModelState.AddModelError("EffectiveDateStart", Resources.Resource.required_field + " " + Resources.Resource.effective_date_start);

            if (parLevel1XModule.EffectiveDateStart > parLevel1XModule.EffectiveDateEnd) 
                ModelState.AddModelError("EffectiveDateEnd", Resources.Resource.effective_date_start + " " + Resources.Resource.are_greater_than + " " + Resources.Resource.effective_date_end);

        }
        private void ValidaDataEntre(ParLevel1XModule parLevel1XModule)
        {
            var indicadorXmodulo = db.ParLevel1XModule.AsNoTracking()
                .Where(x => x.ParModule_Id == parLevel1XModule.ParModule_Id)
                .Where(x => x.ParLevel1_Id == parLevel1XModule.ParLevel1_Id)
                .Where(x => x.IsActive).FirstOrDefault();

            if (parLevel1XModule.EffectiveDateStart < indicadorXmodulo?.EffectiveDateStart && parLevel1XModule.EffectiveDateEnd > indicadorXmodulo?.EffectiveDateEnd)
                ModelState.AddModelError("EffectiveDateEnd", "Já existe um vínculo no período selecionado");

            if (parLevel1XModule.EffectiveDateStart < indicadorXmodulo?.EffectiveDateStart && parLevel1XModule.EffectiveDateEnd < indicadorXmodulo?.EffectiveDateEnd)
                ModelState.AddModelError("EffectiveDateStart", "Já existe um vínculo no período selecionado");

            if (parLevel1XModule.EffectiveDateStart <= indicadorXmodulo?.EffectiveDateStart && parLevel1XModule.EffectiveDateEnd > indicadorXmodulo?.EffectiveDateEnd)
                ModelState.AddModelError("EffectiveDateStart", "Já existe um vínculo no período selecionado");

            if (parLevel1XModule.EffectiveDateStart >= indicadorXmodulo?.EffectiveDateStart && parLevel1XModule.EffectiveDateEnd == null || 
                parLevel1XModule.EffectiveDateEnd == DateTime.MinValue || parLevel1XModule.EffectiveDateEnd < indicadorXmodulo?.EffectiveDateEnd)
                ModelState.AddModelError("EffectiveDateStart", "Já existe um vínculo no período selecionado");

        }
    }
}

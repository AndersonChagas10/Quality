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

namespace SgqSystem.Controllers
{
    public class ParLevel1XModuleController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParLevel1XModule
        public async Task<ActionResult> Index()
        {
            return View(await db.ParLevel1XModule.ToListAsync());
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
        public async Task<ActionResult> Create([Bind(Include = "Id,ParLevel1_Id,ParModule_Id,Points,IsActive,EffectiveDateStart,EffectiveDateEnd")] ParLevel1XModule parLevel1XModule)
        {
            parLevel1XModule.AddDate = DateTime.Now;
            parLevel1XModule.AlterDate = DateTime.Now;
            ValidaIndicadoresxModulos(parLevel1XModule);
            if (ModelState.IsValid)
            {
                db.ParLevel1XModule.Add(parLevel1XModule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(parLevel1XModule);
        }

        // GET: ParLevel1XModule/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParLevel1_Id,ParModule_Id,Points,IsActive,EffectiveDateStart,EffectiveDateEnd")] ParLevel1XModule parLevel1XModule)
        {
            parLevel1XModule.AlterDate = DateTime.Now;
            

            if (ModelState.IsValid)
            {
                db.Entry(parLevel1XModule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(parLevel1XModule);
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
                ModelState.AddModelError("ParLevel1_Id", Guard.MesangemModelError("Indicador", true));

            if (parLevel1XModule.ParModule_Id == 0)
                ModelState.AddModelError("ParModule_Id", Guard.MesangemModelError("Modulos", true));
        }
    }
}

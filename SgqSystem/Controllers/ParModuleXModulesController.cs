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

namespace SgqSystem.Controllers
{
    public class ParModuleXModulesController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParModuleXModules
        public async Task<ActionResult> Index()
        {
            var parModuleXModule = db.ParModuleXModule.Include(p => p.ParModule).Include(p => p.ParModule1);
            return View(await parModuleXModule.ToListAsync());
        }

        // GET: ParModuleXModules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParModuleXModule parModuleXModule = await db.ParModuleXModule.FindAsync(id);
            if (parModuleXModule == null)
            {
                return HttpNotFound();
            }
            return View(parModuleXModule);
        }

        // GET: ParModuleXModules/Create
        public ActionResult Create(int moduleId)
        {
            ViewBag.ModuleId = moduleId;
            ViewBag.ParModuleChild_Id = new SelectList(db.ParModule, "Id", "Name");
            ViewBag.ParModuleParent_Id = new SelectList(db.ParModule, "Id", "Name", moduleId);
            return View();
        }

        // POST: ParModuleXModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParModuleParent_Id,ParModuleChild_Id")] ParModuleXModule parModuleXModule)
        {
            if (ModelState.IsValid)
            {
                db.ParModuleXModule.Add(parModuleXModule);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","ParModules",new { id = parModuleXModule.ParModuleParent_Id });
            }

            ViewBag.ParModuleChild_Id = new SelectList(db.ParModule, "Id", "Name", parModuleXModule.ParModuleChild_Id);
            ViewBag.ParModuleParent_Id = new SelectList(db.ParModule, "Id", "Name", parModuleXModule.ParModuleParent_Id);
            return View(parModuleXModule);
        }

        // GET: ParModuleXModules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParModuleXModule parModuleXModule = await db.ParModuleXModule.FindAsync(id);
            if (parModuleXModule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParModuleChild_Id = new SelectList(db.ParModule, "Id", "Name", parModuleXModule.ParModuleChild_Id);
            ViewBag.ParModuleParent_Id = new SelectList(db.ParModule, "Id", "Name", parModuleXModule.ParModuleParent_Id);
            return View(parModuleXModule);
        }

        // POST: ParModuleXModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParModuleParent_Id,ParModuleChild_Id")] ParModuleXModule parModuleXModule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parModuleXModule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "ParModules", new { id = parModuleXModule.ParModuleParent_Id });
            }
            ViewBag.ParModuleChild_Id = new SelectList(db.ParModule, "Id", "Name", parModuleXModule.ParModuleChild_Id);
            ViewBag.ParModuleParent_Id = new SelectList(db.ParModule, "Id", "Name", parModuleXModule.ParModuleParent_Id);
            return View(parModuleXModule);
        }

        // GET: ParModuleXModules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParModuleXModule parModuleXModule = await db.ParModuleXModule.FindAsync(id);
            if (parModuleXModule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModuleId = parModuleXModule.ParModuleParent_Id;
            return View(parModuleXModule);
        }

        // POST: ParModuleXModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParModuleXModule parModuleXModule = await db.ParModuleXModule.FindAsync(id);
            db.ParModuleXModule.Remove(parModuleXModule);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "ParModules", new { id = parModuleXModule.ParModuleParent_Id });
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

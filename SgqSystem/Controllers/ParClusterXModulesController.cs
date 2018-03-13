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
    public class ParClusterXModulesController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParClusterXModules
        public async Task<ActionResult> Index()
        {
            var parClusterXModule = db.ParClusterXModule.Include(p => p.ParCluster).Include(p => p.ParModule);
            return View(await parClusterXModule.ToListAsync());
        }

        // GET: ParClusterXModules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterXModule parClusterXModule = await db.ParClusterXModule.FindAsync(id);
            if (parClusterXModule == null)
            {
                return HttpNotFound();
            }
            return View(parClusterXModule);
        }

        // GET: ParClusterXModules/Create
        public ActionResult Create()
        {
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name");
            ViewBag.ParModule_Id = new SelectList(db.ParModule, "Id", "Name");
            return View();
        }

        // POST: ParClusterXModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParCluster_Id,ParModule_Id,Points,AddDate,AlterDate,IsActive,EffectiveDate")] ParClusterXModule parClusterXModule)
        {
            if (ModelState.IsValid)
            {
                db.ParClusterXModule.Add(parClusterXModule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name", parClusterXModule.ParCluster_Id);
            ViewBag.ParModule_Id = new SelectList(db.ParModule, "Id", "Name", parClusterXModule.ParModule_Id);
            return View(parClusterXModule);
        }

        // GET: ParClusterXModules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterXModule parClusterXModule = await db.ParClusterXModule.FindAsync(id);
            if (parClusterXModule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name", parClusterXModule.ParCluster_Id);
            ViewBag.ParModule_Id = new SelectList(db.ParModule, "Id", "Name", parClusterXModule.ParModule_Id);
            return View(parClusterXModule);
        }

        // POST: ParClusterXModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParCluster_Id,ParModule_Id,Points,AddDate,AlterDate,IsActive,EffectiveDate")] ParClusterXModule parClusterXModule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parClusterXModule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name", parClusterXModule.ParCluster_Id);
            ViewBag.ParModule_Id = new SelectList(db.ParModule, "Id", "Name", parClusterXModule.ParModule_Id);
            return View(parClusterXModule);
        }

        // GET: ParClusterXModules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterXModule parClusterXModule = await db.ParClusterXModule.FindAsync(id);
            if (parClusterXModule == null)
            {
                return HttpNotFound();
            }
            return View(parClusterXModule);
        }

        // POST: ParClusterXModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParClusterXModule parClusterXModule = await db.ParClusterXModule.FindAsync(id);
            db.ParClusterXModule.Remove(parClusterXModule);
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
    }
}

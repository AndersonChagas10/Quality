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
    public class ParCompany2XClusterController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParCompany2XCluster
        public async Task<ActionResult> Index()
        {
            var parCompanyXCluster = db.ParCompanyCluster.Include(p => p.ParCompany);
            return View(await parCompanyXCluster.ToListAsync());
        }

        // GET: ParCompany2XCluster/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompanyCluster parCompanyXCluster = await db.ParCompanyCluster.FindAsync(id);
            if (parCompanyXCluster == null)
            {
                return HttpNotFound();
            }
            return View(parCompanyXCluster);
        }

        // GET: ParCompany2XCluster/Create
        public ActionResult Create(int parCompanyId)
        {
            ViewBag.ParCompanyId = parCompanyId;
            var listlinkedCompany = db.ParCompanyCluster.Where(m => m.ParCompany_Id == parCompanyId && m.Active == true).Select(m=>m.ParCluster_Id).ToList();
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster.Where(m => !listlinkedCompany.Contains(m.Id) && m.Id != parCompanyId ).Select(m=>m).ToList(), "Id", "Name");
            return View(new ParCompanyCluster() { ParCompany_Id = parCompanyId });
        }

        // POST: ParCompany2XCluster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParCluster_Id,ParCompany_Id")] ParCompanyCluster parCompanyXCluster)
        {
            if (ModelState.IsValid)
            {
                parCompanyXCluster.Active = true;
                db.ParCompanyCluster.Add(parCompanyXCluster);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","ParCompany2",new { id = parCompanyXCluster.ParCompany_Id });
            }

            var listlinkedCompany = db.ParCompanyCluster.Where(m => m.ParCompany_Id == parCompanyXCluster.ParCompany_Id).ToList();
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster.Where(m => !listlinkedCompany.Any(u => u.ParCluster_Id == m.Id)), "Id", "Name", parCompanyXCluster.ParCompany_Id);
            return View(parCompanyXCluster);
        }

        // GET: ParCompany2XCluster/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompanyCluster parCompanyXCluster = await db.ParCompanyCluster.FindAsync(id);
            if (parCompanyXCluster == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name", parCompanyXCluster.ParCompany_Id);
            return View(parCompanyXCluster);
        }

        // POST: ParCompany2XCluster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParCluster_Id,ParCompany_Id")] ParCompanyCluster parCompanyXCluster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parCompanyXCluster).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "ParCompany2", new { id = parCompanyXCluster.ParCompany_Id });
            }
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name", parCompanyXCluster.ParCompany_Id);
            return View(parCompanyXCluster);
        }

        // GET: ParCompany2XCluster/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompanyCluster parCompanyXCluster = await db.ParCompanyCluster.FindAsync(id);
            if (parCompanyXCluster == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCompanyId = parCompanyXCluster.ParCompany_Id;
            return View(parCompanyXCluster);
        }

        // POST: ParCompany2XCluster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParCompanyCluster parCompanyXCluster = await db.ParCompanyCluster.FindAsync(id);
            parCompanyXCluster.Active = false;
            db.Entry(parCompanyXCluster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "ParCompany2", new { id = parCompanyXCluster.ParCompany_Id });
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

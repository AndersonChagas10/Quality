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
    public class ParCompany2XStructureController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParCompany2XStructure
        public async Task<ActionResult> Index()
        {
            var parCompanyXStructure = db.ParCompanyXStructure.Include(p => p.ParCompany);
            return View(await parCompanyXStructure.ToListAsync());
        }

        // GET: ParCompany2XStructure/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompanyXStructure parCompanyXStructure = await db.ParCompanyXStructure.FindAsync(id);
            if (parCompanyXStructure == null)
            {
                return HttpNotFound();
            }
            return View(parCompanyXStructure);
        }

        // GET: ParCompany2XStructure/Create
        public ActionResult Create(int parCompanyId)
        {
            ViewBag.ParCompanyId = parCompanyId;
            var listlinkedCompany = db.ParCompanyXStructure.Where(m=> m.ParCompany_Id == parCompanyId).Select(m=>m.ParCompany_Id).ToList();
            var listaDeEmpresasSalvas = db.ParCompanyXStructure.Where(x => x.ParCompany_Id == parCompanyId && x.Active == true).Select(m => m.ParStructure_Id).ToList();

            ViewBag.ParStructure_Id = new SelectList(db.ParStructure.Where(m=> !listaDeEmpresasSalvas.Contains(m.Id)).Select(m=>m).ToList(), "Id", "Name");

            return View(new ParCompanyXStructure() { ParCompany_Id = parCompanyId });
        }

        // POST: ParCompany2XStructure/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParStructure_Id,ParCompany_Id")] ParCompanyXStructure parCompanyXStructure)
        {
            ValidaVinculo(parCompanyXStructure);
            if (ModelState.IsValid)
            {
                parCompanyXStructure.Active = true;
                db.ParCompanyXStructure.Add(parCompanyXStructure);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","ParCompany2",new { id = parCompanyXStructure.ParCompany_Id });
            }

            //var listlinkedCompany = db.ParCompanyXStructure.Where(m => m.ParCompany_Id == parCompanyXStructure.ParCompany_Id).ToList();
            var listlinkedCompany = db.ParCompanyXStructure.Where(m => m.ParCompany_Id == parCompanyXStructure.ParCompany_Id).ToList();
            ViewBag.ParStructure_Id = new SelectList(db.ParStructure.Where(m => !listlinkedCompany.Any(u => u.ParCompany_Id == m.Id)), "Id", "Name", parCompanyXStructure.ParCompany_Id);
            return View(parCompanyXStructure);
        }

        private void ValidaVinculo(ParCompanyXStructure parCompanyXStructure)
        {
            var empresa = db.ParCompany.Where(x => x.Id == parCompanyXStructure.ParCompany_Id).FirstOrDefault();
            if(parCompanyXStructure.ParCompany_Id == empresa.Id)
                ModelState.AddModelError("ParCompany_Id", "ja existe um vinculo para esta empresa");
        }

        // GET: ParCompany2XStructure/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompanyXStructure parCompanyXStructure = await db.ParCompanyXStructure.FindAsync(id);
            if (parCompanyXStructure == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParStructure_Id = new SelectList(db.ParStructure, "Id", "Name", parCompanyXStructure.ParCompany_Id);
            return View(parCompanyXStructure);
        }

        // POST: ParCompany2XStructure/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParStructure_Id,ParCompany_Id")] ParCompanyXStructure parCompanyXStructure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parCompanyXStructure).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "ParCompany2", new { id = parCompanyXStructure.ParCompany_Id });
            }
            ViewBag.ParStructure_Id = new SelectList(db.ParStructure, "Id", "Name", parCompanyXStructure.ParCompany_Id);
            return View(parCompanyXStructure);
        }

        // GET: ParCompany2XStructure/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCompanyXStructure parCompanyXStructure = await db.ParCompanyXStructure.FindAsync(id);
            if (parCompanyXStructure == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParCompanyId = parCompanyXStructure.ParCompany_Id;
            return View(parCompanyXStructure);
        }

        // POST: ParCompany2XStructure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParCompanyXStructure parCompanyXStructure = await db.ParCompanyXStructure.FindAsync(id);
            parCompanyXStructure.Active = false;
            db.Entry(parCompanyXStructure).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "ParCompany2", new { id = parCompanyXStructure.ParCompany_Id });
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

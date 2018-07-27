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

namespace SgqSystem.Controllers.ImportFormat
{
    public class ImportFormatsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ImportFormats
        public async Task<ActionResult> Index()
        {
            return View(await db.ImportFormat.ToListAsync());
        }

        // GET: ImportFormats/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dominio.ImportFormat importFormat = await db.ImportFormat.FindAsync(id);
            if (importFormat == null)
            {
                return HttpNotFound();
            }
            return View(importFormat);
        }

        // GET: ImportFormats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImportFormats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,IsActive")] Dominio.ImportFormat importFormat)
        {
            importFormat.AddDate = DateTime.Now;
            importFormat.AlterDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.ImportFormat.Add(importFormat);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(importFormat);
        }

        // GET: ImportFormats/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dominio.ImportFormat importFormat = await db.ImportFormat.FindAsync(id);
            if (importFormat == null)
            {
                return HttpNotFound();
            }
            return View(importFormat);
        }

        // POST: ImportFormats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,IsActive")] Dominio.ImportFormat importFormat)
        {
            importFormat.AlterDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(importFormat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(importFormat);
        }

        // GET: ImportFormats/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dominio.ImportFormat importFormat = await db.ImportFormat.FindAsync(id);
            if (importFormat == null)
            {
                return HttpNotFound();
            }
            return View(importFormat);
        }

        // POST: ImportFormats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Dominio.ImportFormat importFormat = await db.ImportFormat.FindAsync(id);
            db.ImportFormat.Remove(importFormat);
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

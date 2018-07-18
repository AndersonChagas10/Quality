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
    public class ImportFormatItemsController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ImportFormatItems
        public async Task<ActionResult> Index()
        {
            return View(await db.ImportFormatItem.ToListAsync());
        }

        // GET: ImportFormatItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportFormatItem importFormatItem = await db.ImportFormatItem.FindAsync(id);
            if (importFormatItem == null)
            {
                return HttpNotFound();
            }
            return View(importFormatItem);
        }

        // GET: ImportFormatItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImportFormatItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Key,Value,AddDate,AlterDate,ImportFormat_Id")] ImportFormatItem importFormatItem)
        {
            if (ModelState.IsValid)
            {
                db.ImportFormatItem.Add(importFormatItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(importFormatItem);
        }

        // GET: ImportFormatItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportFormatItem importFormatItem = await db.ImportFormatItem.FindAsync(id);
            if (importFormatItem == null)
            {
                return HttpNotFound();
            }
            return View(importFormatItem);
        }

        // POST: ImportFormatItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Key,Value,AddDate,AlterDate,ImportFormat_Id")] ImportFormatItem importFormatItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(importFormatItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(importFormatItem);
        }

        // GET: ImportFormatItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportFormatItem importFormatItem = await db.ImportFormatItem.FindAsync(id);
            if (importFormatItem == null)
            {
                return HttpNotFound();
            }
            return View(importFormatItem);
        }

        // POST: ImportFormatItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ImportFormatItem importFormatItem = await db.ImportFormatItem.FindAsync(id);
            db.ImportFormatItem.Remove(importFormatItem);
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

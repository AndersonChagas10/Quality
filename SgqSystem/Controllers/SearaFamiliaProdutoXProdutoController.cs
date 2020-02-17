using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Dominio.Seara;
using PagedList;
using SgqSystem.ViewModels;

namespace SgqSystem.Controllers
{
    public class SearaFamiliaProdutoXProdutoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: SearaFamiliaProdutoXProduto
        public ActionResult Index()
        {
            return View(db.SearaFamiliaProdutoXProduto.ToList());
        }

        // GET: SearaFamiliaProdutoXProduto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaFamiliaProdutoXProduto searaFamiliaProdutoXProduto = db.SearaFamiliaProdutoXProduto.Find(id);
            if (searaFamiliaProdutoXProduto == null)
            {
                return HttpNotFound();
            }
            return View(searaFamiliaProdutoXProduto);
        }

        // GET: SearaFamiliaProdutoXProduto/Create
        public ActionResult Create(int id)
        {
            SearaFamiliaProdutoXProduto searaFamiliaProdutoXProduto = new SearaFamiliaProdutoXProduto();
            searaFamiliaProdutoXProduto.SearaFamiliaProduto_Id = id;

            ViewBag.SearaFamiliaProduto_Id = id;

            return View(searaFamiliaProdutoXProduto);
        }

        // POST: SearaFamiliaProdutoXProduto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SearaFamiliaProduto_Id,SearaProduto_Id,ParCompany_Id,IsActive,AddDate,AlterDate")] SearaFamiliaProdutoXProduto searaFamiliaProdutoXProduto)
        {
            if (ModelState.IsValid)
            {
                searaFamiliaProdutoXProduto.SearaFamiliaProduto_Id = searaFamiliaProdutoXProduto.Id;
                db.SearaFamiliaProdutoXProduto.Add(searaFamiliaProdutoXProduto);
                db.SaveChanges();
                return RedirectToAction("Details", "SearaFamiliaProduto", new { Id = searaFamiliaProdutoXProduto.SearaFamiliaProduto_Id });
            }

            return View(searaFamiliaProdutoXProduto);
        }

        // GET: SearaFamiliaProdutoXProduto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaFamiliaProdutoXProduto searaFamiliaProdutoXProduto = db.SearaFamiliaProdutoXProduto.Find(id);
            if (searaFamiliaProdutoXProduto == null)
            {
                return HttpNotFound();
            }
            return View(searaFamiliaProdutoXProduto);
        }

        // POST: SearaFamiliaProdutoXProduto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SearaFamiliaProduto_Id,SearaProduto_Id,ParCompany_Id,IsActive,AddDate,AlterDate")] SearaFamiliaProdutoXProduto searaFamiliaProdutoXProduto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searaFamiliaProdutoXProduto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(searaFamiliaProdutoXProduto);
        }

        // GET: SearaFamiliaProdutoXProduto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaFamiliaProdutoXProduto searaFamiliaProdutoXProduto = db.SearaFamiliaProdutoXProduto.Find(id);
            if (searaFamiliaProdutoXProduto == null)
            {
                return HttpNotFound();
            }
            return View(searaFamiliaProdutoXProduto);
        }

        // POST: SearaFamiliaProdutoXProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SearaFamiliaProdutoXProduto searaFamiliaProdutoXProduto = db.SearaFamiliaProdutoXProduto.Find(id);
            db.SearaFamiliaProdutoXProduto.Remove(searaFamiliaProdutoXProduto);
            db.SaveChanges();
            return RedirectToAction("Details", "SearaFamiliaProduto", new { Id = searaFamiliaProdutoXProduto.SearaFamiliaProduto_Id });
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

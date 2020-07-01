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
    public class ParFamiliaProdutoXParProdutoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParFamiliaProdutoXParProduto
        public ActionResult Index()
        {
            return View(db.ParFamiliaProdutoXParProduto.ToList());
        }

        // GET: ParFamiliaProdutoXParProduto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParFamiliaProdutoXParProduto parFamiliaProdutoXParProduto = db.ParFamiliaProdutoXParProduto.Find(id);
            if (parFamiliaProdutoXParProduto == null)
            {
                return HttpNotFound();
            }
            return View(parFamiliaProdutoXParProduto);
        }

        // GET: ParFamiliaProdutoXParProduto/Create
        public ActionResult Create(int id)
        {
            ParFamiliaProdutoXParProduto parFamiliaProdutoXParProduto = new ParFamiliaProdutoXParProduto();
            parFamiliaProdutoXParProduto.ParFamiliaProduto_Id = id;
            return View(parFamiliaProdutoXParProduto);
        }

        // POST: ParFamiliaProdutoXParProduto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParFamiliaProduto_Id,ParProduto_Id,ParCompany_Id,IsActive,AddDate,AlterDate")] ParFamiliaProdutoXParProduto parFamiliaProdutoXParProduto)
        {
            if (ModelState.IsValid)
            {
                var verificaSeJaExiste = db.ParFamiliaProdutoXParProduto.Where(x => x.ParProduto_Id == parFamiliaProdutoXParProduto.ParProduto_Id && x.ParFamiliaProduto_Id == parFamiliaProdutoXParProduto.ParFamiliaProduto_Id).ToList();
                if (verificaSeJaExiste.Count() > 0)
                {
                    ViewBag.VinculoJaExiste = "Já existe um vínculo com o mesmo produto cadastrado!";
                }
                else
                {
                    db.ParFamiliaProdutoXParProduto.Add(parFamiliaProdutoXParProduto);
                    db.SaveChanges();
                    return RedirectToAction("Details", "ParFamiliaProduto", new { Id = parFamiliaProdutoXParProduto.ParFamiliaProduto_Id });
                }
            }

            return View(parFamiliaProdutoXParProduto);
        }

        // GET: ParFamiliaProdutoXParProduto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParFamiliaProdutoXParProduto parFamiliaProdutoXParProduto = db.ParFamiliaProdutoXParProduto.Find(id);
            if (parFamiliaProdutoXParProduto == null)
            {
                return HttpNotFound();
            }
            MontaLista(parFamiliaProdutoXParProduto.ParProduto_Id);
            return View(parFamiliaProdutoXParProduto);
        }

        // POST: ParFamiliaProdutoXParProduto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParFamiliaProduto_Id,ParProduto_Id,ParCompany_Id,IsActive,AddDate,AlterDate")] ParFamiliaProdutoXParProduto parFamiliaProdutoXParProduto)
        {
            if (ModelState.IsValid)
            {
                var verificaSeJaExiste = db.ParFamiliaProdutoXParProduto.Where(x => x.ParProduto_Id == parFamiliaProdutoXParProduto.ParProduto_Id && x.ParFamiliaProduto_Id == parFamiliaProdutoXParProduto.ParFamiliaProduto_Id && x.Id != parFamiliaProdutoXParProduto.Id).ToList();
                if (verificaSeJaExiste.Count() > 0)
                {
                    ViewBag.VinculoJaExiste = "Já existe um vínculo com o mesmo produto cadastrado!";
                }
                else
                {
                    db.Entry(parFamiliaProdutoXParProduto).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "ParFamiliaProduto", new { Id = parFamiliaProdutoXParProduto.ParFamiliaProduto_Id });
                }
            }
            MontaLista(parFamiliaProdutoXParProduto.ParProduto_Id);
            return View(parFamiliaProdutoXParProduto);
        }

        // GET: ParFamiliaProdutoXParProduto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParFamiliaProdutoXParProduto parFamiliaProdutoXParProduto = db.ParFamiliaProdutoXParProduto.Find(id);
            if (parFamiliaProdutoXParProduto == null)
            {
                return HttpNotFound();
            }
            return View(parFamiliaProdutoXParProduto);
        }

        // POST: ParFamiliaProdutoXParProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParFamiliaProdutoXParProduto parFamiliaProdutoXParProduto = db.ParFamiliaProdutoXParProduto.Find(id);
            db.ParFamiliaProdutoXParProduto.Remove(parFamiliaProdutoXParProduto);
            db.SaveChanges();
            return RedirectToAction("Details", "ParFamiliaProduto", new { Id = parFamiliaProdutoXParProduto.ParFamiliaProduto_Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void MontaLista(int? id)
        {
            if (id != null)
            {
                ViewBag.ProdutoEdit = db.ParProduto.Where(x => x.Id == id).ToList()
              .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
              .ToList();
            }

            if (id == null || ViewBag.ProdutoEdit.Count == 0)
            {
                var semDados = new List<KeyValuePair<int?, string>>() {
                    new KeyValuePair<int?, string>(null, ""),
                    };
                ViewBag.ProdutoEdit = semDados;
            }

        }
    }
}

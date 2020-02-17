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
    public class SearaFamiliaProdutoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: SearaFamiliaProduto
        public ActionResult Index(int? page, string filtro = "")
        {
            List<SearaFamiliaProduto> searaFamiliaProduto = new List<SearaFamiliaProduto>();

            searaFamiliaProduto = db.SearaFamiliaProduto.OrderBy(x => x.Id).ToList();

            if (filtro != "")
            {
                searaFamiliaProduto = db.SearaFamiliaProduto.Where(x => x.Name.Contains(filtro)).OrderBy(x => x.Id).ToList();
                ViewBag.filtro = filtro;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(searaFamiliaProduto.ToPagedList(pageNumber, pageSize));
        }

        // GET: SearaFamiliaProduto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaFamiliaProduto searaFamiliaProduto = db.SearaFamiliaProduto.Find(id);
            if (searaFamiliaProduto == null)
            {
                return HttpNotFound();
            }

            searaFamiliaProduto.SearaFamiliaProdutoXProduto = db.SearaFamiliaProdutoXProduto.Where(x => x.SearaFamiliaProduto_Id == id && x.SearaFamiliaProduto_Id == searaFamiliaProduto.Id).ToList();

            return View(searaFamiliaProduto);
        }

        // GET: SearaFamiliaProduto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearaFamiliaProduto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ParCompany_Id,IsActive,AddDate,AlterDate")] SearaFamiliaProduto searaFamiliaProduto)
        {
            if (ModelState.IsValid)
            {
                db.SearaFamiliaProduto.Add(searaFamiliaProduto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(searaFamiliaProduto);
        }

        // GET: SearaFamiliaProduto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaFamiliaProduto searaFamiliaProduto = db.SearaFamiliaProduto.Find(id);
            if (searaFamiliaProduto == null)
            {
                return HttpNotFound();
            }

            MontaLista(searaFamiliaProduto.ParCompany);
            return View(searaFamiliaProduto);
        }

        // POST: SearaFamiliaProduto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParCompany_Id,IsActive,AddDate,AlterDate")] SearaFamiliaProduto searaFamiliaProduto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searaFamiliaProduto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(searaFamiliaProduto);
        }

        // GET: SearaFamiliaProduto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaFamiliaProduto searaFamiliaProduto = db.SearaFamiliaProduto.Find(id);
            if (searaFamiliaProduto == null)
            {
                return HttpNotFound();
            }
            return View(searaFamiliaProduto);
        }

        // POST: SearaFamiliaProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SearaFamiliaProduto searaFamiliaProduto = db.SearaFamiliaProduto.Find(id);
            db.SearaFamiliaProduto.Remove(searaFamiliaProduto);
            db.SaveChanges();
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

        private void MontaLista(ParCompany parCompany)
        {
            ViewBag.ParentsCreate = db.ParCompany.Where(x => x.Id == parCompany.Id).ToList()
           .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
           .ToList();

            if (ViewBag.ParentsCreate.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                    new KeyValuePair<int, string>(0, ""),
                    };
                ViewBag.ParentsCreate = semDados;
            }

        }
    }
}

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
    public class SearaProdutoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: SearaProduto
        public ActionResult Index(int? page, string filtro = "")
        {
            List<SearaProduto> searaProduto = new List<SearaProduto>();

            searaProduto = db.SearaProduto.OrderBy(x => x.Id).ToList();

            if (filtro != "")
            {
                searaProduto = db.SearaProduto.Where(x => x.Name.Contains(filtro)).OrderBy(x => x.Id).ToList();
                ViewBag.filtro = filtro;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(searaProduto.ToPagedList(pageNumber, pageSize));
        }

        // GET: SearaProduto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaProduto searaProduto = db.SearaProduto.Find(id);
            if (searaProduto == null)
            {
                return HttpNotFound();
            }
            return View(searaProduto);
        }

        // GET: SearaProduto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearaProduto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ParCompany_Id,IsActive,AddDate,AlterDate")] SearaProduto searaProduto)
        {
            if (ModelState.IsValid)
            {
                db.SearaProduto.Add(searaProduto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(searaProduto);
        }

        // GET: SearaProduto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaProduto searaProduto = db.SearaProduto.Find(id);
            if (searaProduto == null)
            {
                return HttpNotFound();
            }

            MontaLista(searaProduto.ParCompany);
            return View(searaProduto);
        }

        // POST: SearaProduto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParCompany_Id,IsActive,AddDate,AlterDate")] SearaProduto searaProduto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searaProduto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(searaProduto);
        }

        // GET: SearaProduto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearaProduto searaProduto = db.SearaProduto.Find(id);
            if (searaProduto == null)
            {
                return HttpNotFound();
            }
            return View(searaProduto);
        }

        // POST: SearaProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SearaProduto searaProduto = db.SearaProduto.Find(id);
            db.SearaProduto.Remove(searaProduto);
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

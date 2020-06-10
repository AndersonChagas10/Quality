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
    public class ParFamiliaProdutoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParFamiliaProduto
        public ActionResult Index(int? page, string filtro = "")
        {
            List<ParFamiliaProduto> parFamiliaProduto = new List<ParFamiliaProduto>();

            parFamiliaProduto = db.ParFamiliaProduto.OrderBy(x => x.Id).ToList();

            if (filtro != "")
            {
                parFamiliaProduto = db.ParFamiliaProduto.Where(x => x.Name.Contains(filtro)).OrderBy(x => x.Id).ToList();
                ViewBag.filtro = filtro;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(parFamiliaProduto.ToPagedList(pageNumber, pageSize));
        }

        // GET: ParFamiliaProduto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParFamiliaProduto parFamiliaProduto = db.ParFamiliaProduto.Find(id);
            if (parFamiliaProduto == null)
            {
                return HttpNotFound();
            }

            parFamiliaProduto.ParFamiliaProdutoXParProduto = db.ParFamiliaProdutoXParProduto.Where(x => x.ParFamiliaProduto_Id == id && x.ParFamiliaProduto_Id == parFamiliaProduto.Id).ToList();

            return View(parFamiliaProduto);
        }

        // GET: ParFamiliaProduto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParFamiliaProduto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ParCompany_Id,IsActive,AddDate,AlterDate")] ParFamiliaProduto parFamiliaProduto)
        {
            if (ModelState.IsValid)
            {
                var verificaSeJaExiste = db.ParFamiliaProduto.Where(x => x.Name == parFamiliaProduto.Name).ToList();
                if (verificaSeJaExiste.Count() > 0)
                {
                    ViewBag.NomeJaExiste = "Já existe um item com o mesmo nome cadastrado!";
                }
                else
                {
                    db.ParFamiliaProduto.Add(parFamiliaProduto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(parFamiliaProduto);
        }

        // GET: ParFamiliaProduto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParFamiliaProduto parFamiliaProduto = db.ParFamiliaProduto.Find(id);
            if (parFamiliaProduto == null)
            {
                return HttpNotFound();
            }

            MontaLista(parFamiliaProduto.ParCompany_Id);
            return View(parFamiliaProduto);
        }

        // POST: ParFamiliaProduto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParCompany_Id,IsActive,AddDate,AlterDate")] ParFamiliaProduto parFamiliaProduto)
        {
            if (ModelState.IsValid)
            {
                var verificaSeJaExiste = db.ParFamiliaProduto.Where(x => x.Name == parFamiliaProduto.Name && x.Id != parFamiliaProduto.Id).ToList();
                if (verificaSeJaExiste.Count() > 0)
                {
                    ViewBag.NomeJaExiste = "Já existe um item com o mesmo nome cadastrado!";
                }
                else
                {
                    db.Entry(parFamiliaProduto).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            MontaLista(parFamiliaProduto.ParCompany_Id);
            return View(parFamiliaProduto);
        }

        // GET: ParFamiliaProduto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParFamiliaProduto parFamiliaProduto = db.ParFamiliaProduto.Find(id);
            if (parFamiliaProduto == null)
            {
                return HttpNotFound();
            }
            return View(parFamiliaProduto);
        }

        // POST: ParFamiliaProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParFamiliaProduto parFamiliaProduto = db.ParFamiliaProduto.Find(id);
            db.ParFamiliaProduto.Remove(parFamiliaProduto);
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

        private void MontaLista(int? id)
        {
            if (id != null)
            {
                ViewBag.ParentsCreate = db.ParCompany.Where(x => x.Id == id).ToList()
              .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
              .ToList();
            }

            if (id == null || ViewBag.ParentsCreate.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                    new KeyValuePair<int, string>(0, ""),
                    };
                ViewBag.ParentsCreate = semDados;
            }

        }
    }
}

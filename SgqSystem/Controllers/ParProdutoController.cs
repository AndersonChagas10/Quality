﻿using System;
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
    public class ParProdutoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParProduto
        public ActionResult Index(int? page, string filtro = "")
        {
            List<ParProduto> parProduto = new List<ParProduto>();

            parProduto = db.ParProduto.OrderBy(x => x.Id).ToList();

            if (filtro != "")
            {
                parProduto = db.ParProduto.Where(x => x.Name.Contains(filtro)).OrderBy(x => x.Id).ToList();
                ViewBag.filtro = filtro;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(parProduto.ToPagedList(pageNumber, pageSize));
        }

        // GET: ParProduto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParProduto parProduto = db.ParProduto.Find(id);
            if (parProduto == null)
            {
                return HttpNotFound();
            }
            return View(parProduto);
        }

        // GET: ParProduto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParProduto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ParCompany_Id,IsActive,AddDate,AlterDate")] ParProduto parProduto)
        {
            if (ModelState.IsValid)
            {
                db.ParProduto.Add(parProduto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parProduto);
        }

        // GET: ParProduto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParProduto parProduto = db.ParProduto.Find(id);
            if (parProduto == null)
            {
                return HttpNotFound();
            }

            MontaLista(parProduto.ParCompany);
            return View(parProduto);
        }

        // POST: ParProduto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParCompany_Id,IsActive,AddDate,AlterDate")] ParProduto parProduto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parProduto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parProduto);
        }

        // GET: ParProduto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParProduto parProduto = db.ParProduto.Find(id);
            if (parProduto == null)
            {
                return HttpNotFound();
            }
            return View(parProduto);
        }

        // POST: ParProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParProduto parProduto = db.ParProduto.Find(id);
            db.ParProduto.Remove(parProduto);
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

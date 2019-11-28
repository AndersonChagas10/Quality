﻿using Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    public class ParCargoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public ActionResult Index()
        {

            return View(db.ParCargo.ToList());
        }

        // GET: ParCargo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCargo parCargo = db.ParCargo.Find(id);
            if (parCargo == null)
            {
                return HttpNotFound();
            }
            return View(parCargo);
        }

        public ActionResult Create()
        {
            ViewBag.Departamentos = db.ParDepartment.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate,ParDepartment_Ids")] ParCargo parCargo)
        {
            var exist = db.ParCargo.Any(x => x.Name == parCargo.Name);

            if (exist)
            {
                ModelState.AddModelError("Name", "Já existe um Cargo com este nome!");
            }

            if (ModelState.IsValid && exist == false)
            {
                parCargo.AddDate = DateTime.Now;
                db.ParCargo.Add(parCargo);

                if (parCargo.ParDepartment_Ids != null)
                {
                    foreach (var item in parCargo.ParDepartment_Ids)
                    {
                        db.ParCargoXDepartment.Add(new ParCargoXDepartment()
                        {
                            AddDate = DateTime.Now,
                            ParDepartment_Id = item,
                            ParCargo_Id = parCargo.Id,
                            IsActive = true
                        });
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parCargo);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ParCargo parCargo = db.ParCargo.Find(id);

            if (parCargo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Departamentos = db.ParDepartment.ToList();

            var parDepartment_Ids = db.ParCargoXDepartment.Where(x => x.ParCargo_Id == parCargo.Id && x.IsActive).Select(x => x.ParDepartment_Id);
            parCargo.ParDepartment_Ids = db.ParDepartment.Where(x => parDepartment_Ids.Contains(x.Id)).Select(x => x.Id).ToArray();


            ViewBag.Departments = db.ParDepartment.Where(x => parDepartment_Ids.Contains(x.Id)).ToList()
           .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
           .ToList();

            if (ViewBag.Departments.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(0, ""),

            };
                ViewBag.Departments = semDados;
            }

            return View(parCargo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate,ParDepartment_Ids")] ParCargo parCargo)
        {
            var exist = db.ParCargo.Any(x => x.Name == parCargo.Name && x.Id != parCargo.Id);

            if (exist)
            {
                ModelState.AddModelError("Name", "Já existe um cargo idêntico cadastrado.");
            } 

            if (ModelState.IsValid && exist == false)
            {
                parCargo.AlterDate = DateTime.Now;
                db.Entry(parCargo).State = EntityState.Modified;

                if (parCargo.ParDepartment_Ids != null)
                {
                    foreach (var item in parCargo.ParDepartment_Ids)//Insere novos
                    {
                        db.ParCargoXDepartment.Add(new ParCargoXDepartment()
                        {
                            AddDate = DateTime.Now,
                            ParDepartment_Id = item,
                            ParCargo_Id = parCargo.Id,
                            IsActive = true
                        });
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parCargo);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCargo parCargo = db.ParCargo.Find(id);
            if (parCargo == null)
            {
                return HttpNotFound();
            }
            return View(parCargo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParCargo parCargo = db.ParCargo.Find(id);
            db.ParCargo.Remove(parCargo);
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
    }
}

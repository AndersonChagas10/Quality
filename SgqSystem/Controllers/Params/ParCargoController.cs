using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;

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
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ParCargo parCargo = db.ParCargo.Find(id);
        //    if (parCargo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(parCargo);
        //}

        public ActionResult Create()
        {
            ViewBag.Departamentos = db.ParDepartment.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate,ParDepartment_Ids")] ParCargo parCargo)
        {
            if (ModelState.IsValid)
            {
                parCargo.AddDate = DateTime.Now;
                db.ParCargo.Add(parCargo);

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

            return View(parCargo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive,AddDate,AlterDate,ParDepartment_Ids")] ParCargo parCargo)
        {
            if (ModelState.IsValid)
            {
                parCargo.AlterDate = DateTime.Now;
                db.Entry(parCargo).State = EntityState.Modified;

                var parCargoXDepartments = db.ParCargoXDepartment.Where(x => x.ParCargo_Id == parCargo.Id && x.IsActive).ToList();

                foreach (var item in parCargoXDepartments)//inativa todos os inseridos
                {
                    item.AlterDate = DateTime.Now;
                    item.IsActive = false;
                }

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

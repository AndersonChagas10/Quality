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
    public class ParStructuresController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParStructures
        public ActionResult Index()
        {
            var parStructure = db.ParStructure.Include(p => p.ParStructureGroup).ToList();

            foreach (var item in parStructure)
            {
                if (item.ParStructureParent_Id > 0)
                    item.ParStructureParent = db.ParStructure.Find(item.ParStructureParent_Id);
            }

            return View(parStructure);
        }

        // GET: ParStructures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParStructure parStructure = db.ParStructure.Find(id);
            if (parStructure == null)
            {
                return HttpNotFound();
            }
            return View(parStructure);
        }

        // GET: ParStructures/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ParStructureGroup_Id = new SelectList(db.ParStructureGroup.Where(x => x.Active).OrderBy(x => x.Name), "Id", "Name");

            var listStrunct = db.ParStructure.Where(x => x.Active).OrderBy(x => x.Name).ToList();
            listStrunct.Insert(0, new ParStructure() { Id = 0, Name = Resources.Resource.select });

            ViewBag.ParStructureParentList = listStrunct;
            return View();
        }

        // POST: ParStructures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParStructureGroup_Id,Name,Description,ParStructureParent_Id,Active,AddDate,AlterDate")] ParStructure parStructure)
        {
            ValidarModel(parStructure);

            if (ModelState.IsValid)
            {
                parStructure.AddDate = DateTime.Now;
                db.ParStructure.Add(parStructure);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParStructureGroup_Id = new SelectList(db.ParStructureGroup.Where(x => x.Active).OrderBy(x => x.Name), "Id", "Name", parStructure.ParStructureGroup_Id);

            var listStrunct = db.ParStructure.Where(x => x.Active).OrderBy(x => x.Name).ToList();
            listStrunct.Insert(0, new ParStructure() { Id = 0, Name = Resources.Resource.select });

            ViewBag.ParStructureParentList = listStrunct;

            return View(parStructure);
        }

        // GET: ParStructures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParStructure parStructure = db.ParStructure.Find(id);
            if (parStructure == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParStructureGroup_Id = new SelectList(db.ParStructureGroup.Where(x => x.Active).OrderBy(x => x.Name), "Id", "Name", parStructure.ParStructureGroup_Id);
            var listStrunct = db.ParStructure.Where(x => x.Active && x.Id != id).OrderBy(x => x.Name).ToList();
            listStrunct.Insert(0, new ParStructure() { Id = 0, Name = Resources.Resource.select });
            ViewBag.ParStructureParentList = listStrunct;
            return View(parStructure);
        }

        // POST: ParStructures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParStructureGroup_Id,Name,Description,ParStructureParent_Id,Active,AddDate,AlterDate")] ParStructure parStructure)
        {
            ValidarModel(parStructure);

            if (ModelState.IsValid)
            {
                parStructure.AlterDate = DateTime.Now;
                db.Entry(parStructure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParStructureGroup_Id = new SelectList(db.ParStructureGroup.Where(x => x.Active).OrderBy(x => x.Name), "Id", "Name", parStructure.ParStructureGroup_Id);
            var listStrunct = db.ParStructure.Where(x => x.Active && x.Id != parStructure.Id).OrderBy(x => x.Name).ToList();
            listStrunct.Insert(0, new ParStructure() { Id = 0, Name = Resources.Resource.select });
            ViewBag.ParStructureParentList = listStrunct;
            return View(parStructure);
        }

        // GET: ParStructures/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ParStructure parStructure = db.ParStructure.Find(id);
        //    if (parStructure == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(parStructure);
        //}

        // POST: ParStructures/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ParStructure parStructure = db.ParStructure.Find(id);
        //    db.ParStructure.Remove(parStructure);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ValidarModel(ParStructure parStructure)
        {
            if (db.ParStructure.Any(x => x.Name == parStructure.Name && x.Id != parStructure.Id))
                ModelState.AddModelError("Name", "Já existe uma estrutura com este nome");
        }
    }
}

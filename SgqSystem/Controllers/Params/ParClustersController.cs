﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using SgqSystem.Secirity;
using Helper;
using System;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class ParClustersController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParClusters
        public ActionResult Index()
        {
            var parCluster = db.ParCluster.Include(p => p.ParClusterGroup);
            return View(parCluster.ToList());
        }

        // GET: ParClusters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCluster parCluster = db.ParCluster.Find(id);
            if (parCluster == null)
            {
                return HttpNotFound();
            }
            return View(parCluster);
        }

        // GET: ParClusters/Create
        public ActionResult Create()
        {
           // ViewBag.ParClusterGroup_Id = new SelectList(db.ParClusterGroup.Where(x => x.IsActive), "Id", "Name");

            var groupGlusterList = db.ParClusterGroup.Where(x => x.IsActive).ToList();
            groupGlusterList.Add(new ParClusterGroup() { Id = -1, Name = "Selecione" });
            ViewBag.ParClusterGroup_Id = new SelectList(groupGlusterList, "Id", "Name", -1);

            return View();
        }

        // POST: ParClusters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParClusterGroup_Id,Name,Description,ParClusterParent_Id,AddDate,AlterDate,IsActive")] ParCluster parCluster)
        {
            ValidModelState(parCluster);
            if (ModelState.IsValid)
            {
                parCluster.IsActive = true;
                db.ParCluster.Add(parCluster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var groupGlusterList = db.ParClusterGroup.Where(x => x.IsActive).ToList();
            groupGlusterList.Add(new ParClusterGroup() { Id = -1, Name = "Selecione" });
            ViewBag.ParClusterGroup_Id = new SelectList(groupGlusterList, "Id", "Name", -1);

            return View(parCluster);
        }

        private void ValidModelState(ParCluster parCluster)
        {

            ModelState.Clear();
            
            if (string.IsNullOrEmpty(parCluster.Name))
                ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);

            if (parCluster.ParClusterGroup_Id < 0 || parCluster.ParClusterGroup_Id == 0)
                ModelState.AddModelError("ParClusterGroup_Id", Resources.Resource.required_field + " " + Resources.Resource.cluster_group);

            if (string.IsNullOrEmpty(parCluster.Description))
                ModelState.AddModelError("Description", Resources.Resource.required_field + " " + Resources.Resource.description);

            if (!parCluster.ParClusterParent_Id.HasValue)
                ModelState.AddModelError("ParClusterParent_Id", Resources.Resource.required_field + " " + Resources.Resource.cluster_group);
        }

        // GET: ParClusters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCluster parCluster = db.ParCluster.Find(id);
            if (parCluster == null)
            {
                return HttpNotFound();
            }
  
            var groupGlusterList = db.ParClusterGroup.Where(x => x.IsActive).ToList();
            groupGlusterList.Add(new ParClusterGroup() { Id = -1, Name = "Selecione" });
            ViewBag.ParClusterGroup_Id = new SelectList(groupGlusterList, "Id", "Name", parCluster.ParClusterGroup_Id);

            return View(parCluster);
        }

        // POST: ParClusters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParClusterGroup_Id,Name,Description,ParClusterParent_Id,AddDate,AlterDate,IsActive")] ParCluster parCluster)
        {
            ValidModelState(parCluster);
            if (ModelState.IsValid)
            {
                db.Entry(parCluster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var groupGlusterList = db.ParClusterGroup.Where(x => x.IsActive).ToList();
            groupGlusterList.Add(new ParClusterGroup() { Id = -1, Name = "Selecione" });
            ViewBag.ParClusterGroup_Id = new SelectList(groupGlusterList, "Id", "Name", parCluster.ParClusterGroup_Id);

            return View(parCluster);
        }

        // GET: ParClusters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParCluster parCluster = db.ParCluster.Find(id);
            if (parCluster == null)
            {
                return HttpNotFound();
            }
            return View(parCluster);
        }

        // POST: ParClusters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParCluster parCluster = db.ParCluster.Find(id);
            db.ParCluster.Remove(parCluster);
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

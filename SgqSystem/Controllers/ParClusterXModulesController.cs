﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;
using AutoMapper;
using DTO.DTO;
using System.Text.RegularExpressions;

namespace SgqSystem.Controllers
{
    public class ParClusterXModulesController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParClusterXModules
        public async Task<ActionResult> Index()
        {
            var parClusterXModule = db.ParClusterXModule.Include(p => p.ParCluster).Include(p => p.ParModule);
            return View(await parClusterXModule.ToListAsync());
        }

        // GET: ParClusterXModules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterXModule parClusterXModule = await db.ParClusterXModule.FindAsync(id);
            if (parClusterXModule == null)
            {
                return HttpNotFound();
            }
            return View(parClusterXModule);
        }

        // GET: ParClusterXModules/Create
        public ActionResult Create()
        {
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name");
            ViewBag.ParModule_Id = new SelectList(db.ParModule, "Id", "Name");
            return View();
        }

        // POST: ParClusterXModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParCluster_Id,ParModule_Id,Points,AddDate,AlterDate,IsActive,EffectiveDate")] ParClusterXModulesDTO parClusterXModuleDTO)
        {
            ValidModelState(parClusterXModuleDTO);

            ParClusterXModule parClusterXModule = Mapper.Map<ParClusterXModule>(parClusterXModuleDTO);
            if (ModelState.IsValid)
            {
                parClusterXModule.AddDate = DateTime.Now;
                db.ParClusterXModule.Add(parClusterXModule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name", parClusterXModule.ParCluster_Id);
            ViewBag.ParModule_Id = new SelectList(db.ParModule, "Id", "Name", parClusterXModule.ParModule_Id);
            return View(parClusterXModuleDTO);
        }

        // GET: ParClusterXModules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterXModule parClusterXModule = await db.ParClusterXModule.FindAsync(id);
            if (parClusterXModule == null)
            {
                return HttpNotFound();
            }
            ParClusterXModulesDTO parClusterXModuleDTO = Mapper.Map<ParClusterXModulesDTO>(parClusterXModule);
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name", parClusterXModuleDTO.ParCluster_Id);
            ViewBag.ParModule_Id = new SelectList(db.ParModule, "Id", "Name", parClusterXModuleDTO.ParModule_Id);
                     
            return View(parClusterXModuleDTO);
        }

        // POST: ParClusterXModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParCluster_Id,ParModule_Id,Points,AddDate,AlterDate,IsActive,EffectiveDate")] ParClusterXModulesDTO parClusterXModuleDTO)
        {
            ValidModelState(parClusterXModuleDTO);
            ParClusterXModule parClusterXModule = Mapper.Map<ParClusterXModule>(parClusterXModuleDTO);
            if (ModelState.IsValid)
            {
                parClusterXModule.AlterDate = DateTime.Now;
                db.Entry(parClusterXModule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParCluster_Id = new SelectList(db.ParCluster, "Id", "Name", parClusterXModuleDTO.ParCluster_Id);
            ViewBag.ParModule_Id = new SelectList(db.ParModule, "Id", "Name", parClusterXModuleDTO.ParModule_Id);
            return View(parClusterXModuleDTO);
        }

        // GET: ParClusterXModules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParClusterXModule parClusterXModule = await db.ParClusterXModule.FindAsync(id);
            if (parClusterXModule == null)
            {
                return HttpNotFound();
            }
            return View(parClusterXModule);
        }

        // POST: ParClusterXModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParClusterXModule parClusterXModule = await db.ParClusterXModule.FindAsync(id);
            db.ParClusterXModule.Remove(parClusterXModule);
            await db.SaveChangesAsync();
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

        private void ValidModelState(ParClusterXModulesDTO parClusterXModule)
        {
            //ParClusterXModulesDTO testeDTO = Mapper.Map<ParClusterXModule>(parClusterXModule);
           
            ModelState.Clear();

            if (parClusterXModule.EffectiveDate == null || parClusterXModule.EffectiveDate == DateTime.MinValue)
                ModelState.AddModelError("EffectiveDate", Resources.Resource.required_field + " " + Resources.Resource.effectiveDate);

            if (parClusterXModule.Points <= 0 || parClusterXModule.Points == null)
                ModelState.AddModelError("Points", Resources.Resource.required_field + " " + Resources.Resource.points);

            if (parClusterXModule.ParCluster_Id <= 0)
                ModelState.AddModelError("ParCluster_Id", Resources.Resource.required_field + " " + Resources.Resource.cluster);

            if (parClusterXModule.ParModule_Id <= 0)
                ModelState.AddModelError("ParModule_Id", Resources.Resource.required_field + " " + Resources.Resource.module);
        }
    }
}

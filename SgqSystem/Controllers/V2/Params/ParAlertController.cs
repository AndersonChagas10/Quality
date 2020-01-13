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
    public class ParAlertController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public void FillViewBag(ParAlert parAlert = null)
        {
            if(parAlert == null)
            {
                parAlert = new ParAlert();
            }
            //var parDepartments = db.ParDepartment.Where(x => x.Active).ToList();
            //parDepartments.Insert(0, new ParDepartment() { Id = 0, Name = Resources.Resource.all });
            //ViewBag.ParDepartment_Id = new SelectList(parDepartments, "Id", "Name",parAlert.ParDepartment_Id);

            var parCargos = db.ParCargo.Where(x => x.IsActive).ToList();
            parCargos.Insert(0, new ParCargo() { Id = 0, Name = Resources.Resource.all });
            ViewBag.ParCargo_Id = new SelectList(parCargos, "Id", "Name", parAlert.ParCargo_Ids);

            var parLevel1s = db.ParLevel1.Where(x => x.IsActive).ToList();
            parLevel1s.Insert(0, new ParLevel1() { Id = 0, Name = Resources.Resource.all });
            ViewBag.ParLevel1_Id = new SelectList(parLevel1s, "Id", "Name", parAlert.ParLevel1_Id);

            var parLevel2s = db.ParLevel2.Where(x => x.IsActive).ToList();
            parLevel2s.Insert(0, new ParLevel2() { Id = 0, Name = Resources.Resource.all });
            ViewBag.ParLevel2_Id = new SelectList(parLevel2s, "Id", "Name", parAlert.ParLevel2_Id);

            var parLevel3s = db.ParLevel3.Where(x => x.IsActive).ToList();
            parLevel3s.Insert(0, new ParLevel3() { Id = 0, Name = Resources.Resource.all });
            ViewBag.ParLevel3_Id = new SelectList(parLevel3s, "Id", "Name", parAlert.ParLevel3_Id);

            var parCompanys = db.ParCompany.Where(x => x.IsActive).ToList();
            parCompanys.Insert(0, new ParCompany() { Id = 0, Name = Resources.Resource.all });
            ViewBag.ParCompany_Id = new SelectList(parCompanys, "Id", "Name", parAlert.ParCompany_Ids);

            var parAlertTypes = db.ParAlertType.Where(x => x.IsActive).ToList();
            ViewBag.ParAlertType_Id = new SelectList(parAlertTypes, "Id", "Name", parAlert.ParAlertType_Id);


            ViewBag.Department = db.ParDepartment.Where(x => x.Id == parAlert.ParSecao_Ids).ToList()
             .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
             .ToList();

            if (ViewBag.Department.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(0, ""),

            };
                ViewBag.Department = semDados;
            }

            ViewBag.SonDepartments = db.ParDepartment.Where(x => x.Id == parAlert.ParSecao_Ids).ToList()
             .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
             .ToList();

            if (ViewBag.SonDepartments.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(0, ""),

            };
                ViewBag.SonDepartments = semDados;
            }

            ViewBag.Company = db.ParCompany.Where(x => x.Id == parAlert.ParCompany_Ids).ToList()
             .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
             .ToList();

            if (ViewBag.Company.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(0, ""),

            };
                ViewBag.Company = semDados;
            }

            ViewBag.Cargo = db.ParCargo.Where(x => x.Id == parAlert.ParCargo_Ids).ToList()
            .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
            .ToList();

            if (ViewBag.Cargo.Count == 0)
            {
                var semDados = new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(0, ""),

            };
                ViewBag.Cargo = semDados;
            }
        }

        // GET: ParAlerts
        public async Task<ActionResult> Index()
        {
            var parAlert = db.ParAlert
                .Include(p => p.ParDepartment)
                .Include(p => p.ParCargo)
                .Include(p => p.ParLevel1)
                .Include(p => p.ParLevel2)
                .Include(p => p.ParLevel3)
                .Include(p => p.ParCompany)
                .Include(p => p.ParAlertType);
            return View(await parAlert.ToListAsync());
        }

        // GET: ParAlerts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParAlert parAlert = await db.ParAlert.FindAsync(id);
            if (parAlert == null)
            {
                return HttpNotFound();
            }
            return View(parAlert);
        }

        // GET: ParAlerts/Create
        public ActionResult Create()
        {
            FillViewBag();
            return View(new ParAlert());
        }

        // POST: ParAlerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,ParSecao_Ids,ParCargo_Ids,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id,ParCompany_Ids,ParAlertType_Id,IsCollectAlert,HasCorrectiveAction,IsActive")] ParAlert parAlert)
        {
            SetValues(parAlert);
            ValidarAlerta(parAlert);

            if (ModelState.IsValid)
            {
                parAlert.AddDate = DateTime.Now;
                db.ParAlert.Add(parAlert);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            FillViewBag(parAlert);
            return View(parAlert);
        }

        // GET: ParAlerts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParAlert parAlert = await db.ParAlert.FindAsync(id);
            if (parAlert == null)
            {
                return HttpNotFound();
            }
            FillViewBag(parAlert);

            return View(parAlert);
        }

        // POST: ParAlerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ParSecao_Ids,ParCargo_Ids,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id,ParCompany_Ids,ParAlertType_Id,IsCollectAlert,HasCorrectiveAction,IsActive")] ParAlert parAlert)
        {

            SetValues(parAlert);

            ValidarAlerta(parAlert);

            if (ModelState.IsValid)
            {
                parAlert.AlterDate = DateTime.Now;
                db.Entry(parAlert).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            FillViewBag(parAlert);
            return View(parAlert);
        }

        // GET: ParAlerts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParAlert parAlert = await db.ParAlert.FindAsync(id);
            if (parAlert == null)
            {
                return HttpNotFound();
            }
            return View(parAlert);
        }

        // POST: ParAlerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParAlert parAlert = await db.ParAlert.FindAsync(id);
            db.ParAlert.Remove(parAlert);
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

        private void SetValues(ParAlert parAlert)
        {
            parAlert.ParSecao_Ids = parAlert.ParSecao_Ids == 0 ? null : parAlert.ParSecao_Ids;
            parAlert.ParCompany_Ids = parAlert.ParCompany_Ids == 0 ? null : parAlert.ParCompany_Ids;
            parAlert.ParCargo_Ids = parAlert.ParCargo_Ids == 0 ? null : parAlert.ParCargo_Ids;
            parAlert.ParLevel1_Id = parAlert.ParLevel1_Id == 0 ? null : parAlert.ParLevel1_Id;
            parAlert.ParLevel2_Id = parAlert.ParLevel2_Id == 0 ? null : parAlert.ParLevel2_Id;
            parAlert.ParLevel3_Id = parAlert.ParLevel3_Id == 0 ? null : parAlert.ParLevel3_Id;
        }

        private void ValidarAlerta(ParAlert parAlert)
        {
            if(parAlert.ParLevel3_Id == null || parAlert.ParLevel3_Id == 0)
            {
                ModelState.AddModelError("ParLevel3_Id", Resources.Resource.select_the_level3);
            }
        }
    }
}

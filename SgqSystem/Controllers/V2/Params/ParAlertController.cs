using System;
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
            var parDepartments = db.ParDepartment.Where(x => x.Active).ToList();
            parDepartments.Insert(0, new ParDepartment() { Id = 0, Name = Resources.Resource.all });
            ViewBag.ParDepartment_Id = new SelectList(parDepartments, "Id", "Name",parAlert.ParDepartment_Id);

            var parCargos = db.ParCargo.Where(x => x.IsActive).ToList();
            parCargos.Insert(0, new ParCargo() { Id = 0, Name = Resources.Resource.all });
            ViewBag.ParCargo_Id = new SelectList(parCargos, "Id", "Name", parAlert.ParCargo_Id);

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
            ViewBag.ParCompany_Id = new SelectList(parCompanys, "Id", "Name", parAlert.ParCompany_Id);

            var parAlertTypes = db.ParAlertType.Where(x => x.IsActive).ToList();
            ViewBag.ParAlertType_Id = new SelectList(parAlertTypes, "Id", "Name", parAlert.ParAlertType_Id);
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,ParDepartment_Id,ParCargo_Id,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id,ParCompany_Id,ParAlertType_Id,IsCollectAlert,HasCorrectiveAction,IsActive")] ParAlert parAlert)
        {
            SetValues(parAlert);
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ParDepartment_Id,ParCargo_Id,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id,ParCompany_Id,ParAlertType_Id,IsCollectAlert,HasCorrectiveAction,IsActive")] ParAlert parAlert)
        {
            SetValues(parAlert);
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
            parAlert.ParDepartment_Id = parAlert.ParDepartment_Id == 0 ? null : parAlert.ParDepartment_Id;
            parAlert.ParCargo_Id = parAlert.ParCargo_Id == 0 ? null : parAlert.ParCargo_Id;
            parAlert.ParLevel1_Id = parAlert.ParLevel1_Id == 0 ? null : parAlert.ParLevel1_Id;
            parAlert.ParLevel2_Id = parAlert.ParLevel2_Id == 0 ? null : parAlert.ParLevel2_Id;
            parAlert.ParLevel3_Id = parAlert.ParLevel3_Id == 0 ? null : parAlert.ParLevel3_Id;
        }
    }
}

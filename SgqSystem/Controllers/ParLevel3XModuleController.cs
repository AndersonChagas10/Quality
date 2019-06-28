using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;

namespace SgqSystem.Controllers
{
    public class ParLevel3XModuleController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParLevel3XModule
        public ActionResult Index()
        {          
            return View(db.ParLevel3XModule.ToList());
        }

        // GET: ParLevel3XModule/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel3XModule parLevel3XModule = db.ParLevel3XModule.Find(id);
            if (parLevel3XModule == null)
            {
                return HttpNotFound();
            }
            return View(parLevel3XModule);
        }

        // GET: ParLevel3XModule/Create
        public ActionResult Create()
        {
            var parCompanyList = db.ParCompany.Where(x => x.IsActive).ToList();
            parCompanyList.Insert(0, new ParCompany() { Id = 0, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(parCompanyList, "Id", "Name");

            var parDepartmentList = db.ParDepartment.Where(x => x.Active).ToList();
            parDepartmentList.Insert(0, new ParDepartment() { Id = 0, Name = "Selecione" });
            ViewBag.ParDepartment_Id = new SelectList(parDepartmentList, "Id", "Name");

            var parLevel1List = db.ParLevel1.Where(x => x.IsActive).ToList();
            parLevel1List.Insert(0, new ParLevel1() { Id = 0, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(parLevel1List, "Id", "Name");

            var parLevel2List = db.ParLevel2.Where(x => x.IsActive).ToList();
            parLevel2List.Insert(0, new ParLevel2() { Id = 0, Name = "Selecione" });
            ViewBag.ParLevel2_Id = new SelectList(parLevel2List, "Id", "Name");

            var parLevel3List = db.ParLevel3.Where(x => x.IsActive).ToList();
            parLevel3List.Insert(0, new ParLevel3() { Id = 0, Name = "Selecione" });
            ViewBag.ParLevel3_Id = new SelectList(parLevel3List, "Id", "Name");

            var parModuleList = db.ParModule.Where(x => x.IsActive).ToList();
            parModuleList.Insert(0, new ParModule() { Id = 0, Name = "Selecione" });
            ViewBag.ParModule_Id = new SelectList(parModuleList, "Id", "Name");

            return View();
        }

        // POST: ParLevel3XModule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsActive,Points,EffectiveDateEnd,EffectiveDateStart,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id,ParCompany_Id,ParDepartment_Id,ParModule_Id,AddDate,AlterDate")] ParLevel3XModule parLevel3XModule)
        {
            if (ModelState.IsValid)
            {
                db.ParLevel3XModule.Add(parLevel3XModule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var parCompanyList = db.ParCompany.Where(x => x.IsActive).ToList();
            parCompanyList.Insert(0, new ParCompany() { Id = 0, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(parCompanyList, "Id", "Name");

            var parDepartmentList = db.ParDepartment.Where(x => x.Active).ToList();
            parDepartmentList.Insert(0, new ParDepartment() { Id = 0, Name = "Selecione" });
            ViewBag.ParDepartment_Id = new SelectList(parDepartmentList, "Id", "Name");

            var parLevel1List = db.ParLevel1.Where(x => x.IsActive).ToList();
            parLevel1List.Insert(0, new ParLevel1() { Id = 0, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(parLevel1List, "Id", "Name");

            var parLevel2List = db.ParLevel2.Where(x => x.IsActive).ToList();
            parLevel2List.Insert(0, new ParLevel2() { Id = 0, Name = "Selecione" });
            ViewBag.ParLevel2_Id = new SelectList(parLevel2List, "Id", "Name");

            var parLevel3List = db.ParLevel3.Where(x => x.IsActive).ToList();
            parLevel3List.Insert(0, new ParLevel3() { Id = 0, Name = "Selecione" });
            ViewBag.ParLevel3_Id = new SelectList(parLevel3List, "Id", "Name");

            var parModuleList = db.ParModule.Where(x => x.IsActive).ToList();
            parModuleList.Insert(0, new ParModule() { Id = 0, Name = "Selecione" });
            ViewBag.ParModule_Id = new SelectList(parModuleList, "Id", "Name");
            return View(parLevel3XModule);
        }

        // GET: ParLevel3XModule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel3XModule parLevel3XModule = db.ParLevel3XModule.Find(id);
            if (parLevel3XModule == null)
            {
                return HttpNotFound();
            }

            var parCompanyList = db.ParCompany.Where(x => x.IsActive).ToList();
            parCompanyList.Insert(0, new ParCompany() { Id = -1, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(parCompanyList, "Id", "Name", parLevel3XModule.ParCompany_Id);

            var parDepartmentList = db.ParDepartment.Where(x => x.Active).ToList();
            parDepartmentList.Insert(0, new ParDepartment() { Id = -1, Name = "Selecione" });
            ViewBag.ParDepartment_Id = new SelectList(parDepartmentList, "Id", "Name", parLevel3XModule.ParDepartment_Id);

            var parLevel1List = db.ParLevel1.Where(x => x.IsActive).ToList();
            parLevel1List.Insert(0, new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(parLevel1List, "Id", "Name", parLevel3XModule.ParLevel1_Id);

            var parLevel2List = db.ParLevel2.Where(x => x.IsActive).ToList();
            parLevel2List.Insert(0, new ParLevel2() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel2_Id = new SelectList(parLevel2List, "Id", "Name", parLevel3XModule.ParLevel2_Id);

            var parLevel3List = db.ParLevel3.Where(x => x.IsActive).ToList();
            parLevel3List.Insert(0, new ParLevel3() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel3_Id = new SelectList(parLevel3List, "Id", "Name", parLevel3XModule.ParLevel3_Id);

            var parModuleList = db.ParModule.Where(x => x.IsActive).ToList();
            parModuleList.Insert(0, new ParModule() { Id = -1, Name = "Selecione" });
            ViewBag.ParModule_Id = new SelectList(parModuleList, "Id", "Name", parLevel3XModule.ParModule_Id);

            return View(parLevel3XModule);
        }

        // POST: ParLevel3XModule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive,Points,EffectiveDateEnd,EffectiveDateStart,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id,ParCompany_Id,ParDepartment_Id,ParModule_Id,AddDate,AlterDate")] ParLevel3XModule parLevel3XModule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parLevel3XModule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var parCompanyList = db.ParCompany.Where(x => x.IsActive).ToList();
            parCompanyList.Insert(0, new ParCompany() { Id = -1, Name = "Selecione" });
            ViewBag.ParCompany_Id = new SelectList(parCompanyList, "Id", "Name", parLevel3XModule.ParCompany_Id);

            var parDepartmentList = db.ParDepartment.Where(x => x.Active).ToList();
            parDepartmentList.Insert(0, new ParDepartment() { Id = -1, Name = "Selecione" });
            ViewBag.ParDepartment_Id = new SelectList(parDepartmentList, "Id", "Name", parLevel3XModule.ParDepartment_Id);

            var parLevel1List = db.ParLevel1.Where(x => x.IsActive).ToList();
            parLevel1List.Insert(0, new ParLevel1() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel1_Id = new SelectList(parLevel1List, "Id", "Name", parLevel3XModule.ParLevel1_Id);

            var parLevel2List = db.ParLevel2.Where(x => x.IsActive).ToList();
            parLevel2List.Insert(0, new ParLevel2() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel2_Id = new SelectList(parLevel2List, "Id", "Name", parLevel3XModule.ParLevel2_Id);

            var parLevel3List = db.ParLevel3.Where(x => x.IsActive).ToList();
            parLevel3List.Insert(0, new ParLevel3() { Id = -1, Name = "Selecione" });
            ViewBag.ParLevel3_Id = new SelectList(parLevel3List, "Id", "Name", parLevel3XModule.ParLevel3_Id);

            var parModuleList = db.ParModule.Where(x => x.IsActive).ToList();
            parModuleList.Insert(0, new ParModule() { Id = -1, Name = "Selecione" });
            ViewBag.ParModule_Id = new SelectList(parModuleList, "Id", "Name", parLevel3XModule.ParModule_Id);
            return View(parLevel3XModule);
        }

        // GET: ParLevel3XModule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel3XModule parLevel3XModule = db.ParLevel3XModule.Find(id);
            if (parLevel3XModule == null)
            {
                return HttpNotFound();
            }
            return View(parLevel3XModule);
        }

        // POST: ParLevel3XModule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParLevel3XModule parLevel3XModule = db.ParLevel3XModule.Find(id);
            db.ParLevel3XModule.Remove(parLevel3XModule);
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

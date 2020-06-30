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
    public class PargroupQualificationXParQualificationsController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: PargroupQualificationXParQualifications
        public ActionResult Index()
        {
            var pargroupQualificationXParQualification = db.PargroupQualificationXParQualification.Include(p => p.ParQualification);
            return View(pargroupQualificationXParQualification.ToList());
        }

        // GET: PargroupQualificationXParQualifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PargroupQualificationXParQualification pargroupQualificationXParQualification = db.PargroupQualificationXParQualification.Find(id);
            if (pargroupQualificationXParQualification == null)
            {
                return HttpNotFound();
            }
            return View(pargroupQualificationXParQualification);
        }

        // GET: PargroupQualificationXParQualifications/Create
        public ActionResult Create(int id)
        {
            PargroupQualificationXParQualification pargroupQualificationXParQualification = new PargroupQualificationXParQualification();
            pargroupQualificationXParQualification.PargroupQualification_Id = id;

            ViewBag.PargroupQualification_Id = id;

            return View(pargroupQualificationXParQualification);
        }

        // POST: PargroupQualificationXParQualifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IsActive,PargroupQualification_Id,ParQualification_Id,AddDate,AlterDate")] PargroupQualificationXParQualification pargroupQualificationXParQualification)
        {
            if (ModelState.IsValid)
            {
                db.PargroupQualificationXParQualification.Add(pargroupQualificationXParQualification);
                db.SaveChanges();
                return RedirectToAction("Details", "PargroupQualification", new { Id = pargroupQualificationXParQualification.PargroupQualification_Id });
            }

            ViewBag.ParQualification_Id = new SelectList(db.ParQualification, "Id", "Name", pargroupQualificationXParQualification.ParQualification_Id);
            return View(pargroupQualificationXParQualification);
        }

        // GET: PargroupQualificationXParQualifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PargroupQualificationXParQualification pargroupQualificationXParQualification = db.PargroupQualificationXParQualification.Find(id);
            if (pargroupQualificationXParQualification == null)
            {
                return HttpNotFound();
            }
            MontaLista(pargroupQualificationXParQualification.ParQualification_Id);
            return View(pargroupQualificationXParQualification);
        }

        // POST: PargroupQualificationXParQualifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IsActive,PargroupQualification_Id,ParQualification_Id,AddDate,AlterDate")] PargroupQualificationXParQualification pargroupQualificationXParQualification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pargroupQualificationXParQualification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "PargroupQualification", new { Id = pargroupQualificationXParQualification.PargroupQualification_Id });
            }
            ViewBag.ParQualification_Id = new SelectList(db.ParQualification, "Id", "Name", pargroupQualificationXParQualification.ParQualification_Id);
            return View(pargroupQualificationXParQualification);
        }

        // GET: PargroupQualificationXParQualifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PargroupQualificationXParQualification pargroupQualificationXParQualification = db.PargroupQualificationXParQualification.Find(id);
            if (pargroupQualificationXParQualification == null)
            {
                return HttpNotFound();
            }
            return View(pargroupQualificationXParQualification);
        }

        // POST: PargroupQualificationXParQualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PargroupQualificationXParQualification pargroupQualificationXParQualification = db.PargroupQualificationXParQualification.Find(id);
            db.PargroupQualificationXParQualification.Remove(pargroupQualificationXParQualification);
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
            ViewBag.ParentsCreate = db.ParQualification.Where(x => x.Id == id).ToList()
           .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
           .ToList();

            if (ViewBag.ParentsCreate.Count == 0)
            {
                var semDados = new List<KeyValuePair<int?, string>>() {
                    new KeyValuePair<int?, string>(null, ""),
                    };
                ViewBag.ParentsCreate = semDados;
            }

        }
    }
}

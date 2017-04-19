using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using Helper;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class ParMeasurementUnitsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParMeasurementUnits
        public ActionResult Index()
        {
            return View(db.ParMeasurementUnit.ToList());
        }

        // GET: ParMeasurementUnits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParMeasurementUnit parMeasurementUnit = db.ParMeasurementUnit.Find(id);
            if (parMeasurementUnit == null)
            {
                return HttpNotFound();
            }
            return View(parMeasurementUnit);
        }

        // GET: ParMeasurementUnits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParMeasurementUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,AddDate,AlterDate,IsActive")] ParMeasurementUnit parMeasurementUnit)
        {
            if (ModelState.IsValid)
            {
                db.ParMeasurementUnit.Add(parMeasurementUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parMeasurementUnit);
        }

        // GET: ParMeasurementUnits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParMeasurementUnit parMeasurementUnit = db.ParMeasurementUnit.Find(id);
            if (parMeasurementUnit == null)
            {
                return HttpNotFound();
            }
            return View(parMeasurementUnit);
        }

        // POST: ParMeasurementUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,AddDate,AlterDate,IsActive")] ParMeasurementUnit parMeasurementUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parMeasurementUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parMeasurementUnit);
        }

        // GET: ParMeasurementUnits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParMeasurementUnit parMeasurementUnit = db.ParMeasurementUnit.Find(id);
            if (parMeasurementUnit == null)
            {
                return HttpNotFound();
            }
            return View(parMeasurementUnit);
        }

        // POST: ParMeasurementUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParMeasurementUnit parMeasurementUnit = db.ParMeasurementUnit.Find(id);
            db.ParMeasurementUnit.Remove(parMeasurementUnit);
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

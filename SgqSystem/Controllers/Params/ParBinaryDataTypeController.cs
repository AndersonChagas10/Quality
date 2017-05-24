using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using Helper;
using System;
using System.Dynamic;

namespace SgqSystem.Controllers.Params
{
    [CustomAuthorize]
    public class ParBinaryDataTypeController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _DataTypeFalse()
        {
            return PartialView(db.ParLevel3BoolFalse.ToList());
        }
        public ActionResult _DataTypeTrue()
        {
            return PartialView(db.ParLevel3BoolTrue.ToList());
        }

        public ActionResult CreateTrue()
        {
            return View();
        }
        public ActionResult CreateFalse()
        {
            return View();
        }

        public ActionResult EditTrue(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var parBinaryDataTypeTrue = db.ParLevel3BoolTrue.Find(id);

            if (parBinaryDataTypeTrue == null)
            {
                return HttpNotFound();
            }

            return View(parBinaryDataTypeTrue);

        }
        public ActionResult EditFalse(int? id)
        {
            ParLevel3BoolFalse parBinaryDataTypeFalse = new ParLevel3BoolFalse();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            parBinaryDataTypeFalse = db.ParLevel3BoolFalse.Find(id);
            if (parBinaryDataTypeFalse == null)
            {
                return HttpNotFound();
            }

            return View(parBinaryDataTypeFalse);
        }

        public ActionResult DeleteTrue(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var parBinaryDataTypeTrue = db.ParLevel3BoolTrue.Find(id);
            if (parBinaryDataTypeTrue == null)
            {
                return HttpNotFound();
            }

            return View(parBinaryDataTypeTrue);

        }
        public ActionResult DeleteFalse(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var parBinaryDataTypeFalse = db.ParLevel3BoolFalse.Find(id);
            if (parBinaryDataTypeFalse == null)
            {
                return HttpNotFound();
            }
            return View(parBinaryDataTypeFalse);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTrue(int id, ParLevel3BoolTrue parLevel3Bool)
        {
            var parBinaryDataTypeTrue = db.ParLevel3BoolTrue.Find(id);
            db.ParLevel3BoolTrue.Remove(parBinaryDataTypeTrue);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFalse(int id, ParLevel3BoolFalse parLevel3Bool)
        {

            ParLevel3BoolFalse parBinaryDataTypeFalse = new ParLevel3BoolFalse();

            parBinaryDataTypeFalse = db.ParLevel3BoolFalse.Find(id);
            db.ParLevel3BoolFalse.Remove(parBinaryDataTypeFalse);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTrue(ParLevel3BoolTrue parLevel3Bool)
        {

            if (ModelState.IsValid)
            {
                parLevel3Bool.AddDate = DateTime.Now;

                //Converter DTO para false
                db.ParLevel3BoolTrue.Add(parLevel3Bool);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parLevel3Bool);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFalse(ParLevel3BoolFalse parLevel3Bool)
        {
            if (ModelState.IsValid)
            {
                parLevel3Bool.AddDate = DateTime.Now;

                //Converter DTO para false
                db.ParLevel3BoolFalse.Add(parLevel3Bool);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(parLevel3Bool);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTrue(int? id, ParLevel3BoolTrue parLevel3Bool)
        {

            var original = db.ParLevel3BoolTrue.Find(id);

            if (original != null)
            {
                parLevel3Bool.AlterDate = DateTime.Now;
                parLevel3Bool.AddDate = original.AddDate;
                db.Entry(original).CurrentValues.SetValues(parLevel3Bool);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(parLevel3Bool);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFalse(int? id, ParLevel3BoolTrue parLevel3Bool)
        {

            var original = db.ParLevel3BoolFalse.Find(id);

            if (original != null)
            {
                parLevel3Bool.AlterDate = DateTime.Now;
                parLevel3Bool.AddDate = original.AddDate;
                db.Entry(original).CurrentValues.SetValues(parLevel3Bool);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(parLevel3Bool);
        }
        
    }   

}

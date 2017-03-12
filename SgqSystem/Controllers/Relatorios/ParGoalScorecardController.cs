using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;

namespace SgqSystem.Controllers.RelatoriosBrasil
{
    public class ParGoalScorecardController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParGoalScorecard
        public ActionResult Index()
        {
            return View(db.ParGoalScorecard.ToList());
        }

        // GET: ParGoalScorecard/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGoalScorecard parGoalScorecard = db.ParGoalScorecard.Find(id);
            if (parGoalScorecard == null)
            {
                return HttpNotFound();
            }
            return View(parGoalScorecard);
        }

        // GET: ParGoalScorecard/Create
        public ActionResult Create(int? id)
        {
            if (id > 0)
            {
                ParGoalScorecard parGoalScorecard = db.ParGoalScorecard.Find(id);
                if (parGoalScorecard == null)
                {
                    return HttpNotFound();
                }
                return View(parGoalScorecard);
            }

            return View(new ParGoalScorecard());
        }

        // POST: ParGoalScorecard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AddDate,AlterDate,IsActive,PercentValueMid,PercentValueHigh,InitDate")] ParGoalScorecard parGoalScorecard)
        {
            if (ModelState.IsValid)
            {
                if (parGoalScorecard.Id > 0)
                {
                    db.Entry(parGoalScorecard).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.ParGoalScorecard.Add(parGoalScorecard);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(parGoalScorecard);
        }


        // GET: ParGoalScorecard/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParGoalScorecard parGoalScorecard = db.ParGoalScorecard.Find(id);
            if (parGoalScorecard == null)
            {
                return HttpNotFound();
            }
            return View(parGoalScorecard);
        }

        // POST: ParGoalScorecard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParGoalScorecard parGoalScorecard = db.ParGoalScorecard.Find(id);
            db.ParGoalScorecard.Remove(parGoalScorecard);
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

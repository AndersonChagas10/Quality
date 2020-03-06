using Dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ParAlertXUserController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParAlertXUser
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int parAlertId)
        {
            ViewBag.ParAlertId = parAlertId;

            return View(new ParAlertXUser() { ParAlert_Id = parAlertId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParAlert_Id,ParCompany_Ids,UserSgq_Id,IsActive,AddDate,AlterDate")] ParAlertXUser parAlertXUser)
        {

            //buscar se ja existe um vinculo entre departamento e cargo, se sim bloquear a criação
            var parAlertXUserExistente = db.ParAlertXUser
                                                    .Where(x => x.ParAlert_Id == parAlertXUser.ParAlert_Id && x.UserSgq_Id == parAlertXUser.UserSgq_Id && x.IsActive)
                                                    .FirstOrDefault();

            if (parAlertXUserExistente != null)
            {
                ModelState.AddModelError("ParCompany_Ids", "Já existe um vinculo idêntico para este Alerta.");
            }

            ValidaVinculoAlertaxUsuario(parAlertXUser);

            if (ModelState.IsValid)
            {
                db.ParAlertXUser.Add(parAlertXUser);
                db.SaveChanges();
                return RedirectToAction("Details", "ParAlert", new { id = parAlertXUser.ParAlert_Id });
                //return RedirectToAction("Index");
            }

            ViewBag.ParAlertId = parAlertXUser.ParAlert_Id;
            //ViewBag.ParDepartment_Id = new SelectList(db.ParDepartment, "Id", "Name", parCargoXDepartment.ParDepartment_Id);
            return View(parAlertXUser);
        }

        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParAlertXUser parAlertXUser = db.ParAlertXUser.Find(id);
            if (parAlertXUser == null)
            {
                return HttpNotFound();
            }
          
            return View(parAlertXUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParAlertXUser parAlertXUser = db.ParAlertXUser.Find(id);
            db.Entry(parAlertXUser).State = EntityState.Modified;

            parAlertXUser.IsActive = false;
            db.SaveChanges();

            return RedirectToAction("Details", "ParAlert", new { id = parAlertXUser.ParAlert_Id });
        }

        private void ValidaVinculoAlertaxUsuario(ParAlertXUser parAlertXUser)
        {
            if (parAlertXUser.UserSgq_Id == 0)
                ModelState.AddModelError("UserSgq_Id", Resources.Resource.required_field + " " + "Usuário");
        }
    }
}
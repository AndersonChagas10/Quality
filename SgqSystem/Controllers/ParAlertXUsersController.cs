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
            //var listaDeCargosSalvos = db.ParCargoXDepartment.Where(x => x.ParCargo_Id == parCargoId && x.IsActive == true).Select(m => m.ParDepartment_Id).ToList();

           // ViewBag.ParDepartment_Id = db.ParDepartment.ToList()
           //.Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
           //.ToList();

           // if (ViewBag.ParDepartment_Id.Count == 0)
           // {
           //     var semDados = new List<KeyValuePair<int, string>>() {
           //     new KeyValuePair<int, string>(0, ""),

           // };
           //     ViewBag.ParDepartment_Id = semDados;
           // }

            //ViewBag.ParDepartment_Id = new SelectList(db.ParDepartment.Where(m => !listaDeCargosSalvos.Contains(m.Id) && m.Active).Select(m => m).ToList(), "Id", "Name");

            return View(new ParAlertXUser() { ParAlert_Id = parAlertId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParAlert_Id,ParCompany_Ids,UserSgq_Id,IsActive,AddDate,AlterDate")] ParAlertXUser parAlertXUser)
        {

            //buscar se ja existe um vinculo entre departamento e cargo, se sim bloquear a criação
            var parAlertXUserExistente = db.ParAlertXUser
                                                    .Where(x => x.ParCompany_Ids == parAlertXUser.ParCompany_Ids && x.UserSgq_Id == parAlertXUser.UserSgq_Id)
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
            if(parAlertXUser.ParCompany_Ids == 0)
                ModelState.AddModelError("ParCompany_Ids", Resources.Resource.required_field + " " + Resources.Resource.company);

            if (parAlertXUser.UserSgq_Id == 0)
                ModelState.AddModelError("UserSgq_Id", Resources.Resource.required_field + " " + "Usuário");
        }
    }
}
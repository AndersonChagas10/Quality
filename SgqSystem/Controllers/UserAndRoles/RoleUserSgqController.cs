using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.UserAndRoles
{
    public class RoleUserSgqController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: RoleUserSgq
        public ActionResult Index()
        {
            var roles = db.RoleUserSgq.ToList();

            return View(roles);
        }

        // GET: RoleUserSgq/Create
        public ActionResult Create()
        {

            //ViewBag.listaItensMenu = db.ItemMenu.ToList();

            return View();
        }

        // POST: RoleUserSgq/Create
        [HttpPost]
        public ActionResult Create(RoleUserSgqDTO regra)
        {
            try
            {
                //var roleUserSgq = Mapper.Map<RoleUserSgq>(regra);             

                //db.RoleUserSgq.Add(roleUserSgq);

                //var ListaModfy = db.RoleUserSgqXItemMenu.Where(r => r.RoleUserSgq_Id == regra.Id);

                //foreach (var item in ListaModfy)
                //{
                //    item.IsActive = false;
                //}

                //db.SaveChanges();

                //foreach (var item in regra.RoleUserSgqXItemMenuDTO)
                //{
                //    var RoleUserSgqXItemMenu = Mapper.Map<RoleUserSgqXItemMenu>(item);

                //    item.IsActive = true;
                //    item.RoleUserSgq_Id = regra.Id;

                //    db.RoleUserSgqXItemMenu.Add(RoleUserSgqXItemMenu);
                //}

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleUserSgq/Edit/5
        public ActionResult Edit(int id)
        {

            var roleToEdit = db.RoleUserSgq.Find(id);

            return View(roleToEdit);
        }

        // POST: RoleUserSgq/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleUserSgq/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoleUserSgq/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

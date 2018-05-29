using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.UserAndRoles
{
    public class RoleUserSgqController : BaseController
    {
        private SgqDbDevEntities db;

        public RoleUserSgqController()
        {
            db = new SgqDbDevEntities();
            ViewBag.listaItensMenu = db.ItemMenu.ToList();
        }

        

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

        // GET: RoleUserSgq/Edit/5
        public ActionResult Edit(int id)
        {
            var roleToEdit = Mapper.Map<RoleUserSgqDTO>(db.RoleUserSgq.Find(id));

            var roleUserSgqXItemMenu = db.RoleUserSgqXItemMenu.Where(r => r.RoleUserSgq_Id == roleToEdit.Id);

            foreach (var item in roleUserSgqXItemMenu)
            {
                roleToEdit.RoleUserSgqXItemMenuDTO.Add(Mapper.Map<RoleUserSgqXItemMenuDTO>(item));
            }

            //ViewBag.listaItensMenu = db.ItemMenu.ToList();

            return View("Create", roleToEdit);
        }

        // POST: RoleUserSgq/Create
        [HttpPost]
        public ActionResult Create(RoleUserSgqDTO regra)
        {
            try
            {
                ValidaUserSgqDto(regra);

                if (!ModelState.IsValid)
                    return View("Create", regra);

                var roleUserSgq = Mapper.Map<RoleUserSgq>(regra);

                db.RoleUserSgq.Add(roleUserSgq);
                db.SaveChanges();

                SaveRoleSgqXItemMenu(regra);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }


        // POST: RoleUserSgq/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, RoleUserSgqDTO regra)
        {
            try
            {
                ValidaUserSgqDto(regra);

                if (!ModelState.IsValid)
                    return View("Create", regra);

                var roleUserSgq = Mapper.Map<RoleUserSgq>(regra);

                db.RoleUserSgq.AddOrUpdate(roleUserSgq);
                db.SaveChanges();

                SaveRoleSgqXItemMenu(regra);

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

        private void SaveRoleSgqXItemMenu(RoleUserSgqDTO regra)
        {
            var ListaModfy = db.RoleUserSgqXItemMenu.Where(r => r.RoleUserSgq_Id == regra.Id).ToList();

            foreach (var item in ListaModfy)
            {
                item.IsActive = false;
            }

            db.SaveChanges();

            if (regra.RoleUserSgqXItemMenuDTO != null)
            {
                foreach (var item in regra.RoleUserSgqXItemMenuDTO)
                {
                    var RoleUserSgqXItemMenu = Mapper.Map<RoleUserSgqXItemMenu>(item);

                    item.IsActive = true;
                    item.RoleUserSgq_Id = regra.Id;

                    db.RoleUserSgqXItemMenu.Add(RoleUserSgqXItemMenu);
                }
            }
        }

        private void ValidaUserSgqDto(RoleUserSgqDTO regra)
        {
            if (string.IsNullOrEmpty(regra.Name))
            {
                ModelState.AddModelError("Name", Resources.Resource.name_can_not_be_empty);
            }

            if (regra.Id > 0)
            {
                if (db.RoleUserSgq.Where(r => r.Name == regra.Name && r.Id != regra.Id).ToList().Count() > 0)
                    ModelState.AddModelError("Name", Resources.Resource.repeated_username);
            }
            else if (db.RoleUserSgq.Where(r => r.Name == regra.Name).ToList().Count() > 0)
            {
                ModelState.AddModelError("Name", Resources.Resource.repeated_username);
            }
        }
    }
}

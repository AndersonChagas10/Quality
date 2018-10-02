using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ItemMenuController : BaseController
    {
        private SgqDbDevEntities db;

        public ItemMenuController()
        {
            db = new SgqDbDevEntities();
        }

        // GET: ItemMenu
        public ActionResult Index()
        {
            var ItensMenu = db.ItemMenu.ToList();

            var ItensMenuDTO = new List<ItemMenuDTO>();

            foreach (var item in ItensMenu)
            {
                ItensMenuDTO.Add(Mapper.Map<ItemMenuDTO>(item));
            }

            foreach (var item in ItensMenuDTO)
            {
                if (item.ItemMenu_Id > 0)
                    item.MenuPredecessor = Mapper.Map<ItemMenuDTO>(db.ItemMenu.Find(item.ItemMenu_Id));
            }

            return View(ItensMenuDTO);
        }

        // GET: ItemMenu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemMenu/Create
        [HttpPost]
        public ActionResult Create(ItemMenuDTO itemMenu)
        {
            try
            {
                ValidaItemMenu(itemMenu);

                if (!ModelState.IsValid)
                    return View(itemMenu);

                var ItemMenu = Mapper.Map<ItemMenu>(itemMenu);

                if (ItemMenu.Id > 0)
                {
                    ItemMenu.AlterDate = DateTime.Now;
                }

                db.ItemMenu.AddOrUpdate(ItemMenu);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: ItemMenu/Edit/5
        public ActionResult Edit(int id)
        {

            var ItenMenu = Mapper.Map<ItemMenuDTO>(db.ItemMenu.Find(id));

            return View("Create", ItenMenu);
        }

        private void ValidaItemMenu(ItemMenuDTO itemMenu)
        {
            if (string.IsNullOrEmpty(itemMenu.Name))
            {
                ModelState.AddModelError("Name", Resources.Resource.name_can_not_be_empty);
            }

            if (itemMenu.Id > 0)
            {
                if (db.RoleUserSgq.Where(r => r.Name == itemMenu.Name && r.Id != itemMenu.Id).ToList().Count() > 0)
                    ModelState.AddModelError("Name", Resources.Resource.repeated_username);
            }
            else if (db.RoleUserSgq.Where(r => r.Name == itemMenu.Name).ToList().Count() > 0)
            {
                ModelState.AddModelError("Name", Resources.Resource.repeated_username);
            }
        }


        public ActionResult EstruturaMenus()
        {
            return View();
        }
    }
}

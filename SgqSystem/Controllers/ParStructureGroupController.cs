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
    public class ParStructureGroupController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParStructureGroup
        public ActionResult Index()
        {
            return View(db.ParStructureGroup.Where(x => x.Active).ToList());
        }

        // GET: ParStructureGroup/Create
        [HttpGet]
        public ActionResult Create()
        {
            var listaFilhos = db.ParStructureGroup.Where(x => x.Active).ToList();
            listaFilhos.Insert(0, new ParStructureGroup() { Id = 0, Name = "Selecione" });
            ViewBag.ParStructureGroupParent_Id = new SelectList(listaFilhos, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ParStructureGroup parStructureGroup)
        {
            ValidaParStructureGroups(parStructureGroup);

            if (ModelState.IsValid)
            {
                db.ParStructureGroup.Add(parStructureGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
                return View(parStructureGroup);
        }

        // GET: ParStructureGroup/Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ParStructureGroup parStructureGroup = db.ParStructureGroup.Find(id);

            var listaFilhos = db.ParStructureGroup.Where(x => x.Active).ToList();
            listaFilhos.Insert(0, new ParStructureGroup() { Id = 0, Name = "Selecione" });
            ViewBag.ParStructureGroupParent_Id = new SelectList(listaFilhos, "Id", "Name", parStructureGroup.ParStructureGroupParent_Id);

            return View(parStructureGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ParStructureGroup parStructureGroup)
        {
            ValidaParStructureGroups(parStructureGroup);
            if (ModelState.IsValid)
            {
                db.Entry(parStructureGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parStructureGroup);
        }

        private void ValidaParStructureGroups(ParStructureGroup parStructureGroup)
        {
            if (parStructureGroup.Name == null)
                ModelState.AddModelError("Name", Resources.Resource.required_field + " " + Resources.Resource.name);

            if (parStructureGroup.Description == null)
                ModelState.AddModelError("Description", Resources.Resource.required_field + " " + Resources.Resource.description);
        }
    }
}
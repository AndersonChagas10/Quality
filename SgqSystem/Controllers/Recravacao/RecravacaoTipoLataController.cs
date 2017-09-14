using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SgqSystem.Controllers.Recravacao;
using Dominio;
using System.Data.SqlClient;

namespace SgqSystem.Controllers.Recravacao
{
    public class RecravacaoTipoLataController : Controller
    {
        private SgqDbDevEntities db;
        public RecravacaoTipoLataController()
        {
            db = new SgqDbDevEntities();
            //repo = new Repo<UserSgq>();
        }

        // GET: RecravacaoTipoLata
        public ActionResult Index()
        {
            var model = db.Database.SqlQuery<ParRecravacao_TipoLata>("SELECT * FROM ParRecravacao_TipoLata WHERE IsActive = 1").ToList();
            return View(model);
        }

        // GET: RecravacaoTipoLata/Create
        public ActionResult Create()
        {
            var model = new ParRecravacao_TipoLata();
            model.IsActive = true;
            return View(model);
        }

        // GET: RecravacaoTipoLata/Edit/5
        public ActionResult Edit(int id)
        {
            ParRecravacao_TipoLata model = GetTipoLata(id);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(ParRecravacao_TipoLata collection)
        {
            return Create(collection);
        }

        // GET: RecravacaoTipoLata/Details/5
        public ActionResult Details(int id)
        {
            ParRecravacao_TipoLata model = GetTipoLata(id);
            return View(model);
        }

        // POST: RecravacaoTipoLata/Create
        [HttpPost]
        public ActionResult Create(ParRecravacao_TipoLata collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                    Save(collection);
                else
                    return View();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void Save(ParRecravacao_TipoLata model)
        {
            if (model.Id > 0)
            {
                var update = string.Format("\n UPDATE [dbo].[ParRecravacao_TipoLata] " +
                    "\n   SET[Name] = N'{0}'" +
                    "\n      ,[Description] = N'{1}'" +
                    "\n      ,[NumberOfPoints] = {2}" +
                    "\n      ,[AlterDate] = {3}" +
                    "\n      ,[IsActive] = {4}" +
                    "\n WHERE Id = {5}"
                    , model.Name
                    , model.Description
                    , model.NumberOfPoints.ToString()
                    , "GETDATE()"
                    , model.IsActive ? "1" : "0"
                    , model.Id
                    );

                db.Database.ExecuteSqlCommand(update);

            }
            else
            {
                var insert = string.Format("\n INSERT INTO[dbo].[ParRecravacao_TipoLata] " +
                    "\n        ([Name]                          " +
                    "\n        ,[Description]                   " +
                    "\n        ,[NumberOfPoints]                " +
                    "\n        ,[AddDate]                       " +
                    "\n        ,[IsActive])                     " +
                    "\n  VALUES                                 " +
                    "\n        (N'{0}'                           " +
                    "\n        ,N'{1}'                    " +
                    "\n        ,{2}                         " +
                    "\n        ,{3}                       " +
                    "\n        ,{4} ) SELECT SCOPE_IDENTITY()"
                    , model.Name
                    , model.Description
                    , model.NumberOfPoints.ToString()
                    , "GETDATE()"
                    , model.IsActive ? "1" : "0");

                model.Id = int.Parse(db.Database.SqlQuery<decimal>(insert).FirstOrDefault().ToString());
            }
        }

        private ParRecravacao_TipoLata GetTipoLata(int id)
        {
            var model = new ParRecravacao_TipoLata();
            if (id > 0)
                model = db.Database.SqlQuery<ParRecravacao_TipoLata>("SELECT * FROM ParRecravacao_TipoLata WHERE IsActive = 1 AND Id = " + id).FirstOrDefault();
            return model;
        }

    }
}

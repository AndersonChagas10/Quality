﻿using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Recravacao
{
    public class RecravacaoLinhasController : Controller
    {
        private SgqDbDevEntities db;

        public RecravacaoLinhasController()
        {
            db = new SgqDbDevEntities();
            //repo = new Repo<UserSgq>();
        }

        // GET: RecravacaoTipoLata
        public ActionResult Index()
        {
            var model = db.Database.SqlQuery<ParRecravacao_Linhas>("SELECT * FROM ParRecravacao_Linhas WHERE IsActive = 1").ToList();
            return View(model);
        }

        //// GET: RecravacaoTipoLata/Create
        //public ActionResult Create()
        //{
        //    var model = new ParRecravacao_Linhas();
        //    model.IsActive = true;
        //    return View(model);
        //}

        //// GET: RecravacaoTipoLata/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    ParRecravacao_Linhas model = GetTipoLinhas(id);
        //    return View("Create", model);
        //}

        //[HttpPost]
        //public ActionResult Edit(ParRecravacao_Linhas collection)
        //{
        //    return Create(collection);
        //}

        //// GET: RecravacaoTipoLata/Details/5
        //public ActionResult Details(int id)
        //{
        //    ParRecravacao_Linhas model = GetTipoLinhas(id);
        //    return View(model);
        //}

        //// POST: RecravacaoTipoLata/Create
        //[HttpPost]
        //public ActionResult Create(ParRecravacao_Linhas collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        if (ModelState.IsValid)
        //            Save(collection);
        //        else
        //            return View();

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //private void Save(ParRecravacao_Linhas model)
        //{
        //    if (model.Id > 0)
        //    {
        //        var update = string.Format("\n UPDATE [dbo].[ParRecravacao_TipoLata] " +
        //            "\n   SET[Name] = N'{0}'" +
        //            "\n      ,[Description] = N'{1}'" +
        //            "\n      ,[NumberOfPoints] = {2}" +
        //            "\n      ,[AlterDate] = {3}" +
        //            "\n      ,[IsActive] = {4}" +
        //            "\n WHERE Id = {5}"
        //            , model.Name
        //            , model.Description
        //            , model.NumberOfPoints.ToString()
        //            , "GETDATE()"
        //            , model.IsActive ? "1" : "0"
        //            , model.Id
        //            );

        //        db.Database.ExecuteSqlCommand(update);

        //    }
        //    else
        //    {
        //        var insert = string.Format("\n INSERT INTO[dbo].[ParRecravacao_TipoLata] " +
        //            "\n        ([Name]                          " +
        //            "\n        ,[Description]                   " +
        //            "\n        ,[NumberOfPoints]                " +
        //            "\n        ,[AddDate]                       " +
        //            "\n        ,[IsActive])                     " +
        //            "\n  VALUES                                 " +
        //            "\n        (N'{0}'                           " +
        //            "\n        ,N'{1}'                    " +
        //            "\n        ,{2}                         " +
        //            "\n        ,{3}                       " +
        //            "\n        ,{4} ) SELECT SCOPE_IDENTITY()"
        //            , model.Name
        //            , model.Description
        //            , model.NumberOfPoints.ToString()
        //            , "GETDATE()"
        //            , model.IsActive ? "1" : "0");

        //        model.Id = int.Parse(db.Database.SqlQuery<decimal>(insert).FirstOrDefault().ToString());
        //    }
        //}

        //private ParRecravacao_Linhas GetTipoLinhas(int id)
        //{
        //    var model = new ParRecravacao_Linhas();
        //    if (id > 0)
        //        model = db.Database.SqlQuery<ParRecravacao_Linhas>("SELECT * FROM ParRecravacao_Linhas WHERE IsActive = 1 AND Id = " + id).FirstOrDefault();
        //    return model;
        //}
    }
}

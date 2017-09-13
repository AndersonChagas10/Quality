using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Recravacao
{
    public class ParRecravacaoLinhasController : Controller
    {
        // GET: ParRecravacaoLinhas
        public ActionResult Index()
        {
            return View();
        }

        // GET: ParRecravacaoLinhas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ParRecravacaoLinhas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParRecravacaoLinhas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ParRecravacaoLinhas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ParRecravacaoLinhas/Edit/5
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

        // GET: ParRecravacaoLinhas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ParRecravacaoLinhas/Delete/5
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

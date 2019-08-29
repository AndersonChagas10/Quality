using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Dominio.AppViewModel;

namespace SgqSystem.Controllers
{
    public class ComponenteGenericoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ComponenteGenerico
        public ActionResult Index()
        {
            return View(db.ComponenteGenerico.ToList());
        }

        // GET: ComponenteGenerico/Create
        public ActionResult Edit(int? id)
        {
            var componenteGenerico = new ComponenteGenericoViewModel();
            componenteGenerico.ComponentesGenericosColuna = new List<ComponenteGenericoColuna>();

            if (id != null && id > 0)
            {
                componenteGenerico.ComponenteGenerico = db.ComponenteGenerico.Find(id);
                componenteGenerico.ComponentesGenericosColuna = db.ComponenteGenericoColuna.Include("ComponenteGenericoTipoColuna").Where(x => x.ComponenteGenerico_Id == id).ToList();
            }

            ViewBag.ComponentesGenericosTipoColuna = db.ComponenteGenericoTipoColuna.Where(x => x.IsActive).ToList();

            return View(componenteGenerico);
        }

        // POST: ComponenteGenerico/Create
        [HttpPost]
        public ActionResult Edit(ComponenteGenericoViewModel collection)
        {
            try
            {

                SaveOrUpdateComponenteGenerico(collection.ComponenteGenerico);
                SaveOrUpdateComponenteGenericoColuna(collection);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ComponenteGenerico/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return RedirectToAction("Create", new { componenteGenerico_Id = id });
        //}

        //// POST: ComponenteGenerico/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ComponenteGenerico/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ComponenteGenerico/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        private ComponenteGenerico SaveOrUpdateComponenteGenerico(ComponenteGenerico componenteGenerico)
        {
            using (var db = new SgqDbDevEntities())
            {

                if (componenteGenerico.Id <= 0)
                {
                    componenteGenerico.AddDate = DateTime.Now;
                    db.ComponenteGenerico.Add(componenteGenerico);

                }
                else
                {
                    componenteGenerico.AlterDate = DateTime.Now;
                    db.Entry(componenteGenerico).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }

            return componenteGenerico;
        }

        private List<ComponenteGenericoColuna> SaveOrUpdateComponenteGenericoColuna(ComponenteGenericoViewModel collection)
        {
            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                foreach (var componenteGenericoColuna in collection.ComponentesGenericosColuna)
                {
                    if (componenteGenericoColuna.Id > 0)
                    {
                        componenteGenericoColuna.ComponenteGenerico_Id = collection.ComponenteGenerico.Id;
                        componenteGenericoColuna.AlterDate = DateTime.Now;
                        componenteGenericoColuna.ComponenteGenerico = null;
                        componenteGenericoColuna.ComponenteGenericoTipoColuna = null;
                        db.Entry(componenteGenericoColuna).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        componenteGenericoColuna.ComponenteGenerico_Id = collection.ComponenteGenerico.Id;
                        componenteGenericoColuna.AddDate = DateTime.Now;
                        db.ComponenteGenericoColuna.Add(componenteGenericoColuna);
                    }
                }

                db.SaveChanges();
            }

            return collection.ComponentesGenericosColuna;
        }
    }
}

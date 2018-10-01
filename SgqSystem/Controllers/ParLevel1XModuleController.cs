﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;
using DTO.Helpers;
using Microsoft.Ajax.Utilities;
using System.Text;

namespace SgqSystem.Controllers
{
    public class ParLevel1XModuleController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ParLevel1XModule
        public ActionResult Index()
        {
            return View(db.ParLevel1XModule.Where(x => x.IsActive).OrderBy(x => x.ParModule_Id).ToList());
        }

        // GET: ParLevel1XModule/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel1XModule parLevel1XModule = await db.ParLevel1XModule.FindAsync(id);
            if (parLevel1XModule == null)
            {
                return HttpNotFound();
            }
            return View(parLevel1XModule);
        }

        // GET: ParLevel1XModule/Create
        public ActionResult Create()
        {
            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            var listaModulos = db.ParModule.Where(x => x.IsActive).ToList();
            ViewBag.Indicadores = listaIndicadores;
            ViewBag.Modulos = listaModulos;
            return View();
        }

        // POST: ParLevel1XModule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Points,ParModule_Id,IsActive,EffectiveDateStart,EffectiveDateEnd,ParLevel1Helper,ParLevel1_IdHelper")] ParLevel1XModule parLevel1XModule)
        {
            parLevel1XModule.AddDate = DateTime.Now;
            parLevel1XModule.AlterDate = DateTime.Now;
            var quantSalvo = 0;
            List<int> arrayIndicadorDuplicado = new List<int>();
            if (parLevel1XModule.ParLevel1_IdHelper?.Count() > 0)
            {
                foreach (var item in parLevel1XModule.ParLevel1_IdHelper)
                {
                    parLevel1XModule.ParLevel1_Id = item;
                    ValidaIndicadoresxModulos(parLevel1XModule);
                    var listaErros = ValidaDataEntre(parLevel1XModule, item);

                    if (ModelState.IsValid)
                    {
                        var objInserir = new ParLevel1XModule()
                        {
                            ParLevel1_Id = parLevel1XModule.ParLevel1_Id,
                            EffectiveDateEnd = parLevel1XModule.EffectiveDateEnd,
                            EffectiveDateStart = parLevel1XModule.EffectiveDateStart,
                            ParModule_Id = parLevel1XModule.ParModule_Id,
                            Points = parLevel1XModule.Points,
                            IsActive = parLevel1XModule.IsActive,
                            AddDate = DateTime.Now,
                            AlterDate = DateTime.Now
                        };
                        db.ParLevel1XModule.Add(objInserir);
                        quantSalvo++;
                    }
                    else
                    {
                        //arrayIndicadorDuplicado.Add(parLevel1XModule.ParLevel1_Id);
                        arrayIndicadorDuplicado.Add(listaErros);
                    }
                }
                if (quantSalvo == parLevel1XModule.ParLevel1_IdHelper.Count())
                {

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var ListaDeIndicadoresQueJaExistem = db.ParLevel1.Where(r => arrayIndicadorDuplicado.Contains(r.Id)).Select(r => r.Name).ToList();
                    if (ListaDeIndicadoresQueJaExistem.Count() > 0)
                    {
                        var mensagemErro = new StringBuilder();
                        foreach (var item in ListaDeIndicadoresQueJaExistem)
                        {
                            if(mensagemErro.Length > 0) 
                                mensagemErro.Append(", ");
                            mensagemErro.Append(item);
                        }
                        ModelState.AddModelError("ParLevel1_Id", Resources.Resource.indicator_alredy_used + mensagemErro);
                    }

                }

            }
            else
            {
                ModelState.AddModelError("ParLevel1_IdHelper", Resources.Resource.required_field + " " + Resources.Resource.parlevel1);
            }
            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            var listaModulos = db.ParModule.Where(x => x.IsActive).ToList();
            ViewBag.Indicadores = listaIndicadores;
            ViewBag.Modulos = listaModulos;
            return View(parLevel1XModule);
        }

        // GET: ParLevel1XModule/Edit/5
        public async Task<ActionResult> Edit(int? id, int? moduloId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel1XModule parLevel1XModule = await db.ParLevel1XModule.FindAsync(id);
            if (parLevel1XModule == null)
            {
                return HttpNotFound();
            }
            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            var listaModulos = db.ParModule.Where(x => x.IsActive).ToList();
            ViewBag.Indicadores = listaIndicadores;
            ViewBag.Modulos = listaModulos;
            return View(parLevel1XModule);
        }

        // POST: ParLevel1XModule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParLevel1_Id,ParModule_Id,Points,IsActive,EffectiveDateStart,EffectiveDateEnd,ParLevel1Helper")] ParLevel1XModule parLevel1XModule)
        {
            parLevel1XModule.AlterDate = DateTime.Now;
            ValidaIndicadoresxModulosEdicao(parLevel1XModule);
            ValidaDataEntre(parLevel1XModule, parLevel1XModule.ParLevel1_Id);
            var indicadorxModuloEditado = db.ParLevel1XModule.Where(x => x.Id == parLevel1XModule.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                indicadorxModuloEditado.EffectiveDateEnd = parLevel1XModule.EffectiveDateEnd;
                indicadorxModuloEditado.EffectiveDateStart = parLevel1XModule.EffectiveDateStart;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("ParLevel1_Id", Resources.Resource.indicator_in_use);
            var listaIndicadores = db.ParLevel1.Where(x => x.IsActive).ToList();
            var listaModulos = db.ParModule.Where(x => x.IsActive).ToList();
            ViewBag.Indicadores = listaIndicadores;
            ViewBag.Modulos = listaModulos;
            return View(parLevel1XModule);
        }

        private void ValidaIndicadoresxModulosEdicao(ParLevel1XModule parLevel1XModule)
        {
            if (parLevel1XModule.ParLevel1_Id == 0)
                ModelState.AddModelError("ParLevel1_Id", Resources.Resource.required_field + " " + Resources.Resource.parlevel1);

            if (parLevel1XModule.ParModule_Id.IsNull())
                ModelState.AddModelError("ParModule_Id", Resources.Resource.required_field + " " + Resources.Resource.parModule);

            if (parLevel1XModule.EffectiveDateStart.IsNull())
                ModelState.AddModelError("EffectiveDateStart", Resources.Resource.required_field + " " + Resources.Resource.effective_date_start);

            if (parLevel1XModule.EffectiveDateStart > parLevel1XModule.EffectiveDateEnd)
                ModelState.AddModelError("EffectiveDateEnd", Resources.Resource.effective_date_start + " " + Resources.Resource.are_greater_than + " " + Resources.Resource.effective_date_end);
        }

        // GET: ParLevel1XModule/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParLevel1XModule parLevel1XModule = await db.ParLevel1XModule.FindAsync(id);
            if (parLevel1XModule == null)
            {
                return HttpNotFound();
            }
            return View(parLevel1XModule);
        }

        // POST: ParLevel1XModule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParLevel1XModule parLevel1XModule = await db.ParLevel1XModule.FindAsync(id);
            db.ParLevel1XModule.Remove(parLevel1XModule);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ValidaIndicadoresxModulos(ParLevel1XModule parLevel1XModule)
        {
            //parLevel1XModule.ParLevel1 == null && 
            if (parLevel1XModule.ParLevel1_Id == 0)
                ModelState.AddModelError("ParLevel1_Id", Resources.Resource.required_field + " " + Resources.Resource.parlevel1);

            if (parLevel1XModule.ParModule_Id.IsNull())
                ModelState.AddModelError("ParModule_Id", Resources.Resource.required_field + " " + Resources.Resource.parModule);

            if (parLevel1XModule.ParLevel1_IdHelper.IsNull())
                ModelState.AddModelError("ParLevel1_IdHelper", Resources.Resource.required_field + " " + Resources.Resource.parlevel1);

            if (!parLevel1XModule.IsActive)
                ModelState.AddModelError("IsActive", Resources.Resource.required_field + " " + Resources.Resource.is_active);

            if (parLevel1XModule.EffectiveDateStart.IsNull())
                ModelState.AddModelError("EffectiveDateStart", Resources.Resource.required_field + " " + Resources.Resource.effective_date_start);

            if (parLevel1XModule.EffectiveDateStart > parLevel1XModule.EffectiveDateEnd)
                ModelState.AddModelError("EffectiveDateEnd", Resources.Resource.effective_date_start + " " + Resources.Resource.are_greater_than + " " + Resources.Resource.effective_date_end);

        }
        private int ValidaDataEntre(ParLevel1XModule parLevel1XModule, int indicadorId)
        {
            var isNotValid = true;

            var haveDataEndNull = db.ParLevel1XModule
                .Any(x => x.ParModule_Id == parLevel1XModule.ParModule_Id && x.ParLevel1_Id == indicadorId
                && (x.EffectiveDateEnd == null || x.EffectiveDateEnd == DateTime.MinValue)
                && x.Id != parLevel1XModule.Id);

            if (haveDataEndNull)
            {
                if (parLevel1XModule.EffectiveDateEnd == null || parLevel1XModule.EffectiveDateEnd == DateTime.MinValue)
                {
                    isNotValid = true;
                }
                else
                {
                    isNotValid = db.ParLevel1XModule
                    .Any(x => x.ParModule_Id == parLevel1XModule.ParModule_Id && x.ParLevel1_Id == indicadorId
                    && (x.EffectiveDateStart < parLevel1XModule.EffectiveDateEnd) && x.Id != parLevel1XModule.Id);
                }
            }
            else
            {

                //isNotValid = db.ParLevel1XModule
                //    .Any(x => x.ParModule_Id == parLevel1XModule.ParModule_Id && x.ParLevel1_Id == indicadorId
                //    && (x.EffectiveDateStart < parLevel1XModule.EffectiveDateStart
                //    && x.EffectiveDateEnd < parLevel1XModule.EffectiveDateStart) && x.Id != parLevel1XModule.Id);

                //nova verificação
                isNotValid = db.ParLevel1XModule
                    .Any(x => x.ParModule_Id == parLevel1XModule.ParModule_Id && x.ParLevel1_Id == indicadorId
                    && ((x.EffectiveDateStart <= parLevel1XModule.EffectiveDateStart && x.EffectiveDateEnd >= x.EffectiveDateStart)
                    || (x.EffectiveDateStart <= parLevel1XModule.EffectiveDateEnd && x.EffectiveDateEnd >= parLevel1XModule.EffectiveDateEnd)
                    || (parLevel1XModule.EffectiveDateStart < x.EffectiveDateStart && parLevel1XModule.EffectiveDateEnd > x.EffectiveDateStart)));
            }

            if (isNotValid)
            {
                ModelState.AddModelError("ParLevel1_IdHelper", "Alerta!");
                return parLevel1XModule.ParLevel1_Id;
            }

            return 0;
        }
    }
}

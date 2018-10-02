﻿using Dominio;
using DTO.Helpers;
using Helper;
using SgqSystem.Secirity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [FilterUnit]
    [CustomAuthorize]
    public class Pcc1bController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: Pcc1b
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Index()
        {
            var userId = Guard.GetUsuarioLogado_Id(HttpContext);
            var userLogado = db.UserSgq.Where(r => r.Id == userId);
            var pcc1b = db.VolumePcc1b.Where(VCD => userLogado.FirstOrDefault().ParCompanyXUserSgq.Any(c => c.ParCompany_Id == VCD.ParCompany_id) || VCD.ParCompany_id == userLogado.FirstOrDefault().ParCompany_Id).Include(c => c.ParCompany).Include(c => c.ParLevel1);
            //var pcc1b = db.VolumePcc1b.Include(p => p.ParCompany).Include(p => p.ParLevel1).OrderByDescending(p => p.Data);

            //Date filter
            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]) && !string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                //Date filter
                System.DateTime startDate = Guard.ParseDateToSqlV2(Request.QueryString["startDate"]);
                System.DateTime endDate = Guard.ParseDateToSqlV2(Request.QueryString["endDate"]);

                pcc1b = pcc1b.Where(VCD => VCD.Data >= startDate && VCD.Data <= endDate);
            }
            else
            {
                System.DateTime startDate = System.DateTime.Now.AddDays(-2);
                System.DateTime endDate = System.DateTime.Now;
                pcc1b = pcc1b.Where(VCD => VCD.Data >= startDate && VCD.Data <= endDate);
            }

            //Company filter
            if (!string.IsNullOrEmpty(Request.QueryString["ParCompany_id"]))
            {
                int id = System.Convert.ToInt32(Request.QueryString["ParCompany_id"]);
                pcc1b = pcc1b.Where(VCD => VCD.ParCompany_id == id);
            }

            // pcc1b = pcc1b.OrderByDescending(c => c.Data);

            if (pcc1b.Count() > 0)
                //return View(pcc1b.ToList());
                return View(pcc1b.OrderByDescending(c => c.Data).ToList());
            else
                return View(new System.Collections.Generic.List<VolumePcc1b>());
        }

        // GET: Pcc1b/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumePcc1b pcc1b = db.VolumePcc1b.Find(id);
            if (pcc1b == null)
            {
                return HttpNotFound();
            }
            return View(pcc1b);
        }

        // GET: Pcc1b/Create
        public ActionResult Create()
        {
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 3), "Id", "Name");
            return View();
        }

        // POST: Pcc1b/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,VolumeAnimais,Quartos,Meta,ToleranciaDia,Nivel11,Nivel12,Nivel13,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumePcc1b pcc1b)
        {
            ValidaPcc1B(pcc1b);

            if (ModelState.IsValid)
            {
                //Verifica se já existe uma coleta no mesmo dia
                if (db.VolumePcc1b.Where(r => r.Data == pcc1b.Data && r.ParCompany_id == pcc1b.ParCompany_id).ToList().Count() == 0)
                {
                    pcc1b.AddDate = DateTime.Now;
                    db.VolumePcc1b.Add(pcc1b);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ReturnError(pcc1b);
                    //return View(pcc1b);
                }
            }

            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", pcc1b.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", pcc1b.ParLevel1_id);
            return View(pcc1b);
        }

        // GET: Pcc1b/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumePcc1b pcc1b = db.VolumePcc1b.Find(id);
            if (pcc1b == null)
            {
                return HttpNotFound();
            }

            //if (pcc1b.Data != null && pcc1b.Data?.Date < DateTime.Now.Date)
            //{
            //    return RedirectToAction("Index");
            //}

            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", pcc1b.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1.Where(c => c.Id == 3), "Id", "Name", pcc1b.ParLevel1_id);
            ViewBag.UnidadeUsuario = new SelectList(db.ParCompany.Where(c => c.Id == pcc1b.ParCompany_id), "Id", "Name", pcc1b.ParCompany_id);
            return View(pcc1b);
        }

        // POST: Pcc1b/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Indicador,Unidade,Data,Departamento,VolumeAnimais,Quartos,Meta,ToleranciaDia,Nivel11,Nivel12,Nivel13,Avaliacoes,Amostras,AddDate,AlterDate,ParCompany_id,ParLevel1_id")] VolumePcc1b pcc1b)
        {
            ValidaPcc1B(pcc1b);

            if (ModelState.IsValid)
            {
                if (db.VolumePcc1b.Where(r => r.Data == pcc1b.Data && r.ParCompany_id == pcc1b.ParCompany_id).ToList().Count() == 0)
                {
                    pcc1b.AlterDate = DateTime.Now;
                    db.Entry(pcc1b).State = EntityState.Modified;
                    if (db.VolumePcc1b.Where(r => r.Id == pcc1b.Id).Select(r => r.Data).FirstOrDefault() < DateTime.Now.Date)
                    {
                        db.Entry(pcc1b).Property(x => x.Data).IsModified = false;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    //Se for a edição da mesma data e parCompany
                    if (db.VolumePcc1b.Where(r => r.Data == pcc1b.Data &&
                                                       r.ParCompany_id == pcc1b.ParCompany_id &&
                                                       r.Id == pcc1b.Id).ToList().Count() == 1)
                    {
                        using (var db2 = new SgqDbDevEntities())
                        {
                            pcc1b.AlterDate = DateTime.Now;
                            db2.Entry(pcc1b).State = EntityState.Modified;
                            if (db2.VolumePcc1b.Where(r => r.Id == pcc1b.Id).Select(r => r.Data).FirstOrDefault() < DateTime.Now.Date)
                            {
                                db2.Entry(pcc1b).Property(x => x.Data).IsModified = false;
                            }
                            db2.SaveChanges();
                            return RedirectToAction("Index");
                        }

                    }
                    else
                    {
                        ReturnError(pcc1b);
                        //return View(pcc1b);
                    }
                }
            }
            ViewBag.ParCompany_id = new SelectList(db.ParCompany.OrderBy(c => c.Name), "Id", "Name", pcc1b.ParCompany_id);
            ViewBag.ParLevel1_id = new SelectList(db.ParLevel1, "Id", "Name", pcc1b.ParLevel1_id);
            ViewBag.UnidadeUsuario = new SelectList(db.ParCompany.Where(c => c.Id == pcc1b.ParCompany_id), "Id", "Name", pcc1b.ParCompany_id);
            return View(pcc1b);
        }

        private void ValidaPcc1B(VolumePcc1b pcc1b)
        {
            if (pcc1b.Data == null)
                ModelState.AddModelError("Data", Guard.MesangemModelError("Data", false));

            if (pcc1b.ParCompany_id == null)
                ModelState.AddModelError("ParCompany_id", Guard.MesangemModelError("Unidade", false));

            if (pcc1b.ParLevel1_id == null)
                ModelState.AddModelError("ParLevel1_id", Guard.MesangemModelError("Indicador", false));

            if (pcc1b.VolumeAnimais == null)
                ModelState.AddModelError("VolumeAnimais", Guard.MesangemModelError("Número de animais", false));

            //if (pcc1b.Id > 0 && pcc1b.Data != null && pcc1b.Data?.Date < DateTime.Now.Date)
            //{
            //    ModelState.AddModelError("Data", "Não é possível alterar o volume com data menor que a data atual");
            //}
        }

        private void ReturnError(VolumePcc1b obj)
        {
            ModelState.AddModelError("Data", $"Já existe registro na data {obj.Data.Value.ToShortDateString()} para esta unidade!");
            obj.Data = DateTime.Now;
        }

        // GET: Pcc1b/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolumePcc1b pcc1b = db.VolumePcc1b.Find(id);
            if (pcc1b == null)
            {
                return HttpNotFound();
            }
            return View(pcc1b);
        }

        // POST: Pcc1b/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolumePcc1b pcc1b = db.VolumePcc1b.Find(id);
            db.VolumePcc1b.Remove(pcc1b);
            db.SaveChanges();
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
    }
}

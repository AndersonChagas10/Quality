using Dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.CadastrosGerais
{
    public class ParReportLayoutXReportXUserController : BaseController
    {

        private SgqDbDevEntities db = new SgqDbDevEntities();

        public enum LayoutLevelEnum
        {
            Cabecalho = 1,
            Linha = 2,
            Valor = 3
        }

        public class LayoutLevel{

            public int Id { get; set; }

            public string Name { get; set; }

        }

        public class LayoutHeaderFieldValue
        {
            public string Name { get; set; }

        }

        List<LayoutLevel> NiveisLayout = new List<LayoutLevel>();

        // GET: ParReportLayoutXReportXUser
        public ActionResult Index()
        {
            return View();
        }

        private void PreencheViewBags(int reportXUserId, int idLevel1)
        {
            AdicionaItemNivelLayout(LayoutLevelEnum.Cabecalho.ToString(), (int)LayoutLevelEnum.Cabecalho);
            AdicionaItemNivelLayout(LayoutLevelEnum.Linha.ToString(), (int)LayoutLevelEnum.Linha);
            AdicionaItemNivelLayout(LayoutLevelEnum.Valor.ToString(), (int)LayoutLevelEnum.Valor);

            ViewBag.ReportXUser_Id = reportXUserId;

            ViewBag.Level1Id = idLevel1;

            ViewBag.NiveisLayout_Id = new SelectList(NiveisLayout.ToList(), "Id", "Name");

            var layoutHeaderField = new List<LayoutHeaderFieldValue>();

            var cabecalhosVinculadosIndicador = db.ParLevel1XHeaderField
                .Where(x => x.ParLevel1_Id == idLevel1 && x.IsActive)
                .Include(x => x.ParHeaderField)
                .Distinct()
                .ToList();

            var itensDoRelatorio = db.ReportLayoutItens.Where(x => x.IsActive).ToList();

            for (int i = 0; i < cabecalhosVinculadosIndicador.Count(); i++)
            {
                itensDoRelatorio.Add(new ReportLayoutItens() { Name = cabecalhosVinculadosIndicador[i].ParHeaderField_Id + " | " + cabecalhosVinculadosIndicador[i].ParHeaderField.Name.ToString() });
            }

            ViewBag.Level1_Id = idLevel1;
            ViewBag.ReportLayoutItens = new SelectList(itensDoRelatorio, "Name", "Name");
        }

        private void AdicionaItemNivelLayout(string name, int id)
        {
            LayoutLevel niveisLayout = new LayoutLevel();

            niveisLayout.Id = id;
            niveisLayout.Name = name;

            NiveisLayout.Add(niveisLayout);
        }


        public ActionResult Create(int reportXUserId, int level1_Id)
        {
            PreencheViewBags(reportXUserId, level1_Id);

            return View();
        }


        [HttpPost]
        public ActionResult Create(ParReportLayoutXReportXUser reportLayoutXReportXUser)
        {

            ValidaObjeto(reportLayoutXReportXUser);

            if (ModelState.IsValid)
            {

                if (reportLayoutXReportXUser.Value.Split('|').Count() > 1)
                {
                    reportLayoutXReportXUser.IdReference = int.Parse(reportLayoutXReportXUser.Value.Split('|')[0]);
                   
                    reportLayoutXReportXUser.TableReference = "ParHeaderField";
                }

                using (db)
                {
                    reportLayoutXReportXUser.IsActive = true;
                    db.ParReportLayoutXReportXUser.Add(reportLayoutXReportXUser);

                    db.SaveChanges();

                    return RedirectToAction("Details/" + reportLayoutXReportXUser.ReportXUserSgq_Id, "ReportXUserSgq");
                }
            }

            PreencheViewBags(reportLayoutXReportXUser.ReportXUserSgq_Id, reportLayoutXReportXUser.Level1_Id);

            return View(reportLayoutXReportXUser);
        }

        // GET: ReportXUserSgqs/Edit/5
        public ActionResult Edit(int? id, int level1_Id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ParReportLayoutXReportXUser reportLayoutXReportXUser = db.ParReportLayoutXReportXUser.Find(id);

            if (reportLayoutXReportXUser == null)
            {
                return HttpNotFound();
            }
            PreencheViewBags(reportLayoutXReportXUser.ReportXUserSgq_Id, level1_Id);
            return View(reportLayoutXReportXUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ParReportLayoutXReportXUser reportLayoutXReportXUser)
        {
            ValidaObjeto(reportLayoutXReportXUser);
          
            if (ModelState.IsValid)
            {
                if (reportLayoutXReportXUser.Value.Split('|').Count() > 1)
                {
                    reportLayoutXReportXUser.IdReference = int.Parse(reportLayoutXReportXUser.Value.Split('|')[0]);

                    reportLayoutXReportXUser.TableReference = "ParHeaderField";
                }

                reportLayoutXReportXUser.AlterDate = DateTime.Now;
                reportLayoutXReportXUser.IsActive = true;
                db.Entry(reportLayoutXReportXUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + reportLayoutXReportXUser.ReportXUserSgq_Id, "ReportXUserSgq");
            }

            PreencheViewBags(reportLayoutXReportXUser.ReportXUserSgq_Id, reportLayoutXReportXUser.Level1_Id);

            return View(reportLayoutXReportXUser);
        }

        private void ValidaObjeto(ParReportLayoutXReportXUser reportLayoutXReportXUser)
        {
            if(reportLayoutXReportXUser.Value == null)
            {
                ModelState.AddModelError("Value", "O campo Valor é obrigatório");
            }
        }


        // GET: ParReportLayoutXReportXUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var reportLayoutXReportXUser = db.ParReportLayoutXReportXUser.Where(r => r.Id == id).FirstOrDefault();

            if (reportLayoutXReportXUser == null)
            {
                return HttpNotFound();
            }
            return View(reportLayoutXReportXUser);
        }

        // POST: ParReportLayoutXReportXUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParReportLayoutXReportXUser reportLayoutXReportXUser = db.ParReportLayoutXReportXUser.Find(id);

            reportLayoutXReportXUser.IsActive = false;
            db.SaveChanges();

            return RedirectToAction("Details/" + reportLayoutXReportXUser.ReportXUserSgq_Id, "ReportXUserSgq");
        }

    }
}
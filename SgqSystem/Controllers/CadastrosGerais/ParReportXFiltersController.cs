using Dominio;
using Dominio.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.CadastrosGerais
{
    public class ParReportXFiltersController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public class FilterLevel
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        List<FilterLevel> NiveisFilter = new List<FilterLevel>();

        // GET: ParReportXFilters
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create(int reportXUserId, int level1_Id)
        {
            PreencheViewBags(reportXUserId, level1_Id);

            return View();
        }

        [HttpPost]
        public ActionResult Create(ParReportXFilter reportXFilter)
        {

            if (ModelState.IsValid)
            {
                reportXFilter.IsActive = true;
                using (db)
                {
                    reportXFilter.IsActive = true;
                    db.ParReportXFilter.Add(reportXFilter);

                    db.SaveChanges();

                    return RedirectToAction("Details/" + reportXFilter.ReportXUserSgq_Id, "ReportXUserSgq");
                }
            }

            PreencheViewBags(reportXFilter.ReportXUserSgq_Id, reportXFilter.Level1_Id);

            return View(reportXFilter);
        }

        public ActionResult Edit(int? id, int level1_Id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ParReportXFilter reportXFilter = db.ParReportXFilter.Find(id);

            if (reportXFilter == null)
            {
                return HttpNotFound();
            }
            PreencheViewBags(reportXFilter.ReportXUserSgq_Id, level1_Id);
            return View(reportXFilter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ParReportXFilter reportXFilter)
        {

            if (ModelState.IsValid)
            {
                reportXFilter.AlterDate = DateTime.Now;
                reportXFilter.IsActive = true;
                db.Entry(reportXFilter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + reportXFilter.ReportXUserSgq_Id, "ReportXUserSgq");
            }

            PreencheViewBags(reportXFilter.ReportXUserSgq_Id, reportXFilter.Level1_Id);

            return View(reportXFilter);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var reportXFilter = db.ParReportXFilter.Where(r => r.Id == id).FirstOrDefault();


            FilterEnum enumFiltro = (FilterEnum)reportXFilter.FilterLevel;

            reportXFilter.FilterLevel_Name = enumFiltro.ToString();

            PreencheViewBags(reportXFilter.ReportXUserSgq_Id, reportXFilter.Level1_Id);
            if (reportXFilter == null)
            {
                return HttpNotFound();
            }
            return View(reportXFilter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParReportXFilter reportXFilter = db.ParReportXFilter.Find(id);

            reportXFilter.IsActive = false;
            db.SaveChanges();

            return RedirectToAction("Details/" + reportXFilter.ReportXUserSgq_Id, "ReportXUserSgq");
        }

        public void PreencheViewBags(int reportXUserId, int level1_Id)
        {
            AdicionaItemObjFiltro(FilterEnum.Data.ToString(), (int)FilterEnum.Data);
            AdicionaItemObjFiltro(FilterEnum.Indicador.ToString(), (int)FilterEnum.Indicador);
            AdicionaItemObjFiltro(FilterEnum.Monitoramento.ToString(), (int)FilterEnum.Monitoramento);
            AdicionaItemObjFiltro(FilterEnum.Tarefa.ToString(), (int)FilterEnum.Tarefa);
            AdicionaItemObjFiltro(FilterEnum.Modulo.ToString(), (int)FilterEnum.Modulo);
            AdicionaItemObjFiltro(FilterEnum.GrupoDeCluster.ToString(), (int)FilterEnum.GrupoDeCluster);
            AdicionaItemObjFiltro(FilterEnum.Cluster.ToString(), (int)FilterEnum.Cluster);
            AdicionaItemObjFiltro(FilterEnum.Holding.ToString(), (int)FilterEnum.Holding);
            AdicionaItemObjFiltro(FilterEnum.Negocio.ToString(), (int)FilterEnum.Negocio);
            AdicionaItemObjFiltro(FilterEnum.Regional.ToString(), (int)FilterEnum.Regional);
            AdicionaItemObjFiltro(FilterEnum.Unidade.ToString(), (int)FilterEnum.Unidade);
            AdicionaItemObjFiltro(FilterEnum.Turno.ToString(), (int)FilterEnum.Turno);
            AdicionaItemObjFiltro(FilterEnum.Departamento.ToString(), (int)FilterEnum.Departamento);
            AdicionaItemObjFiltro(FilterEnum.Secao.ToString(), (int)FilterEnum.Secao);
            AdicionaItemObjFiltro(FilterEnum.Cargo.ToString(), (int)FilterEnum.Cargo);          
            AdicionaItemObjFiltro(FilterEnum.Avaliacao.ToString(), (int)FilterEnum.Avaliacao);          
            AdicionaItemObjFiltro(FilterEnum.Amostra.ToString(), (int)FilterEnum.Amostra);          

            ViewBag.NiveisFiltro_Id = new SelectList(NiveisFilter.ToList(), "Id", "Name");

            ViewBag.ReportXUser_Id = reportXUserId;

            ViewBag.Level1Id = level1_Id;
        }

        private void AdicionaItemObjFiltro(string name, int id)
        {
            FilterLevel niveisLayout = new FilterLevel();

            niveisLayout.Id = id;
            niveisLayout.Name = name;

            NiveisFilter.Add(niveisLayout);
        }
    }
}
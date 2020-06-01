using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
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

        List<LayoutLevel> NiveisLayout = new List<LayoutLevel>();

        // GET: ParReportLayoutXReportXUser
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int reportXUserId)
        {
            salvar(LayoutLevelEnum.Cabecalho.ToString(), (int)LayoutLevelEnum.Cabecalho);
            salvar(LayoutLevelEnum.Linha.ToString(), (int)LayoutLevelEnum.Linha);
            salvar(LayoutLevelEnum.Valor.ToString(), (int)LayoutLevelEnum.Valor);

            ViewBag.ReportXUser_Id = reportXUserId;

            ViewBag.NiveisLayout_Id = new SelectList(NiveisLayout.ToList(), "Id", "Name");

            return View();
        }

        private void salvar(string name, int id)
        {
            LayoutLevel niveisLayout = new LayoutLevel();

            niveisLayout.Id = id;
            niveisLayout.Name = name;

            NiveisLayout.Add(niveisLayout);
        }

        [HttpPost]
        public ActionResult Create(ParReportLayoutXReportXUser form)
        {

            if (ModelState.IsValid)
            {
                using (db)
                {
                    form.IsActive = true;
                    db.ParReportLayoutXReportXUser.Add(form);

                    db.SaveChanges();
                }
            }

            return RedirectToAction("Details", "ReportXUserSgq", form.ReportXUserSgq_Id);
        }
    }
}
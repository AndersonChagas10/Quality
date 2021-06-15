using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class AcoesController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: Acoes
        public ActionResult Index()
        {
            var teste = new List<Acao>();
            using (db) {

                 teste = db.Acao
                    .Include("ParLevel1")
                    .Include("ParLevel2")
                    .Include("ParLevel3")
                    .Include("ParCargo")
                    .Include("ParCompany")
                    .Include("ParDepartment")
                    .ToList();

             }

            return View(teste);
        }
    }
}
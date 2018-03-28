using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    public class Home2Controller : Controller
    {
        public Home2Controller()
        {
            Jobs.UpdateStatus();
        }


        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = "Gabriel Page";
            return View("Index2");
        }

        // GET: Home2
        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Index3()
        {
            return View();
        }

        //protected void UpdateStatus()
        //{
        //    using (var dbPa = new PlanoAcaoEF.PlanoDeAcaoEntities())
        //    {
        //        dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 1 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] = (5) AND  CONVERT (date ,QuandoFim) < CONVERT (date ,GETDATE()))");
        //        dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 5 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] = (1) AND  CONVERT (date ,QuandoFim) >= CONVERT (date ,GETDATE()))");
        //    }
        //}
    }
}
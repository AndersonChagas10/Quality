using DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
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

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            if (languageCookie != null)
            {
                if (languageCookie.Value == "en")
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
                }
            }
            else
            {
                if (GlobalConfig.LanguageBrasil)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
                }
                else if (GlobalConfig.LanguageEUA)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
                }
            }

            try
            {

                System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

                ViewBag.Resources = resourceManager.GetResourceSet(
                    Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();

            }
            catch (Exception ex)
            {
            }

            base.Initialize(requestContext);
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
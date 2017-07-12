using System;
using System.Web;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{

    public class HomeController : Controller
    {
        public HomeController()
        {
            UpdateStatus();
        }

        protected void UpdateStatus()
        {
            using (var dbPa = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 1 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] = (5) AND  CONVERT (date ,QuandoFim) < CONVERT (date ,GETDATE()))");
            }
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public ActionResult LogOut()
        {
            // clear cookies
            ExpireCookie();
            return View("Index");
        }

        protected void ExpireCookie()
        {
            HttpCookie currentUserCookie = Request.Cookies["webControlCookie"];
            if (currentUserCookie != null)
            {
                Response.Cookies.Remove("webControlCookie");
                Response.Cookies.Remove("Language");

                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
            }

        }

    }
}

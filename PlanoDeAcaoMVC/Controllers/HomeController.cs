using System;
using System.Web;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{

    public class HomeController : Controller
    {
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

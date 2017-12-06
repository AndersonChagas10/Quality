using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    public class Home2Controller : Controller
    {
        // GET: Home2
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
    }
}
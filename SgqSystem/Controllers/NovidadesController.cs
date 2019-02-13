using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class NovidadesController : BaseController
    {
        // GET: Novidades
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Versao1_4()
        {
            return View();
        }

        public ActionResult Versao1_5()
        {
            return View();
        }
    }
}
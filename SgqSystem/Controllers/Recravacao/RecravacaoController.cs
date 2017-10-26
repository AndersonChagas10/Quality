using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Recravacao
{
    [CustomAuthorize]
    public class RecravacaoController : Controller
    {
        // GET: Recravacao
        public ActionResult Index()
        {
            return View();
        }

        // GET: Recravacao
        public ActionResult Print()
        {
            return View();
        }
    }
}
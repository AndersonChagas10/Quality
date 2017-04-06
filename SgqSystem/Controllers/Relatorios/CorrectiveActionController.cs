using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Relatorios
{
    [CustomAuthorize]
    public class CorrectiveActionController : BaseController
    {
        // GET: CorrectiveAction
        public ActionResult Index()
        {
            return View();
        }
    }
}
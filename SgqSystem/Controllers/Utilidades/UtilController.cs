using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Utilidades
{
    public class UtilidadesController : BaseController
    {
        // GET: Util
        public ActionResult SqlStudio()
        {
            return View();
        }
    }
}
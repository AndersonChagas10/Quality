using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    
    public class RelatoriosCovidController : BaseController
    {
        [CustomAuthorize]
        public ActionResult RelatorioResultado()
        {
            ViewBag.ShowRangeDate = true;
            ViewBag.ShowParCluster = true;
            ViewBag.ShowParCompany = true;
            ViewBag.ShowParStructure2 = true;
            ViewBag.ShowParStructure3 = true;
            return View();
        }
    }
}
using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Integracao
{
    public class IntegracaoController : BaseController
    {
        // GET: Integracao
        public ActionResult Index()
        {
            ViewBag.UnitId = getUserUnitId();
            return View();
        }
    }

}
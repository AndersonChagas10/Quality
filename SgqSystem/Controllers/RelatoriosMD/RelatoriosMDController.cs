using Helper;
using SgqSystem.Secirity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.RelatoriosMD
{
    [CustomAuthorize]
    [OutputCache(Duration = 20, VaryByParam = "none")]
    public class RelatoriosMDController : BaseController
    {
        // GET: RelatoriosMD
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult AvaliacaoSensorial()
        {
            return View();
        }
    }
}
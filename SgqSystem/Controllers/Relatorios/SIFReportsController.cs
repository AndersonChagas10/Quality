using Helper;
using SgqSystem.Secirity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Relatorios
{
    [CustomAuthorize]
    [OutputCache(Duration = 20, VaryByParam = "none")]
    public class SIFReportsController : BaseController
    {
        // GET: SIFReports
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Index()
        {
            return View();
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult AcompanhamentoEmbarque()
        {
            return View();
        }
    }
}
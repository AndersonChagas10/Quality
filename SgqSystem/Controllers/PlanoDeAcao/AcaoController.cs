using Dominio;
using SgqSystem.Secirity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class AcaoController : BaseController
    {
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Index()
        {
            return View("~/Views/PlanoDeAcao/Acao/index.cshtml");
        }
    }
}
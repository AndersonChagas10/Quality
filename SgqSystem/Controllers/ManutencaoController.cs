﻿using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    [FormularioPesquisa(filtraUnidadePorUsuario = true)]
    public class ManutencaoController : BaseController
    {
        #region Constructor

        private FormularioParaRelatorioViewModel form;
      

        public ManutencaoController()
        {
            form = new FormularioParaRelatorioViewModel();
        }

        #endregion

        // GET: Manutencao
        public ActionResult Index()
        {
            return View(form);
        }
    }
}
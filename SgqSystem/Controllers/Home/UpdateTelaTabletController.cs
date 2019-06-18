using Dominio;
using DTO;
using DTO.DTO.Params;
using SgqSystem.Secirity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Home
{
    public class UpdateTelaTabletController : BaseController
    {
        SgqDbDevEntities db;

        public UpdateTelaTabletController()
        {
            db = new SgqDbDevEntities();
        }

        // GET: UpdateTelaTablet
        [FormularioPesquisa(filtraUnidadePorUsuario=true)]
        public ActionResult Index(bool isBtnAtualizar = false)
        {
            List<ParCompanyDTO> listaMinhasUnidades = (List<ParCompanyDTO>)ViewBag.UnidadeUsuario;
            ViewBag.UnidadesUsuario = listaMinhasUnidades;
            ViewBag.IdUnidadesUsuario = listaMinhasUnidades.Select(u => u.Id).ToList();

            ViewBag.UpdateTelaTabletRoot = DicionarioEstaticoHelper.DicionarioEstaticoHelpers.UpdateTelaTabletRoot;
            //listaMinhasUnidades.Select(u=>u.Id);

            ViewBag.IsBtnAtualizar = false;

            if (isBtnAtualizar)
            {
                ViewBag.IsBtnAtualizar = true;
            }

            return View();
        }

    }
}
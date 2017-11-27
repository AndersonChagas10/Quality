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
    public class UpdateTelaTabletController : Controller
    {
        SgqDbDevEntities db;

        public UpdateTelaTabletController()
        {
            db = new SgqDbDevEntities();
        }

        // GET: UpdateTelaTablet
        [FormularioPesquisa(filtraUnidadePorUsuario=true)]
        public ActionResult Index()
        {
            List<ParCompanyDTO> listaMinhasUnidades = (List<ParCompanyDTO>)ViewBag.UnidadeUsuario;
            ViewBag.UnidadesUsuario = listaMinhasUnidades;
            ViewBag.IdUnidadesUsuario = listaMinhasUnidades.Select(u => u.Id).ToList();
            //listaMinhasUnidades.Select(u=>u.Id);

            return View();
        }

    }
}
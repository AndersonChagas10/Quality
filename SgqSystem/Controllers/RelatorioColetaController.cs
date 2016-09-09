using Application.Interface;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
namespace SgqSystem.Controllers
{
    public class RelatorioColetaController : Controller
    {

        private readonly IRelatorioColetaApp _relatorioColetaApp;

        public RelatorioColetaController(IRelatorioColetaApp relatorioColetaApp)
        {
            _relatorioColetaApp = relatorioColetaApp;
        }

        // GET: RelatorioColeta
        public ActionResult Index()
        {

            var form = new FormularioParaRelatorioViewModel();
            
            //MOCK
            form.level01DTO = new List<SelectListItem>()
            {
                new SelectListItem() {Text="HTP", Value="1"},
                new SelectListItem() {Text="Carcass Contamination Audit", Value="2"},
                new SelectListItem() {Text="CFF (Cut, Fold and Flaps)", Value="3"},
            };

            return View(form);
        }
    }
}
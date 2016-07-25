using Application.Interface;
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
            var resultadosLista = _relatorioColetaApp.GetColetas();
            return View(resultadosLista.Retorno);
        }
    }
}
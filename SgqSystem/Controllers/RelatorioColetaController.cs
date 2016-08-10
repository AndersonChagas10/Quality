using Application.Interface;
using NLog;
using System;
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
            //Logger logger = LogManager.GetLogger("dataBaseLogger");
            //logger.Error(new Exception("teste"), "teste", null);
            var resultadosLista = _relatorioColetaApp.GetColetas();
            return View(resultadosLista.Retorno);
        }
    }
}
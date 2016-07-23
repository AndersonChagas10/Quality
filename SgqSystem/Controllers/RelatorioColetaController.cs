using Application.Interface;
using Dominio;
using System.Web.Mvc;
//Este controller não esta funcionando depois da migração de code first pra EDMX =)  Celso Géa 21 07 2016.
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
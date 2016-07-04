using Application.Interface;
using Dominio.Entities;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class RelatorioColetaController : Controller
    {

        private readonly IAppServiceBase<ResultOld> _serviceAppBase;

        public RelatorioColetaController(IAppServiceBase<ResultOld> serviceAppBase)
        {
            _serviceAppBase = serviceAppBase;
        }
        // GET: RelatorioColeta
        public ActionResult Index()
        {
            var resultadosLista = _serviceAppBase.GetAll();
            return View(resultadosLista);
        }
    }
}
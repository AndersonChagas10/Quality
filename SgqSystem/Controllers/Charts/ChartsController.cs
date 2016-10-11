using Application.Interface;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ChartsController : Controller
    {
        #region Constructor

        private readonly IRelatorioColetaApp _relatorioColetaApp;
        private readonly IUserApp _userApp;

        public ChartsController(IRelatorioColetaApp relatorioColetaApp, IUserApp userApp)
        {
            _userApp = userApp;
            _relatorioColetaApp = relatorioColetaApp;
        }

        #endregion

        // GET: RelatorioBeta
        public ActionResult Index()
        {
            return View();
        }
    }
}
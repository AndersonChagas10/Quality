using Application.Interface;
using Dominio.Interfaces.Services;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ChartsController : Controller
    {
        #region Constructor

        private readonly IRelatorioColetaDomain _relatorioColetaDomain;
        private readonly IUserDomain _userDomain;

        public ChartsController(IRelatorioColetaDomain relatorioColetaDomain, IUserDomain userDomain)
        {
            _userDomain = userDomain;
            _relatorioColetaDomain = relatorioColetaDomain;
        }

        #endregion

        // GET: RelatorioBeta
        public ActionResult Index()
        {
            return View();
        }
    }
}
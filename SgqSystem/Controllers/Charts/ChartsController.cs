using Dominio.Interfaces.Services;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ChartsController : BaseController
    {
        #region Constructor
        
        private readonly IUserDomain _userDomain;

        public ChartsController( IUserDomain userDomain)
        {
            _userDomain = userDomain;
        }

        #endregion

        // GET: RelatorioBeta
        public ActionResult Index()
        {
            return View();
        }
    }
}
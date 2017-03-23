using Helper;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class GestaoController : BaseController
    {
        // GET: Gestao
        public ActionResult Index()
        {
            return View();
        }
    }
}
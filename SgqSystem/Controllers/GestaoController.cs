using SgqSystem.Secirity;
using System.Web.Mvc;
using Helper;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class GestaoController : Controller
    {
        // GET: Gestao
        public ActionResult Index()
        {
            return View();
        }
    }
}
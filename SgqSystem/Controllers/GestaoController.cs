using SgqSystem.Secirity;
using System.Web.Mvc;

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
using Helper;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class ExemploInputController : Controller
    {
        // GET: ExemploInput
        public ActionResult Index()
        {
            return View();
        }
    }
}
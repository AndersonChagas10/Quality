using SgqSystem.Secirity;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class HomeController : Controller
    {

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Title = "Sgq Global";
            return View();
        }
    }
}

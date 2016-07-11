using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Sgq Global Beta";

            return View();
        }
    }
}

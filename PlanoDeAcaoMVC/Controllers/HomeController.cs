using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}

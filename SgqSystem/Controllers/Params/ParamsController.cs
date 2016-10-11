using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{

    public class ParamsController : Controller
    {
        // GET: Par
        public ActionResult Index()
        {
            ParamsViewModel pvm = new ParamsViewModel();
            return View(pvm);
        }

    }

}
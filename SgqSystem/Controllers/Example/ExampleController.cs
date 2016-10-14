using Helper;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{

    public class ExampleController : Controller
    {
        // GET: Par
        [HandlerParams()]
        public ActionResult Index()
        {
            ContextExampleViewModel pvm = new ContextExampleViewModel();
            return View(pvm);
        }

    }

}
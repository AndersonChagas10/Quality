using Helper;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    /// <summary>
    /// Controller para aprendizagem do sistema.
    /// </summary>
    public class ExampleController : Controller
    {
        [HandleController()]
        public ActionResult Index()
        {
            ContextExampleViewModel pvm = new ContextExampleViewModel();
            return View(pvm);
        }

    }

}
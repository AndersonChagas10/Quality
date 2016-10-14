using Helper;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    public class ParamsController : Controller
    {
        // GET: Params
        [HandleController()]
        public ActionResult Index()
        {
            ParamsViewModel paramsViewModel = new ParamsViewModel();
            return View(paramsViewModel);
        }
    }
}
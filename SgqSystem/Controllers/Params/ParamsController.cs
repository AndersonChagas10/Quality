using DTO.DTO.Params;
using Helper;
using SgqSystem.ViewModels;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    [HandleController()]
    public class ParamsController : Controller
    {

        // GET: Params
        public ActionResult Index()
        {
            var teste = new ParamsViewModel();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            return View(teste);
        }

        

       

    }
}
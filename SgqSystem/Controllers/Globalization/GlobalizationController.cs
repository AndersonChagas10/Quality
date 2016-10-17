using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace SgqSystem.Controllers.Globalization
{
    public class GlobalizationController : Controller
    {

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        // GET: Globalization
        public ActionResult Index()
        {
            var teste = new ParamsViewModel();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            return View(teste);
        }

    }
}
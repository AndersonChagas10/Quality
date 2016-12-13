using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class BaseController : Controller
    {

        public BaseController() {
            ViewBag.UrlDataCollectBR = "http://192.168.25.200/AppColeta/";
            ViewBag.UrlDataCollectBRHml = "http://mtzsvmqsc/AppColeta/";
            ViewBag.UrlDataCollectBRPRD = "http://192.168.25.200/AppColeta/";
            ViewBag.UrlDataCollectBRPastaDataCollect = VirtualPathUtility.ToAbsolute("~/DataCollect/");
            ViewBag.UrlDataCollectUSA = "http://192.168.25.200/AppBrasil/";
            ViewBag.UrlDataCollectUSA2 = "http://192.168.25.200/AppBrasil/";
            ViewBag.UrlDataCollect = "http://192.168.25.200/AppBrasil/";
        }
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            if (languageCookie != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }


            base.Initialize(requestContext);
        }
    }
}
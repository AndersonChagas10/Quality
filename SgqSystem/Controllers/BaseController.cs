using DTO;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{

    public class BaseController : Controller
    {
        public BaseController()
        {
            //GlobalConfig.linkDataCollect = "http://192.168.25.200/AppColeta/";
            ViewBag.UrlDataCollect = GlobalConfig.linkDataCollect;
            //UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            //ViewBag.UrlScorecard = u.Action("Scorecard", "RelatoriosSgq");
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
                if (GlobalConfig.Brasil)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
                }
                else if (GlobalConfig.Eua)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
                }
            }


            base.Initialize(requestContext);
        }

    
    }

}
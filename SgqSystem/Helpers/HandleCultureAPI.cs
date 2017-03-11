using DTO;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace SgqSystem.Helpers
{
    public class HandleCultureAPI : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            if (GlobalConfig.Brasil)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-br");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-br");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
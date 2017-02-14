using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SgqSystem.Handlres
{
    public class CookieTimerControl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            // I need to read cookie values here
            //Read the cookie from Request.
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get("webControlCookie");
            if (cookie == null)
            {
                //No cookie found or cookie expired.
                //Handle the situation here, Redirect the user or simply return;
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "UserAuthentication", action = "LogIn" }));
            }
            else
            {
                //if (!string.IsNullOrEmpty(cookie.Values["roles"]))
                //    _userSgqRoles = cookie.Values["roles"].ToString();
                ////Extends cookie ttl
                //cookie.Expires = DateTime.Now.AddMinutes(60);
                //filterContext.HttpContext.Response.Cookies.Set(cookie);
                ////ok - cookie is found.
                ////Gracefully check if the cookie has the key-value as expected.
                //if (!string.IsNullOrEmpty(Roles))
                //{
                //    if (!string.IsNullOrEmpty(_userSgqRoles))
                //    {
                //        //Yes userId is found. Mission accomplished.
                //        //CustomPrincipal cp = new CustomPrincipal(SessionPersister.Username);
                //        if (!IsInRole(_userSgqRoles))
                //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccesDenied", action = "Index" }));

                //    }
                //    else
                //    {
                //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccesDenied", action = "Index" }));
                //    }
                }

                //if (!string.IsNullOrEmpty(_userSgqRoles) && !Roles.Contains("somentemanutencao-sgq"))
                //    if (_userSgqRoles.Contains("somentemanutencao-sgq") && !HttpContext.Current.Request.RawUrl.Contains("ManPainelGestao/Index"))
                //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "ManPainelGestao", action = "Index" }));
            }

           

            //var modelState = filterContext.Controller.ViewData.ModelState;
            //if (!modelState.IsValid)
            //{
            //    var errorModel =
            //            from x in modelState.Keys
            //            where modelState[x].Errors.Count > 0
            //            select new
            //            {
            //                key = x,
            //                errors = modelState[x].Errors.
            //                                              Select(y => y.ErrorMessage).
            //                                              ToArray()
            //            };
            //    filterContext.Result = new JsonResult()
            //    {
            //        Data = errorModel
            //    };
            //    filterContext.HttpContext.Response.StatusCode =
            //                                          (int)HttpStatusCode.BadRequest;
            //}
        }
    }
}
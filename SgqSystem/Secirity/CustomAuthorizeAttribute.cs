using Application.Interface;
using System.Web.Mvc;
using System.Web.Routing;

namespace SgqSystem.Secirity
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //private readonly IUserApp iuserApp;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (string.IsNullOrEmpty(SessionPersister.Username))
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "UserAuthentication", action = "LogIn" }));
            else
            {
                CustomPrincipal cp = new CustomPrincipal(SessionPersister.Username);

                //if (!cp.IsInRole(Roles))
                //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccesDenied", action = "Index" }));

            }
        }
    }
}
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SgqSystem.Secirity
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string _filter;
        private string _userSgqRoles;

        public CustomAuthorizeAttribute()
        {
            _filter = this.Roles;
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            // I need to read cookie values here
            //Read the cookie from Request.
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get("webControlCookie");
            if (cookie == null)
            {
                //No cookie found or cookie expired.
                //Handle the situation here, Redirect the user or simply return;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "UserAuthentication", action = "LogIn" }));
            }
            else
            {
                if (!string.IsNullOrEmpty(cookie.Values["roles"]))
                    _userSgqRoles = cookie.Values["roles"].ToString();
                    //Extends cookie ttl
                    cookie.Expires = DateTime.Now.AddMinutes(60);
                //ok - cookie is found.
                //Gracefully check if the cookie has the key-value as expected.
                if (!string.IsNullOrEmpty(Roles))
                {
                    if (!string.IsNullOrEmpty(_userSgqRoles))
                    {
                        //Yes userId is found. Mission accomplished.
                        //CustomPrincipal cp = new CustomPrincipal(SessionPersister.Username);
                        if (!IsInRole(_userSgqRoles))
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccesDenied", action = "Index" }));

                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccesDenied", action = "Index" }));
                    }
                }

                if (!string.IsNullOrEmpty(_userSgqRoles))
                    if (_userSgqRoles.Contains("somentemanutencao-sgq") && !HttpContext.Current.Request.RawUrl.Contains("/Manutencao/Index"))
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Manutencao", action = "Index" }));
            }
          

        //if (string.IsNullOrEmpty(SessionPersister.Username))
        //else
        //{


        //}
    }

        /// <summary>
        /// Se o usuario não estiver nas roles ele não tera acesso = return false;
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });
            //return true;
            return roles.Any(r => Roles.Contains(r));
        }
    }
}
using DTO;
using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Helper
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string _filter;
        private string _userSgqRoles;
        private bool _isLogin;

        public CustomAuthorizeAttribute()
        {
            _filter = this.Roles;

        }

        public CustomAuthorizeAttribute(bool isLogin)
        {
            _isLogin = isLogin;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_isLogin)
                if (!GlobalConfig.VerifyConfig("DbContextSgqEUA"))
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "FirstConfig", action = "Index" }));

            if (!_isLogin)
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

                    var userId = 0;
                    if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                    {
                        int.TryParse(cookie.Values["userId"].ToString(), out userId);
                        filterContext.Controller.ViewBag.KeepAlive = "KeepAlive/" + userId;
                    }

                    #region Vrificação Senha Expirada

                    var dataSenhaUsuario = new DateTime();

                    if (!string.IsNullOrEmpty(cookie.Values["passwordDate"]))
                    {
                        dataSenhaUsuario = DateTime.ParseExact(cookie.Values["passwordDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else if (!string.IsNullOrEmpty(cookie.Values["alterDate"]))
                    {
                        dataSenhaUsuario = DateTime.ParseExact(cookie.Values["alterDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(cookie.Values["addDate"]))
                        {
                            dataSenhaUsuario = DateTime.ParseExact(cookie.Values["addDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                    }

                    TimeSpan timeSpan = DateTime.Today - dataSenhaUsuario;

                    TimeSpan timeSpanTwoMonths = dataSenhaUsuario.AddMonths(2) - dataSenhaUsuario;


                    if (timeSpan.Days >= timeSpanTwoMonths.Days)
                    {
                        UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
                        filterContext.Result = new RedirectResult(urlHelper.Action("Perfil", "UserSgq", new { motivo = "passwordExpired" }));
                        //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "UserSgq", action = "Perfil" }));
                    }

                    #endregion

                    else
                    {
                        if (!string.IsNullOrEmpty(cookie.Values["roles"]))
                        {
                            _userSgqRoles = cookie.Values["roles"].ToString();
                            filterContext.Controller.ViewBag.IsAdmin = VerificarRole("Admin");

                        }
                        else
                        {//NAO TEM ROLES
                            filterContext.Controller.ViewBag.IsAdmin = false;
                        }
                        //Extends cookie ttl
                        cookie.Expires = DateTime.Now.AddMinutes(60);
                        filterContext.HttpContext.Response.Cookies.Set(cookie);
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

                        filterContext.Controller.ViewBag.CompanyId = cookie.Values["CompanyId"].ToString();
                        Manutencao(filterContext);


                    }

                } 
            }

        }

        private void Manutencao(AuthorizationContext filterContext)
        {
            if (!string.IsNullOrEmpty(_userSgqRoles) && !Roles.Contains("somentemanutencao-sgq"))
                if (_userSgqRoles.Contains("somentemanutencao-sgq") && !HttpContext.Current.Request.RawUrl.Contains("ManPainelGestao/Index"))
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "ManPainelGestao", action = "Index" }));
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
            return roles.Any(r => Roles.ToLowerInvariant().Contains(r.ToLowerInvariant()));
        }

        protected bool VerificarRole(string role)
        {
            return _userSgqRoles.ToLowerInvariant().Contains(role.ToLowerInvariant());
        }
    }
}

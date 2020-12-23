﻿using Dominio;
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
        private string _controllerLogin;
        private string _actionLogin;

        public CustomAuthorizeAttribute()
        {
            _filter = this.Roles;

        }

        public CustomAuthorizeAttribute(bool isLogin)
        {
            _isLogin = isLogin;
        }

        public CustomAuthorizeAttribute(string controllerLogin, string actionLogin)
        {
            _controllerLogin = controllerLogin;
            _actionLogin = actionLogin;
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_isLogin)
                if (!GlobalConfig.VerifyConfig("DefaultConnection"))
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

                    if (!string.IsNullOrEmpty(_controllerLogin) && !string.IsNullOrEmpty(_actionLogin))
                    {
                        filterContext.HttpContext.Response.SetCookie(new HttpCookie("redirect") { Expires = DateTime.Now.AddMinutes(5), Value = filterContext.HttpContext.Request.Url.OriginalString });
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = _controllerLogin, action = _actionLogin }));
                    }
                    else
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "UserAuthentication", action = "LogIn" }));//Default SGQ
                }
                else
                {
                    using (var db = new SgqDbDevEntities())
                    {

                        var userId = 0;
                        if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                        {
                            int.TryParse(cookie.Values["userId"].ToString(), out userId);
                            filterContext.Controller.ViewBag.KeepAlive = "KeepAlive/" + userId;
                        }

                        #region Vrificação Senha Expirada

                        //var dataSenhaUsuario = new DateTime();

                        //if (!string.IsNullOrEmpty(cookie.Values["passwordDate"]))
                        //{
                        //    dataSenhaUsuario = DateTime.ParseExact(cookie.Values["passwordDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //}
                        //else if (!string.IsNullOrEmpty(cookie.Values["alterDate"]))
                        //{
                        //    dataSenhaUsuario = DateTime.ParseExact(cookie.Values["alterDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //}
                        //else
                        //{
                        //    if (!string.IsNullOrEmpty(cookie.Values["addDate"]))
                        //    {
                        //        dataSenhaUsuario = DateTime.ParseExact(cookie.Values["addDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //    }
                        //}

                        //TimeSpan timeSpan = DateTime.Today - dataSenhaUsuario;

                        //TimeSpan timeSpanTwoMonths = dataSenhaUsuario.AddMonths(2) - dataSenhaUsuario;

                        ////Mock Gabriel para parar de dar erro nas datas 2017-04-16

                        ////if (timeSpan.Days >= timeSpanTwoMonths.Days)

                        //if (1 >= 2)
                        //{
                        //    UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
                        //    filterContext.Result = new RedirectResult(urlHelper.Action("Perfil", "UserSgq", new { motivo = "passwordExpired" }));
                        //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "UserSgq", action = "Perfil" }));
                        //}

                        #endregion

                        if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                        {
                            _userSgqRoles = db.UserSgq.Find(int.Parse(cookie.Values["userId"])).Role.ToString();
                            filterContext.Controller.ViewBag.IsAdmin = VerificarRole("Admin");

                        }
                        else
                        {
                            filterContext.Controller.ViewBag.IsAdmin = false;
                        }

                        cookie.Expires = DateTime.Now.AddHours(48);
                        filterContext.HttpContext.Response.Cookies.Set(cookie);

                        if (!string.IsNullOrEmpty(Roles))
                        {
                            if (!string.IsNullOrEmpty(_userSgqRoles))
                            {

                                if (!IsInRole(_userSgqRoles))
                                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccesDenied", action = "Index" }));
                            }
                            else
                            {
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccesDenied", action = "Index" }));
                            }
                        }

                        filterContext.Controller.ViewBag.CompanyId = cookie.Values["CompanyId"].ToString();
                        filterContext.Controller.ViewBag.UserSgqId = userId;


                        var userSgq = db.UserSgq.Find(userId);
                        var linkedComapnyIds = db.ParCompanyXUserSgq.Where(c => c.UserSgq_Id == userId).Select(c => c.ParCompany_Id).ToList();
                        filterContext.Controller.ViewBag.LinkedCompanyIds = System.Web.Helpers.Json.Encode(linkedComapnyIds);


                        if (userSgq.ParCompanyXUserSgq.Any(x => x.Role != null))
                        {
                            filterContext.Controller.ViewBag.Roles = userSgq.Role.Replace(';', ',').ToString();
                        }
                        else
                        {
                            filterContext.Controller.ViewBag.Roles = "";
                        }

                        if (userSgq.ParCompanyXUserSgq != null && userSgq.ParCompanyXUserSgq.Any(r => r.Role != null))
                            if (userSgq.ParCompanyXUserSgq.Any(r => r.Role != null))
                                filterContext.Controller.ViewBag.RolesCompany = userSgq.ParCompanyXUserSgq.Select(n => n.Role).Distinct().ToArray();
                            else
                                filterContext.Controller.ViewBag.RolesCompany = userSgq.ParCompanyXUserSgq.Select(n => n.ParCompany_Id).Distinct().ToArray();

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

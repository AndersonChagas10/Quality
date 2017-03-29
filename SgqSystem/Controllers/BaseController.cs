﻿using DTO;
using DTO.DTO;
using System;
using System.Globalization;
using System.Linq;
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

        public void CreateCookieFromUserDTO(UserDTO isAuthorized)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(60);
                HttpContext.Response.Cookies.Set(cookie);
            }
            else
            {
                //create a cookie
                HttpCookie myCookie = new HttpCookie("webControlCookie");

                //Add key-values in the cookie
                myCookie.Values.Add("userId", isAuthorized.Id.ToString());
                myCookie.Values.Add("userName", isAuthorized.Name);

                if (isAuthorized.AlterDate != null)
                {
                    myCookie.Values.Add("alterDate", isAuthorized.AlterDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                }
                else
                {
                    myCookie.Values.Add("alterDate", "");
                }

                myCookie.Values.Add("addDate", isAuthorized.AddDate.ToString("dd/MM/yyyy"));

                if (isAuthorized.Role != null)
                    myCookie.Values.Add("roles", isAuthorized.Role.Replace(';', ',').ToString());//"admin, teste, operacional, 3666,344, 43434,...."
                else
                    myCookie.Values.Add("roles", "");

                if (isAuthorized.ParCompanyXUserSgq != null)
                    if (isAuthorized.ParCompanyXUserSgq.Any(r => r.Role != null))
                        myCookie.Values.Add("rolesCompany", string.Join(",", isAuthorized.ParCompanyXUserSgq.Select(n => n.Role).Distinct().ToArray()));
                    else
                        myCookie.Values.Add("rolesCompany", "");

                //set cookie expiry date-time. Made it to last for next 12 hours.
                myCookie.Expires = DateTime.Now.AddMinutes(60);

                //Most important, write the cookie to client.
                Response.Cookies.Add(myCookie);
            }
        }

      

    }

}
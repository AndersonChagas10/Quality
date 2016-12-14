﻿using Dominio;
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
            GlobalConfig.linkDataCollect = "http://mtzsvmqsc/AppColeta/";
            ViewBag.UrlDataCollect = GlobalConfig.linkDataCollect;
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
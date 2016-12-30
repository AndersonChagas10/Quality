﻿using Helper;
using SgqSystem.Secirity;
using System.Diagnostics;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [HandleController()]
    [CustomAuthorize]
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            ViewBag.Asembly = version;
            ViewBag.Title = "Sgq Global";
            //teste();
            return View();
        }

        //public void ChangeCulture(string language = "pt-BR")
        //{
        //    Response.Cookies.Remove("Language");
        //    HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

        //    if (languageCookie == null) languageCookie = new HttpCookie("Language");

        //    languageCookie.Value = language;

        //    languageCookie.Expires = DateTime.Now.AddDays(10);

        //    Response.SetCookie(languageCookie);

        //    Response.Redirect(Request.UrlReferrer.ToString());

        //}

        //public void teste()
        //{
        //    var obj = new CollectionLevel02DTO() { AddDate = DateTime.Now, Name = "aaa" };
        //    new CreateLog(new Exception("a"), obj, "Save Collection");
        //    new CreateLog(new Exception("a"));
        //}

    }
}

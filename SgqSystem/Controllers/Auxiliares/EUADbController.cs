using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Auxiliares
{
    public class EUADbController : BaseController
    {
        // GET: EUADb
        public ActionResult Index()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddHours(16);
                HttpContext.Response.Cookies.Set(cookie);
            }
            //Most important, write the cookie to client.
            Response.Cookies.Add(cookie);
            return View();
        }
    }
}
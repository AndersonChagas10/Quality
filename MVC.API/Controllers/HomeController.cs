using Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.API.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUserAppService _userAppService;

        public HomeController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }


        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}

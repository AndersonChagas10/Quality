using Application.Interface;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
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
           var user = new User(){ Name="teste", Password = "1234"};
           var teste = _userAppService.Autorizado(user);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
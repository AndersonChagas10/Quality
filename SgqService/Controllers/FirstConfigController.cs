using AutoMapper;
using Dominio;
using DTO;
using DTO.Helpers;
//using Microsoft.SqlServer.Management.Common;
//using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace SgqService.Controllers
{
    public class FirstConfigController : Controller
    {

        // GET: FirstConfig
        public ActionResult Index()
        {
            List<SelectListItem> listaProjetos = new List<SelectListItem>();
            listaProjetos.Add(new SelectListItem() { Text = "BRASIL", Value = "1" });
            listaProjetos.Add(new SelectListItem() { Text = "EUA", Value = "2" });
            listaProjetos.Add(new SelectListItem() { Text = "YTOARA", Value = "3" });

            ViewBag.ActiveIn = new SelectList(listaProjetos, "Value", "Text");

            return View();
        }


        // GET: FirstConfig
        [HttpPost]
        public ActionResult Index(FirstConfigDTO cfg)
        {
            GlobalConfig.ConfigWebSystem(cfg.SgqConfig);

            return RedirectToAction("Index", "Home");
        }

        public class FirstConfigDTO
        {
            public DTO.SgqConfig SgqConfig { get; set; }
            /*Wizard Inicial*/
            public UserSgq userSgqDto { get; set; }
            public ParCompany parCompanyDto { get; set; }
        }
    }
}
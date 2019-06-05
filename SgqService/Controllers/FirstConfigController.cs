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
            List<KeyValuePair<string, string>> listaProjetos = new List<KeyValuePair<string, string>>();
            listaProjetos.Add(new KeyValuePair<string, string>("BRASIL", "1" ));
            listaProjetos.Add(new KeyValuePair<string, string>("EUA",    "2" ));
            listaProjetos.Add(new KeyValuePair<string, string>("YTOARA", "3" ));

            ViewBag.ActiveIn = new SelectList(listaProjetos, "Value", "Key");

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
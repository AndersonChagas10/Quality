using Dominio.Interfaces.Services;
using DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class FirstConfigController : Controller
    {

        //private IBaseDomain< _defectDomain;

        //public FirstConfigController(IDefectDomain defectDomain)
        //{
        //    _defectDomain = defectDomain;
        //}

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
        public ActionResult Index(SgqConfig cfg)
        {

            using (var db = new ADOFactory.Factory("DbContextSgqEUA"))/*Caso nao configurado, procura config no DB*/
            {
                var result = db.InsertUpdateData(cfg);
                GlobalConfig.ConfigWebSystem(result);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
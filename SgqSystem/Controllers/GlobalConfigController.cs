using System.Collections.Generic;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{

    public class GlobalConfigController : Controller
    {
        // GET: GlobalConfig
        //[CustomAuthorize(Roles = "tato")]
        public ActionResult Config()
        {
            var listLinksDataCollect = new List<string>();
            listLinksDataCollect.Add("http://192.168.25.200/AppColeta/");
            listLinksDataCollect.Add("http://mtzsvmqsc/AppColeta/");

            ViewBag.linksDataCollect = listLinksDataCollect;
            return View();
        }
    }
}
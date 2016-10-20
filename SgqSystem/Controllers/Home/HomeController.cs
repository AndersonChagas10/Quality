using DTO;
using DTO.DTO;
using SgqSystem.Secirity;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class HomeController : Controller
    {

        [CustomAuthorize(Roles = "Admin")]
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

        //public void teste()
        //{
        //    var obj = new CollectionLevel02DTO() { AddDate = DateTime.Now, Name = "aaa" };
        //    new CreateLog(new Exception("a"), obj, "Save Collection");
        //    new CreateLog(new Exception("a"));
        //}

    }
}

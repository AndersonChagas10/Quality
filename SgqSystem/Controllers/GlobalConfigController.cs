using Dominio;
using DTO;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{

    public class GlobalConfigController : Controller
    {
        // GET: GlobalConfig
        //[CustomAuthorize(Roles = "tato")]
        public ActionResult Config()
        {
            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                ViewBag.unidades = db.ParCompany.ToList();
            }


                return View();
        }

        // GET: JobsMonitor
        public ActionResult Jobs()
        {
            ViewBag.UltimaExecucaoDoJob = GlobalConfig.UltimaExecucaoDoJob;
            return View();
        }


    }
}
using Dominio;
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


    }
}
using System.Web.Mvc;

namespace SgqSystem.Controllers
{

    public class GlobalConfigController : Controller
    {
        // GET: GlobalConfig
        //[CustomAuthorize(Roles = "tato")]
        public ActionResult Config()
        {
            return View();
        }
    }
}
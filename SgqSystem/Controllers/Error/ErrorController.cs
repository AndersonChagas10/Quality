using System.Web.Mvc;

namespace SgqSystem.Controllers.Error
{
    public class ErrorController : BaseController
    {
        
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.Title = "Error.";
            return View();
        }
    }
}
using System.Web.Mvc;

namespace SgqSystem.Controllers.Error
{
    public class ErrorController : Controller
    {
        
        // GET: Error
        public ActionResult Params()
        {
            ViewBag.Title = "Error in Params";
            return View();
        }
    }
}
using DTO.DTO.Params;
using Helper;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    public class ParamsController : Controller
    {
        // GET: Params
        [HandleController()]
        public ActionResult Index()
        {
            var teste = new ParLevel1DTO();
            return View(teste);
        }
    }
}
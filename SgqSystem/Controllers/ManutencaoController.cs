using SgqSystem.Secirity;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class ManutencaoController : BaseController
    {
        // GET: Manutencao
        public ActionResult Index()
        {
            return View();
        }
    }
}
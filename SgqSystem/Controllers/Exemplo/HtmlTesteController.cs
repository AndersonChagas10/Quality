using DTO.DTO;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Exemplo
{
    public class HtmlTesteController : Controller
    {
        [HttpPost]
        public ActionResult GetParLevel1ById()
        {
            return PartialView("_IndexTeste");
        }
    }
}
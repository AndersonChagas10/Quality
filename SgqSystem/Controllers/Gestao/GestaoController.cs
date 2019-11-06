using Dominio;
using Helper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class GestaoController : BaseController
    {
        // GET: Gestao
        public ActionResult Index()
        {
            var listaIntemMenu = new List<ItemMenu>();

            using (var db = new SgqDbDevEntities())
            {
                listaIntemMenu = db.ItemMenu.Where(x => x.IsActive == true && x.PDCAMenuItem != null).ToList();
            }

            return View(listaIntemMenu);
        }
    }
}
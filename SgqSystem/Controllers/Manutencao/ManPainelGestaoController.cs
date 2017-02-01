using Dominio;
using SgqSystem.Secirity;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Manutencao
{
    [CustomAuthorize]
    [OutputCache(Duration = 20, VaryByParam = "none")]
    public class ManPainelGestaoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult PainelGestaoManutencao()
        {
            return View();
        }


    }
}
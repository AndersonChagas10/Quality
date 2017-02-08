using Dominio;
using SgqSystem.Secirity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Manutencao
{
    [CustomAuthorize(Roles = "somentemanutencao-sgq")]
    [OutputCache(Duration = 20, VaryByParam = "none")]
    public class ManPainelGestaoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult PainelGestaoManutencao()
        {
            return View();
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult PainelIndicadoresUniManutencao()
        {
            List<Date> anos;

            string query = "SELECT DISTINCT ISNULL(YEAR(BASE_DATEREF),YEAR(BASE_DATEADD)) Ano FROM MANCOLETADADOS";

            anos = db.Database.SqlQuery<Date>(query).ToList();

            ViewBag.Anos = anos;

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }

    public class Date
    {
        public int Ano { get; set; }
    }

}
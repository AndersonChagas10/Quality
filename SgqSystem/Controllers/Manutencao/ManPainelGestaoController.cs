using Dominio;
using SgqSystem.Secirity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Helper;

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
            List<Regional> regionais;
            List<SubRegional> subRegionais;
            List<Unidade> unidades;

            string query = "SELECT DISTINCT ISNULL(YEAR(BASE_DATEREF),YEAR(BASE_DATEADD)) Ano FROM MANCOLETADADOS";
            anos = db.Database.SqlQuery<Date>(query).ToList();
            ViewBag.Anos = anos;

            query = "select Distinct EmpresaRegionalGrupo as Nome from DimManBaseUni";
            regionais = db.Database.SqlQuery<Regional>(query).ToList();
            ViewBag.Regionais = regionais;

            query = "select distinct EmpresaRegional as Nome, EmpresaRegionalGrupo as Grupo from DimManBaseUni";
            subRegionais = db.Database.SqlQuery<SubRegional>(query).ToList();
            ViewBag.SubRegionais = subRegionais;

            query = "SELECT DISTINCT par.Name as Nome, EmpresaRegional as Grupo FROM DimManBaseUni Uni LEFT JOIN parcompany Par on Uni.ParCompany_id = par.id where par.Name is not null";
            unidades = db.Database.SqlQuery<Unidade>(query).ToList();
            ViewBag.Unidades= unidades;

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

    public class Regional
    {
        public string Nome { get; set; }
    }

    public class SubRegional
    {
        public string Nome { get; set; }
        public string Grupo { get; set; }
    }

    public class Unidade
    {
        public string Nome { get; set; }
        public string Grupo { get; set; }

    }
}
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;
using Helper;

namespace SgqSystem.Controllers
{
    [CustomAuthorize(Roles = "somentemanutencao-sgq")]
    [FormularioPesquisa(filtraUnidadePorUsuario = true)]
    public class ManutencaoController : BaseController
    {
        #region Constructor

        private FormularioParaRelatorioViewModel form;
      

        public ManutencaoController()
        {
            form = new FormularioParaRelatorioViewModel();
        }

        #endregion

        // GET: Manutencao
        public ActionResult Index()
        {
            return View(form);
        }
    }
}
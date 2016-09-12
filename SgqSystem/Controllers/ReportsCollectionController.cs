using Application.Interface;
using DTO.DTO;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;
namespace SgqSystem.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ReportsCollectionController : Controller
    {

        private readonly IRelatorioColetaApp _relatorioColetaApp;
        private readonly IUserApp _userApp;

        public ReportsCollectionController(IRelatorioColetaApp relatorioColetaApp, IUserApp userApp)
        {
            _userApp = userApp;
            _relatorioColetaApp = relatorioColetaApp;
        }

        // GET: RelatorioColeta
        public ActionResult Index()
        {
            var form = new FormularioParaRelatorioViewModel();
            form.SetUsers(_userApp.GetAllUserValidationAd(new UserDTO()).Retorno);
            return View(form);
        }
    }
}
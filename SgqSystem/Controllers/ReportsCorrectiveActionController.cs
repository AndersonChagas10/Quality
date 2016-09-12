using Application.Interface;
using DTO.DTO;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ReportsCorrectiveActionController : Controller
    {
        private readonly IRelatorioColetaApp _relatorioColetaApp;
        private readonly IUserApp _userApp;

        public ReportsCorrectiveActionController(IRelatorioColetaApp relatorioColetaApp, IUserApp userApp)
        {
            _userApp = userApp;
            _relatorioColetaApp = relatorioColetaApp;
        }

        public ActionResult Index()
        {
            var form = new FormularioParaRelatorioViewModel();
            form.SetUsers(_userApp.GetAllUserValidationAd(new UserDTO()).Retorno);
            return View(form);
        }
    }
}
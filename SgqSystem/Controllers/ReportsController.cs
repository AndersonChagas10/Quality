using Application.Interface;
using DTO.DTO;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;
namespace SgqSystem.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ReportsController : Controller
    {

        #region Constructor

        private readonly IRelatorioColetaApp _relatorioColetaApp;
        private readonly IUserApp _userApp;

        public ReportsController(IRelatorioColetaApp relatorioColetaApp, IUserApp userApp)
        {
            _userApp = userApp;
            _relatorioColetaApp = relatorioColetaApp;
        }

        #endregion

        #region DataCollectionReport

        public ActionResult DataCollectionReport()
        {
            var form = new FormularioParaRelatorioViewModel();
            form.SetUsers(_userApp.GetAllUserValidationAd(new UserDTO()).Retorno);
            return View(form);
        }

        #endregion

        #region CorrectiveActionReport

        public ActionResult CorrectiveActionReport()
        {
            var form = new FormularioParaRelatorioViewModel();
            form.SetUsers(_userApp.GetAllUserValidationAd(new UserDTO()).Retorno);
            return View(form);
        }

        #endregion

        public ActionResult teste()
        {
            var form = new FormularioParaRelatorioViewModel();
            form.SetUsers(_userApp.GetAllUserValidationAd(new UserDTO()).Retorno);
            return View(form);
        }
    }
}
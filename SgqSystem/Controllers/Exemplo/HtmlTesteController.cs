using Dominio.Interfaces.Services;
using SgqSystem.ViewModels;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Exemplo
{
    public class HtmlTesteController : BaseController
    {

        #region Construtor

        private IParamsDomain _paramDomain;
        private ParamsViewModel ViewModel;

        public HtmlTesteController(IParamsDomain paramDomain)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            _paramDomain = paramDomain;
            ViewModel = new ParamsViewModel(_paramDomain.CarregaDropDownsParams());/*Cria view model vazio.*/
        }

        #endregion

        #region Get Tela Parametrizada

        [HttpPost]
        public ActionResult GetParLevel1ById()
        {
            ViewModel.paramsDto.collectionObject = _paramDomain.GetAllLevel1();

            /*Retorna View com Model ParLevel1 encontrado no DB.*/
            return PartialView("_IndexTeste", ViewModel);
        }

        #endregion

        //[HttpPost]
        //public ActionResult GetParLevel1ById()
        //{
        //    return PartialView("_IndexTeste");
        //}
    }
}
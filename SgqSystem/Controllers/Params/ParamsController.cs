using Dominio.Interfaces.Services;
using SgqSystem.ViewModels;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    //[HandleController()]
    public class ParamsController : Controller
    {

        #region Construtor

        private IParamsDomain _paramDomain;
        private ParamsViewModel ViewModel;

        public ParamsController(IParamsDomain paramDomain)
        {
            _paramDomain = paramDomain;
            ViewModel = new ParamsViewModel(_paramDomain.CarregaDropDownsParams());/*Cria view model vazio.*/
        }

        #endregion

        public ActionResult Index()
        {
            return View(ViewModel);
        }

        public ActionResult GetParLevel1ById(int id)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");

            if (id == -1) /*Retorna View Vazia*/
                return PartialView("_ParLevel1", ViewModel);

            var viewModelPreenchido = ViewModel;
            viewModelPreenchido.paramsDto.parLevel1Dto = _paramDomain.GetLevel1(id);
            /*Retorna View com Model ParLevel1 encontrado no DB.*/
            return PartialView("_ParLevel1",  viewModelPreenchido);
        }

        public ActionResult Index2()
        {
            return View(ViewModel);
        }

        public ActionResult Index3()
        {
            return View(ViewModel);
        }


    }
}
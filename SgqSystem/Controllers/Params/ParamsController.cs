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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");

            return View(ViewModel);
        }

        public ActionResult GetParLevel1ById(int id)
        {
            if (id == -1)/*Retorna View Vazia*/
                return PartialView("_ParLevel1", ViewModel);

            ViewModel.paramsDto.parLevel1Dto = _paramDomain.GetLevel1(id);
            /*Retorna View com Model ParLevel1 encontrado no DB.*/
            return PartialView("_ParLevel1", ViewModel);
        }

        [HttpGet]
        public ActionResult UpdateSelectLevel2(int id)
        {
            if (id == -1)/*Retorna View Vazia*/
                return PartialView("_SelectBoxLevel2", ViewModel);

            ViewModel.paramsDto.parLevel1Dto = _paramDomain.GetLevel1(id);
            return PartialView("_SelectBoxLevel2", ViewModel);
        }

        [HttpGet]
        public ActionResult UpdateSelectLevel3(int id)
        {
            if (id == -1)/*Retorna View Vazia*/
                return PartialView("_SelectBoxLevel3", ViewModel);

            ViewModel.paramsDto.parLevel2Dto = _paramDomain.GetLevel2(id);
            return PartialView("_SelectBoxLevel3", ViewModel);
        }

        public ActionResult GetParLevel2ById(int id)
        {
            if (id <= 0) /*Retorna View Vazia*/
                return PartialView("_ParLevel2", ViewModel);

            var viewModelPreenchido = ViewModel;
            viewModelPreenchido.paramsDto.parLevel2Dto = _paramDomain.GetLevel2(id);
            /*Retorna View com Model ParLevel2 encontrado no DB.*/
            return PartialView("_ParLevel2", viewModelPreenchido);
        }

        public ActionResult GetParLevel3ById(int id, int idParLevel2)
        {
            if (id <= 0) /*Retorna View Vazia*/
                return PartialView("_ParLevel3", ViewModel);

            var viewModelPreenchido = ViewModel;
            viewModelPreenchido.paramsDto.parLevel3Dto = _paramDomain.GetLevel3(id, idParLevel2);
            /*Retorna View com Model ParLevel3 encontrado no DB.*/
            return PartialView("_ParLevel3", viewModelPreenchido);
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
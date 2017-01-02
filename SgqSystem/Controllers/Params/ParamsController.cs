using Dominio.Interfaces.Services;
using Helper;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    [CustomAuthorize()]
    [HandleController()]
    public class ParamsController : BaseController
    {

        #region Construtor

        private IParamsDomain _paramDomain;
        private ParamsViewModel ViewModel { get; set; }

        public ParamsController(IParamsDomain paramDomain)
        {
            _paramDomain = paramDomain;
            if(ViewModel == null)
                ViewModel = new ParamsViewModel(_paramDomain.CarregaDropDownsParams());/*Cria view model vazio.*/
        }

        #endregion

        public ActionResult Index()
        {
            return View(ViewModel);
        }

        #region Get L1, L2 e L3

        public ActionResult GetParLevel1ById(int id)
        {
            ViewModel.levelControl = 1;
            if (id == -1)/*Retorna View Vazia*/
                return PartialView("_ParLevel1", ViewModel);

            ViewModel.paramsDto.parLevel1Dto = _paramDomain.GetLevel1(id);
            ViewModel.paramsDto.parLevel1Dto.listParLevel3Level2Level1Dto = null;
            return PartialView("_ParLevel1", ViewModel);/*Retorna View com Model ParLevel1 encontrado no DB.*/
        }

        public ActionResult GetParLevel2ById(int level2Id, int? level3Id)
        {
            ViewModel.levelControl = 2;
            if (level2Id <= 0) /*Retorna View Vazia*/
                return PartialView("_ParLevel2", ViewModel);

            var viewModelPreenchido = ViewModel;
            viewModelPreenchido.paramsDto = _paramDomain.GetLevel2(level2Id, level3Id);

            return PartialView("_ParLevel2", viewModelPreenchido);/*Retorna View com Model ParLevel2 encontrado no DB.*/
        }

        public ActionResult GetParLevel3ById(int id, int? idParLevel2 = 0)
        {
            ViewModel.levelControl = 3;
            if (id <= 0) /*Retorna View Vazia*/
                return PartialView("_ParLevel3", ViewModel);

            var viewModelPreenchido = ViewModel;
            viewModelPreenchido.paramsDto = _paramDomain.GetLevel3(id, idParLevel2);
            return PartialView("_ParLevel3", viewModelPreenchido); /*Retorna View com Model ParLevel3 encontrado no DB.*/
        }

        #endregion

        #region Update Select Box

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

            ViewModel.paramsDto = _paramDomain.GetLevel2(id);
            return PartialView("_SelectBoxLevel3", ViewModel);
        }

        #endregion
        
        #region Testes

        public ActionResult Index2()
        {
            return View(ViewModel);
        }

        public ActionResult Index3()
        {
            return View(ViewModel);
        }

        #endregion

        #region Get Tela Parametrizada

        public ActionResult GetCollectionLevel1()
        {
            ViewModel.paramsDto.collectionObject = _paramDomain.GetAllLevel1();

            /*Retorna View com Model ParLevel1 encontrado no DB.*/
            return View("_IndexTeste", ViewModel);
        }

        #endregion
    }
}
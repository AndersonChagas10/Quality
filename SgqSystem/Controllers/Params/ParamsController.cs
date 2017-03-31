using Dominio.Interfaces.Services;
using Helper;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    [CustomAuthorize]
    [HandleController()]
    public class ParamsController : BaseController
    {

        #region Construtor

        private IParamsDomain _paramDomain;
        private ParamsViewModel ViewModel { get; set; }

        public ParamsController(IParamsDomain paramDomain)
        {
            
            _paramDomain = paramDomain;
            if (ViewModel == null)
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
            //if (!GlobalConfig.Eua)
            //    return PartialView("_ParLevel1", ViewModel);
            //else
            //  return PartialView("Blank");

            ViewModel.paramsDto.parLevel1Dto = _paramDomain.GetLevel1(id);
            ViewModel.paramsDto.parLevel1Dto.listParLevel3Level2Level1Dto = null;

            if (ViewModel.paramsDto.parLevel1Dto.listParCounterXLocal != null)
                for (int i = 0; i < ViewModel.paramsDto.parLevel1Dto.listParCounterXLocal.Count; i++)
                {
                    ViewModel.paramsDto.parLevel1Dto.listParCounterXLocal[i].ParCounter.Name =
                        CommonData.getResource(ViewModel.paramsDto.parLevel1Dto.listParCounterXLocal[i].ParCounter.Name).Value.ToString();

                    ViewModel.paramsDto.parLevel1Dto.listParCounterXLocal[i].ParLocal.Name =
                        CommonData.getResource(ViewModel.paramsDto.parLevel1Dto.listParCounterXLocal[i].ParLocal.Name).Value.ToString();
                }


            return PartialView("_ParLevel1", ViewModel);/*Retorna View com Model ParLevel1 encontrado no DB.*/
        }

        public ActionResult GetParLevel2ById(int level2Id, int level3Id = 0, int level1Id = 0)
        {
            if (level2Id <= 0) /*Retorna View Vazia*/
                return PartialView("_ParLevel2", ViewModel);

            ViewModel.levelControl = 2;
            ViewModel.paramsDto = _paramDomain.GetLevel2(level2Id, level3Id, level1Id);

            if (ViewModel.paramsDto.parLevel2Dto.listParCounterXLocal != null)
                for (int i = 0; i < ViewModel.paramsDto.parLevel2Dto.listParCounterXLocal.Count; i++)
                {
                    ViewModel.paramsDto.parLevel2Dto.listParCounterXLocal[i].ParCounter.Name =
                        CommonData.getResource(ViewModel.paramsDto.parLevel2Dto.listParCounterXLocal[i].ParCounter.Name).Value.ToString();

                    ViewModel.paramsDto.parLevel2Dto.listParCounterXLocal[i].ParLocal.Name =
                        CommonData.getResource(ViewModel.paramsDto.parLevel2Dto.listParCounterXLocal[i].ParLocal.Name).Value.ToString();
                }


            return PartialView("_ParLevel2", ViewModel);/*Retorna View com Model ParLevel2 encontrado no DB.*/
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
        public ActionResult UpdateSelectLevel3(int id, int level1Id = 0)
        {
            if (id == -1)/*Retorna View Vazia*/
                return PartialView("_SelectBoxLevel3", ViewModel);

            ViewModel.paramsDto = _paramDomain.GetLevel2(id, 0, level1Id);
            return PartialView("_SelectBoxLevel3", ViewModel);
        }

        #endregion

        //#region Get Tela Parametrizada
        //public ActionResult GetCollectionLevel1()
        //{
        //    ViewModel.paramsDto.collectionObject = _paramDomain.GetAllLevel1();
        //    /*Retorna View com Model ParLevel1 encontrado no DB.*/
        //    return View("_IndexTeste", ViewModel);
        //}
        //#endregion

    }
}
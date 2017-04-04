using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using Dominio.Services;
using DTO;
using DTO.DTO;
using DTO.DTO.Params;
using Helper;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web;
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
        private IBaseDomain<ParHeaderField , ParHeaderFieldDTO> _parHeaderField;
        private IBaseDomain<ParMultipleValues, ParMultipleValuesDTO> _parMultipleValues;
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public ParamsController(IParamsDomain paramDomain, IBaseDomain<ParHeaderField, ParHeaderFieldDTO> parHeaderField, IBaseDomain<ParMultipleValues, ParMultipleValuesDTO> parmultiplevalues)
        {
            _paramDomain = paramDomain;
            _parHeaderField = parHeaderField;
            _parMultipleValues = parmultiplevalues;
            if (ViewModel == null)
                ViewModel = new ParamsViewModel(_paramDomain.CarregaDropDownsParams());/*Cria view model vazio.*/
        }

        #endregion

        public ActionResult Index()
        {
            ViewBag.Role = VerificarRole();
            return View(ViewModel);
        }

        private string VerificarRole()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");

            string _userSgqRoles = "";

            if (!string.IsNullOrEmpty(cookie.Values["roles"]))
            {
                _userSgqRoles = cookie.Values["roles"].ToString();
            }

            return _userSgqRoles;
        }

        #region Get L1, L2 e L3

        public ActionResult GetParLevel1ById(int id)
        {

            ViewBag.Role = VerificarRole();

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
            ViewBag.Role = VerificarRole();

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
            ViewBag.Role = VerificarRole();

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
        
        public ActionResult EditParHeaderField(int id)
        {
            ViewModel.paramsDto.parHeaderFieldDto = _parHeaderField.GetById(id);
            return View("EditParHeaderField" , ViewModel);
        }

        [HttpPost]
        public JsonResult AttParHeaderField(ParHeaderField parHF)
        {
            ParHeaderField par = parHF;
            //foreach (var i in h.parHeaderFieldDto.ParMultipleValues) {
            //    _parMultipleValues.AddOrUpdate(i);
            //}
            //_parHeaderField.AddOrUpdate(ViewModel.paramsDto.parHeaderFieldDto);
            //GetParLevel1ById(ViewModel.paramsDto.parLevel1Dto.Id);
            return new JsonResult();
        }
    }
    
}
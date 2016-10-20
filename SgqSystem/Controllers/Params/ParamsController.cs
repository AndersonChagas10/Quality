using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using Helper;
using SgqSystem.ViewModels;
using System;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    //[HandleController()]
    public class ParamsController : Controller
    {

        #region Construtor

        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParCluster, ParClusterDTO> _baseParCluster;
        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency;
        private IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType;
        private IBaseDomain<ParLevel1XCluster, ParLevel1XClusterDTO> _baseParLevel1XCluster;
        private IParamsDomain _paramDomain;
        private ParamsViewModel ViewModel;

        public ParamsController(
             IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1,
             IBaseDomain<ParFrequency, ParFrequencyDTO> baseParFrequency,
             IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> baseParConsolidationType,
             IBaseDomain<ParCluster, ParClusterDTO> baseParCluster,
             IBaseDomain<ParLevel1XCluster, ParLevel1XClusterDTO> baseParLevel1XCluster,
            IParamsDomain paramDomain
            )
        {
            _paramDomain = paramDomain;
            _baseParLevel1XCluster = baseParLevel1XCluster;
            _baseParLevel1 = baseParLevel1;
            _baseParFrequency = baseParFrequency;
            _baseParConsolidationType = baseParConsolidationType;
            _baseParCluster = baseParCluster;
            /*Construtor que carrega as DropDowns do Banco de dados para o View Model.*/
            ViewModel = new ParamsViewModel(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParCluster, _baseParLevel1XCluster);
        }

        #endregion

        //[HandleController()]
        public ActionResult Index()
        {
            //throw new Exception("teste");
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            return View(ViewModel);
        }

        //[HandleController()]
        public ActionResult GetParLevel1ById(int id)
        {
            if (id == 0) /*View Vazia*/
                return PartialView("_ParLevel1", ViewModel);
            else         /*Construtor que carrega as DropDowns do Banco de dados para o View Model e o Model ParLevel1.*/
                return PartialView("_ParLevel1", new ParamsViewModel(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParCluster, _baseParLevel1XCluster, _paramDomain.GetLevel1(id)));
        }

      
    }
}
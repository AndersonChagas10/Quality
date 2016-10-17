using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using Helper;
using SgqSystem.ViewModels;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Params
{
    [HandleController()]
    public class ParamsController : Controller
    {
        #region Construtor

        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParCluster, ParClusterDTO> _baseParCluster;
        //private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency;
        private IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType;
        private ParamsViewModel ViewModel;

        public ParamsController(
             IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1,
             IBaseDomain<ParFrequency, ParFrequencyDTO> baseParFrequency,
             IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> baseParConsolidationType,
             IBaseDomain<ParCluster, ParClusterDTO> baseParCluster
            )
        {
            _baseParLevel1 = baseParLevel1;
            _baseParFrequency = baseParFrequency;
            _baseParConsolidationType = baseParConsolidationType;
            _baseParCluster = baseParCluster;
            ViewModel = new ParamsViewModel(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParCluster);
        }

        #endregion

        // GET: Params
        public ActionResult Index()
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            return View(ViewModel);
        }

        public ActionResult GetParLevel1ById(int id)
        {
            if (id == 0)
                return PartialView("_ParLevel1", ViewModel);
            else
                return PartialView("_ParLevel1", new ParamsViewModel(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParLevel1.GetById(id)));
        }

        public ActionResult GetParLevel2ById(int id)
        {
            if (id == 0)
                return PartialView("_ParLevel2", ViewModel);
            else
                return PartialView("_ParLevel2", new ParamsViewModel(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParLevel1.GetById(id)));
        }

        public ActionResult GetParLevel3ById(int id)
        {
            if (id == 0)
                return PartialView("_ParLevel3", ViewModel);
            else
                return PartialView("_ParLevel3", new ParamsViewModel(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParLevel1.GetById(id)));
        }
    }
}
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace SgqSystem.Controllers.Globalization
{
    public class GlobalizationController : Controller
    {
        #region Construtor

        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParCluster, ParClusterDTO> _baseParCluster;
        //private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency;
        private IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType;
        private ParamsViewModel ViewModel;

        public GlobalizationController(
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

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        // GET: Globalization
        public ActionResult Index()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            return View(ViewModel);
        }

    }
}
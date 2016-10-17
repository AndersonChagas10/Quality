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
        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseParFrequency;
        private IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> _baseParConsolidationType;

        public ParamsController(IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1,
            IBaseDomain<ParFrequency, ParFrequencyDTO> baseParFrequency,
             IBaseDomain<ParConsolidationType, ParConsolidationTypeDTO> baseParConsolidationType)
        {
            _baseParLevel1 = baseParLevel1;
            _baseParFrequency = baseParFrequency;
            _baseParConsolidationType = baseParConsolidationType;
        }

        #endregion

        // GET: Params
        public ActionResult Index()
        {
            var teste = new ParamsViewModel(_baseParLevel1, _baseParFrequency, _baseParConsolidationType);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            return View(teste);
        }

        public ActionResult GetParLevel1ById(int id)
        {
            var ViewModel = new ParamsViewModel(_baseParLevel1, _baseParFrequency, _baseParConsolidationType, _baseParLevel1.GetById(id));
            var retorno = PartialView("_ParLevel1", ViewModel);
            return PartialView("_ParLevel1", ViewModel);
        }
        

    }
}
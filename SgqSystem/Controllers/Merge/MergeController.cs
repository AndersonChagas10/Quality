using Dominio.Interfaces.Services;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class MergeController : BaseController
    {

        #region Construtor e atributos

        public string UserSgqDTO { get; private set; }
        private readonly IGetConsolidateDataCollectionDomain _getConsolidateDataCollectionDomain;
        public ResultsDivViewModel ViewModel;

        public MergeController(IGetConsolidateDataCollectionDomain getConsolidateDataCollectionDomain)
        {
            _getConsolidateDataCollectionDomain = getConsolidateDataCollectionDomain;

            ViewModel = new ResultsDivViewModel()
            {
                consolidationLevel01 = _getConsolidateDataCollectionDomain.GetLastEntryToMerge()
            };

        }

        #endregion

        // GET: Merge
        public ActionResult DivResults()
        {
            return PartialView("_DivResults", ViewModel);
        }
    }
}
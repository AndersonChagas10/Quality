using Dominio.Interfaces.Services;
using SgqSystem.ViewModels;
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
            if (id == -1) /*Retorna View Vazia*/
                return PartialView("_ParLevel1", ViewModel);
            else         /*Retorna View com Model ParLevel1 encontrado no DB.*/
                return PartialView("_ParLevel1", new ParamsViewModel());
        }

      
    }
}
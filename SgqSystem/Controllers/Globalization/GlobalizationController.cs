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

        private IParamsDomain _paramDomain;
        private ParamsViewModel ViewModel;

        public GlobalizationController(IParamsDomain paramDomain)
        {
            _paramDomain = paramDomain;
            ViewModel = new ParamsViewModel(_paramDomain.CarregaDropDownsParams());/*Cria view model vazio.*/
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
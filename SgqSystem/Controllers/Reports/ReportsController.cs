using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.DTO.Params;
using Helper;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;


namespace SgqSystem.Controllers
{
    //techservices
    [CustomAuthorize]
    //[OutputCache(Duration = 20, VaryByParam = "none")]
    public class ReportsController : BaseController
    {

        #region Constructor
       
        #endregion
        [FormularioPesquisa(filtraUnidadeDoUsuario = true)]
        public ActionResult DataCollectionReport()
        {
            return View();
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult CorrectiveActionReport()
        {
            return View();
        }

        //public ActionResult CorrectiveActionReportDetails()
        //{
        //    using (var teste = new CorrectActApiController())
        //    {
        //        var model = teste.GetCorrectiveActionById(706);
        //        return View(model);
        //    }
        //}


    }
}
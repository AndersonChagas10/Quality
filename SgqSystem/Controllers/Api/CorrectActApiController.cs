using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/CorrectiveAction")]
    public class CorrectActApiController : ApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        //private readonly ICorrectiveActionDomain _correctiveActionAppService;
        //private readonly IUserDomain _userAppService;

        //public CorrectActApiController(ICorrectiveActionDomain correctiveActionAppService)//, IUserDomain userAppService)
        //{
        //    _correctiveActionAppService = correctiveActionAppService;
        //    _userAppService = userAppService;
        //}

        //[Route("SalvarAcaoCorretiva")]
        //[HttpPost]
        //public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva([FromBody]CorrectiveActionViewModel model)
        //{
        //    return _correctiveActionAppService.SalvarAcaoCorretiva(model);
        //}

        [Route("GetCorrectiveAction")]
        [HttpPost]
        public List<CorrectiveAction> GetCorrectiveAction([FromBody]FormularioParaRelatorioViewModel model)
        {

            //      var sql = "SELECT[Id] " +
            //",[AuditorId] " +
            //",[CollectionLevel02Id] " +
            //",[SlaughterId] " +
            //",[TechinicalId] " +
            //",[DateTimeSlaughter] " +
            //",[DateTimeTechinical] " +
            //",[AddDate] " +
            //",[AlterDate] " +
            //",[DateCorrectiveAction] " +
            //",[AuditStartTime] " +
            //",[DescriptionFailure] " +
            //",[ImmediateCorrectiveAction] " +
            //",[ProductDisposition] " +
            //",[PreventativeMeasure] " +
            //   "FROM[dbo].[CorrectiveAction]"; //.Where(r => r.AddDate >= model.startDate && r.AddDate >= model.endDate);

            //      var list = db.Database.SqlQuery<CorrectiveAction>(sql).ToList();

            var list = db.CorrectiveAction.ToList();

            return list;
            //return _correctiveActionAppService.GetCorrectiveAction(model);
        }

        //[Route("LoginSlaughterTechinical")]
        //[HttpPost]
        //public GenericReturn<UserDTO> LoginSlaughterTechinical([FromBody]UserViewModel model)
        //{
        //    var result = _userAppService.AuthenticationLogin(model);
        //    return result;
        //}

        //[Route("VerificarAcaoCorretivaIncompleta")]
        //[HttpPost]
        //public GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta([FromBody]CorrectiveActionViewModel model)
        //{
        //    var result = _correctiveActionAppService.VerificarAcaoCorretivaIncompleta(model);
        //    return result;
        //}

        //[Route("GetCorrectiveActionById")]
        //[HttpPost]
        //public GenericReturn<CorrectiveActionDTO> GetCorrectiveActionById([FromBody]int id)
        //{
        //    return _correctiveActionAppService.GetCorrectiveActionById(id);
        //}

    }
}

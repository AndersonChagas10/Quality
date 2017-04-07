using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System;
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

        public CorrectActApiController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        [Route("GetCorrectiveAction")]
        [HttpPost]
        public List<CorrectiveActionDTO> GetCorrectiveAction([FromBody]FormularioParaRelatorioViewModel model)
        {

            // var sql = "SELECT[Id] " +
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

            //List<CorrectiveAction> list = db.CorrectiveAction.ToList();
            //List<CorrectiveActionDTO> list2 = new List<CorrectiveActionDTO>();
            var list = db.CorrectiveAction.ToList();

            //list = list2;

            

            List<CorrectiveActionDTO> clienteView2 = new List<CorrectiveActionDTO>();
            CorrectiveActionDTO ca = new CorrectiveActionDTO();

            foreach (var item in list)
            {
                int leve1Id = 0;
                int leve2Id = 0;

                ca = Mapper.Map<CorrectiveAction, CorrectiveActionDTO>(item);
                ca.AuditorName = db.UserSgq.Where(r => r.Id == item.AuditorId).FirstOrDefault().Name;
                ca.NameSlaughter = db.UserSgq.Where(r => r.Id == item.AuditorId).FirstOrDefault().Name;
                ca.NameTechinical = db.UserSgq.Where(r => r.Id == item.AuditorId).FirstOrDefault().Name;
                ca.AuditorName = db.UserSgq.Where(r => r.Id == item.AuditorId).FirstOrDefault().Name;
                leve1Id = db.CollectionLevel2.Where(r => r.Id == item.CollectionLevel02Id).FirstOrDefault().ParLevel1_Id;
                leve2Id = db.CollectionLevel2.Where(r => r.Id == item.CollectionLevel02Id).FirstOrDefault().ParLevel2_Id;
                ca.level01Name = db.ParLevel1.Where(r => r.Id == leve1Id).FirstOrDefault().Name;
                ca.level02Name = db.ParLevel2.Where(r => r.Id == leve2Id).FirstOrDefault().Name;
                clienteView2.Add(ca);
            }

            return clienteView2;
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

using AutoMapper;
using Dominio;
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

        public CorrectActApiController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        [Route("GetCorrectiveAction")]
        [HttpPost]
        public List<CorrectiveActionDTO> GetCorrectiveAction([FromBody]FormularioParaRelatorioViewModel model)
        {

            var sql = "select " +
            " \n CorrectiveAction.id as Id, " +
            " \n CorrectiveAction.id as idcorrectiveaction, " +
            " \n UserSgqAuditor.Name as AuditorName, --join " +
            " \n UserSgqSlaugther.Name as NameSlaughter,--join " +
            " \n UserSgqTechinical.Name as NameTechinical, --join " +
            " \n ParLevel1.Name as level01Name, --join " +
            " \n ParLevel2.Name as level02Name, --join " +

            " \n DateTimeSlaughter, " +
            " \n DateTimeTechinical, " +
            " \n CorrectiveAction.AddDate, " +
            " \n CorrectiveAction.AlterDate, " +
            " \n DateCorrectiveAction, " +
            " \n AuditStartTime, " +
            " \n DescriptionFailure, " +
            " \n AuditStartTime, " +
            " \n ImmediateCorrectiveAction, " +
            " \n ProductDisposition, " +
            " \n PreventativeMeasure, " +
            " \n CollectionLevel02Id, " +
            " \n CollectionLevel2_Id " +

            " \n from CorrectiveAction " +

            " \n INNER JOIN CollectionLevel2 ON CorrectiveAction.CollectionLevel02Id = CollectionLevel2.Id " +
            " \n INNER JOIN UserSgq UserSgqAuditor on CorrectiveAction.AuditorId = UserSgqAuditor.Id " +
            " \n INNER JOIN UserSgq UserSgqSlaugther on CorrectiveAction.SlaughterId = UserSgqSlaugther.Id " +
            " \n INNER JOIN UserSgq UserSgqTechinical on CorrectiveAction.TechinicalId = UserSgqTechinical.Id " +
            " \n INNER JOIN ParLevel1 on CollectionLevel2.ParLevel1_Id = ParLevel1.Id " +
            " \n INNER JOIN ParLevel2 on CollectionLevel2.ParLevel2_Id = ParLevel2.Id " +

            " \n where " +

            " \n DateCorrectiveAction BETWEEN '" + model._dataInicioSQL + "' and '" + model._dataFimSQL + "' ";

            if (model.unitId > 0)
            {
                sql += " \n and CollectionLevel2.UnitId = " + model.unitId + " ";
            }

            if (model.shift > 0)
            {
                sql += " \n and CollectionLevel2.[Shift] = " + model.shift + " ";
            }

            if (model.period > 0)
            {
                sql += " \n and CollectionLevel2.Period = " + model.period + " ";
            }

            if (model.auditorId > 0)
            {
                sql += " \n and CorrectiveAction.AuditorId = " + model.auditorId + "";
            }

            var dados = db.Database.SqlQuery<CorrectiveActionDTO>(sql).ToList();

            //var list = new List<CorrectiveAction>();

            //if (model.unitId == 0)
            //{
            //    list = db.CorrectiveAction.Where(r => r.DateCorrectiveAction >= model._dataInicio)
            //    .Where(r => r.DateCorrectiveAction <= model._dataFim).ToList();
            //}
            //else
            //{
            //    var level2 = db.CollectionLevel2.Where(r => r.UnitId == model.unitId).ToList();

            //    foreach (var item in level2)
            //    {

            //        var corrective = db.CorrectiveAction
            //            .Where(r => r.DateCorrectiveAction >= model._dataInicio)
            //            .Where(r => r.DateCorrectiveAction <= model._dataFim)
            //            .Where(r => r.CollectionLevel02Id == item.Id).FirstOrDefault();

            //        if (corrective != null)
            //        {
            //            list.Add(corrective);
            //        }
            //    }
            //}

            //List<CorrectiveActionDTO> clienteView2 = new List<CorrectiveActionDTO>();
            //CorrectiveActionDTO ca = new CorrectiveActionDTO();

            //if (list == null)
            //    return clienteView2;

            //foreach (var item in list)
            //{
            //    int leve1Id = 0;
            //    int leve2Id = 0;

            //    ca = Mapper.Map<CorrectiveAction, CorrectiveActionDTO>(item);
            //    ca.AuditorName = db.UserSgq.Where(r => r.Id == item.AuditorId).FirstOrDefault().Name;
            //    ca.NameSlaughter = db.UserSgq.Where(r => r.Id == item.AuditorId).FirstOrDefault().Name;
            //    ca.NameTechinical = db.UserSgq.Where(r => r.Id == item.AuditorId).FirstOrDefault().Name;
            //    ca.AuditorName = db.UserSgq.Where(r => r.Id == item.AuditorId).FirstOrDefault().Name;
            //    leve1Id = db.CollectionLevel2.Where(r => r.Id == item.CollectionLevel02Id).FirstOrDefault().ParLevel1_Id;
            //    leve2Id = db.CollectionLevel2.Where(r => r.Id == item.CollectionLevel02Id).FirstOrDefault().ParLevel2_Id;
            //    ca.level01Name = db.ParLevel1.Where(r => r.Id == leve1Id).FirstOrDefault().Name;
            //    ca.level02Name = db.ParLevel2.Where(r => r.Id == leve2Id).FirstOrDefault().Name;
            //    clienteView2.Add(ca);
            //}

            //return clienteView2;

            return dados;
        }

        [Route("GetCorrectiveActionById")]
        [HttpPost]
        public CorrectiveActionDTO GetCorrectiveActionById([FromBody]int id)
        {
            int leve1Id = 0;
            int leve2Id = 0;
            Shift shift = new Shift();
            Period period = new Period();

            CorrectiveAction obj = db.CorrectiveAction.Where(r => r.Id == id).FirstOrDefault();
            CorrectiveActionDTO obj2 = Mapper.Map<CorrectiveAction, CorrectiveActionDTO>(obj);
            obj2.AuditorName = db.UserSgq.Where(r => r.Id == obj.AuditorId).FirstOrDefault().Name;
            obj2.NameSlaughter = db.UserSgq.Where(r => r.Id == obj.SlaughterId).FirstOrDefault().Name;
            obj2.NameTechinical = db.UserSgq.Where(r => r.Id == obj.TechinicalId).FirstOrDefault().Name;

            leve1Id = db.CollectionLevel2.Where(r => r.Id == obj.CollectionLevel02Id).FirstOrDefault().ParLevel1_Id;
            leve2Id = db.CollectionLevel2.Where(r => r.Id == obj.CollectionLevel02Id).FirstOrDefault().ParLevel2_Id;

            obj2.level01Name = db.ParLevel1.Where(r => r.Id == leve1Id).FirstOrDefault().Name;
            obj2.level02Name = db.ParLevel2.Where(r => r.Id == leve2Id).FirstOrDefault().Name;

            var level2 = db.CollectionLevel2.Where(r => r.Id == obj.CollectionLevel02Id).FirstOrDefault();
            var pc = db.ParCompany.Where(r => r.Id == level2.UnitId).FirstOrDefault();
            shift = db.Shift.Where(r => r.Id == level2.Shift).FirstOrDefault();
            period = db.Period.Where(r => r.Id == level2.Period).FirstOrDefault();

            if (shift.IsNull())
            {
                //shift.Id = 0;
                obj2.ShiftName = "";
            }
            else
            {
                //obj2.ShiftId = shift.Id;
                obj2.ShiftName = shift.Description;
            }

            if (period.IsNull())
            {
                //period.Id = 0;
                obj2.PeriodName = "";
            }
            else
            {
                //obj2.PeriodId = period.Id;
                obj2.PeriodName = period.Description;
            }

            Unit unit = new Unit();
            unit.Code = pc.SIF;
            unit.Name = pc.Name;
            UnitDTO unitDto = Mapper.Map<Unit, UnitDTO>(unit);

            obj2.Unit = unitDto;

            return obj2;
        }

    }
}

using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/AgendamentoAPI")]
    public class AgendamentoApiController : ApiController
    {
        [HttpPost]
        [Route("Post")]
        public ParEvaluationSchedule Post([FromBody] ParEvaluationSchedule agendamento)
        {
            //ValidaDados(agendamento);
            agendamento.Shift_Id = 1;
            agendamento.AddDate = DateTime.Now;
            agendamento.AlterDate = DateTime.Now;

            using (var db = new SgqDbDevEntities())
            {
                //agendamento.ParEvaluation_Id = db.ParEvaluation.Where(x => x.Id == agendamento.ParEvaluation_Id.Id).FirstOrDefault();
                db.ParEvaluationSchedule.Add(agendamento);
                db.SaveChanges();
            }
            return agendamento;
        }

        //private void ValidaDados(ParEvaluationSchedule agendamento)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

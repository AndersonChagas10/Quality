using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        [HttpGet]
        [Route("Get/{id}")]
        public List<ParEvaluationSchedule> Get(int id)
        {
            var db = new SgqDbDevEntities();
            db.Configuration.LazyLoadingEnabled = false;
            var agendamentos = db.ParEvaluationSchedule.Where(x => x.ParEvaluation_Id == id).ToList();         
            return agendamentos;
        }

        [HttpPost]
        [Route("Post")]
        public ParEvaluationSchedule Post([FromBody] ParEvaluationSchedule agendamento)
        {
            //ValidaDados(agendamento);
            agendamento.Shift_Id = 1;

            agendamento.IsActive = true;
            using (var db = new SgqDbDevEntities())
            {
                //http://localhost/SgqSystem/api/Shift/Get
                //agendamento.ParEvaluation_Id = db.ParEvaluation.Where(x => x.Id == agendamento.ParEvaluation_Id.Id).FirstOrDefault();
                db.ParEvaluationSchedule.Add(agendamento);
                db.SaveChanges();
            }
            return agendamento;
        }

        [HttpDelete]
        [Route("Delete")]
        public ParEvaluationSchedule Delete([FromBody] ParEvaluationSchedule agendamento)
        {
            agendamento.Shift_Id = 1;
            agendamento.IsActive = false;
            using (var db = new SgqDbDevEntities())
            {
                db.Entry(agendamento).State = EntityState.Modified;
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

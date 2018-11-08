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

            var agendamentos = db.ParEvaluationSchedule.Where(x => x.ParEvaluation_Id == id && x.IsActive).ToList();
            
            return agendamentos;
        }

        [HttpPost]
        [Route("Post")]
        public ParEvaluationSchedule Post([FromBody] ParEvaluationSchedule agendamento)
        {
            //ValidaDados(agendamento);
            agendamento.IsActive = true;
            
            using (var db = new SgqDbDevEntities())
            {
                if (agendamento.Id > 0)
                {
                    db.Entry(agendamento).State = EntityState.Modified;                   
                }
                else
                {
                    db.ParEvaluationSchedule.Add(agendamento);
                }
                db.SaveChanges();
            }
            return agendamento;
        }

        [HttpDelete]
        [Route("Delete")]
        public ParEvaluationSchedule Delete([FromBody] ParEvaluationSchedule agendamento)
        {   
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

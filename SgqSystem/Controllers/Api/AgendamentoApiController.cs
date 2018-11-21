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
        public List<ParEvaluationSchedule> Post([FromBody] List<ParEvaluationSchedule> listaAgendamento)
        {
            foreach (var item in listaAgendamento)
            {
                if (item.isDeletar && item.Id > 0)
                {
                    Delete(item);
                }
                else if(!item.isDeletar)
                {
                    item.IsActive = true;
                    if (item.Shift_Id <= 0)
                        item.Shift_Id = null;
                    using (var db = new SgqDbDevEntities())
                    {
                        if (item.Id > 0)
                        {
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            db.ParEvaluationSchedule.Add(item);
                        }
                        db.SaveChanges();
                    }
                }
            }
            return listaAgendamento.Where(x => x.isDeletar != true).ToList();
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
    }
}

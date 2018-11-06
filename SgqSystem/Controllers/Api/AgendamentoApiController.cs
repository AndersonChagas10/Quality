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
            
            using (var db = new SgqDbDevEntities())
            {
                //var avaliacao = db.ParEvaluation.Where(x => x.ParCompany_Id == agendamento.ParEvaluation.Id).ToList();

                db.ParEvaluationSchedule.Add(agendamento);
            }
            return null;
        }

        //private void ValidaDados(ParEvaluationSchedule agendamento)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

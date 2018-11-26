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
    public class ParInputTypeValuesApiController : ApiController
    {
        [HttpGet]
        [Route("Get/{id}")]
        public List<ParInputTypeValues> Get(int id)
        {
            var db = new SgqDbDevEntities();
            db.Configuration.LazyLoadingEnabled = false;

            var agendamentos = db.ParInputTypeValues.Where(x => x.IsActive).ToList();

            return agendamentos;
        }

        [HttpPost]
        [Route("Post")]
        public List<ParInputTypeValues> Post(List<ParInputTypeValues> listaAgendamento)
        {
            var db = new SgqDbDevEntities();
            db.Configuration.LazyLoadingEnabled = false;

            var agendamentos = db.ParInputTypeValues.Where(x => x.IsActive).ToList();

            return agendamentos;
        }
    }
}

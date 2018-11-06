using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    public class Agendamento
    {
        public int Id { get; set; }
        public int Avaliacao { get; set; }
        public string Turno { get; set; }
        public int Inicio { get; set; }
        public int Termino { get; set; }
    }

    [HandleApi()]
    [RoutePrefix("api/AgendamentoAPI")]
    public class AgendamentoApiController : ApiController
    {
        [HttpPost]
        [Route("Post")]
        public Agendamento Post([FromBody] Agendamento agendamento)
        {
            return null;
        }
    }
}

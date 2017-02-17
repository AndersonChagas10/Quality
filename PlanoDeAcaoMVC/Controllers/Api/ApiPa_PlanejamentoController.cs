using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Planejamento")]
    public class ApiPa_PlanejamentoController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IEnumerable<Pa_Planejamento> List()
        {
            return Pa_Planejamento.Listar();
        }

        [HttpGet]
        [Route("GET")]
        public Pa_Planejamento Get(int id)
        {
            return Pa_Planejamento.Get(id);
        }

        [HttpPost]
        [Route("Save")]
        public Pa_Planejamento Save([FromBody]Pa_Planejamento planejamento)
        {
            planejamento.AddOrUpdate();
            return planejamento;
        }
       
      
    }
}

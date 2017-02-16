using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Planejamento")]
    public class ApiPa_PlanejamentoController : ApiController
    {
        // GET: api/Pa_Planejamento
        public IEnumerable<Pa_Planejamento> Get()
        {
            return Pa_Planejamento.Listar();
        }

        // GET: api/Pa_Planejamento/5
        public Pa_Planejamento Get(int id)
        {
            return Pa_Planejamento.Get(id);
        }

        // POST: api/Pa_Planejamento
        public void Post([FromBody]Pa_Planejamento planejamento)
        {
            planejamento.AddOrUpdate();
        }

        // PUT: api/Pa_Planejamento/5
        public void Put(int id, [FromBody]Pa_Planejamento planejamento)
        {
            planejamento.AddOrUpdate();
        }
      
    }
}

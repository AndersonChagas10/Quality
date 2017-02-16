using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_ObjetivoGerais")]
    public class ApiPa_ObjetivoGeraisController : ApiController
    {
        // GET: api/Pa_ObjetivoGerais
        public IEnumerable<Pa_ObjetivoGerais> Get()
        {
            return Pa_ObjetivoGerais.Listar();
        }

        // GET: api/Pa_ObjetivoGerais/5
        public Pa_ObjetivoGerais Get(int id)
        {
            return Pa_ObjetivoGerais.Get(id);
        }

        // POST: api/Pa_ObjetivoGerais
        public void Post([FromBody]Pa_ObjetivoGerais objetivoGerais)
        {
            objetivoGerais.AddOrUpdate();
        }

      
        // DELETE: api/Pa_ObjetivoGerais/5
        public void Delete(int id)
        {
        }
    }
}

using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Indicadores")]
    public class ApiPa_IndicadoresController : ApiController
    {
        // GET: api/Pa_Indicadores
        public IEnumerable<Pa_Indicadores> Get()
        {
            return Pa_Indicadores.Listar();
        }

        // GET: api/Pa_Indicadores/5
        public Pa_Indicadores Get(int id)
        {
            return Pa_Indicadores.Get(id);
        }

        // POST: api/Pa_Indicadores
        public void Post([FromBody]Pa_Indicadores indicadores)
        {
            indicadores.AddOrUpdate();
        }

      
        // DELETE: api/Pa_Indicadores/5
        public void Delete(int id)
        {
        }
    }
}

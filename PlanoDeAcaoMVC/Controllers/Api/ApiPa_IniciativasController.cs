using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Iniciativas")]
    public class ApiPa_IniciativasController : ApiController
    {
        // GET: api/Pa_Iniciativas
        public IEnumerable<Pa_Iniciativas> Get()
        {
            return Pa_Iniciativas.Listar();
        }

        // GET: aapi/Pa_Iniciativas/5
        public Pa_Iniciativas Get(int id)
        {
            return Pa_Iniciativas.Get(id);
        }

        // POST: api/Pa_Iniciativas
        public void Post([FromBody]Pa_Iniciativas iniciativas)
        {
            iniciativas.AddOrUpdate();
        }


        // DELETE: api/ApiPa_Iniciativas/5
        public void Delete(int id)
        {
        }
    }
}

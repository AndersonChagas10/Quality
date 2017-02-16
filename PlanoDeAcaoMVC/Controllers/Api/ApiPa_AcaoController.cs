using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Acao")]
    public class ApiPa_AcaoController : ApiController
    {
        // GET: api/Pa_Acao
        public IEnumerable<Pa_Acao> Get()
        {
            return Pa_Acao.Listar();
        }

        // GET: api/Pa_Acao/5
        public Pa_Acao Get(int id)
        {
            return Pa_Acao.Get(id);
        }

        // POST: api/Pa_Acao
        public void Post([FromBody]Pa_Acao acao)
        {
            acao.AddOrUpdate();
        }

        // DELETE: api/Pa_Acao/5
        public void Delete(int id)
        {
        }
    }
}

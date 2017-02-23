using PlanoAcaoCore;
using System.Collections.Generic;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Acao")]
    public class ApiPa_AcaoController : ApiController
    {
        [HttpPost]
        [Route("GETCausaGenerica")]
        public IEnumerable<Pa_CausaGenerica> GETCausaGenerica()
        {
            return Pa_CausaGenerica.Listar();
        }

        [HttpPost]
        [Route("GETGrupoCausa/{id}")]
        public IEnumerable<Pa_GrupoCausa> GETGrupoCausa(int id)
        {
            return Pa_GrupoCausa.Listar();
        }

        [HttpPost]
        [Route("GETContramedidaGenerica/{id}")]
        public IEnumerable<Pa_ContramedidaGenerica> GETContramedidaGenerica(int id)
        {
            return Pa_ContramedidaGenerica.Listar();
        }


        [HttpGet]
        [Route("List")]
        public IEnumerable<Pa_Acao> List()
        {
            return Pa_Acao.Listar();
        }

        [HttpGet]
        [Route("GET")]
        public Pa_Acao Get(int id)
        {
            return Pa_Acao.Get(id);
        }

        [HttpPost]
        [Route("Save")]
        public Pa_Acao Save([FromBody] Pa_Acao planejamento)
        {
            planejamento.AddOrUpdate();
            return planejamento;
        }
    }
}

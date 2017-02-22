using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    public class PaPlanejamentoAcaoController : Controller
    {
        // GET: PaPlanejamentoAcao
        public ActionResult Index()
        {
            return View();
        }

        public IEnumerable<Pa_CausaGenerica> GETCausaGenerica()
        {
            return Pa_CausaGenerica.Listar();
        }

        public IEnumerable<Pa_GrupoCausa> GETGrupoCausa(int id)
        {
            return Pa_GrupoCausa.Listar();
        }

        public IEnumerable<Pa_ContramedidaGenerica> GETContramedidaGenerica(int id)
        {
            return Pa_ContramedidaGenerica.Listar();
        }

    }
}
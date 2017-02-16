using PlanoAcaoCore;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    //[CustomAuthorizeAttribute]
    public class Pa_PlanejamentoController : Controller
    {
        public Pa_PlanejamentoController()
        {
            ViewBag.Diretoria = Pa_Diretoria.Listar();
            ViewBag.Gerencia = Pa_Gerencia.Listar();
            ViewBag.Coordenacao = Pa_Coordenacao.Listar();
            ViewBag.Missao = Pa_Missao.Listar();
            ViewBag.Visao = Pa_Visao.Listar();
            ViewBag.TemaAssunto = Pa_TemaAssunto.Listar();
            ViewBag.Indicadores = Pa_Indicadores.Listar();
            ViewBag.Iniciativa = Pa_Iniciativas.Listar();
            ViewBag.ObjetivoGerencial = Pa_ObjetivoGerais.Listar();
        }

        // GET: Pa_Planejamento
        public ActionResult Index()
        {
            ViewBag.urlSend = Url.Action("Save", "api/Pa_Planejamento");
            ViewBag.urlList = Url.Action("List", "api/Pa_Planejamento");
            return PartialView();
        }

        public ActionResult Details(int? id = 0)
        {
            if (id > 0)
            {
                var model = Pa_Planejamento.Get(id.GetValueOrDefault());
                if(model != null)
                    return PartialView("Details", model);
            }

            return null;
        }

        public ActionResult Buscar()
        {
            return PartialView("Buscar");
        }
    }
}

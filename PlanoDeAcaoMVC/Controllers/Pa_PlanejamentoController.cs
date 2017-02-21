using PlanoAcaoCore;
using System.Linq;
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
            ViewBag.ObjetivoGerencial = Pa_ObjetivoGeral.Listar();
            ViewBag.Objetivo = Pa_Objetivo.Listar();
            ViewBag.Dimensao = Pa_Dimensao.Listar();
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

        [HttpPost]
        public ActionResult Filtrar(Pa_Planejamento filtro)
        {
            var lista = Pa_Planejamento.Listar();

            if (filtro.Diretoria_Id > 0)
                lista =  lista.Where(r => r.Diretoria_Id == filtro.Diretoria_Id).ToList();

            if (filtro.Gerencia_Id > 0)
                lista = lista.Where(r => r.Gerencia_Id == filtro.Gerencia_Id).ToList();

            if (filtro.Coordenacao_Id > 0)
                lista = lista.Where(r => r.Coordenacao_Id == filtro.Coordenacao_Id).ToList();

            if (filtro.TemaAssunto_Id > 0)
                lista = lista.Where(r => r.TemaAssunto_Id == filtro.TemaAssunto_Id).ToList();

            if (filtro.ObjetivoGerencial_Id > 0)
                lista = lista.Where(r => r.ObjetivoGerencial_Id == filtro.ObjetivoGerencial_Id).ToList();

            if (filtro.Indicadores_Id > 0)
                lista = lista.Where(r => r.Indicadores_Id == filtro.Indicadores_Id).ToList();

            ViewBag.Filtradas = lista;

            return PartialView("Filtrar");
        }
    }
}

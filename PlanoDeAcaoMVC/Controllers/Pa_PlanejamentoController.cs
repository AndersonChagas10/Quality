using PlanoAcaoCore;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    //[CustomAuthorizeAttribute]
    public class Pa_PlanejamentoController : Controller
    {
        public Pa_PlanejamentoController()
        {

            //select* from Pa_Dimensao
            //select* from[Pa_Objetivo]
            //select* from Pa_IndicadoresDiretriz
            ViewBag.Dimensao = Pa_Dimensao.Listar();
            ViewBag.Objetivo = Pa_Objetivo.Listar();
            ViewBag.IndicadoresDiretriz = Pa_IndicadoresDiretriz.Listar();

            ViewBag.Diretoria = Pa_Diretoria.Listar();
            ViewBag.Gerencia = Pa_Gerencia.Listar();
            ViewBag.Coordenacao = Pa_Coordenacao.Listar();
            ViewBag.Missao = Pa_Missao.Listar();
            ViewBag.Visao = Pa_Visao.Listar();
            ViewBag.Quem = Pa_Quem.Listar();
            ViewBag.TemaAssunto = Pa_TemaAssunto.Listar();
            ViewBag.IndicadoresDeProjeto = Pa_IndicadoresDeProjeto.Listar();
            ViewBag.Iniciativa = Pa_Iniciativas.Listar();
            ViewBag.ObjetivoGerencial = Pa_ObjetivoGeral.Listar();
            ViewBag.UnidadeMedida = Pa_UnidadeMedida.Listar();
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
                if (model != null)
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
            lista = lista.Where(r => r.Estrategico_Id.GetValueOrDefault() == 0 || r.Estrategico_Id == null).ToList();

            if (filtro.Diretoria_Id > 0)
                lista = lista.Where(r => r.Diretoria_Id == filtro.Diretoria_Id).ToList();

            if (filtro.Missao_Id > 0)
                lista = lista.Where(r => r.Missao_Id == filtro.Gerencia_Id).ToList();

            if (filtro.Visao_Id > 0)
                lista = lista.Where(r => r.Visao_Id == filtro.Coordenacao_Id).ToList();

            if (filtro.Dimensao_Id > 0)
                lista = lista.Where(r => r.Dimensao_Id == filtro.TemaAssunto_Id).ToList();

            if (filtro.Objetivo_Id > 0)
                lista = lista.Where(r => r.Objetivo_Id == filtro.ObjetivoGerencial_Id).ToList();

            if (filtro.IndicadoresDiretriz_Id > 0)
                lista = lista.Where(r => r.IndicadoresDiretriz_Id == filtro.IndicadoresDeProjeto_Id).ToList();

            if (filtro.Responsavel_Diretriz > 0)
                lista = lista.Where(r => r.Responsavel_Diretriz == filtro.IndicadoresDiretriz_Id).ToList();

            ViewBag.Filtradas = lista;

            return PartialView("Filtrar");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            //var model = Pa_Planejamento.Listar().FirstOrDefault();
            var model = Pa_Planejamento.Get(id);
            return PartialView("Index", model);
        }

        public ActionResult GETObjetivo(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "Objetivo_Id";

            var results = Pa_Objetivo.GetObjetivoXDimensao(id);
            if (results == null)
                results = new List<Pa_Objetivo>();


            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }

        public ActionResult GETIndicadoresDiretriz(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "IndicadoresDiretriz_Id";

            var results = Pa_IndicadoresDiretriz.GetIndicadoresDiretrizXObjetivo(id);
            if (results == null)
                results = new List<Pa_IndicadoresDiretriz>();

            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }

        public ActionResult GETIndicadoresProjetoIniciativa(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "IndicadoresDeProjeto_Id";

            var results = Pa_IndicadoresDeProjeto.GetIndicadoresProjetoXiniciativa(id);
            if (results == null)
                results = new List<Pa_IndicadoresDeProjeto>();

            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }

        public ActionResult GETObjetivosGerenciais(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "ObjetivoGerencial_Id";

            var results = Pa_ObjetivoGeral.GetObjetivoXIndicadoresProjeto(id);
            if (results == null)
                results = new List<Pa_ObjetivoGeral>();

            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }
    }
}

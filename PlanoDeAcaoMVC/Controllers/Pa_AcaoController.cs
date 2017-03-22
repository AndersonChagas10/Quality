using PlanoAcaoCore;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    [IntegraSgq]
    public class Pa_AcaoController : Controller
    {
        public Pa_AcaoController()
        {

            if (ViewBag.Unidade == null)
                ViewBag.Unidade = Pa_Unidade.Listar();

            if (ViewBag.Quem == null)
                ViewBag.Quem = Pa_Quem.Listar();

            ViewBag.Departamento = Pa_Departamento.Listar();
            ViewBag.CausaGenerica = Pa_CausaGenerica.Listar();
            ViewBag.GrupoCausa = Pa_GrupoCausa.Listar();
            ViewBag.ContramedidaGenerica = Pa_ContramedidaGenerica.Listar();
            ViewBag.Predecessora = Pa_Planejamento.Listar();
            ViewBag.Status = Pa_Status.Listar();
            ViewBag.Pa_IndicadorSgqAcao = Pa_IndicadorSgqAcao.Listar();
            ViewBag.Pa_Problema_Desvio = Pa_Problema_Desvio.Listar();
        }
        
        // GET: Pa_Acao
        public ActionResult Index()
        {
            //ViewBag.Panejamento = Pa_Planejamento
            return PartialView();
        }

        // GET: Pa_Acao
        public ActionResult Edit(int id)
        {
            var obj = Pa_Acao.Get(id);
            return PartialView("Index", obj);
        }

        public ActionResult GETGrupoCausa(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "CausaMedidasXAcao.GrupoCausa_Id";

            var results = Pa_GrupoCausa.GrupoCausaPorCausaGenerica(id);
            if (results == null)
                results = new List<Pa_GrupoCausa>();

            if (results.Count == 1)
                ViewBag.Ddl = new SelectList(results, "Id", "GrupoCausa", results.FirstOrDefault().Id);
            else
                ViewBag.Ddl = new SelectList(results, "Id", "GrupoCausa");

            return PartialView("_DdlGenerica");
        }

        public ActionResult GETContramedidaGenerica(int id)
        {
            if (id > 0)
                ViewBag.Disabled = true;
            else
                ViewBag.Disabled = false;

            ViewBag.DdlName = "CausaMedidasXAcao.ContramedidaGenerica_Id";

            var results = Pa_ContramedidaGenerica.ContramedidaGenericaPorGrupoCausa(id);
            if (results == null)
                results = new List<Pa_ContramedidaGenerica>();

            if (results.Count == 1)
                ViewBag.Ddl = new SelectList(results, "Id", "ContramedidaGenerica", results.FirstOrDefault().Id);
            else
                ViewBag.Ddl = new SelectList(results, "Id", "ContramedidaGenerica");
          
            return PartialView("_DdlGenerica");
        }
    }
}
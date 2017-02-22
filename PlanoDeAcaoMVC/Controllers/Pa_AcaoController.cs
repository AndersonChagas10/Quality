using PlanoAcaoCore;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    public class Pa_AcaoController : Controller
    {
        public Pa_AcaoController()
        {
            ViewBag.Unidade = Pa_Unidade.Listar();
            ViewBag.Departamento = Pa_Departamento.Listar();
            ViewBag.CausaGenerica = Pa_CausaGenerica.Listar();
            ViewBag.GrupoCausa = Pa_GrupoCausa.Listar();
            ViewBag.ContramedidaGenerica = Pa_ContramedidaGenerica.Listar();
            ViewBag.Quem = Pa_Quem.Listar();
            ViewBag.Predecessora = Pa_Planejamento.Listar();
            ViewBag.Status = Pa_Status.Listar();

        }
        
        // GET: Pa_Acao
        public ActionResult Index()
        {
            //ViewBag.Panejamento             = Pa_Planejamento
            return PartialView();
        }


    }
}
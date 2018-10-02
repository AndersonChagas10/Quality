using PlanoAcaoCore;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    [IntegraSgq]
    public class RelatoriosController : Controller
    {
        public RelatoriosController()
        {
            UpdateStatus();
        }
        // GET: Relatorios
        public ActionResult Relatorios()
        {
            return View();
        }

        protected void UpdateStatus()
        {
            using (var dbPa = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 1 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] = (5) AND  CONVERT (date ,QuandoFim) < CONVERT (date ,GETDATE()))");
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 9 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] in (5, 1) AND CONVERT(DATE, QuandoInicio) > CONVERT(DATE, GETDATE()))");
            }
        }
    }
}
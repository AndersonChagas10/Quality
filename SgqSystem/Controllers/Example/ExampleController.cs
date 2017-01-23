using Dominio;
using Dominio.ADO;
using Helper;
using SgqSystem.ViewModels;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    /// <summary>
    /// Controller para aprendizagem do sistema.
    /// </summary>
    [HandleController()]
    public class ExampleController : BaseController
    {
        public ActionResult Index()
        {
            using (var db = new FactoryADO(@"SERVERGRT\MSSQLSERVER2014", "SgqDbDev", "1qazmko0", "sa"))
            {
               var results = db.SearchQuery<UserSgq>("Select * from UserSgq");
            }

            ContextExampleViewModel pvm = new ContextExampleViewModel();
            return View(pvm);
        }

        public ActionResult TesteSelect2()
        {
            //using (var db = new FactoryADO(@"SERVERGRT\MSSQLSERVER2014", "SgqDbDev", "1qazmko0", "sa"))
            //{
            //    var results = db.SearchQuery<UserSgq>("Select * from UserSgq");
            //}

            return View();
        }

        [HttpGet]
        public ActionResult TiposDeInputSgq()
        {

            return View(new ContextExampleViewModel() { IntegerProp = 2, DecimalProp = 3.983M });
        }

        [HttpPost]
        public ActionResult TiposDeInputSgq(ContextExampleViewModel model)
        {
            return View(model);
        }

       
    }

    

}
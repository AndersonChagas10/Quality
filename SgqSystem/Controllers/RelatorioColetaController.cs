using Application.Interface;
using Dominio;
using System.Web.Mvc;
//Este controller não esta funcionando depois da migração de code first pra EDMX =)  Celso Géa 21 07 2016.
namespace SgqSystem.Controllers
{
    public class RelatorioColetaController : Controller
    {
        private readonly IAppServiceBase<Coleta> _serviceAppBase;
        private readonly IAppServiceBase<Level1> _opAppBase;
        private readonly IAppServiceBase<Level2> _monAppBase;
        private readonly IAppServiceBase<Level3> _tarAppBase;

        public RelatorioColetaController(IAppServiceBase<Coleta> serviceAppBase,
            IAppServiceBase<Level1> opAppBase,
            IAppServiceBase<Level2> monAppBase,
            IAppServiceBase<Level3> tarAppBase
            )
        {
            _serviceAppBase = serviceAppBase;
            _opAppBase = opAppBase;
            _monAppBase = monAppBase;
            _tarAppBase = tarAppBase;
        }
        // GET: RelatorioColeta
        public ActionResult Index()
        {
            var resultadosLista = _serviceAppBase.GetAll();

            foreach (var i in resultadosLista)
            {
                //i.Level1 = _opAppBase.GetById(i.Id_Level1).Name;
                //i.Level2 = _monAppBase.GetById(i.Id_Level2).Name;
                //i.Level3 = _tarAppBase.GetById(i.Id_Level3).Name;
            }

            return View(resultadosLista);
        }
    }
}
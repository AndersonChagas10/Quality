using Application.Interface;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class RelatorioColetaController : Controller
    {
        private readonly IAppServiceBase<ResultOld> _serviceAppBase;
        private readonly IAppServiceBase<Operacao> _opAppBase;
        private readonly IAppServiceBase<Monitoramento> _monAppBase;
        private readonly IAppServiceBase<Tarefa> _tarAppBase;

        public RelatorioColetaController(IAppServiceBase<ResultOld> serviceAppBase,
            IAppServiceBase<Operacao> opAppBase,
            IAppServiceBase<Monitoramento> monAppBase,
            IAppServiceBase<Tarefa> tarAppBase
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
                i.Operacao = _opAppBase.GetById(i.Id_Operacao).Name;
                i.Monitoramento = _monAppBase.GetById(i.Id_Monitoramento).Name;
                i.Tarefa = _tarAppBase.GetById(i.Id_Tarefa).Name;
            }

            return View(resultadosLista);
        }
    }
}
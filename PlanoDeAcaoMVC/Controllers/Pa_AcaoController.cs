using DTO.DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using PlanoAcaoCore;
using PlanoAcaoCore.Acao;
using System;
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
            //return PartialView("Index", obj);
            return PartialView("Edit", obj);
        }

        public ActionResult GETGrupoCausa(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "GrupoCausa_Id";

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

            ViewBag.DdlName = "ContramedidaGenerica_Id";

            var results = Pa_ContramedidaGenerica.ContramedidaGenericaPorGrupoCausa(id);
            if (results == null)
                results = new List<Pa_ContramedidaGenerica>();

            if (results.Count == 1)
                ViewBag.Ddl = new SelectList(results, "Id", "ContramedidaGenerica", results.FirstOrDefault().Id);
            else
                ViewBag.Ddl = new SelectList(results, "Id", "ContramedidaGenerica");
          
            return PartialView("_DdlGenerica");
        }

        public ActionResult Acompanhamento(int id)
        {
            var obj = Pa_Acao.Get(id);
            return PartialView("Acompanhamento", obj);
        }

        [HttpGet]
        public ActionResult FTAMock()
        {
            var fta = new FTA();
            fta._DataInicioFTA = "08/05/2017";
            fta._DataFimFTA = "08/05/2017";

            fta._Unidade = "Corporativo";
            fta._Departamento = "Curral";
            fta._Supervisor = "camilaprata-mtz";
            fta._Level1 = "(%) NC Expedição";
            fta.MetaFTA = 5;
            fta.ReincidenciaDesvioFTA = 15;
            fta.PercentualNCFTA = 15;

            return View("NewFTA", fta);

        }

        //Pa_Acao/NewFTA?MetaFTA=30&PercentualNCFTA=40&ReincidenciaDesvioFTA=60&Level1Id=1&Supervisor_Id=10&Unidade_Id=3&Departamento_Id=4&_DataInicioFTA="22-05-2017"&_DataFimFTA="22-05-2017"
        [HttpGet]
        public ActionResult NewFTA(FTA fta)
        {
            #region MOCK

            /*Recebe do AJAX*/
            //Guard.ParseDateToSqlV2(fta._DataInicioFTA);
            fta._DataInicioFTA = Guard.ParseDateToSqlV2(fta._DataInicioFTA).ToShortDateString();
            fta._DataFimFTA = Guard.ParseDateToSqlV2(fta._DataFimFTA).ToShortDateString();
            //fta.MetaFTA = 40;
            //fta.PercentualNCFTA = 60;
            //fta.ReincidenciaDesvioFTA = 50;
            //fta.Level1Id = 8;
            //fta.Supervisor_Id = 8;
            //fta.Unidade_Id = 1;
            //fta.Departamento_Id = 1;
            
            /*Preenche na tela*/
            //fta.ContramedidaGenerica_Id = 1;
            //fta.ContramedidaEspecifica = "ContramedidaEspecifica TESTE";
            //fta.Quem_Id = 5;
            //fta.QuandoInicio = DateTime.Now;
            //fta.QuandoFim = DateTime.Now;
            //fta.ComoPontosimportantes = "ComoPontosimportantes TESTE";

            #endregion

            fta.ValidaFTA();



            using (var db = new ADOFactory.Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
            {

                var level1 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel1 WHERE ID = " + fta.Level1Id).FirstOrDefault(r => r.IsActive);
                var usersgq = db.SearchQuery<UserDTO>("Select * from usersgq WHERE ID = " + fta.Supervisor_Id).FirstOrDefault();
                var parcompany = db.SearchQuery<ParCompanyDTO>("Select * from parcompany WHERE ID = " + fta.Unidade_Id).FirstOrDefault(r => r.IsActive);
                var parDepartment = db.SearchQuery<ParDepartmentDTO>("Select * from ParDepartment WHERE ID = " + fta.Departamento_Id).FirstOrDefault();

                fta._Level1 = level1.Name;

                if (fta.Unidade_Id > 0)
                {
                    fta._Unidade = parcompany.Name;
                }
                else
                {
                    fta._Unidade = "Corporativo";
                }

                fta._Departamento = parDepartment.Name;
                fta._Supervisor = usersgq.Name;

            }

            //fta._Unidade = "Corporativo";
            //fta._Departamento = "Curral";
            //fta._Supervisor = "camilaprata-mtz";
            //fta._Level1 = "(%) NC Expedição";
            //fta.MetaFTA = 5;
            //fta.ReincidenciaDesvioFTA = 15;
            //fta.PercentualNCFTA = 15;



            return View(fta);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {           
            var obj = Pa_Acao.Get(id);
            return PartialView("Details", obj);       
        }

    }
}
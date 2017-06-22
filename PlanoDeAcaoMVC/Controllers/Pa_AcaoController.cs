using AutoMapper;
using DTO.DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using PlanoAcaoCore.Acao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    [IntegraSgq]
    public class Pa_AcaoController : Controller
    {

        PlanoAcaoEF.PlanoDeAcaoEntities db;

        /// <summary>
        /// Construtor Com drop down lists para views e partial de Ações
        /// </summary>
        public Pa_AcaoController()
        {
            db = new PlanoAcaoEF.PlanoDeAcaoEntities();
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

        #region Ações

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //ViewBag.Panejamento = Pa_Planejamento
            return PartialView();
        }

        /// <summary>
        /// Get Ação
        /// </summary>
        /// <param name="id">Id Ação</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var obj = Pa_Acao.Get(id);
            //return PartialView("Index", obj);
            return PartialView("Edit", obj);
        }

        /// <summary>
        /// DEtalhes da Ação.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details(int id)
        {
            var obj = Pa_Acao.Get(id);
            return PartialView("Details", obj);
        }

        /// <summary>
        /// DDL Dinamica Grupo Causa
        /// </summary>
        /// <param name="id">Id Causa Genérica</param>
        /// <returns></returns>
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

        /// <summary>
        /// DDL Dinamica Contramedida Genérica
        /// </summary>
        /// <param name="id">Id Causa Genérica</param>
        /// <returns></returns>
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

        /// <summary>
        /// Busca Ação e retorna View de Acompanhamento da Ação.
        /// </summary>
        /// <param name="id">ID da Ação</param>
        /// <returns></returns>
        public ActionResult Acompanhamento(int id)
        {
            var obj = db.Pa_Acao.FirstOrDefault(r => r.Id == id);
            return PartialView("Acompanhamento", Mapper.Map<Pa_Acao>(obj));
        }

        #endregion

        #region FTA

        /// <summary>
        /// FTA Original
        /// </summary>
        /// <param name="fta">
        /// Formulario de tratamento de anomalia propriedades:
        /// 
        /// MetaFTA
        /// PercentualNCFTA
        /// ReincidenciaDesvioFTA
        /// Level1Id
        /// Supervisor_Id
        /// Unidade_Id (Se 0 considera-se corporativo)
        /// Departamento_Id
        /// _DataInicioFTA
        /// _DataFimFTA
        /// 
        /// GET: Pa_Acao/NewFTA?MetaFTA=30&PercentualNCFTA=40&ReincidenciaDesvioFTA=60&Level1Id=1&Supervisor_Id=10&Unidade_Id=3&Departamento_Id=4&_DataInicioFTA="22-05-2017"&_DataFimFTA="22-05-2017"
        /// </param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewFTA(FTA fta)
        {
            #region MOCK
            fta._DataInicioFTA = Guard.ParseDateToSqlV2(fta._DataInicioFTA).ToShortDateString();
            fta._DataFimFTA = Guard.ParseDateToSqlV2(fta._DataFimFTA).ToShortDateString();
            #endregion

            ViewBag.PlanejamentosComFTA = db.Pa_Planejamento.FirstOrDefault(r=>r.IsFta == true).Id;
            fta.ValidaFTA();
            NovoFtaModelParaSgq(fta);

            return View(fta);
        }

        private static void NovoFtaModelParaSgq(FTA fta)
        {
            Guard.CheckStringFullSimple(fta._Level1, "_Level1");
            Guard.CheckStringFullSimple(fta._Level2, "_Level2");
            Guard.CheckStringFullSimple(fta._Level3, "_Level3");
            Guard.ForValidId(fta.Supervisor_Id, "NovoFtaModelParaSgq");

            using (var dbFActory = new ADOFactory.Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
            {

                var level1 = dbFActory.SearchQuery<ParLevel1DTO>("Select * from parlevel1 WHERE Name = '" + fta._Level1 + "'").FirstOrDefault(r => r.IsActive);
                var level2 = dbFActory.SearchQuery<ParLevel2DTO>("Select * from parlevel2 WHERE Name = '" + fta._Level2 + "'").FirstOrDefault(r => r.IsActive);
                var level3 = dbFActory.SearchQuery<ParLevel3DTO>("Select * from parlevel3 WHERE Name = '" + fta._Level3 + "'").FirstOrDefault(r => r.IsActive);
                var usersgq = dbFActory.SearchQuery<UserDTO>("Select * from usersgq WHERE ID = " + fta.Supervisor_Id).FirstOrDefault();
                var parcompany = dbFActory.SearchQuery<ParCompanyDTO>("Select * from parcompany WHERE ID = " + fta.Unidade_Id).FirstOrDefault(r => r.IsActive);
                var parDepartment = dbFActory.SearchQuery<ParDepartmentDTO>("Select * from ParDepartment WHERE ID = " + level2.ParDepartment_Id).FirstOrDefault();


                if (fta.Unidade_Id > 0)
                {
                    fta._Unidade = parcompany.Name;
                }
                else
                {
                    fta._Unidade = "Corporativo";
                }

                var dtInit = Guard.ParseDateToSqlV2(fta._DataInicioFTA).ToString("yyyyMMdd");
                var dtEnd = Guard.ParseDateToSqlV2(fta._DataFimFTA).ToString("yyyyMMdd");

                var metaQuery = "SELECT ROUND(CASE" +
    "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = "+ level1 .Id + " AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) AND G.AddDate <= '"+ dtEnd + " 23:59:59') > 0 THEN  " +
    "\n     (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = " + level1.Id + "  AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) AND G.AddDate <= '" + dtEnd + " 23:59:59' ORDER BY G.ParCompany_Id DESC, AddDate DESC)" +
    "\n     ELSE    " +
    "\n     (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = " + level1.Id + "  AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)  " +
    "\n  END,2) " +
    "\n  AS META";

                fta._Level1 = level1.Name;
                fta._Departamento = parDepartment.Name;
                fta.Departamento_Id = parDepartment.Id;
                fta.Level1Id = level1.Id;
                fta.Level2Id = level2.Id;
                fta.Level3Id = level3.Id;
                fta.MetaFTA += " %";
                fta.PercentualNCFTA = level2.Name + " > " + level3.Name + ": " + fta.PercentualNCFTA + " %";
                fta.ReincidenciaDesvioFTA = level2.Name + " > " + level3.Name + ": " + fta.ReincidenciaDesvioFTA;
                fta._Supervisor = usersgq.Name;
                dynamic meta = dbFActory.QueryNinjaADO(metaQuery).FirstOrDefault();
                fta.MetaFTA = meta.META;


            }
        }

        /// <summary>
        /// Mock para apresentação do plano de ação.
        /// </summary>
        /// <returns></returns>
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
            fta.MetaFTA = "Teste > Teste :5";
            fta.ReincidenciaDesvioFTA = "Teste > Teste :15";
            fta.PercentualNCFTA = "Teste > Teste :15";

            return View("NewFTA", fta);

        }

        #endregion

    }
}
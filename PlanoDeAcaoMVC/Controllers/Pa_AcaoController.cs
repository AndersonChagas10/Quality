﻿using ADOFactory;
using AutoMapper;
using DTO.DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using PlanoAcaoCore.Acao;
using PlanoAcaoCore.Enum;
using PlanoDeAcaoMVC.Controllers.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    [IntegraSgq]
    public class Pa_AcaoController : Controller
    {

        Dominio.SgqDbDevEntities db;

        /// <summary>
        /// Construtor Com drop down lists para views e partial de Ações
        /// </summary>
        public Pa_AcaoController()
        {
            db = new Dominio.SgqDbDevEntities();
            if (ViewBag.Unidade == null)
                ViewBag.Unidade = Pa_Unidade.Listar();

            if (ViewBag.Quem == null)
                ViewBag.Quem = Pa_Quem.Listar().OrderBy(r => r.Name);

            ViewBag.Departamento = Pa_Departamento.Listar();
            ViewBag.CausaGenerica = Pa_CausaGenerica.Listar();
            ViewBag.GrupoCausa = Pa_GrupoCausa.Listar();
            ViewBag.ContramedidaGenerica = Pa_ContramedidaGenerica.Listar();
            ViewBag.Predecessora = Pa_Planejamento.Listar();

            var Status = Pa_Status.Listar();

            ViewBag.Status = Status;
            ViewBag.Status2 = GetStatusAcompanhamento(Status);
            ViewBag.Pa_IndicadorSgqAcao = Pa_IndicadorSgqAcao.Listar();
            ViewBag.Pa_Problema_Desvio = Pa_Problema_Desvio.Listar();

            ViewBag.UnidadeMedida = Pa_UnidadeMedida.Listar();
        }

        private static IEnumerable<Pa_Status> GetStatusAcompanhamento(IEnumerable<Pa_Status> Status)
        {
            int[] statusAcompanhamento = { (int)Enums.Status.Cancelado, (int)Enums.Status.Concluido, (int)Enums.Status.Aberto, (int)Enums.Status.Finalizada };

            Status = Status.Where(r => statusAcompanhamento.Contains(r.Id));

            return Status;
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
            Pa_Acao model;
            using (var dbADO = ConexaoADO())
            {
                //dynamic obj2 = dbADO.QueryNinjaADO("select * from pa_acao where id = " + id).FirstOrDefault();

                //int quemId = obj2.Quem_Id;
                //var obj = db.Pa_Acao.FirstOrDefault(r => r.Id == id);
                //model = Mapper.Map<Pa_Acao>(obj);
                //model.Quem_Id = obj2.Quem_Id;

                model = Pa_Acao.Get(id);

                if (model.Quem_Id > 0)
                {
                    dynamic quem = dbADO.QueryNinjaADO("select * from pa_quem where id = " + model.Quem_Id).FirstOrDefault();
                    model._Quem = quem.Name;
                }

            }
            //var obj = db.Pa_Acao.FirstOrDefault(r => r.Id == id);
            //var model = Mapper.Map<Pa_Acao>(obj);

            return PartialView("Acompanhamento", model);
        }

        protected Factory ConexaoADO()
        {
            return new Factory(Conn.dataSource, Conn.catalog, Conn.pass, Conn.user);
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
            ViewBag.PlanejamentosComFTA = fta.Panejamento_Id;
            //ViewBag.PlanejamentosComFTA = GetPlanejamentoFTAId();
            fta.ValidaFTA();
            NovoFtaModelParaSgq(fta);
            fta.IsFTA = true;
            return View(fta);
        }

        private static void NovoFtaModelParaSgq(FTA fta)
        {
            Guard.CheckStringFullSimple(fta._Level1, "_Level1");
            Guard.CheckStringFullSimple(fta._Level2, "_Level2");
            Guard.CheckStringFullSimple(fta._Level3, "_Level3");
            Guard.CheckStringFullSimple(fta._DataInicioFTA, "_DataInicioFTA");
            Guard.CheckStringFullSimple(fta._DataFimFTA, "_DataFimFTA");
            Guard.ForValidId(fta.Supervisor_Id, "NovoFtaModelParaSgq");
            //fta._DataInicioFTA = Guard.ParseDateToSqlV2(fta._DataInicioFTA).ToShortDateString();
            //fta._DataFimFTA = Guard.ParseDateToSqlV2(fta._DataFimFTA).ToShortDateString();

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
    "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = " + level1.Id + " AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= '" + dtEnd + " 23:59:59') > 0 THEN  " +
    "\n     (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = " + level1.Id + "  AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= '" + dtEnd + " 23:59:59' ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)" +
    "\n     ELSE    " +
    "\n     (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = " + level1.Id + "  AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= '" + dtEnd + " 23:59:59' ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)  " +
    "\n  END,2) " +
    "\n  AS META";

                fta._Level1 = level1.Name;
                fta._Departamento = parDepartment.Name;
                fta.Departamento_Id = parDepartment.Id;
                fta.Level1Id = level1.Id;
                fta.Level2Id = level2.Id;
                fta.Level3Id = level3.Id;
                fta.MetaFTA += " %";
                var PercentualNCFTA2f = decimal.Round(decimal.Parse(fta.PercentualNCFTA.Replace(".", ",")), 2, MidpointRounding.AwayFromZero).ToString();
                fta.PercentualNCFTA = level2.Name + " > " + level3.Name + ": " + PercentualNCFTA2f + " %";
                fta.ReincidenciaDesvioFTA = level2.Name + " > " + level3.Name + ": " + fta.ReincidenciaDesvioFTA;
                fta._Supervisor = usersgq.Name;
                if (fta.MetaFTA == null || fta.MetaFTA == "")
                {
                    dynamic meta = dbFActory.QueryNinjaADO(metaQuery).FirstOrDefault();
                    string meta2 = meta.META;
                    fta.MetaFTA = decimal.Round(decimal.Parse(meta2), 2, MidpointRounding.AwayFromZero).ToString();
                }
            }
        }


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
        /// 
        /// FTA2: Permite inserir um FTA Sem Level1, Level2 ou Level3
        /// 
        /// </param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewFTA2(FTA fta)
        {
            ViewBag.PlanejamentosComFTA = fta.Panejamento_Id;//GetPlanejamentoFTAId();
            fta.ValidaFTA();
            NovoFtaModelParaSgq2(fta);
            fta.IsFTA = true;
            return View("NewFTA", fta);
        }

        private static void NovoFtaModelParaSgq2(FTA fta)
        {
            //Guard.CheckStringFullSimple(fta._Level1, "_Level1");
            //Guard.CheckStringFullSimple(fta._Level2, "_Level2");
            //Guard.CheckStringFullSimple(fta._Level3, "_Level3");
            Guard.CheckStringFullSimple(fta._DataInicioFTA, "_DataInicioFTA");
            Guard.CheckStringFullSimple(fta._DataFimFTA, "_DataFimFTA");
            Guard.ForValidId(fta.Supervisor_Id, "NovoFtaModelParaSgq");
            //fta._DataInicioFTA = Guard.ParseDateToSqlV2(fta._DataInicioFTA).ToShortDateString();
            //fta._DataFimFTA = Guard.ParseDateToSqlV2(fta._DataFimFTA).ToShortDateString();

            var level1 = new ParLevel1DTO();
            var level2 = new ParLevel2DTO();
            var level3 = new ParLevel3DTO();
            var parDepartment = new ParDepartmentDTO();
            var metaQuery = "";
            var aux = "";

            using (var dbFActory = new ADOFactory.Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
            {
                if (fta.Level1Id > 0)
                {
                    level1 = dbFActory.SearchQuery<ParLevel1DTO>("Select * from parlevel1 WHERE Id = '" + fta.Level1Id + "'").FirstOrDefault(r => r.IsActive);
                    fta.Level1Id = level1.Id;
                }
                if (fta.Level2Id > 0)
                {
                    level2 = dbFActory.SearchQuery<ParLevel2DTO>("Select * from parlevel2 WHERE Id = '" + fta.Level2Id + "'").FirstOrDefault(r => r.IsActive);
                    parDepartment = dbFActory.SearchQuery<ParDepartmentDTO>("Select * from ParDepartment WHERE ID = " + level2.ParDepartment_Id).FirstOrDefault();
                    fta.Level2Id = level2.Id;
                }
                if (fta.Level3Id > 0)
                {
                    level3 = dbFActory.SearchQuery<ParLevel3DTO>("Select * from parlevel3 WHERE Id = '" + fta.Level3Id + "'").FirstOrDefault(r => r.IsActive);
                    fta.Level3Id = level3.Id;
                }

                var usersgq = dbFActory.SearchQuery<UserDTO>("Select * from usersgq WHERE ID = " + fta.Supervisor_Id).FirstOrDefault();
                var parcompany = dbFActory.SearchQuery<ParCompanyDTO>("Select * from parcompany WHERE ID = " + fta.Unidade_Id).FirstOrDefault(r => r.IsActive);

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


                if (level1.IsNotNull() && level1.Id > 0)
                {
                    metaQuery = "SELECT ISNULL(ROUND(CASE" +
"\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = " + level1.Id + " AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= '" + dtEnd + " 23:59:59') > 0 THEN  " +
"\n     (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = " + level1.Id + "  AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= '" + dtEnd + " 23:59:59' ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)" +
"\n     ELSE    " +
"\n     (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = " + level1.Id + "  AND(G.ParCompany_id = " + fta.Unidade_Id + " OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= '" + dtEnd + " 23:59:59' ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)  " +
"\n  END,2),0) " +
"\n  AS META";

                }

                fta._Level1 = level1.Name;
                fta._Departamento = parDepartment.Name;
                fta.Departamento_Id = parDepartment.Id;

                string PercentualNCFTA2f = null;

                if (fta.PercentualNCFTA != null)
                {
                    PercentualNCFTA2f = decimal.Round(decimal.Parse(fta.PercentualNCFTA.Replace(".", ",")), 2, MidpointRounding.AwayFromZero).ToString();
                }


                if (fta.Level2Id.IsNotNull())
                {
                    aux += level2.Name;
                }

                if (fta.Level3Id.IsNotNull())
                {
                    if (aux == "")
                    {
                        aux = level3.Name;
                    }
                    else
                    {
                        aux += " > " + level3.Name;
                    }
                }

                if (aux == "")
                {
                    fta.PercentualNCFTA = PercentualNCFTA2f + " %";
                    fta.ReincidenciaDesvioFTA = fta.ReincidenciaDesvioFTA;
                }
                else
                {
                    if (PercentualNCFTA2f != null)
                    {
                        fta.PercentualNCFTA = aux + ": " + PercentualNCFTA2f + " %";
                    }

                    if (fta.ReincidenciaDesvioFTA != null)
                    {
                        fta.ReincidenciaDesvioFTA = aux + ": " + fta.ReincidenciaDesvioFTA;
                    }
                    else
                    {
                        fta.ReincidenciaDesvioFTA = "0";
                    }

                }

                //fta.PercentualNCFTA = level2.Name + " > " + level3.Name + ": " + PercentualNCFTA2f + " %";
                //fta.ReincidenciaDesvioFTA = level2.Name + " > " + level3.Name + ": " + fta.ReincidenciaDesvioFTA;
                fta._Supervisor = usersgq.Name;

                if (metaQuery != "")
                {
                    dynamic meta = dbFActory.QueryNinjaADO(metaQuery).FirstOrDefault();
                    string meta2 = meta.META;
                    fta.MetaFTA = decimal.Round(decimal.Parse(meta2), 2, MidpointRounding.AwayFromZero).ToString();
                }

                if (fta.MetaFTA != null)
                {
                    fta.MetaFTA += " %";
                }
                else
                {
                    fta.MetaFTA = "0";
                }


                #region RH

                if (!string.IsNullOrEmpty(fta.ParDepartments_Hash))
                {
                    //verificar se vamos serparar por - os departamentos validar se pode ter esse caractere na url
                    var parDepartment_ids = fta.ParDepartments_Hash.Split('-').ToList();

                    var departamentos = dbFActory.SearchQuery<ParDepartmentDTO>("Select * from ParDepartment WHERE Id in (" + string.Join(",", parDepartment_ids) + ")").ToList();

                    //ultimo departamento é a seção
                    var secao = departamentos.Last();

                    //departamentos.RemoveAt(departamentos.Count - 1);
                   
                    fta.ParDepartmentsName += string.Join(" | ", departamentos.Select(x => x.Name).ToList());
                    fta.SecaoName = secao.Name;

                }

                if (fta.ParCargo_Id != null && fta.ParCargo_Id > 0)
                {
                    fta.ParCargoName = dbFActory.SearchQuery<ParCargoDTO>("Select * from ParCargo WHERE Id = " + fta.ParCargo_Id).FirstOrDefault().Name;
                }

                #endregion

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

        private int GetPlanejamentoFTAId()
        {
            var novoPlanejamentoTatico = Mapper.Map<Pa_Planejamento>(db.Pa_Planejamento.FirstOrDefault(r => r.IsFta == true));

            if (novoPlanejamentoTatico == null)
                using (var apiTmp = new ApiPa_PlanejamentoController())
                    novoPlanejamentoTatico = apiTmp.CreateGenericEstrategicoTaticoFta();

            return novoPlanejamentoTatico.Id;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");

            try
            {

                System.Resources.ResourceManager resourceManager = ResourcesPA.Resource.ResourceManager;

                ViewBag.Resources = resourceManager.GetResourceSet(
                    Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();

            }
            catch (Exception ex)
            {
            }

            base.Initialize(requestContext);
        }

        [HttpGet]
        public ActionResult NewFTARH(FTA fta)
        {
            ViewBag.PlanejamentosComFTA = fta.Panejamento_Id;
            fta.ValidaFTA();
            NovoFtaModelParaSgq2(fta);
            fta.IsFTA = true;
            return View("NewFTASESMT", fta);
        }

        #endregion

    }
}
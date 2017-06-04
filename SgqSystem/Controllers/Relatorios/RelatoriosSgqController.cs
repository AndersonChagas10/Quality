using Dominio;
using Dominio.Interfaces.Services;
using DTO;
using DTO.ResultSet;
using Helper;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    [OutputCache(Duration = 20, VaryByParam = "none")]
    public class RelatoriosSgqController : BaseController
    {

        #region Constructor

        private FormularioParaRelatorioViewModel form;


        public RelatoriosSgqController(IRelatorioColetaDomain relatorioColetaDomain)
        {

            form = new FormularioParaRelatorioViewModel();

        }

        #endregion

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Scorecard()
        {
            GetMetaAtualScorecard();
            return View(form);
        }

        private void GetMetaAtualScorecard()
        {
            using (var db = new SgqDbDevEntities())
            {
                var atual = db.ParGoalScorecard.OrderByDescending(r => r.Id).FirstOrDefault();
                if (atual != null)
                {
                    ViewBag.PercentValueMid = atual.PercentValueMid;
                    ViewBag.PercentValueHigh = atual.PercentValueHigh;
                }
                else
                {
                    ViewBag.PercentValueMid = "70";
                    ViewBag.PercentValueHigh = "99";
                }
            }
        }

        public ActionResult ScorecardConfig()
        {
            GetMetaAtualScorecard();
            return View(new ParGoalScorecard());
        }

        [HttpPost]
        public ActionResult ScorecardConfig(ParGoalScorecard parGoalScorecard)
        {
            using (var db = new SgqDbDevEntities())
            {
                parGoalScorecard.InitDate = DateTime.Now;
                db.ParGoalScorecard.Add(parGoalScorecard);
                db.SaveChanges();

                var atual = db.ParGoalScorecard.OrderByDescending(r => r.Id).FirstOrDefault();
                if (atual != null)
                {
                    ViewBag.PercentValueMid = atual.PercentValueMid;
                    ViewBag.PercentValueHigh = atual.PercentValueHigh;
                }
                else
                {
                    ViewBag.PercentValueMid = "70";
                    ViewBag.PercentValueHigh = "99";
                }
            }

            return View(parGoalScorecard);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult RelatorioDiario()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true, parLevel1e2 = true)]
        public ActionResult ApontamentosDiarios()
        {
            //Retorna as Roles do usuário logado para filtrar o botão de edição
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
            var db = new SgqDbDevEntities();
            List<string> Retorno = new List<string>();

            int _userId = 0;
            if (!string.IsNullOrEmpty(cookie.Values["roles"]))
            {
                _userId = Convert.ToInt32(cookie.Values["userId"].ToString());
            }

            var roles = db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == _userId).ToList();

            foreach (var role in roles)
            {
                Retorno.Add(role.Role);
            }

            ViewBag.Roles = Retorno;
            //Fim da Role

            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult NaoConformidade()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult ExemploRelatorio()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult VisaoGeralDaArea()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadeDoUsuario = true, parLevel1e2 = true)]
        public ActionResult CartasCep()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult RelatorioGenerico()
        {
            return View(form);
        }

        #region Visao Geral da Area

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult GetTable(DataCarrierFormulario form)
        {
            TabelaDinamicaResultados tabela;
            if (form.Query.Equals("GetTbl1"))
            {
                tabela = GetTbl1(form);
            }
            else if (form.Query.Equals("GetTbl2"))
            {
                tabela = GetTbl2(form);
            }
            else
            {
                tabela = MockTabelaVisaoGeralDaArea();
            }

            tabela.CallBackTableBody = form.CallBackTableBody;
            tabela.Title = form.Title;
            return View(tabela);
        }

        public TabelaDinamicaResultados GetTbl1(DataCarrierFormulario form)
        {
            #region consultaPrincipal

            var query = "" +

// "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'    " +
// "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n  IF OBJECT_ID('tempdb.dbo.#SCORE', 'U') IS NOT NULL																																																										                                               " +
"\n    DROP TABLE #SCORE; 																																																																	                                               " +
"\n  																																																																						                                               " +
"\n  CREATE TABLE #SCORE (																																																																	                                               " +
"\n  	Cluster int null,																																																																	                                               " +
"\n  	ClusterName Varchar(max) null,																																																														                                               " +
"\n  	Regional int null,																																																																	                                               " +
"\n  	RegionalName Varchar(max) null, 																																																													                                               " +
"\n  	ParCompany_Id int null,																																																																                                               " +
"\n  	ParCompanyName Varchar(max) null, 																																																													                                               " +
"\n  	TipoIndicador int null,																																																																                                               " +
"\n  	TipoIndicadorName Varchar(max) null, 																																																												                                               " +
"\n  	Level1Id int null,																																																																	                                               " +
"\n  	Level1Name Varchar(max) null, 																																																														                                               " +
"\n  	Criterio int null,																																																																	                                               " +
"\n  	CriterioName Varchar(max) null, 																																																													                                               " +
"\n  	Av decimal(30,5) null,																																																																                                               " +
"\n  	Nc decimal(30,5) null,																																																																                                               " +
"\n  	Pontos decimal(30,5) null,																																																															                                               " +
"\n  	PontosIndicador decimal(30,5) null,																																																													                                               " +
"\n  	Meta decimal(30,5) null,																																																															                                               " +
"\n  	Real decimal(30,5) null,																																																															                                               " +
"\n  	PontosAtingidos decimal(30,5) null,																																																													                                               " +
"\n  	Scorecard decimal(30,5) null,																																																														                                               " +
"\n  	TipoScore Varchar(max) null																																																															                                               " +
"\n  	)																																																																					                                               " +
"\n  																																																																						                                               " +
"\n  																																																																						                                               " +
"\n  																																																																						                                               " +
"\n  DECLARE @I INT = 0																																																																		                                               " +
"\n  																																																																						                                               " +
"\n  WHILE (SELECT @I) < 1																																																																	                                               " +
"\n  BEGIN  																																																																					                                           " +
"\n     																																																																						                                           " +
"\n      																																																																					                                               " +
"\n   DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + " 00:00'                                                                                                                                                                                                                    					                                               " +
"\n   DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + " 23:59'                                                                                                                                                                                                                    					                                               " +
"\n   CREATE TABLE #AMOSTRATIPO4 ( 																																																															                                               " +
"\n   UNIDADE INT NULL, 																																																																		                                           " +
"\n   INDICADOR INT NULL, 																																																																	                                               " +
"\n   AM INT NULL, 																																																																			                                               " +
"\n   DEF_AM INT NULL 																																																																		                                               " +
"\n   ) 																																																																						                                           " +
"\n   INSERT INTO #AMOSTRATIPO4 																																																																                                           " +
"\n   SELECT 																																																																				                                               " +
"\n    UNIDADE, INDICADOR, 																																																																	                                               " +
"\n   COUNT(1) AM 																																																																			                                               " +
"\n   ,SUM(DEF_AM) DEF_AM 																																																																	                                               " +
"\n   FROM 																																																																					                                               " +
"\n   ( 																																																																						                                           " +
"\n       SELECT 																																																																			                                               " +
"\n       cast(C2.CollectionDate as DATE) AS DATA 																																																											                                               " +
"\n       , C.Id AS UNIDADE 																																																																	                                           " +
"\n       , C2.ParLevel1_Id AS INDICADOR 																																																													                                               " +
"\n       , C2.EvaluationNumber AS AV 																																																														                                               " +
"\n       , C2.Sample AS AM 																																																																	                                           " +
"\n       , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM 																																																						                                               " +
"\n       FROM CollectionLevel2 C2 																																																															                                               " +
"\n       INNER JOIN ParLevel1 L1 																																																															                                               " +
"\n       ON L1.Id = C2.ParLevel1_Id 																																																														                                               " +
"\n       INNER JOIN ParCompany C 																																																															                                               " +
"\n       ON C.Id = C2.UnitId 																																																																                                               " +
"\n       where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL 																																																			                                           " +
"\n       and C2.NotEvaluatedIs = 0 																																																															                                           " +
"\n       and C2.Duplicated = 0 																																																																                                           " +
"\n       and L1.ParConsolidationType_Id = 4 																																																												                                               " +
"\n       group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) 																																																                                               " +
"\n   ) TAB 																																																																					                                           " +
"\n   GROUP BY UNIDADE, INDICADOR 																																																															                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   DECLARE @VOLUMEPCC INT                                                                                                                                                                                                                                              					                                               " +
"\n   DECLARE @DIASABATE INT                                                                                                                                                                                                                                              					                                               " +
"\n   DECLARE @DIASDEVERIFICACAO INT                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   DECLARE @AVFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                					                                               " +
"\n   DECLARE @NCFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   /* INICIO DADOS DA FREQUENCIA ------------------------------------------------------*/                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   DECLARE @CLUSTER INT                                                                                                                                                                                                                                                					                                               " +
"\n   DECLARE @CLUSTERNAME VARCHAR(MAX)                                                                                                                                                                                                                                   					                                               " +
"\n   DECLARE @REGIONAL INT                                                                                                                                                                                                                                               					                                               " +
"\n   DECLARE @REGIONALNAME VARCHAR(MAX)                                                                                                                                                                                                                                  					                                               " +
"\n   DECLARE @PARCOMPANY INT                                                                                                                                                                                                                                             					                                               " +
"\n   DECLARE @PARCOMPANYNAME VARCHAR(MAX)                                                                                                                                                                                                                                					                                               " +
"\n   DECLARE @CRITERIO INT                                                                                                                                                                                                                                               					                                               " +
"\n   DECLARE @CRITERIONAME VARCHAR(MAX)                                                                                                                                                                                                                                  					                                               " +
"\n   DECLARE @PONTOS VARCHAR(MAX)                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n    @CLUSTER = CL.Id                                                                                                                                                                                                                                                   					                                               " +
"\n   , @CLUSTERNAME = CL.Name                                                                                                                                                                                                                                            					                                               " +
"\n   , @REGIONAL = S.Id                                                                                                                                                                                                                                                  					                                               " +
"\n   , @REGIONALNAME = S.Name                                                                                                                                                                                                                                            					                                               " +
"\n   , @PARCOMPANY = C.Id                                                                                                                                                                                                                                                					                                               " +
"\n   , @PARCOMPANYNAME = C.Name                                                                                                                                                                                                                                          					                                               " +
"\n   , @CRITERIO = L1C.ParCriticalLevel_Id                                                                                                                                                                                                                               					                                               " +
"\n   , @CRITERIONAME = CRL.Name                                                                                                                                                                                                                                          					                                               " +
"\n   , @PONTOS = L1C.Points                                                                                                                                                                                                                                              					                                               " +
"\n   FROM ParCompany C                                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   					                                               " +
"\n   LEFT JOIN ParStructure S                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 					                                               " +
"\n   LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            					                                               " +
"\n   LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                     					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                                                               " +
"\n   LEFT JOIN ParCluster CL                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 					                                               " +
"\n   LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                     					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1                                                                                                                                                                                                                                                 " +
"\n   LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON L1C.ParCriticalLevel_Id = CRL.Id                                                                                                                                                                                                                          					                                               " +
"\n   WHERE C.Id = 1                                                                                                                                                                                                                                                                                                                       " +
"\n   --AND L1C.ParLevel1_Id = 25                                                                                                                                                                                                                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   /* FIM DOS DADOS DA FREQUENCIA -----------------------------------------------------*/                                                                                                                                                                              					                                               " +
"\n   CREATE TABLE #VOLUMES (                                                                                                                                                                                                                                                                                                              " +
"\n 	DIASABATE INT NULL,                                                                                                                                                                                                                                                                                                                " +
"\n 	VOLUMEPCC INT NULL                                                                                                                                                                                                                                                                                                                 " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #VOLUMES                                                                                                                                                                                                                                                                    					                           " +
"\n   SELECT COUNT(1) AS DIASABATE, SUM(Quartos) AS VOLUMEPCC FROM VolumePcc1b WHERE Data BETWEEN @DATAINICIAL AND @DATAFINAL GROUP BY ParCompany_id                                                                                                  					                                                                   " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n   CREATE TABLE #DIASVERIFICACAO (                                                                                                                                                                                                                                                                                                      " +
"\n 	DIASVERIFICACAO INT NULL,                                                                                                                                                                                                                                                                                                          " +
"\n 	UnitId INT NULL                                                                                                                                                                                                                                                                                                                    " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #DIASVERIFICACAO                                                                                                                                                                                                                                                                    					                   " +
"\n   SELECT COUNT(1) as DIASDEVERIFICACAO, UnitId FROM                                                                                                                                                                                                                                                                                    " +
"\n   (                                                                                                                                                                                                                                                                                                                                    " +
"\n 	SELECT CL1.UnitId, CONVERT(DATE, ConsolidationDate) DATA                                                                                                                                                                                                                                                                           " +
"\n 	FROM ConsolidationLevel1 CL1                                                                                                                                                                                                                                                                                                       " +
"\n 	WHERE ParLevel1_Id = 24 AND CONVERT(DATE, ConsolidationDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                                   " +
"\n 	GROUP BY CONVERT(DATE, ConsolidationDate), CL1.UnitId                                                                                                                                                                                                                                                                              " +
"\n   ) VT                                                                                                                                                                                                                                                                                                                                 " +
"\n   GROUP BY UnitId                                                                                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n   CREATE TABLE #NAPCC (                                                                                                                                                                                                                                                                                                                " +
"\n 	NAPCC INT NULL,                                                                                                                                                                                                                                                                                                                    " +
"\n 	UnitId INT NULL                                                                                                                                                                                                                                                                                                                    " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #NAPCC  					                                                                                                                                                                                                                                               					                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n            COUNT(1) as NAPCC,                                                                                                                                                                                                                                                                                                          " +
"\n 		   UnitId                                                                                                                                                                                                                                                 						                                               " +
"\n            FROM                                                                                                                                                                                                                                                     						                                           " +
"\n       (                                                                                                                                                                                                                                                             						                                           " +
"\n                SELECT                                                                                                                                                                                                                                               						                                           " +
"\n                COUNT(1) AS NA,                                                                                                                                                                                                                                                                                                         " +
"\n 			   C2.UnitId                                                                                                                                                                                                                                      						                                                   " +
"\n                FROM CollectionLevel2 C2                                                                                                                                                                                                                             						                                           " +
"\n                LEFT JOIN Result_Level3 C3                                                                                                                                                                                                                           						                                           " +
"\n                ON C3.CollectionLevel2_Id = C2.Id                                                                                                                                                                                                                    						                                           " +
"\n                WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                           						                                           " +
"\n                AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                                                                                                                                             						                                           " +
"\n                --AND C2.UnitId = @ParCompany_Id                                                                                                                                                                                                                       						                                           " +
"\n                AND IsNotEvaluate = 1                                                                                                                                                                                                                                						                                           " +
"\n                GROUP BY C2.ID, C2.UnitId                                                                                                                                                                                                                                       						                                   " +
"\n            ) NA		                                                                                                                                                                                                                                                        						                                   " +
"\n            WHERE NA = 2                                                                                                                                                                                                                                             						                                           " +
"\n 		   GROUP BY UnitId                                                                                                                                                                                                                                                                                                             " +
"\n   																																																																						                                               " +
"\n   INSERT INTO #SCORE																																																																		                                           " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   SELECT * FROM                                                                                                                                                                                                                                            								                                               " +
"\n   (                                                                                                                                                                                                                                                                   					                                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n     Cluster                                                                                                                                                                                                                                                           					                                               " +
"\n    , ClusterName                                                                                                                                                                                                                                                      					                                               " +
"\n    , Regional                                                                                                                                                                                                                                                         					                                               " +
"\n    , RegionalName                                                                                                                                                                                                                                                     					                                               " +
"\n    , ParCompanyId                                                                                                                                                                                                                                                     					                                               " +
"\n    , ParCompanyName                                                                                                                                                                                                                                                   					                                               " +
"\n    , CASE WHEN TipoIndicador = 0 THEN 1 ELSE 2 END TipoIndicador                                                                                                                                                                                                      					                                               " +
"\n    , CASE WHEN TipoIndicador = 0 THEN 'Menor' ELSE 'Maior' END TipoIndicadorName                                                                                                                                                                                      					                                               " +
"\n    , Level1Id                                                                                                                                                                                                                                                         					                                               " +
"\n    , Level1Name                                                                                                                                                                                                                                                       					                                               " +
"\n    , Criterio                                                                                                                                                                                                                                                         					                                               " +
"\n    , CriterioName                                                                                                                                                                                                                                                     					                                               " +
"\n    , ROUND(AV,2) AV                                                                                                                                                                                                                                                               		                                               " +
"\n    , ROUND(CASE WHEN Level1Id = 25 THEN AV - NC ELSE NC END,2) NC /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                                                                             			                                               " +
"\n    , ROUND(Pontos,2) Pontos                                                                                                                                                                                                                                                           	                                               " +
"\n    , ROUND(CASE WHEN AV = 0 THEN 0 ELSE Pontos END,2) AS PontosIndicador                                                                                                                                                                                                                                                               " +
"\n    , ROUND(Meta,2) AS Meta                                                                                                                                                                                                                                                             	                                               " +
"\n    , ROUND(CASE WHEN Level1Id = 25 THEN CASE WHEN AV = 0 THEN 0 ELSE (AV - NC) / AV * 100 END ELSE Real END,2) Real /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                            			                                           " +
"\n    , ROUND(PontosAtingidos,2)  PontosAtingidos                                                                                                                                                                                                                                                                                         " +
"\n    , ROUND(Scorecard,2) Scorecard                                                                                                                                                                                                                                                                                                      " +
"\n    , TipoScore                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   FROM                                                                                                                                                                                                                                                                					                                               " +
"\n   (                                                                                                                                                                                                                                                                   					                                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n   *,                                                                                                                                                                                                                                                                  					                                               " +
"\n   /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 					                                               " +
"\n   CASE                                                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       > 1                                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       THEN 1                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       ELSE                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n   END * 100                                                                                                                                                                                                                                                           					                                               " +
"\n   /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 					                                               " +
"\n   AS SCORECARD                                                                                                                                                                                                                                                        					                                               " +
"\n   ,                                                                                                                                                                                                                                                                   					                                               " +
"\n   /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 					                                               " +
"\n   CASE                                                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       > 1                                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       THEN 1                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       < 0 --< 0.7                                                                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       THEN 0                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       ELSE                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n   END                                                                                                                                                                                                                                                                 					                                               " +
"\n   /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 					                                               " +
"\n   * /* MULTIPLICAÇÃO */                                                                                                                                                                                                                                               					                                               " +
"\n   PONTOS                                                                                                                                                                                                                                                              					                                               " +
"\n   AS PONTOSATINGIDOS                                                                                                                                                                                                                                                  					                                               " +
"\n   FROM                                                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   (                                                                                                                                                                                                                                                                   					                                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n     ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                                					                                               " +
"\n    , ISNULL(CL.Name, @CLUSTERNAME) AS ClusterName                                                                                                                                                                                                                     					                                               " +
"\n    , ISNULL(S.Id, @REGIONAL) AS Regional                                                                                                                                                                                                                              					                                               " +
"\n    , ISNULL(S.Name, @REGIONALNAME) AS RegionalName                                                                                                                                                                                                                    					                                               " +
"\n    , ISNULL(CL1.UnitId, @PARCOMPANY) AS ParCompanyId                                                                                                                                                                                                                  					                                               " +
"\n    , ISNULL(C.Name, @PARCOMPANYNAME) AS ParCompanyName                                                                                                                                                                                                                					                                               " +
"\n    , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                             					                                               " +
"\n    , L1.Id AS Level1Id                                                                                                                                                                                                                                                					                                               " +
"\n    , L1.Name AS Level1Name                                                                                                                                                                                                                                            					                                               " +
"\n    , ISNULL(CRL.Id, @CRITERIO) AS Criterio                                                                                                                                                                                                                            					                                               " +
"\n    , ISNULL(CRL.Name, @CRITERIONAME) AS CriterioName                                                                                                                                                                                                                  					                                               " +
"\n    , ISNULL((select top 1 Points from ParLevel1XCluster aaa where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL), @PONTOS) AS Pontos                                                                                                                                                            " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n    --ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                               					                                               " +
"\n    --, (CL.Name)AS ClusterName                                                                                                                                                                                                                                        					                                               " +
"\n    --, (S.Id)AS Regional                                                                                                                                                                                                                                              					                                               " +
"\n    --, (S.Name)AS RegionalName                                                                                                                                                                                                                                        					                                               " +
"\n    --, (CL1.UnitId)AS ParCompanyId                                                                                                                                                                                                                                    					                                               " +
"\n    --, (C.Name)AS ParCompanyName                                                                                                                                                                                                                                      					                                               " +
"\n    --, L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                           					                                               " +
"\n    --, L1.Id AS Level1Id                                                                                                                                                                                                                                              					                                               " +
"\n    --, L1.Name AS Level1Name                                                                                                                                                                                                                                          					                                               " +
"\n    --, (CRL.Id)AS Criterio                                                                                                                                                                                                                                            					                                               " +
"\n    --, (CRL.Name)AS CriterioName                                                                                                                                                                                                                                      					                                               " +
"\n    --, (L1C.Points)AS Pontos                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n    , ST.Name AS TipoScore                                                                                                                                                                                                                                             					                                               " +
"\n    ,                                                                                                                                                                                                                                                                  					                                               " +
"\n     /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                              					                                               " +
"\n     CASE                                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)             " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
"\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
"\n     END                                                                                                                                                                                                                                                               					                                               " +
"\n     /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                              					                                               " +
"\n     AS AV                                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n    ,                                                                                                                                                                                                                                                                  					                                               " +
"\n     /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                              					                                               " +
"\n     CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                            					                                               " +
"\n         /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n 		  WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)         " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n 		  WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                     " +
"\n                                                                                                                                                                                                                                                                  					                                                   " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n           /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                        					                                               " +
"\n           -                                                                                                                                                                                                                                                           					                                               " +
"\n         /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                        					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n         /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n      ELSE                                                                                                                                                                                                                                                             					                                               " +
"\n         /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                        					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n         /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n      END                                                                                                                                                                                                                                                              					                                               " +
"\n      /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                             					                                               " +
"\n      AS NC                                                                                                                                                                                                                                                            					                                               " +
"\n    ,                                                                                                                                                                                                                                                                  					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n     CASE                                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN                                                                                                                                                                                                                                                            					                                               " +
"\n         /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)         " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n 		  WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                     " +
"\n                                                                                                                                                                                                                                                                   					                                                   " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n         /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         = 0                                                                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       THEN 0                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n        ELSE                                                                                                                                                                                                                                                           					                                               " +
"\n         /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                        					                                               " +
"\n             /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                      					                                               " +
"\n             CASE                                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)     " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                 " +
"\n                                                                                                                                                                                                                                                              					                                                       " +
"\n               WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            				                                               " +
"\n             END                                                                                                                                                                                                                                                       					                                               " +
"\n               /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                    					                                               " +
"\n               -  /* SUBTRAÇÃO */                                                                                                                                                                                                                                      					                                               " +
"\n                  /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                 					                                               " +
"\n             CASE                                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                            			                                               " +
"\n             END                                                                                                                                                                                                                                                       					                                               " +
"\n             /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      					                                               " +
"\n          ELSE                                                                                                                                                                                                                                                         					                                               " +
"\n             /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                      					                                               " +
"\n             CASE                                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                            			                                               " +
"\n             END                                                                                                                                                                                                                                                       					                                               " +
"\n             /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      					                                               " +
"\n          END                                                                                                                                                                                                                                                          					                                               " +
"\n          /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          / /*DIVISÃO*/                                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n         /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)         " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n 		  WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                     " +
"\n                                                                                                                                                                                                                                                                    					                                                   " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n         /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n        END * 100                                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n        AS REAL                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n    ,                                                                                                                                                                                                                                                                  					                                               " +
"\n    CASE                                                                                                                                                                                                                                                               					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   					                                               " +
"\n           (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       ELSE                                                                                                                                                                                                                                                            					                                               " +
"\n           (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      					                                               " +
"\n    END                                                                                                                                                                                                                                                                					                                               " +
"\n    AS META                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   FROM      ParLevel1 L1                                                                                                                                                                                                                                              					                                               " +
"\n   LEFT JOIN ConsolidationLevel1 CL1                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON L1.Id = CL1.ParLevel1_Id                                                                                                                                                                                                                                  					                                               " +
"\n   LEFT JOIN ParScoreType ST                                                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                					                                               " +
"\n   LEFT JOIN ParCompany C                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON C.Id = CL1.UnitId                                                                                                                                                                                                                                         					                                               " +
"\n   LEFT JOIN #AMOSTRATIPO4 A4                                                                                                                                                                                                                                          					                                               " +
"\n           ON A4.UNIDADE = C.Id                                                                                                                                                                                                                                      						                                           " +
"\n           AND A4.INDICADOR = L1.ID                                                                                                                                 																														                                               " +
"\n   LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   					                                               " +
"\n   LEFT JOIN ParStructure S                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 					                                               " +
"\n   LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            					                                               " +
"\n   LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                     					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                                                               " +
"\n   LEFT JOIN ParCluster CL                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 					                                               " +
"\n   LEFT JOIN ParConsolidationType CT                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                        					                                               " +
"\n   LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                     					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1                                                                                                                                                                                                                                               " +
"\n   LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CRL.Id  = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL)                                                                                                                                                         " +
"\n   WHERE(ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL OR L1.Id = 25)                                                                                                                                                                                          					                                               " +
"\n     AND(C.Id >= 1 OR(C.Id IS NULL AND L1.Id = 25))                                                                                                                                                                                                       					                                                           " +
"\n   GROUP BY                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n        CL.Id                                                                                                                                                                                                                                                          					                                               " +
"\n       , CL.Name                                                                                                                                                                                                                                                       					                                               " +
"\n       , S.Id                                                                                                                                                                                                                                                          					                                               " +
"\n       , S.Name                                                                                                                                                                                                                                                        					                                               " +
"\n       , CL1.UnitId                                                                                                                                                                                                                                                    					                                               " +
"\n       , C.Name                                                                                                                                                                                                                                                        					                                               " +
"\n       , L1.IsRuleConformity                                                                                                                                                                                                                                           					                                               " +
"\n       , L1.Id                                                                                                                                                                                                                                                         					                                               " +
"\n       , L1.Name                                                                                                                                                                                                                                                       					                                               " +
"\n       , CRL.Id                                                                                                                                                                                                                                                        					                                               " +
"\n       , CRL.Name                                                                                                                                                                                                                                                      					                                               " +
"\n       , ST.Name                                                                                                                                                                                                                                                       					                                               " +
"\n       , CT.Id                                                                                                                                                                                                                                                         					                                               " +
"\n       , L1.HashKey                                                                                                                                                                                                                                                    					                                               " +
"\n       , C.Id                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   ) SCORECARD                                                                                                                                                                                                                                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   ) FIM                                                                                                                                                                                                                                                               					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n    ) SC                                                                                                                                                                                                                                                               					                                               " +
"\n    ORDER BY 11, 10                                                                                                                                                                                                                                                    					                                               " +
"\n    DROP TABLE #AMOSTRATIPO4                                                                                                                                                                                                                                                                                                            " +
"\n    DROP TABLE #VOLUMES	                                                                                                                                                                                                                                                                                                               " +
"\n    DROP TABLE #DIASVERIFICACAO                                                                                                                                                                                                                                                                                                         " +
"\n    DROP TABLE #NAPCC																																																															                                                       " +
"\n  																																																																						                                               " +
"\n    SET @I = @I + 1																																																																		                                               " +
"\n  																																																																						                                               " +
"\n  END                                                                                                                                                                                                                                                                                                                                   " +
"\n  																																																																						                                               ";
            #endregion

            #region Queryes Trs Meio

            var tabela = new TabelaDinamicaResultados();

            var where = string.Empty;
            where += "";

            //if (!string.IsNullOrEmpty(form.ParametroTableRow[0]))
            //{
            //    where += "\n  MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}
            //if (!string.IsNullOrEmpty(form.ParametroTableRow[1]))
            //{
            //    //where += "\n AND MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}
            //if (!string.IsNullOrEmpty(form.ParametroTableRow[2]))
            //{
            //    //where += "\n AND MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}

            //Nomes das colunas do corpo da tabela de dados central
            var query0 = "SELECT  distinct(Reg.Name) name, 4 coolspan" +
                "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null";
                

            //Dados das colunas do corpo da tabela de dados central
            var query1 = "SELECT PP1.Name as CLASSIFIC_NEGOCIO, Reg.Name as MACROPROCESSO, " +
                  "\n case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end as REAL," +
                  "\n 100  as ORCADO, " +
                  "\n 100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end ) as DESVIO, " +
                  "\n (100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end )) / 100 as \"DESVIOPERCENTUAL\" " +
                   "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +

                  "\n GROUP BY Reg.Name, PP1.Name" +
                  "\n ORDER BY 1, 2";

            // Total Direita
            var query2 =
           "SELECT PP1.Name as CLASSIFIC_NEGOCIO," +
                  "\n case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end as REAL," +
                  "\n 100  as ORCADO, " +
                  "\n 100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end ) as DESVIO, " +
                  "\n (100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end )) / 100 as \"DESVIOPERCENTUAL\" " +

                  "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2 and PP1.Name is not null" +

                  "\n GROUP BY PP1.Name " +
                  "\n ORDER BY 1";

            // Total Inferior Esquerda
            var query3 =
           "SELECT Reg.Name as MACROPROCESSO, " +
                  "\n case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end as REAL," +
                  "\n 100  as ORCADO, " +
                  "\n 100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end ) as DESVIO, " +
                  "\n (100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end )) / 100 as \"DESVIOPERCENTUAL\" " +

                  "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +

                  "\n GROUP BY Reg.Name " +
                  "\n ORDER BY 1";

            // Total Inferior Direita
            var query4 =
                "SELECT " +
                  "\n case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end as REAL," +
                  "\n 100  as ORCADO, " +
                  "\n 100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end ) as DESVIO, " +
                  "\n (100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end )) / 100 as \"DESVIOPERCENTUAL\" " +

                  "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +

                  "\n";


            //Nome das linhas da tabela esquerda por ex, indicador X, indicador Y (de uma unidade X, y...)
            var query6 = "SELECT DISTINCT PP1.Name " +
                "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2 and PP1.Name is not null ";

            var db = new SgqDbDevEntities();
            //db.Database.ExecuteSqlCommand(query);
            var result1 = db.Database.SqlQuery<ResultQuery1>(query + " " + query1).ToList();
            var result2 = db.Database.SqlQuery<ResultQuery1>(query + " " + query2).ToList();
            var result3 = db.Database.SqlQuery<ResultQuery1>(query + " " + query3).ToList();
            var result4 = db.Database.SqlQuery<ResultQuery1>(query + " " + query4).ToList();
            var queryRowsBody = db.Database.SqlQuery<string>(query + " " + query6).ToList();

            #endregion

            #region Cabecalhos

            /*1º*/
            tabela.trsCabecalho1 = new List<Ths>();
            tabela.trsCabecalho1.Add(new Ths() { name = "" });
            tabela.trsCabecalho1.Add(new Ths() { name = "" });
            /*Fim  1º*/

            #region DESCRIÇÃO
            /*2º CRIANDO CABECALHO DA SEGUNDA TABELA

                  name   | coolspan
                  ------------------
                   Reg1   | 4 
                   Reg2   | 4
                   RegN   | 4

                  coolspan depende do que vai mostrar em Orçado, real, Desvio, etc...
               */
            #endregion
            tabela.trsCabecalho2 = db.Database.SqlQuery<Ths>(query + " " + query0).OrderBy(r => r.name).ToList();

            var thsMeio = new List<Ths>();
            thsMeio.Add(new Ths() { name = "R", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "M", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "D", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "%", coolspan = 1 });

            foreach (var i in tabela.trsCabecalho2)
                i.tds = thsMeio; //ESTA PROPERTY DEVE CONTER OS ITENS AGRUPADOS (EX: OÇADO, REAL, DESVIO ETC....)

            tabela.trsCabecalho3 = new List<Ths>();
            tabela.trsCabecalho3.Add(new Ths() { name = "Total", coolspan = 4, tds = thsMeio });

            /*Fim  2º*/
            #endregion

            #region Meio

            tabela.trsMeio = new List<Trs>();

            #region DESCRIÇÃO
            /*tdsEsquerda e tdsDireita:

                    LISTA DE TDS, cada row deve ser uma TD, por ex, 
                    uma para REG 1 com os dados para 
                    as Colunas: Real	Desvio %	Desvio $	Orçado, 
                    devem estar em 1 ROW do resultado do SQL, a REG 2,
                    na ROW consecutiva, até REG N.

                   O Resultado Ficara (Query para LINHA Teste1): 

                   Row     | TH   | Col       | valor | coolspan    > new List<Tds>();
                   ----------------------------------------------
                   Teste1  | REG1 | Orçado    | 1     | 1           > new Tds() { valor = 1, coolspan = 1 };
                   Teste1  | REG1 | Real      | 2     | 1           > new Tds() { valor = 2, coolspan = 1 };
                   Teste1  | REG1 | Desvio %  | 3     | 1           .   
                   Teste1  | REG1 | Desvio $  | 4     | 1           .   
                   ----------------------------------------------   .
                   Teste1  | REG2 | Orçado    | 5     | 1
                   Teste1  | REG2 | Real      | 6     | 1
                   Teste1  | REG2 | Desvio %  | 7     | 1
                   Teste1  | REG2 | Desvio $  | 8     | 1
                   ----------------------------------------------
                   Teste1  | REGN | Orçado    | -     | 1
                   Teste1  | REGN | Real      | -    | 1
                   Teste1  | REGN | Desvio %  | -    | 1
                   Teste1  | REGN | Desvio $  | -    | 1
                   ----------------------------------------------
                   Teste2  | REG1 | Orçado    | 1     | 1        
                   Teste2 | REG1 | Real      | 2     | 1        
                   Teste2  | REG1 | Desvio %  | 3     | 1        
                   Teste2  | REG1 | Desvio $  | 4     | 1        
                   ----------------------------------------------
                   Teste2  | REG2 | Orçado    | 5     | 1
                   Teste2  | REG2 | Real      | 6     | 1
                   Teste2  | REG2 | Desvio %  | 7     | 1
                   Teste2  | REG2 | Desvio $  | 8     | 1
                   ----------------------------------------------
                   Teste2  | REGN | Orçado    | 9     | 1
                   Teste2  | REGN | Real      | 10    | 1
                   Teste2  | REGN | Desvio %  | 11    | 1
                   Teste2  | REGN | Desvio $  | 12    | 1

                   OBS: mesmo que a query retorne, para facilitar a coluna TH , col, ROW, o sistema só considera as colunas coolspan e valor.

                   O mesmo para tdsDireita:

                   Row     | TH    | Col        | valor | coolspan
                   ----------------------------------------------
                   Teste1  | TOTAL | Orçado    | 10    | 1
                   Teste1  | TOTAL | Real      | 12    | 1
                   Teste1  | TOTAL | Desvio %  | 14    | 1
                   Teste1  | TOTAL | Desvio $  | 16    | 1

                    */
            //"; 
            #endregion
            foreach (var i in queryRowsBody)
            {

                var filtro = result1.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                var Tr = new Trs()
                {
                    name = i,
                    tdsEsquerda = new List<Tds>(),
                    tdsDireita = new List<Tds>()
                };

                #region Result1 

                /*Caso não exista MACROPROCESSO*/
                //foreach (var x in tabela.trsCabecalho2)
                //    if (!filtro.Any(r => r.MACROPROCESSO.Equals(x.name)))
                //        filtro.Add(new ResultQuery1() { MACROPROCESSO = x.name, CLASSIFIC_NEGOCIO = filtro.FirstOrDefault().CLASSIFIC_NEGOCIO });
                filtro = filtro.OrderBy(r => r.MACROPROCESSO).ToList();
                foreach (var ii in filtro)
                {
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                #region Result2

                filtro = result2.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                foreach (var ii in filtro)
                {
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                tabela.trsMeio.Add(Tr);
            }

            #endregion

            #region Rodapé

            var queryRowsFooter = new List<string>();// TOTAL por ex.
            queryRowsFooter.Add("Total");
            tabela.footer = new List<Trs>();
            foreach (var i in queryRowsFooter)
            {
                //var filtro = result3.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                var Tr = new Trs()
                {
                    name = i,
                    tdsEsquerda = new List<Tds>(),
                    tdsDireita = new List<Tds>()
                };

                #region Result3

                foreach (var ii in result3)
                {
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                #region Result4

                foreach (var ii in result4)
                {
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                tabela.footer.Add(Tr);
            }

            #endregion

            return tabela;
        }

        public TabelaDinamicaResultados GetTbl2(DataCarrierFormulario form)
        {
            #region consultaPrincipal

            var query = "" +

// "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'    " +
// "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n  IF OBJECT_ID('tempdb.dbo.#SCORE', 'U') IS NOT NULL																																																										                                               " +
"\n    DROP TABLE #SCORE; 																																																																	                                               " +
"\n  																																																																						                                               " +
"\n  CREATE TABLE #SCORE (																																																																	                                               " +
"\n  	Cluster int null,																																																																	                                               " +
"\n  	ClusterName Varchar(max) null,																																																														                                               " +
"\n  	Regional int null,																																																																	                                               " +
"\n  	RegionalName Varchar(max) null, 																																																													                                               " +
"\n  	ParCompany_Id int null,																																																																                                               " +
"\n  	ParCompanyName Varchar(max) null, 																																																													                                               " +
"\n  	TipoIndicador int null,																																																																                                               " +
"\n  	TipoIndicadorName Varchar(max) null, 																																																												                                               " +
"\n  	Level1Id int null,																																																																	                                               " +
"\n  	Level1Name Varchar(max) null, 																																																														                                               " +
"\n  	Criterio int null,																																																																	                                               " +
"\n  	CriterioName Varchar(max) null, 																																																													                                               " +
"\n  	Av decimal(30,5) null,																																																																                                               " +
"\n  	Nc decimal(30,5) null,																																																																                                               " +
"\n  	Pontos decimal(30,5) null,																																																															                                               " +
"\n  	PontosIndicador decimal(30,5) null,																																																													                                               " +
"\n  	Meta decimal(30,5) null,																																																															                                               " +
"\n  	Real decimal(30,5) null,																																																															                                               " +
"\n  	PontosAtingidos decimal(30,5) null,																																																													                                               " +
"\n  	Scorecard decimal(30,5) null,																																																														                                               " +
"\n  	TipoScore Varchar(max) null																																																															                                               " +
"\n  	)																																																																					                                               " +
"\n  																																																																						                                               " +
"\n  																																																																						                                               " +
"\n  																																																																						                                               " +
"\n  DECLARE @I INT = 0																																																																		                                               " +
"\n  																																																																						                                               " +
"\n  WHILE (SELECT @I) < 1																																																																	                                               " +
"\n  BEGIN  																																																																					                                           " +
"\n     																																																																						                                           " +
"\n      																																																																					                                               " +
"\n   DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + " 00:00'                                                                                                                                                                                                                    					                                               " +
"\n   DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + " 23:59'                                                                                                                                                                                                                    					                                               " +
"\n   CREATE TABLE #AMOSTRATIPO4 ( 																																																															                                               " +
"\n   UNIDADE INT NULL, 																																																																		                                           " +
"\n   INDICADOR INT NULL, 																																																																	                                               " +
"\n   AM INT NULL, 																																																																			                                               " +
"\n   DEF_AM INT NULL 																																																																		                                               " +
"\n   ) 																																																																						                                           " +
"\n   INSERT INTO #AMOSTRATIPO4 																																																																                                           " +
"\n   SELECT 																																																																				                                               " +
"\n    UNIDADE, INDICADOR, 																																																																	                                               " +
"\n   COUNT(1) AM 																																																																			                                               " +
"\n   ,SUM(DEF_AM) DEF_AM 																																																																	                                               " +
"\n   FROM 																																																																					                                               " +
"\n   ( 																																																																						                                           " +
"\n       SELECT 																																																																			                                               " +
"\n       cast(C2.CollectionDate as DATE) AS DATA 																																																											                                               " +
"\n       , C.Id AS UNIDADE 																																																																	                                           " +
"\n       , C2.ParLevel1_Id AS INDICADOR 																																																													                                               " +
"\n       , C2.EvaluationNumber AS AV 																																																														                                               " +
"\n       , C2.Sample AS AM 																																																																	                                           " +
"\n       , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM 																																																						                                               " +
"\n       FROM CollectionLevel2 C2 																																																															                                               " +
"\n       INNER JOIN ParLevel1 L1 																																																															                                               " +
"\n       ON L1.Id = C2.ParLevel1_Id 																																																														                                               " +
"\n       INNER JOIN ParCompany C 																																																															                                               " +
"\n       ON C.Id = C2.UnitId 																																																																                                               " +
"\n       where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL 																																																			                                           " +
"\n       and C2.NotEvaluatedIs = 0 																																																															                                           " +
"\n       and C2.Duplicated = 0 																																																																                                           " +
"\n       and L1.ParConsolidationType_Id = 4 																																																												                                               " +
"\n       group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) 																																																                                               " +
"\n   ) TAB 																																																																					                                           " +
"\n   GROUP BY UNIDADE, INDICADOR 																																																															                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   DECLARE @VOLUMEPCC INT                                                                                                                                                                                                                                              					                                               " +
"\n   DECLARE @DIASABATE INT                                                                                                                                                                                                                                              					                                               " +
"\n   DECLARE @DIASDEVERIFICACAO INT                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   DECLARE @AVFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                					                                               " +
"\n   DECLARE @NCFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   /* INICIO DADOS DA FREQUENCIA ------------------------------------------------------*/                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   DECLARE @CLUSTER INT                                                                                                                                                                                                                                                					                                               " +
"\n   DECLARE @CLUSTERNAME VARCHAR(MAX)                                                                                                                                                                                                                                   					                                               " +
"\n   DECLARE @REGIONAL INT                                                                                                                                                                                                                                               					                                               " +
"\n   DECLARE @REGIONALNAME VARCHAR(MAX)                                                                                                                                                                                                                                  					                                               " +
"\n   DECLARE @PARCOMPANY INT                                                                                                                                                                                                                                             					                                               " +
"\n   DECLARE @PARCOMPANYNAME VARCHAR(MAX)                                                                                                                                                                                                                                					                                               " +
"\n   DECLARE @CRITERIO INT                                                                                                                                                                                                                                               					                                               " +
"\n   DECLARE @CRITERIONAME VARCHAR(MAX)                                                                                                                                                                                                                                  					                                               " +
"\n   DECLARE @PONTOS VARCHAR(MAX)                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n    @CLUSTER = CL.Id                                                                                                                                                                                                                                                   					                                               " +
"\n   , @CLUSTERNAME = CL.Name                                                                                                                                                                                                                                            					                                               " +
"\n   , @REGIONAL = S.Id                                                                                                                                                                                                                                                  					                                               " +
"\n   , @REGIONALNAME = S.Name                                                                                                                                                                                                                                            					                                               " +
"\n   , @PARCOMPANY = C.Id                                                                                                                                                                                                                                                					                                               " +
"\n   , @PARCOMPANYNAME = C.Name                                                                                                                                                                                                                                          					                                               " +
"\n   , @CRITERIO = L1C.ParCriticalLevel_Id                                                                                                                                                                                                                               					                                               " +
"\n   , @CRITERIONAME = CRL.Name                                                                                                                                                                                                                                          					                                               " +
"\n   , @PONTOS = L1C.Points                                                                                                                                                                                                                                              					                                               " +
"\n   FROM ParCompany C                                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   					                                               " +
"\n   LEFT JOIN ParStructure S                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 					                                               " +
"\n   LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            					                                               " +
"\n   LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                     					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                                                               " +
"\n   LEFT JOIN ParCluster CL                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 					                                               " +
"\n   LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                     					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1                                                                                                                                                                                                                                                 " +
"\n   LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON L1C.ParCriticalLevel_Id = CRL.Id                                                                                                                                                                                                                          					                                               " +
"\n   WHERE C.Id = 1                                                                                                                                                                                                                                                                                                                       " +
"\n   --AND L1C.ParLevel1_Id = 25                                                                                                                                                                                                                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   /* FIM DOS DADOS DA FREQUENCIA -----------------------------------------------------*/                                                                                                                                                                              					                                               " +
"\n   CREATE TABLE #VOLUMES (                                                                                                                                                                                                                                                                                                              " +
"\n 	DIASABATE INT NULL,                                                                                                                                                                                                                                                                                                                " +
"\n 	VOLUMEPCC INT NULL                                                                                                                                                                                                                                                                                                                 " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #VOLUMES                                                                                                                                                                                                                                                                    					                           " +
"\n   SELECT COUNT(1) AS DIASABATE, SUM(Quartos) AS VOLUMEPCC FROM VolumePcc1b WHERE Data BETWEEN @DATAINICIAL AND @DATAFINAL GROUP BY ParCompany_id                                                                                                  					                                                                   " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n   CREATE TABLE #DIASVERIFICACAO (                                                                                                                                                                                                                                                                                                      " +
"\n 	DIASVERIFICACAO INT NULL,                                                                                                                                                                                                                                                                                                          " +
"\n 	UnitId INT NULL                                                                                                                                                                                                                                                                                                                    " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #DIASVERIFICACAO                                                                                                                                                                                                                                                                    					                   " +
"\n   SELECT COUNT(1) as DIASDEVERIFICACAO, UnitId FROM                                                                                                                                                                                                                                                                                    " +
"\n   (                                                                                                                                                                                                                                                                                                                                    " +
"\n 	SELECT CL1.UnitId, CONVERT(DATE, ConsolidationDate) DATA                                                                                                                                                                                                                                                                           " +
"\n 	FROM ConsolidationLevel1 CL1                                                                                                                                                                                                                                                                                                       " +
"\n 	WHERE ParLevel1_Id = 24 AND CONVERT(DATE, ConsolidationDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                                   " +
"\n 	GROUP BY CONVERT(DATE, ConsolidationDate), CL1.UnitId                                                                                                                                                                                                                                                                              " +
"\n   ) VT                                                                                                                                                                                                                                                                                                                                 " +
"\n   GROUP BY UnitId                                                                                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n   CREATE TABLE #NAPCC (                                                                                                                                                                                                                                                                                                                " +
"\n 	NAPCC INT NULL,                                                                                                                                                                                                                                                                                                                    " +
"\n 	UnitId INT NULL                                                                                                                                                                                                                                                                                                                    " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #NAPCC  					                                                                                                                                                                                                                                               					                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n            COUNT(1) as NAPCC,                                                                                                                                                                                                                                                                                                          " +
"\n 		   UnitId                                                                                                                                                                                                                                                 						                                               " +
"\n            FROM                                                                                                                                                                                                                                                     						                                           " +
"\n       (                                                                                                                                                                                                                                                             						                                           " +
"\n                SELECT                                                                                                                                                                                                                                               						                                           " +
"\n                COUNT(1) AS NA,                                                                                                                                                                                                                                                                                                         " +
"\n 			   C2.UnitId                                                                                                                                                                                                                                      						                                                   " +
"\n                FROM CollectionLevel2 C2                                                                                                                                                                                                                             						                                           " +
"\n                LEFT JOIN Result_Level3 C3                                                                                                                                                                                                                           						                                           " +
"\n                ON C3.CollectionLevel2_Id = C2.Id                                                                                                                                                                                                                    						                                           " +
"\n                WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                           						                                           " +
"\n                AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                                                                                                                                             						                                           " +
"\n                --AND C2.UnitId = @ParCompany_Id                                                                                                                                                                                                                       						                                           " +
"\n                AND IsNotEvaluate = 1                                                                                                                                                                                                                                						                                           " +
"\n                GROUP BY C2.ID, C2.UnitId                                                                                                                                                                                                                                       						                                   " +
"\n            ) NA		                                                                                                                                                                                                                                                        						                                   " +
"\n            WHERE NA = 2                                                                                                                                                                                                                                             						                                           " +
"\n 		   GROUP BY UnitId                                                                                                                                                                                                                                                                                                             " +
"\n   																																																																						                                               " +
"\n   INSERT INTO #SCORE																																																																		                                           " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   SELECT * FROM                                                                                                                                                                                                                                            								                                               " +
"\n   (                                                                                                                                                                                                                                                                   					                                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n     Cluster                                                                                                                                                                                                                                                           					                                               " +
"\n    , ClusterName                                                                                                                                                                                                                                                      					                                               " +
"\n    , Regional                                                                                                                                                                                                                                                         					                                               " +
"\n    , RegionalName                                                                                                                                                                                                                                                     					                                               " +
"\n    , ParCompanyId                                                                                                                                                                                                                                                     					                                               " +
"\n    , ParCompanyName                                                                                                                                                                                                                                                   					                                               " +
"\n    , CASE WHEN TipoIndicador = 0 THEN 1 ELSE 2 END TipoIndicador                                                                                                                                                                                                      					                                               " +
"\n    , CASE WHEN TipoIndicador = 0 THEN 'Menor' ELSE 'Maior' END TipoIndicadorName                                                                                                                                                                                      					                                               " +
"\n    , Level1Id                                                                                                                                                                                                                                                         					                                               " +
"\n    , Level1Name                                                                                                                                                                                                                                                       					                                               " +
"\n    , Criterio                                                                                                                                                                                                                                                         					                                               " +
"\n    , CriterioName                                                                                                                                                                                                                                                     					                                               " +
"\n    , ROUND(AV,2) AV                                                                                                                                                                                                                                                               		                                               " +
"\n    , ROUND(CASE WHEN Level1Id = 25 THEN AV - NC ELSE NC END,2) NC /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                                                                             			                                               " +
"\n    , ROUND(Pontos,2) Pontos                                                                                                                                                                                                                                                           	                                               " +
"\n    , ROUND(CASE WHEN AV = 0 THEN 0 ELSE Pontos END,2) AS PontosIndicador                                                                                                                                                                                                                                                               " +
"\n    , ROUND(Meta,2) AS Meta                                                                                                                                                                                                                                                             	                                               " +
"\n    , ROUND(CASE WHEN Level1Id = 25 THEN CASE WHEN AV = 0 THEN 0 ELSE (AV - NC) / AV * 100 END ELSE Real END,2) Real /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                            			                                           " +
"\n    , ROUND(PontosAtingidos,2)  PontosAtingidos                                                                                                                                                                                                                                                                                         " +
"\n    , ROUND(Scorecard,2) Scorecard                                                                                                                                                                                                                                                                                                      " +
"\n    , TipoScore                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   FROM                                                                                                                                                                                                                                                                					                                               " +
"\n   (                                                                                                                                                                                                                                                                   					                                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n   *,                                                                                                                                                                                                                                                                  					                                               " +
"\n   /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 					                                               " +
"\n   CASE                                                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       > 1                                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       THEN 1                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       ELSE                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n   END * 100                                                                                                                                                                                                                                                           					                                               " +
"\n   /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 					                                               " +
"\n   AS SCORECARD                                                                                                                                                                                                                                                        					                                               " +
"\n   ,                                                                                                                                                                                                                                                                   					                                               " +
"\n   /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 					                                               " +
"\n   CASE                                                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       > 1                                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       THEN 1                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       < 0 --< 0.7                                                                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       THEN 0                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       ELSE                                                                                                                                                                                                                                                            					                                               " +
"\n       /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n       CASE                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE REAL / META END                                                                                                                                                                                                          		                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           ELSE                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           END                                                                                                                                                                                                                                                         					                                               " +
"\n       /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            					                                               " +
"\n   END                                                                                                                                                                                                                                                                 					                                               " +
"\n   /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 					                                               " +
"\n   * /* MULTIPLICAÇÃO */                                                                                                                                                                                                                                               					                                               " +
"\n   PONTOS                                                                                                                                                                                                                                                              					                                               " +
"\n   AS PONTOSATINGIDOS                                                                                                                                                                                                                                                  					                                               " +
"\n   FROM                                                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   (                                                                                                                                                                                                                                                                   					                                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n     ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                                					                                               " +
"\n    , ISNULL(CL.Name, @CLUSTERNAME) AS ClusterName                                                                                                                                                                                                                     					                                               " +
"\n    , ISNULL(S.Id, @REGIONAL) AS Regional                                                                                                                                                                                                                              					                                               " +
"\n    , ISNULL(S.Name, @REGIONALNAME) AS RegionalName                                                                                                                                                                                                                    					                                               " +
"\n    , ISNULL(CL1.UnitId, @PARCOMPANY) AS ParCompanyId                                                                                                                                                                                                                  					                                               " +
"\n    , ISNULL(C.Name, @PARCOMPANYNAME) AS ParCompanyName                                                                                                                                                                                                                					                                               " +
"\n    , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                             					                                               " +
"\n    , L1.Id AS Level1Id                                                                                                                                                                                                                                                					                                               " +
"\n    , L1.Name AS Level1Name                                                                                                                                                                                                                                            					                                               " +
"\n    , ISNULL(CRL.Id, @CRITERIO) AS Criterio                                                                                                                                                                                                                            					                                               " +
"\n    , ISNULL(CRL.Name, @CRITERIONAME) AS CriterioName                                                                                                                                                                                                                  					                                               " +
"\n    , ISNULL((select top 1 Points from ParLevel1XCluster aaa where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL), @PONTOS) AS Pontos                                                                                                                                                            " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n    --ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                               					                                               " +
"\n    --, (CL.Name)AS ClusterName                                                                                                                                                                                                                                        					                                               " +
"\n    --, (S.Id)AS Regional                                                                                                                                                                                                                                              					                                               " +
"\n    --, (S.Name)AS RegionalName                                                                                                                                                                                                                                        					                                               " +
"\n    --, (CL1.UnitId)AS ParCompanyId                                                                                                                                                                                                                                    					                                               " +
"\n    --, (C.Name)AS ParCompanyName                                                                                                                                                                                                                                      					                                               " +
"\n    --, L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                           					                                               " +
"\n    --, L1.Id AS Level1Id                                                                                                                                                                                                                                              					                                               " +
"\n    --, L1.Name AS Level1Name                                                                                                                                                                                                                                          					                                               " +
"\n    --, (CRL.Id)AS Criterio                                                                                                                                                                                                                                            					                                               " +
"\n    --, (CRL.Name)AS CriterioName                                                                                                                                                                                                                                      					                                               " +
"\n    --, (L1C.Points)AS Pontos                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n    , ST.Name AS TipoScore                                                                                                                                                                                                                                             					                                               " +
"\n    ,                                                                                                                                                                                                                                                                  					                                               " +
"\n     /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                              					                                               " +
"\n     CASE                                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)             " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
"\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
"\n     END                                                                                                                                                                                                                                                               					                                               " +
"\n     /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                              					                                               " +
"\n     AS AV                                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n    ,                                                                                                                                                                                                                                                                  					                                               " +
"\n     /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                              					                                               " +
"\n     CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                            					                                               " +
"\n         /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n 		  WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)         " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n 		  WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                     " +
"\n                                                                                                                                                                                                                                                                  					                                                   " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n           /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                        					                                               " +
"\n           -                                                                                                                                                                                                                                                           					                                               " +
"\n         /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                        					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n         /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n      ELSE                                                                                                                                                                                                                                                             					                                               " +
"\n         /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                               					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                        					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n         /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n      END                                                                                                                                                                                                                                                              					                                               " +
"\n      /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                             					                                               " +
"\n      AS NC                                                                                                                                                                                                                                                            					                                               " +
"\n    ,                                                                                                                                                                                                                                                                  					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n     CASE                                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN                                                                                                                                                                                                                                                            					                                               " +
"\n         /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)         " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n 		  WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                     " +
"\n                                                                                                                                                                                                                                                                   					                                                   " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n         /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         = 0                                                                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       THEN 0                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n        ELSE                                                                                                                                                                                                                                                           					                                               " +
"\n         /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                        					                                               " +
"\n             /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                      					                                               " +
"\n             CASE                                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)     " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                 " +
"\n                                                                                                                                                                                                                                                              					                                                       " +
"\n               WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            				                                               " +
"\n             END                                                                                                                                                                                                                                                       					                                               " +
"\n               /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                    					                                               " +
"\n               -  /* SUBTRAÇÃO */                                                                                                                                                                                                                                      					                                               " +
"\n                  /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                 					                                               " +
"\n             CASE                                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                            			                                               " +
"\n             END                                                                                                                                                                                                                                                       					                                               " +
"\n             /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      					                                               " +
"\n          ELSE                                                                                                                                                                                                                                                         					                                               " +
"\n             /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                      					                                               " +
"\n             CASE                                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN L1.Id = 25 THEN @NCFREQUENCIAVERIFICACAO                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n               WHEN CT.Id IN(4) THEN SUM(A4.DEF_AM)                                                                                                                                                                                                                            			                                               " +
"\n             END                                                                                                                                                                                                                                                       					                                               " +
"\n             /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      					                                               " +
"\n          END                                                                                                                                                                                                                                                          					                                               " +
"\n          /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          / /*DIVISÃO*/                                                                                                                                                                                                                                                					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n         /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n         CASE                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN L1.Id = 25 THEN (CASE WHEN (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) > (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) THEN (SELECT sum(DIASABATE) FROM #VOLUMES WHERE UnitId = C.Id) ELSE (SELECT sum(DIASVERIFICACAO) FROM #DIASVERIFICACAO WHERE UnitId = C.Id) END)         " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n 		  WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT sum(NAPCC) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                     " +
"\n                                                                                                                                                                                                                                                                    					                                                   " +
"\n           WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n           WHEN CT.Id IN(4) THEN SUM(A4.AM)                                                                                                                                                                                                                            					                                               " +
"\n         END                                                                                                                                                                                                                                                           					                                               " +
"\n         /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n        END * 100                                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n        AS REAL                                                                                                                                                                                                                                                        					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n    ,                                                                                                                                                                                                                                                                  					                                               " +
"\n    CASE                                                                                                                                                                                                                                                               					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   					                                               " +
"\n           (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n       ELSE                                                                                                                                                                                                                                                            					                                               " +
"\n           (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      					                                               " +
"\n    END                                                                                                                                                                                                                                                                					                                               " +
"\n    AS META                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   FROM      ParLevel1 L1                                                                                                                                                                                                                                              					                                               " +
"\n   LEFT JOIN ConsolidationLevel1 CL1                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON L1.Id = CL1.ParLevel1_Id                                                                                                                                                                                                                                  					                                               " +
"\n   LEFT JOIN ParScoreType ST                                                                                                                                                                                                                                           					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                					                                               " +
"\n   LEFT JOIN ParCompany C                                                                                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON C.Id = CL1.UnitId                                                                                                                                                                                                                                         					                                               " +
"\n   LEFT JOIN #AMOSTRATIPO4 A4                                                                                                                                                                                                                                          					                                               " +
"\n           ON A4.UNIDADE = C.Id                                                                                                                                                                                                                                      						                                           " +
"\n           AND A4.INDICADOR = L1.ID                                                                                                                                 																														                                               " +
"\n   LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   					                                               " +
"\n   LEFT JOIN ParStructure S                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 					                                               " +
"\n   LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            					                                               " +
"\n   LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                     					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                                                               " +
"\n   LEFT JOIN ParCluster CL                                                                                                                                                                                                                                             					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 					                                               " +
"\n   LEFT JOIN ParConsolidationType CT                                                                                                                                                                                                                                   					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                        					                                               " +
"\n   LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                     					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1                                                                                                                                                                                                                                               " +
"\n   LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                      					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n          ON CRL.Id  = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL)                                                                                                                                                         " +
"\n   WHERE(ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL OR L1.Id = 25)                                                                                                                                                                                          					                                               " +
"\n     AND(C.Id >= 1 OR(C.Id IS NULL AND L1.Id = 25))                                                                                                                                                                                                       					                                                           " +
"\n   GROUP BY                                                                                                                                                                                                                                                            					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n        CL.Id                                                                                                                                                                                                                                                          					                                               " +
"\n       , CL.Name                                                                                                                                                                                                                                                       					                                               " +
"\n       , S.Id                                                                                                                                                                                                                                                          					                                               " +
"\n       , S.Name                                                                                                                                                                                                                                                        					                                               " +
"\n       , CL1.UnitId                                                                                                                                                                                                                                                    					                                               " +
"\n       , C.Name                                                                                                                                                                                                                                                        					                                               " +
"\n       , L1.IsRuleConformity                                                                                                                                                                                                                                           					                                               " +
"\n       , L1.Id                                                                                                                                                                                                                                                         					                                               " +
"\n       , L1.Name                                                                                                                                                                                                                                                       					                                               " +
"\n       , CRL.Id                                                                                                                                                                                                                                                        					                                               " +
"\n       , CRL.Name                                                                                                                                                                                                                                                      					                                               " +
"\n       , ST.Name                                                                                                                                                                                                                                                       					                                               " +
"\n       , CT.Id                                                                                                                                                                                                                                                         					                                               " +
"\n       , L1.HashKey                                                                                                                                                                                                                                                    					                                               " +
"\n       , C.Id                                                                                                                                                                                                                                                          					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   ) SCORECARD                                                                                                                                                                                                                                                         					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   ) FIM                                                                                                                                                                                                                                                               					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n    ) SC                                                                                                                                                                                                                                                               					                                               " +
"\n    ORDER BY 11, 10                                                                                                                                                                                                                                                    					                                               " +
"\n    DROP TABLE #AMOSTRATIPO4                                                                                                                                                                                                                                                                                                            " +
"\n    DROP TABLE #VOLUMES	                                                                                                                                                                                                                                                                                                               " +
"\n    DROP TABLE #DIASVERIFICACAO                                                                                                                                                                                                                                                                                                         " +
"\n    DROP TABLE #NAPCC																																																															                                                       " +
"\n  																																																																						                                               " +
"\n    SET @I = @I + 1																																																																		                                               " +
"\n  																																																																						                                               " +
"\n  END                                                                                                                                                                                                                                                                                                                                   " +
"\n  																																																																						                                               ";
            #endregion

            #region Queryes Trs Meio

            var tabela = new TabelaDinamicaResultados();

            var where = string.Empty;
            where += "";

            //if (!string.IsNullOrEmpty(form.ParametroTableRow[0]))
            //{
            //    where += "\n  MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}
            //if (!string.IsNullOrEmpty(form.ParametroTableRow[1]))
            //{
            //    //where += "\n AND MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}
            //if (!string.IsNullOrEmpty(form.ParametroTableRow[2]))
            //{
            //    //where += "\n AND MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}

            //Nomes das colunas do corpo da tabela de dados central
            var query0 = "SELECT  distinct(C.Initials) name, 4 coolspan  " +

                    "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n WHERE Reg.Name = '" + form.ParametroTableCol[0] + "'" +
                    "\n AND PP1.Name = '" + form.ParametroTableRow[0] + "'" +

                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +

                    "\n ORDER BY 1";

            //Dados das colunas do corpo da tabela de dados central
            var query1 = "SELECT P1.Name as CLASSIFIC_NEGOCIO, C.Initials as MACROPROCESSO, " +
                  "\n case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end as REAL," +
                  "\n 100  as ORCADO, " +
                  "\n 100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end ) as DESVIO, " +
                  "\n (100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end )) / 100 as \"DESVIOPERCENTUAL\" " +

                   "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n WHERE Reg.Name = '" + form.ParametroTableCol[0] + "'" +
                    "\n AND PP1.Name = '" + form.ParametroTableRow[0] + "'" +

                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +

                  "\n GROUP BY P1.Name, C.Initials " +
                  "\n ORDER BY 1, 2";

            // Total Direita
            var query2 =
           "SELECT P1.Name as CLASSIFIC_NEGOCIO," +
                  "\n case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end as REAL," +
                  "\n 100  as ORCADO, " +
                  "\n 100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end ) as DESVIO, " +
                  "\n (100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end )) / 100 as \"DESVIOPERCENTUAL\" " +

                   "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n WHERE Reg.Name = '" + form.ParametroTableCol[0] + "'" +
                    "\n AND PP1.Name = '" + form.ParametroTableRow[0] + "'" +

                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +

                  "\n GROUP BY P1.Name " +
                  "\n ORDER BY 1";

            // Total Inferior Esquerda
            var query3 =
           "SELECT C.Initials as MACROPROCESSO, " +
                  "\n case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end as REAL," +
                  "\n 100  as ORCADO, " +
                  "\n 100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end ) as DESVIO, " +
                  "\n (100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end )) / 100 as \"DESVIOPERCENTUAL\" " +

                   "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n WHERE Reg.Name = '" + form.ParametroTableCol[0] + "'" +
                    "\n AND PP1.Name = '" + form.ParametroTableRow[0] + "'" +

                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +

                  "\n GROUP BY C.Initials " +
                  "\n ORDER BY 1";

            // Total Inferior Direita
            var query4 =
                "SELECT " +
                  "\n case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end as REAL," +
                  "\n 100  as ORCADO, " +
                  "\n 100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end ) as DESVIO, " +
                  "\n (100 - (case when isnull(sum(Pontos),0) = 0 or isnull(sum(PontosAtingidos),0) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 0) / isnull(sum(Pontos),0))*100  end )) / 100 as \"DESVIOPERCENTUAL\" " +

                    "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n WHERE Reg.Name = '" + form.ParametroTableCol[0] + "'" +
                    "\n AND PP1.Name = '" + form.ParametroTableRow[0] + "'" +

                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +

                  "\n";


            //Nome das linhas da tabela esquerda por ex, indicador X, indicador Y (de uma unidade X, y...)
            var query6 = "SELECT DISTINCT P1.Name " +
             "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 " +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n LEFT JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id and S.Level1Id = P1.Id " +

                    "\n WHERE Reg.Name = '" + form.ParametroTableCol[0] + "'" +
                    "\n AND PP1.Name = '" + form.ParametroTableRow[0] + "'" +

                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null";

            var db = new SgqDbDevEntities();
            //db.Database.ExecuteSqlCommand(query);
            var result1 = db.Database.SqlQuery<ResultQuery1>(query + " " + query1).ToList();
            var result2 = db.Database.SqlQuery<ResultQuery1>(query + " " + query2).ToList();
            var result3 = db.Database.SqlQuery<ResultQuery1>(query + " " + query3).ToList();
            var result4 = db.Database.SqlQuery<ResultQuery1>(query + " " + query4).ToList();
            var queryRowsBody = db.Database.SqlQuery<string>(query + " " + query6).ToList();

            #endregion

            #region Cabecalhos

            /*1º*/
            tabela.trsCabecalho1 = new List<Ths>();
            tabela.trsCabecalho1.Add(new Ths() { name = "Pacote: " + form.ParametroTableRow[0]  });
            tabela.trsCabecalho1.Add(new Ths() { name = "Regional: " + form.ParametroTableCol[0] });
            /*Fim  1º*/

                #region DESCRIÇÃO
                /*2º CRIANDO CABECALHO DA SEGUNDA TABELA

                      name   | coolspan
                      ------------------
                       Reg1   | 4 
                       Reg2   | 4
                       RegN   | 4

                      coolspan depende do que vai mostrar em Orçado, real, Desvio, etc...
                   */
                #endregion
                tabela.trsCabecalho2 = db.Database.SqlQuery<Ths>(query + " " + query0).OrderBy(r => r.name).ToList();

            var thsMeio = new List<Ths>();
            thsMeio.Add(new Ths() { name = "R", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "M", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "D", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "%", coolspan = 1 });

            foreach (var i in tabela.trsCabecalho2)
                i.tds = thsMeio; //ESTA PROPERTY DEVE CONTER OS ITENS AGRUPADOS (EX: OÇADO, REAL, DESVIO ETC....)

            tabela.trsCabecalho3 = new List<Ths>();
            tabela.trsCabecalho3.Add(new Ths() { name = "Total", coolspan = 4, tds = thsMeio });

            /*Fim  2º*/
            #endregion

            #region Meio

            tabela.trsMeio = new List<Trs>();

            #region DESCRIÇÃO
            /*tdsEsquerda e tdsDireita:

                    LISTA DE TDS, cada row deve ser uma TD, por ex, 
                    uma para REG 1 com os dados para 
                    as Colunas: Real	Desvio %	Desvio $	Orçado, 
                    devem estar em 1 ROW do resultado do SQL, a REG 2,
                    na ROW consecutiva, até REG N.

                   O Resultado Ficara (Query para LINHA Teste1): 

                   Row     | TH   | Col       | valor | coolspan    > new List<Tds>();
                   ----------------------------------------------
                   Teste1  | REG1 | Orçado    | 1     | 1           > new Tds() { valor = 1, coolspan = 1 };
                   Teste1  | REG1 | Real      | 2     | 1           > new Tds() { valor = 2, coolspan = 1 };
                   Teste1  | REG1 | Desvio %  | 3     | 1           .   
                   Teste1  | REG1 | Desvio $  | 4     | 1           .   
                   ----------------------------------------------   .
                   Teste1  | REG2 | Orçado    | 5     | 1
                   Teste1  | REG2 | Real      | 6     | 1
                   Teste1  | REG2 | Desvio %  | 7     | 1
                   Teste1  | REG2 | Desvio $  | 8     | 1
                   ----------------------------------------------
                   Teste1  | REGN | Orçado    | -     | 1
                   Teste1  | REGN | Real      | -    | 1
                   Teste1  | REGN | Desvio %  | -    | 1
                   Teste1  | REGN | Desvio $  | -    | 1
                   ----------------------------------------------
                   Teste2  | REG1 | Orçado    | 1     | 1        
                   Teste2 | REG1 | Real      | 2     | 1        
                   Teste2  | REG1 | Desvio %  | 3     | 1        
                   Teste2  | REG1 | Desvio $  | 4     | 1        
                   ----------------------------------------------
                   Teste2  | REG2 | Orçado    | 5     | 1
                   Teste2  | REG2 | Real      | 6     | 1
                   Teste2  | REG2 | Desvio %  | 7     | 1
                   Teste2  | REG2 | Desvio $  | 8     | 1
                   ----------------------------------------------
                   Teste2  | REGN | Orçado    | 9     | 1
                   Teste2  | REGN | Real      | 10    | 1
                   Teste2  | REGN | Desvio %  | 11    | 1
                   Teste2  | REGN | Desvio $  | 12    | 1

                   OBS: mesmo que a query retorne, para facilitar a coluna TH , col, ROW, o sistema só considera as colunas coolspan e valor.

                   O mesmo para tdsDireita:

                   Row     | TH    | Col        | valor | coolspan
                   ----------------------------------------------
                   Teste1  | TOTAL | Orçado    | 10    | 1
                   Teste1  | TOTAL | Real      | 12    | 1
                   Teste1  | TOTAL | Desvio %  | 14    | 1
                   Teste1  | TOTAL | Desvio $  | 16    | 1

                    */
            //"; 
            #endregion
            foreach (var i in queryRowsBody)
            {

                var filtro = result1.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                var Tr = new Trs()
                {
                    name = i,
                    tdsEsquerda = new List<Tds>(),
                    tdsDireita = new List<Tds>()
                };

                #region Result1 

                /*Caso não exista MACROPROCESSO*/
                //foreach (var x in tabela.trsCabecalho2)
                //    if (!filtro.Any(r => r.MACROPROCESSO.Equals(x.name)))
                //        filtro.Add(new ResultQuery1() { MACROPROCESSO = x.name, CLASSIFIC_NEGOCIO = filtro.FirstOrDefault().CLASSIFIC_NEGOCIO });
                filtro = filtro.OrderBy(r => r.MACROPROCESSO).ToList();
                foreach (var ii in filtro)
                {
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                #region Result2

                filtro = result2.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                foreach (var ii in filtro)
                {
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                tabela.trsMeio.Add(Tr);
            }

            #endregion

            #region Rodapé

            var queryRowsFooter = new List<string>();// TOTAL por ex.
            queryRowsFooter.Add("Total");
            tabela.footer = new List<Trs>();
            foreach (var i in queryRowsFooter)
            {
                //var filtro = result3.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                var Tr = new Trs()
                {
                    name = i,
                    tdsEsquerda = new List<Tds>(),
                    tdsDireita = new List<Tds>()
                };

                #region Result3

                foreach (var ii in result3)
                {
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                #region Result4

                foreach (var ii in result4)
                {
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                tabela.footer.Add(Tr);
            }

            #endregion

            return tabela;
        }

        public class ResultQuery1
        {
            public string CLASSIFIC_NEGOCIO { get; set; }
            public string MACROPROCESSO { get; set; }
            public int ORCADO { get; set; }
            public decimal DESVIO { get; set; }
            public decimal DESVIOPERCENTUAL { get; set; }
            public decimal REAL { get; set; }
        }

        /// <summary>
        /// Cria mock para tabela dinamica Visão Geral da área
        /// 
        /// Params(bool): tbl2: caso seja tabela 2, para alterar o onclick da TD de dados.
        /// 
        /// ----------------------
        /// Query Headers (Region > Cabecalhos):
        /// 
        /// 1º Devem ser declaradas quantas LINHAS possuem no cabeçalho e seus NOMES:
        /// Query:
        ///     Coluna    |   Tipagem
        ///               |
        ///     name      |   string
        ///     coolspan  |   int
        /// Objeto de retorno: Ths.
        /// 
        /// 2º PARA CADA OBJETO DO ITEM 1, DEVE EXISTIR SEU CORRESPONDENTE DESTE ITEM. Query com os valores  > Ths:
        /// Query:
        ///     Coluna    |   Tipagem
        ///               |
        ///     name      |   string
        ///     coolspan  |   int
        ///     
        /// Objeto de retorno: Ths.
        /// 
        /// 
        /// 
        ///     Ex:
        ///     
        ///     
        /// 
        /// ----------------------
        /// 
        /// ----------------------
        /// Query Body (Region > Meio):
        /// 
        /// 
        /// ----------------------
        /// ----------------------
        /// Query Footer (Region > Rodapé):
        /// 
        /// 
        /// ----------------------
        /// 
        /// 
        /// Return "TabelaDinamicaResultados" object.
        /// </summary>
        /// <param name="tbl2"></param>
        /// <returns></returns>
        public static TabelaDinamicaResultados MockTabelaVisaoGeralDaArea()
        {
            var tabela = new TabelaDinamicaResultados();

            #region Cabecalhos

            /*1º*/
            tabela.trsCabecalho1 = new List<Ths>();

            tabela.trsCabecalho1.Add(new Ths()
            {
                name = " "
            });

            tabela.trsCabecalho1.Add(new Ths()
            {
                name = "Pacotes"
            });
            /*Fim  1º*/

            /*2º CRIANDO CABECALHO DA SEGUNDA TABELA*/
            tabela.trsCabecalho2 = new List<Ths>();
            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg1", //TITULO DO AGRUPAMENTO EX: REG1, REG2, ETC...
                coolspan = 4,
                //tds = thsMeio
            });

            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg2",
                coolspan = 4,
                //tds = thsMeio
            });

            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg3",
                coolspan = 4,
                //tds = thsMeio
            });


            var thsMeio = new List<Ths>();
            thsMeio.Add(new Ths() { name = "Orçado", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Real", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio %", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio $", coolspan = 1 });

            foreach (var i in tabela.trsCabecalho2)
            {
                i.tds = thsMeio; //ESTA PROPERTY DEVE CONTER OS ITENS AGRUPADOS (EX: OÇADO, REAL, DESVIO ETC....)
            }

            tabela.trsCabecalho3 = new List<Ths>();
            tabela.trsCabecalho3.Add(new Ths()
            {
                name = "Total",
                coolspan = 4,
                tds = thsMeio
            });

            /*Fim  2º*/
            #endregion

            #region Meio

            tabela.trsMeio = new List<Trs>();

            var tdsEsquerda1 = new List<Tds>();
            var tdsEsquerda2 = new List<Tds>();
            var tdsEsquerda3 = new List<Tds>();

            for (int i = 0; i < 4 * 3; i++)
            {
                tdsEsquerda1.Add(new Tds() { valor = (1M * i).ToString(), coolspan = 1 });

                tdsEsquerda2.Add(new Tds() { valor = (6M * i).ToString(), coolspan = 1 });

                tdsEsquerda3.Add(new Tds() { valor = (9M * i).ToString(), coolspan = 1 });
            }

            var tdsDireita1 = new List<Tds>();
            var tdsDireita2 = new List<Tds>();
            var tdsDireita3 = new List<Tds>();

            for (int i = 0; i < 4; i++)
            {
                tdsDireita1.Add(new Tds() { valor = (i * 90M).ToString(), coolspan = 1 });

                tdsDireita2.Add(new Tds() { valor = (i * 900M).ToString(), coolspan = 1 });

                tdsDireita3.Add(new Tds() { valor = (i * 9000M).ToString(), coolspan = 1 });

            }

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste1",
                tdsEsquerda = tdsEsquerda1,
                tdsDireita = tdsDireita1
            });

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste2",
                tdsEsquerda = tdsEsquerda2,
                tdsDireita = tdsDireita2

            });

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste3",
                tdsEsquerda = tdsEsquerda3,
                tdsDireita = tdsDireita3

            });




            #endregion

            #region Rodapé

            tabela.footer = new List<Trs>();

            var tdsDoFooter2 = new List<Tds>();
            for (int i = 0; i < 4 * 3; i++)
            {
                tdsDoFooter2.Add(new Tds()
                {
                    coolspan = 1,
                    valor = (1.2M * i).ToString()
                });
            }

            var tdsDoFooter3 = new List<Tds>();
            for (int i = 0; i < 4 * 1; i++)
            {
                tdsDoFooter3.Add(new Tds()
                {
                    coolspan = 1,
                    valor = (2.2M * i).ToString()
                });
            }

            tabela.footer.Add(new Trs()
            {
                name = "Total:",
                coolspan = 4,
                tdsEsquerda = tdsDoFooter2,
                tdsDireita = tdsDoFooter3

            });

            #endregion

            return tabela;
        }

        #endregion

    }

}
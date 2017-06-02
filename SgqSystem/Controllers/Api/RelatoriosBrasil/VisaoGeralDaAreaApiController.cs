using Dominio;
using DTO;
using DTO.Helpers;
using DTO.ResultSet;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/VisaoGeralDaArea")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VisaoGeralDaAreaApiController : ApiController
    {

        private List<VisaoGeralDaAreaResultSet> _mock { get; set; }
        private List<VisaoGeralDaAreaResultSet> _list { get; set; }

        [HttpPost]
        [Route("Grafico1")]
        public List<VisaoGeralDaAreaResultSet> Grafico1([FromBody] DataCarrierFormulario form)
        {
            CriaMockG1(form);
            //return _mock;
            return _list;
        }

        [HttpPost]
        [Route("Grafico2")]
        public List<VisaoGeralDaAreaResultSet> Grafico2([FromBody] DataCarrierFormulario form)
        {
            CriaMockG2(form);
            //return _mock;
            return _list;
        }

        [HttpPost]
        [Route("Grafico3")]
        public List<VisaoGeralDaAreaResultSet> Grafico3([FromBody] DataCarrierFormulario form)
        {
            CriaMockG3(form);
            return _mock;
        }

        [HttpPost]
        [Route("Grafico4")]
        public List<VisaoGeralDaAreaResultSet> Grafico4([FromBody] DataCarrierFormulario form)
        {
            CriaMockG4(form);
            return _mock;
        }

        [HttpPost]
        [Route("Grafico5")]
        public List<VisaoGeralDaAreaResultSet> Grafico5([FromBody] DataCarrierFormulario form)
        {
            CriaMockG5(form);
            return _mock;
        }

        public static string sqlBase(DataCarrierFormulario form)
        {
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
"\n       < 0.7                                                                                                                                                                                                                                                           					                                               " +
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
"\n  																																																																						                                               " +
"\n     ";

            return query;

        }

        /// <summary>
        /// A query para o grafico1 da Visão geral da área SGQ deve retornar:
        ///   
        ///   Coluna            |   Tipagem
        ///                     |
        ///   regId             |   Int
        ///   regName           |   string
        ///   scorecardJbs      |   decimal
        ///   scorecardJbsReg   |   decimal
        ///   
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG1(DataCarrierFormulario form)
        {
            //_mock = new List<VisaoGeralDaAreaResultSet>();

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    regId = 1,
            //    regName = "reg1",
            //    scorecardJbs = 80M,
            //    scorecardJbsReg = 26.5M
            //});

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    regId = 2,
            //    regName = "reg2",
            //    scorecardJbs = 80M,
            //    scorecardJbsReg = 66.2M
            //});

            _list = new List<VisaoGeralDaAreaResultSet>();

            string query = VisaoGeralDaAreaApiController.sqlBase(form) +

"\n  																																																																						                                               " +
"\n   SELECT                                                                                                                                                                                                                                                                                                                               " +
"\n   Reg.Name regName,                                                                                                                                                                                                                                                                                                                " +
"\n   Reg.Id regId,                                                                                                                                                                                                                                                                                                                      " +
"\n   case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbs,                                                                                                                                                                               " +
"\n   case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbsReg                                                                                                                                                                             " +
"\n   FROM ParStructure Reg                                                                                                                                                                                                                                                                                                                " +
"\n   left join #SCORE S                                                                                                                                                                                                                                                                                                                   " +
"\n   on S.Regional = Reg.Id                                                                                                                                                                                                                                                                                                               " +
"\n   where                                                                                                                                                                                                                                                                                                            " +
"\n   Reg.Active = 1 and Reg.ParStructureGroup_Id = 2                                                                                                                                                                                                                                                                                                                  " +
"\n   GROUP BY Reg.Name, Reg.id ORDER BY 4 DESC                                                                                                                                                                                                                                                                                                      ";
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
            using (var db = new SgqDbDevEntities())                                                                                                                                                                                                                                                                                        
            {
                _list = db.Database.SqlQuery<VisaoGeralDaAreaResultSet>(query).ToList();
            }

            //return _list;

        }

        /// <summary>
        /// A query para o grafico2 da Visão geral da área SGQ deve retornar:
        /// 
        ///    Coluna            |   Tipagem
        ///                      |
        ///    companySigla      |   string
        ///    companyScorecard  |   decimal
        ///    scorecardJbs      |   decimal
        ///    scorecardJbsReg   |   decimal
        /// 
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG2(DataCarrierFormulario form)
        {
            //_mock = new List<VisaoGeralDaAreaResultSet>();

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    companySigla = "Com",
            //    companyScorecard = 90M,
            //    scorecardJbs = 80M,
            //    scorecardJbsReg = 66.2M
            //});

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    companySigla = "Pan",
            //    companyScorecard = 70M,
            //    scorecardJbs = 80M,
            //    scorecardJbsReg = 66.2M
            //});

            _list = new List<VisaoGeralDaAreaResultSet>();

            string query = VisaoGeralDaAreaApiController.sqlBase(form) +

"\n  SELECT " +
"\n  C.Initials companySigla, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end companyScorecard, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbs, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbsReg " +
"\n  FROM ParStructure Reg " +
"\n  LEFT JOIN ParCompanyXStructure CS " +
"\n  ON CS.ParStructure_Id = Reg.Id " +
"\n  left join ParCompany C " +
"\n  on C.Id = CS.ParCompany_Id "+
"\n  left join #SCORE S  " +
"\n  on C.Id = S.ParCompany_Id " +
"\n  where Reg.Id = '" + form.Query.ToString() + "'" +
"\n  GROUP BY S.ParCompany_Id, S.ParCompanyName, C.Initials ORDER BY 2 DESC ";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<VisaoGeralDaAreaResultSet>(query).ToList();
            }



        }

        /// <summary>
        /// A query para o grafico3 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      date           |   datetime
        ///      
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        /// <param name="form"></param>
        private void CriaMockG3(DataCarrierFormulario form)
        {
            //var primeiroDiaMesAnterior = Guard.PrimeiroDiaMesAnterior(form._dataInicio);
            //var proximoDomingo = Guard.GetNextWeekday(form._dataFim, DayOfWeek.Sunday);

            //_mock = new List<VisaoGeralDaAreaResultSet>();

            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    nc = 10M,
            //    procentagemNc = 90M,
            //    date = proximoDomingo.AddDays(-8)
            //});
            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    nc = 50M,
            //    av = 50M,
            //    procentagemNc = 40M,
            //    date = proximoDomingo.AddDays(-9)
            //});
            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    nc = 20M,
            //    av = 150M,
            //    procentagemNc = 50M,
            //    date = proximoDomingo.AddDays(-18)
            //});
            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    nc = 90M,
            //    av = 200M,
            //    procentagemNc = 90M,
            //    date = proximoDomingo.AddDays(-15)
            //});
            //_mock.Add(new VisaoGeralDaAreaResultSet()
            //{
            //    nc = 120M,
            //    av = 75M,
            //    procentagemNc = 20M,
            //    date = proximoDomingo.AddDays(-22)
            //});

            //for (DateTime i = primeiroDiaMesAnterior; i < proximoDomingo; i = i.AddDays(1))
            //{
            //    if (_mock.FirstOrDefault(r => r.date == i) == null)
            //    {
            //        _mock.Add(new VisaoGeralDaAreaResultSet()
            //        {
            //            nc = 0M,
            //            av = 0M,
            //            procentagemNc = 0M,
            //            date = i
            //        });

            //        //_mock.Add(new VisaoGeralDaAreaResultSet());

            //    }
            //}
            //_mock = _mock.OrderBy(r => r.date).ToList();
            _list = new List<VisaoGeralDaAreaResultSet>();

            string query = VisaoGeralDaAreaApiController.sqlBase(form) +

                 " \n DECLARE @dataFim_ date = '" + form._dataFimSQL + "' " +
                 " \n DECLARE @dataInicio_ date = DATEADD(MONTH, -1, @dataFim_) " +
                 " \n SET @dataInicio_ = datefromparts(year(@dataInicio_), month(@dataInicio_), 01) " +
                 " \n declare @ListaDatas_ table(data_ date) " +
                 " \n WHILE @dataInicio_ <= @dataFim_ " +
                 " \n BEGIN " +
                 " \n INSERT INTO @ListaDatas_ " +
                 " \n SELECT @dataInicio_ " +
                 " \n SET @dataInicio_ = DATEADD(DAY, 1, @dataInicio_) " +
                 " \n END " +


                 " \n SET @DATAFINAL = @dataFim_ " +
                 " \n SET @DATAINICIAL = DateAdd(mm, DateDiff(mm, 0, @DATAFINAL) - 1, 0) " +
                 " \n DECLARE @UNIDADE INT = " + form.unitId + " " +



                 " \n CREATE TABLE #AMOSTRATIPO4a (  " +
                 " \n UNIDADE INT NULL,  " +
                 " \n INDICADOR INT NULL,  " +
                 " \n AM INT NULL,  " +
                 " \n DEF_AM INT NULL " +
                 " \n )  " +
                 " \n INSERT INTO #AMOSTRATIPO4a  " +
                 " \n SELECT " +
                  " \n UNIDADE, INDICADOR, " +
                 " \n COUNT(1) AM " +
                 " \n ,SUM(DEF_AM) DEF_AM " +
                 " \n FROM " +
                 " \n ( " +
                     " \n SELECT " +
                     " \n cast(C2.CollectionDate as DATE) AS DATA " +
                     " \n , C.Id AS UNIDADE " +
                     " \n , C2.ParLevel1_Id AS INDICADOR " +
                     " \n , C2.EvaluationNumber AS AV " +
                     " \n , C2.Sample AS AM " +
                     " \n , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                     " \n FROM CollectionLevel2 C2 (nolock) " +
                     " \n INNER JOIN ParLevel1 L1 (nolock) " +
                     " \n ON L1.Id = C2.ParLevel1_Id " +
                     " \n INNER JOIN ParCompany C (nolock) " +
                     " \n ON C.Id = C2.UnitId " +
                     " \n where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL " +
                     " \n and C2.NotEvaluatedIs = 0 " +
                     " \n and C2.Duplicated = 0 " +
                     " \n and L1.ParConsolidationType_Id = 4 " +
                     " \n group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) " +
                 " \n ) TAB " +
                 " \n GROUP BY UNIDADE, INDICADOR " +

                 " \n DECLARE @RESS INT " +
                 " \n SELECT " +
                       " \n @RESS = " +
                         " \n COUNT(1) " +
                         " \n FROM " +
                         " \n ( " +
                         " \n SELECT " +
                         " \n COUNT(1) AS NA " +
                         " \n FROM CollectionLevel2 C2 (nolock) " +
                         " \n LEFT JOIN Result_Level3 C3 (nolock) " +
                         " \n ON C3.CollectionLevel2_Id = C2.Id " +
                         " \n WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL " +
                         " \n AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 (nolock) where Hashkey = 1) " +
                         " \n AND C2.UnitId = @UNIDADE " +
                         " \n AND IsNotEvaluate = 1 " +
                         " \n GROUP BY C2.ID " +
                         " \n ) NA " +
                         " \n WHERE NA = 2 " +

                 " \n SELECT " +
                  " \n level1_Id " +
                 " \n , Level1Name " +
                 " \n , Level2Name AS Level2Name " +
                  " \n , Unidade_Id " +
                  " \n , Unidade " +
                  " \n , ProcentagemNc " +
                  " \n ,(case when IsRuleConformity = 1 THEN(100 - META) WHEN IsRuleConformity IS NULL THEN 0 ELSE Meta END) AS Meta " +
                 " \n , NcSemPeso as NC " +
                 " \n ,AvSemPeso as Av " +
                 " \n ,Data AS _Data " +
                 " \n FROM " +
                 " \n ( " +
                    " \n  SELECT " +
                     " \n * " +

                     " \n , CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC / AV * 100 END AS ProcentagemNc " +
                     " \n , CASE WHEN CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC / AV * 100 END >= (case when IsRuleConformity = 1 THEN(100 - META) ELSE Meta END) THEN 1 ELSE 0 END RELATORIO_DIARIO " +

                     " \n FROM " +
                     " \n ( " +
                         " \n SELECT " +

                          " \n NOMES.A1 AS level1_Id--IND.Id         AS level1_Id " +
                         " \n , NOMES.A2 AS Level1Name--IND.Name     AS Level1Name " +
                        " \n , 'Tendência do Indicador ' + NOMES.A2 AS Level2Name " +
                        " \n , IND.IsRuleConformity " +
                         " \n , NOMES.A4 AS Unidade_Id--UNI.Id            AS Unidade_Id " +
                         " \n , NOMES.A5 AS Unidade--UNI.Name     AS Unidade " +
                         " \n , CASE " +
                         " \n WHEN IND.HashKey = 1 THEN(SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b (nolock) WHERE ParCompany_id = UNI.Id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL) " +
                         " \n WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation " +
                         " \n WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
                         " \n WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
                         " \n WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM " +
                         " \n ELSE 0 " +
                        " \n END  AS Av " +
                       " \n , CASE " +
                         " \n WHEN IND.HashKey = 1 THEN(SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b (nolock) WHERE ParCompany_id = UNI.Id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL) " +
                         " \n WHEN IND.ParConsolidationType_Id = 1 THEN EvaluateTotal " +
                         " \n WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
                         " \n WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
                         " \n WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM " +
                         " \n ELSE 0 " +
                        " \n END AS AvSemPeso " +
                         " \n , CASE " +
                         " \n WHEN IND.ParConsolidationType_Id = 1 THEN WeiDefects " +
                         " \n WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects " +
                         " \n WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult " +
                         " \n WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM " +
                         " \n ELSE 0 " +
                         " \n END AS NC " +
                         " \n , CASE " +
                         " \n WHEN IND.ParConsolidationType_Id = 1 THEN DefectsTotal " +
                         " \n WHEN IND.ParConsolidationType_Id = 2 THEN DefectsTotal " +
                         " \n WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult " +
                         " \n WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM " +
                         " \n ELSE 0 " +
                         " \n END AS NCSemPeso " +
                  " \n , " +
                  " \n CASE " +


                     " \n WHEN(SELECT COUNT(1) FROM ParGoal G (nolock) WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN " +
                         " \n (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock) WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC) " +


                     " \n ELSE " +
                         " \n (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock) WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC) " +
                  " \n END " +
                  " \n AS Meta " +
                         " \n --, CL1.ConsolidationDate as Data " +
                         " \n , DD.Data_ as Data " +

                        " \n FROM @ListaDatas_ DD " +

                         " \n LEFT JOIN(SELECT * FROM ConsolidationLevel1 (nolock) WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL AND UnitId <> 12341614) CL1 " +

                         " \n ON DD.Data_ = CL1.ConsolidationDate " +

                         " \n LEFT JOIN ParLevel1 IND (nolock) " +

                         " \n ON IND.Id = CL1.ParLevel1_Id--AND IND.ID = 1 " +

                         " \n LEFT JOIN ParCompany UNI (nolock) " +

                         " \n ON UNI.Id = CL1.UnitId " +
                         " \n LEFT JOIN #AMOSTRATIPO4a A4 (nolock)  " +
                         " \n ON A4.UNIDADE = UNI.Id " +
                         " \n AND A4.INDICADOR = IND.ID " +


                         " \n LEFT JOIN " +
                         " \n ( " +
                             " \n SELECT " +

                             " \n IND.ID A1, " +
                             " \n IND.NAME A2, " +
                             " \n 'Tendência do Indicador ' + IND.NAME AS A3, " +
                             " \n CL1.UnitId A4, " +
                             " \n UNI.NAME A5, " +
                             " \n 0 AS A6 " +


                             " \n FROM(SELECT * FROM ConsolidationLevel1 (nolock) WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL AND UnitId <> 11514) CL1 " +


                              " \n LEFT JOIN ParLevel1 IND (nolock) " +

                              " \n ON IND.Id = CL1.ParLevel1_Id--AND IND.ID = 1 " +

                             " \n LEFT JOIN ParCompany UNI (nolock) " +

                              " \n ON UNI.Id = CL1.UnitId " +

                             " \n LEFT JOIN #AMOSTRATIPO4a A4 (nolock)  " +

                             " \n ON A4.UNIDADE = UNI.Id " +

                             " \n AND A4.INDICADOR = IND.ID " +


                             " \n GROUP BY " +

                             " \n IND.ID, " +
                             " \n IND.NAME, " +
                             " \n CL1.UnitId, " +
                             " \n UNI.NAME " +

                         " \n ) NOMES " +

                         " \n ON 1 = 1 AND(NOMES.A1 = CL1.ParLevel1_Id AND NOMES.A4 = UNI.ID) OR(IND.ID IS NULL) " +



                    " \n ) S1 " +
                 " \n ) S2 " +
                 " \n WHERE RELATORIO_DIARIO = 1 OR(RELATORIO_DIARIO = 0 AND AV = 0) " +
                  " \n DROP TABLE #AMOSTRATIPO4a  ";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<VisaoGeralDaAreaResultSet>(query).ToList();
            }


        }

        /// <summary>
        /// A query para o grafico4 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      av             |   decimal
        ///      level1Name     |   string
        ///      level2Name     |   string
        ///      level1Id       |   int
        ///      level2Id       |   int
        ///      
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG4(DataCarrierFormulario form)
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 10M,
                procentagemNc = 50M,
                av = 20M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level1Id = 1,
                level2Id = 1,
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 50M,
                procentagemNc = 33.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name2",
                level1Id = 1,
                level2Id = 2,
            });

        }

        /// <summary>
        /// A query para o grafico5 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      av             |   decimal
        ///      level1Name     |   string
        ///      level2Name     |   string
        ///      level3Name     |   string
        ///      level1Id       |   int
        ///      level2Id       |   int
        ///      level3Id       |   int
        /// 
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG5(DataCarrierFormulario form)
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 10M,
                procentagemNc = 50M,
                av = 20M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name1",
                level1Id = 1,
                level2Id = 1,
                level3Id = 1,
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 50M,
                procentagemNc = 33.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name2",
                level1Id = 1,
                level2Id = 1,
                level3Id = 2,
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 102M,
                procentagemNc = 90.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name3",
                level1Id = 1,
                level2Id = 1,
                level3Id = 3,
            });

        }
    }

}

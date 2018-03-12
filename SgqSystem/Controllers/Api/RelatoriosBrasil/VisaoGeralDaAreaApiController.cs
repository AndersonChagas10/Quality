using ADOFactory;
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
using static SgqSystem.Controllers.RelatoriosSgqController;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/VisaoGeralDaArea")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VisaoGeralDaAreaApiController : ApiController
    {

        private List<VisaoGeralDaAreaResultSet> _mock { get; set; }
        private List<VisaoGeralDaAreaResultSet> _list { get; set; }
        private List<ResultQueryEvolutivo> _listEvol { get; set; }
        private List<ResultQueryEvolutivo> _listEvolReg { get; set; }
        private List<ListResultQueryEvolutivo> listaRegs { get; set; }
        private List<ListResultQueryEvolutivo> listaUnidades { get; set; }

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
            return _list;
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

        public static string sqlBase(DataCarrierFormulario form, bool evolutivo = false)
        {
            DateTime dtIni = form._dataInicio;
            if (evolutivo)
            {
                var year = dtIni.Year - 1;
                dtIni = new DateTime(year, dtIni.Month, form._dataFim.Day);
            }


            /*
             * neste score devo considerar a regra dos 70 %
             * 
             */
            var query = "" +

 // "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'    " +
 // "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n  IF OBJECT_ID('tempdb.dbo.#SCORE', 'U') IS NOT NULL																																																										                                               " +
 "\n    DROP TABLE #SCORE; 																																																																	                                               " +
 "\n  																																																																						                                               " +
 "\n  CREATE TABLE #SCORE (																																																																	                                               " +
 "\n  	Cluster int null,																																																																	                                               " +
 "\n  	ClusterName Varchar(153) null,																																																														                                               " +
 "\n  	Regional int null,																																																																	                                               " +
 "\n  	RegionalName Varchar(153) null, 																																																													                                               " +
 "\n  	ParCompany_Id int null,																																																																                                               " +
 "\n  	ParCompanyName Varchar(153) null, 																																																													                                               " +
 "\n  	TipoIndicador int null,																																																																                                               " +
 "\n  	TipoIndicadorName Varchar(153) null, 																																																												                                               " +
 "\n  	Level1Id int null,																																																																	                                               " +
 "\n  	Level1Name Varchar(153) null, 																																																														                                               " +
 "\n  	Criterio int null,																																																																	                                               " +
 "\n  	CriterioName Varchar(153) null, 																																																													                                               " +
 "\n  	Av decimal(30,5) null,																																																																                                               " +
 "\n  	Nc decimal(30,5) null,																																																																                                               " +
 "\n  	Pontos decimal(30,5) null,																																																															                                               " +
 "\n  	PontosIndicador decimal(30,5) null,																																																													                                               " +
 "\n  	Meta decimal(30,5) null,																																																															                                               " +
 "\n  	Real decimal(30,5) null,																																																															                                               " +
 "\n  	PontosAtingidos decimal(30,5) null,																																																													                                               " +
 "\n  	Scorecard decimal(30,5) null,																																																														                                               " +
 "\n  	TipoScore Varchar(153) null," +
 "\n mesData datetime2(7) null  " +
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

@"            SELECT 

                CL1.id,
            	CL1.ConsolidationDate,
            	CL1.UnitId,
            	CL1.ParLevel1_Id,
            	CL1.DefectsResult,
            	CL1.WeiDefects,
            	CL1.EvaluatedResult,
            	CL1.WeiEvaluation,
            	CL1.EvaluateTotal,
            	CL1.TotalLevel3WithDefects,
            	CL1.DefectsTotal
            INTO #ConsolidationLevel
            FROM ConsolidationLevel1 CL1 WITH(NOLOCK)
            WHERE 1 = 1
            AND CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL

            CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_level1 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId); 
            CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id); " +


                // Alteração
                "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                 "\n UNIDADE INT NULL, " +
                 "\n INDICADOR INT NULL, " +
                 "\n AM INT NULL, " +
                 "\n DEF_AM INT NULL " +
                 "\n ) " +


                 "\n INSERT INTO #AMOSTRATIPO4 " +
                 /*
                 "\n SELECT " +
                 "\n  UNIDADE, INDICADOR, " +
                 "\n FROM " +
                 "\n ( " +
                 */
                 "\n     SELECT " +
                 //"\n     cast(C2.CollectionDate as DATE) AS DATA " +
                 "\n     C.Id AS UNIDADE " +
                 "\n     , C2.ParLevel1_Id AS INDICADOR " +
                 "\n , COUNT(DISTINCT CONCAT(C2.EvaluationNumber, C2.Sample, cast(cast(C2.CollectionDate as date) as varchar))) AM " +
                 "\n , SUM(IIF(C2.WeiDefects = 0, 0, 1)) DEF_AM " +
                 //"\n     , C2.EvaluationNumber AS AV " +
                 // "\n     , C2.Sample AS AM " +
                 //"\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                 "\n     FROM CollectionLevel2 C2 (nolock) " +
                 "\n     INNER JOIN ParLevel1 L1 (nolock)  " +
                 "\n     ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1" +

                 "\n     INNER JOIN ParCompany C (nolock)  " +
                 "\n     ON C.Id = C2.UnitId " +
                 "\n     where C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                 "\n     and C2.NotEvaluatedIs = 0 " +
                 "\n     and C2.Duplicated = 0 " +
                 "\n     and L1.ParConsolidationType_Id = 4 " +
                 "\n     group by C.Id, ParLevel1_Id" +
            /*
            "\n ) TAB " +
            "\n GROUP BY UNIDADE, INDICADOR " +
            */



            "\n                                                                                                                                                                                                                                                                     " +
            "\n DECLARE @VOLUMEPCC INT                                                                                                                                                                                                                                              " +
            "\n DECLARE @DIASABATE INT                                                                                                                                                                                                                                              " +
            "\n DECLARE @DIASDEVERIFICACAO INT                                                                                                                                                                                                                                      " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n DECLARE @AVFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                " +
            "\n DECLARE @NCFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                " +
            "\n                                                                                                                                                                                                                                                                     " +

 "\n   /* INICIO DADOS DA FREQUENCIA ------------------------------------------------------*/                                                                                                                                                                              					                                               " +
 "\n                                                                                                                                                                                                                                                                       					                                               " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n CREATE TABLE #FREQ (                                                                                                                                                                                                                                                                                                                   " +
 "\n    clusterId int null                                                                                                                                                                                                                                                                                                                  " +
 "\n    ,     cluster varchar(255) null                                                                                                                                                                                                                                                                                                     " +
 "\n    , regionalId int null                                                                                                                                                                                                                                                                                                               " +
 "\n    , regional varchar(255) null                                                                                                                                                                                                                                                                                                        " +
 "\n    , unitId int null                                                                                                                                                                                                                                                                                                                   " +
 "\n    , unidade  varchar(255) null                                                                                                                                                                                                                                                                                                        " +
 "\n    , criticalLevelId  int null                                                                                                                                                                                                                                                                                                         " +
 "\n    , criticalLevel varchar(255) null                                                                                                                                                                                                                                                                                                   " +
 "\n    , pontos decimal(35, 10) null                                                                                                                                                                                                                                                                                                       " +
 "\n    )                                                                                                                                                                                                                                                                                                                                   " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                 					                                                                                   " +
 "\n                                                                                                                                                                                                                                                                        					                                               " +
 "\n    insert into #freq                                                                                                                                                                                                                                                                                                                   " +
 "\n    SELECT                                                                                                                                                                                                                                                                                                                              " +
 "\n    CL.Id                                                                                                                                                                                                                                                                                                                               " +
 "\n    , CL.Name                                                                                                                                                                                                                                                                                                                           " +
 "\n    , S.Id                                                                                                                                                                                                                                                                                                                              " +
 "\n    , S.Name                                                                                                                                                                                                                                                                                                                            " +
 "\n    , C.Id                                                                                                                                                                                                                                                                                                                              " +
 "\n    , C.Name                                                                                                                                                                                                                                                                                                                            " +
 "\n    , L1C.ParCriticalLevel_Id                                                                                                                                                                                                                                                                                                           " +
 "\n    , CRL.Name                                                                                                                                                                                                                                                                                                                          " +
 "\n    , L1C.Points                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n    FROM ParCompany C                                                                                                                                                                                                                                                                                                                   " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n    LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                                                                                   " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n           ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                                                                                   " +
 "\n    LEFT JOIN ParStructure S                                                                                                                                                                                                                                                                                                            " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n           ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                                                                                 " +
 "\n    LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                                                                                      " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n           ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                                                                                            " +
 "\n    LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                                                                                     " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n           ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                                                              " +
 "\n    LEFT JOIN ParCluster CL                                                                                                                                                                                                                                                                                                             " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n           ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                                                                                 " +
 "\n    LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                                                                                     " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n           ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1                                                                                                                                                                                                                                                " +
 "\n    LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                                                                                      " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n           ON L1C.ParCriticalLevel_Id = CRL.Id                                                                                                                                                                                                                                                                                          " +
 "\n    WHERE C.Id >= 1                                                                                                                                                                                                                                                                                                                     " +
 "\n    AND L1C.ParLevel1_Id = 25                                                                                                                                                                                                                                                                                                           " +
 "\n                                                                                                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                       					                                               " +
 "\n   /* FIM DOS DADOS DA FREQUENCIA -----------------------------------------------------*/                                                                                                                                                                              					                                               " +
 "\n   CREATE TABLE #VOLUMES (                                                                                                                                                                                                                                                                                                              " +
 "\n 	DIASABATE INT NULL,                                                                                                                                                                                                                                                                                                                " +
 "\n 	VOLUMEPCC INT NULL, unitid int null                                                                                                                                                                                                                                                                                                                 " +
 "\n   )                                                                                                                                                                                                                                                                                                                                    " +
 "\n   INSERT INTO #VOLUMES                                                                                                                                                                                                                                                                    					                           " +
 "\n   SELECT COUNT(1) AS DIASABATE, SUM(Quartos) AS VOLUMEPCC, ParCompany_id as UnitId FROM VolumePcc1b WHERE Data BETWEEN @DATAINICIAL AND @DATAFINAL GROUP BY ParCompany_id                                                                                                  					                                                                   " +
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
 "\n   GROUP BY UnitId    " +
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
 "\n                AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)                                                                                                                                                                             						                                           " +
 "\n                --AND C2.UnitId = @ParCompany_Id                                                                                                                                                                                                                       						                                           " +
 "\n                AND IsNotEvaluate = 1                                                                                                                                                                                                                                						                                           " +
 "\n                GROUP BY C2.ID, C2.UnitId                                                                                                                                                                                                                                       						                                   " +
 "\n            ) NA		                                                                                                                                                                                                                                                        						                                   " +
 "\n            WHERE NA = 2                                                                                                                                                                                                                                             						                                           " +
 "\n 		   GROUP BY UnitId                                                                                                                                                                                                                                                                                                                                                                                                                                " +
 "\n   																																																																						                                               " +
 "\n   INSERT INTO #SCORE																																						" +
 "\n SELECT                                                                                                                                                                                                                                                                                                                                                        " +
 "\n   Cluster                                                                                                                                                                                                                                                                                                                                                     " +
 "\n  , ClusterName                                                                                                                                                                                                                                                                                                                                                " +
 "\n  , Regional                                                                                                                                                                                                                                                                                                                                                   " +
 "\n  , RegionalName                                                                                                                                                                                                                                                                                                                                               " +
 "\n  , ParCompanyId                                                                                                                                                                                                                                                                                                                                              " +
 "\n  , ParCompanyName                                                                                                                                                                                                                                                                                                                                             " +
 "\n  , TipoIndicador                                                                                                                                                                                                                                                                                                                                              " +
 "\n  , TipoIndicadorName                                                                                                                                                                                                                                                                                                                                          " +
 "\n  , Level1Id                                                                                                                                                                                                                                                                                                                                                   " +
 "\n  , Level1Name                                                                                                                                                                                                                                                                                                                                                 " +
 "\n  , Criterio                                                                                                                                                                                                                                                                                                                                                   " +
 "\n  , CriterioName                                                                                                                                                                                                                                                                                                                                               " +
 "\n  , case when Level1Id = 3 then avg(av) else sum(av) end av                                                                                                                                                                                                                                                                                                                                                 " +
 "\n  , sum(nc) nc                                                                                                                                                                                                                                                                                                                                                 " +
 "\n  , (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) pontos                                                                                                                                                                                                                                                                                                                                         " +
 "\n  , (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) pontosIndicador                                                                                                                                                                                                                                                                                                                                " +
 "\n  , max(meta) meta                                                                                                                                                                                                                                                                                                                                             " +

 "\n  , round(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) * 100) end end, 2) real " +
 "\n  , case when(isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) > 100 then 100 else (isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) end / 100 * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) as pontosAtingidos " +
 "\n  , case when(isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) > 100 then 100 else (isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) end as Scorecard " +

 "\n  , TipoScore " +
 "\n  , max(mesData) mesData FROM                                                                                                                                                                                                                                           " +
            "\n (                                                                                                                                                                                                                                                                   " +
            "\n SELECT                                                                                                                                                                                                                                                              " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n   Cluster                                                                                                                                                                                                                                                           " +
            "\n  , ClusterName                                                                                                                                                                                                                                                      " +
            "\n  , Regional                                                                                                                                                                                                                                                         " +
            "\n  , RegionalName                                                                                                                                                                                                                                                     " +
            "\n  , ParCompanyId                                                                                                                                                                                                                                                     " +
            "\n  , ParCompanyName                                                                                                                                                                                                                                                   " +
            "\n  , CASE WHEN TipoIndicador = 0 THEN 1 ELSE 2 END TipoIndicador                                                                                                                                                                                                      " +
            "\n  , CASE WHEN TipoIndicador = 0 THEN 'Menor' ELSE 'Maior' END TipoIndicadorName                                                                                                                                                                                      " +
            "\n  , Level1Id                                                                                                                                                                                                                                                         " +
            "\n  , Level1Name                                                                                                                                                                                                                                                       " +
            "\n  , Criterio                                                                                                                                                                                                                                                         " +
            "\n  , CriterioName                                                                                                                                                                                                                                                     " +
            "\n  , AV AV                                                                                                                                                                                                                                                               " +
            "\n  , CASE WHEN Level1Id = 25 THEN AV - NC ELSE NC END NC /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                                                                             " +
            "\n  , Pontos Pontos                                                                                                                                                                                                                                                           " +
            "\n  , CASE WHEN AV = 0 THEN 0 ELSE Pontos END AS PontosIndicador                                                                                                                                                                                                                                        " +
            "\n  , Meta AS Meta                                                                                                                                                                                                                                                             " +
            "\n  , CASE WHEN Level1Id = 25 THEN CASE WHEN AV = 0 THEN 0 ELSE (AV - NC) / AV * 100 END WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 else Meta / NC end * 100 ELSE Real END Real /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                            " +
            "\n  , CASE WHEN Level1Id = 43 AND AV > 0 AND  NC = 0 THEN Pontos ELSE PontosAtingidos END  PontosAtingidos                                                                                                                                                                                                                                               " +
            "\n  , CASE WHEN Level1Id = 43 AND AV > 0 AND NC = 0 THEN 100 ELSE Scorecard END  Scorecard                                                                                                                                                                                                                                                        " +
            "\n  , TipoScore ,mesData                                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n FROM                                                                                                                                                                                                                                                                " +
            "\n (                                                                                                                                                                                                                                                                   " +
            "\n SELECT                                                                                                                                                                                                                                                              " +
            "\n *,                                                                                                                                                                                                                                                                  " +
            "\n /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 " +
            "\n CASE                                                                                                                                                                                                                                                                " +
            "\n     WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 else Meta / NC end                                                                                                                                                                                                                                                                " +
            "\n     WHEN                                                                                                                                                                                                                                                            " +
            "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
            "\n     CASE                                                                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         ELSE                                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         END                                                                                                                                                                                                                                                         " +
            "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
            "\n     > 1                                                                                                                                                                                                                                                             " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     THEN 1                                                                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     ELSE                                                                                                                                                                                                                                                            " +
            "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
            "\n     CASE                                                                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         ELSE                                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         END                                                                                                                                                                                                                                                         " +
            "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
            "\n END * 100                                                                                                                                                                                                                                                           " +
            "\n /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 " +
            "\n AS SCORECARD                                                                                                                                                                                                                                                        " +
            "\n ,                                                                                                                                                                                                                                                                   " +
            "\n /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 " +
            "\n CASE                                                                                                                                                                                                                                                                " +
            "\n     WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 when (Meta / NC) < 0.00 then 0 else Meta / NC end                                                                                                                                                                                                                                                                 " +
            "\n     WHEN                                                                                                                                                                                                                                                            " +
            "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
            "\n     CASE                                                                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         ELSE                                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         END                                                                                                                                                                                                                                                         " +
            "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
            "\n     > 1                                                                                                                                                                                                                                                             " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     THEN 1                                                                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     WHEN                                                                                                                                                                                                                                                            " +
            "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
            "\n     CASE                                                                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         ELSE                                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         END                                                                                                                                                                                                                                                         " +
            "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
            "\n     < 0.00                                                                                                                                                                                                                                                           " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     THEN 0                                                                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     ELSE                                                                                                                                                                                                                                                            " +
            "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
            "\n     CASE                                                                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         ELSE                                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         END                                                                                                                                                                                                                                                         " +
            "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
            "\n END                                                                                                                                                                                                                                                                 " +
            "\n /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 " +
            "\n * /* MULTIPLICAÇÃO */                                                                                                                                                                                                                                               " +
            "\n PONTOS                                                                                                                                                                                                                                                              " +
            "\n AS PONTOSATINGIDOS                                                                                                                                                                                                                                                  " +
            "\n FROM                                                                                                                                                                                                                                                                " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n (                                                                                                                                                                                                                                                                   " +
            "\n SELECT                                                                                                                                                                                                                                                              " +
            "\n                                                                                                                                                                                                                                                                     " +
 //"\n           ISNULL(CL.Id, (SELECT top 1 clusterId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Cluster                                                                                                                                                                      " +
 //"\n , ISNULL(CL.Name, (SELECT top 1 cluster FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS ClusterName                                                                                                                                                                          " +
 //"\n , ISNULL(S.Id, (SELECT top 1 regionalId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Regional                                                                                                                                                                             " +
 //"\n , ISNULL(S.Name, (SELECT top 1 regional FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS RegionalName                                                                                                                                                                         " +
 //"\n , ISNULL(CL1.UnitId, ft.ParCompany_id) AS ParCompanyId                                                                                                                                                                                                                       " +
 //"\n , ISNULL(C.Name, (SELECT top 1 unidade FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS ParCompanyName                                                                                                                                                                        " +
 //"\n , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                                       " +
 //"\n , L1.Id AS Level1Id                                                                                                                                                                                                                                                          " +
 //"\n , L1.Name AS Level1Name                                                                                                                                                                                                                                                      " +
 //"\n , ISNULL(CRL.Id, (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Criterio                                                                                                                                                                      " +
 //"\n , ISNULL(CRL.Name, (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS CriterioName                                                                                                                                                                  " +
 //"\n , ISNULL((select top 1 Points from ParLevel1XCluster aaa (nolock) where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate < @DATAFINAL), (SELECT top 1 pontos FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Pontos                                    " +
 //"\n   , ISNULL(CL1.ConsolidationDate, FT.Data) as mesData                                                                                                                                                                                                                       " +


 "\n           ISNULL(CL.Id, (SELECT top 1 clusterId FROM #FREQ WHERE unitId = 0)) AS Cluster                                                                                                                                                                      " +
   "\n , ISNULL(CL.Name, (SELECT top 1 cluster FROM #FREQ WHERE unitId = 0)) AS ClusterName                                                                                                                                                                          " +
   "\n , ISNULL(S.Id, (SELECT top 1 regionalId FROM #FREQ WHERE unitId = 0)) AS Regional                                                                                                                                                                             " +
   "\n , ISNULL(S.Name, (SELECT top 1 regional FROM #FREQ WHERE unitId = 0)) AS RegionalName                                                                                                                                                                         " +
   "\n , ISNULL(CL1.UnitId, 0) AS ParCompanyId                                                                                                                                                                                                                       " +
   "\n , ISNULL(C.Name, (SELECT top 1 unidade FROM #FREQ WHERE unitId = 0)) AS ParCompanyName                                                                                                                                                                        " +
   "\n , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                                       " +
   "\n , L1.Id AS Level1Id                                                                                                                                                                                                                                                          " +
   "\n , L1.Name AS Level1Name                                                                                                                                                                                                                                                      " +
   "\n , ISNULL(" +
   "( " +

    "\n         SELECT TOP 1 L1Ca.ParCriticalLevel_Id FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

    "\n         WHERE CCL.ParCluster_ID = L1Ca.ParCluster_ID " +

    "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

    "\n             -- AND L1Ca.IsActive = 1 " +

    "\n             AND L1Ca.ValidoApartirDe <= @DATAFINAL " +

    "\n         ORDER BY L1Ca.ValidoApartirDe  desc " +
     "\n	)" +
   "" +
   "\n , (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = 0)) AS Criterio                                                                                                                                                                      " +
   "\n , ISNULL(" +
   "( " +

    "\n         SELECT TOP 1 (select top 1 name from ParCriticalLevel where id = L1Ca.ParCriticalLevel_Id) FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

    "\n         WHERE CCL.ParCluster_ID = L1Ca.ParCluster_ID " +

    "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

    "\n            -- AND L1Ca.IsActive = 1 " +

    "\n             AND L1Ca.ValidoApartirDe <= @DATAFINAL " +

    "\n         ORDER BY L1Ca.ValidoApartirDe  desc " +
     "\n	)" +
   ", (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = 0)) AS CriterioName                                                                                                                                                                  " +
   "\n , ISNULL(" +
   "( " +

    "\n         SELECT TOP 1 L1Ca.Points FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

    "\n         WHERE CCL.ParCluster_ID = L1Ca.ParCluster_ID " +

    "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

    "\n           --  AND L1Ca.IsActive = 1 " +

    "\n             AND L1Ca.ValidoApartirDe <= @DATAFINAL " +

    "\n         ORDER BY L1Ca.ValidoApartirDe desc  " +
     "\n	)" +
   ", (SELECT top 1 pontos FROM #FREQ WHERE unitId = 0)) AS Pontos                                    " +
   "\n   , ISNULL(CL1.ConsolidationDate, '0001-01-01') as mesData                                                                                                                                                                                                                       " +


 "\n                                                                                                                                                                                                                                                                     " +
            "\n  --ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                               " +
            "\n  --, (CL.Name)AS ClusterName                                                                                                                                                                                                                                        " +
            "\n  --, (S.Id)AS Regional                                                                                                                                                                                                                                              " +
            "\n  --, (S.Name)AS RegionalName                                                                                                                                                                                                                                        " +
            "\n  --, (CL1.UnitId)AS ParCompanyId                                                                                                                                                                                                                                    " +
            "\n  --, (C.Name)AS ParCompanyName                                                                                                                                                                                                                                      " +
            "\n  --, L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                           " +
            "\n  --, L1.Id AS Level1Id                                                                                                                                                                                                                                              " +
            "\n  --, L1.Name AS Level1Name                                                                                                                                                                                                                                          " +
            "\n  --, (CRL.Id)AS Criterio                                                                                                                                                                                                                                            " +
            "\n  --, (CRL.Name)AS CriterioName                                                                                                                                                                                                                                      " +
            "\n  --, (L1C.Points)AS Pontos                                                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n  , ST.Name AS TipoScore                                                                                                                                                                                                                                             " +
            "\n  ,                                                                                                                                                                                                                                                                  " +
            "\n   /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                              " +


            "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             ////"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
             "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
             "\n     END                                                                                                                                                                                                                                                               					                                               " +



            "\n   /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                              " +
            "\n   AS AV                                                                                                                                                                                                                                                             " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n  ,                                                                                                                                                                                                                                                                  " +
            "\n   /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                              " +
            "\n   CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                            " +
            "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             ////"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
             "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
             "\n     END                                                                                                                                                                                                                                                               					                                               " +


            "\n         /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                        " +
            "\n         -                                                                                                                                                                                                                                                           " +
            "\n       /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n       CASE                                                                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            //"\n         WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                        " +

            "\n       END                                                                                                                                                                                                                                                           " +
            "\n       /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n    ELSE                                                                                                                                                                                                                                                             " +
            "\n       /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n       CASE                                                                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            //"\n         WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n         WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                        " +

            "\n       END                                                                                                                                                                                                                                                           " +
            "\n       /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n      end                                                                                                                                                                                                                                                           " +
            "\n    /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                             " +
            "\n    AS NC                                                                                                                                                                                                                                                            " +
            "\n  ,                                                                                                                                                                                                                                                                  " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n   CASE                                                                                                                                                                                                                                                              " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     WHEN                                                                                                                                                                                                                                                            " +
            "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
             "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
             "\n     END                                                                                                                                                                                                                                                               					                                               " +

            "\n                                                                                                                                                                                                                                                                 " +
            "\n       /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n       = 0                                                                                                                                                                                                                                                           " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     THEN 0                                                                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n      ELSE                                                                                                                                                                                                                                                           " +
            "\n       /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                          " +
            "\n       CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                        " +
            "\n           /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                      " +
            "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
             "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
             "\n     END                                                                                                                                                                                                                                                               					                                               " +
            "\n             /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                    " +
            "\n             -  /* SUBTRAÇÃO */                                                                                                                                                                                                                                      " +
            "\n                /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                 " +
            "\n           CASE                                                                                                                                                                                                                                                      " +
            "\n                                                                                                                                                                                                                                                                     " +
            //"\n             WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                            " +
            "\n           END                                                                                                                                                                                                                                                       " +
            "\n           /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      " +
            "\n        ELSE                                                                                                                                                                                                                                                         " +
            "\n           /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                      " +
            "\n           CASE                                                                                                                                                                                                                                                      " +
            "\n                                                                                                                                                                                                                                                                     " +
            //"\n             WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n             WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                            " +

            "\n           END                                                                                                                                                                                                                                                       " +
            "\n           /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      " +
            "\n        END                                                                                                                                                                                                                                                         " +
            "\n        /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                         " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        / /*DIVISÃO*/                                                                                                                                                                                                                                                " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
             "\n                                                                                                                                                                                                                                                                       					                                               " +
             "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
             "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
             "\n     END                                                                                                                                                                                                                                                               					                                               " +
            "\n       /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n      END * 100                                                                                                                                                                                                                                                      " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n      AS REAL                                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n  ,                                                                                                                                                                                                                                                                  " +
            "\n  CASE                                                                                                                                                                                                                                                               " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
            "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n     ELSE                                                                                                                                                                                                                                                            " +
            "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      " +
            "\n  END                                                                                                                                                                                                                                                                " +
            "\n  AS META                                                                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n                                                                                                                                                                                                                                                                     " +
           "\n FROM ParLevel1(nolock) L1    -- (SELECT* FROM ParLevel1(nolock) WHERE ISNULL(ShowScorecard, 1) = 1) L1                                                                                                                                                                                                                                           " +
           "\n LEFT JOIN #ConsolidationLevel CL1   (nolock)                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1.Id = CL1.ParLevel1_Id AND ISNULL(ShowScorecard, 1) = 1                                                                                                                                                                                                                                " +
            "\n LEFT JOIN ParScoreType ST  (nolock)                                                                                                                                                                                                                                           " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                " +
            "\n LEFT JOIN ParCompany C    (nolock)                                                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON C.Id = CL1.UnitId                                                                                                                                                                                                                                         " +
            "\n LEFT JOIN #AMOSTRATIPO4 A4    (nolock)                                                                                                                                                                                                                                        " +
            "\n         ON A4.UNIDADE = C.Id                                                                                                                                                                                                                                      " +
            "\n         AND A4.INDICADOR = L1.ID                                                                                                                                 " +
            "\n LEFT JOIN ParCompanyXStructure CS   (nolock)                                                                                                                                                                                                                                  " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   " +
            "\n LEFT JOIN ParStructure S     (nolock)                                                                                                                                                                                                                                         " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 " +
            "\n LEFT JOIN ParStructureGroup SG     (nolock)                                                                                                                                                                                                                                   " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            " +
            "\n LEFT JOIN ParCompanyCluster CCL   (nolock)                                                                                                                                                                                                                                    " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                 " +
            "\n LEFT JOIN ParCluster CL       (nolock)                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 " +
            "\n LEFT JOIN ParConsolidationType CT  (nolock)                                                                                                                                                                                                                                   " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                        " +
            "\n LEFT JOIN ParLevel1XCluster L1C  (nolock)                                                                                                                                                                                                                                     " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1                                                                                                                                                                                                  " +
            "\n LEFT JOIN ParCriticalLevel CRL   (nolock)                                                                                                                                                                                                                                     " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n        ON CRL.Id  = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL)                                                                              " +
 "\n  --------------------------                                                                                                                                                                                                                                                    " +
 "\n  --------------------------                                                                                                                                                                                                                                                    " +
 "\n                                                                                                                                                                                                                                                                                " +
 //"\n  LEFT JOIN                                                                                                                                                                                                                                                                     " +
 //"\n (                                                                                                                                                                                                                                                                              " +
 //"\n SELECT 25 AS INDICADOR, CASE WHEN DATAP IS NULL THEN DATAV ELSE DATAP END AS DATA, *,                                                                                                                                                                                                                                                     " +
 //"\n CASE WHEN ISNULL(V.DIASDEVERIFICACAO, 0) > ISNULL(P.DIASABATE, 0) THEN ISNULL(P.DIASABATE, 0) ELSE ISNULL(V.DIASDEVERIFICACAO, 0) END AS FREQ                                                                                                                                  " +
 //"\n FROM                                                                                                                                                                                                                                                                           " +
 //"\n (                                                                                                                                                                                                                                                                              " +
 //"\n SELECT Data AS DATAP, COUNT(1) DIASABATE, SUM(Quartos) VOLUMEPCC, ParCompany_id                                                                                                                                                                                                " +
 //"\n FROM VolumePcc1b(nolock)                                                                                                                                                                                                                                                       " +
 //"\n WHERE Data BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                                 " +
 //"\n GROUP BY ParCompany_id, Data                                                                                                                                                                                                                                                   " +
 //"\n ) P                                                                                                                                                                                                                                                                            " +
 //"\n FULL JOIN                                                                                                                                                                                                                                                                      " +
 //"\n (                                                                                                                                                                                                                                                                              " +
 //"\n SELECT COUNT(1) AS DIASDEVERIFICACAO, UNITID, DATA AS DATAV                                                                                                                                                                                                                    " +
 //"\n FROM(SELECT CONVERT(DATE, ConsolidationDate) DATA, cl1.UNITID FROM ConsolidationLevel1 CL1(nolock)                                                                                                                                                                             " +
 //"\n WHERE ParLevel1_Id = 24                                                                                                                                                                                                                                                        " +
 //"\n AND ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                      " +
 //"\n GROUP BY CONVERT(DATE, ConsolidationDate), UNITID) VT                                                                                                                                                                                                                          " +
 //"\n GROUP BY DATA, UNITID                                                                                                                                                                                                                                                          " +
 //"\n ) V                                                                                                                                                                                                                                                                            " +
 //"\n ON V.DATAV = P.DataP                                                                                                                                                                                                                                                           " +
 //"\n AND V.UnitId = P.ParCompany_id                                                                                                                                                                                                                                                 " +
 //"\n                                                                                                                                                                                                                                                                                " +
 //"\n ) FT                                                                                                                                                                                                                                                                           " +
 //"\n ON L1.Id = FT.INDICADOR                                                                                                                                                                                                                                                        " +
 "\n                                                                                                                                                                                                                                                                                " +
 "\n  --------------------------                                                                                                                                                                                                                                                    " +
 "\n  --------------------------                                                                                                                                                                                                                                                    " +
            "\n WHERE(ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL OR L1.Id = 25)                                                                                                                                                                                          " +
            "\n   AND(C.Id >= 1  OR(C.Id IS NULL AND L1.Id = 25))                                                                                                                                                                                                       " +
            "\n GROUP BY                                                                                                                                                                                                                                                            " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n      CL.Id                                                                                                                                                                                                                                                          " +
            "\n     , CL.Name                                                                                                                                                                                                                                                       " +
            "\n     , S.Id                                                                                                                                                                                                                                                          " +
            "\n     , S.Name                                                                                                                                                                                                                                                        " +
            "\n     , CL1.UnitId                                                                                                                                                                                                                                                    " +
            "\n     , C.Name                                                                                                                                                                                                                                                        " +
            "\n     , L1.IsRuleConformity                                                                                                                                                                                                                                           " +
            "\n     , L1.Id                                                                                                                                                                                                                                                         " +
            "\n     , L1.Name                                                                                                                                                                                                                                                       " +
            "\n     , CRL.Id                                                                                                                                                                                                                                                        " +
            "\n     , CRL.Name                                                                                                                                                                                                                                                      " +
            //"\n     , L1C.Points                                                                                                                                                                                                                                                    " +
            "\n     , ST.Name                                                                                                                                                                                                                                                       " +
            "\n     , CT.Id                                                                                                                                                                                                                                                         " +
            "\n     , L1.HashKey " +
            "\n     , CCL.ParCluster_ID                                                                                                                                                                                                                                                   " +
            //"\n     , C.Id   , CL1.ConsolidationDate,FT.DATA, FT.PARCOMPANY_ID                                                                                                                                                                                                                                                        " +
            "\n     , C.Id   , CL1.ConsolidationDate                                                                                                                                                                                                                                                        " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n ) SCORECARD                                                                                                                                                                                                                                                         " +
            "\n                                                                                                                                                                                                                                                                     " +
            "\n ) FIM                                                                                                                                                                                                                                                               " +
            "\n                                                                                                                                                                                                                                                                     " +
 "\n    ) SC                                                                                                                                                                                                                                                               					                                               " +
 "\n WHERE ParCompanyId <> 2 " +
 "\n GROUP BY                 " +
 "\n   Cluster                " +
 "\n   , ClusterName          " +
 "\n   , Regional             " +
 "\n   , RegionalName         " +
 "\n   , ParCompanyId         " +
 "\n   , ParCompanyName       " +
 "\n   , TipoIndicador        " +
 "\n   , TipoIndicadorName    " +
 "\n   , Level1Id             " +
 "\n   , Level1Name           " +
 "\n   , Criterio             " +
 "\n   , CriterioName         " +
 "\n   , TipoScore            " +

 "\n    ORDER BY 11, 10                                                                                                                                                                                                                                                    					                                               " +
 "\n    DROP TABLE #AMOSTRATIPO4                                                                                                                                                                                                                                                                                                            " +
 "\n    DROP TABLE #VOLUMES	                                                                                                                                                                                                                                                                                                               " +
 "\n    DROP TABLE #DIASVERIFICACAO                                                                                                                                                                                                                                                                                                         " +
 "\n    DROP TABLE #NAPCC	DROP TABLE #FREQ																																																														                                                       " +
 "\n  																																																																						                                               " +
 "\n    SET @I = @I + 1																																																																		                                               " +
 "\n  																																																																						                                               " +
 "\n  END                                                                                                                                                                                                                                                                                                                                   " +
 "\n  																																																																						                                               " +
 "\n     ";

            return query;

        }


        // sqlBase1 foi criado especialmente para o Grafico 1, para seguir os mesmos valores que os resultados da matriz por regional, conforme combinado com o Gabriel [BS]
        public static string sqlBase1(DataCarrierFormulario form, bool evolutivo = false)
        {
            DateTime dtIni = form._dataInicio;
            if (evolutivo)
            {
                var year = dtIni.Year - 1;
                dtIni = new DateTime(year, dtIni.Month, form._dataFim.Day);
            }


            /*
             * neste score devo considerar a regra dos 70 %
             * 
             */
            var query = "" +

  // "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'    " +
  // "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n  IF OBJECT_ID('tempdb.dbo.#SCORE', 'U') IS NOT NULL																																																										                                               " +
  "\n    DROP TABLE #SCORE; 																																																																	                                               " +
  "\n  																																																																						                                               " +
  "\n  CREATE TABLE #SCORE (																																																																	                                               " +
  "\n  	Cluster int null,																																																																	                                               " +
  "\n  	ClusterName Varchar(153) null,																																																														                                               " +
  "\n  	Regional int null,																																																																	                                               " +
  "\n  	RegionalName Varchar(153) null, 																																																													                                               " +
  "\n  	ParCompany_Id int null,																																																																                                               " +
  "\n  	ParCompanyName Varchar(153) null, 																																																													                                               " +
  "\n  	TipoIndicador int null,																																																																                                               " +
  "\n  	TipoIndicadorName Varchar(153) null, 																																																												                                               " +
  "\n  	Level1Id int null,																																																																	                                               " +
  "\n  	Level1Name Varchar(153) null, 																																																														                                               " +
  "\n  	Criterio int null,																																																																	                                               " +
  "\n  	CriterioName Varchar(153) null, 																																																													                                               " +
  "\n  	Av decimal(30,5) null,																																																																                                               " +
  "\n  	Nc decimal(30,5) null,																																																																                                               " +
  "\n  	Pontos decimal(30,5) null,																																																															                                               " +
  "\n  	PontosIndicador decimal(30,5) null,																																																													                                               " +
  "\n  	Meta decimal(30,5) null,																																																															                                               " +
  "\n  	Real decimal(30,5) null,																																																															                                               " +
  "\n  	PontosAtingidos decimal(30,5) null,																																																													                                               " +
  "\n  	Scorecard decimal(30,5) null,																																																														                                               " +
  "\n  	TipoScore Varchar(153) null," +
  "\n mesData datetime2(7) null  " +
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


@"            SELECT 

                CL1.id,
            	CL1.ConsolidationDate,
            	CL1.UnitId,
            	CL1.ParLevel1_Id,
            	CL1.DefectsResult,
            	CL1.WeiDefects,
            	CL1.EvaluatedResult,
            	CL1.WeiEvaluation,
            	CL1.EvaluateTotal,
            	CL1.TotalLevel3WithDefects,
            	CL1.DefectsTotal
            INTO #ConsolidationLevel
            FROM ConsolidationLevel1 CL1 WITH(NOLOCK)
            WHERE 1 = 1
            AND CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL

            CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_level1 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId); 
            CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id); " +


                 // Alteração
                 "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                  "\n UNIDADE INT NULL, " +
                  "\n INDICADOR INT NULL, " +
                  "\n AM INT NULL, " +
                  "\n DEF_AM INT NULL " +
                  "\n ) " +


                  "\n INSERT INTO #AMOSTRATIPO4 " +
                  /*
                  "\n SELECT " +
                  "\n  UNIDADE, INDICADOR, " +
                  "\n FROM " +
                  "\n ( " +
                  */
                  "\n     SELECT " +
                  //"\n     cast(C2.CollectionDate as DATE) AS DATA " +
                  "\n     C.Id AS UNIDADE " +
                  "\n     , C2.ParLevel1_Id AS INDICADOR " +
                  "\n , COUNT(DISTINCT CONCAT(C2.EvaluationNumber, C2.Sample, cast(cast(C2.CollectionDate as date) as varchar))) AM " +
                  "\n , SUM(IIF(C2.WeiDefects = 0, 0, 1)) DEF_AM " +
                  //"\n     , C2.EvaluationNumber AS AV " +
                  // "\n     , C2.Sample AS AM " +
                  //"\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                  "\n     FROM CollectionLevel2 C2 (nolock) " +
                  "\n     INNER JOIN ParLevel1 L1 (nolock)  " +
                  "\n     ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1" +

                  "\n     INNER JOIN ParCompany C (nolock)  " +
                  "\n     ON C.Id = C2.UnitId " +
                  "\n     where C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                  "\n     and C2.NotEvaluatedIs = 0 " +
                  "\n     and C2.Duplicated = 0 " +
                  "\n     and L1.ParConsolidationType_Id = 4 " +
                  "\n     group by C.Id, ParLevel1_Id" +
             /*
             "\n ) TAB " +
             "\n GROUP BY UNIDADE, INDICADOR " +
             */



             "\n                                                                                                                                                                                                                                                                     " +
             "\n DECLARE @VOLUMEPCC INT                                                                                                                                                                                                                                              " +
             "\n DECLARE @DIASABATE INT                                                                                                                                                                                                                                              " +
             "\n DECLARE @DIASDEVERIFICACAO INT                                                                                                                                                                                                                                      " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n DECLARE @AVFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                " +
             "\n DECLARE @NCFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                " +
             "\n                                                                                                                                                                                                                                                                     " +

  "\n   /* INICIO DADOS DA FREQUENCIA ------------------------------------------------------*/                                                                                                                                                                              					                                               " +
  "\n                                                                                                                                                                                                                                                                       					                                               " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n CREATE TABLE #FREQ (                                                                                                                                                                                                                                                                                                                   " +
  "\n    clusterId int null                                                                                                                                                                                                                                                                                                                  " +
  "\n    ,     cluster varchar(255) null                                                                                                                                                                                                                                                                                                     " +
  "\n    , regionalId int null                                                                                                                                                                                                                                                                                                               " +
  "\n    , regional varchar(255) null                                                                                                                                                                                                                                                                                                        " +
  "\n    , unitId int null                                                                                                                                                                                                                                                                                                                   " +
  "\n    , unidade  varchar(255) null                                                                                                                                                                                                                                                                                                        " +
  "\n    , criticalLevelId  int null                                                                                                                                                                                                                                                                                                         " +
  "\n    , criticalLevel varchar(255) null                                                                                                                                                                                                                                                                                                   " +
  "\n    , pontos decimal(35, 10) null                                                                                                                                                                                                                                                                                                       " +
  "\n    )                                                                                                                                                                                                                                                                                                                                   " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                 					                                                                                   " +
  "\n                                                                                                                                                                                                                                                                        					                                               " +
  "\n    insert into #freq                                                                                                                                                                                                                                                                                                                   " +
  "\n    SELECT                                                                                                                                                                                                                                                                                                                              " +
  "\n    CL.Id                                                                                                                                                                                                                                                                                                                               " +
  "\n    , CL.Name                                                                                                                                                                                                                                                                                                                           " +
  "\n    , S.Id                                                                                                                                                                                                                                                                                                                              " +
  "\n    , S.Name                                                                                                                                                                                                                                                                                                                            " +
  "\n    , C.Id                                                                                                                                                                                                                                                                                                                              " +
  "\n    , C.Name                                                                                                                                                                                                                                                                                                                            " +
  "\n    , L1C.ParCriticalLevel_Id                                                                                                                                                                                                                                                                                                           " +
  "\n    , CRL.Name                                                                                                                                                                                                                                                                                                                          " +
  "\n    , L1C.Points                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n    FROM ParCompany C                                                                                                                                                                                                                                                                                                                   " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n    LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                                                                                   " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n           ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                                                                                   " +
  "\n    LEFT JOIN ParStructure S                                                                                                                                                                                                                                                                                                            " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n           ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                                                                                 " +
  "\n    LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                                                                                      " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n           ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                                                                                            " +
  "\n    LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                                                                                     " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n           ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                                                              " +
  "\n    LEFT JOIN ParCluster CL                                                                                                                                                                                                                                                                                                             " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n           ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                                                                                 " +
  "\n    LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                                                                                     " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n           ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1                                                                                                                                                                                                                                                " +
  "\n    LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                                                                                      " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n           ON L1C.ParCriticalLevel_Id = CRL.Id                                                                                                                                                                                                                                                                                          " +
  "\n    WHERE C.Id >= 1                                                                                                                                                                                                                                                                                                                     " +
  "\n    AND L1C.ParLevel1_Id = 25                                                                                                                                                                                                                                                                                                           " +
  "\n                                                                                                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                       					                                               " +
  "\n   /* FIM DOS DADOS DA FREQUENCIA -----------------------------------------------------*/                                                                                                                                                                              					                                               " +
  "\n   CREATE TABLE #VOLUMES (                                                                                                                                                                                                                                                                                                              " +
  "\n 	DIASABATE INT NULL,                                                                                                                                                                                                                                                                                                                " +
  "\n 	VOLUMEPCC INT NULL, unitid int null                                                                                                                                                                                                                                                                                                                 " +
  "\n   )                                                                                                                                                                                                                                                                                                                                    " +
  "\n   INSERT INTO #VOLUMES                                                                                                                                                                                                                                                                    					                           " +
  "\n   SELECT COUNT(1) AS DIASABATE, SUM(Quartos) AS VOLUMEPCC, ParCompany_id as UnitId FROM VolumePcc1b WHERE Data BETWEEN @DATAINICIAL AND @DATAFINAL GROUP BY ParCompany_id                                                                                                  					                                                                   " +
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
  "\n   GROUP BY UnitId    " +
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
  "\n                AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)                                                                                                                                                                             						                                           " +
  "\n                --AND C2.UnitId = @ParCompany_Id                                                                                                                                                                                                                       						                                           " +
  "\n                AND IsNotEvaluate = 1                                                                                                                                                                                                                                						                                           " +
  "\n                GROUP BY C2.ID, C2.UnitId                                                                                                                                                                                                                                       						                                   " +
  "\n            ) NA		                                                                                                                                                                                                                                                        						                                   " +
  "\n            WHERE NA = 2                                                                                                                                                                                                                                             						                                           " +
  "\n 		   GROUP BY UnitId                                                                                                                                                                                                                                                                                                                                                                                                                                " +
  "\n   																																																																						                                               " +
  "\n   INSERT INTO #SCORE																																						" +
  "\n SELECT                                                                                                                                                                                                                                                                                                                                                        " +
  "\n   Cluster                                                                                                                                                                                                                                                                                                                                                     " +
  "\n  , ClusterName                                                                                                                                                                                                                                                                                                                                                " +
  "\n  , Regional                                                                                                                                                                                                                                                                                                                                                   " +
  "\n  , RegionalName                                                                                                                                                                                                                                                                                                                                               " +
  "\n  , ParCompanyId                                                                                                                                                                                                                                                                                                                                              " +
  "\n  , ParCompanyName                                                                                                                                                                                                                                                                                                                                             " +
  "\n  , TipoIndicador                                                                                                                                                                                                                                                                                                                                              " +
  "\n  , TipoIndicadorName                                                                                                                                                                                                                                                                                                                                          " +
  "\n  , Level1Id                                                                                                                                                                                                                                                                                                                                                   " +
  "\n  , Level1Name                                                                                                                                                                                                                                                                                                                                                 " +
  "\n  , Criterio                                                                                                                                                                                                                                                                                                                                                   " +
  "\n  , CriterioName                                                                                                                                                                                                                                                                                                                                               " +
  "\n  , case when Level1Id = 3 then avg(av) else sum(av) end av                                                                                                                                                                                                                                                                                                                                                 " +
  "\n  , sum(nc) nc                                                                                                                                                                                                                                                                                                                                                 " +
  "\n  , (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) pontos                                                                                                                                                                                                                                                                                                                                         " +
  "\n  , (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) pontosIndicador                                                                                                                                                                                                                                                                                                                                " +
  "\n  , max(meta) meta                                                                                                                                                                                                                                                                                                                                             " +

  "\n  , round(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) * 100) end end, 2) real " +
  "\n  , case when(isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) > 100 then 100 else (isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) end / 100 * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) as pontosAtingidos " +
  "\n  , case when(isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) > 100 then 100 else (isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) end as Scorecard " +

  "\n  , TipoScore " +
  "\n  , max(mesData) mesData FROM                                                                                                                                                                                                                                           " +
             "\n (                                                                                                                                                                                                                                                                   " +
             "\n SELECT                                                                                                                                                                                                                                                              " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n   Cluster                                                                                                                                                                                                                                                           " +
             "\n  , ClusterName                                                                                                                                                                                                                                                      " +
             "\n  , Regional                                                                                                                                                                                                                                                         " +
             "\n  , RegionalName                                                                                                                                                                                                                                                     " +
             "\n  , ParCompanyId                                                                                                                                                                                                                                                     " +
             "\n  , ParCompanyName                                                                                                                                                                                                                                                   " +
             "\n  , CASE WHEN TipoIndicador = 0 THEN 1 ELSE 2 END TipoIndicador                                                                                                                                                                                                      " +
             "\n  , CASE WHEN TipoIndicador = 0 THEN 'Menor' ELSE 'Maior' END TipoIndicadorName                                                                                                                                                                                      " +
             "\n  , Level1Id                                                                                                                                                                                                                                                         " +
             "\n  , Level1Name                                                                                                                                                                                                                                                       " +
             "\n  , Criterio                                                                                                                                                                                                                                                         " +
             "\n  , CriterioName                                                                                                                                                                                                                                                     " +
             "\n  , AV AV                                                                                                                                                                                                                                                               " +
             "\n  , CASE WHEN Level1Id = 25 THEN AV - NC ELSE NC END NC /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                                                                             " +
             "\n  , Pontos Pontos                                                                                                                                                                                                                                                           " +
             "\n  , CASE WHEN AV = 0 THEN 0 ELSE Pontos END AS PontosIndicador                                                                                                                                                                                                                                        " +
             "\n  , Meta AS Meta                                                                                                                                                                                                                                                             " +
             "\n  , CASE WHEN Level1Id = 25 THEN CASE WHEN AV = 0 THEN 0 ELSE (AV - NC) / AV * 100 END WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 else Meta / NC end * 100 ELSE Real END Real /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                            " +
             "\n  , CASE WHEN Level1Id = 43 AND AV > 0 AND  NC = 0 THEN Pontos ELSE PontosAtingidos END  PontosAtingidos                                                                                                                                                                                                                                               " +
             "\n  , CASE WHEN Level1Id = 43 AND AV > 0 AND NC = 0 THEN 100 ELSE Scorecard END  Scorecard                                                                                                                                                                                                                                                        " +
             "\n  , TipoScore ,mesData                                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n FROM                                                                                                                                                                                                                                                                " +
             "\n (                                                                                                                                                                                                                                                                   " +
             "\n SELECT                                                                                                                                                                                                                                                              " +
             "\n *,                                                                                                                                                                                                                                                                  " +
             "\n /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 " +
             "\n CASE                                                                                                                                                                                                                                                                " +
             "\n     WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 else Meta / NC end                                                                                                                                                                                                                                                                " +
             "\n     WHEN                                                                                                                                                                                                                                                            " +
             "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
             "\n     CASE                                                                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         ELSE                                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         END                                                                                                                                                                                                                                                         " +
             "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
             "\n     > 1                                                                                                                                                                                                                                                             " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     THEN 1                                                                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     ELSE                                                                                                                                                                                                                                                            " +
             "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
             "\n     CASE                                                                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         ELSE                                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         END                                                                                                                                                                                                                                                         " +
             "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
             "\n END * 100                                                                                                                                                                                                                                                           " +
             "\n /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 " +
             "\n AS SCORECARD                                                                                                                                                                                                                                                        " +
             "\n ,                                                                                                                                                                                                                                                                   " +
             "\n /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 " +
             "\n CASE                                                                                                                                                                                                                                                                " +
             "\n     WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 when (Meta / NC) < 0.00 then 0 else Meta / NC end                                                                                                                                                                                                                                                                 " +
             "\n     WHEN                                                                                                                                                                                                                                                            " +
             "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
             "\n     CASE                                                                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         ELSE                                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         END                                                                                                                                                                                                                                                         " +
             "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
             "\n     > 1                                                                                                                                                                                                                                                             " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     THEN 1                                                                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     WHEN                                                                                                                                                                                                                                                            " +
             "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
             "\n     CASE                                                                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         ELSE                                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         END                                                                                                                                                                                                                                                         " +
             "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
             "\n     < 0.00                                                                                                                                                                                                                                                           " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     THEN 0                                                                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     ELSE                                                                                                                                                                                                                                                            " +
             "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
             "\n     CASE                                                                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         ELSE                                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         END                                                                                                                                                                                                                                                         " +
             "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
             "\n END                                                                                                                                                                                                                                                                 " +
             "\n /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 " +
             "\n * /* MULTIPLICAÇÃO */                                                                                                                                                                                                                                               " +
             "\n PONTOS                                                                                                                                                                                                                                                              " +
             "\n AS PONTOSATINGIDOS                                                                                                                                                                                                                                                  " +
             "\n FROM                                                                                                                                                                                                                                                                " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n (                                                                                                                                                                                                                                                                   " +
             "\n SELECT                                                                                                                                                                                                                                                              " +
             "\n                                                                                                                                                                                                                                                                     " +
  //"\n           ISNULL(CL.Id, (SELECT top 1 clusterId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Cluster                                                                                                                                                                      " +
  //"\n , ISNULL(CL.Name, (SELECT top 1 cluster FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS ClusterName                                                                                                                                                                          " +
  //"\n , ISNULL(S.Id, (SELECT top 1 regionalId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Regional                                                                                                                                                                             " +
  //"\n , ISNULL(S.Name, (SELECT top 1 regional FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS RegionalName                                                                                                                                                                         " +
  //"\n , ISNULL(CL1.UnitId, ft.ParCompany_id) AS ParCompanyId                                                                                                                                                                                                                       " +
  //"\n , ISNULL(C.Name, (SELECT top 1 unidade FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS ParCompanyName                                                                                                                                                                        " +
  //"\n , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                                       " +
  //"\n , L1.Id AS Level1Id                                                                                                                                                                                                                                                          " +
  //"\n , L1.Name AS Level1Name                                                                                                                                                                                                                                                      " +
  //"\n , ISNULL(CRL.Id, (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Criterio                                                                                                                                                                      " +
  //"\n , ISNULL(CRL.Name, (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS CriterioName                                                                                                                                                                  " +
  //"\n , ISNULL((select top 1 Points from ParLevel1XCluster aaa (nolock) where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate < @DATAFINAL), (SELECT top 1 pontos FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Pontos                                    " +
  //"\n   , ISNULL(CL1.ConsolidationDate, FT.Data) as mesData                                                                                                                                                                                                                       " +


  "\n           ISNULL(CL.Id, (SELECT top 1 clusterId FROM #FREQ WHERE unitId = 0)) AS Cluster                                                                                                                                                                      " +
    "\n , ISNULL(CL.Name, (SELECT top 1 cluster FROM #FREQ WHERE unitId = 0)) AS ClusterName                                                                                                                                                                          " +
    "\n , ISNULL(S.Id, (SELECT top 1 regionalId FROM #FREQ WHERE unitId = 0)) AS Regional                                                                                                                                                                             " +
    "\n , ISNULL(S.Name, (SELECT top 1 regional FROM #FREQ WHERE unitId = 0)) AS RegionalName                                                                                                                                                                         " +
    "\n , ISNULL(CL1.UnitId, 0) AS ParCompanyId                                                                                                                                                                                                                       " +
    "\n , ISNULL(C.Name, (SELECT top 1 unidade FROM #FREQ WHERE unitId = 0)) AS ParCompanyName                                                                                                                                                                        " +
    "\n , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                                       " +
    "\n , L1.Id AS Level1Id                                                                                                                                                                                                                                                          " +
    "\n , L1.Name AS Level1Name                                                                                                                                                                                                                                                      " +
    "\n , ISNULL(" +
    "( " +

     "\n         SELECT TOP 1 L1Ca.ParCriticalLevel_Id FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

     "\n         WHERE CCL.ParCluster_ID = L1Ca.ParCluster_ID " +

     "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

     "\n           --  AND L1Ca.IsActive = 1 " +

     "\n             AND L1Ca.ValidoApartirDe <= @DATAFINAL " +

     "\n         ORDER BY L1Ca.ValidoApartirDe  desc " +
      "\n	)" +
    "" +
    "\n , (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = 0)) AS Criterio                                                                                                                                                                      " +
    "\n , ISNULL(" +
    "( " +

     "\n         SELECT TOP 1 (select top 1 name from ParCriticalLevel where id = L1Ca.ParCriticalLevel_Id) FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

     "\n         WHERE CCL.ParCluster_ID = L1Ca.ParCluster_ID " +

     "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

     "\n           --  AND L1Ca.IsActive = 1 " +

     "\n             AND L1Ca.ValidoApartirDe <= @DATAFINAL " +

     "\n         ORDER BY L1Ca.ValidoApartirDe  desc " +
      "\n	)" +
    ", (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = 0)) AS CriterioName                                                                                                                                                                  " +
    "\n , ISNULL(" +
    "( " +

     "\n         SELECT TOP 1 L1Ca.Points FROM ParLevel1XCluster L1Ca WITH(NOLOCK) " +

     "\n         WHERE CCL.ParCluster_ID = L1Ca.ParCluster_ID " +

     "\n             AND L1.Id = L1Ca.ParLevel1_Id " +

     "\n           --  AND L1Ca.IsActive = 1 " +

     "\n             AND L1Ca.ValidoApartirDe <= @DATAFINAL " +

     "\n         ORDER BY L1Ca.ValidoApartirDe desc  " +
      "\n	)" +
    ", (SELECT top 1 pontos FROM #FREQ WHERE unitId = 0)) AS Pontos                                    " +
    "\n   , ISNULL(CL1.ConsolidationDate, '0001-01-01') as mesData                                                                                                                                                                                                                       " +


  "\n                                                                                                                                                                                                                                                                     " +
             "\n  --ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                               " +
             "\n  --, (CL.Name)AS ClusterName                                                                                                                                                                                                                                        " +
             "\n  --, (S.Id)AS Regional                                                                                                                                                                                                                                              " +
             "\n  --, (S.Name)AS RegionalName                                                                                                                                                                                                                                        " +
             "\n  --, (CL1.UnitId)AS ParCompanyId                                                                                                                                                                                                                                    " +
             "\n  --, (C.Name)AS ParCompanyName                                                                                                                                                                                                                                      " +
             "\n  --, L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                           " +
             "\n  --, L1.Id AS Level1Id                                                                                                                                                                                                                                              " +
             "\n  --, L1.Name AS Level1Name                                                                                                                                                                                                                                          " +
             "\n  --, (CRL.Id)AS Criterio                                                                                                                                                                                                                                            " +
             "\n  --, (CRL.Name)AS CriterioName                                                                                                                                                                                                                                      " +
             "\n  --, (L1C.Points)AS Pontos                                                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n  , ST.Name AS TipoScore                                                                                                                                                                                                                                             " +
             "\n  ,                                                                                                                                                                                                                                                                  " +
             "\n   /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                              " +


             "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              ////"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
              "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
              "\n     END                                                                                                                                                                                                                                                               					                                               " +



             "\n   /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                              " +
             "\n   AS AV                                                                                                                                                                                                                                                             " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n  ,                                                                                                                                                                                                                                                                  " +
             "\n   /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                              " +
             "\n   CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                            " +
             "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
            "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              ////"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
              "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
              "\n     END                                                                                                                                                                                                                                                               					                                               " +


             "\n         /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                        " +
             "\n         -                                                                                                                                                                                                                                                           " +
             "\n       /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          " +
             "\n       CASE                                                                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             //"\n         WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                        " +

             "\n       END                                                                                                                                                                                                                                                           " +
             "\n       /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          " +
             "\n    ELSE                                                                                                                                                                                                                                                             " +
             "\n       /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          " +
             "\n       CASE                                                                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             //"\n         WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n         WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                        " +

             "\n       END                                                                                                                                                                                                                                                           " +
             "\n       /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          " +
             "\n      end                                                                                                                                                                                                                                                           " +
             "\n    /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                             " +
             "\n    AS NC                                                                                                                                                                                                                                                            " +
             "\n  ,                                                                                                                                                                                                                                                                  " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n   CASE                                                                                                                                                                                                                                                              " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     WHEN                                                                                                                                                                                                                                                            " +
             "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
             "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
              "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
              "\n     END                                                                                                                                                                                                                                                               					                                               " +

             "\n                                                                                                                                                                                                                                                                 " +
             "\n       /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          " +
             "\n       = 0                                                                                                                                                                                                                                                           " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     THEN 0                                                                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n      ELSE                                                                                                                                                                                                                                                           " +
             "\n       /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                          " +
             "\n       CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                        " +
             "\n           /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                      " +
             "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
              "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
              "\n     END                                                                                                                                                                                                                                                               					                                               " +
             "\n             /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                    " +
             "\n             -  /* SUBTRAÇÃO */                                                                                                                                                                                                                                      " +
             "\n                /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                 " +
             "\n           CASE                                                                                                                                                                                                                                                      " +
             "\n                                                                                                                                                                                                                                                                     " +
             //"\n             WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                            " +
             "\n           END                                                                                                                                                                                                                                                       " +
             "\n           /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      " +
             "\n        ELSE                                                                                                                                                                                                                                                         " +
             "\n           /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                      " +
             "\n           CASE                                                                                                                                                                                                                                                      " +
             "\n                                                                                                                                                                                                                                                                     " +
             //"\n             WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n             WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                            " +

             "\n           END                                                                                                                                                                                                                                                       " +
             "\n           /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      " +
             "\n        END                                                                                                                                                                                                                                                         " +
             "\n        /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                         " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        / /*DIVISÃO*/                                                                                                                                                                                                                                                " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
             "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
              "\n                                                                                                                                                                                                                                                                       					                                               " +
              "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
              "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
              "\n     END                                                                                                                                                                                                                                                               					                                               " +
             "\n       /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n      END * 100                                                                                                                                                                                                                                                      " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n      AS REAL                                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n  ,                                                                                                                                                                                                                                                                  " +
             "\n  CASE                                                                                                                                                                                                                                                               " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
             "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n     ELSE                                                                                                                                                                                                                                                            " +
             "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      " +
             "\n  END                                                                                                                                                                                                                                                                " +
             "\n  AS META                                                                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n FROM ParLevel1(nolock) L1    -- (SELECT* FROM ParLevel1(nolock) WHERE ISNULL(ShowScorecard, 1) = 1) L1                                                                                                                                                                                                                                           " +
             "\n LEFT JOIN #ConsolidationLevel CL1   (nolock)                                                                                                                                                                                                                                  " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON L1.Id = CL1.ParLevel1_Id AND ISNULL(ShowScorecard, 1) = 1                                                                                                                                                                                                                                " +
             "\n LEFT JOIN ParScoreType ST  (nolock)                                                                                                                                                                                                                                           " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                " +
             "\n LEFT JOIN ParCompany C    (nolock)                                                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON C.Id = CL1.UnitId                                                                                                                                                                                                                                         " +
             "\n LEFT JOIN #AMOSTRATIPO4 A4    (nolock)                                                                                                                                                                                                                                        " +
             "\n         ON A4.UNIDADE = C.Id                                                                                                                                                                                                                                      " +
             "\n         AND A4.INDICADOR = L1.ID                                                                                                                                 " +
             "\n LEFT JOIN ParCompanyXStructure CS   (nolock)                                                                                                                                                                                                                                  " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   " +
             "\n LEFT JOIN ParStructure S     (nolock)                                                                                                                                                                                                                                         " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 " +
             "\n LEFT JOIN ParStructureGroup SG     (nolock)                                                                                                                                                                                                                                   " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            " +
             "\n LEFT JOIN ParCompanyCluster CCL   (nolock)                                                                                                                                                                                                                                    " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                 " +
             "\n LEFT JOIN ParCluster CL       (nolock)                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 " +
             "\n LEFT JOIN ParConsolidationType CT  (nolock)                                                                                                                                                                                                                                   " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                        " +
             "\n LEFT JOIN ParLevel1XCluster L1C  (nolock)                                                                                                                                                                                                                                     " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1                                                                                                                                                                                                  " +
             "\n LEFT JOIN ParCriticalLevel CRL   (nolock)                                                                                                                                                                                                                                     " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n        ON CRL.Id  = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL)                                                                              " +
  "\n  --------------------------                                                                                                                                                                                                                                                    " +
  "\n  --------------------------                                                                                                                                                                                                                                                    " +
  "\n                                                                                                                                                                                                                                                                                " +
  //"\n  LEFT JOIN                                                                                                                                                                                                                                                                     " +
  //"\n (                                                                                                                                                                                                                                                                              " +
  //"\n SELECT 25 AS INDICADOR, CASE WHEN DATAP IS NULL THEN DATAV ELSE DATAP END AS DATA, *,                                                                                                                                                                                                                                                     " +
  //"\n CASE WHEN ISNULL(V.DIASDEVERIFICACAO, 0) > ISNULL(P.DIASABATE, 0) THEN ISNULL(P.DIASABATE, 0) ELSE ISNULL(V.DIASDEVERIFICACAO, 0) END AS FREQ                                                                                                                                  " +
  //"\n FROM                                                                                                                                                                                                                                                                           " +
  //"\n (                                                                                                                                                                                                                                                                              " +
  //"\n SELECT Data AS DATAP, COUNT(1) DIASABATE, SUM(Quartos) VOLUMEPCC, ParCompany_id                                                                                                                                                                                                " +
  //"\n FROM VolumePcc1b(nolock)                                                                                                                                                                                                                                                       " +
  //"\n WHERE Data BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                                 " +
  //"\n GROUP BY ParCompany_id, Data                                                                                                                                                                                                                                                   " +
  //"\n ) P                                                                                                                                                                                                                                                                            " +
  //"\n FULL JOIN                                                                                                                                                                                                                                                                      " +
  //"\n (                                                                                                                                                                                                                                                                              " +
  //"\n SELECT COUNT(1) AS DIASDEVERIFICACAO, UNITID, DATA AS DATAV                                                                                                                                                                                                                    " +
  //"\n FROM(SELECT CONVERT(DATE, ConsolidationDate) DATA, cl1.UNITID FROM ConsolidationLevel1 CL1(nolock)                                                                                                                                                                             " +
  //"\n WHERE ParLevel1_Id = 24                                                                                                                                                                                                                                                        " +
  //"\n AND ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                      " +
  //"\n GROUP BY CONVERT(DATE, ConsolidationDate), UNITID) VT                                                                                                                                                                                                                          " +
  //"\n GROUP BY DATA, UNITID                                                                                                                                                                                                                                                          " +
  //"\n ) V                                                                                                                                                                                                                                                                            " +
  //"\n ON V.DATAV = P.DataP                                                                                                                                                                                                                                                           " +
  //"\n AND V.UnitId = P.ParCompany_id                                                                                                                                                                                                                                                 " +
  //"\n                                                                                                                                                                                                                                                                                " +
  //"\n ) FT                                                                                                                                                                                                                                                                           " +
  //"\n ON L1.Id = FT.INDICADOR                                                                                                                                                                                                                                                        " +
  "\n                                                                                                                                                                                                                                                                                " +
  "\n  --------------------------                                                                                                                                                                                                                                                    " +
  "\n  --------------------------                                                                                                                                                                                                                                                    " +
             "\n WHERE(ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL OR L1.Id = 25)                                                                                                                                                                                          " +
             "\n   AND(C.Id >= 1  OR(C.Id IS NULL AND L1.Id = 25))                                                                                                                                                                                                       " +
             "\n GROUP BY                                                                                                                                                                                                                                                            " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n      CL.Id                                                                                                                                                                                                                                                          " +
             "\n     , CL.Name                                                                                                                                                                                                                                                       " +
             "\n     , S.Id                                                                                                                                                                                                                                                          " +
             "\n     , S.Name                                                                                                                                                                                                                                                        " +
             "\n     , CL1.UnitId                                                                                                                                                                                                                                                    " +
             "\n     , C.Name                                                                                                                                                                                                                                                        " +
             "\n     , L1.IsRuleConformity                                                                                                                                                                                                                                           " +
             "\n     , L1.Id                                                                                                                                                                                                                                                         " +
             "\n     , L1.Name                                                                                                                                                                                                                                                       " +
             "\n     , CRL.Id                                                                                                                                                                                                                                                        " +
             "\n     , CRL.Name                                                                                                                                                                                                                                                      " +
             //"\n     , L1C.Points                                                                                                                                                                                                                                                    " +
             "\n     , ST.Name                                                                                                                                                                                                                                                       " +
             "\n     , CT.Id                                                                                                                                                                                                                                                         " +
             "\n     , L1.HashKey " +
             "\n     , CCL.ParCluster_ID                                                                                                                                                                                                                                                   " +
             //"\n     , C.Id   , CL1.ConsolidationDate,FT.DATA, FT.PARCOMPANY_ID                                                                                                                                                                                                                                                        " +
             "\n     , C.Id   , CL1.ConsolidationDate                                                                                                                                                                                                                                                        " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n ) SCORECARD                                                                                                                                                                                                                                                         " +
             "\n                                                                                                                                                                                                                                                                     " +
             "\n ) FIM                                                                                                                                                                                                                                                               " +
             "\n                                                                                                                                                                                                                                                                     " +
  "\n    ) SC                                                                                                                                                                                                                                                               					                                               " +
  "\n WHERE ParCompanyId <> 2 " +
  "\n GROUP BY                 " +
  "\n   Cluster                " +
  "\n   , ClusterName          " +
  "\n   , Regional             " +
  "\n   , RegionalName         " +
  "\n   , ParCompanyId         " +
  "\n   , ParCompanyName       " +
  "\n   , TipoIndicador        " +
  "\n   , TipoIndicadorName    " +
  "\n   , Level1Id             " +
  "\n   , Level1Name           " +
  "\n   , Criterio             " +
  "\n   , CriterioName         " +
  "\n   , TipoScore            " +

  "\n    ORDER BY 11, 10                                                                                                                                                                                                                                                    					                                               " +
  "\n    DROP TABLE #AMOSTRATIPO4                                                                                                                                                                                                                                                                                                            " +
  "\n    DROP TABLE #VOLUMES	                                                                                                                                                                                                                                                                                                               " +
  "\n    DROP TABLE #DIASVERIFICACAO                                                                                                                                                                                                                                                                                                         " +
  "\n    DROP TABLE #NAPCC	DROP TABLE #FREQ																																																														                                                       " +
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
            var whereClusterGroup = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var whereUnit = "";

            if (form.clusterGroupId > 0)
            {
                whereClusterGroup = $@"AND C.id IN (SELECT DISTINCT c.Id FROM Parcompany c LEFT JOIN ParCompanyCluster PCC WITH (NOLOCK) ON C.Id = PCC.ParCompany_Id LEFT JOIN ParCluster PC WITH (NOLOCK) ON PC.Id = PCC.ParCluster_Id LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) ON PC.ParClusterGroup_Id = PCG.Id WHERE PCG.id = { form.clusterGroupId } AND PCC.Active = 1)";
            }

            if (form.clusterSelected_Id > 0)
            {
                whereCluster = $@"AND C.ID IN (SELECT DISTINCT c.id FROM Parcompany c Left Join ParCompanyCluster PCC with (nolock) on c.id= pcc.ParCompany_Id WHERE PCC.ParCluster_Id = { form.clusterSelected_Id } and PCC.Active = 1)";
            }

            if (form.structureId > 0)
            {
                whereStructure = $@"AND reg.id = { form.structureId }";
            }

            if (form.unitId > 0)
            {
                whereUnit = $@"AND C.Id = { form.unitId }";
            }

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND P1.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

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

            string query = VisaoGeralDaAreaApiController.sqlBase1(form) +
            //string query = ""+

            //query 1 retorna o valor da empresa    
            //"\n  																																																																						                                               " +
            "\n declare @valorEmpresa decimal(5,2) " +
"\n select @valorEmpresa = sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100" +
"\n from #Score s left join ParStructure reg on s.regional = reg.id where reg.Active =1 and Reg.ParStructureGroup_Id = 2 " +

@"
     SELECT
    @valorEmpresa = case when sum(av) is null or sum(av) = 0 then 0 else cast(round(cast(case when isnull(sum(Pontos), 100) = 0 or isnull(sum(PontosAtingidos), 100) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 100) / isnull(sum(Pontos), 100)) * 100  end as decimal (10, 1)), 2) as decimal (10, 1)) end
        FROM ParStructure Reg  with(nolock)
     LEFT JOIN ParCompanyXStructure CS  with(nolock)
  ON CS.ParStructure_Id = Reg.Id
     left join ParCompany C  with(nolock)
  on C.Id = CS.ParCompany_Id
     left join ParLevel1 P1  with(nolock)
  on 1 = 1 AND ISNULL(P1.ShowScorecard, 1) = 1
     LEFT JOIN ParGroupParLevel1XParLevel1 PP  with(nolock)
  ON PP.ParLevel1_Id = P1.Id
     LEFT JOIN ParGroupParLevel1 PP1  with(nolock)
  ON PP.ParGroupParLevel1_Id = PP1.Id
     LEFT JOIN #SCORE S  with (nolock)
  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id 
  WHERE 1 = 1
  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null
  -- AND C.id IN(SELECT DISTINCT c.Id FROM Parcompany c LEFT JOIN ParCompanyCluster PCC WITH (NOLOCK) ON C.Id = PCC.ParCompany_Id LEFT JOIN ParCluster PC WITH (NOLOCK) ON PC.Id = PCC.ParCluster_Id LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) ON PC.ParClusterGroup_Id = PCG.Id WHERE PCG.id = 8 AND PCC.Active = 1)
" +


            ////query 2 retorna o valor da regional       
            //"\n   SELECT                                                                                                                                                                                                                                                                                                                               " +
            //"\n   Reg.Name regName,                                                                                                                                                                                                                                                                                                                " +
            //"\n   Reg.Id regId,                                                                                                                                                                                                                                                                                                                      " +
            //"\n @valorEmpresa as scorecardJbs, " +
            ////"\n   case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbs,                                                                                                                                                                               " +
            //"\n   case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as scorecardJbsReg                                                                                                                                                                             " +
            //"\n   FROM ParStructure Reg                                                                                                                                                                                                                                                                                                                " +
            //"\n   left join #SCORE S                                                                                                                                                                                                                                                                                                                   " +
            //"\n   on S.Regional = Reg.Id INNER JOIN ParCompany PC ON S.ParCompany_id = pc.id                                                                                                                                                                                                                                                                                                               " +
            //"\n   where                                                                                                                                                                                                                                                                                                            " +
            //"\n   Reg.Active = 1 and Reg.ParStructureGroup_Id = 2 AND PC.IsActive = 1                                                                                                                                                                                                                                                                                                                  " +
            //"\n   GROUP BY Reg.Name, Reg.id ORDER BY 4 DESC   ";

            $@" SELECT Reg.Name as regName, Reg.id as regId,
 @valorEmpresa as scorecardJbs,
 case when sum(av) is null or sum(av) = 0 then 0 else cast(round(cast(case when isnull(sum(Pontos), 100) = 0 or isnull(sum(PontosAtingidos), 100) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 100) / isnull(sum(Pontos), 100)) * 100  end as decimal (10, 1)), 2) as decimal (10, 1)) end as scorecardJbsReg
 FROM ParStructure Reg
  LEFT JOIN ParCompanyXStructure CS
  ON CS.ParStructure_Id = Reg.Id
  left join ParCompany C
  on C.Id = CS.ParCompany_Id
  left join ParLevel1 P1
  on 1 = 1 AND ISNULL(P1.ShowScorecard, 1) = 1
  LEFT JOIN ParGroupParLevel1XParLevel1 PP
  ON PP.ParLevel1_Id = P1.Id
  LEFT JOIN ParGroupParLevel1 PP1
  ON PP.ParGroupParLevel1_Id = PP1.Id
 LEFT JOIN #SCORE S 
  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id
  WHERE 1 = 1
  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null
 { whereClusterGroup }
 { whereCluster }
 { whereStructure }
 { whereCriticalLevel }
 -- { whereUnit }
 GROUP BY Reg.Name , Reg.id
 ORDER BY 4 DESC ";



            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<VisaoGeralDaAreaResultSet>(query).ToList();
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

            var whereClusterGroup = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var whereUnit = "";

            if (form.clusterGroupId > 0)
            {
                whereClusterGroup = $@" AND C.id IN (SELECT DISTINCT c.Id FROM Parcompany c LEFT JOIN ParCompanyCluster PCC WITH (NOLOCK) ON C.Id = PCC.ParCompany_Id LEFT JOIN ParCluster PC WITH (NOLOCK) ON PC.Id = PCC.ParCluster_Id LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) ON PC.ParClusterGroup_Id = PCG.Id WHERE PCG.id = { form.clusterGroupId } AND PCC.Active = 1)";
            }

            if (form.clusterSelected_Id > 0)
            {
                whereCluster = $@" AND C.ID IN (SELECT DISTINCT c.id FROM Parcompany c Left Join ParCompanyCluster PCC with (nolock) on c.id= pcc.ParCompany_Id WHERE PCC.ParCluster_Id = { form.clusterSelected_Id } and PCC.Active = 1)";
            }

            if (form.structureId > 0)
            {
                whereStructure = $@" AND reg.id = { form.structureId }";
            }

            if (form.unitId > 0)
            {
                whereUnit = $@"AND C.Id = { form.unitId }";
            }

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"  AND S.Level1Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

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

//  "\n declare @valorEmpresa decimal(5,2) " +
//"\n select @valorEmpresa = sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100" +
//"\n from #Score s left join ParStructure reg on s.regional = reg.id where reg.Active =1 and Reg.ParStructureGroup_Id = 2 " +


"\n declare @valorEmpresa decimal(5,2) " +
"\n declare @valorRegional decimal(5, 2) " +

@"
     SELECT
    @valorEmpresa = case when sum(av) is null or sum(av) = 0 then 0 else cast(round(cast(case when isnull(sum(Pontos), 100) = 0 or isnull(sum(PontosAtingidos), 100) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 100) / isnull(sum(Pontos), 100)) * 100  end as decimal (10, 1)), 2) as decimal (10, 1)) end
        FROM ParStructure Reg  with(nolock)
     LEFT JOIN ParCompanyXStructure CS  with(nolock)
  ON CS.ParStructure_Id = Reg.Id
     left join ParCompany C  with(nolock)
  on C.Id = CS.ParCompany_Id
     left join ParLevel1 P1  with(nolock)
  on 1 = 1 AND ISNULL(P1.ShowScorecard, 1) = 1
     LEFT JOIN ParGroupParLevel1XParLevel1 PP  with(nolock)
  ON PP.ParLevel1_Id = P1.Id
     LEFT JOIN ParGroupParLevel1 PP1  with(nolock)
  ON PP.ParGroupParLevel1_Id = PP1.Id
     LEFT JOIN #SCORE S  with (nolock)
  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id 
  WHERE 1 = 1
  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null
  -- AND C.id IN(SELECT DISTINCT c.Id FROM Parcompany c LEFT JOIN ParCompanyCluster PCC WITH (NOLOCK) ON C.Id = PCC.ParCompany_Id LEFT JOIN ParCluster PC WITH (NOLOCK) ON PC.Id = PCC.ParCluster_Id LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) ON PC.ParClusterGroup_Id = PCG.Id WHERE PCG.id = 8 AND PCC.Active = 1)
" +


@"
     SELECT
    @valorRegional = case when sum(av) is null or sum(av) = 0 then 0 else cast(round(cast(case when isnull(sum(Pontos), 100) = 0 or isnull(sum(PontosAtingidos), 100) = 0 then 0 else (ISNULL(sum(PontosAtingidos), 100) / isnull(sum(Pontos), 100)) * 100  end as decimal (10, 1)), 2) as decimal (10, 1)) end
        FROM ParStructure Reg  with(nolock)
     LEFT JOIN ParCompanyXStructure CS  with(nolock)
  ON CS.ParStructure_Id = Reg.Id
     left join ParCompany C  with(nolock)
  on C.Id = CS.ParCompany_Id
     left join ParLevel1 P1  with(nolock)
  on 1 = 1 AND ISNULL(P1.ShowScorecard, 1) = 1
     LEFT JOIN ParGroupParLevel1XParLevel1 PP  with(nolock)
  ON PP.ParLevel1_Id = P1.Id
     LEFT JOIN ParGroupParLevel1 PP1  with(nolock)
  ON PP.ParGroupParLevel1_Id = PP1.Id
     LEFT JOIN #SCORE S  with (nolock)
  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id 
	INNER JOIN ParCompany PC 
  ON S.ParCompany_id = pc.id 
  WHERE 1 = 1
  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null
  -- AND C.id IN(SELECT DISTINCT c.Id FROM Parcompany c LEFT JOIN ParCompanyCluster PCC WITH (NOLOCK) ON C.Id = PCC.ParCompany_Id LEFT JOIN ParCluster PC WITH (NOLOCK) ON PC.Id = PCC.ParCluster_Id LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) ON PC.ParClusterGroup_Id = PCG.Id WHERE PCG.id = 8 AND PCC.Active = 1)
" +
"\n  AND Reg.Id = '" + form.Query.ToString() + "' AND pC.IsActive = 1 " +


"\n  SELECT " +
"\n  C.Initials companySigla, " +
"\n  case when sum(isnull(PontosIndicador, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end companyScorecard, " +
"\n  @valorEmpresa as scorecardJbs, " +
"\n  @valorRegional as scorecardJbsReg " +
"\n  FROM ParStructure Reg " +
"\n  LEFT JOIN ParCompanyXStructure CS " +
"\n  ON CS.ParStructure_Id = Reg.Id " +
"\n  left join ParCompany C " +
"\n  on C.Id = CS.ParCompany_Id " +
"\n  left join #SCORE S  " +
"\n  on C.Id = S.ParCompany_Id INNER JOIN ParCompany PC ON S.ParCompany_id = pc.id  " +
"\n  where Reg.Id = '" + form.Query.ToString() + "' AND pC.IsActive = 1 " +
     whereClusterGroup +
     whereCluster +
     whereStructure +
     whereCriticalLevel +
// whereUnit +
"\n  GROUP BY S.ParCompany_Id, S.ParCompanyName, C.Initials ORDER BY 2 DESC ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<VisaoGeralDaAreaResultSet>(query).ToList();
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
            var primeiroDiaMesAnterior = Guard.PrimeiroDiaMesAnterior(form._dataInicio);
            var proximoDomingo = Guard.GetNextWeekday(form._dataFim, DayOfWeek.Sunday);

            var whereClusterGroup = "";
            var whereCluster = "";
            var whereCriticalLevel = "";
            var whereUnit = "";

            if (form.clusterGroupId > 0)
            {
                whereClusterGroup = $@" AND UNI.id IN (SELECT DISTINCT c.Id FROM Parcompany c LEFT JOIN ParCompanyCluster PCC WITH (NOLOCK) ON C.Id = PCC.ParCompany_Id LEFT JOIN ParCluster PC WITH (NOLOCK) ON PC.Id = PCC.ParCluster_Id LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) ON PC.ParClusterGroup_Id = PCG.Id WHERE PCG.id = { form.clusterGroupId } AND PCC.Active = 1)";
            }

            if (form.clusterSelected_Id > 0)
            {
                whereCluster = $@" AND UNI.ID IN (SELECT DISTINCT c.id FROM Parcompany c Left Join ParCompanyCluster PCC with (nolock) on c.id= pcc.ParCompany_Id WHERE PCC.ParCluster_Id = { form.clusterSelected_Id } and PCC.Active = 1)";
            }

            //if (form.structureId > 0)
            //{
            //    whereStructure = $@" AND reg.id = { form.structureId }";
            //}

            if (form.unitId > 0)
            {
                whereUnit = $@"AND UNI.Id = { form.unitId }";
            }

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"  AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

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

            string query = "" +

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




                 " \n DECLARE @DATAFINAL DATE = @dataFim_ " +
                 " \n DECLARE @DATAINICIAL DATE = DateAdd(mm, DateDiff(mm, 0, @DATAFINAL) - 1, 0) " +
                 " \n DECLARE @UNIDADE INT = (SELECT Id FROM ParCompany where Initials = '" + form.ParametroTableCol[1] + "') " +



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
                     " \n ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1 " +
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
                         " \n AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 (nolock) where Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1) " +
                         " \n AND C2.UnitId = @UNIDADE " +
                         " \n AND IsNotEvaluate = 1 " +
                         " \n GROUP BY C2.ID " +
                         " \n ) NA " +
                         " \n WHERE NA = 2 " +
                 "\n select level1_id, Level1Name, Level2Name, Unidade_Id, Unidade, sum(procentagemNc) as procentagemNc, sum(Meta) as Meta, sum(nc) as nc, sum(av) av, [date] from ( " +
                 " \n SELECT " +
                  " \n level1_Id " +
                 " \n , Level1Name " +
                 " \n , Level2Name AS Level2Name " +
                  " \n , Unidade_Id " +
                  " \n , Unidade " +
                  " \n , procentagemNc " +
                  " \n ,(case when IsRuleConformity = 1 THEN(100 - META) WHEN IsRuleConformity IS NULL THEN 0 ELSE Meta END) AS Meta " +
                 " \n , NcSemPeso as nc " +
                 " \n ,AvSemPeso as av " +
                 " \n ,Data AS date " +
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

                         " \n ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1 AND IND.Name = '" + form.ParametroTableRow[1] + "' " +

                         " \n LEFT JOIN ParCompany UNI (nolock) " +

                         " \n ON UNI.Id = CL1.UnitId and UNI.Id = @UNIDADE" +
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

                              " \n ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1--AND IND.ID = 1 " +

                             " \n LEFT JOIN ParCompany UNI (nolock) " +

                              " \n ON UNI.Id = CL1.UnitId " +

                             " \n LEFT JOIN #AMOSTRATIPO4a A4 (nolock)  " +

                             " \n ON A4.UNIDADE = UNI.Id " +

                             " \n AND A4.INDICADOR = IND.ID " +
                             "\n WHERE 1 = 1 " +
                             whereClusterGroup +
                             whereCluster +
                             whereUnit +
                             whereCriticalLevel +
                             " \n GROUP BY " +

                             " \n IND.ID, " +
                             " \n IND.NAME, " +
                             " \n CL1.UnitId, " +
                             " \n UNI.NAME " +

                         " \n ) NOMES " +

                         " \n ON 1 = 1 AND(NOMES.A1 = CL1.ParLevel1_Id AND NOMES.A4 = UNI.ID) OR(IND.ID IS NULL) " +



                    " \n ) S1 " +
                 " \n ) S2 " +
                 " \n WHERE (RELATORIO_DIARIO = 1 OR(RELATORIO_DIARIO = 0 AND AV = 0)) AND Level1Name = '" + form.ParametroTableRow[1] + "' and Unidade_Id = @UNIDADE" +
                 "\n ) ff group by level1_id, Level1Name, Level2Name, Unidade_Id, Unidade,  [date] " +
                 " \n ORDER BY 10" +
                 " \n DROP TABLE #AMOSTRATIPO4a  ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<VisaoGeralDaAreaResultSet>(query).ToList();
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

        [HttpPost]
        [Route("GraficoEvolutivoEmpresa")]
        public List<ResultQueryEvolutivo> GraficoEvolutivoEmpresa([FromBody] DataCarrierFormulario form)
        {
            CriaMockGraficoEvolutivoEmpresa(form);
            //return _mock;
            return _listEvol;
        }
        private void CriaMockGraficoEvolutivoEmpresa(DataCarrierFormulario form)
        {
            _list = new List<VisaoGeralDaAreaResultSet>();

            string query = VisaoGeralDaAreaApiController.sqlBase(form, true) +

//query 2 retorna o valor da regional       
"\n   SELECT                                                                                                                                                                                                                                                                                                                               " +
"\n   case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as real,                                                                                                                                                                               " +
"\n   100 as orcado," +
"\n   100 - (case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end ) as desvio," +
"\n   (100 - (case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end )) / 100 as desviopercentual," +
"\n   cast(month(cast(s.mesData as date)) as varchar) as mes" +
"\n   FROM ParStructure Reg left join ParCompanyXStructure pcs on reg.Id = pcs.ParStructure_Id" +
"\n   left join parCompany comp on comp.id = pcs.parCompany_id left join #score s on comp.id = s.parCompany_id" +
"\n   group by month(cast(s.mesData as date))";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _listEvol = factory.SearchQuery<ResultQueryEvolutivo>(query).ToList();
            }

        }
        [HttpPost]
        [Route("GraficoEvolutivoRegional")]
        public List<ListResultQueryEvolutivo> GraficoEvolutivoRegional([FromBody] DataCarrierFormulario form)
        {
            CriaMockGraficoEvolutivoRegional(form);
            //return _mock;
            return listaRegs;
        }
        private void CriaMockGraficoEvolutivoRegional(DataCarrierFormulario form)
        {
            // Busca as regionais
            List<ParStructure> list = null;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                string consulta = "select * from parstructure where parstructuregroup_id =2";

                list = factory.SearchQuery<ParStructure>(consulta).ToList();
            }
            //Busca os dados por regional 
            listaRegs = new List<ListResultQueryEvolutivo>();
            foreach (var item in list)
            {
                string query = VisaoGeralDaAreaApiController.sqlBase(form, true) +
        //query 2 retorna o valor da regional       
        "\n   SELECT reg.name as CLASSIFIC_NEGOCIO, cast(reg.id as varchar) as MACROPROCESSO," +
        "\n   case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as real,                                                                                                                                                                               " +
        "\n   100 as orcado," +
        "\n   100 - (case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end ) as desvio," +
        "\n   (100 - (case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end )) / 100 as desviopercentual," +
        "\n   cast(month(cast(s.mesData as date)) as varchar) as mes" +
        "\n   FROM ParStructure Reg left join ParCompanyXStructure pcs on reg.Id = pcs.ParStructure_Id" +
        "\n   left join parCompany comp on comp.id = pcs.parCompany_id left join #score s on comp.id = s.parCompany_id" +
        "\n   left join parcompanyxusersgq pcu on pcu.ParCompany_Id = s.parCompany_id" +
        "\n   where reg.id = " + item.Id + "and pcu.UserSgq_Id =" + form.auditorId +
        "\n   group by reg.name, reg.id, month(cast(s.mesData as date))" +
        "\n   order by reg.id, mes ";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    ListResultQueryEvolutivo it = new ListResultQueryEvolutivo();
                    it.lista = factory.SearchQuery<ResultQueryEvolutivo>(query).ToList();
                    listaRegs.Add(it);
                }

            }
        }
        [HttpPost]
        [Route("GraficoEvolutivoUnidade")]
        public List<ListResultQueryEvolutivo> GraficoEvolutivoUnidade([FromBody] DataCarrierFormulario form)
        {
            CriaMockGraficoEvolutivoUnidade(form);
            //return _mock;
            return listaUnidades;
        }
        private void CriaMockGraficoEvolutivoUnidade(DataCarrierFormulario form)
        {
            // Busca as unidades na regional
            List<ParCompanyXStructure> list = null;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                string consulta = "select * from ParCompanyXStructure where ParStructure_Id =" + form.Query;
                list = factory.SearchQuery<ParCompanyXStructure>(consulta).ToList();
            }
            //Busca os dados por unidade 
            listaUnidades = new List<ListResultQueryEvolutivo>();
            foreach (var item in list)
            {
                string query = VisaoGeralDaAreaApiController.sqlBase(form, true) +
        //query 2 retorna o valor da regional       
        "\n   SELECT s.ParCompanyName as CLASSIFIC_NEGOCIO, cast(s.ParCompany_Id as varchar) as MACROPROCESSO," +
        "\n   case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end as real,                                                                                                                                                                               " +
        "\n   100 as orcado," +
        "\n   100 - (case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end ) as desvio," +
        "\n   (100 - (case when sum(isnull(PontosAtingidos, 0)) = 0 then 0 else sum(isnull(PontosAtingidos, 0)) / sum(isnull(PontosIndicador, 0)) * 100 end )) / 100 as desviopercentual," +
        "\n   cast(month(cast(s.mesData as date)) as varchar) as mes" +
        "\n   FROM ParStructure Reg left join ParCompanyXStructure pcs on reg.Id = pcs.ParStructure_Id" +
        "\n   left join parCompany comp on comp.id = pcs.parCompany_id left join #score s on comp.id = s.parCompany_id" +
        "\n   left join parcompanyxusersgq pcu on pcu.ParCompany_Id = s.parCompany_id" +
        "\n   where s.ParCompany_Id = " + item.ParCompany_Id + "and pcu.UserSgq_Id =" + form.auditorId + " and s.mesData is not null" +
        "\n   group by s.ParCompanyName,s.parCompany_id,month(cast(s.mesData as date))" +
        "\n   order by mes ";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    ListResultQueryEvolutivo it = new ListResultQueryEvolutivo();
                    it.lista = factory.SearchQuery<ResultQueryEvolutivo>(query).ToList();
                    if (it.lista.Count > 0)
                        listaUnidades.Add(it);
                }

            }
        }

    }

    public class ResultQueryEvolutivo
    {
        public string CLASSIFIC_NEGOCIO { get; set; }
        public string MACROPROCESSO { get; set; }
        public int ORCADO { get; set; }
        public decimal DESVIO { get; set; }
        public decimal DESVIOPERCENTUAL { get; set; }
        public decimal REAL { get; set; }
        public string mes { get; set; } = null;

    }
    public class ListResultQueryEvolutivo
    {
        public List<ResultQueryEvolutivo> lista { get; set; }

    }
}

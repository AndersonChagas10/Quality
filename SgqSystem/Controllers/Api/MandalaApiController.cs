using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Mandala")]
    public class MandalaApiController : BaseApiController
    {
        private List<JObject> Lista { get; set; }

        [HttpPost]
        [Route("MostrarIndicadorMandala")]
        public List<JObject> MostrarIndicadorMandala([FromBody] FormularioParaRelatorioViewModel form)
        {
            var whereDepartment = "";
            var whereShift = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereUnit = "";
            var whereCriticalLevel = "";
            var whereClusterGroup = "";

            var dataInicio = "20170304";
            var dataFim = "20180304";

            var query = $@"
                 DECLARE @DATAINICIAL DATETIME = '{ dataInicio} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ dataFim } {" 23:59:59"}'
                 DECLARE @VOLUMEPCC int
                                                                  
                 CREATE TABLE #AMOSTRATIPO4 ( 
                 UNIDADE INT NULL, 
                 INDICADOR INT NULL, 
                 AM INT NULL, 
                 DEF_AM INT NULL 
                 )
                INSERT INTO #AMOSTRATIPO4
                	SELECT
                		UNIDADE
                	   ,INDICADOR
                	   ,COUNT(1) AM
                	   ,SUM(DEF_AM) DEF_AM
                	FROM (SELECT
                			CAST(C2.CollectionDate AS DATE) AS DATA
                		   ,C.Id AS UNIDADE
                		   ,C2.ParLevel1_Id AS INDICADOR
                		   ,C2.EvaluationNumber AS AV
                		   ,C2.Sample AS AM
                		   ,CASE
                				WHEN SUM(C2.WeiDefects) = 0 THEN 0
                				ELSE 1
                			END DEF_AM
                		FROM CollectionLevel2 C2 (NOLOCK)
                		INNER JOIN ParLevel1 L1 (NOLOCK)
                			ON L1.Id = C2.ParLevel1_Id
                		INNER JOIN ParCompany C (NOLOCK)
                			ON C.Id = C2.UnitId
                		WHERE C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                		AND C2.NotEvaluatedIs = 0
                		AND C2.Duplicated = 0
                		AND L1.ParConsolidationType_Id = 4
                		GROUP BY C.Id
                				,ParLevel1_Id
                				,EvaluationNumber
                				,Sample
                				,CAST(CollectionDate AS DATE)) TAB
                	GROUP BY UNIDADE
                			,INDICADOR
                SELECT
                	Unidade AS UnidadeName
                   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
                   ,ProcentagemNc AS [proc]
                   ,nc
                   ,av
                FROM (SELECT
                		Unidade
                	   ,Unidade_Id
                	   ,SUM(avSemPeso) AS av
                	   ,SUM(ncSemPeso) AS nc
                	   ,CASE
                			WHEN SUM(AV) IS NULL OR
                				SUM(AV) = 0 THEN 0
                			ELSE SUM(NC) / SUM(AV) * 100
                		END AS ProcentagemNc
                	FROM (SELECT
                			IND.Id AS level1_Id
                		   ,IND.Name AS Level1Name
                		   ,UNI.Id AS Unidade_Id
                		   ,UNI.Name AS Unidade
                		   ,CASE
                				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
                							SUM(Quartos)
                						FROM VolumePcc1b
                						WHERE ParCompany_id = UNI.Id
                						AND Data = CL1.ConsolidationDate)
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.EvaluatedResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.WeiEvaluation
                				ELSE 0
                			END AS Av
                		   ,CASE
                				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
                							SUM(Quartos)
                						FROM VolumePcc1b
                						WHERE ParCompany_id = UNI.Id
                						AND Data = CL1.ConsolidationDate)
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.EvaluateTotal
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.EvaluatedResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.EvaluateTotal
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.EvaluateTotal
                				ELSE 0
                			END AS AvSemPeso
                		   ,CASE
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.DefectsResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.TotalLevel3WithDefects
                				ELSE 0
                			END AS NC
                		   ,CASE
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.DefectsResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.TotalLevel3WithDefects
                				ELSE 0
                			END AS NCSemPeso
                		   ,CASE
                
                				WHEN (SELECT
                							COUNT(1)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						AND G.AddDate <= CL1.ConsolidationDate)
                					> 0 THEN (SELECT TOP 1
                							ISNULL(G.PercentValue, 0)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						AND G.AddDate <= CL1.ConsolidationDate
                						ORDER BY G.ParCompany_Id DESC, AddDate DESC)
                
                				ELSE (SELECT TOP 1
                							ISNULL(G.PercentValue, 0)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						ORDER BY G.ParCompany_Id DESC, AddDate ASC)
                			END
                			AS Meta
                		FROM ConsolidationLevel1 CL1 (NOLOCK)
                		INNER JOIN ParLevel1 IND (NOLOCK)
                			ON IND.Id = CL1.ParLevel1_Id 
                            AND isnull(IND.ShowScorecard,1) = 1
                            AND IND.IsActive = 1
                            AND IND.ID != 43
                		INNER JOIN ParCompany UNI (NOLOCK)
                			ON UNI.Id = CL1.UnitId
                            and UNI.IsActive = 1
                		--INNER JOIN ParCompanyXUserSgq CU(nolock) 
                		--ON CU.UserSgq_Id = { 1 } and CU.ParCompany_Id = UNI.Id 
                		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
                			ON A4.UNIDADE = UNI.Id
                			AND A4.INDICADOR = IND.ID
                		-- INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
                			-- ON CL2.ConsolidationLevel1_id = CL1.Id
                		-- INNER JOIN ParLevel2 L2 WITH (NOLOCK)
                			-- ON CL2.ParLevel2_id = L2.Id
                		-- INNER JOIN ParDepartment D WITH (NOLOCK)
                			-- ON L2.ParDepartment_Id = D.Id
                        LEFT JOIN ParCompanyCluster PCC
		                	ON PCC.ParCompany_Id = UNI.Id
                            and PCC.Active = 1
		                LEFT JOIN ParCluster PC
		                	ON PCC.ParCluster_Id = PC.Id
		                LEFT JOIN ParClusterGroup PCG
		                	ON PC.ParClusterGroup_Id = PCG.Id
                		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
                        
                        { whereDepartment }
                        { whereShift }
                		-- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) 
                        { whereUnit }
                		{ whereCluster }
                        { whereStructure } 
                        { whereCriticalLevel }
                        { whereClusterGroup }
                		) S1
                	GROUP BY Unidade
                			,Unidade_Id) S2
                 WHERE ProcentagemNc <> 0
                ORDER BY 3 DESC
                DROP TABLE #AMOSTRATIPO4";

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                Lista = QueryNinja(dbSgq, query);
            }

            return Lista;
        }

        [HttpPost]
        [Route("MostrarMonitoramentoMandala")]
        public List<JObject> MostrarMonitoramentoMandala([FromBody] FormularioParaRelatorioViewModel form)
        {
            var whereDepartment = "";
            var whereShift = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereUnit = "";
            var whereCriticalLevel = "";
            var whereClusterGroup = "";

            var dataInicio = "20170304";
            var dataFim = "20180304";

            var query = $@"
                 DECLARE @DATAINICIAL DATETIME = '{ dataInicio} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ dataFim } {" 23:59:59"}'
                 DECLARE @VOLUMEPCC int
                                                                  
                 CREATE TABLE #AMOSTRATIPO4 ( 
                 UNIDADE INT NULL, 
                 INDICADOR INT NULL, 
                 AM INT NULL, 
                 DEF_AM INT NULL 
                 )
                INSERT INTO #AMOSTRATIPO4
                	SELECT
                		UNIDADE
                	   ,INDICADOR
                	   ,COUNT(1) AM
                	   ,SUM(DEF_AM) DEF_AM
                	FROM (SELECT
                			CAST(C2.CollectionDate AS DATE) AS DATA
                		   ,C.Id AS UNIDADE
                		   ,C2.ParLevel1_Id AS INDICADOR
                		   ,C2.EvaluationNumber AS AV
                		   ,C2.Sample AS AM
                		   ,CASE
                				WHEN SUM(C2.WeiDefects) = 0 THEN 0
                				ELSE 1
                			END DEF_AM
                		FROM CollectionLevel2 C2 (NOLOCK)
                		INNER JOIN ParLevel1 L1 (NOLOCK)
                			ON L1.Id = C2.ParLevel1_Id
                		INNER JOIN ParCompany C (NOLOCK)
                			ON C.Id = C2.UnitId
                		WHERE C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                		AND C2.NotEvaluatedIs = 0
                		AND C2.Duplicated = 0
                		AND L1.ParConsolidationType_Id = 4
                		GROUP BY C.Id
                				,ParLevel1_Id
                				,EvaluationNumber
                				,Sample
                				,CAST(CollectionDate AS DATE)) TAB
                	GROUP BY UNIDADE
                			,INDICADOR
                SELECT
                	Unidade AS UnidadeName
                   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
                   ,ProcentagemNc AS [proc]
                   ,nc
                   ,av
                FROM (SELECT
                		Unidade
                	   ,Unidade_Id
                	   ,SUM(avSemPeso) AS av
                	   ,SUM(ncSemPeso) AS nc
                	   ,CASE
                			WHEN SUM(AV) IS NULL OR
                				SUM(AV) = 0 THEN 0
                			ELSE SUM(NC) / SUM(AV) * 100
                		END AS ProcentagemNc
                	FROM (SELECT
                			IND.Id AS level1_Id
                		   ,IND.Name AS Level1Name
                		   ,UNI.Id AS Unidade_Id
                		   ,UNI.Name AS Unidade
                		   ,CASE
                				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
                							SUM(Quartos)
                						FROM VolumePcc1b
                						WHERE ParCompany_id = UNI.Id
                						AND Data = CL1.ConsolidationDate)
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.EvaluatedResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.WeiEvaluation
                				ELSE 0
                			END AS Av
                		   ,CASE
                				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
                							SUM(Quartos)
                						FROM VolumePcc1b
                						WHERE ParCompany_id = UNI.Id
                						AND Data = CL1.ConsolidationDate)
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.EvaluateTotal
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.EvaluatedResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.EvaluateTotal
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.EvaluateTotal
                				ELSE 0
                			END AS AvSemPeso
                		   ,CASE
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.DefectsResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.TotalLevel3WithDefects
                				ELSE 0
                			END AS NC
                		   ,CASE
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.DefectsResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.TotalLevel3WithDefects
                				ELSE 0
                			END AS NCSemPeso
                		   ,CASE
                
                				WHEN (SELECT
                							COUNT(1)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						AND G.AddDate <= CL1.ConsolidationDate)
                					> 0 THEN (SELECT TOP 1
                							ISNULL(G.PercentValue, 0)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						AND G.AddDate <= CL1.ConsolidationDate
                						ORDER BY G.ParCompany_Id DESC, AddDate DESC)
                
                				ELSE (SELECT TOP 1
                							ISNULL(G.PercentValue, 0)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						ORDER BY G.ParCompany_Id DESC, AddDate ASC)
                			END
                			AS Meta
                		FROM ConsolidationLevel1 CL1 (NOLOCK)
                		INNER JOIN ParLevel1 IND (NOLOCK)
                			ON IND.Id = CL1.ParLevel1_Id 
                            AND isnull(IND.ShowScorecard,1) = 1
                            AND IND.IsActive = 1
                            AND IND.ID != 43
                		INNER JOIN ParCompany UNI (NOLOCK)
                			ON UNI.Id = CL1.UnitId
                            and UNI.IsActive = 1
                		--INNER JOIN ParCompanyXUserSgq CU(nolock) 
                		--ON CU.UserSgq_Id = { 1 } and CU.ParCompany_Id = UNI.Id 
                		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
                			ON A4.UNIDADE = UNI.Id
                			AND A4.INDICADOR = IND.ID
                		-- INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
                			-- ON CL2.ConsolidationLevel1_id = CL1.Id
                		-- INNER JOIN ParLevel2 L2 WITH (NOLOCK)
                			-- ON CL2.ParLevel2_id = L2.Id
                		-- INNER JOIN ParDepartment D WITH (NOLOCK)
                			-- ON L2.ParDepartment_Id = D.Id
                        LEFT JOIN ParCompanyCluster PCC
		                	ON PCC.ParCompany_Id = UNI.Id
                            and PCC.Active = 1
		                LEFT JOIN ParCluster PC
		                	ON PCC.ParCluster_Id = PC.Id
		                LEFT JOIN ParClusterGroup PCG
		                	ON PC.ParClusterGroup_Id = PCG.Id
                		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
                        
                        { whereDepartment }
                        { whereShift }
                		-- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) 
                        { whereUnit }
                		{ whereCluster }
                        { whereStructure } 
                        { whereCriticalLevel }
                        { whereClusterGroup }
                		) S1
                	GROUP BY Unidade
                			,Unidade_Id) S2
                 WHERE ProcentagemNc <> 0
                ORDER BY 3 DESC
                DROP TABLE #AMOSTRATIPO4";

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                Lista = QueryNinja(dbSgq, query);
            }

            return Lista;
        }

        [HttpPost]
        [Route("MostrarTarefaMandala")]
        public List<JObject> MostrarTarefaMandala([FromBody] FormularioParaRelatorioViewModel form)
        {
            var whereDepartment = "";
            var whereShift = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereUnit = "";
            var whereCriticalLevel = "";
            var whereClusterGroup = "";

            var dataInicio = "20170304";
            var dataFim = "20180304";

            var query = $@"
                 DECLARE @DATAINICIAL DATETIME = '{ dataInicio} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ dataFim } {" 23:59:59"}'
                 DECLARE @VOLUMEPCC int
                                                                  
                 CREATE TABLE #AMOSTRATIPO4 ( 
                 UNIDADE INT NULL, 
                 INDICADOR INT NULL, 
                 AM INT NULL, 
                 DEF_AM INT NULL 
                 )
                INSERT INTO #AMOSTRATIPO4
                	SELECT
                		UNIDADE
                	   ,INDICADOR
                	   ,COUNT(1) AM
                	   ,SUM(DEF_AM) DEF_AM
                	FROM (SELECT
                			CAST(C2.CollectionDate AS DATE) AS DATA
                		   ,C.Id AS UNIDADE
                		   ,C2.ParLevel1_Id AS INDICADOR
                		   ,C2.EvaluationNumber AS AV
                		   ,C2.Sample AS AM
                		   ,CASE
                				WHEN SUM(C2.WeiDefects) = 0 THEN 0
                				ELSE 1
                			END DEF_AM
                		FROM CollectionLevel2 C2 (NOLOCK)
                		INNER JOIN ParLevel1 L1 (NOLOCK)
                			ON L1.Id = C2.ParLevel1_Id
                		INNER JOIN ParCompany C (NOLOCK)
                			ON C.Id = C2.UnitId
                		WHERE C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                		AND C2.NotEvaluatedIs = 0
                		AND C2.Duplicated = 0
                		AND L1.ParConsolidationType_Id = 4
                		GROUP BY C.Id
                				,ParLevel1_Id
                				,EvaluationNumber
                				,Sample
                				,CAST(CollectionDate AS DATE)) TAB
                	GROUP BY UNIDADE
                			,INDICADOR
                SELECT
                	Unidade AS UnidadeName
                   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
                   ,ProcentagemNc AS [proc]
                   ,nc
                   ,av
                FROM (SELECT
                		Unidade
                	   ,Unidade_Id
                	   ,SUM(avSemPeso) AS av
                	   ,SUM(ncSemPeso) AS nc
                	   ,CASE
                			WHEN SUM(AV) IS NULL OR
                				SUM(AV) = 0 THEN 0
                			ELSE SUM(NC) / SUM(AV) * 100
                		END AS ProcentagemNc
                	FROM (SELECT
                			IND.Id AS level1_Id
                		   ,IND.Name AS Level1Name
                		   ,UNI.Id AS Unidade_Id
                		   ,UNI.Name AS Unidade
                		   ,CASE
                				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
                							SUM(Quartos)
                						FROM VolumePcc1b
                						WHERE ParCompany_id = UNI.Id
                						AND Data = CL1.ConsolidationDate)
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.EvaluatedResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.WeiEvaluation
                				ELSE 0
                			END AS Av
                		   ,CASE
                				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
                							SUM(Quartos)
                						FROM VolumePcc1b
                						WHERE ParCompany_id = UNI.Id
                						AND Data = CL1.ConsolidationDate)
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.EvaluateTotal
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiEvaluation
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.EvaluatedResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.EvaluateTotal
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.EvaluateTotal
                				ELSE 0
                			END AS AvSemPeso
                		   ,CASE
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.DefectsResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.WeiDefects
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.TotalLevel3WithDefects
                				ELSE 0
                			END AS NC
                		   ,CASE
                				WHEN IND.ParConsolidationType_Id = 1 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 2 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 3 THEN CL1.DefectsResult
                				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
                				WHEN IND.ParConsolidationType_Id = 5 THEN CL1.DefectsTotal
                				WHEN IND.ParConsolidationType_Id = 6 THEN CL1.TotalLevel3WithDefects
                				ELSE 0
                			END AS NCSemPeso
                		   ,CASE
                
                				WHEN (SELECT
                							COUNT(1)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						AND G.AddDate <= CL1.ConsolidationDate)
                					> 0 THEN (SELECT TOP 1
                							ISNULL(G.PercentValue, 0)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						AND G.AddDate <= CL1.ConsolidationDate
                						ORDER BY G.ParCompany_Id DESC, AddDate DESC)
                
                				ELSE (SELECT TOP 1
                							ISNULL(G.PercentValue, 0)
                						FROM ParGoal G
                						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
                						AND (G.ParCompany_id = CL1.UnitId
                						OR G.ParCompany_id IS NULL)
                						ORDER BY G.ParCompany_Id DESC, AddDate ASC)
                			END
                			AS Meta
                		FROM ConsolidationLevel1 CL1 (NOLOCK)
                		INNER JOIN ParLevel1 IND (NOLOCK)
                			ON IND.Id = CL1.ParLevel1_Id 
                            AND isnull(IND.ShowScorecard,1) = 1
                            AND IND.IsActive = 1
                            AND IND.ID != 43
                		INNER JOIN ParCompany UNI (NOLOCK)
                			ON UNI.Id = CL1.UnitId
                            and UNI.IsActive = 1
                		--INNER JOIN ParCompanyXUserSgq CU(nolock) 
                		--ON CU.UserSgq_Id = { 1 } and CU.ParCompany_Id = UNI.Id 
                		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
                			ON A4.UNIDADE = UNI.Id
                			AND A4.INDICADOR = IND.ID
                		-- INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
                			-- ON CL2.ConsolidationLevel1_id = CL1.Id
                		-- INNER JOIN ParLevel2 L2 WITH (NOLOCK)
                			-- ON CL2.ParLevel2_id = L2.Id
                		-- INNER JOIN ParDepartment D WITH (NOLOCK)
                			-- ON L2.ParDepartment_Id = D.Id
                        LEFT JOIN ParCompanyCluster PCC
		                	ON PCC.ParCompany_Id = UNI.Id
                            and PCC.Active = 1
		                LEFT JOIN ParCluster PC
		                	ON PCC.ParCluster_Id = PC.Id
		                LEFT JOIN ParClusterGroup PCG
		                	ON PC.ParClusterGroup_Id = PCG.Id
                		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
                        
                        { whereDepartment }
                        { whereShift }
                		-- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) 
                        { whereUnit }
                		{ whereCluster }
                        { whereStructure } 
                        { whereCriticalLevel }
                        { whereClusterGroup }
                		) S1
                	GROUP BY Unidade
                			,Unidade_Id) S2
                 WHERE ProcentagemNc <> 0
                ORDER BY 3 DESC
                DROP TABLE #AMOSTRATIPO4";

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                Lista = QueryNinja(dbSgq, query);
            }

            return Lista;
        }
    }
}



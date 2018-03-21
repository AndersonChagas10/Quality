using ADOFactory;
using Dominio;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/NaoConformidade")]
    public class NaoConformidadeApiController : ApiController
    {
        private List<NaoConformidadeResultsSet> _mock { get; set; }
        private List<NaoConformidadeResultsSet> _list { get; set; }

        [HttpPost]
        [Route("GraficoUnidades")]
        public List<NaoConformidadeResultsSet> GraficoUnidades([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Relatorio_Nao_Conformidade");

            var whereDepartment = "";
            var whereShift = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereUnit = "";
            var whereCriticalLevel = "";
            var whereClusterGroup = "";
            var whereModule = "";

            if (form.moduleId != 0)
            {
                whereModule = $@"AND PC.Id in (SELECT ParCluster_Id FROM ParClusterXModule WHERE isActive = 1 AND ParModule_Id = " + form.moduleId + ") ";
            }
                if (form.departmentId != 0)
            {
                whereDepartment = $@" AND IND.ID IN (
                                select distinct
                                    l21.ParLevel1_Id
                                    from ParLevel2Level1 l21

                                    inner join ParLevel2 l1

                                        on l21.ParLevel1_Id = l1.id

                                    inner join ParLevel2 l2

                                        on l21.ParLevel2_Id = l2.id
                                WHERE 1 = 1
                                AND L2.ParDepartment_Id = '{ form.departmentId }'
                                ";
            }

            if (form.shift != 0)
            {
                whereShift = "\n AND CL1.Shift = " + form.shift + " ";
            }

            if (form.unitId > 0)
            {
                whereUnit = $@"AND UNI.Id = { form.unitId }";
            }
            else
            {
                whereUnit = $@"AND UNI.Id IN (SELECT
                				ParCompany_Id
                			FROM ParCompanyXUserSgq
                			WHERE UserSgq_Id = { form.auditorId })";
            }

            if (form.clusterGroupId > 0)
            {
                whereClusterGroup = $@"AND PCG.Id = { form.clusterGroupId }";
            }

            if (form.clusterSelected_Id > 0)
            {
                whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id = { form.clusterSelected_Id } AND PCC.Active = 1)";
            }

            if (form.structureId > 0)
            {
                whereStructure = $@"AND UNI.Id IN(SELECT PXS.ParCompany_Id FROM ParCompanyXStructure PXS WHERE PXS.ParStructure_Id = { form.structureId } AND PXS.Active = 1)";
            }

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var query = $@"
                 DECLARE @DATAINICIAL DATETIME = '{ form._dataInicioSQL} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ form._dataFimSQL } {" 23:59:59"}'
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
                		--ON CU.UserSgq_Id = { form.auditorId } and CU.ParCompany_Id = UNI.Id 
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
                        { whereModule }
                        -- { whereClusterGroup }
                		) S1
                	GROUP BY Unidade
                			,Unidade_Id) S2
                 WHERE ProcentagemNc <> 0
                ORDER BY 3 DESC
                DROP TABLE #AMOSTRATIPO4";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoDepartamentos")]
        public List<NaoConformidadeResultsSet> GraficoDepartamentos([FromBody] FormularioParaRelatorioViewModel form)
        {

            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";
            var whereModule = "";

            if (form.moduleId != 0)
            {
                whereModule = $@"AND PC.Id in (SELECT ParCluster_Id FROM ParClusterXModule WHERE isActive = 1 AND ParModule_Id = " + form.moduleId + ") ";
            }

            if (form.departmentId != 0)
            {
                whereDepartment = "\n AND L2.ParDepartment_Id = " + form.departmentId + " ";
            }

            if (form.departmentName != "" && form.departmentName != null)
            {
                whereDepartment = "\n AND D.Name = '" + form.departmentName + "'";
            }

            if (form.shift != 0)
            {
                whereShift = "\n AND CL1.Shift = " + form.shift + " ";
            }

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var query = @"
             
                         DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL +" 00:00:00"+ @"'
                                                                                                                                                                                                                                            
                         DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + " 23:59:59" + @"'
                                                                                                                                                                                                                                            
                         DECLARE @VOLUMEPCC int
                                                                          
                         DECLARE @ParCompany_id INT
            SELECT
            	@ParCompany_id = ID
            FROM PARCOMPANY
            WHERE NAME = '" + form.unitName + @"'
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
            --------------------------------                                                                                                                     
            
            SELECT TOP 1
            	@VOLUMEPCC = SUM(Quartos)
            FROM VolumePcc1b(nolock)
            WHERE ParCompany_id = @ParCompany_id
            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL

            DECLARE @NAPCC INT
            
            SELECT
            	@NAPCC =
            	COUNT(1)
            FROM (SELECT
            		COUNT(1) AS NA
            	FROM CollectionLevel2 C2 (NOLOCK)
            	LEFT JOIN Result_Level3 C3 (NOLOCK)
            		ON C3.CollectionLevel2_Id = C2.Id
            	WHERE C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
            	AND C2.ParLevel1_Id = (SELECT TOP 1
            			id
            		FROM Parlevel1
            		WHERE Hashkey = 1)
            	AND C2.UnitId = @ParCompany_Id
            	AND IsNotEvaluate = 1
            	GROUP BY C2.ID) NA
            WHERE NA = 2
            --------------------------------  
            SELECT
            	UnidadeName
               ,Unidade_Id
               ,IIF(SUM(avComPeso) IS NULL OR SUM(avComPeso) = 0, 0, SUM(ncComPeso) / SUM(avComPeso) * 100) AS [proc]
               ,SUM(Meta) as Meta
               ,SUM(NC) AS NC
               ,SUM(Av) AS Av
               ,DepartamentoName
               ,Departamento_Id
            FROM (SELECT
            		CONVERT(VARCHAR(153), Unidade) AS UnidadeName
            	   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
            		--,CONVERT(VARCHAR(153), level1_Id) AS Indicador_Id
            		--,CONVERT(VARCHAR(153), Level1Name) AS IndicadorName
            	   ,avComPeso
                   ,ncComPeso
            	   ,(CASE
            			WHEN IsRuleConformity = 1 THEN (100 - META)
            			ELSE Meta
            		END) AS Meta
            	   ,NC
            	   ,Av
            	   ,DepartamentoName
            	   ,CONVERT(VARCHAR(153), Departamento_Id) AS Departamento_Id
            	--,IsRuleConformity
            	FROM (SELECT
            			Unidade
            		   ,IsRuleConformity
            		   ,Unidade_Id
            			--,Level1Name
            			--,level1_Id
            		   ,SUM(avSemPeso) AS av
            		   ,SUM(ncSemPeso) AS nc
            		   ,SUM(av) AS avComPeso
            		   ,SUM(nc) AS ncComPeso
            		   ,CASE
            				WHEN SUM(AV) IS NULL OR
            					SUM(AV) = 0 THEN 0
            				ELSE SUM(NC) / SUM(AV) * 100
            			END AS ProcentagemNc
            		   ,MAX(Meta) AS Meta
            		   ,DepartamentoName
            		   ,Departamento_Id
            		FROM (SELECT
            				IND.Id AS level1_Id
            			   ,IND.IsRuleConformity
            			   ,IND.Name AS Level1Name
            			   ,UNI.Id AS Unidade_Id
            			   ,UNI.Name AS Unidade
            			   ,CASE
            					WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation
            					ELSE 0
            				END AS Av
            			   ,CASE
            					WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal
            					ELSE 0
            				END AS AvSemPeso
            			   ,CASE
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
            					ELSE 0
            				END AS NC
            			   ,CASE
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
            					ELSE 0
            				END AS NCSemPeso
            			   ,CASE
            
            					WHEN (SELECT
            								COUNT(1)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.AddDate <= CL2.ConsolidationDate)
            						> 0 THEN (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.AddDate <= CL2.ConsolidationDate
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
            			   ,D.Name AS DepartamentoName
            			   ,D.Id AS Departamento_Id
            			FROM ConsolidationLevel1 CL1 (NOLOCK)
            			INNER JOIN ParLevel1 IND (NOLOCK)
            				ON IND.Id = CL1.ParLevel1_Id
                            AND ISNULL(IND.ShowScorecard,1) = 1
                            AND IND.IsActive = 1
                            AND IND.ID != 43
            			INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
            				ON CL2.ConsolidationLevel1_id = CL1.Id
            			INNER JOIN ParLevel2 L2 WITH (NOLOCK)
            				ON CL2.ParLevel2_id = L2.Id
            			INNER JOIN ParDepartment D WITH (NOLOCK)
            				ON L2.ParDepartment_Id = D.Id
            			INNER JOIN ParCompany UNI (NOLOCK)
            				ON UNI.Id = CL1.UnitId
            			LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
            				ON A4.UNIDADE = UNI.Id
            				AND A4.INDICADOR = IND.ID
            			WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
            			AND UNI.Name = '" + form.unitName + @"'
                        " + whereDepartment + @"
                        " + whereShift + @"
                        " + whereCriticalLevel + @"
            		--AND D.Id = 2
            		) S1
            		GROUP BY Unidade
            				,Unidade_Id
            				 --,Level1Name
            				 --,level1_Id
            				,IsRuleConformity
            				,DepartamentoName
            				,Departamento_Id) S2
            	 ) A
            GROUP BY UnidadeName
            		,Unidade_Id
            		,DepartamentoName
            		,Departamento_Id
            HAVING SUM(ncComPeso) <> 0
            ORDER BY 3 DESC
            DROP TABLE #AMOSTRATIPO4 ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoDepartamentosPorShift")]
        public List<NaoConformidadeResultsSet> GraficoDepartamentosPorShift([FromBody] FormularioParaRelatorioViewModel form)
        {

            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";
            var whereModule = "";

            if (form.moduleId != 0)
            {
                whereModule = $@"AND PC.Id in (SELECT ParCluster_Id FROM ParClusterXModule WHERE isActive = 1 AND ParModule_Id = " + form.moduleId + ") ";
            }

            if (form.departmentId != 0)
            {
                whereDepartment = "\n AND L2.ParDepartment_Id = " + form.departmentId + " ";
            }

            if (form.departmentName != "" && form.departmentName != null)
            {
                whereDepartment = "\n AND D.Name = '" + form.departmentName + "'";
            }

            if (form.shift != 0)
            {
                whereShift = "\n AND CL1.Shift = " + form.shift + " ";
            }

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var query = @"
             
                         DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'
                                                                                                                                                                                                                                            
                         DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
                                                                                                                                                                                                                                            
                         DECLARE @VOLUMEPCC int
                                                                          
                         DECLARE @ParCompany_id INT
            SELECT
            	@ParCompany_id = ID
            FROM PARCOMPANY
            WHERE NAME = '" + form.unitName + @"'
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
            		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATAINICIAL AND @DATAFINAL
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
            --------------------------------                                                                                                                     
            
            SELECT TOP 1
            	@VOLUMEPCC = SUM(Quartos)
            FROM VolumePcc1b(nolock)
            WHERE ParCompany_id = @ParCompany_id
            AND Data BETWEEN @DATAINICIAL AND @DATAFINAL
                                                                                                                                                                          
                          DECLARE @NAPCC INT
            
            SELECT
            	@NAPCC =
            	COUNT(1)
            FROM (SELECT
            		COUNT(1) AS NA
            	FROM CollectionLevel2 C2 (NOLOCK)
            	LEFT JOIN Result_Level3 C3 (NOLOCK)
            		ON C3.CollectionLevel2_Id = C2.Id
            	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
            	AND C2.ParLevel1_Id = (SELECT TOP 1
            			id
            		FROM Parlevel1
            		WHERE Hashkey = 1)
            	AND C2.UnitId = @ParCompany_Id
            	AND IsNotEvaluate = 1
            	GROUP BY C2.ID) NA
            WHERE NA = 2
            --------------------------------  
            SELECT
               concat(DepartamentoName, ' - Shift ', Case when Shift = 1 then 'A' else 'B' END) as 'dataX'   
               ,UnidadeName
               ,Unidade_Id
               ,SUM([proc]) AS 'proc'
                  ,SUM(Meta) as Meta
               ,SUM(NC) AS NC
               ,SUM(Av) AS Av
               ,DepartamentoName
               ,Departamento_Id
               ,Shift
            FROM (SELECT
            		CONVERT(VARCHAR(153), Unidade) AS UnidadeName
            	   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
            		--,CONVERT(VARCHAR(153), level1_Id) AS Indicador_Id
            		--,CONVERT(VARCHAR(153), Level1Name) AS IndicadorName
            	   ,ProcentagemNc AS [proc]
            	   ,(CASE
            			WHEN IsRuleConformity = 1 THEN (100 - META)
            			ELSE Meta
            		END) AS Meta
            	   ,NC
            	   ,Av
            	   ,DepartamentoName
            	   ,CONVERT(VARCHAR(153), Departamento_Id) AS Departamento_Id
            	--,IsRuleConformity
                  ,Shift
            	FROM (SELECT
            			Unidade
            		   ,IsRuleConformity
            		   ,Unidade_Id
            			--,Level1Name
            			--,level1_Id
            		   ,SUM(avSemPeso) AS av
            		   ,SUM(ncSemPeso) AS nc
            		   ,CASE
            				WHEN SUM(AV) IS NULL OR
            					SUM(AV) = 0 THEN 0
            				ELSE SUM(NC) / SUM(AV) * 100
            			END AS ProcentagemNc
            		   ,MAX(Meta) AS Meta
            		   ,DepartamentoName
            		   ,Departamento_Id
                       ,Shift
            		FROM (SELECT
            				IND.Id AS level1_Id
            			   ,IND.IsRuleConformity
            			   ,IND.Name AS Level1Name
            			   ,UNI.Id AS Unidade_Id
            			   ,UNI.Name AS Unidade
                           ,CL1.Shift AS Shift
            			   ,CASE
            					WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation
            					ELSE 0
            				END AS Av
            			   ,CASE
            					WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal
            					ELSE 0
            				END AS AvSemPeso
            			   ,CASE
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
            					ELSE 0
            				END AS NC
            			   ,CASE
            					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal
            					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
            					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
            					WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal
            					WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
            					ELSE 0
            				END AS NCSemPeso
            			   ,CASE
            
            					WHEN (SELECT
            								COUNT(1)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.AddDate <= @DATAFINAL)
            						> 0 THEN (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.AddDate <= @DATAFINAL
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
            			   ,D.Name AS DepartamentoName
            			   ,D.Id AS Departamento_Id
            			FROM ConsolidationLevel1 CL1 (NOLOCK)
            			INNER JOIN ParLevel1 IND (NOLOCK)
            				ON IND.Id = CL1.ParLevel1_Id
                            AND IND.ID != 43
            			INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
            				ON CL2.ConsolidationLevel1_id = CL1.Id
            			INNER JOIN ParLevel2 L2 WITH (NOLOCK)
            				ON CL2.ParLevel2_id = L2.Id
            			INNER JOIN ParDepartment D WITH (NOLOCK)
            				ON L2.ParDepartment_Id = D.Id
            			INNER JOIN ParCompany UNI (NOLOCK)
            				ON UNI.Id = CL1.UnitId
            			LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
            				ON A4.UNIDADE = UNI.Id
            				AND A4.INDICADOR = IND.ID
            			WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
            			AND UNI.Name = '" + form.unitName + @"'
                        " + whereDepartment + @"
                        " + whereShift + @"
                        " + whereCriticalLevel + @"
            		--AND D.Id = 2
            		) S1
            		GROUP BY Unidade
            				,Unidade_Id
            				 --,Level1Name
            				 --,level1_Id
            				,IsRuleConformity
            				,DepartamentoName
            				,Departamento_Id
                            ,Shift) S2
            	WHERE nc > 0) A
            GROUP BY UnidadeName
            		,Unidade_Id
            		,DepartamentoName
            		,Departamento_Id
                    ,Shift
            ORDER BY 5 DESC
            DROP TABLE #AMOSTRATIPO4 ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }
            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicadorDepartamento")]
        public List<NaoConformidadeResultsSet> GraficoIndicadorDepartamento([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoNcPorUnidadeIndicador();

            //    public string Indicador_Id { get; set; }
            //public string IndicadorName { get; set; }
            //public string Unidade_Id { get; set; }
            //public string UnidadeName { get; set; }
            //public string Monitoramento_Id { get; set; }
            //public string MonitoramentoName { get; set; }
            //public string Tarefa_Id { get; set; }
            //public string TarefaName { get; set; }
            //public decimal Nc { get; set; }
            //public decimal Av { get; set; }
            //public decimal Meta { get; set; }
            //public decimal Proc { get; internal set; }

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'                                                                                                                                                                                                                    " +

                "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.unitName + "'" +

                "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                "\n UNIDADE INT NULL, " +
                "\n INDICADOR INT NULL, " +
                "\n DATA DATETIME, " +
                "\n AM INT NULL, " +
                "\n DEF_AM INT NULL " +
                "\n ) " +

                "\n INSERT INTO #AMOSTRATIPO4 " +

                "\n SELECT " +
                "\n  UNIDADE, INDICADOR, DATA, " +
                "\n COUNT(1) AM " +
                "\n ,SUM(DEF_AM) DEF_AM " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n     cast(C2.CollectionDate as DATE) AS DATA " +
                "\n     , C.Id AS UNIDADE " +
                "\n     , C2.ParLevel1_Id AS INDICADOR " +
                "\n     , C2.EvaluationNumber AS AV " +
                "\n     , C2.Sample AS AM " +
                "\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                "\n     FROM CollectionLevel2 C2  (nolock)" +
                "\n     INNER JOIN ParLevel1 L1  (nolock)" +
                "\n     ON L1.Id = C2.ParLevel1_Id " +

                "\n     INNER JOIN ParCompany C  (nolock)" +
                "\n     ON C.Id = C2.UnitId " +
                "\n     where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n     and C2.NotEvaluatedIs = 0 " +
                "\n     and C2.Duplicated = 0 " +
                "\n     and L1.ParConsolidationType_Id = 4 " +
                "\n     group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) " +
                "\n ) TAB " +
                "\n GROUP BY UNIDADE, INDICADOR, DATA " +

                "\n --------------------------------                                                                                                                     " +
                "\n                                                                                                                                                      " +
                "\n  SELECT TOP 1 @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  DECLARE @NAPCC INT                                                                                                                                  " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  SELECT                                                                                                                                              " +
                "\n         @NAPCC =                                                                                                                                     " +
                "\n           COUNT(1)                                                                                                                                   " +
                "\n           FROM                                                                                                                                       " +
                "\n      (                                                                                                                                               " +
                "\n               SELECT                                                                                                                                 " +
                "\n               COUNT(1) AS NA                                                                                                                         " +
                "\n               FROM CollectionLevel2 C2(nolock)                                                                                                       " +
                "\n               LEFT JOIN Result_Level3 C3(nolock)                                                                                                     " +
                "\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                      " +
                "\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                             " +
                "\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                               " +
                "\n               AND C2.UnitId = @ParCompany_Id                                                                                                         " +
                "\n               AND IsNotEvaluate = 1                                                                                                                  " +
                "\n               GROUP BY C2.ID                                                                                                                         " +
                "\n           ) NA                                                                                                                                       " +
                "\n           WHERE NA = 2                                                                                                                               " +
                "\n  --------------------------------                                                                                                                    " +



                "\n SELECT " +

                "\n  CONVERT(varchar(153), Unidade) as UnidadeName" +
                "\n ,CONVERT(varchar(153), Unidade_Id) as Unidade_Id" +
                "\n ,CONVERT(varchar(153), level1_Id) as Indicador_Id" +
                "\n ,CONVERT(varchar(153), Level1Name) as IndicadorName" +


                "\n ,ProcentagemNc as [proc] " +

               "\n ,IIF(IsRuleConformity = 1, (100 - META), Meta) AS Meta  " +
               "\n ,NC " +
               "\n ,Av " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n       Unidade  " +
                "\n     , IsRuleConformity " +
                "\n     , Unidade_Id " +
                "\n     , Level1Name " +
                "\n     , level1_Id " +

                "\n     , sum(avSemPeso) as av " +
                "\n     , sum(ncSemPeso) as nc " +
                "\n     , IIF(sum(AV) IS NULL OR sum(AV) = 0, 0, sum(NC) / sum(AV) * 100) AS ProcentagemNc " +
                "\n     , max(Meta) as Meta" +

                "\n     FROM " +
                "\n     ( " +
                "\n         SELECT " +
                "\n          IND.Id         AS level1_Id " +
                "\n         , IND.IsRuleConformity " +
                "\n         , IND.Name       AS Level1Name " +
                "\n         , UNI.Id         AS Unidade_Id " +
                "\n         , UNI.Name       AS Unidade " +
                "\n         , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation " +


                "\n         ELSE 0 " +
                "\n        END AS Av " +

                "\n       , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal " +
                "\n         ELSE 0 " +
                "\n        END AS AvSemPeso " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NC " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NCSemPeso " +
               "\n  ,                                                                                                                                                                                                                                                                  " +
               "\n  CASE                                                                                                                                                                                                                                                               " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     ELSE                                                                                                                                                                                                                                                            " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      " +
               "\n  END                                                                                                                                                                                                                                                                " +
               "\n  AS Meta                                                                                                                                                                                                                                                            " +
                "\n         FROM ConsolidationLevel1 CL1  (nolock)" +
                "\n         INNER JOIN ParLevel1 IND  (nolock)" +
                "\n            ON IND.Id = CL1.ParLevel1_Id  "+
                "\n            AND ISNULL(IND.ShowScorecard,1) = 1 " +
                "\n            AND IND.IsActive = 1 " +
                "\n            AND IND.ID != 43 " +
                "\n         INNER JOIN ConsolidationLevel2 CL2 with (nolock) " +
                "\n         ON CL2.ConsolidationLevel1_id = CL1.Id " +
                "\n         INNER JOIN ParLevel2 L2 with (nolock) " +
                "\n         ON CL2.ParLevel2_id = L2.Id " +
                "\n         INNER JOIN ParDepartment D with (nolock) " +
                "\n         ON L2.ParDepartment_Id = D.Id " +


                "\n         INNER JOIN ParCompany UNI  (nolock)" +
                "\n         ON UNI.Id = CL1.UnitId " +
                "\n         LEFT JOIN #AMOSTRATIPO4 A4  (nolock)" +
                "\n         ON A4.UNIDADE = UNI.Id " +
                "\n         AND A4.INDICADOR = IND.ID " +
                "\n         AND A4.DATA = CL1.ConsolidationDate " +

                "\n         WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n         AND UNI.Name = '" + form.unitName + "'" +
                "\n         -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) " +

                "\n         AND D.Id = 2 " +

                "\n     ) S1 " +

                "\n     GROUP BY Unidade, Unidade_Id, Level1Name, level1_Id, IsRuleConformity  " +

                "\n ) S2 " +
                "\n WHERE ProcentagemNc <> 0  " +
                "\n ORDER BY 5 DESC" +

                "\n  DROP TABLE #AMOSTRATIPO4 ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicador")]
        public List<NaoConformidadeResultsSet> GraficoIndicador([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoNcPorUnidadeIndicador();

            //    public string Indicador_Id { get; set; }
            //public string IndicadorName { get; set; }
            //public string Unidade_Id { get; set; }
            //public string UnidadeName { get; set; }
            //public string Monitoramento_Id { get; set; }
            //public string MonitoramentoName { get; set; }
            //public string Tarefa_Id { get; set; }
            //public string TarefaName { get; set; }
            //public decimal Nc { get; set; }
            //public decimal Av { get; set; }
            //public decimal Meta { get; set; }
            //public decimal Proc { get; internal set; }

            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";
            var whereModule = "";

            if (form.moduleId != 0)
            {
                whereModule = $@"AND PC.Id in (SELECT ParCluster_Id FROM ParClusterXModule WHERE isActive = 1 AND ParModule_Id = " + form.moduleId + ") ";
            }


            if (form.departmentId != 0)
            {
                whereDepartment = $@"AND L2.ParDepartment_Id = { form.departmentId } ";
            }

            if (form.departmentName != "" && form.departmentName != null)
            {
                whereDepartment = $@"AND D.Name = '{ form.departmentName }'";
            }

            if (form.shift != 0)
            {
                whereShift = $@"AND CL1.Shift = { form.shift } ";
            }

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var query = $@"
 DECLARE @DATAINICIAL DATETIME = '{ form._dataInicioSQL } 00:00:00'
                                                                                                                                                                                                                    
 DECLARE @DATAFINAL   DATETIME = '{ form._dataFimSQL } 23:59:59'
                                                                                                                                                                                                                    
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
WHERE NAME = '{ form.unitName }'
 CREATE TABLE #AMOSTRATIPO4 ( 
 UNIDADE INT NULL, 
 INDICADOR INT NULL,
 DATA DATETIME,
 AM INT NULL, 
 DEF_AM INT NULL 
 )
INSERT INTO #AMOSTRATIPO4
	SELECT
		UNIDADE
	   ,INDICADOR
       ,DATA
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
		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATAINICIAL AND @DATAFINAL
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
            ,DATA
--------------------------------                                                                                                                     
                                                                                                                                                      
                                                                                                                                                      
  DECLARE @NAPCC INT


SELECT
	@NAPCC =
	COUNT(1)
FROM (SELECT
		COUNT(1) AS NA
	FROM CollectionLevel2 C2 (NOLOCK)
	LEFT JOIN Result_Level3 C3 (NOLOCK)
		ON C3.CollectionLevel2_Id = C2.Id
	WHERE C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
	AND C2.ParLevel1_Id = (SELECT TOP 1
			id
		FROM Parlevel1
		WHERE Hashkey = 1)
	AND C2.UnitId = @ParCompany_Id
	AND IsNotEvaluate = 1
	GROUP BY C2.ID) NA
WHERE NA = 2
--------------------------------                                                                                                                    
SELECT
	CONVERT(VARCHAR(153), Unidade) AS UnidadeName
   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
   ,CONVERT(VARCHAR(153), level1_Id) AS Indicador_Id
   ,CONVERT(VARCHAR(153), Level1Name) AS IndicadorName
   ,ProcentagemNc AS [proc]
   ,IIF(IsRuleConformity = 1, (100 - META),Meta) AS Meta
   ,NC
   ,Av
FROM (SELECT
		Unidade
	   ,IsRuleConformity
	   ,Unidade_Id
	   ,Level1Name
	   ,level1_Id
	   ,SUM(avSemPeso) AS av
	   ,SUM(ncSemPeso) AS nc
	   ,IIF(SUM(AV) IS NULL OR SUM(AV) = 0, 0 ,SUM(NC) / SUM(AV) * 100) AS ProcentagemNc
	   ,MAX(Meta) AS Meta
	FROM (SELECT
			IND.Id AS level1_Id
		   ,IND.IsRuleConformity
		   ,IND.Name AS Level1Name
		   ,UNI.Id AS Unidade_Id
		   ,UNI.Name AS Unidade
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT TOP 1 SUM(Quartos)
															FROM VolumePcc1b(nolock)
															WHERE ParCompany_id = @ParCompany_id
															AND Data  = CL2.ConsolidationDate) - isnull(@NAPCC,0)
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.EvaluatedResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.WeiEvaluation)
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT TOP 1 SUM(Quartos)
															FROM VolumePcc1b(nolock)
															WHERE ParCompany_id = @ParCompany_id
															AND Data  = CL2.ConsolidationDate) - isnull(@NAPCC,0)
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.EvaluateTotal)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.EvaluatedResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.EvaluateTotal)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.EvaluateTotal)
				ELSE 0
			END AS AvSemPeso
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.DefectsResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.DEF_AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.TotalLevel3WithDefects)
				ELSE 0
			END AS NC
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.DefectsTotal)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.DefectsTotal)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.DefectsResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.DEF_AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.DefectsTotal)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.TotalLevel3WithDefects)
				ELSE 0
			END AS NCSemPeso
		   ,CASE

				WHEN (SELECT
							COUNT(1)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.AddDate <= CL2.ConsolidationDate)
					> 0 THEN (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.AddDate <= CL2.ConsolidationDate
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
              AND ISNULL(IND.ShowScorecard,1) = 1 
              AND IND.IsActive = 1 
              AND IND.ID != 43 
        INNER JOIN ParCompany UNI (NOLOCK)
			ON UNI.Id = CL1.UnitId
		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
			ON A4.UNIDADE = UNI.Id
			AND A4.INDICADOR = IND.ID
            AND A4.DATA = CL1.ConsolidationDate
		INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
			ON CL2.ConsolidationLevel1_id = CL1.Id
		INNER JOIN ParLevel2 L2 WITH (NOLOCK)
			ON CL2.ParLevel2_id = L2.Id
		INNER JOIN ParDepartment D WITH (NOLOCK)
			ON L2.ParDepartment_Id = D.Id
		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		AND UNI.Name = '{form.unitName }'
        {whereDepartment}
        {whereShift}
        {whereCriticalLevel}
        -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) 
		GROUP BY IND.ParConsolidationType_Id
				,IND.HashKey
				,IND.Id
				,IND.IsRuleConformity
				,IND.Name
				,UNI.Id
				,UNI.Name
				,CL1.ParLevel1_Id
				,CL1.UnitId
                ,CL2.ConsolidationDate) S1
	GROUP BY Unidade
			,Unidade_Id
			,Level1Name
			,level1_Id
			,IsRuleConformity) S2
 WHERE ProcentagemNc <> 0 
ORDER BY 5 DESC
DROP TABLE #AMOSTRATIPO4 ";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicadorPorShift")]
        public List<NaoConformidadeResultsSet> GraficoIndicadorPorShift([FromBody] FormularioParaRelatorioViewModel form)
        {
            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";
            var whereModule = "";

            if (form.moduleId != 0)
            {
                whereModule = $@"AND PC.Id in (SELECT ParCluster_Id FROM ParClusterXModule WHERE isActive = 1 AND ParModule_Id = " + form.moduleId + ") ";
            }


            if (form.departmentId != 0)
            {
                whereDepartment = $@"AND L2.ParDepartment_Id = { form.departmentId } ";
            }

            if (form.departmentName != "" && form.departmentName != null)
            {
                whereDepartment = $@"AND D.Name = '{ form.departmentName }'";
            }

            if (form.shift != 0)
            {
                whereShift = $@"AND CL1.Shift = { form.shift } ";
            }

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var query = $@"
 DECLARE @DATAINICIAL DATETIME = '{ form._dataInicioSQL }'
                                                                                                                                                                                                                    
 DECLARE @DATAFINAL   DATETIME = '{ form._dataFimSQL }'
                                                                                                                                                                                                                    
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
WHERE NAME = '{ form.unitName }'
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
       ,DATA
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
		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATAINICIAL AND @DATAFINAL
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
            ,DATA
--------------------------------                                                                                                                     

SELECT TOP 1
	@VOLUMEPCC = SUM(Quartos)
FROM VolumePcc1b(nolock)
WHERE ParCompany_id = @ParCompany_id
AND Data BETWEEN @DATAINICIAL AND @DATAFINAL
 
                                                                                                                                                      
                                                                                                                                                      
  DECLARE @NAPCC INT


SELECT
	@NAPCC =
	COUNT(1)
FROM (SELECT
		COUNT(1) AS NA
	FROM CollectionLevel2 C2 (NOLOCK)
	LEFT JOIN Result_Level3 C3 (NOLOCK)
		ON C3.CollectionLevel2_Id = C2.Id
	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
	AND C2.ParLevel1_Id = (SELECT TOP 1
			id
		FROM Parlevel1
		WHERE Hashkey = 1)
	AND C2.UnitId = @ParCompany_Id
	AND IsNotEvaluate = 1
	GROUP BY C2.ID) NA
WHERE NA = 2
--------------------------------                                                                                                                    
SELECT
	CONVERT(VARCHAR(153), Unidade) AS UnidadeName
   ,CONVERT(VARCHAR(153), Unidade_Id) AS Unidade_Id
   ,CONVERT(VARCHAR(153), level1_Id) AS Indicador_Id
   ,CONVERT(VARCHAR(153), Level1Name) AS IndicadorName
   ,ProcentagemNc AS [proc]
   ,(CASE
		WHEN IsRuleConformity = 1 THEN (100 - META)
		ELSE Meta
	END) AS Meta
   ,NC
   ,Av
   ,Shift
   ,CONCAT(Level1Name, ' - Shift ', Case when S2.Shift = 1 then 'A' else 'B' END) as 'dataX'
FROM (SELECT
		Unidade
	   ,IsRuleConformity
	   ,Unidade_Id
	   ,Level1Name
	   ,level1_Id
	   ,SUM(avSemPeso) AS av
	   ,SUM(ncSemPeso) AS nc
	   ,CASE
			WHEN SUM(AV) IS NULL OR
				SUM(AV) = 0 THEN 0
			ELSE SUM(NC) / SUM(AV) * 100
		END AS ProcentagemNc
	   ,MAX(Meta) AS Meta
	   ,Shift
	FROM (SELECT
			IND.Id AS level1_Id
		   ,IND.IsRuleConformity
		   ,IND.Name AS Level1Name
		   ,UNI.Id AS Unidade_Id
		   ,UNI.Name AS Unidade
		   ,CL1.Shift AS Shift
		   ,CASE
				WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.EvaluatedResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.WeiEvaluation)
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.EvaluateTotal)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiEvaluation)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.EvaluatedResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.EvaluateTotal)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.EvaluateTotal)
				ELSE 0
			END AS AvSemPeso
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.DefectsResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.DEF_AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.WeiDefects)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.TotalLevel3WithDefects)
				ELSE 0
			END AS NC
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN SUM(CL2.DefectsTotal)
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.DefectsTotal)
				WHEN IND.ParConsolidationType_Id = 3 THEN SUM(CL2.DefectsResult)
				WHEN IND.ParConsolidationType_Id = 4 THEN SUM(A4.DEF_AM)
				WHEN IND.ParConsolidationType_Id = 5 THEN SUM(CL2.DefectsTotal)
				WHEN IND.ParConsolidationType_Id = 6 THEN SUM(CL2.TotalLevel3WithDefects)
				ELSE 0
			END AS NCSemPeso
		   ,CASE

				WHEN (SELECT
							COUNT(1)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.AddDate <= @DATAFINAL)
					> 0 THEN (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.AddDate <= @DATAFINAL
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
		INNER JOIN ParCompany UNI (NOLOCK)
			ON UNI.Id = CL1.UnitId
		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
			ON A4.UNIDADE = UNI.Id
			AND A4.INDICADOR = IND.ID
			AND A4.DATA = CL1.ConsolidationDate
		INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK)
			ON CL2.ConsolidationLevel1_id = CL1.Id
		INNER JOIN ParLevel2 L2 WITH (NOLOCK)
			ON CL2.ParLevel2_id = L2.Id
		INNER JOIN ParDepartment D WITH (NOLOCK)
			ON L2.ParDepartment_Id = D.Id
		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		AND UNI.Name = '{form.unitName }'
        {whereDepartment}
        {whereShift}
        {whereCriticalLevel}
        -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) 
		GROUP BY IND.ParConsolidationType_Id
				,IND.HashKey
				,IND.Id
				,IND.IsRuleConformity
				,IND.Name
				,UNI.Id
				,UNI.Name
				,CL1.ParLevel1_Id
				,CL1.UnitId
                ,cl1.Shift) S1
	GROUP BY Unidade
			,Unidade_Id
			,Level1Name
			,level1_Id
			,IsRuleConformity
            ,Shift) S2
WHERE nc > 0
ORDER BY 5 DESC
DROP TABLE #AMOSTRATIPO4 ";



            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoMonitoramento")]
        public List<NaoConformidadeResultsSet> GraficoMonitoramento([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoMonitoramento();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            var whereDepartment = "";
            var whereShift = "";
            var whereModule = "";

            if (form.moduleId != 0)
            {
                whereModule = $@"AND PC.Id in (SELECT ParCluster_Id FROM ParClusterXModule WHERE isActive = 1 AND ParModule_Id = " + form.moduleId + ") ";
            }

            if (form.departmentId != 0)
            {
                whereDepartment = "\n AND MON.ParDepartment_Id = " + form.departmentId + " ";
            }

            if (form.departmentName != "" && form.departmentName != null)
            {
                whereDepartment = "\n AND D.Name = '" + form.departmentName + "'";
            }

            if (form.shift != 0)
            {
                whereShift = "\n AND CL1.Shift = " + form.shift + " ";
            }

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + " 00:00:00'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + " 23:59:59'       " +

                "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.unitName + "'" +

                "\n --------------------------------                                                                                                                     " +
                "\n                                                                                                                                                      " +
                "\n  SELECT TOP 1 @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  DECLARE @NAPCC INT                                                                                                                                  " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  SELECT                                                                                                                                              " +
                "\n         @NAPCC =                                                                                                                                     " +
                "\n           COUNT(1)                                                                                                                                   " +
                "\n           FROM                                                                                                                                       " +
                "\n      (                                                                                                                                               " +
                "\n               SELECT                                                                                                                                 " +
                "\n               COUNT(1) AS NA                                                                                                                         " +
                "\n               FROM CollectionLevel2 C2(nolock)                                                                                                       " +
                "\n               LEFT JOIN Result_Level3 C3(nolock)                                                                                                     " +
                "\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                      " +
                "\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                             " +
                "\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                               " +
                "\n               AND C2.UnitId = @ParCompany_Id                                                                                                         " +
                "\n               AND IsNotEvaluate = 1                                                                                                                  " +
                "\n               GROUP BY C2.ID                                                                                                                         " +
                "\n           ) NA                                                                                                                                       " +
                "\n           WHERE NA = 2                                                                                                                               " +
                "\n  --------------------------------                                                                                                                    " +

               "\n SELECT " +
               "\n  " +
               "\n  --level1_Id " +
               "\n  --,Level1Name " +
               "\n  --,level2_Id " +
               "\n Level2Name AS MonitoramentoName " +
               "\n --,Unidade_Id " +
               "\n --,Unidade " +

               "\n     , sum(avSemPeso) as av " +
                "\n     , sum(ncSemPeso) as nc " +
                "\n     , CASE WHEN sum(AV) IS NULL OR sum(AV) = 0 THEN 0 ELSE sum(NC) / sum(AV) * 100 END AS [Proc] " +

               "\n FROM " +
               "\n ( " +
               "\n 	SELECT " +
               "\n 	 MON.Id			AS level2_Id " +
               "\n 	,MON.Name		AS Level2Name " +
               "\n 	,IND.Id AS level1_Id " +
               "\n 	,IND.Name AS Level1Name " +
               "\n 	,UNI.Id			AS Unidade_Id " +
               "\n 	,UNI.Name		AS Unidade " +


                               "\n         , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC/2 - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id in (3,4) THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation " +
                "\n         ELSE 0 " +
                "\n        END AS Av " +

                "\n       , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC/2 - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id in (3,4) THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal " +
                "\n         ELSE 0 " +
                "\n        END AS AvSemPeso " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id in (3,4) THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NC " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id in (3,4) THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +
                "\n         END AS NCSemPeso " +

               "\n 	FROM ConsolidationLevel2 CL2  (nolock)" +
               "\n 	INNER JOIN ConsolidationLevel1 CL1  (nolock)" +
               "\n 	ON CL1.Id = CL2.ConsolidationLevel1_Id " +
               "\n 	INNER JOIN ParLevel1 IND  (nolock)" +
               "\n 	ON IND.Id = CL1.ParLevel1_Id " +
               "\n 	AND ISNULL(IND.ShowScorecard,1) = 1 " +
               "\n 	AND IND.IsActive = 1 " +
               "\n 	INNER JOIN ParLevel2 MON  (nolock)" +
               "\n 	ON MON.Id = CL2.ParLevel2_Id " +
               "\n 	INNER JOIN ParCompany UNI (nolock) " +
               "\n 	ON UNI.Id = CL1.UnitId " +
               "\n  INNER JOIN ParDepartment D WITH (NOLOCK) " +
               "\n  ON MON.ParDepartment_Id = D.Id " +
               "\n 	WHERE CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL" +
               whereDepartment +
               whereShift +
               "\n 	AND (UNI.Name = '" + form.unitName + "' OR UNI.Initials = '" + form.unitName + "')" +
               "\n 	AND IND.Name = '" + form.level1Name + "' " + //

               "\n ) S1 " +
            "\n  GROUP BY Level2Name " +
            "\n   HAVING sum(NC) <> 0 " +
            "\n  ORDER BY 4 DESC ";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefasAcumuladas")]
        public List<NaoConformidadeResultsSet> GraficoTarefasAcumuladas([FromBody] FormularioParaRelatorioViewModel form)
        {

            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";
            var whereModule = "";

            if (form.moduleId != 0)
            {
                whereModule = $@"AND PC.Id in (SELECT ParCluster_Id FROM ParClusterXModule WHERE isActive = 1 AND ParModule_Id = " + form.moduleId + ") ";
            }

            if (form.departmentId != 0)
            {
                whereDepartment = "\n AND MON.ParDepartment_Id = " + form.departmentId + " ";
            }

            if (form.departmentName != "" && form.departmentName != null)
            {
                whereDepartment = "\n AND D.Name = '" + form.departmentName + "'";
            }

            if (form.shift != 0)
            {
                whereShift = "\n AND CL1.Shift = " + form.shift + " ";
            }
            
            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var queryGraficoTarefasAcumuladas = $@"
            SELECT
            
            	IND.Id AS Indicador_id
               ,IND.Name AS IndicadorName
               ,IND.Id AS Monitoramento_Id
               ,IND.Name AS MonitoramentoName
               ,R3.ParLevel3_Id AS Tarefa_Id
               ,R3.ParLevel3_Name AS TarefaName
               ,UNI.Name AS UnidadeName
               ,UNI.Id AS Unidade_Id
               ,SUM(R3.Defects) AS NC
            FROM Result_Level3 R3 (NOLOCK)
            INNER JOIN CollectionLevel2 C2 (NOLOCK)
            	ON C2.Id = R3.CollectionLevel2_Id
            INNER JOIN ConsolidationLevel2 CL2 (NOLOCK)
            	ON CL2.Id = C2.ConsolidationLevel2_Id
            INNER JOIN ConsolidationLevel1 CL1 (NOLOCK)
            	ON CL1.Id = CL2.ConsolidationLevel1_Id
            INNER JOIN ParCompany UNI (NOLOCK)
            	ON UNI.Id = CL1.UnitId
            INNER JOIN ParLevel1 IND (NOLOCK)
            	ON IND.Id = CL1.ParLevel1_Id
            INNER JOIN ParLevel2 MON (NOLOCK)
            	ON MON.Id = CL2.ParLevel2_Id
            WHERE 1 = 1 
             AND IND.Name IN ('{ form.level1Name }') 
            /* and MON.Id = 1 */
            AND UNI.Name = '{ form.unitName }'
            AND CL2.ConsolidationDate BETWEEN '{ form._dataInicioSQL }' AND '{ form._dataFimSQL }'
                { whereDepartment }
                { whereShift }            
                { whereCriticalLevel }
            GROUP BY IND.Id
            		,IND.Name
            		,R3.ParLevel3_Id
            		,R3.ParLevel3_Name
            		,UNI.Name
            		,UNI.Id
            HAVING SUM(R3.WeiDefects) > 0
            AND SUM(R3.Defects) > 0
            ORDER BY 9 DESC";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(queryGraficoTarefasAcumuladas).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefa")]
        public List<NaoConformidadeResultsSet> GraficoTarefa([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoTarefas();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //Av = av + i,
            //Nc = nc + i,
            //Proc = proc + i,
            //TarefaName = tarefaName + i.ToString()

            var whereShift = "";
            var whereModule = "";

            if (form.moduleId != 0)
            {
                whereModule = $@"AND PC.Id in (SELECT ParCluster_Id FROM ParClusterXModule WHERE isActive = 1 AND ParModule_Id = " + form.moduleId + ") ";
            }

            if (form.shift != 0)
            {
                whereShift = "\n AND CL1.Shift = " + form.shift + " ";
            }

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +

                 "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.unitName + "'" +

                "\n --------------------------------                                                                                                                     " +
                "\n                                                                                                                                                      " +
                "\n  SELECT TOP 1 @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  DECLARE @NAPCC INT                                                                                                                                  " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  SELECT                                                                                                                                              " +
                "\n         @NAPCC =                                                                                                                                     " +
                "\n           COUNT(1)                                                                                                                                   " +
                "\n           FROM                                                                                                                                       " +
                "\n      (                                                                                                                                               " +
                "\n               SELECT                                                                                                                                 " +
                "\n               COUNT(1) AS NA                                                                                                                         " +
                "\n               FROM CollectionLevel2 C2(nolock)                                                                                                       " +
                "\n               LEFT JOIN Result_Level3 C3(nolock)                                                                                                     " +
                "\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                      " +
                "\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                             " +
                "\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                               " +
                "\n               AND C2.UnitId = @ParCompany_Id                                                                                                         " +
                "\n               AND IsNotEvaluate = 1                                                                                                                  " +
                "\n               GROUP BY C2.ID                                                                                                                         " +
                "\n           ) NA                                                                                                                                       " +
                "\n           WHERE NA = 2                                                                                                                               " +
                "\n  --------------------------------                                                                                                                    " +

                "\n SELECT " +
                "\n TarefaName, NcSemPeso as Nc, AvSemPeso as Av, [Proc] FROM (" +
                "\n SELECT " +
                         "\n  " +
                         //     "\n  IND.Id AS level1_Id " +
                         //     "\n ,IND.Name AS Level1Name " +
                         //     "\n ,IND.Id AS level2_Id " +
                         //     "\n ,IND.Name AS Level2Name " +
                         //     "\n ,R3.ParLevel3_Id AS level3_Id " +
                         "\n R3.ParLevel3_Name AS TarefaName " +
                         //     "\n ,UNI.Name AS Unidade " +
                         //     "\n ,UNI.Id AS Unidade_Id " +
                         "\n ,SUM(R3.WeiDefects) AS Nc " +
                         "\n ,CASE WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiDefects) ELSE SUM(R3.Defects) END AS NcSemPeso  " +
                         "\n ,CASE " +
                         "\n WHEN IND.HashKey = 1 THEN @VOLUMEPCC/2 - @NAPCC " +
                         "\n ELSE SUM(R3.WeiEvaluation) END AS Av " +
                         "\n ,CASE " +
                         "\n WHEN IND.HashKey = 1 THEN @VOLUMEPCC/2 - @NAPCC " +
                         "\n WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiEvaluation) " +
                         "\n ELSE SUM(R3.Evaluation) END AS AvSemPeso " +
                         "\n ,SUM(R3.WeiDefects) / " +
                         "\n CASE " +
                         "\n WHEN IND.HashKey = 1 THEN(SELECT TOP 1 SUM(Quartos) / 2 FROM VolumePcc1b (nolock) WHERE ParCompany_id = UNI.Id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL) " +
                         "\n ELSE SUM(R3.WeiEvaluation) END * 100 AS [Proc] " +
                         "\n FROM Result_Level3 R3 (nolock) " +
                         "\n INNER JOIN CollectionLevel2 C2  (nolock)" +
                         "\n ON C2.Id = R3.CollectionLevel2_Id " +
                         "\n INNER JOIN ConsolidationLevel2 CL2  (nolock)" +
                         "\n ON CL2.Id = C2.ConsolidationLevel2_Id " +
                         "\n INNER JOIN ConsolidationLevel1 CL1  (nolock)" +
                         "\n ON CL1.Id = CL2.ConsolidationLevel1_Id " +
                         "\n INNER JOIN ParCompany UNI  (nolock)" +
                         "\n ON UNI.Id = C2.UnitId " +
                         "\n INNER JOIN ParLevel1 IND   (nolock)" +
                         "\n ON IND.Id = C2.ParLevel1_Id " +
                         "\n INNER JOIN ParLevel2 MON  (nolock)" +
                         "\n ON MON.Id = C2.ParLevel2_Id " +
                         "\n WHERE IND.Name = '" + form.level1Name + "' " +
                         "\n    and MON.Name = '" + form.level2Name + "' " +
                         "\n 	AND (UNI.Name = '" + form.unitName + "' OR UNI.Initials = '" + form.unitName + "')" +
                         "\n    AND R3.IsNotEvaluate = 0 " +
                         "\n 	AND CL2.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                         whereShift +
                         "\n GROUP BY " +
                         "\n  IND.Id " +
                         "\n ,IND.Name " +
                         "\n ,R3.ParLevel3_Id " +
                         "\n ,R3.ParLevel3_Name " +
                         "\n ,UNI.Name " +
                         "\n ,UNI.Id " +
                          "\n ,ind.hashKey " +
                          "\n ,ind.ParConsolidationType_Id " +

                         "\n HAVING SUM(R3.WeiDefects) > 0" +
                         "\n ) TAB ORDER BY 4 DESC";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefasAcumulada")]
        public List<NaoConformidadeResultsSet> GraficoTarefasAcumulada([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoTarefasAcumuladas();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            var query = "SELECT " +
                        "\n  " +
                        //     "\n  IND.Id AS level1_Id " +
                        //     "\n ,IND.Name AS Level1Name " +
                        //     "\n ,IND.Id AS level2_Id " +
                        //     "\n ,IND.Name AS Level2Name " +
                        //     "\n ,R3.ParLevel3_Id AS level3_Id " +
                        "\n R3.ParLevel3_Name AS TarefaName " +
                        //     "\n ,UNI.Name AS Unidade " +
                        //     "\n ,UNI.Id AS Unidade_Id " +
                        "\n ,SUM(R3.WeiDefects) AS Nc " +
                        "\n ,SUM(R3.WeiEvaluation) AS Av " +
                        "\n ,SUM(R3.WeiDefects) / SUM(R3.WeiEvaluation) * 100 AS [Proc] " +
                        "\n FROM Result_Level3 R3  (nolock)" +
                        "\n INNER JOIN CollectionLevel2 C2  (nolock)" +
                        "\n ON C2.Id = R3.CollectionLevel2_Id " +
                        "\n INNER JOIN ConsolidationLevel2 CL2  (nolock)" +
                        "\n ON CL2.Id = C2.ConsolidationLevel2_Id " +
                        "\n INNER JOIN ConsolidationLevel1 CL1  (nolock)" +
                        "\n ON CL1.Id = CL2.ConsolidationLevel1_Id " +
                        "\n INNER JOIN ParCompany UNI  (nolock)" +
                        "\n ON UNI.Id = CL1.UnitId " +
                        "\n INNER JOIN ParLevel1 IND   (nolock)" +
                        "\n ON IND.Id = CL1.ParLevel1_Id " +
                        "\n INNER JOIN ParLevel2 MON  (nolock)" +
                        "\n ON MON.Id = CL2.ParLevel2_Id " +
                        "\n WHERE IND.Name ='" + form.level1Name + "' " +
                        "\n /* and MON.Id = 1 */" +
                        "\n 	AND UNI.Name = '" + form.unitName + "'" +
                        "\n 	AND CL2.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                        "\n GROUP BY " +
                        "\n  IND.Id " +
                        "\n ,IND.Name " +
                        "\n ,R3.ParLevel3_Id " +
                        "\n ,R3.ParLevel3_Name " +
                        "\n ,UNI.Name " +
                        "\n ,UNI.Id " +
                        "\n HAVING (SUM(R3.WeiDefects) / SUM(R3.WeiEvaluation) * 100) <> 0" +
                        "\n ORDER BY 4 DESC";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoUnidades()
        {

            #region Props
            var nc = 10;
            var av = 10;
            var proc = 20;
            var unidade = "Unidade";
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 30; i++)
            {
                list.Add(new NaoConformidadeResultsSet() { Av = av + i, Nc = nc + i, Proc = proc + i, UnidadeName = unidade + i.ToString() });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoNcPorUnidadeIndicador()
        {
            #region Props
            var nc = 10;
            var av = 10;
            var proc = 20;
            var Meta = 2;
            var indicadorName = "Indicador1";
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    Meta = Meta + i - 5,
                    IndicadorName = indicadorName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoMonitoramento()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var monitoramento = "Monitoramento";
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    MonitoramentoName = monitoramento + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoTarefas()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var tarefaName = "Tarefa";
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 90; i++)
            {
                list.Add(new NaoConformidadeResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    TarefaName = tarefaName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoTarefasAcumuladas()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var tarefaName = "TarefaAcumulada";
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 90; i++)
            {
                list.Add(new NaoConformidadeResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    TarefaName = tarefaName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        private string GetUserUnits(int User)
        {
            using (var db = new SgqDbDevEntities())
            {
                return string.Join(",", db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == User).Select(r => r.ParCompany_Id).ToList());
            }
        }

    }

}


public class NaoConformidadeResultsSet
{

    internal string Select(DateTime _dataInicio, DateTime _dataFim, int unitId)
    {
        return "";
    }

    public string Indicador_Id { get; set; }
    public string IndicadorName { get; set; }
    public string DepartamentoName { get; set; }
    public string Departamento_Id { get; set; }
    public string Unidade_Id { get; set; }
    public string UnidadeName { get; set; }
    public string Monitoramento_Id { get; set; }
    public string MonitoramentoName { get; set; }
    public string Tarefa_Id { get; set; }
    public string TarefaName { get; set; }
    public decimal? Nc { get; set; }
    public decimal? Av { get; set; }
    public decimal? Meta { get; set; }
    public decimal? Proc { get; internal set; }
    public int? Shift { get; set; }
    public string dataX { get; set; }
}
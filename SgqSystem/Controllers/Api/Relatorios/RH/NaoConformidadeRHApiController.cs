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

namespace SgqSystem.Controllers.Api.Relatorios.RH
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/NaoConformidadeRH")]
    public class NaoConformidadeRHApiController : BaseApiController
    {
        private List<NaoConformidadeRHResultsSet> _mock { get; set; }
        private List<NaoConformidadeRHResultsSet> _list { get; set; }

        [HttpPost]
        [Route("GraficoUnidades")]
        public List<NaoConformidadeRHResultsSet> GraficoUnidades([FromBody] DTO.DataCarrierFormularioNew form)
        {

            //CommonLog.SaveReport(form, "Relatorio_Nao_Conformidade");

            var whereDepartment = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            //var whereCluster = "";
            //var whereCriticalLevel = "";
            //var whereClusterGroup = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            //if (form.ParClusterGroup_Ids.Length > 0)
            //{
            //    whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            //}

            //if (form.ParCluster_Ids.Length > 0)
            //{
            //    whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id in (" + string.Join(",", form.ParCluster_Ids) + ") AND PCC.Active = 1)";
            //}

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";
                  
            //}

            var query = $@"
                 DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

                SELECT 
	                C.NAME AS UnidadeName,
	                SUM(WeiEvaluation) AS AV,
	                SUM(WeiDefects) AS NC,
	                SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	                FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	                INNER JOIN ParCompany C WITH (NOLOCK)
		                ON L2.Unitid = C.ID
	                WHERE 1=1
	                AND CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL

                    {whereStructure}
                    {whereUnit}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                GROUP BY 
	                C.NAME

                ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoDepartamentos")]
        public List<NaoConformidadeRHResultsSet> GraficoDepartamentos([FromBody] DTO.DataCarrierFormularioNew form)
        {

            var whereDepartment = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            //var whereCluster = "";
            //var whereCriticalLevel = "";
            //var whereClusterGroup = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            //if (form.ParClusterGroup_Ids.Length > 0)
            //{
            //    whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            //}

            //if (form.ParCluster_Ids.Length > 0)
            //{
            //    whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id in (" + string.Join(",", form.ParCluster_Ids) + ") AND PCC.Active = 1)";
            //}

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}

            var query = $@"
             
                         DECLARE @DATAINICIAL DATETIME = '" + form.startDate.ToString("yyyy-MM-dd") + " 00:00:00" + $@"'
                                                                                                                                                                                                                                            
                         DECLARE @DATAFINAL   DATETIME = '" + form.endDate.ToString("yyyy-MM-dd") + " 23:59:59" + $@"'
                                                                          
            SELECT 
	            D1.NAME AS DepartamentoName,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L2.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L2.Parlevel1_Id = L.ID
	            INNER JOIN ParLevel2 M WITH (NOLOCK)
		            ON L2.Parlevel2_Id = M.ID
				INNER JOIN ParDepartment D
					ON L2.Centro_De_Custo_Id = D.ID
				INNER JOIN ParDepartment D1
					ON L2.Secao_Id = D1.ID
				INNER JOIN ParCargo CG
					ON L2.Cargo_Id = CG.ID
	            WHERE 1=1
                AND L2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                AND C.Name  = '{ form.Param["unitName"] }'
                
                {whereStructure}
                {whereUnit}
                {whereDepartment}
                {whereSecao}
                {whereCargo}
            GROUP BY 
	            D1.NAME
            ORDER BY 4 DESC 
";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoDepartamentosPorShift")]
        public List<NaoConformidadeRHResultsSet> GraficoDepartamentosPorShift([FromBody] DTO.DataCarrierFormularioNew form)
        {

            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = "\n AND L2.ParDepartment_Id in (" + string.Join(",", form.ParDepartment_Ids) + ")";
            }

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartment = "\n AND D.Name = '" + form.Param["departmentName"] + "'";
            }

            if (form.Shift_Ids.Length > 0)
            {
                whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            }

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")";
                //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            }

            var query = @"
             
                         DECLARE @DATAINICIAL DATETIME = '" + form.startDate.ToString("yyyy-MM-dd") + @"'
                                                                                                                                                                                                                                            
                         DECLARE @DATAFINAL   DATETIME = '" + form.endDate.ToString("yyyy-MM-dd") + @"'
                                                                                                                                                                                                                                            
                         DECLARE @VOLUMEPCC int
                                                                          
                         DECLARE @ParCompany_id INT
            SELECT
            	@ParCompany_id = ID
            FROM PARCOMPANY
            WHERE NAME = '" + form.Param["unitName"] + @"'
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
            							AND G.EffectiveDate <= @DATAFINAL)
            						> 0 THEN (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.EffectiveDate <= @DATAFINAL
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            
            					ELSE (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G
            							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
                                        AND G.EffectiveDate <= @DATAFINAL
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
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
                        LEFT JOIN ParLevel1XCluster PLC
							ON PLC.ParLevel1_Id = CL1.ParLevel1_Id 
            			WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
            			AND UNI.Name = '" + form.Param["unitName"] + @"'
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
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }
            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicadorDepartamento")]
        public List<NaoConformidadeRHResultsSet> GraficoIndicadorDepartamento([FromBody] DTO.DataCarrierFormularioNew form)
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

                "\n DECLARE @DATAINICIAL DATETIME = '" + form.startDate.ToString("yyyy-MM-dd") + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form.endDate.ToString("yyyy-MM-dd") + "'                                                                                                                                                                                                                    " +

                "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.Param["unitName"] + "'" +

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
               "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                         " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     ELSE                                                                                                                                                                                                                                                            " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                                                      " +
               "\n  END                                                                                                                                                                                                                                                                " +
               "\n  AS Meta                                                                                                                                                                                                                                                            " +
                "\n         FROM ConsolidationLevel1 CL1  (nolock)" +
                "\n         INNER JOIN ParLevel1 IND  (nolock)" +
                "\n            ON IND.Id = CL1.ParLevel1_Id  " +
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
                "\n         AND UNI.Name = '" + form.Param["unitName"] + "'" +
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
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicador")]
        public List<NaoConformidadeRHResultsSet> GraficoIndicador([FromBody] DTO.DataCarrierFormularioNew form)
        {
            var whereDepartment = "";
            var whereDepartmentFiltro = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            //var whereCluster = "";
            //var whereCriticalLevel = "";
            //var whereClusterGroup = "";

            // Filtro = Gráfico Anterior

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartmentFiltro = $@"AND D.Name = '{form.Param["departmentName"]}'";
            }

            // Filtro = Pré-seleção
            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            //if (form.ParClusterGroup_Ids.Length > 0)
            //{
            //    whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            //}

            //if (form.ParCluster_Ids.Length > 0)
            //{
            //    whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id in (" + string.Join(",", form.ParCluster_Ids) + ") AND PCC.Active = 1)";
            //}

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}

            var query = $@"
             DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd") } 00:00:00'
                                                                                                                                                                                                                    
             DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } 23:59:59'
        


            SELECT 
	            L.NAME AS IndicadorName,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L2.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L2.Parlevel1_Id = L.ID
	            INNER JOIN ParDepartment D WITH (NOLOCK)
		            ON L2.Secao_Id = D.ID
	            WHERE 1=1
	            AND CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
		            AND C.Name = '{form.Param["unitName"] }'
                    

                {whereStructure}
                {whereUnit}
                {whereDepartment}
                {whereDepartmentFiltro}
                {whereSecao}
                {whereCargo}
            GROUP BY 
	            L.NAME
            ORDER BY 4 DESC
 ";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicadorPorShift")]
        public List<NaoConformidadeRHResultsSet> GraficoIndicadorPorShift([FromBody] DTO.DataCarrierFormularioNew form)
        {
            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";
            var whereClusterGroup = "";
            var whereCluster = "";

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@"AND L2.ParDepartment_Id  in (" + string.Join(",", form.ParDepartment_Ids) + ") ";
            }

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartment = $@"AND D.Name = '{ form.Param["departmentName"] }'";
            }

            if (form.Shift_Ids.Length > 0)
            {
                whereShift = $@"AND CL1.Shift  in (" + string.Join(",", form.Shift_Ids) + ") ";
            }

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")";
                //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            }

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id in (" + string.Join(",", form.ParCluster_Ids) + ") AND PCC.Active = 1)";
            }

            var query = $@"
 DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd") }'
                                                                                                                                                                                                                    
 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") }'
                                                                                                                                                                                                                    
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
WHERE NAME = '{ form.Param["unitName"] }'
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
				WHEN IND.ParConsolidationType_Id = 2 THEN SUM(CL2.WeiDefects)
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
						AND G.EffectiveDate <= @DATAFINAL)
					> 0 THEN (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.EffectiveDate <= @DATAFINAL
						ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)

				ELSE (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
                        AND G.EffectiveDate <= @DATAFINAL
						ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
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
        LEFT JOIN ParLevel1XCluster PLC
							ON PLC.ParLevel1_Id = CL1.ParLevel1_Id 
		INNER JOIN ParDepartment D WITH (NOLOCK)
			ON L2.ParDepartment_Id = D.Id
        INNER JOIN ParCompany UNI (NOLOCK)
            ON UNI.Id = CL1.UnitId
            and UNI.IsActive = 1
        LEFT JOIN ParCompanyCluster PCC
		    ON PCC.ParCompany_Id = UNI.Id
            and PCC.Active = 1
        LEFT JOIN ParLevel1XCluster PLC
			ON PLC.ParLevel1_Id = CL1.ParLevel1_Id 
        LEFT JOIN ParCluster PC
		    ON PCC.ParCluster_Id = PC.Id
		LEFT JOIN ParClusterGroup PCG
		    ON PC.ParClusterGroup_Id = PCG.Id
		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		AND UNI.Name = '{form.Param["unitName"] }'
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
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoMonitoramento")]
        public List<NaoConformidadeRHResultsSet> GraficoMonitoramento([FromBody] DTO.DataCarrierFormularioNew form)
        {
            //_list = CriaMockGraficoMonitoramento();

            //var query = new NaoConformidadeRHResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);



            var whereDepartment = "";
            var whereDepartmentFiltro = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            //var whereCluster = "";
            //var whereCriticalLevel = "";
            //var whereClusterGroup = "";

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartmentFiltro = $@"AND D.Name = '{form.Param["departmentName"]}'";
            }

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L2.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L2.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L2.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L2.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            //if (form.ParClusterGroup_Ids.Length > 0)
            //{
            //    whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            //}

            //if (form.ParCluster_Ids.Length > 0)
            //{
            //    whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id in (" + string.Join(",", form.ParCluster_Ids) + ") AND PCC.Active = 1)";
            //}

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L2.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}

            var query = "" +

                $@" DECLARE @DATAINICIAL DATETIME = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00'
                 DECLARE @DATAFINAL   DATETIME = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'       


            SELECT 
	            M.NAME AS MonitoramentoName,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L2 L2 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L2.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L2.Parlevel1_Id = L.ID
	            INNER JOIN ParLevel2 M WITH (NOLOCK)
		            ON L2.Parlevel2_Id = M.ID
	            INNER JOIN ParDepartment D WITH (NOLOCK)
		            ON L2.Secao_Id = D.ID
	            WHERE 1=1
                AND L2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                	AND (C.Name = '{form.Param["unitName"] }' OR C.Initials = '{ form.Param["unitName"] }')
                	AND L.Name = '{form.Param["level1Name"]}' 
                    {whereDepartment}
                    {whereDepartmentFiltro}

            GROUP BY 
	            M.NAME
            ORDER BY 4 DESC ";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefasAcumuladas")]
        public List<NaoConformidadeRHResultsSet> GraficoTarefasAcumuladas([FromBody] DTO.DataCarrierFormularioNew form)
        {

            var whereDepartment = "";
            var whereDepartmentFiltro = "";
            var whereSecao = "";
            var whereCargo = "";
            var whereStructure = "";
            var whereUnit = "";
            //var whereShift = "";
            //var whereCluster = "";
            //var whereCriticalLevel = "";
            //var whereClusterGroup = "";

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartmentFiltro = $@"AND D.Name = '{form.Param["departmentName"]}'";
            }

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L3.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            if (form.ParSecao_Ids.Length > 0)
            {
                whereSecao = $@" AND L3.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
            }

            if (form.ParCargo_Ids.Length > 0)
            {
                whereCargo = $@" AND L3.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
            }


            //if (form.Shift_Ids.Length > 0)
            //{
            //    whereShift = "\n AND CL1.Shift in (" + string.Join(",", form.Shift_Ids) + ")";
            //}

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
            {
                whereUnit = $@"AND L3.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
            }
            //else
            //{
            //    whereUnit = $@"AND UNI.Id IN (SELECT
            //    				ParCompany_Id
            //    			FROM ParCompanyXUserSgq
            //    			WHERE UserSgq_Id = { form.Param["auditorId"] })";
            //}

            //if (form.ParClusterGroup_Ids.Length > 0)
            //{
            //    whereClusterGroup = $@"AND PCG.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            //}

            //if (form.ParCluster_Ids.Length > 0)
            //{
            //    whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id in (" + string.Join(",", form.ParCluster_Ids) + ") AND PCC.Active = 1)";
            //}

            if (form.ParStructure_Ids.Length > 0)
            {
                whereStructure = $@"AND L3.Regional in ({string.Join(",", form.ParStructure_Ids)})";
            }

            //if (form.ParCriticalLevel_Ids.Length > 0)
            //{
            //    whereCriticalLevel = $@" AND PLC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")"; 
            //        //" AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id in (" + string.Join(",", form.ParCriticalLevel_Ids) + ")) ";

            //}


            var queryGraficoTarefasAcumuladas = $@"
 
        SELECT 
	            T.NAME AS TarefaName,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L3 L3 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L3.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L3.Parlevel1_Id = L.ID
	            INNER JOIN ParLevel2 M WITH (NOLOCK)
		            ON L3.Parlevel2_Id = M.ID
	            INNER JOIN ParLevel3 T WITH (NOLOCK)
		            ON L3.Parlevel3_Id = T.ID
	            INNER JOIN ParDepartment D WITH (NOLOCK)
		            ON L3.Secao_Id = D.ID
	            WHERE 1=1
                 AND L.Name IN ('{ form.Param["level1Name"] }') 
                 AND C.Name = '{ form.Param["unitName"] }'
                 AND CollectionDate BETWEEN '{ form.startDate.ToString("yyyy-MM-dd") }' AND '{ form.endDate.ToString("yyyy-MM-dd") } 23:59:59'
                 {whereDepartment}
                 {whereDepartmentFiltro}
        GROUP BY 
	        T.NAME
        ORDER BY 4 DESC
 ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(queryGraficoTarefasAcumuladas).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefa")]
        public List<NaoConformidadeRHResultsSet> GraficoTarefa([FromBody] DTO.DataCarrierFormularioNew form)
        {
            //_list = CriaMockGraficoTarefas();

            //var query = new NaoConformidadeRHResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //Av = av + i,
            //Nc = nc + i,
            //Proc = proc + i,
            //TarefaName = tarefaName + i.ToString()

            var whereDepartmentFiltro = "";
            var whereDepartment = "";
            var whereShift = "";
            var whereClusterGroup = "";
            var whereCluster = "";

            if (form.Shift_Ids.Length > 0)
            {
                whereShift = "\n AND L3.Shift   in (" + string.Join(",", form.Shift_Ids) + ") ";
            }

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                whereClusterGroup = $@"AND L3.Id in (" + string.Join(",", form.ParClusterGroup_Ids) + ")";
            }

            if (form.ParCluster_Ids.Length > 0)
            {
                whereCluster = $@"AND UNI.Id IN(SELECT PCC.ParCompany_Id FROM ParCompanyCluster PCC WHERE pcc.ParCluster_Id in (" + string.Join(",", form.ParCluster_Ids) + ") AND PCC.Active = 1)";
            }

            if (form.Param["departmentName"] != null && form.Param["departmentName"].ToString() != "")
            {
                whereDepartment = " AND D.Name = '" + form.Param["departmentName"] + "'";
            }

            if (form.ParDepartment_Ids.Length > 0)
            {
                whereDepartment = $@" AND L3.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
            }

            var query = "" +

                $@" 
 
        SELECT 
	            T.NAME AS TarefaName,
	            SUM(WeiEvaluation) AS AV,
	            SUM(WeiDefects) AS NC,
	            SUM(WeiDefects)/SUM(WeiEvaluation)*100 AS [PROC]
	            FROM DW.Cubo_Coleta_L3 L3 WITH (NOLOCK)
	            INNER JOIN ParCompany C WITH (NOLOCK)
		            ON L3.Unitid = C.ID
	            INNER JOIN ParLevel1 L WITH (NOLOCK)
		            ON L3.Parlevel1_Id = L.ID
	            INNER JOIN ParLevel2 M WITH (NOLOCK)
		            ON L3.Parlevel2_Id = M.ID
	            INNER JOIN ParLevel3 T WITH (NOLOCK)
		            ON L3.Parlevel3_Id = T.ID
	            INNER JOIN ParDepartment D WITH (NOLOCK)
		            ON L3.Secao_Id = D.ID
	            WHERE 1=1
                 AND L.Name IN ('{ form.Param["level1Name"] }') 
                 AND M.Name = '{ form.Param["level2Name"] }'
                 AND C.Name = '{ form.Param["unitName"] }'
                 AND CollectionDate BETWEEN '{ form.startDate.ToString("yyyy-MM-dd") }' AND '{ form.endDate.ToString("yyyy-MM-dd") } 23:59:59'
                
                 {whereDepartment}
                 {whereDepartmentFiltro}

        GROUP BY 
	        T.NAME
        ORDER BY 4 DESC
";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("PivotTable")]
        public dynamic PivotTable([FromBody] DTO.DataCarrierFormularioNew form)
        {
            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {

                var level1Name = form.Param["level1Name"].ToString();
                var level2Name = form.Param["level2Name"].ToString();
                var centroDeCustoName = form.Param["level2Name"].ToString();
                var secaoName = form.Param["level2Name"].ToString();
                var cargoName = form.Param["level2Name"].ToString();
                var unitName = form.Param["unitName"].ToString();

                var parLevel1_Id = dbSgq.ParLevel1.Where(r => r.Name == level1Name).Select(r => r.Id).FirstOrDefault();

                var parLevel2_Id = dbSgq.ParLevel2.Where(r => r.Name == level2Name).Select(r => r.Id).FirstOrDefault();

                var parsecao_Id = dbSgq.ParDepartment.Where(r => r.Name == secaoName).Select(r => r.Id).FirstOrDefault();

                var unit_Id = dbSgq.ParCompany.Where(r => r.Name == unitName).Select(r => r.Id).FirstOrDefault();

                var sql = $@"
		 
		 -------------------------------------------------------------------------------------------------------------------------
		 --------	INPUTS					
		 -------------------------------------------------------------------------------------------------------------------------

		 DECLARE @DATEINI DATETIME = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00' DECLARE @DATEFIM DATETIME = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59';
		 DECLARE @UNITID VARCHAR(10) = '{unit_Id}', @PARLEVEL1_ID VARCHAR(10) = '{parLevel1_Id}',@PARLEVEL2_ID VARCHAR(10) = '{parLevel2_Id}',@SECAO_ID VARCHAR(10) = '{parsecao_Id}';

		 -------------------------------------------------------------------------------------------------------------------------
		 -------------------------------------------------------------------------------------------------------------------------		 
		   
		 DECLARE @DATAINICIAL DATETIME = @DATEINI;
		 DECLARE @DATAFINAL DATETIME = @DATEFIM;                     

                    SELECT 
	                     CL2.id
	                    ,CL2.ParLevel1_Id
	                    ,CL2.ParLevel2_Id
	                    ,D.Parent_Id            AS ParCentroDeCusto_Id
	                    ,CPD.ParDepartment_Id   AS ParSecao_Id
	                    ,CCG.ParCargo_Id        AS ParCargo_Id
	                    ,CL2.UnitId
	                    ,CL2.CollectionDate
	                    ,CL2.EvaluationNumber
	                    ,CL2.Sample
	                    ,CL2.Sequential
	                    ,CL2.Side
	                    ,CL2.Shift
	                    ,CL2.Period
	                    ,CL2.AuditorId
	                    ,CL2.AddDate
	                    ,CL2.AlterDate 
                    INTO #CollectionLevel2
                    FROM collectionlevel2 CL2
                    INNER JOIN CollectionLevel2XParDepartment CPD
                        ON CL2.ID = CPD.CollectionLevel2_Id
                    INNER JOIN ParDepartment D
                        ON D.ID = CPD.ParDepartment_Id
                    INNER JOIN CollectionLevel2XParCargo CCG
                        ON CL2.ID = CCG.CollectionLevel2_Id
                    WHERE 1=1
						AND cl2.Collectiondate BETWEEN @DATEINI AND @DATEFIM
						AND CASE WHEN @UNITID = '0' THEN '0' ELSE cl2.unitid END = @UNITID
						AND CASE WHEN @PARLEVEL1_ID = '0' THEN '0' ELSE cl2.ParLevel1_id END = @PARLEVEL1_ID
						AND CASE WHEN @PARLEVEL2_ID = '0' THEN '0' ELSE cl2.ParLevel2_id END = @PARLEVEL2_ID
						AND CASE WHEN @SECAO_ID = '0' THEN '0' ELSE CPD.ParDepartment_id END = @SECAO_ID

 
                    CREATE INDEX IDX_CollectionLevel2_ID ON #CollectionLevel2(ID);
                    CREATE INDEX IDX_CollectionLevel2_UnitId ON #CollectionLevel2(UnitId);
                    CREATE INDEX IDX_CollectionLevel2_CollectionDate ON #CollectionLevel2(CollectionDate);
                    CREATE INDEX IDX_CollectionLevel2_ParLevel1_Id ON #CollectionLevel2(ParLevel1_Id);
                    CREATE INDEX IDX_CollectionLevel2_ParLevel2_Id ON #CollectionLevel2(ParLevel2_Id);
                    CREATE INDEX IDX_CollectionLevel2_12345 ON #CollectionLevel2(ID,UnitId,CollectionDate,ParLevel1_Id,ParLevel2_Id);



DECLARE @HeaderField varchar(max);

SELECT     @HeaderField =
STUFF(   
(SELECT DISTINCT ', '+ CONCAT(' [',ParHeaderField_Name,' - ',ROW_NUMBER() OVER(partition by cl2xph_.CollectionLevel2_id,cl2xph_.ParHeaderField_Name Order By cl2xph_.Id),']') 
FROM CollectionLevel2XParHeaderFieldGeral cl2xph_ 
INNER JOIN #CollectionLevel2 CL2
	ON cl2xph_.CollectionLevel2_id = CL2.ID
	 FOR XML PATH('')
 ), 1, 1, '')

	
----------------------------------------------------------------------------------------------------------------------------------------------------

-- Trás Os Cabeçalhos da Coleta
----------------------------------------------------------------------------------------------------------------------------------------------------

DECLARE @Header varchar(max) = ISNULL('
  

SELECT * INTO #HeaderField FROM (
SELECT 
	DISTINCT 
		 CL2.id CollectionLevel2_Id
		,CONCAT(CL2HF2.ParHeaderField_Name,'' - '',ROW_NUMBER() OVER(partition by CL2HF2.CollectionLevel2_Id,CL2HF2.ParHeaderField_Name Order By CL2HF2.Id)) ParHeaderField_Name
		,CONCAT(HF.name, '': '', case 
				when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
				when CL2HF2.ParFieldType_Id = 2 then case when EQP.Nome is null then cast(PRD.nCdProduto as varchar(500)) + '' - '' + PRD.cNmProduto else EQP.Nome end 
				when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar, CL2HF2.Value, 103)
				else CL2HF2.Value end) as Valor
FROM CollectionLevel2XParHeaderFieldGeral CL2HF2 (nolock) 
inner join #collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id
left join ParHeaderFieldGeral HF (nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id
left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id
left join ParMultipleValues PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2
left join Produto PRD with(nolock) on cast(PRD.nCdProduto as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 2
-- order by 1,2

) EmLinha
PIVOT 
	(max(VALOR) 
		FOR ParHeaderField_Name 
			in 
			(' + @HeaderField +')) EmColunas;

			
			 ','
CREATE TABLE #HeaderField
(
CollectionLevel2_id bigint,
Campo1 varchar(500)
)
'); 


print @Header

DECLARE @DEFECTS VARCHAR(MAX) = '

		 DECLARE @DATEINI DATETIME = '''+CONVERT(VARCHAR(20),@DATEINI,120)+''' DECLARE @DATEFIM DATETIME = '''+CONVERT(VARCHAR(20),@DATEFIM,120)+''';
		 DECLARE @dataFim_ date = @DATEFIM;
		 DECLARE @dataInicio_ date = @DATEINI;
		 SET @dataInicio_ = @DATEINI;
         
		 DECLARE @DATAFINAL DATE = @dataFim_;
		 DECLARE @DATAINICIAL DATE = DateAdd(mm, DateDiff(mm, 0, @DATAFINAL) - 1, 0);
		 SET @DATAINICIAL = @DATEINI;
        
 

        
        -- C1
        
        SELECT 
        	CL2.id,
        	CL2.CollectionDate  AS ConsolidationDate,
        	CL2.UnitId,
        	CL2.ParLevel1_Id,
        	CL2.ParLevel2_Id,
        	R3.ParLevel3_Id,
            CL2.ParCentroDeCusto_Id,
            CL2.ParSecao_Id,
            CL2.ParCargo_Id,
        	R3.WeiDefects,
        	R3.Defects,
        	R3.WeiEvaluation,
        	R3.Evaluation
        INTO #ConsolidationLevel
        FROM #CollectionLevel2 CL2 WITH (NOLOCK) 
        LEFT JOIN Result_Level3 R3 WITH (NOLOCK) 
        	ON CL2.ID = R3.CollectionLevel2_Id
        WHERE 1=1 
        AND CL2.CollectionDate BETWEEN @DATEINI AND @DATEFIM
        AND R3.IsNotEvaluate = 0      

        CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id); 
        CREATE INDEX IDX_HashConsolidationLevel_level3 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id); 
        CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId); 
        CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id); 

        
        -- CUBO

        SELECT 
        	 C1.ID						AS ID
			,0							AS ParCluster_ID
        	,''''						AS ParCluster_Name
        	,0							AS ParStructure_id
        	,'''' 						AS ParStructure_Name
        	,C1.UnitId					AS Unidade
        	,PC.Name					AS UnidadeName
        	,C1.ConsolidationDate		AS ConsolidationDate
        	,L1.ParConsolidationType_Id AS ParConsolidationType_Id
        	,C1.ParLevel1_Id			AS Indicador
        	,L1.Name					AS IndicadorName
        	,C1.ParLevel2_Id			AS Monitoramento
        	,L2.Name					AS MonitoramentoName
        	,concat(L2.Name, '' - '', PC.Name) as MonitoramentoUnidade
        	,C1.ParLevel3_Id			AS Tarefa
        	,L3.Name					AS TarefaName
            ,C1.ParCentroDeCusto_Id
            ,D1.Name AS Centro_De_Custo
            ,C1.ParSecao_Id
            ,D2.Name AS Secao
            ,C1.ParCargo_Id
            ,CG.Name AS Cargo
        	,0							    AS ParCriticalLevel_Id
        	,''''							AS ParCriticalLevel_Name
        	,L1.IsRuleConformity
        	,SUM(WeiEvaluation) AS [AVComPeso]
        	,SUM(WeiDefects) AS [nCComPeso]
        	,SUM(Evaluation) AS [AV]
        	,SUM(Defects) AS [NC]
                ,ISNULL((  SELECT TOP 1
					                PercentValue
					            FROM ParGoal pg
					            WHERE 1=1
					            AND pg.IsActive = 1
					            AND pg.ParLevel1_Id = C1.ParLevel1_Id
					            AND (isnull(pg.EffectiveDate,pg.EffectiveDate) <= C1.ConsolidationDate)
					            AND (pg.ParCompany_Id =  C1.UnitId or pg.ParCompany_Id is null)
					            Order By EffectiveDate DESC, ParCompany_Id DESC),
					            (  SELECT TOP 1
					                PercentValue
					            FROM ParGoal pg
					            WHERE 1=1
					            AND pg.IsActive = 1
					            AND pg.ParLevel1_Id = C1.ParLevel1_Id
					            AND (isnull(pg.EffectiveDate,pg.EffectiveDate) <= C1.ConsolidationDate)
					            AND (pg.ParCompany_Id =  C1.UnitId or pg.ParCompany_Id is null)
					            Order By EffectiveDate DESC, ParCompany_Id DESC))	AS Meta
        	INTO #CUBO
        	FROM #ConsolidationLevel C1
        	INNER JOIN ParLevel1 L1 WITH (NOLOCK)
         		ON C1.ParLevel1_Id = L1.ID
         		AND ISNULL(L1.ShowScorecard,1) = 1
         		AND L1.IsActive = 1
        
        	INNER JOIN ParLevel2 L2 WITH (NOLOCK)
         		ON C1.ParLevel2_Id = L2.ID
         		AND L2.IsActive = 1
        
        	INNER JOIN ParLevel3 L3 WITH (NOLOCK)
         		ON C1.ParLevel3_Id = L3.ID
         		AND L3.IsActive = 1

            INNER JOIN ParDepartment D1
                ON D1.ID = C1.ParCentroDeCusto_Id

            INNER JOIN ParDepartment D2
                ON D2.ID = C1.ParSecao_Id

            INNER JOIN ParCargo CG
                ON CG.ID = C1.ParCargo_Id
        
        	LEFT JOIN ParCompany PC WITH (NOLOCK)
         		ON PC.Id = C1.Unitid
        


        
        GROUP BY
        	 C1.UnitId 	
        	,PC.Name 	
        	,C1.ConsolidationDate 
        	,L1.ParConsolidationType_Id	
        	,L1.hashKey
        	,C1.ParLevel1_Id 	
        	,L1.Name 	
        	,C1.ParLevel2_Id
        	,L2.Name 	
        	,C1.ParLevel3_id
        	,L3.Name 	
        	,L1.IsRuleConformity
			,C1.ID
            ,C1.ParCentroDeCusto_Id
            ,D1.Name 
            ,C1.ParSecao_Id
            ,D2.Name 
            ,C1.ParCargo_Id
            ,CG.Name 
        
            update #CUBO set Meta = iif(IsRuleConformity = 0,Meta, (100 - Meta)) 

			SELECT 
                IndicadorName as Indicador,
                MonitoramentoName as Monitoramento,
                TarefaName as Tarefa,
                Centro_De_Custo as [Centro De Custo],
                Secao as Seção,
                Cargo as Cargo,
                H.*,
                Meta as Meta,
                AVComPeso as ''AV com Peso'',
                nCComPeso as ''NC com Peso'',
                UnidadeName as Unidade,
                AV as AV,
                NC as NC
			INTO #CUBO_ACERTO
            FROM #CUBO C
			LEFT JOIN #HeaderField H
				ON C.ID = H.CollectionLevel2_Id

            ALTER TABLE #CUBO_ACERTO DROP COLUMN CollectionLevel2_Id

			IF EXISTS (SELECT * FROM tempdb.INFORMATION_SCHEMA.COLUMNS WHERE 1=1 AND TABLE_NAME LIKE ''#CUBO_ACERTO%'' AND COLUMN_NAME LIKE ''Campo1'') 
			BEGIN
			   ALTER TABLE #CUBO_ACERTO DROP COLUMN Campo1
			END
	
			SELECT * FROM #CUBO_ACERTO
	

			';

EXEC(@Header + @DEFECTS)

DROP TABLE #CollectionLevel2

";

                return QueryNinja(dbSgq, sql);

                //return QueryNinja(dbSgq, "select top 1000 parlevel3_id, weight, parlevel3_name from Result_Level3");
            }
        }

        [HttpPost]
        [Route("GraficoTarefasAcumulada")]
        public List<NaoConformidadeRHResultsSet> GraficoTarefasAcumulada([FromBody] DTO.DataCarrierFormularioNew form)
        {
            //_list = CriaMockGraficoTarefasAcumuladas();

            //var query = new NaoConformidadeRHResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

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
                        "\n WHERE IND.Name ='" + form.Param["level1Name"] + "' " +
                        "\n /* and MON.Id = 1 */" +
                        "\n 	AND UNI.Name = '" + form.Param["unitName"] + "'" +
                        "\n 	AND CL2.ConsolidationDate BETWEEN '" + form.startDate.ToString("yyyy-MM-dd") + "' AND '" + form.endDate.ToString("yyyy-MM-dd") + "'" +
                        "\n GROUP BY " +
                        "\n -- IND.Id " +
                        "\n -- ,IND.Name " +
                        "\n  R3.ParLevel3_Id " +
                        "\n ,R3.ParLevel3_Name " +
                        "\n -- ,UNI.Name " +
                        "\n -- ,UNI.Id " +
                        "\n HAVING (SUM(R3.WeiDefects) / SUM(R3.WeiEvaluation) * 100) > 0" +
                        "\n ORDER BY 4 DESC";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeRHResultsSet>(query).ToList();
            }

            return _list;
        }

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoUnidades()
        {

            #region Props
            var nc = 10;
            var av = 10;
            var proc = 20;
            var unidade = "Unidade";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 30; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet() { Av = av + i, Nc = nc + i, Proc = proc + i, UnidadeName = unidade + i.ToString() });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoNcPorUnidadeIndicador()
        {
            #region Props
            var nc = 10;
            var av = 10;
            var proc = 20;
            var Meta = 2;
            var indicadorName = "Indicador1";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet()
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

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoMonitoramento()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var monitoramento = "Monitoramento";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet()
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

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoTarefas()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var tarefaName = "Tarefa";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 90; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet()
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

        internal List<NaoConformidadeRHResultsSet> CriaMockGraficoTarefasAcumuladas()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var tarefaName = "TarefaAcumulada";
            #endregion

            var list = new List<NaoConformidadeRHResultsSet>();
            for (int i = 0; i < 90; i++)
            {
                list.Add(new NaoConformidadeRHResultsSet()
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


public class NaoConformidadeRHResultsSet
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
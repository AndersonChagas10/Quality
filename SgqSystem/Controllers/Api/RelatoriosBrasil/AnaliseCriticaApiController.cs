using ADOFactory;
using DTO;
using SgqService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/AnaliseCritica")]
    public class AnaliseCriticaApiController : BaseApiController
    {

        [HttpPost]
        [Route("GetGraficoHistoricoUnidade")]
        public List<HistoricoUnidades> GetGraficoHistoricoUnidade([FromBody] DataCarrierFormularioNew form)
        {

            var retornoHistoricoUnidade = new List<HistoricoUnidades>();

            //retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 1, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-06", Nc = 1, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/1" });
            //retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 2, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-06", Nc = 2, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/2" });
            //retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 3, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-07", Nc = 3, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/4" });
            //retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 4, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-07", Nc = 4, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/4" });
            //retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 5, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-07", Nc = 5, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/4" });

            var titulo = "Histórico Consolidado";
            var SQLcentro = getQuery(form, 1);
            var script = SQLcentro;

            var tipoConsolidacao = form.DimensaoData;

            //fazer os ifs de tipo de consolidação - Dia - Semana - Mês
            if (tipoConsolidacao == 1)
            {
                script += $@"
                SELECT
                   '{ titulo }' AS ChartTitle
                   ,IIF(SUM(ISNULL (AVComPeso, 0)) = 0, 0, IIF(ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0) > 100, 100, ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0))) AS PorcentagemNc
                   ,ConsolidationDate AS [Date]
                   ,SUM(ISNULL (AVComPeso, 0)) AS AVComPeso
                   ,SUM(ISNULL (NCComPeso, 0)) AS NCComPeso
                   ,SUM(ISNULL (AV, 0)) AS AV
                   ,SUM(ISNULL (NC, 0)) AS NC
                   ,AVG(ISNULL (Meta, 0)) AS Meta
                FROM #CUBO Cubo WITH (NOLOCK)
                WHERE 1 = 1
                

                GROUP BY 
                		ConsolidationDate
                ORDER BY 3
                ";
            }
            else if (tipoConsolidacao == 2)
            {
                //fazer script de semana
                script += $@"
                    SELECT
                        'Histórico Consolidado' AS ChartTitle
                       ,IIF(SUM(ISNULL (AVComPeso, 0)) = 0, 0, IIF(ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0) > 100, 100, ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0))) AS PorcentagemNc
                       ,CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(Week, ConsolidationDate)) AS [Semana]
                       ,SUM(ISNULL (AVComPeso, 0)) AS AVComPeso
                       ,SUM(ISNULL (NCComPeso, 0)) AS NCComPeso
                       ,SUM(ISNULL (AV, 0)) AS AV
                       ,SUM(ISNULL (NC, 0)) AS NC
                       ,AVG(ISNULL (Meta, 0)) AS Meta
                    FROM #CUBO Cubo WITH (NOLOCK)
                    WHERE 1 = 1

                    GROUP BY
                    CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(Week, ConsolidationDate))
                    ORDER BY 3
                    ";
            }
            else
            {
                script += $@"
                SELECT
                    'Histórico Consolidado' AS ChartTitle
                   ,IIF(SUM(ISNULL (AVComPeso, 0)) = 0, 0, IIF(ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0) > 100, 100, ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0))) AS PorcentagemNc
                   ,CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(MONTH, ConsolidationDate)) AS [Mes]
                   ,SUM(ISNULL (AVComPeso, 0)) AS AVComPeso
                   ,SUM(ISNULL (NCComPeso, 0)) AS NCComPeso
                   ,SUM(ISNULL (AV, 0)) AS AV
                   ,SUM(ISNULL (NC, 0)) AS NC
                   ,AVG(ISNULL (Meta, 0)) AS Meta
                FROM #CUBO Cubo WITH (NOLOCK)
                WHERE 1 = 1

                GROUP BY --Indicador
                CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(MONTH, ConsolidationDate))
                ORDER BY CAST(CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(MONTH, ConsolidationDate)) + '-01' AS DATE)
                ";
            }

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoHistoricoUnidade = factory.SearchQuery<HistoricoUnidades>(script).ToList();
            }

            return retornoHistoricoUnidade;
        }



        [HttpPost]
        [Route("GetAnaliseCritica")]
        public List<AnaliseCriticaResultSet> GetAnaliseCritica([FromBody] DataCarrierFormularioNew form)
        {
            var listParLevel1_Id = form.ParLevel1_Ids.ToList();
            var listTendenciaResultSet = new List<AnaliseCriticaResultSet>();

            foreach (var parLevel1_Id in listParLevel1_Id)
            {
                //TODO: Fazer a busca de todos os graficos de tendencia pelo Indicador
                var analiseCriticaResultSet = new AnaliseCriticaResultSet();
                var tendencia = GetGraficoTendenciaIndicador(form, parLevel1_Id);

                if (tendencia.Count > 0)
                {
                    analiseCriticaResultSet.ListaTendenciaResultSet = tendencia;
                    analiseCriticaResultSet.ParLevel1_Id = tendencia[0].level1_Id.GetValueOrDefault();
                    analiseCriticaResultSet.ParLevel1_Name = tendencia[0].Level1Name;
                    listTendenciaResultSet.Add(analiseCriticaResultSet);
                }

                //TODO: Resto dos Indicadores

            }

            return listTendenciaResultSet;
        }

        private List<TendenciaResultSet> GetGraficoTendenciaIndicador(DataCarrierFormularioNew form, int ParLevel1_Id)
        {

            var retornoHistoricoUnidade = new List<TendenciaResultSet>();

            //retornoHistoricoUnidade.Add(new TendenciaResultSet() { Av = 1, Av_Peso = 1, Level1Name = "Indicador 1", level1_Id = 1, NC = 1, NC_Peso = 1, });

            var query = $@" 
            DECLARE @dataFim_ date = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
            DECLARE @dataInicio_ date = DATEADD(MONTH, 0, '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00')
            
            SET @dataInicio_ = DATEFROMPARTS(YEAR(@dataInicio_), MONTH(@dataInicio_), 01)
              
            declare @ListaDatas_ table(data_ date)
              
            WHILE @dataInicio_ <= @dataFim_ 
            
            BEGIN
            INSERT INTO @ListaDatas_
            	SELECT
            		@dataInicio_
            SET @dataInicio_ = DATEADD(DAY, 1, @dataInicio_)
            END
              
            DECLARE @DATAFINAL DATE = @dataFim_
            DECLARE @DATAINICIAL DATE = DateAdd(mm, DateDiff(mm, 0, @DATAFINAL) - 1, 0)
            
            DECLARE @INDICADOR INT = { ParLevel1_Id }
             
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
            		   ,C2.ParLevel1_id AS INDICADOR
            		   ,C2.EvaluationNumber AS AV
            		   ,C2.Sample AS AM
            		   ,CASE
            				WHEN SUM(C2.WeiDefects) = 0 THEN 0
            				ELSE 1
            			END DEF_AM
            		FROM CollectionLevel2 C2 (NOLOCK)
            		INNER JOIN ParLevel1 L1 (NOLOCK)
            			ON L1.Id = C2.ParLevel1_id
            		INNER JOIN ParCompany C (NOLOCK)
            			ON C.Id = C2.UnitId
            		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATAINICIAL AND @DATAFINAL
            		AND C2.NotEvaluatedIs = 0
            		AND C2.Duplicated = 0
            		AND L1.ParConsolidationType_Id = 4
            		GROUP BY C.Id
            				,ParLevel1_id
            				,EvaluationNumber
            				,Sample
            				,CAST(CollectionDate AS DATE)) TAB
            	GROUP BY UNIDADE
            			,INDICADOR
            			,Data
              
            DECLARE @RESS INT
            
            SELECT
            	@RESS =
            	COUNT(1)
            FROM (SELECT
            		COUNT(1) AS NA
            	FROM CollectionLevel2 C2 (NOLOCK)
            	LEFT JOIN Result_Level3 C3 (NOLOCK)
            		ON C3.CollectionLevel2_Id = C2.Id
            	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
            	AND C2.ParLevel1_id = (SELECT TOP 1
            			Id
            		FROM ParLevel1(nolock)
            		WHERE hashKey = 1)
            	AND C2.UnitId IN ({ string.Join(",", form.ParCompany_Ids) })
            	AND IsNotEvaluate = 1
            	GROUP BY C2.Id) NA
            WHERE NA = 2
            SELECT
            	level1_Id
               ,Level1Name
            	--,Level2Name AS Level2Name
            	--,Unidade_Id
            	--,UNIDADE
               ,PorcentagemNc
               ,(CASE
            		WHEN IsRuleConformity = 1 THEN (100 - Meta)
            		WHEN IsRuleConformity IS NULL THEN 0
            		ELSE Meta
            	END) AS Meta
               ,NcSemPeso AS NC
               ,AvSemPeso AS Av
               ,Data AS _Data
            FROM (SELECT
            		*
            	   ,CASE
            			WHEN Av IS NULL OR
            				Av = 0 THEN 0
            			ELSE NC / Av * 100
            		END AS PorcentagemNc
            	   ,CASE
            			WHEN CASE
            					WHEN Av IS NULL OR
            						Av = 0 THEN 0
            					ELSE NC / Av * 100
            				END >= (CASE
            					WHEN IsRuleConformity = 1 THEN (100 - Meta)
            					ELSE Meta
            				END) THEN 1
            			ELSE 0
            		END RELATORIO_DIARIO
            	FROM (SELECT
            			level1_Id
            		   ,IsRuleConformity
            		   ,Level1Name
            		   ,Level2Name
            		   ,Unidade_Id
            		   ,UNIDADE
            		   ,Data
            		   ,SUM(Av) AS Av
            		   ,SUM(AvSemPeso) AS AvSemPeso
            		   ,SUM(NC) AS NC
            		   ,SUM(NCSemPeso) AS NCSemPeso
            		   ,MAX(Meta) AS Meta
            		FROM (SELECT
            				NOMES.A1 AS level1_Id
            			   ,NOMES.A2 AS Level1Name
            			   ,'Tendência do Indicador ' + NOMES.A2 AS Level2Name
            			   ,IND.IsRuleConformity
            			   ,NOMES.A4 AS Unidade_Id
            			   ,NOMES.A5 AS Unidade
            			   ,CASE
            					WHEN IND.hashKey = 1 THEN (SELECT TOP 1
            								SUM(Quartos) - @RESS
            							FROM VolumePcc1b(nolock)
            							WHERE ParCompany_id = UNI.Id
            							AND Data = DD.data_)
            					WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
            					ELSE 0
            				END AS Av
            			   ,CASE
            					WHEN IND.hashKey = 1 THEN (SELECT TOP 1
            								SUM(Quartos) - @RESS
            							FROM VolumePcc1b(nolock)
            							WHERE ParCompany_id = UNI.Id
            							AND Data = DD.data_)
            					WHEN IND.ParConsolidationType_Id = 1 THEN EvaluateTotal
            					WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
            					WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
            					ELSE 0
            				END AS AvSemPeso
            			   ,CASE
            					WHEN IND.ParConsolidationType_Id = 1 THEN WeiDefects
            					WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects
            					WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
            					ELSE 0
            				END AS NC
            			   ,CASE
            					WHEN IND.ParConsolidationType_Id = 1 THEN DefectsTotal
            					WHEN IND.ParConsolidationType_Id = 2 THEN DefectsTotal
            					WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult
            					WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
            					ELSE 0
            				END AS NCSemPeso
            			   ,CASE
            
            					WHEN (SELECT
            								COUNT(1)
            							FROM ParGoal G (NOLOCK)
            							WHERE G.ParLevel1_id = CL1.ParLevel1_id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.IsActive = 1
            							AND G.EffectiveDate <= @DATAFINAL)
            						> 0 THEN (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G (NOLOCK)
            							WHERE G.ParLevel1_id = CL1.ParLevel1_id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							AND G.IsActive = 1
            							AND G.EffectiveDate <= @DATAFINAL
            							ORDER BY G.ParCompany_id DESC, EffectiveDate DESC)
            
            					ELSE (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G (NOLOCK)
            							WHERE G.ParLevel1_id = CL1.ParLevel1_id
            							AND (G.ParCompany_id = CL1.UnitId
            							OR G.ParCompany_id IS NULL)
            							ORDER BY G.ParCompany_id DESC, EffectiveDate ASC)
            				END
            				AS Meta
            			   ,DD.data_ AS Data
            			FROM @ListaDatas_ DD
            			LEFT JOIN (SELECT
            					*
            				FROM ConsolidationLevel1(nolock)
            				WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
            				AND UnitId <> 12341614) CL1
            				ON DD.data_ = CAST(CL1.ConsolidationDate AS DATE)
            			LEFT JOIN ParLevel1 IND (NOLOCK)
            				ON IND.Id = CL1.ParLevel1_id
            			LEFT JOIN ParCompany UNI (NOLOCK)
            				ON UNI.Id = CL1.UnitId
            			LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
            				ON A4.Unidade = UNI.Id
            				AND A4.INDICADOR = IND.Id
            			LEFT JOIN (SELECT
            					IND.Id A1
            				   ,IND.Name A2
            				   ,'Tendência do Indicador ' + IND.Name AS A3
            				   ,CL1.UnitId A4
            				   ,UNI.Name A5
            				   ,0 AS A6
            				FROM (SELECT
            						*
            					FROM ConsolidationLevel1(nolock)
            					WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
            					AND UnitId <> 11514) CL1
            				LEFT JOIN ParLevel1 IND (NOLOCK)
            					ON IND.Id = CL1.ParLevel1_id
            				LEFT JOIN ParCompany UNI (NOLOCK)
            					ON UNI.Id = CL1.UnitId
            				LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
            					ON A4.Unidade = UNI.Id
            					AND A4.INDICADOR = IND.Id
            				WHERE 1 = 1
            				-- Aqui coloca a merda do filtro
            				GROUP BY IND.Id
            						,IND.Name
            						,CL1.UnitId
            						,UNI.Name) NOMES
            				ON 1 = 1
            				AND (NOMES.A1 = CL1.ParLevel1_id
            				AND NOMES.A4 = UNI.Id)
            				OR (IND.Id IS NULL)) AGRUPAMENTO
            		GROUP BY level1_Id
            				,IsRuleConformity
            				,Level1Name
            				,Unidade_Id
            				,UNIDADE
            				,Level2Name
            				,Data) S1) S2
            WHERE 1 = 1
            AND S2.Unidade_Id IN ({ string.Join(",", form.ParCompany_Ids) })
            AND S2.level1_Id = @INDICADOR
            DROP TABLE #AMOSTRATIPO4";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoHistoricoUnidade = factory.SearchQuery<TendenciaResultSet>(query).ToList();
            }

            return retornoHistoricoUnidade;
        }

        private static string getQuery(DataCarrierFormularioNew form, int? nivel)
        {

            var Wunidade = "";
            var Query = "";

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] != 0)
            {
                Wunidade = " AND C1.UnitId IN (" + string.Join(",", form.ParCompany_Ids) + ")";
            }

            Query = $@"  

                DECLARE @DATEINI DATETIME = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00'
                DECLARE @DATEFIM DATETIME = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
                DECLARE @dataFim_ date = @DATEFIM
                DECLARE @dataInicio_ date = @DATEINI
                
                SET @dataInicio_ = @DATEINI
                
                CREATE TABLE #DATA (data date)
                
                WHILE @dataInicio_ <= @dataFim_  
                BEGIN
                INSERT INTO #DATA
                	SELECT
                		@dataInicio_
                SET @dataInicio_ = DATEADD(DAY, 1, @dataInicio_)
                              
                             END
                             DECLARE @DATAFINAL DATE = @dataFim_
                             DECLARE @DATAINICIAL DATE = DateAdd(mm, DateDiff(mm, 0, @DATAFINAL) - 1, 0)
                SET @DATAINICIAL = @DATEINI
                            
                CREATE INDEX IDX_Data ON #Data (Data);
                
                SELECT
                	V.ParCompany_Id
                   ,V.data
                   ,SUM(V.Quartos) AS VOLUMEPCC INTO #VOLUMES
                FROM VolumePcc1b V WITH (NOLOCK)
                WHERE 1 = 1
                GROUP BY V.ParCompany_Id
                		,V.data
                
                SELECT
                	Unidade
                   ,Indicador
                   ,[Shift]
                   ,[Period]
                   ,data
                   ,COUNT(1) AM
                   ,SUM(DEF_AM) DEF_AM INTO #AMOSTRA4
                FROM (SELECT
                		CAST(C2.CollectionDate AS DATE) AS DATA
                	   ,C.Id AS UNIDADE
                	   ,C2.ParLevel1_Id AS INDICADOR
                	   ,C2.[Shift]
                	   ,C2.[Period]
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
                			,C2.[Shift]
                			,C2.[Period]
                			,EvaluationNumber
                			,Sample
                			,CAST(CollectionDate AS DATE)) TAB
                GROUP BY Unidade
                		,Indicador
                		,data
                		,[Shift]
                		,[Period]
                
                SELECT
                	CL2.CollectionDate
                   ,CL2.UnitId
                   ,COUNT(DISTINCT CL2.Id) AS NA INTO #NA
                FROM CollectionLevel2 CL2 WITH (NOLOCK)
                LEFT JOIN Result_Level3 CL3 WITH (NOLOCK)
                	ON CL3.CollectionLevel2_Id = CL2.Id
                WHERE CONVERT(DATE, CL2.CollectionDate) BETWEEN CONVERT(DATE, @DATEINI) AND CONVERT(DATE, @DATEFIM)
                AND CL2.ParLevel1_Id IN (SELECT
                		Id
                	FROM ParLevel1 WITH (NOLOCK)
                	WHERE hashKey = 1)
                AND CL3.IsNotEvaluate = 1
                GROUP BY CL2.CollectionDate
                		,CL2.UnitId
                HAVING COUNT(DISTINCT CL2.Id) > 1
                
                SELECT
                	CL1.Id
                   ,CL1.ConsolidationDate
                   ,CL1.UnitId
                   ,CL1.ParLevel1_Id
                   ,CL1.[Shift]
                   ,CL1.[Period]
                   ,CL1.DefectsResult
                   ,CL1.WeiDefects
                   ,CL1.EvaluatedResult
                   ,CL1.WeiEvaluation
                   ,CL1.EvaluateTotal
                   ,CL1.TotalLevel3WithDefects
                   ,CL1.DefectsTotal INTO #ConsolidationLevel
                FROM ConsolidationLevel1 CL1 WITH (NOLOCK)
                WHERE 1 = 1
                AND CL1.ConsolidationDate BETWEEN @DATEINI AND @DATEFIM
                
                AND CL1.ParLevel1_Id != 43
                AND CL1.ParLevel1_Id != 42
                            
                CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id);
                CREATE INDEX IDX_HashConsolidationLevel_level1 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id);
                CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId);
                CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id);
                
                
                -- CUBO
                SELECT
                	CL.Id AS ParCluster_ID
                   ,CL.Name AS ParCluster_Name
                   ,CS.ParStructure_id AS ParStructure_id
                   ,S.Name AS ParStructure_Name
                   ,C1.UnitId AS Unidade
                   ,PC.Name AS UnidadeName
                   ,C1.ConsolidationDate AS ConsolidationDate
                   ,L1.ParConsolidationType_Id AS ParConsolidationType_Id
                   ,C1.ParLevel1_Id AS Indicador
                   ,L1.Name AS IndicadorName
                   ,L1C.ParCriticalLevel_Id AS ParCriticalLevel_Id
                   ,CRL.Name AS ParCriticalLevel_Name
                   ,L1.IsRuleConformity
                   ,CASE
                		WHEN L1.hashKey = 1 THEN ISNULL ((SELECT TOP 1
                					SUM(VOLUMEPCC)
                				FROM #VOLUMES V WITH (NOLOCK)
                				WHERE 1 = 1
                				AND V.data = C1.ConsolidationDate
                				AND V.ParCompany_Id = C1.UnitId)
                			, 0)
                			-
                			ISNULL ((SELECT
                					SUM(NA) AS NA
                				FROM #NA NA
                				WHERE NA.UnitId = C1.UnitId
                				AND NA.CollectionDate = C1.ConsolidationDate)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.WeiEvaluation)
                		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiEvaluation)
                		WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.EvaluatedResult)
                		WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL ((SELECT
                					SUM(AM) AM
                				FROM #AMOSTRA4 A4
                				WHERE 1 = 1
                				AND C1.UnitId = A4.Unidade
                				AND C1.ParLevel1_Id = A4.Indicador
                				AND C1.[Shift] = A4.[Shift]
                				AND C1.[Period] = A4.[Period]
                				AND C1.ConsolidationDate = A4.data)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.WeiEvaluation)
                		WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.WeiEvaluation)
                		ELSE SUM(0)
                	END AS [AVComPeso]
                   ,CASE
                		WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.WeiDefects)
                		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiDefects)
                		WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.DefectsResult)
                		WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL ((SELECT
                					SUM(DEF_AM) DEF_AM
                				FROM #AMOSTRA4 A4
                				WHERE 1 = 1
                				AND C1.UnitId = A4.Unidade
                				AND C1.ParLevel1_Id = A4.Indicador
                				AND C1.[Shift] = A4.[Shift]
                				AND C1.[Period] = A4.[Period]
                				AND C1.ConsolidationDate = A4.data)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.WeiDefects)
                		WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.TotalLevel3WithDefects)
                		ELSE SUM(0)
                	END AS [NCComPeso]
                   ,CASE
                		WHEN L1.hashKey = 1 THEN ISNULL ((SELECT TOP 1
                					SUM(VOLUMEPCC)
                				FROM #VOLUMES V WITH (NOLOCK)
                				WHERE 1 = 1
                				AND V.data = C1.ConsolidationDate
                				AND V.ParCompany_Id = C1.UnitId)
                			, 0)
                			-
                			ISNULL ((SELECT
                					SUM(NA) AS NA
                				FROM #NA NA
                				WHERE NA.UnitId = C1.UnitId
                				AND NA.CollectionDate = C1.ConsolidationDate)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.EvaluateTotal)
                		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiEvaluation)
                		WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.EvaluatedResult)
                		WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL ((SELECT
                					SUM(AM) AM
                				FROM #AMOSTRA4 A4
                				WHERE 1 = 1
                				AND C1.UnitId = A4.Unidade
                				AND C1.ParLevel1_Id = A4.Indicador
                				AND C1.[Shift] = A4.[Shift]
                				AND C1.[Period] = A4.[Period]
                				AND C1.ConsolidationDate = A4.data)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.EvaluateTotal)
                		WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.EvaluateTotal)
                		ELSE SUM(0)
                	END AS [AV]
                   ,CASE
                		WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.DefectsTotal)
                		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.DefectsTotal)
                		WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.DefectsResult)
                		WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL ((SELECT
                					SUM(DEF_AM) DEF_AM
                				FROM #AMOSTRA4 A4
                				WHERE 1 = 1
                				AND C1.UnitId = A4.Unidade
                				AND C1.ParLevel1_Id = A4.Indicador
                				AND C1.[Shift] = A4.[Shift]
                				AND C1.[Period] = A4.[Period]
                				AND C1.ConsolidationDate = A4.data)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.DefectsTotal)
                		WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.TotalLevel3WithDefects)
                		ELSE SUM(0)
                	END AS [NC]
                   ,ISNULL ((SELECT TOP 1
                			PercentValue
                		FROM ParGoal pg
                		WHERE 1 = 1
                		AND pg.IsActive = 1
                		AND pg.ParLevel1_Id = C1.ParLevel1_Id
                		AND (ISNULL (pg.EffectiveDate, pg.EffectiveDate) <= C1.ConsolidationDate)
                		AND (pg.ParCompany_Id = C1.UnitId
                		OR pg.ParCompany_Id IS NULL)
                		ORDER BY EffectiveDate DESC, ParCompany_Id DESC)
                	, (SELECT TOP 1
                			PercentValue
                		FROM ParGoal pg
                		WHERE 1 = 1
                		AND pg.IsActive = 1
                		AND pg.ParLevel1_Id = C1.ParLevel1_Id
                		AND (ISNULL (pg.EffectiveDate, pg.EffectiveDate) <= C1.ConsolidationDate)
                		AND (pg.ParCompany_Id = C1.UnitId
                		OR pg.ParCompany_Id IS NULL)
                		ORDER BY EffectiveDate DESC, ParCompany_Id DESC)
                	) AS Meta INTO #CUBO
                FROM #ConsolidationLevel C1
                INNER JOIN ParLevel1 L1 WITH (NOLOCK)
                	ON C1.ParLevel1_Id = L1.Id
                		AND ISNULL (L1.ShowScorecard, 1) = 1
                		AND L1.IsActive = 1
                
                LEFT JOIN ParCompany PC WITH (NOLOCK)
                	ON PC.Id = C1.UnitId
                
                INNER JOIN ParCompanyCluster CCL WITH (NOLOCK)
                	ON CCL.ParCompany_Id = PC.Id
                		AND CCL.Active = 1
                
                INNER JOIN ParLevel1XCluster L1C WITH (NOLOCK)
                	ON CCL.ParCluster_ID = L1C.ParCluster_ID
                		AND C1.ParLevel1_Id = L1C.ParLevel1_Id
                		AND L1C.Id = (SELECT TOP 1
                				aaa.Id
                			FROM ParLevel1XCluster aaa (NOLOCK)
                			WHERE aaa.ParLevel1_Id = L1.Id
                			AND aaa.ParCluster_ID = CCL.ParCluster_ID
                			AND aaa.EffectiveDate < @DATAFINAL
                			AND IsActive = 1
                			ORDER BY ParLevel1_Id, ParCluster_ID, EffectiveDate, AddDate, AlterDate)
                		AND L1C.IsActive = 1
                
                INNER JOIN ParCompanyXStructure CS WITH (NOLOCK)
                	ON PC.Id = CS.ParCompany_Id
                		AND CS.Active = 1
                
                INNER JOIN ParStructure S WITH (NOLOCK)
                	ON CS.ParStructure_id = S.Id
                		AND S.Active = 1
                
                INNER JOIN ParCluster CL WITH (NOLOCK)
                	ON L1C.ParCluster_ID = CL.Id
                		AND CL.IsActive = 1
                
                INNER JOIN ParStructureGroup SG WITH (NOLOCK)
                	ON S.ParStructureGroup_Id = SG.Id
                		AND SG.Id = 3
                
                LEFT JOIN ParScoreType ST WITH (NOLOCK)
                	ON L1.ParConsolidationType_Id = ST.Id
                		AND ST.IsActive = 1
                
                LEFT JOIN ParCriticalLevel CRL WITH (NOLOCK)
                	ON L1C.ParCriticalLevel_Id = CRL.Id
                		AND CRL.IsActive = 1
                
                LEFT JOIN ParLevel1XModule P1M WITH (NOLOCK)
                	ON P1M.ParLevel1_Id = L1C.ParLevel1_Id
                		AND P1M.IsActive = 1
                		AND P1M.EffectiveDateStart <= @DATAINICIAL
                		AND (P1M.ParCluster_ID IS NULL
                			OR P1M.ParCluster_ID IN (SELECT
                					ParCluster_ID
                				FROM ParCompanyCluster
                				WHERE Active = 1)
                		)
                
                WHERE 1 = 1
                { Wunidade }
                GROUP BY CL.Id
                		,CL.Name
                		,CS.ParStructure_id
                		,S.Name
                		,C1.UnitId
                		,PC.Name
                		,C1.ConsolidationDate
                		,L1.ParConsolidationType_Id
                		,L1.hashKey
                		,C1.ParLevel1_Id
                		,C1.[Shift]
                		,C1.[Period]
                		,L1.Name
                		,L1C.ParCriticalLevel_Id
                		,CRL.Name
                		,L1.IsRuleConformity
                
                UPDATE #CUBO
                SET Meta = IIF(IsRuleConformity = 0, Meta, (100 - Meta))";


            return Query;
        }
    }
}
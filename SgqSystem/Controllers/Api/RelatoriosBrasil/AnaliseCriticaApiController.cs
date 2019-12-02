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
        public List<HistoricoUnidade> GetGraficoHistoricoUnidade([FromBody] DataCarrierFormularioNew form)
        {

            var retornoHistoricoUnidade = new List<HistoricoUnidade>();

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
                ORDER BY 1
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
                    ORDER BY 1
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
                ORDER BY 1
                ";
            }

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoHistoricoUnidade = factory.SearchQuery<HistoricoUnidade>(script).ToList();
            }

            return retornoHistoricoUnidade;
        }

        [HttpPost]
        [Route("GetGraficoTendenciaIndicador")]
        public List<TendenciaResultSet> GetGraficoTendenciaIndicador([FromBody] DataCarrierFormularioNew form)
        {

            var retornoHistoricoUnidade = new List<TendenciaResultSet>();

            retornoHistoricoUnidade.Add(new TendenciaResultSet() { Av = 1, Av_Peso = 1, Level1Name = "Indicador 1", level1_Id = 1, NC = 1, NC_Peso = 1, });


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

    public class HistoricoUnidade
    {
        public decimal? Nc { get; set; }
        public decimal? PorcentagemNc { get; set; }
        public decimal? Av { get; set; }
        public decimal? AvComPeso { get; set; }
        public decimal? NcComPeso { get; set; }
        public string Semana { get; set; }
        public string Mes { get; set; }
        public DateTime? Date { get; set; }
        public string _Date
        {
            get
            {
                if (Date.HasValue)
                {
                    return Date.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string _DateEUA
        {
            get
            {

                if (Date.HasValue)
                {
                    return Date.Value.ToString("MM-dd-yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }

    public class TendenciaResultSet
    {
        public int? level1_Id { get; set; }
        public string Level1Name { get; set; }
        public int? level2_Id { get; set; }
        public string Level2Name { get; set; }
        public int? level3_Id { get; set; }
        public string Level3Name { get; set; }
        public int? Unidade_Id { get; set; }
        public string Unidade { get; set; }
        public decimal Meta { get; set; }
        public decimal PorcentagemNc { get; set; }
        public decimal Av { get; set; }
        public decimal Av_Peso { get; set; }
        public decimal NC { get; set; }
        public decimal NC_Peso { get; set; }
        public double Data { get { return _Data.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; } }
        public DateTime _Data { get; set; }
    }

}
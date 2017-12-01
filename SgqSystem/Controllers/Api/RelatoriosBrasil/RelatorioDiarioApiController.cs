using Dominio;
using DTO.Helpers;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/RelDiario")]
    public class RelatorioDiarioApiController : ApiController
    {
        #region Mock
        private PanelResulPanel _mock { get; set; }
        private void CriaMockGrafico1Level1()
        {
            var firstDayOfLastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(1);

            _mock = new PanelResulPanel();

            _mock.listResultSetLevel1 = new List<RelDiarioResultSet>();
            _mock.listResultSetTendencia = new List<RelDiarioResultSet>();
            _mock.listResultSetLevel2 = new List<RelDiarioResultSet>();
            _mock.listResultSetTarefaPorIndicador = new List<RelDiarioResultSet>();
            _mock.listResultSetLevel3 = new List<RelDiarioResultSet>();

            /*Query 1 para Indicador*/
            _mock.listResultSetLevel1.Add(new RelDiarioResultSet()
            {
                level1_Id = 1,
                Level1Name = "TINGIR",
                Unidade_Id = 1,
                Unidade = "Lins",
                ProcentagemNc = 66M,
                Meta = 5M,
                NC = 2M,
                Av = 3M
            });
            _mock.listResultSetLevel1.Add(new RelDiarioResultSet()
            {
                level1_Id = 2,
                Level1Name = "SECAR",
                Unidade_Id = 1,
                Unidade = "Lins",
                ProcentagemNc = 0M,
                Meta = 10M,
                NC = 0M,
                Av = 3M
            });

            var counter = 0;
            foreach (var i in _mock.listResultSetLevel1)
            {
                /*Query2 Para Tendencia*/
                for (int j = 0; j < 30; j++)
                {
                    if (j == 5)
                    {
                        //_mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        //{
                        //    level1_Id = i.level1_Id,
                        //    Level1Name = i.Level1Name,
                        //    Level2Name = "Tendência Level2 " + counter,
                        //    ProcentagemNc = 50M + counter,
                        //    NC = 4M + counter,
                        //    Av = 12M + counter,
                        //    Meta = 5M,
                        //    Unidade = i.Unidade,
                        //    Unidade_Id = i.Unidade_Id,
                        //    Data = DateTime.UtcNow.AddMonths(-1).AddDays(4).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                        //});
                    }
                    else if (j == 16)
                    {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            ProcentagemNc = 76M + counter,
                            NC = 8M + counter,
                            Av = 12M + counter,
                            Meta = 5M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            //Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                            //;
                        });
                    }
                    else if (j == 28)
                    {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            ProcentagemNc = 20M + counter,
                            NC = 1M + counter,
                            Av = 12M + counter,
                            Meta = 5M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            //Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                        });
                    }
                    //else {
                    //    _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                    //    {
                    //        level1_Id = i.level1_Id,
                    //        Level1Name = i.Level1Name,
                    //        Level2Name = "Tendência Level2 " + counter,
                    //        //ProcentagemNc = 0M + counter,
                    //        //NC = 0M + counter,
                    //        //Av = 0M + counter,
                    //        //Meta = 0M,
                    //        Unidade = i.Unidade,
                    //        Unidade_Id = i.Unidade_Id,
                    //        Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                    //    });
                    //}


                }

                /*Query 3 para Monitoramentos do Indicador*/
                _mock.listResultSetLevel2.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter,
                    Level1Name = i.Level1Name,
                    Level2Name = "Level2 - 1" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });
                _mock.listResultSetLevel2.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter + 1,
                    Level1Name = i.Level1Name,
                    Level2Name = "Level2 - 2" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });


                /*Query 4 para Tarefas Acumuladas do Indicador*/
                _mock.listResultSetTarefaPorIndicador.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter,
                    level3_Id = counter + 12,
                    Level1Name = i.Level1Name,
                    Level2Name = "Tarefas Acumuladas " + counter,
                    Level3Name = "Tarefas Acumuladas Level3 Senhor Amado" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });

                counter += 5;
            }

            counter = 0;
            foreach (var i in _mock.listResultSetLevel1)
            {
                var counter2 = 10;
                foreach (var ii in _mock.listResultSetLevel2.Where(r => r.level1_Id == i.level1_Id))
                {
                    /*Query 5 para Tarefas dos Monitoramentos dos indicadores*/
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2,
                        Level3Name = "Level3 - 1" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2 + 1,
                        Level3Name = "Level3 - 2" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2 + 2,
                        Level3Name = "Level3 - 3" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    counter2 += counter;
                }
            }

        }
        #endregion

        private PanelResulPanel _todosOsGraficos { get; set; }

        public RelatorioDiarioApiController()
        {
            _todosOsGraficos = new PanelResulPanel();
            CriaMockGrafico1Level1();
            //CriaMockLevel1();
        }

        [HttpPost]
        [Route("Grafico1")]
        public PanelResulPanel GetGrafico1Level1([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Report_Relatorio_Diario");

            using (var db = new SgqDbDevEntities())
            {

                /*Estas 2 primeiras queryes são independentes*/
                var queryIndicadores = PanelResulPanel.QueryIndicadores(form);
                var queryTendencia = PanelResulPanel.QueryGraficoTendencia(form);

                _todosOsGraficos.listResultSetLevel1 = db.Database.SqlQuery<RelDiarioResultSet>(queryIndicadores).ToList();
                _todosOsGraficos.listResultSetTendencia = db.Database.SqlQuery<RelDiarioResultSet>(queryTendencia).ToList();
                if (_todosOsGraficos.listResultSetLevel1.Count() == 0)
                    return _todosOsGraficos;

                var indicadores = _todosOsGraficos._idLevel1QueryIndicadores;
                /*Estas 3 demais queryes dependem do resultado da queryIndicadores*/
                var queryGrafico3 = PanelResulPanel.QueryGrafico3(form, indicadores);
                var queryGraficoTarefasAcumuladas = PanelResulPanel.QueryGraficoTarefasAcumuladas(form, indicadores);
                var queryGrafico4 = PanelResulPanel.QueryGrafico4(form, indicadores);

                _todosOsGraficos.listResultSetLevel2 = db.Database.SqlQuery<RelDiarioResultSet>(queryGrafico3).ToList();
                _todosOsGraficos.listResultSetTarefaPorIndicador = db.Database.SqlQuery<RelDiarioResultSet>(queryGraficoTarefasAcumuladas).ToList();
                _todosOsGraficos.listResultSetLevel3 = db.Database.SqlQuery<RelDiarioResultSet>(queryGrafico4).ToList();
            }

            return _todosOsGraficos;
            //return _mock;
        }

    }

    public class PanelResulPanel
    {
        public List<RelDiarioResultSet> listResultSetLevel1 { get; set; }
        public List<RelDiarioResultSet> listResultSetTendencia { get; set; }
        public List<RelDiarioResultSet> listResultSetLevel2 { get; set; }
        public List<RelDiarioResultSet> listResultSetTarefaPorIndicador { get; set; }
        public List<RelDiarioResultSet> listResultSetLevel3 { get; set; }

        public string _idLevel1QueryIndicadores
        {
            get
            {
                var filtro = string.Empty;

                if (listResultSetLevel1.IsNotNull())
                    if (listResultSetLevel1.Count() > 0)
                    {
                        listResultSetLevel1.ForEach(r => filtro += r.level1_Id + ",");
                        filtro = filtro.Remove(filtro.Length - 1);
                    }

                return filtro;
            }
        }

        internal static string QueryIndicadores(FormularioParaRelatorioViewModel form)
        {

            var whereCriticalLevel = "";

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var queryGrafico1 = $@"
 DECLARE @DATAINICIAL DATE = '{ form._dataInicioSQL }'
 DECLARE @DATAFINAL DATE = '{ form._dataFimSQL }'
 DECLARE @UNIDADE INT = { form.unitId }
 DECLARE @RESS INT

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
SELECT
	@RESS =
	COUNT(1)
FROM (SELECT
		COUNT(1) AS NA
	FROM CollectionLevel2 C2 (NOLOCK)
	LEFT JOIN Result_Level3 C3 (NOLOCK)
		ON C3.CollectionLevel2_Id = C2.Id
	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
	AND C2.ParLevel1_Id = (SELECT TOP 1
			id
		FROM Parlevel1(nolock)
		WHERE Hashkey = 1)
	AND C2.UnitId = @UNIDADE
	AND IsNotEvaluate = 1
	GROUP BY C2.ID) NA
WHERE NA = 2
SELECT
	level1_Id
   ,Level1Name
   ,Unidade_Id
   ,Unidade
   ,ProcentagemNc
   ,(CASE
		WHEN IsRuleConformity = 1 THEN (100 - META)
		ELSE Meta
	END) AS Meta
   ,NcSemPeso AS NC
   ,AvSemPeso AS Av
FROM (SELECT
		*
	   ,CASE
			WHEN AV IS NULL OR
				AV = 0 THEN 0
			ELSE NC / AV * 100
		END AS ProcentagemNc
	   ,CASE
			WHEN CASE
					WHEN AV IS NULL OR
						AV = 0 THEN 0
					ELSE NC / AV * 100
				END >= (CASE
					WHEN IsRuleConformity = 1 THEN (100 - META)
					ELSE Meta
				END) THEN 1
			ELSE 0
		END RELATORIO_DIARIO
	FROM (SELECT
			IND.Id AS level1_Id
		   ,IND.IsRuleConformity
		   ,IND.Name AS Level1Name
		   ,UNI.Id AS Unidade_Id
		   ,UNI.Name AS Unidade
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
							SUM(Quartos) - @RESS
						FROM VolumePcc1b(nolock)
						WHERE ParCompany_id = UNI.Id
						AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
				WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
							SUM(Quartos) - @RESS
						FROM VolumePcc1b(nolock)
						WHERE ParCompany_id = UNI.Id
						AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
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
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.AddDate <= @DATAFINAL)
					> 0 THEN (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G (NOLOCK)
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.AddDate <= @DATAFINAL
						ORDER BY G.ParCompany_Id DESC, AddDate DESC)

				ELSE (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G (NOLOCK)
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						ORDER BY G.ParCompany_Id DESC, AddDate ASC)
			END
			AS Meta
		--    , (SELECT TOP 1 PercentValue FROM ParGoal WHERE ParLevel1_Id = CL1.ParLevel1_Id AND(ParCompany_Id = CL1.UnitId OR ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC) AS Meta 
		FROM ConsolidationLevel1 CL1 (NOLOCK)
		INNER JOIN ParLevel1 IND (NOLOCK)
			ON IND.Id = CL1.ParLevel1_Id
		INNER JOIN ParCompany UNI (NOLOCK)
			ON UNI.Id = CL1.UnitId
		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
			ON A4.UNIDADE = UNI.Id
			AND A4.INDICADOR = IND.ID
		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		AND CL1.UnitId = @UNIDADE
        { whereCriticalLevel }
        ) S1) S2
WHERE RELATORIO_DIARIO = 1
ORDER BY 5 DESC
DROP TABLE #AMOSTRATIPO4 ";


            return queryGrafico1;
        }

        internal static string QueryGraficoTendencia(FormularioParaRelatorioViewModel form)
        {

            var whereCriticalLevel = "";

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var queryGraficoTendencia = $@" 
 DECLARE @dataFim_ date = '{ form._dataFimSQL }'
  
 DECLARE @dataInicio_ date = DATEADD(MONTH, -1, @dataFim_)
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
 DECLARE @UNIDADE INT = { form.unitId }
  
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
	AND C2.ParLevel1_Id = (SELECT TOP 1
			id
		FROM Parlevel1(nolock)
		WHERE Hashkey = 1)
	AND C2.UnitId = @UNIDADE
	AND IsNotEvaluate = 1
	GROUP BY C2.ID) NA
WHERE NA = 2
SELECT
	level1_Id
   ,Level1Name
   ,Level2Name AS Level2Name
   ,Unidade_Id
   ,Unidade
   ,ProcentagemNc
   ,(CASE
		WHEN IsRuleConformity = 1 THEN (100 - META)
		WHEN IsRuleConformity IS NULL THEN 0
		ELSE Meta
	END) AS Meta
   ,NcSemPeso AS NC
   ,AvSemPeso AS Av
   ,Data AS _Data
FROM (SELECT
		*
	   ,CASE
			WHEN AV IS NULL OR
				AV = 0 THEN 0
			ELSE NC / AV * 100
		END AS ProcentagemNc
	   ,CASE
			WHEN CASE
					WHEN AV IS NULL OR
						AV = 0 THEN 0
					ELSE NC / AV * 100
				END >= (CASE
					WHEN IsRuleConformity = 1 THEN (100 - META)
					ELSE Meta
				END) THEN 1
			ELSE 0
		END RELATORIO_DIARIO
	FROM (SELECT
			NOMES.A1 AS level1_Id
			--,IND.Id AS level1_Id  
		   ,NOMES.A2 AS Level1Name
			--,IND.Name AS Level1Name  
		   ,'Tendência do Indicador ' + NOMES.A2 AS Level2Name
		   ,IND.IsRuleConformity
		   ,NOMES.A4 AS Unidade_Id
			--,UNI.Id AS Unidade_Id  
		   ,NOMES.A5 AS Unidade
			--,UNI.Name AS Unidade  
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
							SUM(Quartos) - @RESS
						FROM VolumePcc1b(nolock)
						WHERE ParCompany_id = UNI.Id
						AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
				WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT TOP 1
							SUM(Quartos) - @RESS
						FROM VolumePcc1b(nolock)
						WHERE ParCompany_id = UNI.Id
						AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
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
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.AddDate <= @DATAFINAL)
					> 0 THEN (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G (NOLOCK)
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						AND G.AddDate <= @DATAFINAL
						ORDER BY G.ParCompany_Id DESC, AddDate DESC)
				ELSE (SELECT TOP 1
							ISNULL(G.PercentValue, 0)
						FROM ParGoal G (NOLOCK)
						WHERE G.ParLevel1_id = CL1.ParLevel1_Id
						AND (G.ParCompany_id = CL1.UnitId
						OR G.ParCompany_id IS NULL)
						ORDER BY G.ParCompany_Id DESC, AddDate ASC)
			END
			AS Meta
			--, CL1.ConsolidationDate as Data  
		   ,DD.Data_ AS Data
		FROM @ListaDatas_ DD
		LEFT JOIN (SELECT
				*
			FROM ConsolidationLevel1(nolock)
			WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
			AND UnitId <> 12341614) CL1
			ON DD.Data_ = CL1.ConsolidationDate
		LEFT JOIN ParLevel1 IND (NOLOCK)
			ON IND.Id = CL1.ParLevel1_Id
		--AND IND.ID = 1  
		LEFT JOIN ParCompany UNI (NOLOCK)
			ON UNI.Id = CL1.UnitId
		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
			ON A4.UNIDADE = UNI.Id
			AND A4.INDICADOR = IND.ID
		LEFT JOIN (SELECT
				IND.ID A1
			   ,IND.NAME A2
			   ,'Tendência do Indicador ' + IND.NAME AS A3
			   ,CL1.UnitId A4
			   ,UNI.NAME A5
			   ,0 AS A6
			FROM (SELECT
					*
				FROM ConsolidationLevel1(nolock)
				WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
				AND UnitId <> 11514) CL1
			LEFT JOIN ParLevel1 IND (NOLOCK)
				ON IND.Id = CL1.ParLevel1_Id
			--AND IND.ID = 1  
			LEFT JOIN ParCompany UNI (NOLOCK)
				ON UNI.Id = CL1.UnitId
			LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
				ON A4.UNIDADE = UNI.Id
				AND A4.INDICADOR = IND.ID
			WHERE 1 = 1
            { whereCriticalLevel }
			GROUP BY IND.ID
					,IND.NAME
					,CL1.UnitId
					,UNI.NAME) NOMES
			ON 1 = 1
			AND (NOMES.A1 = CL1.ParLevel1_Id
			AND NOMES.A4 = UNI.ID)
			OR (IND.ID IS NULL)) S1) S2
WHERE (RELATORIO_DIARIO = 1
OR (RELATORIO_DIARIO = 0
AND AV = 0))
AND s2.Unidade_Id = @UNIDADE
DROP TABLE #AMOSTRATIPO4";

            return queryGraficoTendencia;
        }

        internal static string QueryGrafico3(FormularioParaRelatorioViewModel form, string indicadores)
        {
            var whereCriticalLevel = "";

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var queryGrafico3 = $@"
 DECLARE @DATAINICIAL DATE = '{ form._dataInicioSQL }'
 DECLARE @DATAFINAL DATE = '{ form._dataFimSQL }'
 DECLARE @UNIDADE INT = { form.unitId }
 DECLARE @RESS INT
 
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
SELECT
	@RESS =
	COUNT(1)
FROM (SELECT
		COUNT(1) AS NA
	FROM CollectionLevel2 C2 (NOLOCK)
	LEFT JOIN Result_Level3 C3 (NOLOCK)
		ON C3.CollectionLevel2_Id = C2.Id
	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
	AND C2.ParLevel1_Id = (SELECT TOP 1
			id
		FROM Parlevel1(nolock)
		WHERE Hashkey = 1)
	AND C2.UnitId = @UNIDADE
	AND IsNotEvaluate = 1
	GROUP BY C2.ID) NA
WHERE NA = 2
SELECT

	level1_Id
   ,Level1Name
   ,level2_Id
   ,Level2Name
   ,Unidade_Id
   ,Unidade
   ,AvSemPeso AS Av
   ,NcSemPeso AS NC
FROM (SELECT
		MON.Id AS level2_Id
	   ,MON.Name AS Level2Name
	   ,IND.Id AS level1_Id
	   ,IND.Name AS Level1Name
	   ,UNI.Id AS Unidade_Id
	   ,UNI.Name AS Unidade
	   ,CASE
			WHEN IND.HashKey = 1 THEN (SELECT TOP 1
						SUM(Quartos) - @RESS
					FROM VolumePcc1b(nolock)
					WHERE ParCompany_id = UNI.Id
					AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
			WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
			ELSE 0
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN (SELECT TOP 1
						SUM(Quartos) - @RESS
					FROM VolumePcc1b(nolock)
					WHERE ParCompany_id = UNI.Id
					AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
			WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
			ELSE 0
		END AS AvSemPeso
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
			WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
			ELSE 0
		END AS NC
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.DefectsTotal
			WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
			WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
			ELSE 0
		END AS NCSemPeso
	FROM ConsolidationLevel2 CL2 (NOLOCK)
	INNER JOIN ConsolidationLevel1 CL1 (NOLOCK)
		ON CL1.Id = CL2.ConsolidationLevel1_Id
	INNER JOIN ParLevel1 IND (NOLOCK)
		ON IND.Id = CL1.ParLevel1_Id
	INNER JOIN ParLevel2 MON (NOLOCK)
		ON MON.Id = CL2.ParLevel2_Id
	INNER JOIN ParCompany UNI (NOLOCK)
		ON UNI.Id = CL1.UnitId
	LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
		ON A4.UNIDADE = UNI.Id
		AND A4.INDICADOR = IND.ID
	WHERE CL2.ConsolidationDate BETWEEN '{ form._dataInicioSQL }' AND '{ form._dataFimSQL }'
	AND CL2.UnitId = { form.unitId }
    { whereCriticalLevel }
--AND CL1.ParLevel1_Id IN ({ indicadores }) 
) S1
WHERE NC > 0
ORDER BY 8 DESC
DROP TABLE #AMOSTRATIPO4 ";

            return queryGrafico3;
        }

        internal static string QueryGraficoTarefasAcumuladas(FormularioParaRelatorioViewModel form, string indicadores)
        {
            var whereCriticalLevel = "";

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var queryGraficoTarefasAcumuladas = $@"
SELECT

	IND.Id AS level1_Id
   ,IND.Name AS Level1Name
   ,IND.Id AS level2_Id
   ,IND.Name AS Level2Name
   ,R3.ParLevel3_Id AS level3_Id
   ,R3.ParLevel3_Name AS Level3Name
   ,UNI.Name AS Unidade
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
WHERE 1 = 1 --IND.Id IN ({ indicadores }) 
/* and MON.Id = 1 */
AND UNI.Id = { form.unitId }
AND CL2.ConsolidationDate BETWEEN '{ form._dataInicioSQL }' AND '{ form._dataFimSQL }'
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

            return queryGraficoTarefasAcumuladas;
        }

        internal static string QueryGrafico4(FormularioParaRelatorioViewModel form, string indicadores)
        {

            var whereCriticalLevel = "";

            if (form.criticalLevelId > 0)
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var queryGrafico4 = $@"SELECT
	IND.Id AS level1_Id
   ,IND.Name AS Level1Name
   ,MON.Id AS level2_Id
   ,MON.Name AS Level2Name
   ,R3.ParLevel3_Id AS level3_Id
   ,R3.ParLevel3_Name AS Level3Name
   ,UNI.Name AS Unidade
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
--IND.Id IN ({ indicadores }) 
/* and MON.Id = 1 */
AND UNI.Id = { form.unitId }
AND CL2.ConsolidationDate BETWEEN '{ form._dataInicioSQL }' AND '{ form._dataFimSQL }'
AND IND.Id IN (SELECT
		P1XC.ParLevel1_Id
	FROM ParLevel1XCluster P1XC
	WHERE P1XC.ParCriticalLevel_Id = 3)
GROUP BY IND.Id
		,IND.Name
		,MON.Id
		,MON.Name
		,R3.ParLevel3_Id
		,R3.ParLevel3_Name
		,UNI.Name
		,UNI.Id
HAVING SUM(R3.WeiDefects) > 0
AND SUM(R3.Defects) > 0
ORDER BY 9 DESC";

            return queryGrafico4;
        }
    }

    public class RelDiarioResultSet
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
        public decimal ProcentagemNc { get; set; }

        public decimal Av { get; set; }
        public decimal Av_Peso { get; set; }
        public decimal NC { get; set; }
        public decimal NC_Peso { get; set; }

        public double Data { get { return _Data.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; }  }
        //public double Data { get; set; }
        public DateTime _Data { get; set; }
    }


}


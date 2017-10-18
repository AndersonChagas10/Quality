using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/RelatorioDeResultados")]
    public class RelatorioDeResultadosApiController : BaseApiController
    {
        List<RelatorioResultadosPeriodo> retorno;
        List<RetornoGenerico> retorno2;
        List<RetornoGenerico> retorno3;
        List<RetornoGenerico> retorno4;

        private UserSgq usuario;
        private SgqDbDevEntities conexao;

        public RelatorioDeResultadosApiController()
        {
            retorno = new List<RelatorioResultadosPeriodo>();
            retorno2 = new List<RetornoGenerico>();
            retorno3 = new List<RetornoGenerico>();
            retorno4 = new List<RetornoGenerico>();
        }

        [HttpPost]
        [Route("listaResultadosPeriodoTabela")]
        public List<RelatorioResultadosPeriodo> listaResultadosPeriodoTabela([FromBody] FormularioParaRelatorioViewModel form)
        {

            //Nenhum Indicador Sem Unidade
            //Nenhum Indicador Com Unidade
            //Nenhum Monitoramento Sem Unidade
            //Nenhum Monitoramento Com Unidade
            //Nenhuma Tarefa Sem Unidade
            //Nenhuma Tarefa Com Unidade
            //Indicador Monitoramento Tarefa Com Unidade
            //Indicador Monitoramento tarefa Sem Unidade

            if (form.level1Id == 0) //Nenhum Indicador Sem Unidade
            {
                GetResultadosIndicador(form);
            }
            else if (form.level2Id == 0) //Nenhum Monitoramento Sem Unidade
            {
                GetResultadosMonitoramento(form);
            }
            else
            {
                GetResultadosTarefa(form);
            }

            return retorno;
        }


        private void GetResultadosIndicador(FormularioParaRelatorioViewModel form)
        {
            var where = "";
            var where2 = "";
            var whereStatus = "";

            if (form.unitId != 0)
            {
                where = "WHERE ID = " + form.unitId + "";
                where2 = "AND UNI.Id =" + form.unitId + "";
            }

            if (form.statusIndicador == 1)
            {
                whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 0";
            }
            else if (form.statusIndicador == 2)
            {
                whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 1";
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'


 DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
                                                                                                                                                                                                                    
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + where + @"
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
	CONVERT(VARCHAR(153), Unidade) AS UnidadeName
   ,Unidade_Id AS Unidade
   ,level1_Id AS Indicador
   ,CONVERT(VARCHAR(153), Level1Name) AS IndicadorName
   ,Concat(CONVERT(VARCHAR(153), Level1Name), ' - ',CONVERT(VARCHAR(153), Unidade)) AS IndicadorUnidade
   ,ProcentagemNc AS [Pc]
   ,(CASE
		WHEN IsRuleConformity = 1 THEN (100 - META)
		ELSE Meta
	END) AS Meta
   ,NC
   ,Av
   ,case when ProcentagemNc > S2.Meta then 0 else 1 end as Status
   ,CAST(1 as bit) as IsIndicador
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
	FROM (SELECT
			IND.Id AS level1_Id
		   ,IND.IsRuleConformity
		   ,IND.Name AS Level1Name
		   ,UNI.Id AS Unidade_Id
		   ,UNI.Name AS Unidade
		   ,CASE
				WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
				WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
				WHEN IND.ParConsolidationType_Id = 5 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 6 THEN WeiEvaluation
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
				WHEN IND.ParConsolidationType_Id = 1 THEN EvaluateTotal
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
				WHEN IND.ParConsolidationType_Id = 5 THEN EvaluateTotal
				WHEN IND.ParConsolidationType_Id = 6 THEN EvaluateTotal
				ELSE 0
			END AS AvSemPeso
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN WeiDefects
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects
				WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
				WHEN IND.ParConsolidationType_Id = 5 THEN WeiDefects
				WHEN IND.ParConsolidationType_Id = 6 THEN TotalLevel3WithDefects
				ELSE 0
			END AS NC
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN DefectsTotal
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects
				WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
				WHEN IND.ParConsolidationType_Id = 5 THEN DefectsTotal
				WHEN IND.ParConsolidationType_Id = 6 THEN TotalLevel3WithDefects
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
		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		" + where2 + @"
	-- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) 
	) S1
	GROUP BY Unidade
			,Unidade_Id
			,Level1Name
			,level1_Id
			,IsRuleConformity) S2
WHERE nc > 0
" + whereStatus + @"
ORDER BY 6 DESC
DROP TABLE #AMOSTRATIPO4 ";

            using (var db = new SgqDbDevEntities())
            {
                retorno = db.Database.SqlQuery<RelatorioResultadosPeriodo>(query).ToList();
            }
        }

        private void GetResultadosMonitoramento(FormularioParaRelatorioViewModel form)
        {
            var where = "";
            var where2 = "";

            if (form.unitId != 0)
            {
                where = "WHERE ID = " + form.unitId + "";
                where2 = "AND UNI.Id =" + form.unitId + "";
            }


            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'
                                                                                                                                                                                                                    
 DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + where + @"
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

	level1_Id as Indicador
	,Level1Name as IndicadorName
	,level2_Id as Monitoramento
	,Level2Name AS MonitoramentoName
	,concat(S1.Level2Name, ' - ', S1.Unidade) as MonitoramentoUnidade
	,Unidade_Id as Unidade
	,Unidade as UnidadeName
   ,SUM(avSemPeso) AS Av
   ,SUM(ncSemPeso) AS Nc
   ,CASE
		WHEN SUM(AV) IS NULL OR
			SUM(AV) = 0 THEN 0
		ELSE SUM(NC) / SUM(AV) * 100
	END AS Pc
   ,CAST(1 as bit) as IsMonitoramento
FROM (SELECT
		MON.Id AS level2_Id
	   ,MON.Name AS Level2Name
	   ,IND.Id AS level1_Id
	   ,IND.Name AS Level1Name
	   ,UNI.Id AS Unidade_Id
	   ,UNI.Name AS Unidade
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.EvaluatedResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation
			ELSE 0
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.EvaluatedResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal
			ELSE 0
		END AS AvSemPeso
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.DefectsResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
			ELSE 0
		END AS NC
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.DefectsResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
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
	WHERE CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
	" + where2 + @"
	AND IND.Id = " + form.level1Id + @" )S1
GROUP BY Level2Name, Unidade_Id, Unidade, level2_Id, level1_Id, S1.Level1Name
HAVING SUM(NC) > 0
ORDER BY 10 DESC ";

            using (var db = new SgqDbDevEntities())
            {
                retorno = db.Database.SqlQuery<RelatorioResultadosPeriodo>(query).ToList();
            }

        }

        private void GetResultadosTarefa(FormularioParaRelatorioViewModel form)
        {
            var where = "";
            var where2 = "";
            var where3 = "";

            if (form.unitId != 0)
            {
                where = "WHERE ID = " + form.unitId + "";
                where2 = "AND UNI.Id = " + form.unitId + "";
            }

            if (form.level3Id != 0)
            {
                where3 = "AND R3.ParLevel3_Id = " + form.level3Id + "";
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'

 DECLARE @DATAFINAL DATETIME = '" + form._dataFimSQL + @"'
       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + where + @"
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
	TAB.Indicador
   ,TAB.IndicadorName
   ,TAB.Monitoramento
   ,TAB.MonitoramentoName
   ,TAB.TarefaName AS TarefaName
   ,TAB.NcSemPeso AS Nc
   ,TAB.AvSemPeso AS Av
   ,[Proc] AS PC
   ,TAB.TarefaId AS Tarefa
   ,CONCAT(TarefaName, ' - ', UnidadeName) AS TarefaUnidade
   ,Unidade AS Unidade
   ,UnidadeName AS UnidadeName
   ,0 AS Sentido
   ,CAST(1 as bit) as IsTarefa
FROM (SELECT
		UNI.Id AS Unidade
	   ,UNI.Name AS UnidadeName
	   ,IND.Name AS IndicadorName
	   ,Ind.Id AS Indicador
	   ,MON.Name AS MonitoramentoName
	   ,Mon.Id AS Monitoramento
	   ,R3.ParLevel3_Id AS TarefaId
	   ,R3.ParLevel3_Name AS TarefaName
	   ,SUM(R3.WeiDefects) AS Nc
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiDefects)
			ELSE SUM(R3.Defects)
		END AS NcSemPeso
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			ELSE SUM(R3.WeiEvaluation)
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiEvaluation)
			ELSE SUM(R3.Evaluation)
		END AS AvSemPeso
	   ,SUM(R3.WeiDefects) /
		CASE
			WHEN IND.HashKey = 1 THEN (SELECT TOP 1
						SUM(Quartos) / 2
					FROM VolumePcc1b(nolock)
					WHERE ParCompany_id = UNI.Id
					AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
			ELSE SUM(R3.WeiEvaluation)
		END * 100 AS [Proc]
	FROM Result_Level3 R3 (NOLOCK)
	INNER JOIN CollectionLevel2 C2 (NOLOCK)
		ON C2.Id = R3.CollectionLevel2_Id
	INNER JOIN ConsolidationLevel2 CL2 (NOLOCK)
		ON CL2.Id = C2.ConsolidationLevel2_Id
	INNER JOIN ConsolidationLevel1 CL1 (NOLOCK)
		ON CL1.Id = CL2.ConsolidationLevel1_Id
	INNER JOIN ParCompany UNI (NOLOCK)
		ON UNI.Id = C2.UnitId
	INNER JOIN ParLevel1 IND (NOLOCK)
		ON IND.Id = C2.ParLevel1_Id
	INNER JOIN ParLevel2 MON (NOLOCK)
		ON MON.Id = C2.ParLevel2_Id
	WHERE IND.Id = " + form.level1Id + @"
	AND MON.Id = " + form.level2Id + @"
	" + where2 + @"
    " + where3 + @"
	AND R3.IsNotEvaluate = 0
	AND CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
	GROUP BY IND.Id
			,IND.Name
			,Mon.Name
			,MON.Id
			,R3.ParLevel3_Id
			,R3.ParLevel3_Name
			,UNI.Name
			,UNI.Id
			,ind.hashKey
			,ind.ParConsolidationType_Id
	HAVING SUM(R3.WeiDefects) > 0) TAB
ORDER BY 8 DESC ";

            using (var db = new SgqDbDevEntities())
            {
                retorno = db.Database.SqlQuery<RelatorioResultadosPeriodo>(query).ToList();
            }

        }

        [HttpPost]
        [Route("listaResultadosPeriodoSemUnidadeTabela")]
        public List<RelatorioResultadosPeriodo> listaResultadosPeriodoSemUnidadeTabela([FromBody] FormularioParaRelatorioViewModel form)
        {

            if (form.level1Id == 0) //Nenhum Indicador Sem Unidade
            {
                GetResultadosIndicadorSemUnidade(form);
            }
            else if (form.level2Id == 0) //Nenhum Monitoramento Sem Unidade
            {
                GetResultadosMonitoramentoSemUnidade(form);
            }
            else
            {
                GetResultadosTarefaSemUnidade(form);
            }

            return retorno;
        }

        private void GetResultadosIndicadorSemUnidade(FormularioParaRelatorioViewModel form)
        {
            var where = "";
            var where2 = "";
            var whereStatus = "";

            if (form.unitId != 0)
            {
                where = "WHERE ID = " + form.unitId + "";
                where2 = "AND UNI.Id =" + form.unitId + "";
            }

            if (form.statusIndicador == 1)
            {
                whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 0";
            }
            else if (form.statusIndicador == 2)
            {
                whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 1";
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'


 DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
                                                                                                                                                                                                                    
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + where + @"
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
    level1_Id AS Indicador
   ,CONVERT(VARCHAR(153), Level1Name) AS IndicadorName
   ,ProcentagemNc AS [Pc]
   ,(CASE
		WHEN IsRuleConformity = 1 THEN (100 - META)
		ELSE Meta
	END) AS Meta
   ,NC
   ,Av
   ,case when ProcentagemNc > S2.Meta then 0 else 1 end as Status
   ,CAST(1 as bit) as IsIndicador
FROM (SELECT
	   IsRuleConformity
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
	FROM (SELECT
			IND.Id AS level1_Id
		   ,IND.IsRuleConformity
		   ,IND.Name AS Level1Name
		   ,UNI.Id AS Unidade_Id
		   ,UNI.Name AS Unidade
		   ,CASE
				WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
				WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
				WHEN IND.ParConsolidationType_Id = 5 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 6 THEN WeiEvaluation
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC
				WHEN IND.ParConsolidationType_Id = 1 THEN EvaluateTotal
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
				WHEN IND.ParConsolidationType_Id = 5 THEN EvaluateTotal
				WHEN IND.ParConsolidationType_Id = 6 THEN EvaluateTotal
				ELSE 0
			END AS AvSemPeso
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN WeiDefects
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects
				WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
				WHEN IND.ParConsolidationType_Id = 5 THEN WeiDefects
				WHEN IND.ParConsolidationType_Id = 6 THEN TotalLevel3WithDefects
				ELSE 0
			END AS NC
		   ,CASE
				WHEN IND.ParConsolidationType_Id = 1 THEN DefectsTotal
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects
				WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM
				WHEN IND.ParConsolidationType_Id = 5 THEN DefectsTotal
				WHEN IND.ParConsolidationType_Id = 6 THEN TotalLevel3WithDefects
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
		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		" + where2 + @"
	) S1
	GROUP BY 
			Level1Name
			,level1_Id
			,IsRuleConformity) S2
WHERE nc > 0
" + whereStatus + @"
ORDER BY 3 DESC
DROP TABLE #AMOSTRATIPO4 ";

            using (var db = new SgqDbDevEntities())
            {
                retorno = db.Database.SqlQuery<RelatorioResultadosPeriodo>(query).ToList();
            }
        }

        private void GetResultadosMonitoramentoSemUnidade(FormularioParaRelatorioViewModel form)
        {
            var where = "";
            var where2 = "";

            if (form.unitId != 0)
            {
                where = "WHERE ID = " + form.unitId + "";
                where2 = "AND UNI.Id =" + form.unitId + "";
            }


            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'
                                                                                                                                                                                                                    
 DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + where + @"
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

	level1_Id as Indicador
	,Level1Name as IndicadorName
	,level2_Id as Monitoramento
	,Level2Name AS MonitoramentoName
   ,SUM(avSemPeso) AS Av
   ,SUM(ncSemPeso) AS Nc
   ,CASE
		WHEN SUM(AV) IS NULL OR
			SUM(AV) = 0 THEN 0
		ELSE SUM(NC) / SUM(AV) * 100
	END AS Pc
   ,CAST(1 as bit) as IsMonitoramento
FROM (SELECT
		MON.Id AS level2_Id
	   ,MON.Name AS Level2Name
	   ,IND.Id AS level1_Id
	   ,IND.Name AS Level1Name
	   ,UNI.Id AS Unidade_Id
	   ,UNI.Name AS Unidade
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.EvaluatedResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation
			ELSE 0
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.EvaluatedResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal
			ELSE 0
		END AS AvSemPeso
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.DefectsResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
			ELSE 0
		END AS NC
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.DefectsResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects
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
	WHERE CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
	" + where2 + @"
	AND IND.Id = " + form.level1Id + @" )S1
GROUP BY Level2Name, level2_Id, level1_Id, S1.Level1Name
HAVING SUM(NC) > 0
ORDER BY 7 DESC ";

            using (var db = new SgqDbDevEntities())
            {
                retorno = db.Database.SqlQuery<RelatorioResultadosPeriodo>(query).ToList();
            }

        }

        private void GetResultadosTarefaSemUnidade(FormularioParaRelatorioViewModel form)
        {
            var where = "";
            var where2 = "";
            var where3 = "";

            if (form.unitId != 0)
            {
                where = "WHERE ID = " + form.unitId + "";
                where2 = "AND UNI.Id = " + form.unitId + "";
            }

            if (form.level3Id != 0)
            {
                where3 = "AND R3.ParLevel3_Id = " + form.level3Id + "";
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'

 DECLARE @DATAFINAL DATETIME = '" + form._dataFimSQL + @"'
       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + where + @"
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
	TAB.Indicador
   ,TAB.IndicadorName
   ,TAB.Monitoramento
   ,TAB.MonitoramentoName
   ,TAB.TarefaName AS TarefaName
   ,TAB.NcSemPeso AS Nc
   ,TAB.AvSemPeso AS Av
   ,[Proc] AS PC
   ,TAB.TarefaId AS Tarefa
   --,CONCAT(TarefaName, ' - ', UnidadeName) AS TarefaUnidade
   --,Unidade AS Unidade
   --,UnidadeName AS UnidadeName
   ,0 AS Sentido
   ,CAST(1 as bit) as IsTarefa
FROM (SELECT
		--UNI.Id AS Unidade
	   --,UNI.Name AS UnidadeName
	   --,
        IND.Name AS IndicadorName
	   ,Ind.Id AS Indicador
	   ,MON.Name AS MonitoramentoName
	   ,Mon.Id AS Monitoramento
	   ,R3.ParLevel3_Id AS TarefaId
	   ,R3.ParLevel3_Name AS TarefaName
	   ,SUM(R3.WeiDefects) AS Nc
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiDefects)
			ELSE SUM(R3.Defects)
		END AS NcSemPeso
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			ELSE SUM(R3.WeiEvaluation)
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiEvaluation)
			ELSE SUM(R3.Evaluation)
		END AS AvSemPeso
	   ,SUM(R3.WeiDefects) /
		CASE
			WHEN IND.HashKey = 1 THEN (SELECT TOP 1
						SUM(Quartos) / 2
					FROM VolumePcc1b(nolock)
					WHERE 1 = 1 
                    --and ParCompany_id = UNI.Id
					AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
			ELSE SUM(R3.WeiEvaluation)
		END * 100 AS [Proc]
	FROM Result_Level3 R3 (NOLOCK)
	INNER JOIN CollectionLevel2 C2 (NOLOCK)
		ON C2.Id = R3.CollectionLevel2_Id
	INNER JOIN ConsolidationLevel2 CL2 (NOLOCK)
		ON CL2.Id = C2.ConsolidationLevel2_Id
	INNER JOIN ConsolidationLevel1 CL1 (NOLOCK)
		ON CL1.Id = CL2.ConsolidationLevel1_Id
	INNER JOIN ParCompany UNI (NOLOCK)
		ON UNI.Id = C2.UnitId
	INNER JOIN ParLevel1 IND (NOLOCK)
		ON IND.Id = C2.ParLevel1_Id
	INNER JOIN ParLevel2 MON (NOLOCK)
		ON MON.Id = C2.ParLevel2_Id
	WHERE IND.Id = " + form.level1Id + @"
	AND MON.Id = " + form.level2Id + @"
	" + where2 + @"
    " + where3 + @"
	AND R3.IsNotEvaluate = 0
	AND CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
	GROUP BY IND.Id
			,IND.Name
			,Mon.Name
			,MON.Id
			,R3.ParLevel3_Id
			,R3.ParLevel3_Name
			,ind.hashKey
			,ind.ParConsolidationType_Id
	HAVING SUM(R3.WeiDefects) > 0) TAB
ORDER BY 8 DESC ";

            using (var db = new SgqDbDevEntities())
            {
                retorno = db.Database.SqlQuery<RelatorioResultadosPeriodo>(query).ToList();
            }

        }

        [HttpPost]
        [Route("GetGraficoHistoricoModal")]
        public List<RetornoGenerico> GetGraficoHistoricoModal([FromBody] FormularioParaRelatorioViewModel form)
        {

            string query = "";

            if (form.level3Id != 0)
            {
                query = getQueryHistoricoTarefa(form);

            }
            else if (form.level2Id != 0)
            {
                query = getQueryHistoricoMonitoramento(form);
            }
            else
            {
                query = getQueryHistorioIndicador(form);
            }

            using (var db = new SgqDbDevEntities())
            {

                retorno2 = db.Database.SqlQuery<RetornoGenerico>(query).ToList();
            }

            //GetMockHistoricoModal();
            return retorno2;
        }

        private static string getQueryHistoricoTarefa(FormularioParaRelatorioViewModel form)
        {
            return @"


DECLARE @dataFim_ date = '" + form._dataFimSQL + @"'
  
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

       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
WHERE ID = " + form.unitId + @"

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
	TAB.Indicador AS level1id
   ,TAB.IndicadorName AS Level1Name
   ,TAB.Monitoramento AS level2Id
   ,TAB.MonitoramentoName AS Level2Name
   ,TAB.TarefaName AS level3Name
   ,TAB.NcSemPeso AS nc
   ,TAB.AvSemPeso AS av
   ,[Proc] AS procentagemNC
   ,TAB.TarefaId AS level3Id
   ,CONCAT(TarefaName, ' - ', UnidadeName) AS TarefaUnidade
   ,Unidade AS UnidadeId
   ,UnidadeName AS UnidadeName
   ,0 AS Sentido
   ,CAST(1 AS BIT) AS IsTarefa
   ,date
   ,'Histórico da Tarefa: ' + TAB.TarefaName as ChartTitle
FROM (SELECT
		UNI.Id AS Unidade
	   ,UNI.Name AS UnidadeName
	   ,IND.Name AS IndicadorName
	   ,Ind.Id AS Indicador
	   ,MON.Name AS MonitoramentoName
	   ,Mon.Id AS Monitoramento
	   ,R3.ParLevel3_Id AS TarefaId
	   ,R3.ParLevel3_Name AS TarefaName
	   ,SUM(R3.WeiDefects) AS Nc
	   ,CASE
			WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiDefects)
			ELSE SUM(R3.Defects)
		END AS NcSemPeso
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			ELSE SUM(R3.WeiEvaluation)
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiEvaluation)
			ELSE SUM(R3.Evaluation)
		END AS AvSemPeso
	   ,SUM(R3.WeiDefects) /
		CASE
			WHEN IND.HashKey = 1 THEN (SELECT TOP 1
						SUM(Quartos) / 2
					FROM VolumePcc1b(nolock)
					WHERE ParCompany_id = UNI.Id
					AND Data BETWEEN @DATAINICIAL AND @DATAFINAL)
			ELSE SUM(R3.WeiEvaluation)
		END * 100 AS [Proc]
	   ,CAST(c2.CollectionDate AS DATE) AS date
	FROM Result_Level3 R3 (NOLOCK)
	INNER JOIN CollectionLevel2 C2 (NOLOCK)
		ON C2.Id = R3.CollectionLevel2_Id
	INNER JOIN ConsolidationLevel2 CL2 (NOLOCK)
		ON CL2.Id = C2.ConsolidationLevel2_Id
	INNER JOIN ConsolidationLevel1 CL1 (NOLOCK)
		ON CL1.Id = CL2.ConsolidationLevel1_Id
	INNER JOIN ParCompany UNI (NOLOCK)
		ON UNI.Id = C2.UnitId
	INNER JOIN ParLevel1 IND (NOLOCK)
		ON IND.Id = C2.ParLevel1_Id
	INNER JOIN ParLevel2 MON (NOLOCK)
		ON MON.Id = C2.ParLevel2_Id
	WHERE IND.Id = " + form.level1Id + @"
	AND MON.Id = " + form.level2Id + @"
	AND UNI.Id = " + form.unitId + @"
	AND r3.ParLevel3_Id = " + form.level3Id + @"
	AND R3.IsNotEvaluate = 0
	AND CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
	GROUP BY IND.Id
			,IND.Name
			,Mon.Name
			,MON.Id
			,R3.ParLevel3_Id
			,R3.ParLevel3_Name
			,UNI.Name
			,UNI.Id
			,ind.hashKey
			,ind.ParConsolidationType_Id
			,CAST(c2.CollectionDate AS date)
	HAVING SUM(R3.WeiDefects) > 0) TAB
ORDER BY 15";
        }

        private static string getQueryHistoricoMonitoramento(FormularioParaRelatorioViewModel form)
        {
            return @" 
 DECLARE @dataFim_ date = '" + form._dataFimSQL + @"'
  
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
 DECLARE @UNIDADE INT = " + form.unitId + @"

 CREATE TABLE #AMOSTRATIPO4a (   
 UNIDADE INT NULL,   
 INDICADOR INT NULL,   
 AM INT NULL,   
 DEF_AM INT NULL  
 )
INSERT INTO #AMOSTRATIPO4a
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
	level1_id as level1Id
   ,Level1Name AS Level1Name
   ,ff.level2_Id as level2Id
   ,ff.level2Name as Level2Name
   ,ChartTitle
   ,Unidade_Id AS UnidadeId
   ,Unidade AS UnidadeName
   ,SUM(procentagemNc) AS procentagemNc
   ,SUM(Meta) AS Meta
   ,SUM(nc) AS nc
   ,SUM(av) av
   ,[date]
FROM (SELECT
		level1_Id
	   ,Level1Name
	   ,level2_Id
	   ,level2Name
	   ,ChartTitle
	   ,Unidade_Id
	   ,Unidade
	   ,procentagemNc
	   ,(CASE
			WHEN IsRuleConformity = 1 THEN (100 - META)
			WHEN IsRuleConformity IS NULL THEN 0
			ELSE Meta
		END) AS Meta
	   ,NcSemPeso AS nc
	   ,AvSemPeso AS av
	   ,Data AS date
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
				--IND.Id AS level1_Id  
			   ,NOMES.A2 AS Level1Name
				--IND.Name     AS Level1Name  
			   ,'Histórico do Monitoramento ' + NOMES.A8 AS ChartTitle
			   ,IND.IsRuleConformity
			   ,NOMES.A4 AS Unidade_Id
				--UNI.Id  AS Unidade_Id  
			   ,NOMES.A5 AS Unidade
				--UNI.Name     AS Unidade  
				,Nomes.A7 as level2_Id
				,NOMES.a8 as level2Name
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
					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects
					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult
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
			LEFT JOIN ConsolidationLevel2 CL2
				ON CL2.ConsolidationLevel1_Id = CL1.Id
			LEFT JOIN ParLevel1 IND (NOLOCK)
				ON IND.Id = CL1.ParLevel1_Id
				AND IND.Id = " + form.level1Id + @"
            LEFT JOIN ParLevel2 MON (NOLOCK)
				ON MON.Id = CL2.ParLevel2_Id
				AND MON.Id = " + form.level2Id + @"
			LEFT JOIN ParCompany UNI (NOLOCK)
				ON UNI.Id = CL1.UnitId
				AND UNI.Id = @UNIDADE
			LEFT JOIN #AMOSTRATIPO4a A4 (NOLOCK)
				ON A4.UNIDADE = UNI.Id
				AND A4.INDICADOR = IND.ID
			LEFT JOIN (SELECT
					IND.ID A1
				   ,IND.NAME A2
				   ,'Tendência do Indicador ' + IND.NAME AS A3
				   ,CL1.UnitId A4
				   ,UNI.NAME A5
				   ,0 AS A6
				   ,Mon.Id A7
				   ,Mon.Name A8
				FROM (SELECT
						*
					FROM ConsolidationLevel1(nolock)
					WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
					AND UnitId <> 11514) CL1
				LEFT JOIN ConsolidationLevel2 CL2 (NOLOCK)
					ON CL2.ConsolidationLevel1_Id = CL1.Id
				LEFT JOIN ParLevel1 IND (NOLOCK)
					ON IND.Id = CL1.ParLevel1_Id
				LEFT JOIN ParLevel2 MON (NOLOCK)
					ON MON.Id = CL2.ParLevel2_Id
				--AND IND.ID = 1  
				LEFT JOIN ParCompany UNI (NOLOCK)
					ON UNI.Id = CL1.UnitId
				LEFT JOIN #AMOSTRATIPO4a A4 (NOLOCK)
					ON A4.UNIDADE = UNI.Id
					AND A4.INDICADOR = IND.ID
				GROUP BY IND.ID
						,IND.NAME
						,MON.Id
						,Mon.Name
						,CL1.UnitId
						,UNI.NAME) NOMES
				ON 1 = 1
				AND (NOMES.A1 = CL1.ParLevel1_Id
				AND NOMES.A4 = UNI.ID)
				OR (IND.ID IS NULL)
            where CL2.ParLevel2_Id = " + form.level2Id + @") S1) S2
	WHERE 1 = 1
	AND level1_Id = " + form.level1Id + @"
    AND S2.level2_Id = " + form.level2Id + @"
	AND Unidade_Id = @UNIDADE) ff
GROUP BY level1_id
		,Level1Name
		,ChartTitle
		,Unidade_Id
		,Unidade
		,[date]
		,level2_Id
		,level2Name
having sum(av) is not null or sum(nc) is not null
ORDER BY 12
DROP TABLE #AMOSTRATIPO4a  ";
        }

        private static string getQueryHistorioIndicador(FormularioParaRelatorioViewModel form)
        {
            return @" 
 DECLARE @dataFim_ date = '" + form._dataFimSQL + @"'
  
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
 DECLARE @UNIDADE INT = " + form.unitId + @"

 CREATE TABLE #AMOSTRATIPO4a (   
 UNIDADE INT NULL,   
 INDICADOR INT NULL,   
 AM INT NULL,   
 DEF_AM INT NULL  
 )
INSERT INTO #AMOSTRATIPO4a
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
	level1_id as level1Id
   ,Level1Name as Level1Name
   ,ChartTitle
   ,Unidade_Id as UnidadeId
   ,Unidade as UnidadeName
   ,SUM(procentagemNc) AS procentagemNc
   ,SUM(Meta) AS Meta
   ,SUM(nc) AS nc
   ,SUM(av) av
   ,[date]
FROM (SELECT
		level1_Id
	   ,Level1Name
	   ,ChartTitle
	   ,Unidade_Id
	   ,Unidade
	   ,procentagemNc
	   ,(CASE
			WHEN IsRuleConformity = 1 THEN (100 - META)
			WHEN IsRuleConformity IS NULL THEN 0
			ELSE Meta
		END) AS Meta
	   ,NcSemPeso AS nc
	   ,AvSemPeso AS av
	   ,Data AS date
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
				--IND.Id AS level1_Id  
			   ,NOMES.A2 AS Level1Name
			   --IND.Name     AS Level1Name  
			   ,'Histórico do Indicador ' + NOMES.A2 AS ChartTitle
			   ,IND.IsRuleConformity
			   ,NOMES.A4 AS Unidade_Id
			   --UNI.Id  AS Unidade_Id  
			   ,NOMES.A5 AS Unidade
			   --UNI.Name     AS Unidade  
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
					WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects
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
				AND IND.Id = " + form.level1Id + @"
			LEFT JOIN ParCompany UNI (NOLOCK)
				ON UNI.Id = CL1.UnitId
				AND UNI.Id = @UNIDADE
			LEFT JOIN #AMOSTRATIPO4a A4 (NOLOCK)
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
				LEFT JOIN #AMOSTRATIPO4a A4 (NOLOCK)
					ON A4.UNIDADE = UNI.Id
					AND A4.INDICADOR = IND.ID
				GROUP BY IND.ID
						,IND.NAME
						,CL1.UnitId
						,UNI.NAME) NOMES
				ON 1 = 1
				AND (NOMES.A1 = CL1.ParLevel1_Id
				AND NOMES.A4 = UNI.ID)
				OR (IND.ID IS NULL)) S1) S2
	WHERE 1 = 1
	AND level1_Id  = " + form.level1Id + @"
	AND Unidade_Id = @UNIDADE) ff
GROUP BY level1_id
		,Level1Name
		,ChartTitle
		,Unidade_Id
		,Unidade
		,[date]
having sum(av) is not null or sum(nc) is not null
ORDER BY 10
DROP TABLE #AMOSTRATIPO4a  ";
        }

        [HttpPost]
        [Route("listaResultados")]
        public List<RetornoGenerico> listaResultados([FromBody] FormularioParaRelatorioViewModel form)
        {
            string query = "";

            if (form.level3Id != 0)
            {
                query = getQueryHistoricoTarefa(form);
            }
            else if (form.level2Id != 0)
            {
                query = getQueryHistoricoMonitoramento(form);
            }
            else
            {
                query = getQueryHistorioIndicador(form);
            }


            using (var db = new SgqDbDevEntities())
            {
                retorno3 = db.Database.SqlQuery<RetornoGenerico>(query).ToList();
            }

            //GetMockListaResultados();
            return retorno3;
        }

        [HttpPost]
        [Route("listaResultadosAcoesConcluidas")]
        public List<RetornoGenerico> listaResultadosAcoesConcluidas([FromBody] FormularioParaRelatorioViewModel form)
        {

            string query = "";

            if (form.level3Id != 0)
            {
                query = getQueryHistoricoTarefa(form);
            }
            else if (form.level2Id != 0)
            {
                query = getQueryHistoricoMonitoramento(form);
            }
            else
            {
                query = getQueryHistorioIndicador(form);
            }


            using (var db = new SgqDbDevEntities())
            {
                retorno4 = db.Database.SqlQuery<RetornoGenerico>(query).ToList();
            }

            //retorno4.Add(new RelatorioResultadosPeriodo { Av = 1, Data = DateTime.Now, Indicador = 1, IndicadorName = "Nome Indicador", Nc = 10, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 40, UnidadeName = "Lins" });
            //retorno4.Add(new RelatorioResultadosPeriodo { Av = 2, Data = DateTime.Now, Indicador = 2, IndicadorName = "Nome Indicador", Nc = 30, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 10, UnidadeName = "Lins" });
            //retorno4.Add(new RelatorioResultadosPeriodo { Av = 3, Data = DateTime.Now, Indicador = 3, IndicadorName = "Nome Indicador", Nc = 40, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 20, UnidadeName = "Lins" });
            //retorno4.Add(new RelatorioResultadosPeriodo { Av = 4, Data = DateTime.Now, Indicador = 4, IndicadorName = "Nome Indicador", Nc = 20, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 30, UnidadeName = "Lins" });
            //retorno4.Add(new RelatorioResultadosPeriodo { Av = 5, Data = DateTime.Now, Indicador = 5, IndicadorName = "Nome Indicador", Nc = 50, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 50, UnidadeName = "Lins" });
            return retorno4;
        }


        [HttpPost]
        [Route("listaAcoesIndicador")]
        public List<JObject> listaAcoesIndicador([FromBody] FormularioParaRelatorioViewModel form)
        {
            //string sql = "select sei la o que";

            //var result = QueryNinja(conexao, sql);

            var result5 = new List<JObject>();
            result5.Add(new JObject { });

            return result5;
        }

        #region Mocks

        private void GetMockResultadosPeriodo()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola - MTZ", UnidadeName = "Matriz" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa - MTZ", UnidadeName = "Matriz" });

        }

        private void GetMockResultadosPeriodo2()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Sangria - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Sangria - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Sangria - MTZ", UnidadeName = "Matriz" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Esfola da Pata TRS Esquerda - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Esfola da Pata TRS Esquerda - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Esfola da Pata TRS Esquerda - MTZ", UnidadeName = "Matriz" });

        }

        private void GetMockResultadosPeriodo3()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de contato - quarto esfolado / não esfolado - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de contato - quarto esfolado / não esfolado - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de contato - quarto esfolado / não esfolado - MTZ", UnidadeName = "Matriz" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de perfurações / contaminação - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de perfurações / contaminação - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de perfurações / contaminação - MTZ", UnidadeName = "Matriz" });

        }

        private void GetMockResultadosPeriodo4()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola", UnidadeName = "Matriz" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa", UnidadeName = "Matriz" });

        }

        private void GetMockHistoricoModal()
        {
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 250, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 200, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 150, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 50, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 350, date = DateTime.Now });
        }

        private void GetMockListaResultados()
        {
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 120, Real = 10 });
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 150, Real = 10 });
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 200, Real = 10 });
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 300, Real = 10 });
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 400, Real = 10 });
        }

        #endregion

    }


    public class RelatorioResultadosPeriodo
    {
        public DateTime Data { get; set; }
        public int Unidade { get; set; }
        public int Indicador { get; set; }
        public int Monitoramento { get; set; }
        public string MonitoramentoName { get; set; }
        public int Tarefa { get; set; }
        public string TarefaName { get; set; }
        public string UnidadeName { get; set; }
        public string IndicadorName { get; set; }
        public decimal Av { get; set; }
        public decimal Nc { get; set; }
        public decimal Pc { get; set; }
        public string Historico_Id { get; set; }
        public decimal Meta { get; set; }
        public int Status { get; set; }
        public string IndicadorUnidade { get; set; }
        public string MonitoramentoUnidade { get; set; }
        public string TarefaUnidade { get; set; }
        public int NumeroAcoesConcluidas { get; set; }
        public string Name { get; set; }
        public string _Data
        {
            get
            {
                return Data.ToString("dd/MM/yyyy");
            }
        }
        public bool IsIndicador { get; set; }
        public bool IsMonitoramento { get; set; }
        public bool IsTarefa { get; set; }
    }

    public class RetornoGenerico
    {

        public decimal av { get; set; }
        public string ChartTitle { get; set; }
        public decimal companyScorecard { get; set; }
        public string companySigla { get; set; }
        public DateTime date { get; set; }
        public int level1Id { get; set; }
        public string level1Name { get; set; }
        public int level2Id { get; set; }
        public string level2Name { get; set; }
        public int level3Id { get; set; }
        public string level3Name { get; set; }
        public decimal nc { get; set; }
        public decimal procentagemNc { get; set; }
        public int regId { get; set; }
        public string regName { get; set; }
        public decimal scorecard { get; set; }
        public decimal scorecardJbs { get; set; }
        public decimal scorecardJbsReg { get; set; }
        public string _date
        {
            get
            {
                return date.ToString("dd/MM/yyyy");
            }
        }
        public bool haveHistorico { get; set; }
        public decimal? limiteInferior { get; set; }
        public decimal? limiteSuperior { get; set; }
        public string levelName { get; set; }
        public string UnidadeName { get; set; }
        public string HISTORICO_ID { get; set; }
        public int? IsPaAcao { get; set; }
        public decimal Meta { get; set; }
        public string _dateEUA { get { return date.ToString("yyyy-MM-dd"); } }
    }

}

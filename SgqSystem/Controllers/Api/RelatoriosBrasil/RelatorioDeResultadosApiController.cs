using Dominio;
using DTO.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
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
        //List<RetornoLogJson> logJson;


        public RelatorioDeResultadosApiController()
        {
            retorno = new List<RelatorioResultadosPeriodo>();
            retorno2 = new List<RetornoGenerico>();
            retorno3 = new List<RetornoGenerico>();
            retorno4 = new List<RetornoGenerico>();
            //logJson = new List<RetornoLogJson>();
        }

        //[HttpPost]
        //[Route("listaSugestoes")]
        //public List<RetornoSugestao> ListaSugestoes([FromBody] FormularioParaRelatorioViewModel form)
        //{
        //    var script = "";

        //    script += @"SELECT 
        //         AddDate
        //         ,REPLACE(CAST(result AS VARCHAR(8000)),'""','''') AS result
        //         ,callback
        //            FROM logJson
        //            where callback like 'Relatorio_Nao_Conformidade' and AddDate >= '2017-07-27 11:45:31.4432595'
        //            order by 1 desc
        //            ";


        //    using (var db = new SgqDbDevEntities())
        //    {
        //    logJson = db.Database.SqlQuery<RetornoLogJson>(script).ToList();
        //    }

        //    dynamic recebeLista = new List<RetornoSugestao>();


        //    List<RetornoSugestao> montaLista;
        //    montaLista = new List<RetornoSugestao>();

        //    var Data = "";

        //    foreach (var lista in logJson)
        //    {
        //        recebeLista = JsonConvert.DeserializeObject<RetornoSugestao>(lista.result);

        //        recebeLista.DataConsulta = lista.AddDate.Value;

        //        if (recebeLista.DataInicio_ == null)
        //        {
        //            recebeLista.DataInicio_ = recebeLista.DataInicio.Value.ToString("yyyy-MM-dd");
        //        }
        //        else
        //        {
        //            recebeLista.DataInicio_.Value.ToString("");
        //        }

        //        Data = recebeLista.DataInicio.AsString;

        //        recebeLista.DataInicio_ = Data.ToString();
        //        recebeLista.Fim_ = recebeLista.Fim.AsDateTime;

        //        montaLista.Add(recebeLista);
        //    }


        //    return montaLista;
        //}


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

            if (/*form.level1Id == 0 && */form.level1IdArr.Length == 0) //Nenhum Indicador Sem Unidade
            {
                GetResultadosIndicador(form);
            }
            else if (/*form.level2Id == 0 && */form.level2IdArr.Length == 0) //Nenhum Monitoramento Sem Unidade
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
            var whereUnidade = "";
            var whereUnidade2 = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var userUnits = "";
            var whereStatus = "";

            if (form.unitIdArr.Length != 0)
            {
                whereUnidade = "WHERE ID IN (" + string.Join(",", form.unitIdArr) + ")";
                whereUnidade2 = "AND UNI.Id IN (" + string.Join(",", form.unitIdArr) + ")";
            }
            else
            {
                userUnits = GetUserUnits(form.auditorId);
                whereUnidade = "WHERE ID IN (" + userUnits + ")";
                whereUnidade2 = "AND UNI.Id IN (" + userUnits + ")";
            }

            if (form.statusIndicador == 1)
            {
                whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 0";
            }
            else if (form.statusIndicador == 2)
            {
                whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 1";
            }

            if (form.clusterIdArr.Length > 0)
            {
                whereCluster = "AND PCC.ParCluster_Id  IN (" + string.Join(",", form.clusterIdArr) + ")";
            }
            else
            if (form.clusterSelected_Id != 0)
            {
                whereCluster = "and PCC.ParCluster_Id =  " + form.clusterSelected_Id;
            }

            if (form.structureIdArr.Length > 0)
            {
                whereStructure = "AND CXS.ParStructure_Id  IN (" + string.Join(",", form.structureIdArr) + ")";
            }
            else
            if (form.structureId != 0)
            {
                whereStructure = "AND CXS.ParStructure_Id = " + form.structureId;
            }

            if (form.criticalLevelIdArr.Length > 0)
            {
                whereCriticalLevel = "AND L1XC.ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ")";
            }
            else
            if (form.criticalLevelId != 0)
            {
                whereCriticalLevel = "and L1XC.ParCriticalLevel_Id = " + form.criticalLevelId;
            }



            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'


 DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
                                                                                                                                                                                                                    
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + whereUnidade + @"
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
			ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1
            AND L1.Id <> 43
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
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
,avComPeso
   ,ncComPeso
FROM (SELECT
		Unidade
	   ,IsRuleConformity
	   ,Unidade_Id
	   ,Level1Name
	   ,level1_Id
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
	FROM (SELECT
			IND.Id AS level1_Id
		   ,IND.IsRuleConformity
		   ,IND.Name AS Level1Name
		   ,UNI.Id AS Unidade_Id
		   ,UNI.Name AS Unidade
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume) - ISNULL(@NAPCC,0)
				WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
				WHEN IND.ParConsolidationType_Id = 5 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 6 THEN WeiEvaluation
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume) - ISNULL(@NAPCC,0)
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
			ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
            AND IND.Id <> 43
		INNER JOIN ParCompany UNI (NOLOCK)
			ON UNI.Id = CL1.UnitId
		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
			ON A4.UNIDADE = UNI.Id
			AND A4.INDICADOR = IND.ID
		INNER JOIN ParLevel1XCluster L1XC (NOLOCK)
			ON CL1.ParLevel1_Id = L1XC.ParLevel1_Id
            and L1XC.IsActive = 1
		INNER JOIN ParCompanyXStructure CXS (NOLOCK)
			ON CL1.UnitId = CXS.ParCompany_Id
		INNER JOIN ParCompanyCluster PCC
			ON PCC.ParCompany_Id = UNI.Id  AND PCC.ParCluster_Id = L1XC.ParCluster_Id AND PCC.Active = 1
		WHERE 1 = 1
        AND CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		" + whereUnidade2 + @"
        " + whereCluster + @"
        " + whereStructure + @"
        " + whereCriticalLevel + @"
    -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) 
	) S1
	GROUP BY Unidade
			,Unidade_Id
			,Level1Name
			,level1_Id
			,IsRuleConformity) S2
WHERE 1=1 -- nc > 0
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
            var whereUnidade = "";
            var whereUnidade2 = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var userUnits = "";

            if (form.unitIdArr.Length != 0)
            {
                whereUnidade = "WHERE ID  IN (" + string.Join(",", form.unitIdArr) + ")";
                whereUnidade2 = "AND UNI.Id  IN (" + string.Join(",", form.unitIdArr) + ")";
            }
            else
            {
                userUnits = GetUserUnits(form.auditorId);
                whereUnidade = "WHERE ID IN (" + userUnits + ")";
                whereUnidade2 = "AND UNI.Id IN (" + userUnits + ")";
            }


            if (form.clusterIdArr.Length > 0)
            {
                whereCluster = "AND PCC.ParCluster_Id  IN (" + string.Join(",", form.clusterIdArr) + ")";
            }
            else
            if (form.clusterSelected_Id != 0)
            {
                whereCluster = "and PCC.ParCluster_Id =  " + form.clusterSelected_Id;
            }


            if (form.structureIdArr.Length > 0)
            {
                whereStructure = "AND CXS.ParStructure_Id  IN (" + string.Join(",", form.structureIdArr) + ")";
            }
            else
            if (form.structureId != 0)
            {
                whereStructure = "AND CXS.ParStructure_Id = " + form.structureId;
            }


            if (form.criticalLevelIdArr.Length > 0)
            {
                whereCriticalLevel = "AND L1XC.ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ")";
            }
            else
            if (form.criticalLevelId != 0)
            {
                whereCriticalLevel = "and L1XC.ParCriticalLevel_Id = " + form.criticalLevelId;
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'
                                                                                                                                                                                                                    
 DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + whereUnidade + @"
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
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
			WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume) / 2 - ISNULL(@NAPCC,0)
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.EvaluatedResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation
			ELSE 0
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume) / 2 - ISNULL(@NAPCC,0)
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
		ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
        AND IND.Id <> 43
	INNER JOIN ParLevel2 MON (NOLOCK)
		ON MON.Id = CL2.ParLevel2_Id
	INNER JOIN ParCompany UNI (NOLOCK)
		ON UNI.Id = CL1.UnitId
	INNER JOIN ParLevel1XCluster L1XC (NOLOCK)
		ON CL1.ParLevel1_Id = L1XC.ParLevel1_Id
           and L1XC.IsActive = 1
	INNER JOIN ParCompanyXStructure CXS (NOLOCK)
		ON CL1.UnitId = CXS.ParCompany_Id
	INNER JOIN ParCompanyCluster PCC (NOLOCK)
		ON PCC.ParCompany_Id = UNI.Id
        AND PCC.ParCluster_Id = L1XC.ParCluster_Id AND PCC.Active = 1
	WHERE 1 = 1
    AND CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
	" + whereUnidade2 + @"
    " + whereCluster + @"
    " + whereStructure + @"
    " + whereCriticalLevel + @"
	AND IND.Id = " + form.level1Id + @" )S1
GROUP BY Level2Name, Unidade_Id, Unidade, level2_Id, level1_Id, S1.Level1Name
-- HAVING SUM(NC) > 0
ORDER BY 10 DESC ";

            using (var db = new SgqDbDevEntities())
            {
                retorno = db.Database.SqlQuery<RelatorioResultadosPeriodo>(query).ToList();
            }

        }

        private void GetResultadosTarefa(FormularioParaRelatorioViewModel form)
        {
            var whereUnidade = "";
            var whereUnidade2 = "";
            var whereLevel3 = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var userUnits = "";

            if (form.unitIdArr.Length != 0)
            {
                whereUnidade = "WHERE ID  IN (" + string.Join(",", form.unitIdArr) + ")";
                whereUnidade2 = "AND UNI.Id  IN (" + string.Join(",", form.unitIdArr) + ")";
            }
            else
            {
                userUnits = GetUserUnits(form.auditorId);
                whereUnidade = "WHERE ID IN (" + userUnits + ")";
                whereUnidade2 = "AND UNI.Id IN (" + userUnits + ")";
            }

            if (form.level3IdArr.Length > 0)
            {
                whereLevel3 = "AND R3.ParLevel3_Id  IN (" + string.Join(",", form.level3IdArr) + ")";
            }
            else
            if (form.level3Id != 0)
            {
                whereLevel3 = "AND R3.ParLevel3_Id = " + form.level3Id + "";
            }

            if (form.clusterIdArr.Length > 0)
            {
                whereCluster = "AND PCC.ParCluster_Id  IN (" + string.Join(",", form.clusterIdArr) + ")";
            }
            else
            if (form.clusterSelected_Id != 0)
            {
                whereCluster = "and PCC.ParCluster_Id =  " + form.clusterSelected_Id;
            }

            if (form.structureIdArr.Length > 0)
            {
                whereStructure = "AND CXS.ParStructure_Id  IN (" + string.Join(",", form.structureIdArr) + ")";
            }
            else
            if (form.structureId != 0)
            {
                whereStructure = "AND CXS.ParStructure_Id = " + form.structureId;
            }

            if (form.criticalLevelIdArr.Length > 0)
            {
                whereCriticalLevel = "AND L1XC.ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ")";
            }
            else
            if (form.criticalLevelId != 0)
            {
                whereCriticalLevel = "and L1XC.ParCriticalLevel_Id = " + form.criticalLevelId;
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'

 DECLARE @DATAFINAL DATETIME = '" + form._dataFimSQL + @"'
       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + whereUnidade + @"
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
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
   ,SUM(TAB.NcSemPeso) AS Nc
   ,SUM(TAB.AvSemPeso) AS Av
   ,ISNULL(NULLIF(SUM(TAB.Nc),0)/SUM(TAB.AV),0)*100 AS [PC]
   ,TAB.TarefaId AS Tarefa
   ,CONCAT(TarefaName, ' - ', UnidadeName) AS TarefaUnidade
   ,Unidade AS Unidade
   ,UnidadeName AS UnidadeName
   ,0 AS Sentido
   ,CAST(1 as bit) as IsTarefa
FROM (SELECT  
		Unidade,UnidadeName,IndicadorName,Indicador,MonitoramentoName,Monitoramento,TarefaId,TarefaName
			,SUM(NC)NC
			,SUM(NcSemPeso)NcSemPeso
			,SUM(AV)AV
			,SUM(AvSemPeso) AvSemPeso
			,ISNULL(NULLIF(SUM(NC),0)/SUM(AV),0) [proc]
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
			WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = UNI.Id
											GROUP BY ParCompany_id) Volume) / 2 - @NAPCC
			ELSE SUM(R3.WeiEvaluation)
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = UNI.Id
											GROUP BY ParCompany_id) Volume) / 2 - @NAPCC
			WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiEvaluation)
			ELSE SUM(R3.Evaluation)
		END AS AvSemPeso
	   ,ISNULL(NULLIF(SUM(R3.WeiDefects),0) /
		CASE
			WHEN IND.HashKey = 1 THEN ((SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = UNI.Id
											GROUP BY ParCompany_id) Volume) / 2 - @NAPCC)
			ELSE SUM(R3.WeiEvaluation)
		END * 100,0) AS [Proc]
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
		ON IND.Id = C2.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
        AND IND.Id <> 43
	INNER JOIN ParLevel2 MON (NOLOCK)
		ON MON.Id = C2.ParLevel2_Id
	INNER JOIN ParLevel1XCluster L1XC (NOLOCK)
		ON CL1.ParLevel1_Id = L1XC.ParLevel1_Id
           and L1XC.IsActive = 1
	INNER JOIN ParCompanyXStructure CXS (NOLOCK)
		ON CL1.UnitId = CXS.ParCompany_Id
	INNER JOIN ParCompanyCluster PCC (NOLOCK)
		ON PCC.ParCompany_Id = UNI.Id
        AND PCC.ParCluster_Id = L1XC.ParCluster_Id AND PCC.Active = 1  
	WHERE IND.Id IN (" + string.Join(",", form.level1IdArr) + @")
	AND MON.Id IN (" + string.Join(",", form.level2IdArr) + @")
	" + whereUnidade2 + @"
    " + whereLevel3 + @"
    " + whereCluster + @"
    " + whereStructure + @"
    " + whereCriticalLevel + @"
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
            ,CL1.ConsolidationDate
	/*HAVING SUM(R3.WeiDefects) > 0*/) TAB
	GROUP BY 
		TAB.Indicador
	   ,TAB.IndicadorName
	   ,TAB.Monitoramento
	   ,TAB.MonitoramentoName
	   ,TAB.TarefaName 
	   ,TAB.TarefaId 
	   ,Unidade 
	   ,UnidadeName 
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
            var whereUnidade = "";
            var whereUnidade2 = "";
            var whereStatus = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var userUnits = "";

            if (form.unitId != 0)
            {
                whereUnidade = "WHERE ID = " + form.unitId + "";
                whereUnidade2 = "AND UNI.Id =" + form.unitId + "";
            }
            else
            {
                userUnits = GetUserUnits(form.auditorId);
                whereUnidade = "WHERE ID IN (" + userUnits + ")";
                whereUnidade2 = "AND UNI.Id IN (" + userUnits + ")";
            }

            if (form.statusIndicador == 1)
            {
                whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 0";
            }
            else if (form.statusIndicador == 2)
            {
                whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 1";
            }

            if (form.clusterSelected_Id != 0)
            {
                whereCluster = "and PCC.ParCluster_Id =  " + form.clusterSelected_Id;
            }

            if (form.structureId != 0)
            {
                whereStructure = "AND CXS.ParStructure_Id = " + form.structureId;
            }

            if (form.criticalLevelId != 0)
            {
                whereCriticalLevel = "and L1XC.ParCriticalLevel_Id = " + form.criticalLevelId;
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'


 DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
                                                                                                                                                                                                                    
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + whereUnidade + @"
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
			ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1
            AND L1.Id <> 43
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
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
				WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume)  - ISNULL(@NAPCC,0)
				WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
				WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
				WHEN IND.ParConsolidationType_Id = 5 THEN WeiEvaluation
				WHEN IND.ParConsolidationType_Id = 6 THEN WeiEvaluation
				ELSE 0
			END AS Av
		   ,CASE
				WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume)  - ISNULL(@NAPCC,0)
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
			ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
            AND IND.Id <> 43
		INNER JOIN ParCompany UNI (NOLOCK)
			ON UNI.Id = CL1.UnitId
		LEFT JOIN #AMOSTRATIPO4 A4 (NOLOCK)
			ON A4.UNIDADE = UNI.Id
			AND A4.INDICADOR = IND.ID
		INNER JOIN ParLevel1XCluster L1XC (NOLOCK)
			ON CL1.ParLevel1_Id = L1XC.ParLevel1_Id
            and L1XC.IsActive = 1
		INNER JOIN ParCompanyXStructure CXS (NOLOCK)
			ON CL1.UnitId = CXS.ParCompany_Id
		INNER JOIN ParCompanyCluster PCC (NOLOCK)
			ON PCC.ParCompany_Id = UNI.Id
            AND PCC.ParCluster_Id = L1XC.ParCluster_Id AND PCC.Active = 1
		WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
		" + whereUnidade2 + @"
        " + whereCluster + @"
        " + whereStructure + @"
        " + whereCriticalLevel + @"
	) S1
	GROUP BY 
			Level1Name
			,level1_Id
			,IsRuleConformity) S2
WHERE 1=1 -- AND nc > 0
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
            var whereUnidade = "";
            var whereUnidade2 = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var userUnits = "";

            if (form.unitId != 0)
            {
                whereUnidade = "WHERE ID = " + form.unitId + "";
                whereUnidade2 = "AND UNI.Id =" + form.unitId + "";
            }
            else
            {
                userUnits = GetUserUnits(form.auditorId);
                whereUnidade = "WHERE ID IN (" + userUnits + ")";
                whereUnidade2 = "AND UNI.Id IN (" + userUnits + ")";
            }

            if (form.clusterSelected_Id != 0)
            {
                whereCluster = "and PCC.ParCluster_Id =  " + form.clusterSelected_Id;
            }

            if (form.structureId != 0)
            {
                whereStructure = "AND CXS.ParStructure_Id = " + form.structureId;
            }

            if (form.criticalLevelId != 0)
            {
                whereCriticalLevel = "and L1XC.ParCriticalLevel_Id = " + form.criticalLevelId;
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'
                                                                                                                                                                                                                    
 DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + @"'
       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + whereUnidade + @"
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
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
			WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume) / 2 - ISNULL(@NAPCC,0)
			WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id IN (3, 4) THEN CL2.EvaluatedResult
			WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation
			WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation
			ELSE 0
		END AS Av
	   ,CASE
			WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume) / 2 - ISNULL(@NAPCC,0)
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
		ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
        AND IND.Id <> 43
	INNER JOIN ParLevel2 MON (NOLOCK)
		ON MON.Id = CL2.ParLevel2_Id
	INNER JOIN ParCompany UNI (NOLOCK)
		ON UNI.Id = CL1.UnitId
	INNER JOIN ParLevel1XCluster L1XC (NOLOCK)
		ON CL1.ParLevel1_Id = L1XC.ParLevel1_Id
           and L1XC.IsActive = 1
    INNER JOIN ParCompanyXStructure CXS (NOLOCK)
    	ON CL1.UnitId = CXS.ParCompany_Id
    INNER JOIN ParCompanyCluster PCC
    	ON PCC.ParCompany_Id = UNI.Id
        AND PCC.ParCluster_Id = L1XC.ParCluster_Id AND PCC.Active = 1
	WHERE CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
	" + whereUnidade2 + @"
    " + whereCluster + @"
    " + whereStructure + @"
    " + whereCriticalLevel + @"
	AND IND.Id = " + form.level1Id + @" )S1
GROUP BY Level2Name, level2_Id, level1_Id, S1.Level1Name
-- HAVING SUM(NC) > 0
ORDER BY 7 DESC ";

            using (var db = new SgqDbDevEntities())
            {
                retorno = db.Database.SqlQuery<RelatorioResultadosPeriodo>(query).ToList();
            }

        }

        private void GetResultadosTarefaSemUnidade(FormularioParaRelatorioViewModel form)
        {
            var whereUnidade = "";
            var whereUnidade2 = "";
            var whereLevel3 = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var userUnits = "";

            if (form.unitId != 0)
            {
                whereUnidade = "WHERE ID = " + form.unitId + "";
                whereUnidade2 = "AND UNI.Id = " + form.unitId + "";
            }
            else
            {
                userUnits = GetUserUnits(form.auditorId);
                whereUnidade = "WHERE ID IN (" + userUnits + ")";
                whereUnidade2 = "AND UNI.Id IN (" + userUnits + ")";
            }


            if (form.level3Id != 0)
            {
                whereLevel3 = "AND R3.ParLevel3_Id = " + form.level3Id + "";
            }

            if (form.clusterSelected_Id != 0)
            {
                whereCluster = "and PCC.ParCluster_Id =  " + form.clusterSelected_Id;
            }

            if (form.structureId != 0)
            {
                whereStructure = "AND CXS.ParStructure_Id = " + form.structureId;
            }

            if (form.criticalLevelId != 0)
            {
                whereCriticalLevel = "and L1XC.ParCriticalLevel_Id = " + form.criticalLevelId;
            }

            var query = @"
 DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + @"'

 DECLARE @DATAFINAL DATETIME = '" + form._dataFimSQL + @"'
       
 DECLARE @VOLUMEPCC int
                                                  
 DECLARE @ParCompany_id INT
SELECT
	@ParCompany_id = ID
FROM PARCOMPANY
" + whereUnidade + @"
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
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
   ,SUM(TAB.NcSemPeso) AS Nc
   ,SUM(TAB.AvSemPeso) AS Av
   ,ISNULL(NULLIF(SUM(TAB.Nc),0)/SUM(TAB.AV),0)*100 AS [PC]
   -- ,[Proc] AS PC
   ,TAB.TarefaId AS Tarefa
   --,CONCAT(TarefaName, ' - ', UnidadeName) AS TarefaUnidade
   --,TAB.Unidade AS Unidade
   --,TAB.UnidadeName AS UnidadeName
   ,0 AS Sentido
   ,CAST(1 as bit) as IsTarefa
FROM (SELECT  
		IndicadorName,Indicador,MonitoramentoName,Monitoramento,TarefaId,TarefaName
			,SUM(NC)NC
			,SUM(NcSemPeso)NcSemPeso
			,SUM(AV)AV
			,SUM(AvSemPeso) AvSemPeso
			,ISNULL(NULLIF(SUM(NC),0)/SUM(AV),0) [proc]
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
	   ,ISNULL(NULLIF(SUM(R3.WeiDefects),0) /
		CASE
			WHEN IND.HashKey = 1 THEN @VOLUMEPCC / 2 - @NAPCC
			ELSE SUM(R3.WeiEvaluation)
		END,0) * 100 AS [Proc]
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
		ON IND.Id = C2.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
        AND IND.Id <> 43
	INNER JOIN ParLevel2 MON (NOLOCK)
		ON MON.Id = C2.ParLevel2_Id
	INNER JOIN ParLevel1XCluster L1XC (NOLOCK)
		ON CL1.ParLevel1_Id = L1XC.ParLevel1_Id
           and L1XC.IsActive = 1
	INNER JOIN ParCompanyXStructure CXS (NOLOCK)
		ON CL1.UnitId = CXS.ParCompany_Id
	INNER JOIN ParCompanyCluster PCC
		ON PCC.ParCompany_Id = UNI.Id
        AND PCC.ParCluster_Id = L1XC.ParCluster_Id AND PCC.Active = 1
	WHERE IND.Id = " + form.level1Id + @"
	AND MON.Id = " + form.level2Id + @"
	" + whereUnidade2 + @"
    " + whereLevel3 + @"
    " + whereCluster + @"
    " + whereStructure + @"
    " + whereCriticalLevel + @"
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
            ,CL1.ConsolidationDate
	/*HAVING SUM(R3.WeiDefects) > 0*/) TAB
GROUP BY 
	TAB.Indicador
   ,TAB.IndicadorName
   ,TAB.Monitoramento
   ,TAB.MonitoramentoName
   ,TAB.TarefaName 
   ,TAB.TarefaId 
   --,CONCAT(TarefaName, ' - ', UnidadeName) 
   --,Unidade 
   --,UnidadeName 

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

        [HttpPost]
        [Route("GetHistoricoScore")]
        public List<RetornoGenerico> GetHistoricoScore([FromBody] FormularioParaRelatorioViewModel form)
        {

            #region consultaPrincipal

            /*
            * neste score NAO devo considerar a regra dos 70 %
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
"\n           ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1   AND CCL.ParCluster_Id = L1C.ParCluster_Id                                                                                                                                                                                                                                             " +
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
  "\n , ISNULL(CRL.Id, (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = 0)) AS Criterio                                                                                                                                                                      " +
  "\n , ISNULL(CRL.Name, (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = 0)) AS CriterioName                                                                                                                                                                  " +
  "\n , ISNULL((select top 1 Points from ParLevel1XCluster aaa (nolock) where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate < @DATAFINAL), (SELECT top 1 pontos FROM #FREQ WHERE unitId = 0)) AS Pontos                                    " +
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
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //    "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC)        FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date ) ) " +

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
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

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
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

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
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

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
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

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
           "\n FROM      (SELECT* FROM ParLevel1(nolock) WHERE ISNULL(ShowScorecard, 1) = 1) L1                                                                                                                                                                                                                                            " +
           "\n LEFT JOIN ConsolidationLevel1 CL1   (nolock)                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1.Id = CL1.ParLevel1_Id                                                                                                                                                                                                                                  " +
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
           "\n        ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                               " +
           "\n LEFT JOIN ParCluster CL       (nolock)                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParConsolidationType CT  (nolock)                                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                        " +
           "\n LEFT JOIN ParLevel1XCluster L1C  (nolock)                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1 AND CCL.ParCluster_Id = L1C.ParCluster_Id                                                                                                                                                                                                  " +
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
           "\n     , L1.HashKey                                                                                                                                                                                                                                                    " +
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
"\n   , mesData              " +

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

            #endregion

            #region Queryes Trs Meio

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
                whereCriticalLevel = $@"  AND P1.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }



            var where = string.Empty;
            where += "";


            var query4 =

                 "SELECT                     " +

                "\n    0 as level1Id                " +
                "\n   ,'a1' as Level1Name             " +
                "\n   ,'a2' as ChartTitle             " +
                "\n   ,0 as UnidadeId               " +
                "\n   ,'a4' as UnidadeName            " +
                "\n   ,20.0 AS procentagemNc          " +
                "\n   ,10.0 AS Meta                   " +
                "\n   ,CAST(ISNULL(case when sum(av) is null or sum(av) = 0 then '0'else cast(round(cast(case when isnull(sum(Pontos),100) = 0 or isnull(sum(PontosAtingidos),100) = 0 then 0 else (ISNULL(sum(PontosAtingidos),100) / isnull(sum(Pontos),100))*100  end  as decimal (10,1)),2) as varchar) end, 0) AS DECIMAL(10,2)) AS nc        " +
                "\n   ,50.0 as av                     " +
                "\n   ,mesData as [date]                 " +


                  "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 AND ISNULL(P1.ShowScorecard, 1) = 1" +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n INNER JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null " +
                    whereClusterGroup +
                    whereCluster +
                    whereStructure +
                    whereCriticalLevel +
                    whereUnit +
                    "\n group by mesData ORDER BY 10";

            #endregion
            var db = new SgqDbDevEntities();
            //db.Database.ExecuteSqlCommand(query);

            string grandeQuery = query + " " + query4;

            var result = db.Database.SqlQuery<RetornoGenerico>(grandeQuery).ToList();

            //var result1 = result.Where(r => r.QUERY == 1).ToList();
            //var result2 = result.Where(r => r.QUERY == 2).ToList();
            //var result3 = result.Where(r => r.QUERY == 3).ToList();
            // var result4 = result.Where(r => r.QUERY == 4).ToList();
            //var queryRowsBody = result.Where(r => r.QUERY == 6).ToList();

            var retorno = result;



            return retorno;
        }

        [HttpPost]
        [Route("GetHistoricoScoreNumero")]
        public List<RetornoGenerico> GetHistoricoScoreNumero([FromBody] FormularioParaRelatorioViewModel form)
        {
            #region consultaPrincipal

            /*
            * neste score NAO devo considerar a regra dos 70 %
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
                "\n     ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1 " +

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
"\n           ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1    AND CCL.ParCluster_Id = L1C.ParCluster_Id                                                                                                                                                                                                                                              " +
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
  "\n , ISNULL(CRL.Id, (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = 0)) AS Criterio                                                                                                                                                                      " +
  "\n , ISNULL(CRL.Name, (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = 0)) AS CriterioName                                                                                                                                                                  " +
  "\n , ISNULL((select top 1 Points from ParLevel1XCluster aaa (nolock) where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate < @DATAFINAL), (SELECT top 1 pontos FROM #FREQ WHERE unitId = 0)) AS Pontos                                    " +
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
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
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
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
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
           "\n FROM      (SELECT* FROM ParLevel1(nolock) WHERE ISNULL(ShowScorecard, 1) = 1) L1                                                                                                                                                                                                                                            " +
           "\n LEFT JOIN ConsolidationLevel1 CL1   (nolock)                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1.Id = CL1.ParLevel1_Id                                                                                                                                                                                                                                  " +
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
           "\n        ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1   AND CCL.ParCluster_Id = L1C.ParCluster_Id                                                                                                                                                                                                 " +
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
           "\n     , L1.HashKey                                                                                                                                                                                                                                                    " +
           //"\n     , C.Id   , CL1.ConsolidationDate,FT.DATA, FT.PARCOMPANY_ID                                                                                                                                                                                                                                                        " +
           "\n     , C.Id   , CL1.ConsolidationDate                                                                                                                                                                                                                                                       " +
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

            #endregion

            #region Queryes Trs Meio

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
                whereCriticalLevel = $@"  AND P1.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.criticalLevelId })";
            }

            var where = string.Empty;
            where += "";


            var query4 =

                 "SELECT                     " +

                "\n    0 as level1Id                " +
                "\n   ,'a1' as Level1Name             " +
                "\n   ,'a2' as ChartTitle             " +
                "\n   ,0 as UnidadeId               " +
                "\n   ,'a4' as UnidadeName            " +
                "\n   ,20.0 AS procentagemNc          " +
                "\n   ,10.0 AS Meta                   " +
                "\n   ,CAST(ISNULL(case when sum(av) is null or sum(av) = 0 then '0'else cast(round(cast(case when isnull(sum(Pontos),100) = 0 or isnull(sum(PontosAtingidos),100) = 0 then 0 else (ISNULL(sum(PontosAtingidos),100) / isnull(sum(Pontos),100))*100  end  as decimal (10,1)),2) as varchar) end, 0) AS DECIMAL(10,2)) AS nc        " +
                "\n   ,50.0 as av                     " +
                "\n   ,max(mesData) as [date]                 " +


                  "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 AND ISNULL(P1.ShowScorecard, 1) = 1" +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n INNER JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null " +
                    whereClusterGroup +
                    whereCluster +
                    whereStructure +
                    whereCriticalLevel +
                    whereUnit +
                    "\n  ORDER BY 10";

            #endregion
            var db = new SgqDbDevEntities();
            //db.Database.ExecuteSqlCommand(query);

            string grandeQuery = query + " " + query4;

            var result = db.Database.SqlQuery<RetornoGenerico>(grandeQuery).ToList();

            //var result1 = result.Where(r => r.QUERY == 1).ToList();
            //var result2 = result.Where(r => r.QUERY == 2).ToList();
            //var result3 = result.Where(r => r.QUERY == 3).ToList();
            // var result4 = result.Where(r => r.QUERY == 4).ToList();
            //var queryRowsBody = result.Where(r => r.QUERY == 6).ToList();

            var retorno = result;



            return retorno;
        }

        [HttpPost]
        [Route("GetHistoricoScoreMensal")]
        public List<RetornoGenerico> GetHistoricoScoreMensal([FromBody] FormularioParaRelatorioViewModel form)

        {
            #region consultaPrincipal

            /*
            * neste score NAO devo considerar a regra dos 70 %
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
"\n           ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1   AND CCL.ParCluster_Id = L1C.ParCluster_Id                                                                                                                                                                                                                                               " +
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
"\n  , max(month(mesData) + '-' + month(mesData) ) mesData FROM                                                                                                                                                                                                                                           " +
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
  "\n , ISNULL(CRL.Id, (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = 0)) AS Criterio                                                                                                                                                                      " +
  "\n , ISNULL(CRL.Name, (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = 0)) AS CriterioName                                                                                                                                                                  " +
  "\n , ISNULL((select top 1 Points from ParLevel1XCluster aaa (nolock) where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate < @DATAFINAL), (SELECT top 1 pontos FROM #FREQ WHERE unitId = 0)) AS Pontos                                    " +
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
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //    "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC)        FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date ) ) " +

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
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

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
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

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
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

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
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

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
           "\n FROM      (SELECT* FROM ParLevel1(nolock) WHERE ISNULL(ShowScorecard, 1) = 1) L1                                                                                                                                                                                                                                           " +
           "\n LEFT JOIN ConsolidationLevel1 CL1   (nolock)                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1.Id = CL1.ParLevel1_Id                                                                                                                                                                                                                                  " +
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
           "\n        ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1   AND CCL.ParCluster_Id = L1C.ParCluster_Id                                                                                                                                                                                                 " +
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
           "\n     , L1.HashKey                                                                                                                                                                                                                                                    " +
           //"\n     , C.Id   , CL1.ConsolidationDate,FT.DATA, FT.PARCOMPANY_ID                                                                                                                                                                                                                                                        " +
           "\n     , C.Id   , CL1.ConsolidationDate                                                                                                                                                                                                                                                       " +
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
"\n   , month(mesData) + '-' + month(mesData)              " +

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

            #endregion

            #region Queryes Trs Meio

            var where = string.Empty;
            where += "";


            var query4 =

                 "SELECT                     " +

                "\n    0 as level1Id                " +
                "\n   ,'a1' as Level1Name             " +
                "\n   ,'a2' as ChartTitle             " +
                "\n   ,0 as UnidadeId               " +
                "\n   ,'a4' as UnidadeName            " +
                "\n   ,20.0 AS procentagemNc          " +
                "\n   ,10.0 AS Meta                   " +
                "\n   ,CAST(ISNULL(case when sum(av) is null or sum(av) = 0 then '0'else cast(round(cast(case when isnull(sum(Pontos),100) = 0 or isnull(sum(PontosAtingidos),100) = 0 then 0 else (ISNULL(sum(PontosAtingidos),100) / isnull(sum(Pontos),100))*100  end  as decimal (10,1)),2) as varchar) end, 0) AS DECIMAL(10,2)) AS nc        " +
                "\n   ,50.0 as av                     " +
                "\n   ,mesData as [date]                 " +


                  "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 AND ISNULL(P1.ShowScorecard, 1) = 1" +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n INNER JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 2  and PP1.Name is not null" +
                    "\n group by mesData ORDER BY 10";

            #endregion
            var db = new SgqDbDevEntities();
            //db.Database.ExecuteSqlCommand(query);

            string grandeQuery = query + " " + query4;

            var result = db.Database.SqlQuery<RetornoGenerico>(grandeQuery).ToList();

            //var result1 = result.Where(r => r.QUERY == 1).ToList();
            //var result2 = result.Where(r => r.QUERY == 2).ToList();
            //var result3 = result.Where(r => r.QUERY == 3).ToList();
            // var result4 = result.Where(r => r.QUERY == 4).ToList();
            //var queryRowsBody = result.Where(r => r.QUERY == 6).ToList();

            var retorno = result;



            return retorno;
        }

        [HttpPost]
        [Route("GetGraficoHistoricoModalGeral")]
        public List<RetornoGenerico> GetGraficoHistoricoModalGeral([FromBody] FormularioParaRelatorioViewModel form)
        {

            string query = "";

            query = getQueryHistorioGeral(form, false);

            using (var db = new SgqDbDevEntities())
            {

                retorno2 = db.Database.SqlQuery<RetornoGenerico>(query).ToList();
            }

            //GetMockHistoricoModal();
            return retorno2;
        }

        [HttpPost]
        [Route("GetGraficoHistoricoModalUnidade")]
        public List<RetornoGenerico> GetGraficoHistoricoModalUnidade([FromBody] FormularioParaRelatorioViewModel form)
        {

            string query = "";

            query = getQueryHistorioGeral(form);

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
  
 DECLARE @dataInicio_ date = '" + form._dataInicioSQL + @"'
SET @dataInicio_ = '" + form._dataInicioSQL + @"'
  
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


 SET @DATAINICIAL = '" + form._dataInicioSQL + @"'

       
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
	AND C2.UnitId = @ParCompany_Id
	AND IsNotEvaluate = 1
	GROUP BY C2.ID) NA
WHERE NA = 2
--------------------------------                                                                                                                    
SELECT
	Indicador AS level1id
   ,IndicadorName AS Level1Name
   ,Monitoramento AS level2Id
   ,MonitoramentoName AS Level2Name
   ,TarefaName AS level3Name
   ,NcSemPeso AS nc
   ,AvSemPeso AS av
   ,[Proc] AS procentagemNC
   ,TarefaId AS level3Id
   ,CONCAT(TarefaName, ' - ', UnidadeName) AS TarefaUnidade
   ,Unidade AS UnidadeId
   ,UnidadeName AS UnidadeName
   ,0 AS Sentido
   ,CAST(1 AS BIT) AS IsTarefa
   ,date
   --,'Histórico da Tarefa: ' + TAB.TarefaName as ChartTitle
   ,'Histórico da Tarefa' as ChartTitle
FROM (SELECT  
		Date,Unidade,UnidadeName,IndicadorName,Indicador,MonitoramentoName,Monitoramento,TarefaId,TarefaName
			,SUM(NC)NC
			,SUM(NcSemPeso)NcSemPeso
			,SUM(AV)AV
			,SUM(AvSemPeso) AvSemPeso
			,ISNULL(NULLIF(SUM(NC),0)/SUM(AV),0) [proc]
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
	   ,ISNULL(NULLIF(SUM(R3.WeiDefects),0) /
		CASE
			WHEN IND.HashKey = 1 THEN ((SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = CAST(c2.CollectionDate AS DATE) 
											AND ParCompany_id = UNI.Id
											GROUP BY ParCompany_id) Volume) / 2 - @NAPCC)
			ELSE SUM(R3.WeiEvaluation)
		END,0) * 100 AS [Proc]
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
		ON IND.Id = C2.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
        AND IND.Id <> 43
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
	 /* HAVING SUM(R3.WeiDefects) > 0 */ ) TAB GROUP BY Date,Unidade,UnidadeName,IndicadorName,Indicador,MonitoramentoName,Monitoramento,TarefaId,TarefaName)A
ORDER BY 15";
        }

        private static string getQueryHistoricoMonitoramento(FormularioParaRelatorioViewModel form)
        {
            return @" 
 DECLARE @dataFim_ date = '" + form._dataFimSQL + @"'
  
 DECLARE @dataInicio_ date = '" + form._dataInicioSQL + @"'
SET @dataInicio_ = '" + form._dataInicioSQL + @"'
  
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
 SET @DATAINICIAL = '" + form._dataInicioSQL + @"'
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
			ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1
            AND L1.Id <> 43
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
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
			   --,'Histórico do Monitoramento ' + NOMES.A8 AS ChartTitle
               ,'Histórico do Monitoramento' AS ChartTitle
			   ,IND.IsRuleConformity
			   ,NOMES.A4 AS Unidade_Id
				--UNI.Id  AS Unidade_Id  
			   ,NOMES.A5 AS Unidade
				--UNI.Name     AS Unidade  
				,Nomes.A7 as level2_Id
				,NOMES.a8 as level2Name
			   ,CASE
					WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											    AND Data = cl1.ConsolidationDate
											    AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume) - ISNULL(@RESS,0)
					WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation
					WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation
					WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult
					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
					ELSE 0
				END AS Av
			   ,CASE
					WHEN IND.HashKey = 1 THEN (SELECT top 1 VOLUMEPCC From (
											SELECT ParCompany_id, SUM(Quartos) AS VOLUMEPCC
											FROM VolumePcc1b(nolock)
											WHERE 1=1 
											AND Data = cl1.ConsolidationDate
											AND ParCompany_id = cl1.UnitId
											GROUP BY ParCompany_id) Volume) - ISNULL(@RESS,0)
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
                AND ISNULL(IND.ShowScorecard, 1) = 1
				AND IND.Id = " + form.level1Id + @"
                -- AND IND.Id <> 43
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
					ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
                    AND IND.Id <> 43
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

            var unitId="";
            var unitId2="";
            var unitId3="";
            var unitId4= "";
            var unitId5 = "";
            var levelid1="";

            if (form.level1IdArr.Length > 0)
            {
                levelid1 = " AND level1_Id  IN (" + string.Join(",", form.level1IdArr) + @") ";
            }


            if (form.unitIdArr.Length > 0)
            {
                unitId  = " DECLARE @UNIDADE INT = " + form.unitId+" ";
                unitId2 = " AND CL1.UnitId IN (" + string.Join(",", form.unitIdArr) + ") ";
                unitId3 = " AND Unidade_Id IN (" + string.Join(",", form.unitIdArr) + ") ";
                unitId4 = " AND unitid IN (" + string.Join(",", form.unitIdArr) + ") ";
                unitId5 = " AND C2.UnitId IN (" + string.Join(",", form.unitIdArr) + ") ";
            }

            return @" 
 DECLARE @dataFim_ date = '" + form._dataFimSQL + @"'
  
 DECLARE @dataInicio_ date = '" + form._dataInicioSQL + @"'
SET @dataInicio_ = '" + form._dataInicioSQL + @"'
  
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
 SET @DATAINICIAL = '" + form._dataInicioSQL + @"'
 DECLARE @UNIDADE INT = " + form.unitId + @"


 SELECT 
	id,
	ConsolidationDate,
	UnitId,
	ParLevel1_Id,
	DefectsResult,
	WeiDefects,
	EvaluatedResult,
	WeiEvaluation,
	EvaluateTotal,
	TotalLevel3WithDefects,
	DefectsTotal
INTO #ConsolidationLevel1
FROM ConsolidationLevel1 WITH (NOLOCK)
WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
" + unitId4 + @"

CREATE INDEX IDX_HashConsolidationLevel1 ON #ConsolidationLevel1 (ConsolidationDate,UnitId,ParLevel1_Id); 
CREATE INDEX IDX_HashConsolidationLevel1_id ON #ConsolidationLevel1 (id); 



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
			ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1
            AND L1.Id <> 43
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
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
	" + unitId5 + @"
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
   ,SUM(ncComPeso) AS ncComPeso
   ,SUM(avComPeso) AS avComPeso
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
	   ,Nc AS ncComPeso
	   ,Av AS avComPeso
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
			   --,'Histórico do Indicador ' + NOMES.A2 AS ChartTitle
               ,'Histórico do Indicador' AS ChartTitle
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
							AND CAST(Data AS DATE) = CAST(CL1.ConsolidationDate AS DATE))
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
							AND CAST(Data AS DATE) = CAST(CL1.ConsolidationDate AS DATE))
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
			LEFT JOIN #ConsolidationLevel1 CL1
				ON DD.Data_ = CL1.ConsolidationDate
			LEFT JOIN ParLevel1 IND (NOLOCK)
				ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
				AND IND.Id = " + form.level1Id + @"
                --AND IND.Id <> 43
			LEFT JOIN ParCompany UNI (NOLOCK)
				ON UNI.Id = CL1.UnitId
				" + unitId2 + @"
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
				FROM #ConsolidationLevel1 CL1
				LEFT JOIN ParLevel1 IND (NOLOCK)
					ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
					AND IND.Id <> 43 
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
	" + levelid1 + @"
    " + unitId3 + @") ff
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

        private static string getQueryHistorioGeral(FormularioParaRelatorioViewModel form, bool enableFilter = true)
        {
            var where1 = "";
            var where2 = "";
            var where3 = "";
            var where4 = "";
            var where5 = "";
            var whereStatus = "";
            var whereClusterGroup = "";
            var whereCluster = "";


            if (form.clusterIdArr.Length > 0)
            {
                whereCluster = "" + string.Join(",", form.clusterIdArr) + "";
            }

            if (form.clusterGroupId > 0)
            {
                whereClusterGroup = "";
            }

            if (enableFilter)
            {

                #region Where1
                if (form.statusIndicador == 1)
                {
                    whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 0";
                }
                else if (form.statusIndicador == 2)
                {
                    whereStatus = "AND case when ProcentagemNc > S2.Meta then 0 else 1 end = 1";
                }

                if (form.unitIdArr.Length > 0)
                {
                    where1 += " AND C2.UnitId  IN (" + string.Join(",", form.unitIdArr) + ") ";
                }

                if (form.structureIdArr.Length > 0)
                {
                    where1 += " AND C2.UnitId IN (SELECT DISTINCT ParCompany_Id FROM ParCompanyXStructure where ParStructure_Id IN (" + string.Join(",", form.structureIdArr) + ") ) ";
                }
                #endregion
                #region Where2
                if (form.unitIdArr.Length > 0)
                {
                    where2 = " AND UNI.Id  IN (" + string.Join(",", form.unitIdArr) + ") ";
                }

                if (form.structureIdArr.Length > 0)
                {
                    where2 += " AND UNI.Id IN (SELECT ParCompany_Id FROM ParCompanyXStructure where ParStructure_Id  IN (" + string.Join(",", form.structureIdArr) + ")) ";
                }
                #endregion
                #region Where3
                if (form.level1IdArr.Length > 0)
                {
                    where3 = " AND level1_Id  IN (" + string.Join(",", form.level1IdArr) + ") ";
                }
                #endregion
                #region Where4
                if (form.unitIdArr.Length > 0)
                {
                    where4 = " AND Unidade_Id  IN (" + string.Join(",", form.unitIdArr) + ") ";
                }

                if (form.structureIdArr.Length > 0)
                {
                    where4 += " AND Unidade_Id IN (SELECT ParCompany_Id FROM ParCompanyXStructure where ParStructure_Id  IN (" + string.Join(",", form.structureIdArr) + ") ) ";
                }
                #endregion
                #region Where2
                if (form.level1IdArr.Length > 0)
                {
                    where5 = " AND IND.Id IN (" + string.Join(",", form.level1IdArr) + ") ";
                }
                #endregion
            }

            return @"
 DECLARE @dataFim_ datetime = '" + form._dataFimSQL + " 23:59:59" + @"'
  
 DECLARE @dataInicio_ datetime = '" + form._dataInicioSQL + " 00:00:00" + @"'
SET @dataInicio_ = '" + form._dataInicioSQL + " 00:00:00" + @"'
  
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
 SET @DATAINICIAL = '" + form._dataInicioSQL + @"'
 DECLARE @UNIDADE INT = " + form.unitId + @"

 CREATE TABLE #AMOSTRATIPO4a (   
 UNIDADE INT NULL,   
 INDICADOR INT NULL,   
 AM INT NULL,   
 DEF_AM INT NULL  
 )

 DECLARE @P4 INT = ISNULL((SELECT TOP 1 1 FROM PARLEVEL1 WHERE ParConsolidationType_Id = 4 AND IsActive = 1),0)


 IF (@P4 = 1) -- Pergunta se existe algum indicador ativo do tipo de consolidação 4
	
	BEGIN

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
			ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1
            AND L1.Id <> 43
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

END 

 DECLARE @RESS INT
SELECT
	@RESS =
	COUNT(1)
FROM (SELECT
		COUNT(1) AS NA
	FROM CollectionLevel2 C2 (NOLOCK)
	LEFT JOIN Result_Level3 C3 (NOLOCK)
		ON C3.CollectionLevel2_Id = C2.Id
	WHERE C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
	AND C2.ParLevel1_Id = (SELECT TOP 1
			id
		FROM Parlevel1(nolock)
		WHERE Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)
	" + where1 + @"
	AND IsNotEvaluate = 1
	GROUP BY C2.ID) NA
WHERE NA = 2

                SELECT
					IND.ID A1
				   ,IND.NAME A2
				   ,'Tendência do Indicador ' + IND.NAME AS A3
				   ,CL1.UnitId A4
				   ,UNI.NAME A5
				   ,0 AS A6
				INTO #NOMES
				FROM (SELECT
						CL1.UnitId,CL1.ParLevel1_Id
					FROM ConsolidationLevel1 CL1 (nolock)
					WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
					AND UnitId <> 11514) CL1
				LEFT JOIN ParLevel1 IND (NOLOCK)
					ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
					AND IND.Id <> 43 
				LEFT JOIN ParCompany UNI (NOLOCK)
					ON UNI.Id = CL1.UnitId
				LEFT JOIN #AMOSTRATIPO4a A4 (NOLOCK)
					ON A4.UNIDADE = UNI.Id
					AND A4.INDICADOR = IND.ID
				GROUP BY IND.ID
						,IND.NAME
						,CL1.UnitId
						,UNI.NAME

SELECT
	0 as level1Id
   ,'' as Level1Name
   ,ChartTitle
   ,0 as UnidadeId
   ,'' as UnidadeName
   ,case when SUM(av) = 0 then 0 else SUM(nc) / SUM(av) end * 100 AS procentagemNc
   ,SUM(Meta) AS Meta
   ,SUM(nc) AS nc
   ,SUM(av) av
   ,[date]
   ,SUM(ncComPeso) ncComPeso
   ,SUM(avComPeso) avComPeso
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
       ,nc AS ncComPeso
	   ,Av AS avComPeso
	   ,Data AS date
	FROM (SELECT
			S1.level1_Id
			,S1.Level1Name
			,S1.ChartTitle
			,S1.Unidade_Id
			,S1.Unidade
			,S1.IsRuleConformity
			,S1.NCSemPeso
			,S1.AvSemPeso
			,S1.NC 
			,S1.Av 
			,S1.Data
			,S1.Meta
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
			   ,'Histórico Consolidado ' AS ChartTitle
			   ,IND.IsRuleConformity
			   ,NOMES.A4 AS Unidade_Id
			   --UNI.Id  AS Unidade_Id  
			   ,NOMES.A5 AS Unidade
			   --UNI.Name     AS Unidade  
			   ,CASE
					WHEN IND.HashKey = 1 THEN (SELECT 
								SUM(Quartos)
							FROM VolumePcc1b (nolock)
							WHERE ParCompany_id = UNI.Id
							AND CAST(Data AS DATE) = CAST(CL1.ConsolidationDate AS DATE))- ISNULL(@RESS,0)
					WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation
					WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation
					WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult
					WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM
					ELSE 0
				END AS Av
			   ,CASE
					WHEN IND.HashKey = 1 THEN (SELECT 
								SUM(Quartos)
							FROM VolumePcc1b (nolock)
							WHERE ParCompany_id = UNI.Id
							AND CAST(Data AS DATE) = CAST(CL1.ConsolidationDate AS DATE)) - ISNULL(@RESS,0)
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
							AND G.AddDate <= CL1.ConsolidationDate)
						> 0 THEN (SELECT TOP 1
								ISNULL(G.PercentValue, 0)
							FROM ParGoal G (NOLOCK)
							WHERE G.ParLevel1_id = CL1.ParLevel1_Id
							AND (G.ParCompany_id = CL1.UnitId
							OR G.ParCompany_id IS NULL)
							AND G.AddDate <= CL1.ConsolidationDate
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
					CL1.UnitId,CL1.ConsolidationDate,CL1.ParLevel1_Id,CL1.WeiEvaluation,CL1.EvaluatedResult,CL1.EvaluateTotal,CL1.WeiDefects,CL1.DefectsResult,CL1.DefectsTotal
				FROM ConsolidationLevel1 CL1 (nolock)
				WHERE ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL
				AND UnitId <> 12341614) CL1
				ON DD.Data_ = CL1.ConsolidationDate
			LEFT JOIN ParLevel1 IND (NOLOCK)
				ON IND.Id = CL1.ParLevel1_Id AND ISNULL(IND.ShowScorecard, 1) = 1
				" + where5 + @"
                --AND IND.Id <> 43
			LEFT JOIN ParCompany UNI (NOLOCK)
				ON UNI.Id = CL1.UnitId
				" + where2 + @"
			LEFT JOIN #AMOSTRATIPO4a A4 (NOLOCK)
				ON A4.UNIDADE = UNI.Id
				AND A4.INDICADOR = IND.ID
			LEFT JOIN #NOMES NOMES
				ON 1 = 1
				AND (NOMES.A1 = CL1.ParLevel1_Id
				AND NOMES.A4 = UNI.ID)
				OR (IND.ID IS NULL)) S1) S2
	WHERE 1 = 1
	" + where3 + @"
    " + where4 + @"
    " + whereStatus + @"

) ff
GROUP BY ChartTitle		
		,[date]
--having sum(av) is not null or sum(nc) is not null
ORDER BY 10
DROP TABLE #AMOSTRATIPO4a
DROP TABLE #NOMES ";
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

        private string GetUserUnits(int User)
        {
            using (var db = new SgqDbDevEntities())
            {
                return string.Join(",", db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == User).Select(r => r.ParCompany_Id).ToList());
            }
        }

        [HttpPost]
        [Route("GetAcoesByDate")]
        public List<AcoesConcluidas> GetAcoesByDate(JObject filtro)
        {
            try
            {
                var BancoPA = "PlanoDeAcao";
                //const BancoPA = "PlanoDeAcaoUSA2";  

                dynamic filtroDyn = filtro;
                var retorno = new List<AcoesConcluidas>();
                string inicio = filtroDyn.startDate;
                string fim = filtroDyn.endDate;
                int level = filtroDyn.isLevel;
                var dataInicio = Guard.ParseDateToSqlV2(inicio, Guard.CultureCurrent.BR).ToString("yyyyMMdd");
                var dataFim = Guard.ParseDateToSqlV2(fim, Guard.CultureCurrent.BR).ToString("yyyyMMdd");
                //var dataInicio = Guard.ParseDateToSqlV2(inicio, Guard.CultureCurrent.EUA).ToString("yyyyMMdd");
                //var dataFim = Guard.ParseDateToSqlV2(fim, Guard.CultureCurrent.EUA).ToString("yyyyMMdd");

                string unidade = filtroDyn.unitId;
                string level1Id = filtroDyn.level1Id;
                string level2Id = filtroDyn.level2Id;
                string level3Id = filtroDyn.level3Id;

                var whereLevel = "";

                if (level == 3)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                    whereLevel += "AND pa.Level2Id = " + level2Id;
                    whereLevel += "AND pa.Level3Id = " + level3Id;
                }
                else if (level == 2)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                    whereLevel += "AND pa.Level2Id = " + level2Id;
                }
                else if (level == 1)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                }

                var query = @" 
                Use " + BancoPA + @"
                DECLARE @dataFim_ date = '" + dataFim + @"'
  
                 DECLARE @dataInicio_ date = '" + dataInicio + @"'
                SET @dataInicio_ = '" + dataInicio + @"'
                  
                 CREATE TABLE #ListaDatas_ (data_ date)
                  
                 WHILE @dataInicio_ <= @dataFim_  
                 BEGIN
                INSERT INTO #ListaDatas_
                
                	SELECT
                		@dataInicio_
                SET @dataInicio_ = DATEADD(DAY, 1, @dataInicio_)
                  
                 END
                
                SELECT
                	LD.data_ as Data
                   ,COUNT(DISTINCT PA.Id) QteConcluidas
                FROM #ListaDatas_ LD
                
                LEFT JOIN (SELECT
                		Acao_id
                	   ,MAX(AddDate) Max_Date
                	   ,COUNT(DISTINCT Acao_id) QteAcao
                	FROM Pa_Acompanhamento
                	WHERE Status_Id IN (3, 4)
                	GROUP BY Acao_id) PC
                	ON LD.data_ = CAST(PC.Max_Date AS DATE)
                LEFT JOIN (SELECT
                		*
                	FROM Pa_Acao PA
                	WHERE PA.Status IN (3, 4)
                	" + whereLevel + @"
                	AND PA.Unidade_Id = " + unidade + @") PA
                	ON PC.Acao_Id = PA.Id
                GROUP BY LD.data_
                order by ld.data_

                DROP TABLE #ListaDatas_";


                using (var db = new SgqDbDevEntities())
                {
                    retorno = db.Database.SqlQuery<AcoesConcluidas>(query).ToList();
                }

                return retorno;
            }
            catch (Exception e)
            {
                return new List<AcoesConcluidas>();
            }

        }

        [HttpPost]
        [Route("GetAcoesIndicador")]
        public List<Pa_Acao> GetAcoesIndicador(JObject filtro)
        {
            try
            {

                var BancoPA = "PlanoDeAcao";
                //const BancoPA = "PlanoDeAcaoUSA2"; 

                dynamic filtroDyn = filtro;
                var retorno = new List<Pa_Acao>();
                DateTime dataConclusao = filtroDyn.Data;
                int level = filtroDyn.isLevel;
                //var dataConclusao = Guard.ParseDateToSqlV2(data, Guard.CultureCurrent.EUA).ToString("yyyyMMdd");

                string unidade = filtroDyn.unitId;
                string level1Id = filtroDyn.level1Id;
                string level2Id = filtroDyn.level2Id;
                string level3Id = filtroDyn.level3Id;

                var whereLevel = "";

                if (level == 3)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                    whereLevel += "AND pa.Level2Id = " + level2Id;
                    whereLevel += "AND pa.Level3Id = " + level3Id;
                }
                else if (level == 2)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                    whereLevel += "AND pa.Level2Id = " + level2Id;
                }
                else if (level == 1)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                }

                var query = @"
                                Use " + BancoPA + @"
                                SELECT
                                	ISNULL(PA.UnidadeName,'') as 'UnidadeName'
                               ,ISNULL(PA.Level1Name,'') as 'Level1Name'
                               ,ISNULL(PA.Level2Name,'') as 'Level2Name'
                               ,ISNULL(PA.Level3Name,'') as 'Level3Name'
                               ,ISNULL(PC.Max_Date, '') as 'Max_Date'
                               ,ISNULL(PA.QuandoInicio,'') as 'QuandoInicio'
                               ,ISNULL(PA.QuandoFim,'') as 'QuandoFim'
                            	--,'Como' as 'Como'
                               ,PA.QuantoCusta
                               ,ISNULL(S.Name,'') AS '_StatusName'
                               ,ISNULL(U.Name,'') AS '_Quem'
                               ,ISNULL(CG.CausaGenerica,'') AS '_CausaGenerica'
                               ,ISNULL(GC.GrupoCausa,'') AS '_GrupoCausa'
                               ,ISNULL(CMG.ContramedidaGenerica,'') AS '_ContramedidaGenerica'
                            --,'Assunto' as 'Assunto' 
                            --,'O que' as 'O que'
                            --,'Observacao' as 'Observacao'
                            FROM (SELECT
                            		*
                            	FROM Pa_Acao PA
                            	WHERE PA.Status IN (3, 4)
                            	" + whereLevel + @"
                            	AND PA.Unidade_Id = " + unidade + @") PA
                            INNER JOIN (SELECT
                            		Acao_id
                            	   ,MAX(AddDate) Max_Date
                            	   ,COUNT(DISTINCT Acao_id) QteAcao
                            	FROM Pa_Acompanhamento
                            	WHERE Status_Id IN (3, 4)
                            	AND CAST(AddDate AS DATE) = '" + dataConclusao.ToString("yyyyMMdd") + @"'
                            	GROUP BY Acao_id) PC
                            	ON PC.Acao_Id = PA.Id
                            LEFT JOIN Pa_Quem U
                            	ON PA.Quem_Id = U.Id
                            LEFT JOIN Pa_CausaGenerica CG
                            	ON PA.CausaGenerica_Id = CG.id
                            LEFT JOIN Pa_GrupoCausa GC
                            	ON GC.Id = PA.GrupoCausa_Id
                            LEFT JOIN Pa_ContramedidaGenerica CMG
                            	ON CMG.Id = PA.ContramedidaGenerica_Id
                            LEFT JOIN Pa_Status S
                            	ON S.Id = PA.Status";

                using (var db = new SgqDbDevEntities())
                {
                    retorno = db.Database.SqlQuery<Pa_Acao>(query).ToList();
                }

                return retorno;
            }
            catch (Exception e)
            {
                return new List<Pa_Acao>();
            }

        }

        public class AcoesConcluidas
        {
            public DateTime Data { get; set; }
            public int QteConcluidas { get; set; }
            public string _data { get { return Data.ToString("dd/MM/yyyy"); } }
        }

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
        public decimal? Av { get; set; }
        public decimal? Nc { get; set; }
        public decimal? avComPeso { get; set; }
        public decimal? ncComPeso { get; set; }
        public decimal? Pc { get; set; }
        public string Historico_Id { get; set; }
        public decimal? Meta { get; set; }
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

        public decimal? avComPeso { get; set; }
        public decimal? ncComPeso { get; set; }

        public decimal? av { get; set; }
        public string ChartTitle { get; set; }
        public decimal? companyScorecard { get; set; }
        public string companySigla { get; set; }
        public DateTime? date { get; set; }
        public int level1Id { get; set; }
        public string level1Name { get; set; }
        public int level2Id { get; set; }
        public string level2Name { get; set; }
        public int level3Id { get; set; }
        public string level3Name { get; set; }
        public decimal? nc { get; set; }
        public decimal? procentagemNc { get; set; }
        public int regId { get; set; }
        public string regName { get; set; }
        public decimal? scorecard { get; set; }
        public decimal? scorecardJbs { get; set; }
        public decimal? scorecardJbsReg { get; set; }
        public string _date
        {
            get
            {
                if (date.HasValue)
                {
                    return date.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public bool haveHistorico { get; set; }
        public decimal? limiteInferior { get; set; }
        public decimal? limiteSuperior { get; set; }
        public string levelName { get; set; }
        public string UnidadeName { get; set; }
        public string HISTORICO_ID { get; set; }
        public int? IsPaAcao { get; set; }
        public decimal? Meta { get; set; }
        public string _dateEUA
        {
            get
            {

                if (date.HasValue)
                {
                    return date.Value.ToString("yyyy-MM-dd");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }

    //public class RetornoSugestao
    //{

    //    public string Unidade { get; set; }
    //    public string UnidadeNome { get; set; }
    //    public DateTime? DataInicio_ { get; set; }
    //    public string DataInicio { get; set; }
    //    //{
    //    //    get
    //    //    {

    //    //        if (DataInicio_.HasValue)
    //    //        {
    //    //            return DataInicio_.Value.ToString("dd/MM/yyyy HH:mm:ss");
    //    //        }
    //    //        else
    //    //        {
    //    //            return string.Empty;
    //    //        }
    //    //    }
    //    //}
    //    public DateTime? DataFim_ { get; set; }
    //    public string DataFim { get; set; }
    //    //{
    //    //    get
    //    //    {
    //    //        if (DataFim_.HasValue)
    //    //        {
    //    //            return DataFim_.Value.ToString("dd/MM/yyyy HH:mm:ss");
    //    //        }
    //    //        else
    //    //        {
    //    //            return string.Empty;
    //    //        }
    //    //    }
    //    //}
    //    public string Indicador { get; set; }
    //    public string IndicadorNome { get; set; }
    //    public string Monitoramento { get; set; }
    //    public string MonitoramentoNome { get; set; }
    //    public string Tarefa { get; set; }
    //    public string TarefaNome { get; set; }
    //    public string Usuario { get; set; }
    //    public string UsuarioNome { get; set; }
    //    public DateTime? DataConsulta { get; set; }
    //}

    //public class RetornoLogJson
    //{
    //    public DateTime? AddDate { get; set; }
    //    public string result { get; set; }
    //    public string callback { get; set; }
    //}

    /*
     
	"Unidade" : "37"
	"UnidadeNome" : "Pontes e Lacerda"
	"DataInicio" :	"03/04/2017 00:00:00"
	"DataFim" : "03/04/2017 00:00:00"
	"Indicador" : "0"
	"IndicadorNome" : ""
	"Monitoramento" : "0"
	"MonitoramentoNome" : ""
	"Tarefa" : "0"
	"TarefaNome" : ""
	"Usuario" : "1"
	"UsuarioNome": "camilaprata-mtz"
     */

}

using Dominio;
using DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Relatorios
{
    public class Data
    {
        public List<JObject> Resultado { get; set; }

       public Object Aprovador { get; set; }
       public Object Elaborador { get; set; }
       public Object RelatorioName { get; set; }
    }

    [RoutePrefix("api/Formularios")]
    public class FormulariosApiController : BaseApiController
    {
        [Route("Get")]
        [HttpPost]
        public IHttpActionResult GetJsonFormulario([FromBody] DTO.DataCarrierFormularioNew form)
        {
            // List<JObject> resultado = new List<JObject>();

            Data retorno = new Data();

            var indicador_Id = "0";

            if (form.ShowIndicador_Id.Length > 0)
                 indicador_Id = form.ShowIndicador_Id[0].ToString();

      

            using (var db = new SgqDbDevEntities())
            {
                retorno.Aprovador = getAprovadorName(form, db);

                retorno.Elaborador = getElaboradorName(form, db);

                retorno.RelatorioName = getRelatorioName(form, db);
            }
            var query = $@"
            	
---------------------------------------------------------------------------------------------------------------------------------	
				
-------------------------------------------------------------------------------------------------------------------------

-- TRATAMENTO INPUTS BINARIOS
-- binarios 1,6-- Retorna input
-- numeros 2,3,4,5,7,8,9,10,11,13,14,99 -- Retorna o Value
--CREATE TABLE #INPUT_TYPES
--(
--	ParLevel3InputType_Id INT,
--	Sentido INT,
--	ParLevel3Bool_Id INT,
--	SIGLA VARCHAR(10)
--)

--INSERT INTO #INPUT_TYPES
--SELECT 1,0,1,'NC' UNION ALL
--SELECT 1,0,2,'PRT' UNION ALL
--SELECT 1,0,3,'NÃO' UNION ALL
--SELECT 1,0,4,'PRS' UNION ALL
--SELECT 1,0,5,'NA' UNION ALL
--SELECT 1,0,6,'SIM' UNION ALL
--SELECT 1,1,1,'C' UNION ALL
--SELECT 1,1,2,'AUT' UNION ALL
--SELECT 1,1,3,'SIM' UNION ALL
--SELECT 1,1,4,'AUS' UNION ALL
--SELECT 1,1,5,'AC' UNION ALL
--SELECT 1,1,6,'NÃO' UNION ALL

--SELECT 6,0,1,'NC' UNION ALL
--SELECT 6,0,2,'PRT' UNION ALL
--SELECT 6,0,3,'NÃO' UNION ALL
--SELECT 6,0,4,'PRS' UNION ALL
--SELECT 6,0,5,'NA' UNION ALL
--SELECT 6,0,6,'SIM' UNION ALL
--SELECT 6,1,1,'C' UNION ALL
--SELECT 6,1,2,'AUT' UNION ALL
--SELECT 6,1,3,'SIM' UNION ALL
--SELECT 6,1,4,'AUS' UNION ALL
--SELECT 6,1,5,'AC' UNION ALL
--SELECT 6,1,6,'NÃO'

--ID	Descrição		Sigla		ID	Descrição	Sigla
--1		Não Conforme	NC			1	Conforme	C
--2		Presente		PRT			2	Ausente		AUT
--3		Não				NÃO			3	Sim			SIM
--4		Presença		PRS			4	Ausência	AUS
--5		Não Aceitável	NA			5	Aceitável	AC
--6		Sim				SIM			7	Não			NÃO

--ID	Descrição	Sigla
--1	Conforme		C	
--2	Ausente			AUT
--3	Sim				SIM
--4	Ausência		AUS
--5	Aceitável		AC
--7	Não				NÃO



--SELECT 
--	CASE
--		WHEN L3V.ParLevel3InputType_Id in (2,3,4,5,7,8,9,10,11,13,14,99)
--		THEN R.Value
--		WHEN L3V.ParLevel3InputType_Id in (1,6)
--		THEN (--1
--					SELECT TOP 1 IT.SIGLA
--					FROM #INPUT_TYPES IT
--					WHERE 1=1 
--					AND IT.ParLevel3InputType_Id = 1
--					AND IT.Sentido = CAST(R.IsConform AS INT)
--					AND IT.ParLevel3Bool_Id = IIF(R.IsConform = 1,L3V.ParLevel3BoolTrue_Id,ParLevel3BoolFalse_Id)  ) END
--	Resultado,L3V.*,* 
--	FROM CollectionLevel2 C
--	LEFT JOIN Result_Level3 R 
--		ON C.ID = R.CollectionLevel2_Id
--OUTER APPLY (SELECT TOP 1
--				*
--			FROM ParLevel3Value L3V WITH (NOLOCK)
--			WHERE 1=1
--			AND ISNULL(L3V.ParCompany_Id,C.UnitId)  = C.UnitId
--			AND ISNULL(L3V.ParLevel1_Id,C.ParLevel1_Id)  = C.ParLevel1_Id
--			AND ISNULL(L3V.ParLevel2_Id,C.ParLevel2_Id)  = C.ParLevel2_Id
--			AND L3V.ParLevel3_Id  = R.ParLevel3_Id
--			AND L3V.IsActive = 1
--			ORDER BY 3 DESC,2,1) AS L3V
--	WHERE 1=1
--	AND CollectionDate BETWEEN '2020-05-22' AND '2020-05-23'
--	AND C.ParLevel1_Id = 46




	
---------------------------------------------------------------------------------------------------------------------------------	
				
-------------------------------------------------------------------------------------------------------------------------

    
          DECLARE @DATEINI DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}' DECLARE @DATEFIM DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}';
		 DECLARE @UNITID VARCHAR(10) = '{ form.ParCompany_Ids[0]}', @PARLEVEL1_ID VARCHAR(10) = '{indicador_Id}',@PARLEVEL2_ID VARCHAR(10) = '0';



		 -------------------------------------------------------------------------------------------------------------------------
		 -------------------------------------------------------------------------------------------------------------------------		 
		   
		 DECLARE @DATAINICIAL DATETIME = @DATEINI;
		 DECLARE @DATAFINAL DATETIME = @DATEFIM;                     

		CREATE TABLE #INPUT_TYPES
		(
			ParLevel3InputType_Id INT,
			Sentido INT,
			ParLevel3Bool_Id INT,
			SIGLA VARCHAR(10)
		)

		INSERT INTO #INPUT_TYPES
		SELECT 1,0,1,'NC' UNION ALL
		SELECT 1,0,2,'PRT' UNION ALL
		SELECT 1,0,3,'NÃO' UNION ALL
		SELECT 1,0,4,'PRS' UNION ALL
		SELECT 1,0,5,'NA' UNION ALL
		SELECT 1,0,6,'SIM' UNION ALL
		SELECT 1,1,1,'C' UNION ALL
		SELECT 1,1,2,'AUT' UNION ALL
		SELECT 1,1,3,'SIM' UNION ALL
		SELECT 1,1,4,'AUS' UNION ALL
		SELECT 1,1,5,'AC' UNION ALL
		SELECT 1,1,6,'NÃO' UNION ALL

		SELECT 6,0,1,'NC' UNION ALL
		SELECT 6,0,2,'PRT' UNION ALL
		SELECT 6,0,3,'NÃO' UNION ALL
		SELECT 6,0,4,'PRS' UNION ALL
		SELECT 6,0,5,'NA' UNION ALL
		SELECT 6,0,6,'SIM' UNION ALL
		SELECT 6,1,1,'C' UNION ALL
		SELECT 6,1,2,'AUT' UNION ALL
		SELECT 6,1,3,'SIM' UNION ALL
		SELECT 6,1,4,'AUS' UNION ALL
		SELECT 6,1,5,'AC' UNION ALL
		SELECT 6,1,6,'NÃO'



                    SELECT 
	                     id
	                    ,ParLevel1_Id
	                    ,ParLevel2_Id
	                    ,UnitId
	                    ,CollectionDate
	                    ,EvaluationNumber
	                    ,Sample
	                    ,Sequential
	                    ,Side
	                    ,Shift
	                    ,Period
	                    ,AuditorId
	                    ,AddDate
	                    ,AlterDate 
                    INTO #CollectionLevel2
                    FROM collectionlevel2 CL2
                        WHERE 1=1
						AND cl2.Collectiondate BETWEEN @DATEINI AND @DATEFIM
						AND CL2.ParLevel1_Id != 43
						AND CL2.ParLevel1_Id != 42
						AND CASE WHEN @UNITID = '0' THEN '0' ELSE cl2.unitid END = @UNITID
						AND CASE WHEN @PARLEVEL1_ID = '0' THEN '0' ELSE cl2.ParLevel1_id END = @PARLEVEL1_ID
						AND CASE WHEN @PARLEVEL2_ID = '0' THEN '0' ELSE cl2.ParLevel2_id END = @PARLEVEL2_ID

 
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
FROM CollectionLevel2XParHeaderField cl2xph_ 
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
FROM CollectionLevel2XParHeaderField CL2HF2 (nolock) 
inner join #collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id
left join ParHeaderField HF (nolock)on CL2HF2.ParHeaderField_Id = HF.Id
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
        
        SELECT V.ParCompany_id,V.Data
        	, SUM(V.Quartos) AS VOLUMEPCC
        INTO #VOLUMES
        FROM VolumePcc1b V WITH (NOLOCK)
        WHERE 1=1 
        GROUP BY V.ParCompany_id,V.Data
        
        
        
        	SELECT
        		UNIDADE
        	   ,INDICADOR
        	   ,DATA
        	   ,COUNT(1) AM
        	   ,SUM(DEF_AM) DEF_AM
        	INTO #AMOSTRA4
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
        		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATEINI AND @DATEFIM
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
        
        -- NA
        
        SELECT CL2.CollectionDate,CL2.UnitId,COUNT(distinct CL2.id) AS NA
        	INTO #NA
        	FROM CollectionLevel2 CL2 WITH (NOLOCK)
        	LEFT JOIN Result_Level3 CL3 WITH (NOLOCK)
        		ON CL3.CollectionLevel2_Id = CL2.Id
        	WHERE CONVERT(DATE, CL2.CollectionDate) between CONVERT(DATE,@DATEINI) and CONVERT(DATE,@DATEFIM)
        	AND CL2.ParLevel1_Id IN (SELECT 
        			id
        		FROM Parlevel1 WITH (NOLOCK)
        		WHERE Hashkey = 1 )
        	AND CL3.IsNotEvaluate = 1
        	GROUP BY CL2.CollectionDate,CL2.UnitId
        HAVING COUNT(DISTINCT CL2.id) > 1
        
        -- C1
        
        SELECT 
        	CL2.id,
        	CL2.CollectionDate  AS ConsolidationDate,
        	CL2.UnitId,
        	CL2.AuditorId,
			CL2.EvaluationNumber,
			CL2.Sample,
        	CL2.ParLevel1_Id,
        	CL2.ParLevel2_Id,
        	R3.ParLevel3_Id,
			CASE
				WHEN L3V.ParLevel3InputType_Id in (2,3,4,5,7,8,9,10,11,13,14,99)
				THEN R3.Value
				WHEN L3V.ParLevel3InputType_Id in (1,6)
				THEN (--1
							SELECT TOP 1 IT.SIGLA
							FROM #INPUT_TYPES IT
							WHERE 1=1 
							AND IT.ParLevel3InputType_Id = L3V.ParLevel3InputType_Id
							AND IT.Sentido = CAST(R3.IsConform AS INT)
							AND IT.ParLevel3Bool_Id = IIF(R3.IsConform = 1,L3V.ParLevel3BoolTrue_Id,L3V.ParLevel3BoolFalse_Id)  ) END Resultado,
        	R3.WeiDefects,
        	R3.Defects,
        	R3.WeiEvaluation,
        	R3.Evaluation
        INTO #ConsolidationLevel
        FROM #CollectionLevel2 CL2 WITH (NOLOCK) 
        LEFT JOIN Result_Level3 R3 WITH (NOLOCK) 
        	ON CL2.ID = R3.CollectionLevel2_Id
		OUTER APPLY (SELECT TOP 1
						L3V.ParLevel3InputType_Id,L3V.ParLevel3BoolTrue_Id,L3V.ParLevel3BoolFalse_Id
					FROM ParLevel3Value L3V WITH (NOLOCK)
					WHERE 1=1
					AND ISNULL(L3V.ParCompany_Id,CL2.UnitId)  = CL2.UnitId
					AND ISNULL(L3V.ParLevel1_Id,CL2.ParLevel1_Id)  = CL2.ParLevel1_Id
					AND ISNULL(L3V.ParLevel2_Id,CL2.ParLevel2_Id)  = CL2.ParLevel2_Id
					AND L3V.ParLevel3_Id  = R3.ParLevel3_Id
					AND L3V.IsActive = 1
					ORDER BY 3 DESC,2,1) AS L3V
        WHERE 1=1 
        AND CL2.CollectionDate BETWEEN @DATEINI AND @DATEFIM
        --AND R3.IsNotEvaluate = 0      

        CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id); 
        CREATE INDEX IDX_HashConsolidationLevel_level3 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id); 
        CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId); 
        CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id); 

        
        -- CUBO

        SELECT 
        	 C1.ID
			,C1.AuditorId
			,U.Name AS AuditorName
			,C1.EvaluationNumber
			,C1.Sample
			,CL.id						AS ParCluster_ID
        	,CL.Name					AS ParCluster_Name
        	,CS.ParStructure_id			AS ParStructure_id
        	,S.Name						AS ParStructure_Name
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
        	,L1C.ParCriticalLevel_Id	AS ParCriticalLevel_Id
        	,CRL.Name					AS ParCriticalLevel_Name
        	,L1.IsRuleConformity
        	,CASE 
        		WHEN L1.hashKey = 1 THEN ISNULL((SELECT top 1 SUM(VOLUMEPCC) From #VOLUMES V WITH (NOLOCK)
        											WHERE 1=1 
        											AND V.Data = c1.ConsolidationDate
        											AND V.ParCompany_id = c1.UnitId
        										) ,0)/2
        											-
        										ISNULL((SELECT SUM(NA) AS NA FROM #NA NA WHERE NA.UnitId = C1.UnitId AND NA.CollectionDate = C1.ConsolidationDate),0)
        		ELSE SUM(WeiEvaluation)
        	END AS [AVComPeso]
        	,SUM(WeiDefects) AS [nCComPeso]
        	,CASE 
        		WHEN L1.hashKey = 1 THEN ISNULL((SELECT top 1 SUM(VOLUMEPCC) From #VOLUMES V WITH (NOLOCK)
        										WHERE 1=1 
        										AND V.Data = c1.ConsolidationDate
        										AND V.ParCompany_id = c1.UnitId
        									),0)
        									-
        									ISNULL((SELECT SUM(NA) AS NA FROM #NA NA WHERE NA.UnitId = C1.UnitId AND NA.CollectionDate = C1.ConsolidationDate),0)
        		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(WeiEvaluation)
        		ELSE SUM(Evaluation)
        	 END AS [AV]
        	,CASE
        		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(WeiDefects)
        		ELSE SUM(Defects)
        	END AS [NC]
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
				,Resultado
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
        
        	LEFT JOIN ParCompany PC WITH (NOLOCK)
         		ON PC.Id = C1.Unitid
        
        	INNER JOIN ParCompanyCluster CCL WITH (NOLOCK)
        		ON CCL.ParCompany_id = PC.id 
        		AND CCL.Active = 1
        
        	INNER JOIN ParLevel1XCluster L1C WITH (NOLOCK)
        		ON CCL.ParCluster_ID = L1C.ParCluster_ID 
         		AND C1.ParLevel1_Id = L1C.ParLevel1_Id 
             	AND L1C.Id = (select top 1 aaa.ID from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CCL.ParCluster_Id AND aaa.EffectiveDate <  @DATAFINAL AND Isactive = 1 ORDER BY ParLevel1_Id,ParCluster_Id,EffectiveDate,AddDate,AlterDate)
         		AND L1C.IsActive = 1
        
        	INNER JOIN ParCompanyXStructure CS WITH (NOLOCK)
        		ON PC.Id = CS.ParCompany_Id 
        		AND CS.Active = 1
        
        	INNER JOIN ParStructure S WITH (NOLOCK)
        		ON CS.ParStructure_id = S.id 
        		AND S.Active = 1
        
        	INNER JOIN ParCluster CL WITH (NOLOCK)
        		ON L1C.ParCluster_ID = CL.ID 
        		AND CL.IsActive = 1
        
        	INNER JOIN ParStructureGroup SG WITH (NOLOCK)
        		ON S.ParStructureGroup_Id = SG.ID 
        		-- AND SG.ID = 2 
        	
        	LEFT JOIN ParScoreType ST WITH (NOLOCK)
        		ON L1.ParConsolidationType_Id = ST.Id 
        		AND ST.IsActive = 1
        
        	LEFT JOIN ParCriticalLevel CRL WITH (NOLOCK)
        		ON L1C.ParCriticalLevel_id = CRL.id 
        		AND CRL.IsActive = 1
			LEFT JOIN UserSGQ U
				ON C1.AuditorId = U.ID
        
        GROUP BY
        	 CL.id						
        	,CL.Name					
        	,CS.ParStructure_id			
        	,S.Name						
        	,C1.UnitId 	
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
        	,L1C.ParCriticalLevel_Id	
        	,CRL.Name
        	,L1.IsRuleConformity
			,C1.ID
			,C1.AuditorId
			,U.Name
			,C1.EvaluationNumber
			,C1.Sample
			,Resultado
        
            update #CUBO set Meta = iif(IsRuleConformity = 0,Meta, (100 - Meta)) 

			SELECT 
                IndicadorName as Indicador,
                MonitoramentoName as Setor,
                TarefaName as ''Itens Verificados'',
				AuditorId,
				AuditorName as Visto,
				EvaluationNumber as ''Avaliação'',
				Sample as Amostra,
                H.*,
                Meta as Meta,
                --AVComPeso as ''AV com Peso'',
                --nCComPeso as ''NC com Peso'',
                UnidadeName as Unidade,
                Cast(AV as int) as AV,
                Cast(NC as int) as NC,
				Resultado,
                -- iif(NC = 1, ''C'' , ''NC'') as ''Resultado'',
                --''Célia Regina Mattia GQC/ANH'' as Visto,
                CONVERT(VARCHAR(10), ConsolidationDate, 103) as ''Data da Coleta'',
				CONVERT(CHAR(5),ConsolidationDate,108) as ''Hora''

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
            DROP TABLE #INPUT_TYPES

            ";

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                retorno.Resultado = QueryNinja(dbSgq, query);
            }
            return Ok(retorno);
        }

        private object getElaboradorName(DataCarrierFormularioNew form, SgqDbDevEntities db)
        {
            var whereCompany = "";
            if(form.ParCompany_Ids.Length > 0)
            {
                whereCompany = $"AND RXU.Parcompany_Id = { form.ParCompany_Ids[0]}";
            }

            var SQL = $@"SELECT top 1
                    Elaborador
                    FROM ReportXUserSgq RXU      
                    WHERE RXU.ParLevel1_Id = {form.ShowIndicador_Id[0]}
                    {whereCompany} 
                    OR RXU.Parcompany_Id IS NULL
                    Order by RXU.Parcompany_Id desc";

            return QueryNinja(db, SQL);
        }

        private object getRelatorioName(DataCarrierFormularioNew form, SgqDbDevEntities db)
        {
            var whereCompany = "";
            if (form.ParCompany_Ids.Length > 0)
            {
                whereCompany = $"AND RXU.Parcompany_Id = { form.ParCompany_Ids[0]}";
            }

            var SQL = $@"SELECT top 1
                    NomeRelatorio
                    FROM ReportXUserSgq RXU      
                    WHERE RXU.ParLevel1_Id = {form.ShowIndicador_Id[0]}
                    {whereCompany} 
                    OR RXU.Parcompany_Id IS NULL
                    Order by RXU.Parcompany_Id desc";


            return QueryNinja(db, SQL);
        }

        private object getAprovadorName(DataCarrierFormularioNew form, SgqDbDevEntities db)
        {
            var whereCompany = "";
            if (form.ParCompany_Ids.Length > 0)
            {
                whereCompany = $"AND RXU.Parcompany_Id = { form.ParCompany_Ids[0]}";
            }
            var SQL = $@"SELECT top 1
                    Aprovador
                    FROM ReportXUserSgq RXU      
                    WHERE RXU.ParLevel1_Id = {form.ShowIndicador_Id[0]}
                    {whereCompany} 
                    OR RXU.Parcompany_Id IS NULL
                    Order by RXU.Parcompany_Id desc";

            return QueryNinja(db, SQL)[0]["Aprovador"];
        }
    }
}

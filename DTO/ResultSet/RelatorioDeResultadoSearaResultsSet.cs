using DTO;
using System;
using System.Linq;

public class RelatorioDeResultadoSearaResultsSet
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
    public decimal? C { get; set; }
    public decimal? Nc { get; set; }
    public decimal? Av { get; set; }
    public decimal? Meta { get; set; }
    public decimal? PorcNc { get; internal set; }
    public decimal? PorcC { get; internal set; }
    public int? Shift { get; set; }
    public string dataX { get; set; }
    public string Data { get; set; }

    public string ParCompany_Id { get; set; }
    public string ParLevel1_Id { get; set; }
    public string ParLevel2_Id { get; set; }
    public decimal? PESOTOTAL { get; set; }
    public decimal? TOTAL { get; set; }



	public string Empresa { get; set; }
	public string Turno { get; set; }
	public string Familia { get; set; }
	public string SKU { get; set; }
	public string CABECALHO { get; set; }
	public string Grupo { get; set; }
	public decimal? RESPOSTA2 { get; set; }
	public decimal? NotaFamilia { get; set; }
	public decimal? NotaGrupo { get; set; }



	public string SelectUnidadesSeara(DataCarrierFormularioNew form)
    {

        var whereStructure = "";
        var whereUnit = "";
        var whereTurno = "";
        var whereParLevel1 = "";
        var whereParLevel2 = "";
        var whereParLevel3 = "";


        if (form.ParLevel1_Ids.Length > 0)
        {
            whereParLevel1 = $@" AND Parlevel1_Id in ({string.Join(",", form.ParLevel1_Ids)}) ";
        }

        if (form.ParLevel2_Ids.Length > 0)
        {
            whereParLevel2 = $@" AND Parlevel2_Id in ({string.Join(",", form.ParLevel2_Ids)}) ";
        }

        if (form.ParLevel3_Ids.Length > 0)
        {
            whereParLevel3 = $@" AND R3.Parlevel3_Id in ({string.Join(",", form.ParLevel3_Ids)}) ";
        }

        if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
        {
            whereUnit = $@"AND UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
        }

        if (form.ShowTurnoSeara[0].ToString() != "-1")
        {
            whereTurno = $@"AND HFTURNO.HeaderFieldList LIKE '%{ form.ShowTurnoSeara[0] }%' ";
        }


        if (form.ParStructure2_Ids.Length > 0)
        {
            whereStructure = $@"AND CS.ParStructure_Id in ({string.Join(",", form.ParStructure2_Ids)})";
        }

        var query = $@"

            DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
            DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

			SELECT * INTO #Collectionlevel2 
			FROM CollectionLevel2 C2 WITH (NOLOCK) 
			WHERE CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
			
            {whereUnit}
			
            {whereParLevel1}
            {whereParLevel2}
            

			-- Criação da Fato de Cabeçalhos
			SELECT
				CL2HF.Id
				,CL2HF.CollectionLevel2_Id
				,CL2HF.ParHeaderFieldGeral_Id
				,CL2HF.ParFieldType_Id
				,CL2HF.Value INTO #CollectionLevel2XParHeaderFieldGeral
			FROM CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN #Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel_ID ON #CollectionLevel2XParHeaderFieldGeral (CollectionLevel2_Id);

			-- Concatenação da Fato de Cabeçalhos
			SELECT
				CL2HF.CollectionLevel2_Id
				,STUFF((SELECT DISTINCT
						', ' + CONCAT(HF.Name, ': ', CASE
							WHEN CL2HF2.ParFieldType_Id = 1 OR
								CL2HF2.ParFieldType_Id = 3 THEN PMV.Name
							WHEN CL2HF2.ParFieldType_Id = 2 THEN CASE
									WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
									ELSE EQP.Nome
								END
							WHEN CL2HF2.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CL2HF2.Value, 103)
							ELSE CL2HF2.Value
						END)
					FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (NOLOCK)
					LEFT JOIN collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF2.CollectionLevel2_Id
					LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
						ON CL2HF2.ParHeaderFieldGeral_Id = HF.Id
					LEFT JOIN ParLevel2 L2 (NOLOCK)
						ON L2.Id = CL2.ParLevel2_Id
					LEFT JOIN ParMultipleValuesGeral PMV (NOLOCK)
						ON CL2HF2.Value = CAST(PMV.Id AS VARCHAR(500))
						AND CL2HF2.ParFieldType_Id <> 2
					LEFT JOIN Equipamentos EQP (NOLOCK)
						ON CAST(EQP.Id AS VARCHAR(500)) = CL2HF2.Value
						AND EQP.ParCompany_Id = CL2.UnitId
						AND CL2HF2.ParFieldType_Id = 2
					LEFT JOIN Produto PRD WITH (NOLOCK)
						ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CL2HF2.Value
						AND CL2HF2.ParFieldType_Id = 2
					WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
					FOR XML PATH (''))
				, 1, 1, '') AS HeaderFieldList INTO #CollectionLevel2XParHeaderFieldGeral2
			FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id
			LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
				ON CL2HF.ParHeaderFieldGeral_Id = HF.Id
			LEFT JOIN ParLevel2 L2 (NOLOCK)
				ON L2.Id = CL2.ParLevel2_Id
			GROUP BY CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeral2 (CollectionLevel2_Id);
					
			-- Concatenação da Fato de Cabeçalhos TURNO
			SELECT
				CL2HF.CollectionLevel2_Id
				,STUFF((SELECT DISTINCT
						', ' + CONCAT(HF.Name, ': ', CASE
							WHEN CL2HF2.ParFieldType_Id = 1 OR
								CL2HF2.ParFieldType_Id = 3 THEN PMV.Name
							WHEN CL2HF2.ParFieldType_Id = 2 THEN CASE
									WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
									ELSE EQP.Nome
								END
							WHEN CL2HF2.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CL2HF2.Value, 103)
							ELSE CL2HF2.Value
						END)
					FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (NOLOCK)
					LEFT JOIN collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF2.CollectionLevel2_Id
					LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
						ON CL2HF2.ParHeaderFieldGeral_Id = HF.Id 
					LEFT JOIN ParLevel2 L2 (NOLOCK)
						ON L2.Id = CL2.ParLevel2_Id
					LEFT JOIN ParMultipleValuesGeral PMV (NOLOCK)
						ON CL2HF2.Value = CAST(PMV.Id AS VARCHAR(500))
						AND CL2HF2.ParFieldType_Id <> 2
					LEFT JOIN Equipamentos EQP (NOLOCK)
						ON CAST(EQP.Id AS VARCHAR(500)) = CL2HF2.Value
						AND EQP.ParCompany_Id = CL2.UnitId
						AND CL2HF2.ParFieldType_Id = 2
					LEFT JOIN Produto PRD WITH (NOLOCK)
						ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CL2HF2.Value
						AND CL2HF2.ParFieldType_Id = 2
					WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
					and hf.name like '%turno%'
					FOR XML PATH (''))
				, 1, 1, '') AS HeaderFieldList
				INTO #CollectionLevel2XParHeaderFieldGeralTURNO
			FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id
			LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
				ON CL2HF.ParHeaderFieldGeral_Id = HF.Id
			LEFT JOIN ParLevel2 L2 (NOLOCK)
				ON L2.Id = CL2.ParLevel2_Id
				WHERE HF.Name LIKE '%turno%'
			GROUP BY CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeralTURNO (CollectionLevel2_Id);

			SELECT 
				VP.ParLevel1_Id
				, VP.ParLevel2_Id
				, VP.ParLevel3_Id
				, VP.[Sample]
				, VP.ParGroupParLevel1_Id
				, ISNULL(P3V.LimiteNC,0) AS LimiteNC
			INTO #VINCULO
			FROM ParVinculoPeso VP WITH(NOLOCK)
			INNER JOIN ParLevel3Value P3V WITH(NOLOCK) 
			ON VP.ParLevel1_Id = P3V.ParLevel1_Id
			AND VP.ParLevel2_Id = P3V.ParLevel2_Id
			AND VP.ParLevel3_Id = P3V.ParLevel3_Id
			WHERE VP.IsActive = 1
			AND P3V.IsActive = 1
			
			SELECT 
				CAST(C2.CollectionDate AS DATETIME) AS CollectionDate ,
				CAST(C2.CollectionDate AS DATE) AS DataTruncada ,
				CS.ParStructure_Id AS Regional ,
				C2.UnitId ,           
				C2.ParLevel1_Id ,
				C2.ParLevel2_Id ,
				R3.ParLevel3_Id ,
				C2.AuditorId ,
				C2.Sample WeiEvaluation,
				R3.Defects WeiDefects,
				cfpp.ParFamiliaProduto_Id,
				cfpp.ParProduto_Id,
				C2.ID AS CollectionLevel2_Id,
				PL2P.Equacao,
				PL2P.Peso as PESO_MON,
				pp.Name as SKU,
				PGL1.Name as GRUPOTAREFA,
				V.LimiteNC,
				HF.HeaderFieldList AS CABECALHO,
				HFTURNO.HeaderFieldList AS TURNO,
				CASE 
					WHEN V.LimiteNC < R3.Defects THEN 0 --ESTOREI O LIMITE
					WHEN R3.Defects = 0 THEN 1 --TIREI NOTA MÁXIMA
					ELSE 2 -- NEM ESTOUREI, NEM ZEREI
				END RESPOSTA
			INTO #CUBOLEVEL3
			FROM #Collectionlevel2 C2 
			INNER JOIN Result_Level3 R3 WITH (NOLOCK) 
			ON C2.Id = R3.CollectionLevel2_Id
			INNER JOIN parlevel3value R3V WITH (NOLOCK) 
			ON R3V.parlevel3_id = R3.parlevel3_id 
			AND R3V.ParLevel1_id = C2.ParLevel1_id 
			AND R3V.parlevel2_id = C2.ParLevel2_id 
			AND R3V.IsActive = 1 
			INNER JOIN ParVinculoPesoParLevel2 PL2P WITH (NOLOCK) 
			ON PL2P.ParLevel1_Id = C2.ParLevel1_Id 
			AND PL2P.ParLevel2_Id = C2.ParLevel2_Id 
			AND PL2P.IsActive = 1
			LEFT JOIN CollectionLevel2XParDepartment C2XDP WITH (NOLOCK) 
			ON C2.ID = C2XDP.CollectionLevel2_Id
			LEFT JOIN CollectionLevel2XParCargo C2XCG WITH (NOLOCK) 
			ON C2.ID = C2XCG.CollectionLevel2_Id
			LEFT JOIN ParDepartment D WITH (NOLOCK) 
			ON C2XDP.ParDepartment_Id = D.Id
			LEFT JOIN ParCompanyXStructure CS 
			ON CS.ParCompany_Id = c2.UnitId 
			AND cs.active = 1
			LEFT JOIN ParStructure S1 
			ON CS.ParStructure_Id = S1.Id
			LEFT JOIN ParStructure S2 
			ON S1.ParStructureParent_Id = S2.Id
			LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CFPP 
			ON cfpp.CollectionLevel2_Id = c2.Id       
			LEFT JOIN ParProduto PP 
			ON pp.Id = cfpp.ParProduto_Id
			LEFT JOIN #VINCULO V
			ON C2.ParLevel1_Id = V.ParLevel1_Id
			AND C2.ParLevel2_Id = V.ParLevel2_Id
			AND R3.ParLevel3_Id = V.ParLevel3_Id
			LEFT JOIN ParGroupParLevel1 PGL1
			ON PGL1.Id = V.ParGroupParLevel1_Id
			LEFT JOIN #CollectionLevel2XParHeaderFieldGeral2 HF 
			ON HF.CollectionLevel2_Id = C2.ID
			LEFT JOIN #CollectionLevel2XParHeaderFieldGeralTURNO HFTURNO
			ON HFTURNO.CollectionLevel2_Id = C2.ID
			WHERE 1=1
			AND R3.IsNotEvaluate = 0
			AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
			{whereStructure}
            {whereUnit}
			{whereTurno}
            {whereParLevel3}



			SELECT 
				B.PARLEVEL1_ID,
				sum(Peso) Peso
				INTO #PESOMONITORAMENTOINDICADOR
			FROM (			   
				SELECT -- TAREFA
				parlevel1_id,
				parlevel2_id,
				AVG(Peso) Peso
				FROM (				
					SELECT
						CUBOL3.parlevel1_id, 
						CUBOL3.parlevel2_id, 
						CUBOL3.parlevel3_id, 
						AVG(CUBOL3.PESO_MON) Peso
					FROM #CUBOLEVEL3 CUBOL3 WITH (NOLOCK)
					INNER JOIN ParCompany C WITH (NOLOCK)
						ON CUBOL3.UnitId = C.ID
					INNER JOIN ParLevel1 PL1 WITH (NOLOCK)
						ON CUBOL3.ParLevel1_Id = PL1.ID
					INNER JOIN ParLevel2 PL2 WITH (NOLOCK)
						ON CUBOL3.ParLevel2_Id = PL2.ID
					INNER JOIN ParLevel3 PL3 WITH (NOLOCK)
						ON CUBOL3.ParLevel3_Id = PL3.ID
					INNER join CollectionLevel2 cl2	
						on cl2.id = CUBOL3.CollectionLevel2_Id		
					LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CSFP	
						ON CSFP.CollectionLevel2_Id = CL2.Id	
					LEFT JOIN ParFamiliaProduto SFP WITH (NOLOCK)	
						ON CSFP.ParFamiliaProduto_Id = SFP.Id	
					LEFT JOIN ParProduto SP WITH (NOLOCK)	
						ON CSFP.ParProduto_Id = SP.Id
					WHERE 1 = 1
					AND CUBOL3.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
					GROUP BY CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id
				) A
				GROUP BY 
				parlevel1_id,
				parlevel2_id
			) B
			GROUP BY B.PARLEVEL1_ID


			DROP TABLE #VINCULO
			DROP TABLE #Collectionlevel2
			DROP TABLE #CollectionLevel2XParHeaderFieldGeral
			DROP TABLE #CollectionLevel2XParHeaderFieldGeral2
			DROP TABLE #CollectionLevel2XParHeaderFieldGeralTURNO

			--FINAL DA FORMAÇÃO DO CUBO

	 --------------------------------

			SELECT 
				CUBO2.UNITID ParCompany_Id,
				CUBO2.PARLEVEL1_ID,
				CUBO2.PARLEVEL2_ID,
				CUBO2.SKU,
				CUBO2.COLLECTIONDATE DATA,
				CUBO2.DataTruncada,
				CUBO2.CABECALHO,
				CUBO2.TURNO,
				avg(PESO_MON) Peso,
				COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) [NÚMERO DE TAREFAS],

				SUM(CUBO2.WeiEvaluation) AS AV,
				SUM(CUBO2.WeiDefects) AS Defects,

				SUM(IIF(RESPOSTA = 1, 1, 0)) SOMA,

				CASE
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 10 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 10 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 40 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 40 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
				
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 10 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 10 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 40 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 40 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 60 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = 'QtdeTLNC > 0 ? 0 : (QtdeNC == 0 ? 100 : 95 + (QtdeTC/QtdeT*5))' THEN
				CASE 
					WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
					WHEN SUM(RESPOSTA) = COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
					ELSE 95 + (SUM(IIF(RESPOSTA = 1, 1, 0)) / cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) * 5) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = 'QtdeTLNC > 0 ? 0 : 100' THEN
				CASE 
					WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
					WHEN SUM(RESPOSTA) = COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
					ELSE 0 -- NEM ESTOUREI, NEM ZEREI
				END

		END AS RESPOSTA2

	
			INTO #CUBO2
			FROM ( SELECT * FROM #CUBOLEVEL3 ) CUBO2
			GROUP BY 
			CUBO2.UNITID
			, CUBO2.PARLEVEL1_ID
			, CUBO2.PARLEVEL2_ID
			, CUBO2.SKU
			, CUBO2.DataTruncada
			, CUBO2.COLLECTIONDATE 
			, CUBO2.CABECALHO
			, CUBO2.TURNO
			, CUBO2.EQUACAO

			ORDER BY 
			CUBO2.UNITID
			, CUBO2.PARLEVEL1_ID
			, CUBO2.CABECALHO
			, CUBO2.TURNO
			, CUBO2.COLLECTIONDATE 
			, CUBO2.PARLEVEL2_ID
			, CUBO2.SKU


			SELECT PL2P.ParLevel1_Id, SUM(PESO) AS PESO_MONITORAMENTO 
			INTO #PESOTOTALMONITORAMENTOINDICADOR
			FROM ParVinculoPesoParLevel2 PL2P WITH (NOLOCK)
			WHERE IsActive = 1
			GROUP BY PL2P.ParLevel1_Id

			SELECT 
				CUBO1.ParCompany_Id, 
				CUBO1.DataTruncada,
				CUBO1.PARLEVEL1_ID, 
				CUBO1.SKU, 
				CUBO1.CABECALHO, 
				CUBO1.TURNO,
				SUM(CUBO1.AV) AS AV,
				SUM(CUBO1.Defects) AS Defects,
				SUM(CUBO1.PESO) AS [PESOTOTAL],
				PMM.PESO_MONITORAMENTO
				,SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) AS [PESO]
				,CASE 
					WHEN SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) < 1 THEN SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2) + (1 - SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) ) * 100  
					WHEN SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) > 1 THEN ((SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2)) / SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO))) 
				ELSE SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2) END TOTAL 
			INTO #CUBO1
			FROM ( SELECT * FROM #CUBO2) CUBO1
			INNER JOIN #PESOMONITORAMENTOINDICADOR PM 
			ON PM.ParLevel1_Id = CUBO1.ParLevel1_Id
			INNER JOIN #PESOTOTALMONITORAMENTOINDICADOR PMM
			ON PMM.ParLevel1_id = CUBO1.ParLevel1_Id
			GROUP BY 
			CUBO1.ParCompany_Id, 
			CUBO1.DataTruncada,
			CUBO1.PARLEVEL1_ID, 
			CUBO1.SKU, 
			CUBO1.CABECALHO, 
			CUBO1.TURNO,
			PMM.PESO_MONITORAMENTO

			-- TABELA
			SELECT 
				UN.Name as UnidadeName 
				, SUM(C.AV) AS AV
				, SUM(C.AV) - SUM(C.Defects) AS C
				, SUM(C.Defects) AS NC
				, ROUND(avg(C.Total),2) AS PORCC
				, CAST(RIGHT(LEFT(C.DataTruncada,7),2) AS VARCHAR) + '/' + CAST(LEFT(C.DataTruncada,4) AS VARCHAR) AS DATA

			FROM #CUBO1 C
			LEFT JOIN ParCompany UN WITH (NOLOCK)
			ON UN.ID = C.PARCOMPANY_ID
			GROUP BY 
			UN.Name
			, CAST(LEFT(C.DataTruncada,4) AS VARCHAR)
			, CAST(RIGHT(LEFT(C.DataTruncada,7),2) AS VARCHAR)
			ORDER BY 1 ASC

			DROP TABLE #CUBOLEVEL3
			DROP TABLE #CUBO2
			DROP TABLE #CUBO1
			DROP TABLE #PESOMONITORAMENTOINDICADOR
			DROP TABLE #PESOTOTALMONITORAMENTOINDICADOR
	
		";


        return query;
    }

    public string SelectPorcCTotalSeara(DataCarrierFormularioNew form)
    {
        var query = "";

        return query;
    }

    public string SelectSeara(DataCarrierFormularioNew form)
    {
		var whereStructure = "";
		var whereUnit = "";
		var whereTurno = "";
		var whereParLevel1 = "";
		var whereParLevel2 = "";
		var whereParLevel3 = "";

		if (form.ParLevel1_Ids.Length > 0)
		{
			whereParLevel1 = $@" AND Parlevel1_Id in ({string.Join(",", form.ParLevel1_Ids)}) ";
		}

		if (form.ParLevel2_Ids.Length > 0)
		{
			whereParLevel2 = $@" AND Parlevel2_Id in ({string.Join(",", form.ParLevel2_Ids)}) ";
		}

		if (form.ParLevel3_Ids.Length > 0)
		{
			whereParLevel3 = $@" AND R3.Parlevel3_Id in ({string.Join(",", form.ParLevel3_Ids)}) ";
		}

		if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
		{
			whereUnit = $@"AND UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
		}

		if (form.ShowTurnoSeara[0].ToString() != "-1")
		{
			whereTurno = $@"AND HFTURNO.HeaderFieldList LIKE '%{ form.ShowTurnoSeara[0] }%' ";
		}


		if (form.ParStructure2_Ids.Length > 0)
		{
			whereStructure = $@"AND CS.ParStructure_Id in ({string.Join(",", form.ParStructure2_Ids)})";
		}

		var query = $@"

            DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
            DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

			SELECT * INTO #Collectionlevel2 
			FROM CollectionLevel2 C2 WITH (NOLOCK) 
			WHERE CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
			
            {whereUnit}
			
            {whereParLevel1}
            {whereParLevel2}
            

			-- Criação da Fato de Cabeçalhos
			SELECT
				CL2HF.Id
				,CL2HF.CollectionLevel2_Id
				,CL2HF.ParHeaderFieldGeral_Id
				,CL2HF.ParFieldType_Id
				,CL2HF.Value INTO #CollectionLevel2XParHeaderFieldGeral
			FROM CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN #Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel_ID ON #CollectionLevel2XParHeaderFieldGeral (CollectionLevel2_Id);

			-- Concatenação da Fato de Cabeçalhos
			SELECT
				CL2HF.CollectionLevel2_Id
				,STUFF((SELECT DISTINCT
						', ' + CONCAT(HF.Name, ': ', CASE
							WHEN CL2HF2.ParFieldType_Id = 1 OR
								CL2HF2.ParFieldType_Id = 3 THEN PMV.Name
							WHEN CL2HF2.ParFieldType_Id = 2 THEN CASE
									WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
									ELSE EQP.Nome
								END
							WHEN CL2HF2.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CL2HF2.Value, 103)
							ELSE CL2HF2.Value
						END)
					FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (NOLOCK)
					LEFT JOIN collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF2.CollectionLevel2_Id
					LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
						ON CL2HF2.ParHeaderFieldGeral_Id = HF.Id
					LEFT JOIN ParLevel2 L2 (NOLOCK)
						ON L2.Id = CL2.ParLevel2_Id
					LEFT JOIN ParMultipleValuesGeral PMV (NOLOCK)
						ON CL2HF2.Value = CAST(PMV.Id AS VARCHAR(500))
						AND CL2HF2.ParFieldType_Id <> 2
					LEFT JOIN Equipamentos EQP (NOLOCK)
						ON CAST(EQP.Id AS VARCHAR(500)) = CL2HF2.Value
						AND EQP.ParCompany_Id = CL2.UnitId
						AND CL2HF2.ParFieldType_Id = 2
					LEFT JOIN Produto PRD WITH (NOLOCK)
						ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CL2HF2.Value
						AND CL2HF2.ParFieldType_Id = 2
					WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
					FOR XML PATH (''))
				, 1, 1, '') AS HeaderFieldList INTO #CollectionLevel2XParHeaderFieldGeral2
			FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id
			LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
				ON CL2HF.ParHeaderFieldGeral_Id = HF.Id
			LEFT JOIN ParLevel2 L2 (NOLOCK)
				ON L2.Id = CL2.ParLevel2_Id
			GROUP BY CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeral2 (CollectionLevel2_Id);
					
			-- Concatenação da Fato de Cabeçalhos TURNO
			SELECT
				CL2HF.CollectionLevel2_Id
				,STUFF((SELECT DISTINCT
						', ' + CONCAT(HF.Name, ': ', CASE
							WHEN CL2HF2.ParFieldType_Id = 1 OR
								CL2HF2.ParFieldType_Id = 3 THEN PMV.Name
							WHEN CL2HF2.ParFieldType_Id = 2 THEN CASE
									WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
									ELSE EQP.Nome
								END
							WHEN CL2HF2.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CL2HF2.Value, 103)
							ELSE CL2HF2.Value
						END)
					FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (NOLOCK)
					LEFT JOIN collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF2.CollectionLevel2_Id
					LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
						ON CL2HF2.ParHeaderFieldGeral_Id = HF.Id 
					LEFT JOIN ParLevel2 L2 (NOLOCK)
						ON L2.Id = CL2.ParLevel2_Id
					LEFT JOIN ParMultipleValuesGeral PMV (NOLOCK)
						ON CL2HF2.Value = CAST(PMV.Id AS VARCHAR(500))
						AND CL2HF2.ParFieldType_Id <> 2
					LEFT JOIN Equipamentos EQP (NOLOCK)
						ON CAST(EQP.Id AS VARCHAR(500)) = CL2HF2.Value
						AND EQP.ParCompany_Id = CL2.UnitId
						AND CL2HF2.ParFieldType_Id = 2
					LEFT JOIN Produto PRD WITH (NOLOCK)
						ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CL2HF2.Value
						AND CL2HF2.ParFieldType_Id = 2
					WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
					and hf.name like '%turno%'
					FOR XML PATH (''))
				, 1, 1, '') AS HeaderFieldList
				INTO #CollectionLevel2XParHeaderFieldGeralTURNO
			FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id
			LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
				ON CL2HF.ParHeaderFieldGeral_Id = HF.Id
			LEFT JOIN ParLevel2 L2 (NOLOCK)
				ON L2.Id = CL2.ParLevel2_Id
				WHERE HF.Name LIKE '%turno%'
			GROUP BY CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeralTURNO (CollectionLevel2_Id);

			SELECT 
				VP.ParLevel1_Id
				, VP.ParLevel2_Id
				, VP.ParLevel3_Id
				, VP.[Sample]
				, VP.ParGroupParLevel1_Id
				, ISNULL(P3V.LimiteNC,0) AS LimiteNC
			INTO #VINCULO
			FROM ParVinculoPeso VP WITH(NOLOCK)
			INNER JOIN ParLevel3Value P3V WITH(NOLOCK) 
			ON VP.ParLevel1_Id = P3V.ParLevel1_Id
			AND VP.ParLevel2_Id = P3V.ParLevel2_Id
			AND VP.ParLevel3_Id = P3V.ParLevel3_Id
			WHERE VP.IsActive = 1
			AND P3V.IsActive = 1
			
			SELECT 
				CAST(C2.CollectionDate AS DATETIME) AS CollectionDate ,
				CAST(C2.CollectionDate AS DATE) AS DataTruncada ,
				CS.ParStructure_Id AS Regional ,
				C2.UnitId ,           
				C2.ParLevel1_Id ,
				C2.ParLevel2_Id ,
				R3.ParLevel3_Id ,
				C2.AuditorId ,
				C2.Sample WeiEvaluation,
				R3.Defects WeiDefects,
				cfpp.ParFamiliaProduto_Id,
				cfpp.ParProduto_Id,
				C2.ID AS CollectionLevel2_Id,
				PL2P.Equacao,
				PL2P.Peso as PESO_MON,
				pp.Name as SKU,
				PGL1.Name as GRUPOTAREFA,
				V.LimiteNC,
				HF.HeaderFieldList AS CABECALHO,
				HFTURNO.HeaderFieldList AS TURNO,
				CASE 
					WHEN V.LimiteNC < R3.Defects THEN 0 --ESTOREI O LIMITE
					WHEN R3.Defects = 0 THEN 1 --TIREI NOTA MÁXIMA
					ELSE 2 -- NEM ESTOUREI, NEM ZEREI
				END RESPOSTA
			INTO #CUBOLEVEL3
			FROM #Collectionlevel2 C2 
			INNER JOIN Result_Level3 R3 WITH (NOLOCK) 
			ON C2.Id = R3.CollectionLevel2_Id
			INNER JOIN parlevel3value R3V WITH (NOLOCK) 
			ON R3V.parlevel3_id = R3.parlevel3_id 
			AND R3V.ParLevel1_id = C2.ParLevel1_id 
			AND R3V.parlevel2_id = C2.ParLevel2_id 
			AND R3V.IsActive = 1 
			INNER JOIN ParVinculoPesoParLevel2 PL2P WITH (NOLOCK) 
			ON PL2P.ParLevel1_Id = C2.ParLevel1_Id 
			AND PL2P.ParLevel2_Id = C2.ParLevel2_Id 
			AND PL2P.IsActive = 1
			LEFT JOIN CollectionLevel2XParDepartment C2XDP WITH (NOLOCK) 
			ON C2.ID = C2XDP.CollectionLevel2_Id
			LEFT JOIN CollectionLevel2XParCargo C2XCG WITH (NOLOCK) 
			ON C2.ID = C2XCG.CollectionLevel2_Id
			LEFT JOIN ParDepartment D WITH (NOLOCK) 
			ON C2XDP.ParDepartment_Id = D.Id
			LEFT JOIN ParCompanyXStructure CS 
			ON CS.ParCompany_Id = c2.UnitId 
			AND cs.active = 1
			LEFT JOIN ParStructure S1 
			ON CS.ParStructure_Id = S1.Id
			LEFT JOIN ParStructure S2 
			ON S1.ParStructureParent_Id = S2.Id
			LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CFPP 
			ON cfpp.CollectionLevel2_Id = c2.Id       
			LEFT JOIN ParProduto PP 
			ON pp.Id = cfpp.ParProduto_Id
			LEFT JOIN #VINCULO V
			ON C2.ParLevel1_Id = V.ParLevel1_Id
			AND C2.ParLevel2_Id = V.ParLevel2_Id
			AND R3.ParLevel3_Id = V.ParLevel3_Id
			LEFT JOIN ParGroupParLevel1 PGL1
			ON PGL1.Id = V.ParGroupParLevel1_Id
			LEFT JOIN #CollectionLevel2XParHeaderFieldGeral2 HF 
			ON HF.CollectionLevel2_Id = C2.ID
			LEFT JOIN #CollectionLevel2XParHeaderFieldGeralTURNO HFTURNO
			ON HFTURNO.CollectionLevel2_Id = C2.ID
			WHERE 1=1
			AND R3.IsNotEvaluate = 0
			AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
			{whereStructure}
            {whereUnit}
			{whereTurno}
            {whereParLevel3}



			SELECT 
				B.PARLEVEL1_ID,
				sum(Peso) Peso
				INTO #PESOMONITORAMENTOINDICADOR
			FROM (			   
				SELECT -- TAREFA
				parlevel1_id,
				parlevel2_id,
				AVG(Peso) Peso
				FROM (				
					SELECT
						CUBOL3.parlevel1_id, 
						CUBOL3.parlevel2_id, 
						CUBOL3.parlevel3_id, 
						AVG(CUBOL3.PESO_MON) Peso
					FROM #CUBOLEVEL3 CUBOL3 WITH (NOLOCK)
					INNER JOIN ParCompany C WITH (NOLOCK)
						ON CUBOL3.UnitId = C.ID
					INNER JOIN ParLevel1 PL1 WITH (NOLOCK)
						ON CUBOL3.ParLevel1_Id = PL1.ID
					INNER JOIN ParLevel2 PL2 WITH (NOLOCK)
						ON CUBOL3.ParLevel2_Id = PL2.ID
					INNER JOIN ParLevel3 PL3 WITH (NOLOCK)
						ON CUBOL3.ParLevel3_Id = PL3.ID
					INNER join CollectionLevel2 cl2	
						on cl2.id = CUBOL3.CollectionLevel2_Id		
					LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CSFP	
						ON CSFP.CollectionLevel2_Id = CL2.Id	
					LEFT JOIN ParFamiliaProduto SFP WITH (NOLOCK)	
						ON CSFP.ParFamiliaProduto_Id = SFP.Id	
					LEFT JOIN ParProduto SP WITH (NOLOCK)	
						ON CSFP.ParProduto_Id = SP.Id
					WHERE 1 = 1
					AND CUBOL3.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
					GROUP BY CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id
				) A
				GROUP BY 
				parlevel1_id,
				parlevel2_id
			) B
			GROUP BY B.PARLEVEL1_ID


			DROP TABLE #VINCULO
			DROP TABLE #Collectionlevel2
			DROP TABLE #CollectionLevel2XParHeaderFieldGeral
			DROP TABLE #CollectionLevel2XParHeaderFieldGeral2
			DROP TABLE #CollectionLevel2XParHeaderFieldGeralTURNO

			--FINAL DA FORMAÇÃO DO CUBO

	 --------------------------------

	SELECT 
		CUBO2.UNITID ParCompany_Id,
		CUBO2.PARLEVEL1_ID,
		CUBO2.PARLEVEL2_ID,
		CUBO2.SKU,
		CUBO2.COLLECTIONDATE DATA,
		CUBO2.DataTruncada,
		CUBO2.CABECALHO,
		CUBO2.TURNO,
		avg(PESO_MON) Peso,
		COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) [NÚMERO DE TAREFAS],

		SUM(CUBO2.WeiEvaluation) AS AV,
		SUM(CUBO2.WeiDefects) AS Defects,

		SUM(IIF(RESPOSTA = 1, 1, 0)) SOMA,

/*
(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 10 * (QtdeTC / (QtdeT -1))))
(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 40 * (QtdeTC / (QtdeT -1))))
(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 60 * (QtdeTC / (QtdeT -1))))
(QtdeNC == 0 ? 100 : 10 * (QtdeTC / (QtdeT -1)))
(QtdeNC == 0 ? 100 : 40 * (QtdeTC / (QtdeT -1)))
(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))
(QtdeNC == 0 ? 100 :(TCrit > 0 ? 0 : 60 * (QtdeTC / (QtdeT -1))))
QtdeTLNC > 0 ? 0 : (QtdeNC == 0 ? 100 : 95 + (QtdeTC/QtdeT*5))
QtdeTLNC > 0 ? 0 : 100

(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 10 * (QtdeTC / (QtdeT -1))))
(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 40 * (QtdeTC / (QtdeT -1))))
(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 60 * (QtdeTC / (QtdeT -1))))
(QtdeNC == 0 ? 100 : 40 * (QtdeTC / (QtdeT -1)))
(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))
(QtdeNC == 0 ? 100 :(TCrit > 0 ? 0 : 60 * (QtdeTC / (QtdeT -1))))
QtdeTLNC > 0 ? 0 : (QtdeNC == 0 ? 100 : 95 + (QtdeTC/QtdeT*5))
QtdeTLNC > 0 ? 0 : 100

*/

		CASE
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 10 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 10 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 40 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 40 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
				
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 10 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 10 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 40 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 40 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 60 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = 'QtdeTLNC > 0 ? 0 : (QtdeNC == 0 ? 100 : 95 + (QtdeTC/QtdeT*5))' THEN
				CASE 
					WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
					WHEN SUM(RESPOSTA) = COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
					ELSE 95 + (SUM(IIF(RESPOSTA = 1, 1, 0)) / cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) * 5) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = 'QtdeTLNC > 0 ? 0 : 100' THEN
				CASE 
					WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
					WHEN SUM(RESPOSTA) = COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
					ELSE 0 -- NEM ESTOUREI, NEM ZEREI
				END

		END AS RESPOSTA2

	
	INTO #CUBO2
	FROM ( SELECT * FROM #CUBOLEVEL3 ) CUBO2
	GROUP BY 
	CUBO2.UNITID
	, CUBO2.PARLEVEL1_ID
	, CUBO2.PARLEVEL2_ID
	, CUBO2.SKU
	, CUBO2.DataTruncada
	, CUBO2.COLLECTIONDATE 
	, CUBO2.CABECALHO
	, CUBO2.TURNO
    , CUBO2.EQUACAO

	ORDER BY 
	CUBO2.UNITID
	, CUBO2.PARLEVEL1_ID
	, CUBO2.CABECALHO
	, CUBO2.TURNO
	, CUBO2.COLLECTIONDATE 
	, CUBO2.PARLEVEL2_ID
	, CUBO2.SKU

	SELECT PL2P.ParLevel1_Id, SUM(PESO) AS PESO_MONITORAMENTO 
	INTO #PESOTOTALMONITORAMENTOINDICADOR
	FROM ParVinculoPesoParLevel2 PL2P WITH (NOLOCK)
	WHERE IsActive = 1
	GROUP BY PL2P.ParLevel1_Id

	SELECT 
		CUBO1.ParCompany_Id, 
		CUBO1.DataTruncada,
		CUBO1.PARLEVEL1_ID, 
		CUBO1.SKU, 
		CUBO1.CABECALHO, 
		CUBO1.TURNO,
		SUM(CUBO1.AV) AS AV,
		SUM(CUBO1.Defects) AS Defects,
		SUM(CUBO1.PESO) AS [PESOTOTAL],
        PMM.PESO_MONITORAMENTO
		,SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) AS [PESO]
		,CASE 
			WHEN SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) < 1 THEN SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2) + (1 - SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) ) * 100  
			WHEN SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) > 1 THEN ((SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2)) / SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO))) 
		ELSE SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2) END TOTAL 
	INTO #CUBO1
	FROM ( SELECT * FROM #CUBO2) CUBO1
	INNER JOIN #PESOMONITORAMENTOINDICADOR PM 
	ON PM.ParLevel1_Id = CUBO1.ParLevel1_Id
    INNER JOIN #PESOTOTALMONITORAMENTOINDICADOR PMM
    ON PMM.ParLevel1_id = CUBO1.ParLevel1_Id
	GROUP BY 
	CUBO1.ParCompany_Id, 
	CUBO1.DataTruncada,
	CUBO1.PARLEVEL1_ID, 
	CUBO1.SKU, 
	CUBO1.CABECALHO, 
	CUBO1.TURNO,
	PMM.PESO_MONITORAMENTO

	SELECT c2.*, ROUND(C2.RESPOSTA2, 2) as NotaGrupo, u.name as Empresa, p1.name as Familia, p2.name as Grupo, round(C1.TOTAL,2) AS NotaFamilia FROM #CUBO2 C2
	LEFT JOIN ParCompany U WITH (NOLOCK) ON U.ID = C2.PARCOMPANY_ID
	LEFT JOIN PARLEVEL1 P1 WITH (NOLOCK) ON P1.ID = C2.PARLEVEL1_ID
	LEFT JOIN PARLEVEL2 P2 WITH (NOLOCK) ON P2.ID = C2.PARLEVEL2_ID
	LEFT JOIN #CUBO1 C1 ON C1.CABECALHO = C2.CABECALHO
	AND C1.ParCompany_Id = C2.ParCompany_Id
	AND C1.ParLevel1_Id = C2.ParLevel1_Id
	AND C1.DataTruncada = C2.DataTruncada

	DROP TABLE #CUBOLEVEL3
	DROP TABLE #CUBO2
	DROP TABLE #CUBO1
	DROP TABLE #PESOMONITORAMENTOINDICADOR
	DROP TABLE #PESOTOTALMONITORAMENTOINDICADOR
	";

		return query;
	}

    public string SelectGraficoUnidade(DataCarrierFormularioNew form)
    {

        var whereStructure = "";
        var whereUnit = "";
        var whereTurno = "";
        var whereParLevel1 = "";
        var whereParLevel2 = "";
        var whereParLevel3 = "";

        if (form.ParLevel1_Ids.Length > 0)
        {
            whereParLevel1 = $@" AND Parlevel1_Id in ({string.Join(",", form.ParLevel1_Ids)}) ";
        }

        if (form.ParLevel2_Ids.Length > 0)
        {
            whereParLevel2 = $@" AND Parlevel2_Id in ({string.Join(",", form.ParLevel2_Ids)}) ";
        }

        if (form.ParLevel3_Ids.Length > 0)
        {
            whereParLevel3 = $@" AND R3.Parlevel3_Id in ({string.Join(",", form.ParLevel3_Ids)}) ";
        }

        if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
        {
            whereUnit = $@"AND UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
        }

        if (form.ShowTurnoSeara[0].ToString() != "-1")
        {
            whereTurno = $@"AND HFTURNO.HeaderFieldList LIKE '%{ form.ShowTurnoSeara[0] }%' ";
        }


        if (form.ParStructure2_Ids.Length > 0)
        {
            whereStructure = $@"AND CS.ParStructure_Id in ({string.Join(",", form.ParStructure2_Ids)})";
        }

        var query = $@"

            DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
            DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

			SELECT * INTO #Collectionlevel2 
			FROM CollectionLevel2 C2 WITH (NOLOCK) 
			WHERE CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
			
            {whereUnit}
			
            {whereParLevel1}
            {whereParLevel2}
            

			-- Criação da Fato de Cabeçalhos
			SELECT
				CL2HF.Id
				,CL2HF.CollectionLevel2_Id
				,CL2HF.ParHeaderFieldGeral_Id
				,CL2HF.ParFieldType_Id
				,CL2HF.Value INTO #CollectionLevel2XParHeaderFieldGeral
			FROM CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN #Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel_ID ON #CollectionLevel2XParHeaderFieldGeral (CollectionLevel2_Id);

			-- Concatenação da Fato de Cabeçalhos
			SELECT
				CL2HF.CollectionLevel2_Id
				,STUFF((SELECT DISTINCT
						', ' + CONCAT(HF.Name, ': ', CASE
							WHEN CL2HF2.ParFieldType_Id = 1 OR
								CL2HF2.ParFieldType_Id = 3 THEN PMV.Name
							WHEN CL2HF2.ParFieldType_Id = 2 THEN CASE
									WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
									ELSE EQP.Nome
								END
							WHEN CL2HF2.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CL2HF2.Value, 103)
							ELSE CL2HF2.Value
						END)
					FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (NOLOCK)
					LEFT JOIN collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF2.CollectionLevel2_Id
					LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
						ON CL2HF2.ParHeaderFieldGeral_Id = HF.Id
					LEFT JOIN ParLevel2 L2 (NOLOCK)
						ON L2.Id = CL2.ParLevel2_Id
					LEFT JOIN ParMultipleValuesGeral PMV (NOLOCK)
						ON CL2HF2.Value = CAST(PMV.Id AS VARCHAR(500))
						AND CL2HF2.ParFieldType_Id <> 2
					LEFT JOIN Equipamentos EQP (NOLOCK)
						ON CAST(EQP.Id AS VARCHAR(500)) = CL2HF2.Value
						AND EQP.ParCompany_Id = CL2.UnitId
						AND CL2HF2.ParFieldType_Id = 2
					LEFT JOIN Produto PRD WITH (NOLOCK)
						ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CL2HF2.Value
						AND CL2HF2.ParFieldType_Id = 2
					WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
					FOR XML PATH (''))
				, 1, 1, '') AS HeaderFieldList INTO #CollectionLevel2XParHeaderFieldGeral2
			FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id
			LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
				ON CL2HF.ParHeaderFieldGeral_Id = HF.Id
			LEFT JOIN ParLevel2 L2 (NOLOCK)
				ON L2.Id = CL2.ParLevel2_Id
			GROUP BY CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeral2 (CollectionLevel2_Id);
					
			-- Concatenação da Fato de Cabeçalhos TURNO
			SELECT
				CL2HF.CollectionLevel2_Id
				,STUFF((SELECT DISTINCT
						', ' + CONCAT(HF.Name, ': ', CASE
							WHEN CL2HF2.ParFieldType_Id = 1 OR
								CL2HF2.ParFieldType_Id = 3 THEN PMV.Name
							WHEN CL2HF2.ParFieldType_Id = 2 THEN CASE
									WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
									ELSE EQP.Nome
								END
							WHEN CL2HF2.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CL2HF2.Value, 103)
							ELSE CL2HF2.Value
						END)
					FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (NOLOCK)
					LEFT JOIN collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF2.CollectionLevel2_Id
					LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
						ON CL2HF2.ParHeaderFieldGeral_Id = HF.Id 
					LEFT JOIN ParLevel2 L2 (NOLOCK)
						ON L2.Id = CL2.ParLevel2_Id
					LEFT JOIN ParMultipleValuesGeral PMV (NOLOCK)
						ON CL2HF2.Value = CAST(PMV.Id AS VARCHAR(500))
						AND CL2HF2.ParFieldType_Id <> 2
					LEFT JOIN Equipamentos EQP (NOLOCK)
						ON CAST(EQP.Id AS VARCHAR(500)) = CL2HF2.Value
						AND EQP.ParCompany_Id = CL2.UnitId
						AND CL2HF2.ParFieldType_Id = 2
					LEFT JOIN Produto PRD WITH (NOLOCK)
						ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CL2HF2.Value
						AND CL2HF2.ParFieldType_Id = 2
					WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
					and hf.name like '%turno%'
					FOR XML PATH (''))
				, 1, 1, '') AS HeaderFieldList
				INTO #CollectionLevel2XParHeaderFieldGeralTURNO
			FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
			INNER JOIN Collectionlevel2 CL2 (NOLOCK)
				ON CL2.Id = CL2HF.CollectionLevel2_Id
			LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
				ON CL2HF.ParHeaderFieldGeral_Id = HF.Id
			LEFT JOIN ParLevel2 L2 (NOLOCK)
				ON L2.Id = CL2.ParLevel2_Id
				WHERE HF.Name LIKE '%turno%'
			GROUP BY CL2HF.CollectionLevel2_Id

			CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeralTURNO (CollectionLevel2_Id);

			SELECT 
				VP.ParLevel1_Id
				, VP.ParLevel2_Id
				, VP.ParLevel3_Id
				, VP.[Sample]
				, VP.ParGroupParLevel1_Id
				, ISNULL(P3V.LimiteNC,0) AS LimiteNC
			INTO #VINCULO
			FROM ParVinculoPeso VP WITH(NOLOCK)
			INNER JOIN ParLevel3Value P3V WITH(NOLOCK) 
			ON VP.ParLevel1_Id = P3V.ParLevel1_Id
			AND VP.ParLevel2_Id = P3V.ParLevel2_Id
			AND VP.ParLevel3_Id = P3V.ParLevel3_Id
			WHERE VP.IsActive = 1
			AND P3V.IsActive = 1
			
			SELECT 
				CAST(C2.CollectionDate AS DATETIME) AS CollectionDate ,
				CAST(C2.CollectionDate AS DATE) AS DataTruncada ,
				CS.ParStructure_Id AS Regional ,
				C2.UnitId ,           
				C2.ParLevel1_Id ,
				C2.ParLevel2_Id ,
				R3.ParLevel3_Id ,
				C2.AuditorId ,
				C2.Sample WeiEvaluation,
				R3.Defects WeiDefects,
				cfpp.ParFamiliaProduto_Id,
				cfpp.ParProduto_Id,
				C2.ID AS CollectionLevel2_Id,
				PL2P.Equacao,
				PL2P.Peso as PESO_MON,
				pp.Name as SKU,
				PGL1.Name as GRUPOTAREFA,
				V.LimiteNC,
				HF.HeaderFieldList AS CABECALHO,
				HFTURNO.HeaderFieldList AS TURNO,
				CASE 
					WHEN V.LimiteNC < R3.Defects THEN 0 --ESTOREI O LIMITE
					WHEN R3.Defects = 0 THEN 1 --TIREI NOTA MÁXIMA
					ELSE 2 -- NEM ESTOUREI, NEM ZEREI
				END RESPOSTA
			INTO #CUBOLEVEL3
			FROM #Collectionlevel2 C2 
			INNER JOIN Result_Level3 R3 WITH (NOLOCK) 
			ON C2.Id = R3.CollectionLevel2_Id
			INNER JOIN parlevel3value R3V WITH (NOLOCK) 
			ON R3V.parlevel3_id = R3.parlevel3_id 
			AND R3V.ParLevel1_id = C2.ParLevel1_id 
			AND R3V.parlevel2_id = C2.ParLevel2_id 
			AND R3V.IsActive = 1 
			INNER JOIN ParVinculoPesoParLevel2 PL2P WITH (NOLOCK) 
			ON PL2P.ParLevel1_Id = C2.ParLevel1_Id 
			AND PL2P.ParLevel2_Id = C2.ParLevel2_Id 
			AND PL2P.IsActive = 1
			LEFT JOIN CollectionLevel2XParDepartment C2XDP WITH (NOLOCK) 
			ON C2.ID = C2XDP.CollectionLevel2_Id
			LEFT JOIN CollectionLevel2XParCargo C2XCG WITH (NOLOCK) 
			ON C2.ID = C2XCG.CollectionLevel2_Id
			LEFT JOIN ParDepartment D WITH (NOLOCK) 
			ON C2XDP.ParDepartment_Id = D.Id
			LEFT JOIN ParCompanyXStructure CS 
			ON CS.ParCompany_Id = c2.UnitId 
			AND cs.active = 1
			LEFT JOIN ParStructure S1 
			ON CS.ParStructure_Id = S1.Id
			LEFT JOIN ParStructure S2 
			ON S1.ParStructureParent_Id = S2.Id
			LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CFPP 
			ON cfpp.CollectionLevel2_Id = c2.Id       
			LEFT JOIN ParProduto PP 
			ON pp.Id = cfpp.ParProduto_Id
			LEFT JOIN #VINCULO V
			ON C2.ParLevel1_Id = V.ParLevel1_Id
			AND C2.ParLevel2_Id = V.ParLevel2_Id
			AND R3.ParLevel3_Id = V.ParLevel3_Id
			LEFT JOIN ParGroupParLevel1 PGL1
			ON PGL1.Id = V.ParGroupParLevel1_Id
			LEFT JOIN #CollectionLevel2XParHeaderFieldGeral2 HF 
			ON HF.CollectionLevel2_Id = C2.ID
			LEFT JOIN #CollectionLevel2XParHeaderFieldGeralTURNO HFTURNO
			ON HFTURNO.CollectionLevel2_Id = C2.ID
			WHERE 1=1
			AND R3.IsNotEvaluate = 0
			AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
			{whereStructure}
            {whereUnit}
			{whereTurno}
            {whereParLevel3}



			SELECT 
				B.PARLEVEL1_ID,
				sum(Peso) Peso
				INTO #PESOMONITORAMENTOINDICADOR
			FROM (			   
				SELECT -- TAREFA
				parlevel1_id,
				parlevel2_id,
				AVG(Peso) Peso
				FROM (				
					SELECT
						CUBOL3.parlevel1_id, 
						CUBOL3.parlevel2_id, 
						CUBOL3.parlevel3_id, 
						AVG(CUBOL3.PESO_MON) Peso
					FROM #CUBOLEVEL3 CUBOL3 WITH (NOLOCK)
					INNER JOIN ParCompany C WITH (NOLOCK)
						ON CUBOL3.UnitId = C.ID
					INNER JOIN ParLevel1 PL1 WITH (NOLOCK)
						ON CUBOL3.ParLevel1_Id = PL1.ID
					INNER JOIN ParLevel2 PL2 WITH (NOLOCK)
						ON CUBOL3.ParLevel2_Id = PL2.ID
					INNER JOIN ParLevel3 PL3 WITH (NOLOCK)
						ON CUBOL3.ParLevel3_Id = PL3.ID
					INNER join CollectionLevel2 cl2	
						on cl2.id = CUBOL3.CollectionLevel2_Id		
					LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CSFP	
						ON CSFP.CollectionLevel2_Id = CL2.Id	
					LEFT JOIN ParFamiliaProduto SFP WITH (NOLOCK)	
						ON CSFP.ParFamiliaProduto_Id = SFP.Id	
					LEFT JOIN ParProduto SP WITH (NOLOCK)	
						ON CSFP.ParProduto_Id = SP.Id
					WHERE 1 = 1
					AND CUBOL3.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
					GROUP BY CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id
				) A
				GROUP BY 
				parlevel1_id,
				parlevel2_id
			) B
			GROUP BY B.PARLEVEL1_ID


			DROP TABLE #VINCULO
			DROP TABLE #Collectionlevel2
			DROP TABLE #CollectionLevel2XParHeaderFieldGeral
			DROP TABLE #CollectionLevel2XParHeaderFieldGeral2
			DROP TABLE #CollectionLevel2XParHeaderFieldGeralTURNO

			--FINAL DA FORMAÇÃO DO CUBO

	 --------------------------------

	SELECT 
		CUBO2.UNITID ParCompany_Id,
		CUBO2.PARLEVEL1_ID,
		CUBO2.PARLEVEL2_ID,
		CUBO2.SKU,
		CUBO2.COLLECTIONDATE DATA,
		CUBO2.DataTruncada,
		CUBO2.CABECALHO,
		CUBO2.TURNO,
		avg(PESO_MON) Peso,
		COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) [NÚMERO DE TAREFAS],

		SUM(CUBO2.WeiEvaluation) AS AV,
		SUM(CUBO2.WeiDefects) AS Defects,

		SUM(IIF(RESPOSTA = 1, 1, 0)) SOMA,

		CASE
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 10 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 10 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 40 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 40 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
				
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 10 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 10 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 40 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 40 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = '(QtdeNC == 0 ? 100 : ( TCrit > 0 ? 0 : 60 * (QtdeTC / (QtdeT -1))))' THEN
				CASE 
					WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
					WHEN (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
					ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = 'QtdeTLNC > 0 ? 0 : (QtdeNC == 0 ? 100 : 95 + (QtdeTC/QtdeT*5))' THEN
				CASE 
					WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
					WHEN SUM(RESPOSTA) = COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
					ELSE 95 + (SUM(IIF(RESPOSTA = 1, 1, 0)) / cast(COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) as FLOAT) * 5) -- NEM ESTOUREI, NEM ZEREI
				END
			WHEN CUBO2.Equacao = 'QtdeTLNC > 0 ? 0 : 100' THEN
				CASE 
					WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
					WHEN SUM(RESPOSTA) = COUNT(DISTINCT(CUBO2.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
					ELSE 0 -- NEM ESTOUREI, NEM ZEREI
				END

		END AS RESPOSTA2

	
	INTO #CUBO2
	FROM ( SELECT * FROM #CUBOLEVEL3 ) CUBO2
	GROUP BY 
	CUBO2.UNITID
	, CUBO2.PARLEVEL1_ID
	, CUBO2.PARLEVEL2_ID
	, CUBO2.SKU
	, CUBO2.DataTruncada
	, CUBO2.COLLECTIONDATE 
	, CUBO2.CABECALHO
	, CUBO2.TURNO
	, CUBO2.EQUACAO

	ORDER BY 
	CUBO2.UNITID
	, CUBO2.PARLEVEL1_ID
	, CUBO2.CABECALHO
	, CUBO2.TURNO
	, CUBO2.COLLECTIONDATE 
	, CUBO2.PARLEVEL2_ID
	, CUBO2.SKU

	SELECT PL2P.ParLevel1_Id, SUM(PESO) AS PESO_MONITORAMENTO 
	INTO #PESOTOTALMONITORAMENTOINDICADOR
	FROM ParVinculoPesoParLevel2 PL2P WITH (NOLOCK)
	WHERE IsActive = 1
	GROUP BY PL2P.ParLevel1_Id

	SELECT 
		CUBO1.ParCompany_Id, 
		CUBO1.DataTruncada,
		CUBO1.PARLEVEL1_ID, 
		CUBO1.SKU, 
		CUBO1.CABECALHO, 
		CUBO1.TURNO,
		SUM(CUBO1.AV) AS AV,
		SUM(CUBO1.Defects) AS Defects,
		SUM(CUBO1.PESO) AS [PESOTOTAL],
        PMM.PESO_MONITORAMENTO
		,SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) AS [PESO]
		,CASE 
			WHEN SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) < 1 THEN SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2) + (1 - SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) ) * 100  
			WHEN SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO)) > 1 THEN ((SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2)) / SUM((CUBO1.PESO / PMM.PESO_MONITORAMENTO))) 
		ELSE SUM( (CUBO1.PESO / PMM.PESO_MONITORAMENTO) * RESPOSTA2) END TOTAL 
	INTO #CUBO1
	FROM ( SELECT * FROM #CUBO2) CUBO1
	INNER JOIN #PESOMONITORAMENTOINDICADOR PM 
	ON PM.ParLevel1_Id = CUBO1.ParLevel1_Id
    INNER JOIN #PESOTOTALMONITORAMENTOINDICADOR PMM
    ON PMM.ParLevel1_id = CUBO1.ParLevel1_Id
	GROUP BY 
	CUBO1.ParCompany_Id, 
	CUBO1.DataTruncada,
	CUBO1.PARLEVEL1_ID, 
	CUBO1.SKU, 
	CUBO1.CABECALHO, 
	CUBO1.TURNO,
	PMM.PESO_MONITORAMENTO


	


";

        if (form.ShowModeloGrafico_Id[0] == 2) //DIA
        {
            query += $@"-- MES
						SELECT 
							CAST(LEFT(C.DataTruncada,7) AS VARCHAR) as UnidadeName 
							, CAST(LEFT(C.DataTruncada,7) AS VARCHAR) as UnidadeId 
							, SUM(C.AV) AS AV
							, SUM(C.Defects) AS NC
							, SUM(C.AV) - SUM(C.Defects) AS C
							, (SUM(C.AV) - SUM(C.Defects)) / SUM(C.AV) AS PORCC
							, SUM(C.Defects) / SUM(C.AV) AS PORCNC
							, SUM(C.PESO) AS [PESOTOTAL]
							, SUM(C.PESO) AS [PESO]
							, avg(C.Total) TOTAL 
							, CAST(LEFT(C.DataTruncada,7) AS VARCHAR) as UnidadeId
						FROM #CUBO1 C
						GROUP BY 
						CAST(LEFT(C.DataTruncada,7) AS VARCHAR)
						ORDER BY 1 ";
        }
        else if (form.ShowModeloGrafico_Id[0] == 3) //SEMANA
        {
            query += $@"-- SEMANA
						SELECT 
							CAST(YEAR(C.DataTruncada) AS VARCHAR) + '-' + CASE WHEN LEN(CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR)) = 1 THEN '0' + CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR) ELSE CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR) END as UnidadeName 
							, CAST(YEAR(C.DataTruncada) AS VARCHAR) + '-' + CASE WHEN LEN(CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR)) = 1 THEN '0' + CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR) ELSE CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR) END as UnidadeId 
							, SUM(C.AV) AS AV
							, SUM(C.Defects) AS NC
							, SUM(C.AV) - SUM(C.Defects) AS C
							, (SUM(C.AV) - SUM(C.Defects)) / SUM(C.AV) AS PORCC
							, SUM(C.Defects) / SUM(C.AV) AS PORCNC
							, SUM(C.PESO) AS [PESOTOTAL]
							, SUM(C.PESO) AS [PESO]
							, avg(C.Total) TOTAL 
							, CAST(YEAR(C.DataTruncada) AS VARCHAR) + '-' + CASE WHEN LEN(CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR)) = 1 THEN '0' + CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR) ELSE CAST(DATEPART(WEEK, C.DataTruncada) AS VARCHAR) END as UnidadeId
						FROM #CUBO1 C
						GROUP BY 
						C.DataTruncada
						ORDER BY 1 ";
        }
        else if (form.ShowModeloGrafico_Id[0] == 4) //MES
        {
            query += $@" 
						-- DIARIO
						SELECT 
							C.DataTruncada as UnidadeName 
							, C.DataTruncada as UnidadeId 
							, SUM(C.AV) AS AV
							, SUM(C.Defects) AS NC
							, SUM(C.AV) - SUM(C.Defects) AS C
							, (SUM(C.AV) - SUM(C.Defects)) / SUM(C.AV) AS PORCC
							, SUM(C.Defects) / SUM(C.AV) AS PORCNC
							, SUM(C.PESO) AS [PESOTOTAL]
							, SUM(C.PESO) AS [PESO]
							, avg(C.Total) TOTAL 
							, C.DataTruncada as UnidadeId
						FROM #CUBO1 C
						GROUP BY 
						C.DataTruncada
						ORDER BY 1 ";
        }
        else if (form.ShowDimensaoGrafico_Id.Length > 0)
        {
            switch (form.ShowDimensaoGrafico_Id[0])
            {
                case 0: //UNIDADES
                    query += $@"-- UNIDADES
								SELECT 
									UN.Name as UnidadeName 
									, C.ParCompany_Id as UnidadeId 
									, SUM(C.AV) AS AV
									, SUM(C.Defects) AS NC
									, SUM(C.AV) - SUM(C.Defects) AS C
									, (SUM(C.AV) - SUM(C.Defects)) / SUM(C.AV) AS PORCC
									, SUM(C.Defects) / SUM(C.AV) AS PORCNC
									, SUM(C.PESO) AS [PESOTOTAL]
									, SUM(C.PESO) AS [PESO]
									, avg(C.Total) TOTAL 
									, C.ParCompany_Id as UnidadeId
								FROM #CUBO1 C
								LEFT JOIN ParCompany UN WITH (NOLOCK)
								ON UN.ID = C.PARCOMPANY_ID
								GROUP BY 
								UN.Name
								, C.ParCompany_Id
								ORDER BY 10 DESC ";
                    break;

                case 1: //INDICADORES
                    query += $@"-- INDICADOR
								SELECT 
									UN.Name as UnidadeName 
									, C.ParLevel1_id as UnidadeId 
									, SUM(C.AV) AS AV
									, SUM(C.Defects) AS NC
									, SUM(C.AV) - SUM(C.Defects) AS C
									, (SUM(C.AV) - SUM(C.Defects)) / SUM(C.AV) AS PORCC
									, SUM(C.Defects) / SUM(C.AV) AS PORCNC
									, SUM(C.PESO) AS [PESOTOTAL]
									, SUM(C.PESO) AS [PESO]
									, avg(C.Total) TOTAL 
									, C.ParLevel1_id as UnidadeId
								FROM #CUBO1 C
								LEFT JOIN ParLevel1 UN WITH (NOLOCK)
								ON UN.ID = C.ParLevel1_id
								GROUP BY 
								UN.Name
								, C.ParLevel1_id
								ORDER BY 10 DESC ";
                    break;

                case 2: //MONITORAMENTOS
                    query += $@"-- MONITORAMENTO
								SELECT 
									UN.Name as UnidadeName 
									, C.ParLevel2_id as UnidadeId 
									, SUM(C.AV) AS AV
									, SUM(C.Defects) AS NC
									, SUM(C.AV) - SUM(C.Defects) AS C
									, (SUM(C.AV) - SUM(C.Defects)) / SUM(C.AV) AS PORCC
									, SUM(C.Defects) / SUM(C.AV) AS PORCNC
									, SUM(C.PESO) AS [PESOTOTAL]
									, SUM(C.PESO) AS [PESO]
									, avg(C.RESPOSTA2) TOTAL 
									, C.ParLevel2_id as UnidadeId
								FROM #CUBO2 C
								LEFT JOIN ParLevel2 UN WITH (NOLOCK)
								ON UN.ID = C.ParLevel2_id
								GROUP BY 
								UN.Name
								, C.ParLevel2_id
								ORDER BY 10 DESC ";
                    break;

                case 3: //TAREFAS
                    query += $@"-- TAREFAS
								SELECT 
									UN.Name as UnidadeName 
									, C.ParLevel3_id as UnidadeId 
									, SUM(C.WeiEvaluation) AS AV
									, SUM(C.WeiDefects) AS NC
									, SUM(C.WeiEvaluation) - SUM(C.WeiDefects) AS C
									, (SUM(C.WeiEvaluation) - SUM(C.WeiDefects)) / SUM(C.WeiEvaluation) AS PORCC
									, SUM(C.WeiDefects) / SUM(C.WeiEvaluation) AS PORCNC
									, SUM(0) AS [PESOTOTAL]
									, SUM(0) AS [PESO]
									, avg(0) TOTAL 
									, C.ParLevel3_id as UnidadeId
								FROM #CUBOLEVEL3 C
								LEFT JOIN ParLevel3 UN WITH (NOLOCK)
								ON UN.ID = C.ParLevel3_id
								GROUP BY 
								UN.Name
								, C.ParLevel3_id
								ORDER BY 4 DESC ";


                    break;

                case 4: //FAMÍLIA DE PRODUTO

                    break;

                case 5: //SKU
                    query += $@"-- SKU
								SELECT 
									C.SKU as UnidadeName 
									, C.SKU as UnidadeId 
									, SUM(C.AV) AS AV
									, SUM(C.Defects) AS NC
									, SUM(C.AV) - SUM(C.Defects) AS C
									, (SUM(C.AV) - SUM(C.Defects)) / SUM(C.AV) AS PORCC
									, SUM(C.Defects) / SUM(C.AV) AS PORCNC
									, SUM(C.PESO) AS [PESOTOTAL]
									, SUM(C.PESO) AS [PESO]
									, avg(C.Total) TOTAL 
									, C.SKU as UnidadeId
								FROM #CUBO1 C
								GROUP BY 
								C.SKU
								ORDER BY 10 DESC ";


                    break;

                case 6: //GRUPO DE TAREFAS
                    query += $@"-- GRUPO DE TAREFAS
								SELECT 
									C.GRUPOTAREFA as UnidadeName 
									, C.GRUPOTAREFA as UnidadeId 
									, SUM(C.WeiEvaluation) AS AV
									, SUM(C.WeiDefects) AS NC
									, SUM(C.WeiEvaluation) - SUM(C.WeiDefects) AS C
									, (SUM(C.WeiEvaluation) - SUM(C.WeiDefects)) / SUM(C.WeiEvaluation) AS PORCC
									, SUM(C.WeiDefects) / SUM(C.WeiEvaluation) AS PORCNC
									, SUM(0) AS [PESOTOTAL]
									, SUM(0) AS [PESO]
									, avg(0) TOTAL 
									, C.GRUPOTAREFA as UnidadeId
								FROM #CUBOLEVEL3 C
	
								GROUP BY 
								C.GRUPOTAREFA
								, C.GRUPOTAREFA
								ORDER BY 4 DESC ";


                    break;

                default:
                    break;
            }


        }


        query += $@"
					DROP TABLE #CUBOLEVEL3
					DROP TABLE #CUBO2
					DROP TABLE #CUBO1
					DROP TABLE #PESOMONITORAMENTOINDICADOR
					DROP TABLE #PESOTOTALMONITORAMENTOINDICADOR
					";


        return query;
    }

}
﻿using DTO;
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



    public string SelectUnidadesSeara(DataCarrierFormularioNew form)
    {
        var whereDepartment = "";
        var whereSecao = "";
        var whereCargo = "";
        var whereStructure = "";
        var whereUnit = "";
		var whereTurno = "";
		var whereParLevel1 = "";
        var whereParLevel2 = "";
        var whereParLevel3 = "";

        var campos2 = " ,C.ParCompany_Id as Parcompany_id, null as parlevel1_id, null as parlevel2_id, null as data ";
        var campos3 = " , null as data";
        var campos5 = "";
        var groupBy = "  GROUP BY C.NAME, C.Id  ";
        var groupBy2 = " GROUP BY C.ParCompany_Id, data ";
        var groupBy5 = "";
        //var selectTotal = " SELECT A.UnidadeName, A.AV, A.C, A.C, B.TOTAL AS PORCC, A.DATA FROM #RR1 A INNER JOIN #RR2 B ON A.DATA = B.DATA1 ";
        var selectTotal = " SELECT A.UnidadeName, A.AV, A.C, A.C, B.TOTAL AS PORCC, A.DATA FROM #RR1 A INNER JOIN #RR2 B ON A.DATA = B.DATA1 ";
		    selectTotal = " SELECT A.UnidadeName, A.AV, A.C,  (A.AV -A.C) as NC , B.TOTAL AS PORCC, A.DATA FROM #RR1 A INNER JOIN #RR2 B ON A.DATA = B.DATA1 and a.UnidadeName = (select name from ParCompany where id = b.Parcompany_id) ";

		if (1==1)
        {
            campos5 = " ,M.Name + '/' + CAST(DATEPART(YEAR, data1) AS VARCHAR) as data1 ";
            groupBy5 = "  , M.Name + '/' + CAST(DATEPART(YEAR, data1) AS VARCHAR) ";
        }


        if (form.ParLevel1_Ids.Length > 0)
        {
            whereParLevel1 = $@" AND CUBOL3.Parlevel1_Id in ({string.Join(",", form.ParLevel1_Ids)}) ";
        }

        if (form.ParLevel2_Ids.Length > 0)
        {
            whereParLevel2 = $@" AND CUBOL3.Parlevel2_Id in ({string.Join(",", form.ParLevel2_Ids)}) ";
        }

        if (form.ParLevel3_Ids.Length > 0)
        {
            whereParLevel3 = $@" AND CUBOL3.Parlevel3_Id in ({string.Join(",", form.ParLevel3_Ids)}) ";
        }

        if (form.ParDepartment_Ids.Length > 0)
        {
            whereDepartment = $@" AND CUBOL3.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
        }

        if (form.ParSecao_Ids.Length > 0)
        {
            whereSecao = $@" AND CUBOL3.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
        }

        if (form.ParCargo_Ids.Length > 0)
        {
            whereCargo = $@" AND CUBOL3.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
        }

        if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
        {
            whereUnit = $@"AND CUBOL3.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
        }

		if (form.ShowTurnoSeara[0] != "-1")
		{
			whereTurno = $@"AND CUBOL3.HeaderFieldList LIKE '%{ form.ShowTurnoSeara[0] }%' ";
		}


		if (form.ParStructure2_Ids.Length > 0)
        {
            whereStructure = $@"AND CUBOL3.Regional in ({string.Join(",", form.ParStructure2_Ids)})";
        }

        var query = $@"

  DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

            -- Criação da Fato de Cabeçalhos
					SELECT
						CL2HF.Id
					   ,CL2HF.CollectionLevel2_Id
					   ,CL2HF.ParHeaderFieldGeral_Id
					   ,CL2HF.ParFieldType_Id
					   ,CL2HF.Value INTO #CollectionLevel2XParHeaderFieldGeral
					FROM CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
					INNER JOIN Collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF.CollectionLevel2_Id
						WHERE CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
						
										CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel_ID ON #CollectionLevel2XParHeaderFieldGeral (CollectionLevel2_Id);
					-- Concatenação da Fato de Cabeçalhos
			-- Concatenação da Fato de Cabeçalhos
					SELECT
						CL2HF.CollectionLevel2_Id
					   ,STUFF((SELECT DISTINCT
								', ' + CONCAT('', CASE
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
							AND CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
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
						AND CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
					GROUP BY CL2HF.CollectionLevel2_Id
										CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeralTURNO (CollectionLevel2_Id);


            SELECT CAST(C2.CollectionDate AS DATETIME) AS CollectionDate ,
               C2.ParFrequency_Id ,
               S2.ParStructureParent_Id AS Holding ,
               S1.ParStructureParent_Id AS GrupoDeEmpresa ,
               CS.ParStructure_Id AS Regional ,
               C2.UnitId ,
               D.Parent_Id AS Centro_De_Custo_Id ,
               C2XDP.ParDepartment_Id AS Secao_Id ,
               C2XCG.ParCargo_Id AS Cargo_Id ,
               C2.ParLevel1_Id ,
               C2.ParLevel2_Id ,
               R3.ParLevel3_Id ,
               C2.AuditorId ,
               C2.Sample WeiEvaluation,
               R3.Defects WeiDefects,
	           cfpp.ParFamiliaProduto_Id,
	           cfpp.ParProduto_Id,
			   C2.ID AS CollectionLevel2_Id,
			   R3V.LimiteNC,
			   PL2P.Equacao,
			   PL2P.Peso,
			   c2.evaluationNumber as Avaliacao,
			   pp.Name as SKU,
			   PGL1.Name as GRUPOTAREFA,
			   HFTURNO.HeaderFieldList	
	           INTO #CUBOLEVEL3
        FROM CollectionLevel2 C2 WITH (NOLOCK)
        INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON C2.Id = R3.CollectionLevel2_Id
		LEFT JOIN #CollectionLevel2XParHeaderFieldGeralTURNO HFTURNO ON HFTURNO.CollectionLevel2_Id = C2.Id
		INNER JOIN parlevel3value R3V WITH (NOLOCK) ON R3V.parlevel3_id = R3.parlevel3_id and R3V.ParLevel1_id = C2.ParLevel1_id and R3V.parlevel2_id = C2.ParLevel2_id and R3V.IsActive = 1 
		INNER JOIN ParVinculoPesoParLevel2 PL2P WITH (NOLOCK) ON PL2P.ParLevel1_Id = C2.ParLevel1_Id AND PL2P.ParLevel2_Id = C2.ParLevel2_Id AND PL2P.IsActive = 1

        LEFT JOIN CollectionLevel2XParDepartment C2XDP WITH (NOLOCK) ON C2.ID = C2XDP.CollectionLevel2_Id
        LEFT JOIN CollectionLevel2XParCargo C2XCG WITH (NOLOCK) ON C2.ID = C2XCG.CollectionLevel2_Id
        LEFT JOIN ParDepartment D WITH (NOLOCK) ON C2XDP.ParDepartment_Id = D.Id
        LEFT JOIN ParCompanyXStructure CS ON CS.ParCompany_Id = c2.UnitId and cs.active = 1
        LEFT JOIN ParStructure S1 ON CS.ParStructure_Id = S1.Id
        LEFT JOIN ParStructure S2 ON S1.ParStructureParent_Id = S2.Id
        LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CFPP on cfpp.CollectionLevel2_Id = c2.Id
        
        LEFT JOIN ParProduto PP on pp.Id = cfpp.ParProduto_Id

		LEFT JOIN ParVinculoPeso PVP 
		ON PVP.ParLevel1_Id = C2.ParLevel1_Id
		AND PVP.ParLevel2_Id = C2.ParLevel2_Id
		AND PVP.ParLevel3_Id = r3.ParLevel3_Id
		AND PVP.IsActive = 1
		LEFT JOIN ParGroupParLevel1 PGL1
		ON PGL1.Id = PVP.ParGroupParLevel1_Id

        WHERE 1=1
          AND R3.IsNotEvaluate = 0
          AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
		
		DROP TABLE #CollectionLevel2XParHeaderFieldGeral 
		DROP TABLE #CollectionLevel2XParHeaderFieldGeralTURNO
     --------------------------------

               

                    DECLARE @MES TABLE (
	                    ID INT
                       ,Name VARCHAR(10)
                    )

                    INSERT INTO @MES (ID, Name)
	                    VALUES (1, '01')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (2, '02')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (3, '03')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (4, '04')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (5, '05')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (6, '06')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (7, '07')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (8, '08')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (9, '09')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (10, '10')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (11, '11')
                    INSERT INTO @MES (ID, Name)
	                    VALUES (12, '12')



                    SELECT
	                    PC.Name AS UnidadeName
                       ,CAST(SUM(CUBOL3.WeiEvaluation) AS DECIMAL(20,2)) AS AV
                       ,CAST(SUM(CUBOL3.WeiEvaluation) - SUM(CUBOL3.WeiDefects) AS DECIMAL(20,2)) AS C
                       ,CAST(((SUM(CUBOL3.WeiEvaluation) - SUM(CUBOL3.WeiDefects)) / SUM(CUBOL3.WeiEvaluation)) * 100 AS DECIMAL(20,2)) AS PORCC
                       ,M.Name + '/' + CAST(DATEPART(YEAR, CUBOL3.CollectionDate) AS VARCHAR) AS Data

                    INTO #RR1
                    FROM #CUBOLEVEL3 CUBOL3 WITH (NOLOCK)

                    
                    INNER JOIN ParCompany PC WITH (NOLOCK)
	                    ON CUBOL3.UnitId = PC.ID

                    INNER JOIN ParLevel1 PL1 WITH (NOLOCK)
	                    ON CUBOL3.ParLevel1_Id = PL1.ID

                    INNER JOIN ParLevel2 PL2 WITH (NOLOCK)
	                    ON CUBOL3.ParLevel2_Id = PL2.ID

                    INNER JOIN ParLevel3 PL3 WITH (NOLOCK)
	                    ON CUBOL3.ParLevel3_Id = PL3.ID
/*
				 INNER join CollectionLevel2 cl2
				  on cl2.CollectionDate = CUBOL3.CollectionDate

					left JOIN CollectionLevel2XParFamiliaProdutoXParProduto CSFP
						ON CSFP.CollectionLevel2_Id = CL2.Id

                    left JOIN ParFamiliaProduto SFP WITH (NOLOCK)
	                    ON CSFP.ParFamiliaProduto_Id = SFP.Id

                    left JOIN ParProduto SP WITH (NOLOCK)
	                    ON CSFP.ParProduto_Id = SP.Id
*/
                    INNER JOIN @MES M
	                    ON M.ID = DATEPART(MONTH, CUBOL3.CollectionDate)

                    WHERE 1 = 1

                        AND CUBOL3.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}

                    GROUP BY PC.Name
		            ,CAST(DATEPART(YEAR, CUBOL3.CollectionDate) AS VARCHAR)
		            ,M.Name             
                    ORDER BY CAST(DATEPART(YEAR, CUBOL3.CollectionDate) AS VARCHAR) DESC

-------------------------------------------------------------------------------

SELECT -- MONITORAMENTO
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
					   AVG(CUBOL3.Peso) Peso

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

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}
                    
                
                    GROUP BY CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id
				) a
				GROUP BY parlevel1_id,
					parlevel2_id
				) B
				GROUP BY B.PARLEVEL1_ID


SELECT 
CAST(AVG(PESOTOTAL) AS DECIMAL(20,2)) AS PESOTOTAL
,CAST(AVG(TOTAL) AS DECIMAL(20,2)) AS TOTAL
,Parcompany_id 
,parlevel1_id 
,parlevel2_id
-- ,SKU
,data
{campos5}

INTO #RR2 FROM (

SELECT -- INDICADOR
    
	--C.ParCompany_Id,
	--C.ParLevel1_Id,
	SUM(C.PESO) AS [PESOTOTAL]
	,SUM( (C.PESO / PM.PESO) * CASE WHEN C.PARLEVEL2_ID = 605 THEN RESPOSTA3 ELSE RESPOSTA2 END) TOTAL -- 0 98,125 0 100 97,5
    ,DATA1, AVALIACAO, SKU
   
{campos2}
	
	FROM (

            SELECT -- MONITORAMENTO
			B.ParCompany_Id,
			B.PARLEVEL1_ID,
			B.PARLEVEL2_ID,
            B.SKU,
            b.data,
			avg(Peso) Peso,
			COUNT(DISTINCT(B.PARLEVEL3_ID)) [NÚMERO DE TAREFAS],

			SUM(IIF(RESPOSTA = 1, 1, 0)) SOMA,

			CASE 
				WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
				WHEN SUM(RESPOSTA) = COUNT(DISTINCT(B.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
				ELSE 95 + (SUM(IIF(RESPOSTA = 1, 1, 0)) / cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) * 5) -- NEM ESTOUREI, NEM ZEREI
			END RESPOSTA2,
			   -- QtdeTLNC > 0 ? 0 : (QtdeNC == 0 ? 100 : 95 + (QtdeTC/QtdeT*5))

			CASE 
				WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
				WHEN (cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
				ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
			END RESPOSTA3,
            DATA1, AVALIACAO
			   --(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))

			   FROM (
			   
			    SELECT -- TAREFA

					parCompany_Id,
					parlevel1_id,
					parlevel2_id,
					parlevel3_id,
                    SKU,
                    data,
					AV,
					Defeitos,
					LimiteNC,
					Peso,
					
					CASE 
						WHEN LimiteNC < Defeitos THEN 0 --ESTOREI O LIMITE
						WHEN Defeitos = 0 THEN 1 --TIREI NOTA MÁXIMA
						ELSE 2 -- NEM ESTOUREI, NEM ZEREI
					END RESPOSTA,

                    DATA1, AVALIACAO


					 

				FROM (
				
					SELECT
					   
					   CUBOL3.UnitId as parcompany_id,
					   CUBOL3.parlevel1_id, 
					   CUBOL3.parlevel2_id, 
					   CUBOL3.parlevel3_id,
                       CUBOL3.SKU,
					   sum(CUBOL3.WeiEvaluation) AV, 
					   sum(CUBOL3.WeiDefects) Defeitos,
					   AVG(CUBOL3.LimiteNC) LimiteNC,
					   AVG(CUBOL3.Peso) Peso
                       , CAST(CUBOL3.COLLECTIONDATE AS DATE) DATA1, CUBOL3.Avaliacao
					  
{campos3}


					  

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

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}
                    
 {groupBy}
                
                    , CUBOL3.UnitId, CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id, CUBOL3.SKU, CUBOL3.Centro_De_Custo_Id, CUBOL3.Secao_Id, CUBOL3.Cargo_Id, CUBOL3.UnitId, CUBOL3.Equacao, CUBOL3.Avaliacao, CAST(CUBOL3.COLLECTIONDATE AS DATE)
				) a

				) B
				GROUP BY B.parcompany_id, B.PARLEVEL1_ID,  B.PARLEVEL2_ID, data, DATA1, AVALIACAO, SKU

		) C
		INNER JOIN #PESOMONITORAMENTOINDICADOR PM ON PM.ParLevel1_Id = C.ParLevel1_Id
		
 {groupBy2} , DATA1, AVALIACAO, C.PARLEVEL1_ID, C.SKU

) D

INNER JOIN @MES M
ON M.ID = DATEPART(MONTH, data1)

GROUP BY Parcompany_id 
,parlevel1_id 
,parlevel2_id
--,SKU
,data
{groupBy5}
		DROP TABLE #PESOMONITORAMENTOINDICADOR
		DROP TABLE #CUBOLEVEL3  

    {selectTotal}

   

   DROP TABLE #RR1    
   DROP TABLE #RR2

-------------------------------------------------------------------------------




                ";
        return query;
    }

    public string SelectPorcCTotalSeara(DataCarrierFormularioNew form)
    {
        var whereDepartment = "";
        var whereSecao = "";
        var whereCargo = "";
        var whereStructure = "";
        var whereUnit = "";
		var whereTurno = "";
        var whereParLevel1 = "";
        var whereParLevel2 = "";
        var whereParLevel3 = "";

        var campos2 = " ,C.ParCompany_Id as Parcompany_id, null as parlevel1_id, null as parlevel2_id, null as data ";
        var campos3 = " , null as data";
        var groupBy = "  GROUP BY C.NAME, C.Id  ";
        var groupBy2 = " GROUP BY C.ParCompany_Id, data ";
        var selectTotal = " SELECT TOP 1 TOTAL AS PORCC FROM #RR2  ";


        if (form.ParLevel1_Ids.Length > 0)
        {
            whereParLevel1 = $@" AND CUBOL3.Parlevel1_Id in ({string.Join(",", form.ParLevel1_Ids)}) ";
        }

        if (form.ParLevel2_Ids.Length > 0)
        {
            whereParLevel2 = $@" AND CUBOL3.Parlevel2_Id in ({string.Join(",", form.ParLevel2_Ids)}) ";
        }

        if (form.ParLevel3_Ids.Length > 0)
        {
            whereParLevel3 = $@" AND CUBOL3.Parlevel3_Id in ({string.Join(",", form.ParLevel3_Ids)}) ";
        }

        if (form.ParDepartment_Ids.Length > 0)
        {
            whereDepartment = $@" AND CUBOL3.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
        }

        if (form.ParSecao_Ids.Length > 0)
        {
            whereSecao = $@" AND CUBOL3.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
        }

        if (form.ParCargo_Ids.Length > 0)
        {
            whereCargo = $@" AND CUBOL3.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
        }

        if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
        {
            whereUnit = $@"AND CUBOL3.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
        }

		if (form.ShowTurnoSeara[0] != "-1")
		{
			whereTurno = $@"AND CUBOL3.HeaderFieldList LIKE '%{ form.ShowTurnoSeara[0] }%' ";
		}

		if (form.ParStructure2_Ids.Length > 0)
        {
            whereStructure = $@"AND CUBOL3.Regional in ({string.Join(",", form.ParStructure2_Ids)})";
        }

        var query = $@"

                 DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
                 DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

		-- Criação da Fato de Cabeçalhos
					SELECT
						CL2HF.Id
					   ,CL2HF.CollectionLevel2_Id
					   ,CL2HF.ParHeaderFieldGeral_Id
					   ,CL2HF.ParFieldType_Id
					   ,CL2HF.Value INTO #CollectionLevel2XParHeaderFieldGeral
					FROM CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
					INNER JOIN Collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF.CollectionLevel2_Id
						WHERE CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
						
										CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel_ID ON #CollectionLevel2XParHeaderFieldGeral (CollectionLevel2_Id);
					-- Concatenação da Fato de Cabeçalhos
			-- Concatenação da Fato de Cabeçalhos
					SELECT
						CL2HF.CollectionLevel2_Id
					   ,STUFF((SELECT DISTINCT
								', ' + CONCAT('', CASE
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
							AND CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
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
						AND CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
					GROUP BY CL2HF.CollectionLevel2_Id
										CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeralTURNO (CollectionLevel2_Id);


            SELECT CAST(C2.CollectionDate AS DATETIME) AS CollectionDate ,
               C2.ParFrequency_Id ,
               S2.ParStructureParent_Id AS Holding ,
               S1.ParStructureParent_Id AS GrupoDeEmpresa ,
               CS.ParStructure_Id AS Regional ,
               C2.UnitId ,
               D.Parent_Id AS Centro_De_Custo_Id ,
               C2XDP.ParDepartment_Id AS Secao_Id ,
               C2XCG.ParCargo_Id AS Cargo_Id ,
               C2.ParLevel1_Id ,
               C2.ParLevel2_Id ,
               R3.ParLevel3_Id ,
               C2.AuditorId ,
               C2.Sample WeiEvaluation,
               R3.Defects WeiDefects,
	           cfpp.ParFamiliaProduto_Id,
	           cfpp.ParProduto_Id,
			   C2.ID AS CollectionLevel2_Id,
			   R3V.LimiteNC,
			   PL2P.Equacao,
			   PL2P.Peso,
			   c2.evaluationNumber as Avaliacao,
			   pp.Name as SKU,
			   PGL1.Name as GRUPOTAREFA,
			   HFTURNO.HeaderFieldList	
	           INTO #CUBOLEVEL3
        FROM CollectionLevel2 C2 WITH (NOLOCK)
        INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON C2.Id = R3.CollectionLevel2_Id
		LEFT JOIN #CollectionLevel2XParHeaderFieldGeralTURNO HFTURNO ON HFTURNO.CollectionLevel2_Id = C2.Id
		INNER JOIN parlevel3value R3V WITH (NOLOCK) ON R3V.parlevel3_id = R3.parlevel3_id and R3V.ParLevel1_id = C2.ParLevel1_id and R3V.parlevel2_id = C2.ParLevel2_id and R3V.IsActive = 1 
		INNER JOIN ParVinculoPesoParLevel2 PL2P WITH (NOLOCK) ON PL2P.ParLevel1_Id = C2.ParLevel1_Id AND PL2P.ParLevel2_Id = C2.ParLevel2_Id AND PL2P.IsActive = 1

        LEFT JOIN CollectionLevel2XParDepartment C2XDP WITH (NOLOCK) ON C2.ID = C2XDP.CollectionLevel2_Id
        LEFT JOIN CollectionLevel2XParCargo C2XCG WITH (NOLOCK) ON C2.ID = C2XCG.CollectionLevel2_Id
        LEFT JOIN ParDepartment D WITH (NOLOCK) ON C2XDP.ParDepartment_Id = D.Id
        LEFT JOIN ParCompanyXStructure CS ON CS.ParCompany_Id = c2.UnitId and cs.active = 1
        LEFT JOIN ParStructure S1 ON CS.ParStructure_Id = S1.Id
        LEFT JOIN ParStructure S2 ON S1.ParStructureParent_Id = S2.Id
        LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CFPP on cfpp.CollectionLevel2_Id = c2.Id
        
        LEFT JOIN ParProduto PP on pp.Id = cfpp.ParProduto_Id

		LEFT JOIN ParVinculoPeso PVP 
		ON PVP.ParLevel1_Id = C2.ParLevel1_Id
		AND PVP.ParLevel2_Id = C2.ParLevel2_Id
		AND PVP.ParLevel3_Id = r3.ParLevel3_Id
		AND PVP.IsActive = 1
		LEFT JOIN ParGroupParLevel1 PGL1
		ON PGL1.Id = PVP.ParGroupParLevel1_Id

        WHERE 1=1
          AND R3.IsNotEvaluate = 0
          AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
		
		DROP TABLE #CollectionLevel2XParHeaderFieldGeral 
		DROP TABLE #CollectionLevel2XParHeaderFieldGeralTURNO
     --------------------------------

                 

                SELECT
		CAST((SUM(c) / SUM(av)) * 100 AS DECIMAL(20,2)) AS PORCC

    into #RR1
	FROM (SELECT
		   SUM(CUBOL3.WeiEvaluation) AS AV
		   ,SUM(CUBOL3.WeiEvaluation) - SUM(CUBOL3.WeiDefects) AS C

		-- FROM DW.Cubo_Coleta_L3 CUBOL3 WITH (NOLOCK)
        FROM #CUBOLEVEL3 CUBOL3 WITH (NOLOCK)

/*
		INNER JOIN ParCompany C WITH (NOLOCK)
			ON CUBOL3.UnitId = C.ID

		INNER JOIN ParLevel1 PL1 WITH (NOLOCK)
			ON CUBOL3.ParLevel1_Id = PL1.ID

		INNER JOIN ParLevel2 PL2 WITH (NOLOCK)
			ON CUBOL3.ParLevel2_Id = PL2.ID

		INNER JOIN ParLevel3 PL3 WITH (NOLOCK)
			ON CUBOL3.ParLevel3_Id = PL3.ID

						 INNER join CollectionLevel2 cl2	
				  on cl2.CollectionDate = CUBOL3.CollectionDate	
					left JOIN CollectionLevel2XParFamiliaProdutoXParProduto CSFP	
						ON CSFP.CollectionLevel2_Id = CL2.Id	
                    left JOIN ParFamiliaProduto SFP WITH (NOLOCK)	
	                    ON CSFP.ParFamiliaProduto_Id = SFP.Id	
                    left JOIN ParProduto SP WITH (NOLOCK)	
	                    ON CSFP.ParProduto_Id = SP.Id
*/

		WHERE 1 = 1

        AND CUBOL3.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}

                    GROUP BY CUBOL3.CollectionDate) a

-------------------------------------------------------------------------------

SELECT -- MONITORAMENTO
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
					   AVG(CUBOL3.Peso) Peso

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

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}
                    
                
                    GROUP BY CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id
				) a
				GROUP BY parlevel1_id,
					parlevel2_id
				) B
				GROUP BY B.PARLEVEL1_ID

SELECT 
CAST(AVG(PESOTOTAL) AS DECIMAL(20,2)) AS PESOTOTAL
,CAST(AVG(TOTAL) AS DECIMAL(20,2)) AS TOTAL
,Parcompany_id 
,parlevel1_id 
,parlevel2_id
--,SKU
,data

INTO #RR2 FROM (

SELECT -- INDICADOR
    
	--C.ParCompany_Id,
	--C.ParLevel1_Id,
	SUM(C.PESO) AS [PESOTOTAL]
	,SUM( (C.PESO / PM.PESO) * CASE WHEN C.PARLEVEL2_ID = 605 THEN RESPOSTA3 ELSE RESPOSTA2 END) TOTAL -- 0 98,125 0 100 97,5
, DATA1, AVALIACAO, C.SKU
   
{campos2}
	
	 FROM (

            SELECT -- MONITORAMENTO
			B.ParCompany_Id,
			B.PARLEVEL1_ID,
			B.PARLEVEL2_ID,
            B.SKU,
            b.data,
			avg(Peso) Peso,
			COUNT(DISTINCT(B.PARLEVEL3_ID)) [NÚMERO DE TAREFAS],

			SUM(IIF(RESPOSTA = 1, 1, 0)) SOMA,

			CASE 
				WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
				WHEN SUM(RESPOSTA) = COUNT(DISTINCT(B.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
				ELSE 95 + (SUM(IIF(RESPOSTA = 1, 1, 0)) / cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) * 5) -- NEM ESTOUREI, NEM ZEREI
			END RESPOSTA2,
			   -- QtdeTLNC > 0 ? 0 : (QtdeNC == 0 ? 100 : 95 + (QtdeTC/QtdeT*5))

			CASE 
				WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
				WHEN (cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
				ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
			END RESPOSTA3
, DATA1, AVALIACAO
			   --(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))

			   FROM (
			   
			    SELECT -- TAREFA

					parCompany_Id,
					parlevel1_id,
					parlevel2_id,
					parlevel3_id,
                    SKU,
                    data,
					AV,
					Defeitos,
					LimiteNC,
					Peso,
					
					CASE 
						WHEN LimiteNC < Defeitos THEN 0 --ESTOREI O LIMITE
						WHEN Defeitos = 0 THEN 1 --TIREI NOTA MÁXIMA
						ELSE 2 -- NEM ESTOUREI, NEM ZEREI
					END RESPOSTA
, DATA1, AVALIACAO


					 

				FROM (
				
					SELECT
					   
					   CUBOL3.UnitId as parcompany_id,
					   CUBOL3.parlevel1_id, 
					   CUBOL3.parlevel2_id, 
					   CUBOL3.parlevel3_id, 
                       CUBOL3.SKU,
					   sum(CUBOL3.WeiEvaluation) AV, 
					   sum(CUBOL3.WeiDefects) Defeitos,
					   AVG(CUBOL3.LimiteNC) LimiteNC,
					   AVG(CUBOL3.Peso) Peso
, CAST(CUBOL3.COLLECTIONDATE AS DATE) DATA1, AVALIACAO
					  
{campos3}


					  

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

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}
                    
 {groupBy}
                
                    , CUBOL3.UnitId, CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id, CUBOL3.SKU, CUBOL3.Centro_De_Custo_Id, CUBOL3.Secao_Id, CUBOL3.Cargo_Id, CUBOL3.UnitId, CUBOL3.Equacao, CUBOL3.Avaliacao, CAST(CUBOL3.COLLECTIONDATE AS DATE)
				) a

				) B
				GROUP BY B.parcompany_id, B.PARLEVEL1_ID,  B.PARLEVEL2_ID, data, DATA1, AVALIACAO, B.SKU

		) C
		INNER JOIN #PESOMONITORAMENTOINDICADOR PM ON PM.ParLevel1_Id = C.ParLevel1_Id
		
 {groupBy2} , DATA1, AVALIACAO, C.PARLEVEL1_ID, C.SKU

) D

GROUP BY Parcompany_id 
,parlevel1_id 
,parlevel2_id
--,SKU
,data

		DROP TABLE #PESOMONITORAMENTOINDICADOR
		DROP TABLE #CUBOLEVEL3  

    {selectTotal}

   

   DROP TABLE #RR1    
   DROP TABLE #RR2

-------------------------------------------------------------------------------

                ";
        return query;
    }

    public string SelectSeara(DataCarrierFormularioNew form, string userUnits)
    {
        var dtInit = form.startDate.ToString("yyyyMMdd");
        var dtF = form.endDate.ToString("yyyyMMdd");

        var sqlTurno = "";
        var sqlUnidade = "";
        var sqlLevel1 = "";
        var sqlLevel2 = "";
        var sqlLevel3 = "";
        var sqlDepartment = "";
        var sqlCargo = "";
        var formatDate = "";

        var sqlClusterGroup = "";
        var sqlParStructure2 = "";
        var sqlParStructure3 = "";
        var sqlAuditor = "";


        if (form.Shift_Ids.Length > 0)
        {
            sqlTurno = $"\n AND [Shift] in ({string.Join(",", form.Shift_Ids)})";
        }

        if (form.ParCompany_Ids.Length > 0)
        {
            sqlUnidade = $"\n AND UnitId in ({string.Join(",", form.ParCompany_Ids)})";
        }

        if (form.ParLevel1_Ids.Length > 0)
        {
            sqlLevel1 = $"\n AND ParLevel1_id in ({string.Join(",", form.ParLevel1_Ids)})";
        }

        if (form.ParLevel2_Ids.Length > 0)
        {
            sqlLevel2 = $"\n AND ParLevel2_Id in ({string.Join(",", form.ParLevel2_Ids)})";
        }

        if (form.ParLevel3_Ids.Length > 0)
        {
            sqlLevel3 = $"\n AND L3.Id  in ({string.Join(",", form.ParLevel3_Ids)})";
        }

        if (form.ParSecao_Ids.Length > 0)
        {
            var sqlDepartamentoPelaHash = "";
            foreach (var item in form.ParSecao_Ids)
            {
                sqlDepartamentoPelaHash += $@"OR PD.Hash like '{item}|%'
                            OR PD.Hash like '%|{item}|%'
                            OR PD.Hash = '{item}'";
            }
            sqlDepartment = $@" AND (PD.Id in ({string.Join(",", form.ParSecao_Ids)}) 
                             {sqlDepartamentoPelaHash})";
        }
        else if (form.ParDepartment_Ids.Length > 0)
        {
            var sqlDepartamentoPelaHash = "";
            foreach (var item in form.ParDepartment_Ids)
            {
                sqlDepartamentoPelaHash += $@"OR PD.Hash like '{item}|%'
                            OR PD.Hash like '%|{item}|%'
                            OR PD.Hash = '{item}'";
            }
            sqlDepartment = $@" AND (PD.Id in ({string.Join(",", form.ParDepartment_Ids)}) 
                             {sqlDepartamentoPelaHash})";
        }

        if (form.ParCargo_Ids.Length > 0)
        {
            sqlCargo = $"\n AND PCargo.Id  in ({string.Join(",", form.ParCargo_Ids)})";
        }

        if (GlobalConfig.Eua)
        {
            formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 101)";
        }
        else
        {
            formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 103)";
        }


        if (form.ParClusterGroup_Ids.Length > 0)
            sqlClusterGroup = $" AND PC.ParClusterGroup_Id IN({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Cluster";

        //if (form.ParStructure2_Ids.Length > 0)
        //    sqlParStructure2 = $" AND pg.ParStructureParent_Id IN({string.Join(",", form.ParStructure2_Ids)}) --Grupo de empresa";

        if (form.ParStructure2_Ids.Length > 0)
            sqlParStructure2 = $" AND pg.Id IN({string.Join(",", form.ParStructure2_Ids)}) --Regional";

        if (form.userSgqAuditor_Ids.Length > 0)
            sqlAuditor = $"AND C2.AuditorId IN({string.Join(",", form.userSgqAuditor_Ids)}) --Auditor";


        var query = $@" 

                    -- DROP TABLE #CollectionLevel2

                     SELECT 
	                     id
						,ParFrequency_Id
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
                    FROM Collectionlevel2 CL2 WITH (NOLOCK)
                        WHERE 1=1
                         AND NotEvaluatedIs <> 999
                         AND Duplicated <> 999
                         AND CL2.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF }  23:59:59'
                         AND UnitId in ({userUnits})
                         { sqlTurno } 
                         { sqlUnidade } 
                         { sqlLevel1 } 
                         { sqlLevel2 }
 
		CREATE INDEX IDX_CollectionLevel2_ID ON #CollectionLevel2(ID);
		CREATE INDEX IDX_CollectionLevel2_UnitId ON #CollectionLevel2(UnitId);
		CREATE INDEX IDX_CollectionLevel2_CollectionDate ON #CollectionLevel2(CollectionDate);
		CREATE INDEX IDX_CollectionLevel2_ParLevel1_Id ON #CollectionLevel2(ParLevel1_Id);
		CREATE INDEX IDX_CollectionLevel2_ParLevel2_Id ON #CollectionLevel2(ParLevel2_Id);
		CREATE INDEX IDX_CollectionLevel2_12345 ON #CollectionLevel2(ID,UnitId,CollectionDate,ParLevel1_Id,ParLevel2_Id);


					-- Result Level 3

					SELECT
						R3.Id
					   ,R3.CollectionLevel2_Id
					   ,R3.ParLevel3_Id
					   ,R3.ParLevel3_Name
					   ,R3.Weight
					   ,R3.IntervalMin
					   ,R3.IntervalMax
					   ,R3.Value
					   ,R3.ValueText
					   ,R3.IsConform
					   ,R3.IsNotEvaluate
					   ,R3.WeiEvaluation
					   ,R3.WeiDefects INTO #Result_Level3
					FROM Result_Level3 R3 WITH (NOLOCK)
					INNER JOIN #CollectionLevel2 C2
						ON R3.CollectionLevel2_Id = C2.Id
	

					CREATE INDEX IDX_Result_Level3_CollectionLevel2_ID ON #Result_Level3(CollectionLevel2_Id);
					CREATE INDEX IDX_Result_Level3_CollectionLevel2_Lvl3_ID ON #Result_Level3(CollectionLevel2_Id,Parlevel3_Id);


					-- CollectionLevel2XCollectionJson

					SELECT
						CollectionLevel2_Id
					   ,CollectionJson_Id AS CollectionJson_Id
					   ,ROW_NUMBER() OVER (PARTITION BY CollectionLevel2_Id ORDER BY CollectionJson_Id DESC) AS [ROW] INTO #CollectionLevel2XCollectionJson
					FROM CollectionLevel2XCollectionJson C2CJ WITH (NOLOCK)

					INNER JOIN #CollectionLevel2 C2
						ON C2.Id = C2CJ.CollectionLevel2_Id



					DELETE FROM #CollectionLevel2XCollectionJson
					WHERE [ROW] > 1

					CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionLevel2_ID ON #CollectionLevel2XCollectionJson(CollectionLevel2_Id);
					CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionLevel2CollectionJson_ID ON #CollectionLevel2XCollectionJson(CollectionLevel2_Id,CollectionJson_Id);
					CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionJson_ID ON #CollectionLevel2XCollectionJson(CollectionJson_Id);


					-- CollectionJson

					SELECT
						CJ.Id
					   ,CJ.AppVersion INTO #CollectionJson
					FROM CollectionJson CJ WITH (NOLOCK)
					INNER JOIN #CollectionLevel2XCollectionJson C2CJ WITH (NOLOCK)
						ON CJ.Id = C2CJ.CollectionJson_Id

										CREATE INDEX IDX_CollectionJson_CollectionJson_ID ON #CollectionJson(ID);


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
							LEFT JOIN #collectionlevel2 CL2 (NOLOCK)
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
					INNER JOIN #Collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF.CollectionLevel2_Id
					LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
						ON CL2HF.ParHeaderFieldGeral_Id = HF.Id
					LEFT JOIN ParLevel2 L2 (NOLOCK)
						ON L2.Id = CL2.ParLevel2_Id
					GROUP BY CL2HF.CollectionLevel2_Id

										CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeral2 (CollectionLevel2_Id);

					-- Criação da Fato de Coleta x Cluster

					SELECT
						C2XC.Id
					   ,C2XC.CollectionLevel2_Id
					   ,C2XC.ParCluster_Id INTO #CollectionLevel2XCluster
					FROM CollectionLevel2XCluster C2XC WITH (NOLOCK)
					INNER JOIN #CollectionLevel2 C2 WITH (NOLOCK)
						ON C2XC.CollectionLevel2_Id = C2.Id

										CREATE INDEX IDX_CollectionLevel2XCluster_Cluster_ID ON #CollectionLevel2XCluster (CollectionLevel2_Id);


					-- Cubo

					SELECT
						C2.CollectionDate AS Data
					   ,L1.Name AS Indicador
					   ,L2.Name AS Monitoramento
					   ,PC.Name AS ClusterName
					   ,R3.ParLevel3_Name AS Tarefa
					   ,PF.Name AS Frequencia
					   ,R3.Weight AS Peso
					   ,CASE
							WHEN R3.IntervalMin = '-9999999999999.9000000000' THEN ''
							ELSE R3.IntervalMin
						END AS 'IntervaloMinimo'
					   ,CASE
							WHEN R3.IntervalMax = '9999999999999.9000000000' THEN ''
							ELSE R3.IntervalMax
						END AS 'IntervaloMaximo'

					   ,R3.Value AS 'Lancado'
					   ,R3.IsConform AS 'Conforme'
					   ,R3.IsNotEvaluate AS 'NA'
					   ,R3.WeiEvaluation AS 'AV_Peso'
					   ,R3.WeiDefects AS 'NC_Peso'
					   ,CASE
							WHEN ISNULL(R3.ValueText, '') IN ('undefined', 'null') THEN ''
							ELSE ISNULL(R3.ValueText, '')
						END
						AS ValueText
					   ,C2.EvaluationNumber AS 'Avaliacao'
					   ,C2.Sample AS 'Amostra'
					   ,ISNULL(C2.Sequential, 0) AS 'Sequencial'
					   ,ISNULL(C2.Side, 0) AS 'Banda'
					   ,STR(C2.[Shift]) AS 'Turno'
					   ,STR(C2.Period) AS 'Periodo'
					   ,UN.Name AS 'Unidade'
					   ,R3.Id AS 'ResultLevel3Id'
					   ,US.Name AS 'Auditor'
					   ,ISNULL(L1.HashKey, '') AS 'HashKey'
					   ,ISNULL(HF.HeaderFieldList, '') AS 'HeaderFieldList'
					   ,C2.AddDate AS AddDate
					   ,CJ.AppVersion AS Platform
					   ,CASE
							WHEN CJ.AppVersion = 'Excel' THEN '4'
							WHEN C2.AlterDate IS NOT NULL THEN '1'
							WHEN CAST(C2.AddDate AS DATE) > CAST(C2.CollectionDate AS DATE) THEN '2'
							WHEN CAST(C2.AddDate AS DATE) < CAST(C2.CollectionDate AS DATE) THEN '3'
							ELSE '0'
						END
						AS Type
					   ,PC.Name AS Processo
					   ,(SELECT TOP 1
								PL3V.ParLevel3InputType_Id
							FROM ParLevel3Value PL3V
							WHERE 1 = 1
							AND (ISNULL(PL3V.ParCompany_Id, UN.Id) = UN.Id)
							AND (ISNULL(PL3V.ParLevel1_Id, L1.Id) = L1.Id)
							AND (ISNULL(PL3V.ParLevel2_Id, L2.Id) = L2.Id)
							AND PL3V.ParLevel3_Id = L3.Id
							AND PL3V.IsActive = 1
							ORDER BY PL3V.Id DESC, PL3V.ParCompany_Id DESC, PL3V.ParLevel2_Id DESC, PL3V.ParLevel1_Id DESC)
						AS ParLevel3InputType_Id
					   ,CASE
							WHEN MA.Motivo IS NULL THEN 0
							ELSE 1
						END AS IsLate
					   ,CASE
							WHEN (SELECT TOP 1
										Id
									FROM Result_Level3_Photos RL3P
									WHERE RL3P.Result_Level3_Id = R3.Id)
								IS NOT NULL THEN 1
							ELSE 0
						END AS HasPhoto
					   ,MA.Motivo AS ParReason
					   ,PRT.Name AS ParReasonType
					   ,PCargo.Name AS Cargo
					   ,Secao.Name AS Secao
					   ,Centro.Name as CentroCusto
					   ,pgc.Name as GrupoCluster
					   ,psg.Name as GrupoEmpresa
					   ,pg.Name as regional
					FROM #CollectionLevel2 C2 (NOLOCK)
					INNER JOIN ParCompany UN with (NOLOCK)
						ON UN.Id = C2.UnitId
					left join ParCompanyXStructure PCXS with (NOLOCK)
						on pcxs.ParCompany_Id = un.Id and pcxs.Active = 1
					left join ParStructure pg with (NOLOCK)
						on pg.Id = pcxs.ParStructure_Id
					left join ParStructure psg with (NOLOCK)
						on psg.Id = pg.ParStructureParent_Id
					INNER JOIN #Result_Level3 R3 with (NOLOCK)
						ON R3.CollectionLevel2_Id = C2.Id
					INNER JOIN ParLevel3 L3 with (NOLOCK)
						ON L3.Id = R3.ParLevel3_Id
					INNER JOIN ParLevel2 L2 with (NOLOCK)
						ON L2.Id = C2.ParLevel2_Id
					INNER JOIN ParLevel1 L1 with (NOLOCK)
						ON L1.Id = C2.ParLevel1_Id
					INNER JOIN UserSgq US with (NOLOCK)
						ON C2.AuditorId = US.Id
					LEFT JOIN #CollectionLevel2XParHeaderFieldGeral2 HF with (NOLOCK)
						ON C2.Id = HF.CollectionLevel2_Id
					LEFT JOIN #CollectionLevel2XCollectionJson CLCJ with (NOLOCK)
						ON CLCJ.CollectionLevel2_Id = C2.Id
					LEFT JOIN #CollectionJson CJ with (NOLOCK)
						ON CJ.Id = CLCJ.CollectionJson_Id
					LEFT JOIN #CollectionLevel2XCluster C2XC with (NOLOCK)
						ON C2XC.CollectionLevel2_Id = C2.Id
					LEFT JOIN  ParCluster PC with (NOLOCK)
						ON PC.Id = C2XC.ParCluster_Id
					LEFT join ParClusterGroup PGC with (NOLOCK)
						on pgc.id = pc.ParClusterGroup_Id
					LEFT JOIN CollectionLevel2XParReason CL2MA with (NOLOCK)
						ON CL2MA.CollectionLevel2_Id = C2.Id
					LEFT JOIN ParReason MA with (NOLOCK)
						ON MA.Id = CL2MA.ParReason_Id
					LEFT JOIN ParReasonType PRT with (NOLOCK)
						ON PRT.Id = MA.ParReasonType_Id
					LEFT JOIN CollectionLevel2XParDepartment CL2PD with (NOLOCK)
						ON CL2PD.CollectionLevel2_Id = C2.Id
					LEFT JOIN ParDepartment Secao with (NOLOCK)
						ON Secao.Id = CL2PD.ParDepartment_Id
					LEFT JOIN ParDepartment Centro with (NOLOCK)
						ON Secao.Parent_Id = Centro.Id
							AND Centro.Parent_Id IS NULL
					left JOIN CollectionLevel2XParCargo CL2PC with (NOLOCK)
						ON CL2PC.CollectionLevel2_Id = C2.Id
					LEFT JOIN ParCargo PCargo with (NOLOCK)
						ON PCargo.Id = CL2PC.ParCargo_Id
					LEFT JOIN ParFrequency PF with (NOLOCK)
						ON C2.ParFrequency_Id = PF.Id
					WHERE 1=1 
					{ sqlDepartment }
					{ sqlCargo }
					{ sqlLevel3 }
					{ sqlClusterGroup}    
					{ sqlParStructure2}	  
					{ sqlParStructure3}	  
					{ sqlAuditor}	

					DROP TABLE #CollectionLevel2
					DROP TABLE #CollectionJson
					DROP TABLE #Result_Level3
					DROP TABLE #CollectionLevel2XParHeaderFieldGeral
					DROP TABLE #CollectionLevel2XParHeaderFieldGeral2
					DROP TABLE #CollectionLevel2XCluster
					DROP TABLE #CollectionLevel2XCollectionJson

                ";

        return query;
    }

    public string SelectGraficoUnidade(DataCarrierFormularioNew form)
    {
        var whereDepartment = "";
        var whereSecao = "";
        var whereCargo = "";
        var whereStructure = "";
        var whereUnit = "";
		var whereTurno = "";
		var whereParLevel1 = "";
        var whereParLevel2 = "";
        var whereParLevel3 = "";
        var campos = "";
        var campos2 = "";
        var campos3 = ", null as data ";
        var groupBy = "";
        var groupBy2 = " GROUP BY data, c.parlevel1_id, parlevel2_id, parcompany_id  ";
        var orderBy = "";
        var selects = "";
        var selectTotal = "";
        var campos4 = "CAST(AVG(PESOTOTAL) AS DECIMAL(20,2)) AS PESOTOTAL, CAST(AVG(TOTAL) AS DECIMAL(20,2)) AS TOTAL";
        var groupBy3 = "";

        if (form.ShowModeloGrafico_Id[0] == 2)
        {
            campos = $@" cast(year(CUBOL3.CollectionDate) as varchar) + '-' + case when LEN(cast(month(CUBOL3.CollectionDate) as varchar)) = 1 then '0' + cast(month(CUBOL3.CollectionDate) as varchar) else cast(month(CUBOL3.CollectionDate) as varchar) end  AS UnidadeName, 0 as Unidade_Id ";
            campos2 = $@" , Data, null as parlevel1_id, null as parlevel2_id, null as parcompany_id ";
            campos3 = $@" , cast(year(CUBOL3.CollectionDate) as varchar) + '-' + case when LEN(cast(month(CUBOL3.CollectionDate) as varchar)) = 1 then '0' + cast(month(CUBOL3.CollectionDate) as varchar) else cast(month(CUBOL3.CollectionDate) as varchar) end  AS Data ";
            groupBy = $@" GROUP BY cast(year(CUBOL3.CollectionDate) as varchar) + '-' + case when LEN(cast(month(CUBOL3.CollectionDate) as varchar)) = 1 then '0' + cast(month(CUBOL3.CollectionDate) as varchar) else cast(month(CUBOL3.CollectionDate) as varchar) end ";
            orderBy = "ORDER BY 1 ASC";
            selectTotal = " SELECT * FROM #RR1 A INNER JOIN #RR2 B ON A.UNIDADEname = B.Data ORDER BY 1 ASC ";
            campos4 = " sum(PESOTOTAL) AS PESOTOTAL, sum(TOTAL) AS TOTAL";
        }
        else if (form.ShowModeloGrafico_Id[0] == 3)
        {
            campos = $@" cast(year(CUBOL3.CollectionDate) as varchar) + '-' + case when LEN(cast(datepart(week,CUBOL3.CollectionDate) as varchar)) = 1 then '0' + cast(datepart(week,CUBOL3.CollectionDate) as varchar) else cast(datepart(week,CUBOL3.CollectionDate) as varchar) end  AS UnidadeName, 0 as Unidade_Id ";
            campos2 = $@" , Data, null as parlevel1_id, null as parlevel2_id, null as parcompany_id  ";
            campos3 = $@" , cast(year(CUBOL3.CollectionDate) as varchar) + '-' + case when LEN(cast(datepart(week,CUBOL3.CollectionDate) as varchar)) = 1 then '0' + cast(datepart(week,CUBOL3.CollectionDate) as varchar) else cast(datepart(week,CUBOL3.CollectionDate) as varchar) end  AS Data ";
            groupBy = $@" GROUP BY cast(year(CUBOL3.CollectionDate) as varchar) + '-' + case when LEN(cast(datepart(week,CUBOL3.CollectionDate) as varchar)) = 1 then '0' + cast(datepart(week,CUBOL3.CollectionDate) as varchar) else cast(datepart(week,CUBOL3.CollectionDate) as varchar) end ";
            orderBy = "ORDER BY 1 ASC";
            selectTotal = " SELECT * FROM #RR1 A INNER JOIN #RR2 B ON A.UNIDADEname = B.Data ORDER BY 1 ASC ";
            campos4 = " sum(PESOTOTAL) AS PESOTOTAL, sum(TOTAL) AS TOTAL";
        }
        else if (form.ShowModeloGrafico_Id[0] == 4)
        {
            campos = $@" convert(varchar, CUBOL3.CollectionDate ,103) UnidadeName, 0 as Unidade_Id ";
            campos2 = $@" , Data, null as parlevel1_id, null as parlevel2_id, null as parcompany_id  ";
            campos3 = $@" , convert(varchar, CUBOL3.CollectionDate ,103) Data ";
            groupBy = $@" GROUP BY convert(varchar, CUBOL3.CollectionDate ,103)	,C.Name ";
            orderBy = "ORDER BY 1 ASC";
            selectTotal = " SELECT *, CONCAT(RIGHT(B.DATA,4),RIGHT(LEFT(B.DATA,5),2),LEFT(B.DATA,2)) DATAORDEM  FROM #RR1 A INNER JOIN #RR2 B ON A.UNIDADEname = B.Data ORDER BY 14 ASC ";

			selectTotal = @"
							SELECT UnidadeName, Unidade_Id, sum(av) AV, SUM(NC) NC, SUM (C) C, SUM (C) / SUM (AV) PORCC, SUM (NC) / SUM (AV) PORCNC, PESOTOTAL, TOTAL, Parcompany_id, parlevel1_id, parlevel2_id, data, CONCAT(RIGHT(B.DATA,4),RIGHT(LEFT(B.DATA,5),2),LEFT(B.DATA,2)) DATAORDEM  FROM #RR1 A INNER JOIN #RR2 B ON A.UNIDADEname = B.Data 
							group by UnidadeName, Unidade_Id, PESOTOTAL, TOTAL, Parcompany_id, parlevel1_id, parlevel2_id, data
							ORDER BY 14 ASC	
					";
			
			campos4 = " sum(PESOTOTAL) AS PESOTOTAL, sum(TOTAL) AS TOTAL";
        }
        else if (form.ShowDimensaoGrafico_Id.Length > 0)
        {
            switch (form.ShowDimensaoGrafico_Id[0])
            {
                case 0: //UNIDADES
                    campos = $@" C.NAME AS UnidadeName, C.Id as Unidade_Id ";
                    campos2 = $@" ,C.ParCompany_Id as Parcompany_id, null as parlevel1_id, null as parlevel2_id, null as data ";
                    groupBy = $@" GROUP BY C.NAME, C.Id ";
                    groupBy2 = $@" GROUP BY C.ParCompany_Id, data ";
                    orderBy = "ORDER BY 4 DESC";
                    selectTotal = " SELECT * FROM #RR1 A INNER JOIN #RR2 B ON A.UNIDADE_ID = B.PARCOMPANY_ID ORDER BY TOTAL DESC ";
                    break;

                case 1: //INDICADORES
                    campos = $@" PL1.NAME AS UnidadeName, PL1.Id as Unidade_Id ";
                    campos2 = $@" ,null as Parcompany_id, C.parlevel1_id as parlevel1_id, null as parlevel2_id, null as data  ";
                    groupBy = $@" GROUP BY PL1.NAME, PL1.Id ";
                    groupBy2 = $@" GROUP BY C.ParLevel1_Id, data ";
                    orderBy = "ORDER BY 4 DESC";
                    selectTotal = " SELECT * FROM #RR1 A INNER JOIN #RR2 B ON A.UNIDADE_ID = B.PARLevel1_id  ORDER BY TOTAL DESC ";
                    break;

                case 2: //MONITORAMENTOS
                    campos = $@" PL2.NAME AS UnidadeName, PL2.Id as Unidade_Id ";
                    campos2 = $@" ,null as Parcompany_id, null as parlevel1_id, C.parlevel2_id as parlevel2_id , null as data  ";
                    groupBy = $@" GROUP BY PL2.NAME, PL2.Id ";
                    groupBy2 = $@" GROUP BY C.ParLevel2_id, data ";
                    orderBy = "ORDER BY 4 DESC";
                    selectTotal = " SELECT * FROM #RR1 A INNER JOIN #RR2 B ON A.UNIDADE_ID = B.PARlevel2_ID  ORDER BY TOTAL DESC ";
                    break;

                case 3: //TAREFAS
                    //campos = $@" PL3.NAME AS UnidadeName, PL3.Id as Unidade_Id ";
                    
                    //groupBy = $@" GROUP BY PL3.NAME, PL3.Id ";
                    
                    //orderBy = "ORDER BY 4 DESC";

                    campos = $@" PL3.NAME AS UnidadeName, PL3.Id as Unidade_Id ";
                    campos2 = $@" ,null as Parcompany_id, null as parlevel1_id, C.parlevel2_id as parlevel2_id , null as data  ";
                    groupBy = $@" GROUP BY PL3.NAME, PL3.Id ";
                    groupBy2 = $@" GROUP BY C.ParLevel2_id, data ";
                    orderBy = "ORDER BY 4 DESC";
                    selectTotal = " SELECT *, NULL AS PESOTOTAL, NULL AS TOTAL FROM #RR1 A ORDER BY NC DESC  ";
                    

                    break;

                case 4: //FAMÍLIA DE PRODUTO
                    selects = $@", ParFamiliaProduto_Id";
                    campos = $@" SFP.NAME AS UnidadeName, C.Id as Unidade_Id , CSFP.ParFamiliaProduto_Id ";
                   
                    groupBy = $@" GROUP BY SFP.Name, C.Id, CSFP.ParFamiliaProduto_Id ";
                    orderBy = "ORDER BY 4 DESC";
                    break;

                case 5: //SKU
                    //selects = $@", ParProduto_Id";
                    //campos = $@" SP.NAME AS UnidadeName, C.Id as Unidade_Id , CSFP.ParProduto_Id ";
                   
                    //groupBy = $@" GROUP BY SP.Name, C.Id, CSFP.ParProduto_Id ";
                    //orderBy = "ORDER BY 4 DESC";

                    campos = $@" SKU AS UnidadeName, CUBOL3.ParProduto_Id as Unidade_Id ";
                    campos2 = $@" ,null as Parcompany_id, null as parlevel1_id, NULL as parlevel2_id , null as data  ";
                    groupBy = $@" GROUP BY SKU, CUBOL3.ParProduto_Id ";
                    groupBy2 = $@" GROUP BY SKU, data ";
                    orderBy = "ORDER BY 4 DESC";
                    selectTotal = "  SELECT * FROM #RR1 A INNER JOIN #RR2 B ON A.UnidadeName = B.SKU  ORDER BY TOTAL DESC  ";
                    campos4 = "SKU, AVG(PESOTOTAL) AS PESOTOTAL, AVG(TOTAL) AS TOTAL";
                    groupBy3 = ", SKU";
               

                    break;

				case 6: //GRUPO DE TAREFAS
						//campos = $@" PL3.NAME AS UnidadeName, PL3.Id as Unidade_Id ";

					//groupBy = $@" GROUP BY PL3.NAME, PL3.Id ";

					//orderBy = "ORDER BY 4 DESC";

					campos = $@" CUBOL3.GRUPOTAREFA AS UnidadeName, CUBOL3.GRUPOTAREFA as Unidade_Id ";
					campos2 = $@" ,null as Parcompany_id, null as parlevel1_id, C.parlevel2_id as parlevel2_id , null as data  ";
					groupBy = $@" GROUP BY CUBOL3.GRUPOTAREFA, CUBOL3.GRUPOTAREFA ";
					groupBy2 = $@" GROUP BY C.ParLevel2_id, data ";
					orderBy = "ORDER BY 4 DESC";
					selectTotal = " SELECT *, NULL AS PESOTOTAL, NULL AS TOTAL FROM #RR1 A  ORDER BY NC DESC   ";


					break;

				default:
                    break;
            }


        }


        if (form.ParLevel1_Ids.Length > 0)
        {
            whereParLevel1 = $@" AND CUBOL3.Parlevel1_Id in ({string.Join(",", form.ParLevel1_Ids)}) ";
        }

        if (form.ParLevel2_Ids.Length > 0)
        {
            whereParLevel2 = $@" AND CUBOL3.Parlevel2_Id in ({string.Join(",", form.ParLevel2_Ids)}) ";
        }

        if (form.ParLevel3_Ids.Length > 0)
        {
            whereParLevel3 = $@" AND CUBOL3.Parlevel3_Id in ({string.Join(",", form.ParLevel3_Ids)}) ";
        }

        if (form.ParDepartment_Ids.Length > 0)
        {
            whereDepartment = $@" AND CUBOL3.Centro_De_Custo_Id in ({string.Join(",", form.ParDepartment_Ids)}) ";
        }

        if (form.ParSecao_Ids.Length > 0)
        {
            whereSecao = $@" AND CUBOL3.Secao_Id in ({string.Join(",", form.ParSecao_Ids)}) ";
        }

        if (form.ParCargo_Ids.Length > 0)
        {
            whereCargo = $@" AND CUBOL3.Cargo_Id in ({string.Join(",", form.ParCargo_Ids)}) ";
        }

        if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] > 0)
        {
            whereUnit = $@"AND CUBOL3.UnitId in ({ string.Join(",", form.ParCompany_Ids) }) ";
        }

		if (form.ShowTurnoSeara[0].ToString() != "-1")
		{
			whereTurno = $@"AND CUBOL3.HeaderFieldList LIKE '%{ form.ShowTurnoSeara[0] }%' ";
		}


		if (form.ParStructure2_Ids.Length > 0)
        {
            whereStructure = $@"AND CUBOL3.Regional in ({string.Join(",", form.ParStructure2_Ids)})";
        }

        var query = $@"

            DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
            DECLARE @DATAFINAL   DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'

			-- Criação da Fato de Cabeçalhos
					SELECT
						CL2HF.Id
					   ,CL2HF.CollectionLevel2_Id
					   ,CL2HF.ParHeaderFieldGeral_Id
					   ,CL2HF.ParFieldType_Id
					   ,CL2HF.Value INTO #CollectionLevel2XParHeaderFieldGeral
					FROM CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
					INNER JOIN Collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF.CollectionLevel2_Id
						WHERE CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
						
										CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel_ID ON #CollectionLevel2XParHeaderFieldGeral (CollectionLevel2_Id);
					-- Concatenação da Fato de Cabeçalhos
			-- Concatenação da Fato de Cabeçalhos
					SELECT
						CL2HF.CollectionLevel2_Id
					   ,STUFF((SELECT DISTINCT
								', ' + CONCAT('', CASE
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
							AND CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
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
						AND CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
					GROUP BY CL2HF.CollectionLevel2_Id
										CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeralTURNO (CollectionLevel2_Id);


            SELECT CAST(C2.CollectionDate AS DATETIME) AS CollectionDate ,
               C2.ParFrequency_Id ,
               S2.ParStructureParent_Id AS Holding ,
               S1.ParStructureParent_Id AS GrupoDeEmpresa ,
               CS.ParStructure_Id AS Regional ,
               C2.UnitId ,
               D.Parent_Id AS Centro_De_Custo_Id ,
               C2XDP.ParDepartment_Id AS Secao_Id ,
               C2XCG.ParCargo_Id AS Cargo_Id ,
               C2.ParLevel1_Id ,
               C2.ParLevel2_Id ,
               R3.ParLevel3_Id ,
               C2.AuditorId ,
               C2.Sample WeiEvaluation,
               R3.Defects WeiDefects,
	           cfpp.ParFamiliaProduto_Id,
	           cfpp.ParProduto_Id,
			   C2.ID AS CollectionLevel2_Id,
			   R3V.LimiteNC,
			   PL2P.Equacao,
			   PL2P.Peso,
			   c2.evaluationNumber as Avaliacao,
			   pp.Name as SKU,
			   PGL1.Name as GRUPOTAREFA,
			   HFTURNO.HeaderFieldList	
	           INTO #CUBOLEVEL3
        FROM CollectionLevel2 C2 WITH (NOLOCK)
        INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON C2.Id = R3.CollectionLevel2_Id
		LEFT JOIN #CollectionLevel2XParHeaderFieldGeralTURNO HFTURNO ON HFTURNO.CollectionLevel2_Id = C2.Id
		INNER JOIN parlevel3value R3V WITH (NOLOCK) ON R3V.parlevel3_id = R3.parlevel3_id and R3V.ParLevel1_id = C2.ParLevel1_id and R3V.parlevel2_id = C2.ParLevel2_id and R3V.IsActive = 1 
		INNER JOIN ParVinculoPesoParLevel2 PL2P WITH (NOLOCK) ON PL2P.ParLevel1_Id = C2.ParLevel1_Id AND PL2P.ParLevel2_Id = C2.ParLevel2_Id AND PL2P.IsActive = 1

        LEFT JOIN CollectionLevel2XParDepartment C2XDP WITH (NOLOCK) ON C2.ID = C2XDP.CollectionLevel2_Id
        LEFT JOIN CollectionLevel2XParCargo C2XCG WITH (NOLOCK) ON C2.ID = C2XCG.CollectionLevel2_Id
        LEFT JOIN ParDepartment D WITH (NOLOCK) ON C2XDP.ParDepartment_Id = D.Id
        LEFT JOIN ParCompanyXStructure CS ON CS.ParCompany_Id = c2.UnitId and cs.active = 1
        LEFT JOIN ParStructure S1 ON CS.ParStructure_Id = S1.Id
        LEFT JOIN ParStructure S2 ON S1.ParStructureParent_Id = S2.Id
        LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CFPP on cfpp.CollectionLevel2_Id = c2.Id
        
        LEFT JOIN ParProduto PP on pp.Id = cfpp.ParProduto_Id

		LEFT JOIN ParVinculoPeso PVP 
		ON PVP.ParLevel1_Id = C2.ParLevel1_Id
		AND PVP.ParLevel2_Id = C2.ParLevel2_Id
		AND PVP.ParLevel3_Id = r3.ParLevel3_Id
		AND PVP.IsActive = 1
		LEFT JOIN ParGroupParLevel1 PGL1
		ON PGL1.Id = PVP.ParGroupParLevel1_Id

        WHERE 1=1
          AND R3.IsNotEvaluate = 0
          AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
		
		DROP TABLE #CollectionLevel2XParHeaderFieldGeral 
		DROP TABLE #CollectionLevel2XParHeaderFieldGeralTURNO
     --------------------------------
                 

                SELECT
					UnidadeName
					,Unidade_Id
					,CAST(AV AS DECIMAL(20,2)) AS AV
					,CAST((NC) AS DECIMAL(20,2)) AS NC
					,CAST(C AS DECIMAL(20,2)) AS C
					,CAST((C / AV) * 100 AS DECIMAL(20,2)) AS PORCC
					,CAST((NC / AV) * 100 AS DECIMAL(20,2)) AS PORCNC
                    {selects}
				INTO #RR1 FROM (SELECT 
	                {campos}
                    ,SUM(CUBOL3.WeiEvaluation) AS AV	
					,SUM(CUBOL3.WeiDefects) AS NC	
					,SUM(CUBOL3.WeiEvaluation) - SUM(CUBOL3.WeiDefects) AS C	
                    	
	        FROM #CUBOLEVEL3 CUBOL3 WITH (NOLOCK)	
					INNER JOIN ParCompany C WITH (NOLOCK)	
						ON CUBOL3.UnitId = C.Id	
					INNER JOIN ParLevel1 PL1 WITH (NOLOCK)	
						ON CUBOL3.ParLevel1_Id = PL1.Id	
					INNER JOIN ParLevel2 PL2 WITH (NOLOCK)	
						ON CUBOL3.ParLevel2_Id = PL2.Id	
					INNER JOIN ParLevel3 PL3 WITH (NOLOCK)	
						ON CUBOL3.ParLevel3_Id = PL3.Id	
                    left JOIN ParFamiliaProduto SFP WITH (NOLOCK)	
	                    ON SFP.Id = CUBOL3.ParFamiliaProduto_id	
                    left JOIN ParProduto SP WITH (NOLOCK)	
	                    ON  SP.Id = CUBOL3.ParProduto_Id	
					WHERE 1 = 1
					AND CUBOL3.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}
                    {groupBy}) CUBO
                    {orderBy}

SELECT -- MONITORAMENTO
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
					   AVG(CUBOL3.Peso) Peso

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

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}
                    
                
                    GROUP BY CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id
				) a
				GROUP BY parlevel1_id,
					parlevel2_id
				) B
				GROUP BY B.PARLEVEL1_ID

SELECT 
{campos4}
,Parcompany_id 
,parlevel1_id 
,parlevel2_id
--,SKU
,data

INTO #RR2 FROM (

SELECT -- INDICADOR
    
	--C.ParCompany_Id,
	--C.ParLevel1_Id,
	SUM(C.PESO) AS [PESOTOTAL]
	,SUM( (C.PESO / PM.PESO) * CASE WHEN C.PARLEVEL2_ID = 605 THEN RESPOSTA3 ELSE RESPOSTA2 END) TOTAL -- 0 98,125 0 100 97,5
 , DATA1, AVALIACAO, SKU
    {campos2}
	
	FROM (

            SELECT -- MONITORAMENTO
			B.ParCompany_Id,
			B.PARLEVEL1_ID,
			B.PARLEVEL2_ID,
            B.SKU,
            b.data,
			avg(Peso) Peso,
			COUNT(DISTINCT(B.PARLEVEL3_ID)) [NÚMERO DE TAREFAS],

			SUM(IIF(RESPOSTA = 1, 1, 0)) SOMA,

			CASE 
				WHEN MIN(RESPOSTA) = 0 THEN 0 --ESTOREI O LIMITE
				WHEN SUM(RESPOSTA) = COUNT(DISTINCT(B.PARLEVEL3_ID)) THEN 100 --TIREI NOTA MÁXIMA
				ELSE 95 + (SUM(IIF(RESPOSTA = 1, 1, 0)) / cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) * 5) -- NEM ESTOUREI, NEM ZEREI
			END RESPOSTA2,
			   -- QtdeTLNC > 0 ? 0 : (QtdeNC == 0 ? 100 : 95 + (QtdeTC/QtdeT*5))

			CASE 
				WHEN max(RESPOSTA) = 1 and min(RESPOSTA) = 1 THEN 100 --TIREI NOTA MÁXIMA
				WHEN (cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) - 1) = 0 THEN 0
				ELSE 60 * (SUM(IIF(RESPOSTA = 1, 1, 0)) / (cast(COUNT(DISTINCT(B.PARLEVEL3_ID)) as FLOAT) - 1)) -- NEM ESTOUREI, NEM ZEREI
			END RESPOSTA3
 , DATA1, AVALIACAO
			   --(QtdeNC == 0 ? 100 : 60 * (QtdeTC / (QtdeT -1)))

			   FROM (
			   
			    SELECT -- TAREFA

					parCompany_Id,
					parlevel1_id,
					parlevel2_id,
					parlevel3_id,
                    SKU,
                    data,
					AV,
					Defeitos,
					LimiteNC,
					Peso,
					
					CASE 
						WHEN LimiteNC < Defeitos THEN 0 --ESTOREI O LIMITE
						WHEN Defeitos = 0 THEN 1 --TIREI NOTA MÁXIMA
						ELSE 2 -- NEM ESTOUREI, NEM ZEREI
					END RESPOSTA
 , DATA1, AVALIACAO

					 

				FROM (
				
					SELECT
					   
					   CUBOL3.UnitId as parcompany_id,
					   CUBOL3.parlevel1_id, 
					   CUBOL3.parlevel2_id, 
					   CUBOL3.parlevel3_id,
                       CUBOL3.SKU,
					   sum(CUBOL3.WeiEvaluation) AV, 
					   sum(CUBOL3.WeiDefects) Defeitos,
					   AVG(CUBOL3.LimiteNC) LimiteNC,
					   AVG(CUBOL3.Peso) Peso
, CAST(CUBOL3.COLLECTIONDATE AS DATE) DATA1, CUBOL3.AVALIACAO
					  {campos3}


					  

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

                    {whereStructure}
                    {whereUnit}
					{whereTurno}
                    {whereDepartment}
                    {whereSecao}
                    {whereCargo}
                    {whereParLevel1}
                    {whereParLevel2}
                    {whereParLevel3}
                    {groupBy}
                
                    , CUBOL3.UnitId, CUBOL3.parlevel1_id, CUBOL3.parlevel2_id, CUBOL3.parlevel3_id, CUBOL3.SKU, CUBOL3.Centro_De_Custo_Id, CUBOL3.Secao_Id, CUBOL3.Cargo_Id, CUBOL3.UnitId, CUBOL3.Equacao, CUBOL3.Avaliacao, CAST(CUBOL3.COLLECTIONDATE AS DATE)
				) a

				) B
				GROUP BY B.parcompany_id, B.PARLEVEL1_ID,  B.PARLEVEL2_ID, data, DATA1, AVALIACAO, SKU

		) C
		INNER JOIN #PESOMONITORAMENTOINDICADOR PM ON PM.ParLevel1_Id = C.ParLevel1_Id
		{groupBy2} , DATA1, AVALIACAO, C.PARLEVEL1_ID, C.SKU

) D

GROUP BY Parcompany_id 
,parlevel1_id 
,parlevel2_id
--,SKU
,data
{groupBy3}

		DROP TABLE #PESOMONITORAMENTOINDICADOR
		DROP TABLE #CUBOLEVEL3  

   {selectTotal}
   
   DROP TABLE #RR1    
   DROP TABLE #RR2

                ";
        return query;
    }

}
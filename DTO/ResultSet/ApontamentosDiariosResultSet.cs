using DTO;
using System;
using System.Linq;

public class ApontamentosDiariosResultSet
{
    public System.DateTime Data { get; set; }
    public string _Data { get { return Data.ToShortDateString(); /*+ " " + Data.ToShortTimeString();*/ } }
    public string _Hora { get { return Data.ToShortTimeString(); } }

    public string Indicador { get; set; }
    public string Monitoramento { get; set; }
    public string Tarefa { get; set; }
    public Nullable<decimal> Peso { get; set; }
    public string IntervaloMinimo { get; set; }
    public string IntervaloMaximo { get; set; }
    public string Lancado { get; set; }
    public string ClusterName { get; set; }

    public Nullable<bool> Conforme { get; set; }
    public string _Conforme { get { return Conforme.Value ? Resources.Resource.according : Resources.Resource.not_accordance; } }

    public Nullable<bool> NA { get; set; }
    public string _NA { get { return NA.Value ? Resources.Resource.unvalued : Resources.Resource.valued; } }

    public Nullable<decimal> AV_Peso { get; set; }
    public Nullable<decimal> _AV_Peso { get { return AV_Peso.HasValue ? AV_Peso.Value : 0M; } }

    public Nullable<decimal> NC_Peso { get; set; }
    public Nullable<int> Avaliacao { get; set; }
    public int Amostra { get; set; }
    public Nullable<int> Sequencial { get; set; }
    public Nullable<int> Banda { get; set; }
    public int ResultLevel3Id { get; set; }
    public Nullable<int> HashKey { get; set; }

    public string Unidade { get; set; }
    public string Periodo { get; set; }
    public string Turno { get; set; }
    public string Auditor { get; set; }
    public string ValueText { get; set; }
    public string HeaderFieldList { get; set; }
    public string HeaderFieldListL1 { get; set; }
    public string HeaderFieldListL2 { get; set; }
    public string HeaderFieldListL3 { get; set; }

    public System.DateTime AddDate { get; set; }
    public string _AddDate { get { return AddDate.ToShortDateString(); /*+ " " + Data.ToShortTimeString();*/ } }
    public string Platform { get; set; }
    public string Type { get; set; }
    public string Processo { get; set; }

    public string CentroCusto { get; set; }

    public string GrupoCluster { get; set; }

    public string GrupoEmpresa { get; set; }

    public string regional { get; set; }

    public string Secao { get; set; }

    public int? ParLevel3InputType_Id { get; set; }

    public bool IsLate { get; set; }
    public string ParReason { get; set; }
    public string ParReasonType { get; set; }

    public string Cargo { get; set; }
    public string Departamento { get; set; }
    public string Frequencia { get; set; }

    public bool HasPhoto { get; set; }
    public bool HasHistoryResult_Level3 { get; set; }
    public bool HasHistoryHeaderField { get; set; }
    public bool HasHistoryParProduto { get; set; }

    public bool? IsNcTextRequired { get; set; }

    public bool? IsRequired { get; set; }
    public bool? IsAtiveNa { get; set; }

    public int C2ID { get; set; }
    public string DesvioAv { get; set; }

    public string CriticalLevel { get; set; }

    public string Qualification_Group { get; set; }

    public string ParProduto { get; set; }
    public int? ParFamiliaProduto_Id { get; set; }
    public int? ParProduto_Id { get; set; }
    public int? CollectionLevel2_Id { get; set; }


    public string Select(DataCarrierFormulario form)
    {
        var dtInit = form._dataInicio.ToString("yyyyMMdd");
        var dtF = form._dataFim.ToString("yyyyMMdd");

        var sqlTurno = "";
        var sqlUnidade = "";
        var sqlLevel1 = "";
        var sqlLevel2 = "";
        var sqlLevel3 = "";
        var formatDate = "";

        if (form.shift > 0)
        {
            sqlTurno = "\n AND [Shift] = " + form.shift;
        }

        if (form.unitId > 0)
        {
            sqlUnidade = "\n AND UnitId = " + form.unitId;
        }

        if (form.level1Id > 0)
        {
            sqlLevel1 = "\n AND ParLevel1_id = " + form.level1Id;
        }

        if (form.level2Id > 0)
        {
            sqlLevel2 = "\n AND ParLevel2_Id = " + form.level2Id;
        }

        if (form.level3Id > 0)
        {
            sqlLevel3 = "\n AND L3.Id = " + form.level3Id;
        }

        if (GlobalConfig.Eua)
        {
            formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 101)";
        }
        else
        {
            formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 103)";
        }

        var query = $@" 


                    -- DROP TABLE #CollectionLevel2

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
                    FROM Collectionlevel2 CL2 WITH (NOLOCK)
                        WHERE 1=1
                         AND NotEvaluatedIs <> 999
                         AND Duplicated <> 999
                         AND CL2.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF }  23:59:59'
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
						R3.ID
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
						,R3.WeiDefects
						INTO #Result_Level3
						FROM Result_Level3 R3 WITH (NOLOCK)
						INNER JOIN #CollectionLevel2 C2
							ON R3.CollectionLevel2_Id = C2.Id	

                    CREATE INDEX IDX_Result_Level3_CollectionLevel2_ID ON #Result_Level3(CollectionLevel2_Id);
                    CREATE INDEX IDX_Result_Level3_CollectionLevel2_Lvl3_ID ON #Result_Level3(CollectionLevel2_Id,Parlevel3_Id);


                    -- CollectionLevel2XCollectionJson

                    SELECT 
						CollectionLevel2_Id
						,CollectionJson_Id as CollectionJson_Id 
						,ROW_NUMBER() OVER (PARTITION BY CollectionLevel2_Id ORDER BY CollectionJson_Id DESC) AS [ROW]

                    INTO #CollectionLevel2XCollectionJson
					FROM CollectionLevel2XCollectionJson C2CJ WITH(NOLOCK)

                    INNER JOIN #CollectionLevel2 C2
						ON C2.Id = C2CJ.CollectionLevel2_Id



                    DELETE FROM #CollectionLevel2XCollectionJson WHERE [ROW] > 1

                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionLevel2_ID ON #CollectionLevel2XCollectionJson(CollectionLevel2_Id);
                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionLevel2CollectionJson_ID ON #CollectionLevel2XCollectionJson(CollectionLevel2_Id,CollectionJson_Id);
                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionJson_ID ON #CollectionLevel2XCollectionJson(CollectionJson_Id);


                    -- CollectionJson

                    SELECT CJ.ID,CJ.AppVersion
						INTO #CollectionJson 
						FROM CollectionJson CJ WITH (NOLOCK)
						INNER JOIN #CollectionLevel2XCollectionJson C2CJ WITH (NOLOCK)
							ON CJ.Id = C2CJ.CollectionJson_Id

                    CREATE INDEX IDX_CollectionJson_CollectionJson_ID ON #CollectionJson(ID);


                    -- Criação da Fato de Cabeçalhos
					SELECT
						CL2HF.Id
						,CL2HF.CollectionLevel2_Id
						,CL2HF.ParHeaderField_Id
						,CL2HF.ParFieldType_Id
						,CL2HF.Value INTO #CollectionLevel2XParHeaderField
					FROM CollectionLevel2XParHeaderField CL2HF (NOLOCK)
					INNER JOIN #Collectionlevel2 CL2 (NOLOCK) ON CL2.Id = CL2HF.CollectionLevel2_Id

					CREATE INDEX IDX_CollectionLevel2XParHeaderField_CollectionLevel_ID ON #CollectionLevel2XParHeaderField (CollectionLevel2_Id);

					-- Concatenação da Fato de Cabeçalhos

					SELECT
					CL2HF.CollectionLevel2_Id
					,STUFF((SELECT DISTINCT
					', ' + CONCAT(HF.Name, ': ', CASE
							WHEN CL2HF2.ParFieldType_Id = 1 OR	CL2HF2.ParFieldType_Id = 3 THEN PMV.Name
							WHEN CL2HF2.ParFieldType_Id = 2 THEN CASE
							WHEN HF.Description = 'Produto' THEN CAST(PRD.nCdProduto AS VARCHAR(500)) + ' - ' + PRD.cNmProduto
						ELSE EQP.Nome
						END
						WHEN CL2HF2.ParFieldType_Id = 6 THEN CONVERT(VARCHAR, CL2HF2.Value, 103)
						ELSE CL2HF2.Value
						END)
					FROM #CollectionLevel2XParHeaderField CL2HF2 (NOLOCK)
					LEFT JOIN #collectionlevel2 CL2 (NOLOCK) ON CL2.Id = CL2HF2.CollectionLevel2_Id
					LEFT JOIN ParHeaderField HF (NOLOCK) ON CL2HF2.ParHeaderField_Id = HF.Id
					LEFT JOIN ParLevel2 L2 (NOLOCK) ON L2.Id = CL2.ParLevel2_Id
					LEFT JOIN ParMultipleValues PMV (NOLOCK) ON CL2HF2.Value = CAST(PMV.Id AS VARCHAR(500)) AND CL2HF2.ParFieldType_Id <> 2
					LEFT JOIN Equipamentos EQP (NOLOCK) ON CAST(EQP.Id AS VARCHAR(500)) = CL2HF2.Value AND EQP.ParCompany_Id = CL2.UnitId AND CL2HF2.ParFieldType_Id = 2
					LEFT JOIN Produto PRD WITH (NOLOCK) ON CAST(PRD.nCdProduto AS VARCHAR(500)) = CL2HF2.Value AND CL2HF2.ParFieldType_Id = 2
					WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
					FOR XML PATH (''))
					, 1, 1, '') AS HeaderFieldList INTO #CollectionLevel2XParHeaderField2
					FROM #CollectionLevel2XParHeaderField CL2HF (NOLOCK)
					INNER JOIN #Collectionlevel2 CL2 (NOLOCK) ON CL2.Id = CL2HF.CollectionLevel2_Id
					LEFT JOIN ParHeaderField HF (NOLOCK) ON CL2HF.ParHeaderField_Id = HF.Id
					LEFT JOIN ParLevel2 L2 (NOLOCK) ON L2.Id = CL2.ParLevel2_Id
					GROUP BY CL2HF.CollectionLevel2_Id
					CREATE INDEX IDX_CollectionLevel2XParHeaderField_CollectionLevel2_ID ON #CollectionLevel2XParHeaderField2 (CollectionLevel2_Id);
					
					-- Criação da Fato de Coleta x Cluster

                    SELECT 
						C2XC.Id,
						C2XC.CollectionLevel2_Id,
						C2XC.ParCluster_Id
						INTO #CollectionLevel2XCluster
						FROM CollectionLevel2XCluster C2XC WITH (NOLOCK)
						INNER JOIN #CollectionLevel2 C2 WITH (NOLOCK)
							ON C2XC.CollectionLevel2_Id = C2.Id

                    CREATE INDEX IDX_CollectionLevel2XCluster_Cluster_ID ON #CollectionLevel2XCluster (CollectionLevel2_Id);


                 -- Cubo

                 SELECT                                
                  C2.CollectionDate AS Data            
                 ,L1.Name AS Indicador                 
                 ,L2.Name AS Monitoramento             
                 ,R3.ParLevel3_Name AS Tarefa           
                 ,R3.Weight AS Peso                    
                 ,case when R3.IntervalMin = '-9999999999999.9000000000' then '' else R3.IntervalMin end  AS 'IntervaloMinimo'  
                 ,case when R3.IntervalMax = '9999999999999.9000000000' then '' else R3.IntervalMax  end AS 'IntervaloMaximo'
                  
                 ,R3.Value AS 'Lancado'                
                 ,R3.IsConform AS 'Conforme'           
                 ,R3.IsNotEvaluate AS 'NA'             
                 ,R3.WeiEvaluation AS 'AV_Peso'        
                 ,R3.WeiDefects AS 'NC_Peso'              
                 ,case 
					when isnull(R3.ValueText, '') in ('undefined','null')
						THEN '' 
					ELSE isnull(R3.ValueText, '') END 
						AS ValueText 
                 ,C2.EvaluationNumber AS 'Avaliacao'    
                 ,C2.Sample AS 'Amostra'                
                 ,ISNULL(C2.Sequential,0) AS 'Sequencial'
                 ,ISNULL(C2.Side,0) as 'Banda'          
                 ,STR(C2.[Shift]) as 'Turno'            
                 ,STR(C2.Period) as 'Periodo'           
                 ,UN.Name AS 'Unidade'                  
                 ,R3.Id AS 'ResultLevel3Id'             
                 ,US.Name as 'Auditor'                  
                 ,ISNULL(L1.hashKey, '') as 'HashKey'                      
                 ,ISNULL(HF.HeaderFieldList, '') as 'HeaderFieldList' 
                 ,C2.AddDate as AddDate
                 ,CJ.AppVersion as Platform
				 , CASE 
					WHEN CJ.AppVersion = 'Excel'  THEN '4'
					WHEN C2.AlterDate IS NOT NULL THEN '1'
					WHEN CAST(C2.AddDate as date) > CAST(C2.CollectionDate as date) THEN '2'
                    WHEN CAST(C2.AddDate as date) < CAST(C2.CollectionDate as date) THEN '3'
				   ELSE '0'
				   END
				 as Type,
                 PC.Name as Processo,
                  (SELECT top 1 PL3V.ParLevel3InputType_Id 
						FROM parlevel3value PL3V 
						WHERE 1 = 1
						 AND (isnull(PL3V.parcompany_id,un.id) = un.id ) 
						 AND (isnull(PL3V.ParLevel1_id,l1.id) = l1.id ) 
						 AND (isnull(PL3V.ParLevel2_id,l2.id) = l2.id ) 
						 AND PL3V.ParLevel3_Id = L3.Id
						 AND PL3V.IsActive = 1
				 order by PL3V.id DESC,PL3V.parcompany_id DESC, PL3V.ParLevel2_Id DESC,PL3V.ParLevel1_Id DESC) as ParLevel3InputType_Id
	            ,CASE WHEN MA.Motivo IS NULL THEN 0 ELSE 1 END AS IsLate
	            ,CASE WHEN (SELECT TOP 1 Id FROM Result_Level3_Photos RL3P WHERE RL3P.Result_Level3_Id = R3.Id) IS NOT NULL THEN 1 ELSE 0 END AS HasPhoto
	            ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'Result_Level3' AND LT.Json_Id = R3.Id) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryResult_Level3
	            ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'CollectionLevel2XParHeaderField' AND LT.Json_Id IN (select CL2PHF_LT.ID from CollectionLevel2XParHeaderField CL2PHF_LT where CL2PHF_LT.collectionlevel2_ID = C2.ID)) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryHeaderField
                ,ma.Motivo as ParReason
	            ,PRT.Name as ParReasonType
                 FROM #CollectionLevel2 C2 (nolock)     
                 INNER JOIN ParCompany UN (nolock)     
                 ON UN.Id = c2.UnitId                  
                 INNER JOIN #Result_Level3 R3  (nolock) 
                 ON R3.CollectionLevel2_Id = C2.Id     
                 INNER JOIN ParLevel3 L3 (nolock)      
                 ON L3.Id = R3.ParLevel3_Id            
                 INNER JOIN ParLevel2 L2 (nolock)      
                 ON L2.Id = C2.ParLevel2_Id            
                 INNER JOIN ParLevel1 L1 (nolock)      
                 ON L1.Id = C2.ParLevel1_Id         
                 INNER JOIN UserSgq US (nolock)        
                 ON C2.AuditorId = US.Id            
                 LEFT JOIN                             
                 #CollectionLevel2XParHeaderField2 HF 
                 on c2.Id = HF.CollectionLevel2_Id
                 LEFT JOIN #CollectionLevel2XCollectionJson CLCJ
                 ON CLCJ.CollectionLevel2_Id = C2.Id
                 LEFT JOIN #CollectionJson CJ
                 ON CJ.Id = CLCJ.CollectionJson_Id
                 LEFT JOIN #CollectionLevel2XCluster C2XC
				 ON C2XC.CollectionLevel2_Id = C2.Id
				 LEFT JOIN ParCluster PC
				 ON PC.Id = C2XC.ParCluster_Id
                 LEFT JOIN CollectionLevel2XParReason CL2MA
                 ON CL2MA.CollectionLevel2_Id = C2.Id
                 LEFT JOIN ParReason MA
                 ON MA.Id = CL2MA.ParReason_Id
                 LEFT JOIN ParReasonType PRT
                 ON PRT.Id = MA.ParReasonType_Id
                 WHERE 1=1 
                  
                  { sqlLevel3 } 
                
                     DROP TABLE #CollectionLevel2 
                     DROP TABLE #CollectionJson
                     DROP TABLE #Result_Level3
					 DROP TABLE #CollectionLevel2XParHeaderField 
					 DROP TABLE #CollectionLevel2XParHeaderField2
					 DROP TABLE #CollectionLevel2XCluster
					 DROP TABLE #CollectionLevel2XCollectionJson

                ";

        return query;
    }

    public string SelectSeara(DataCarrierFormulario form)
    {
        var dtInit = form._dataInicio.ToString("yyyyMMdd");
        var dtF = form._dataFim.ToString("yyyyMMdd");

        var sqlTurno = "";
        var sqlUnidade = "";
        var sqlLevel1 = "";
        var sqlLevel2 = "";
        var sqlLevel3 = "";
        var formatDate = "";

        if (form.shift > 0)
        {
            sqlTurno = "\n AND [Shift] = " + form.shift;
        }

        if (form.unitId > 0)
        {
            sqlUnidade = "\n AND UnitId = " + form.unitId;
        }

        if (form.level1Id > 0)
        {
            sqlLevel1 = "\n AND ParLevel1_id = " + form.level1Id;
        }

        if (form.level2Id > 0)
        {
            sqlLevel2 = "\n AND ParLevel2_Id = " + form.level2Id;
        }

        if (form.level3Id > 0)
        {
            sqlLevel3 = "\n AND L3.Id = " + form.level3Id;
        }

        if (GlobalConfig.Eua)
        {
            formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 101)";
        }
        else
        {
            formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 103)";
        }

        var query = $@" 


                    -- DROP TABLE #CollectionLevel2

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
                    FROM Collectionlevel2 CL2 WITH (NOLOCK)
                        WHERE 1=1
                         AND NotEvaluatedIs <> 999
                         AND Duplicated <> 999
                         AND CL2.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF }  23:59:59'
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

                    -- Result Level 3 x Qualification
						SELECT
							r3.Id
						   ,STUFF((SELECT DISTINCT
									', ' + pq.Name
								FROM ResultLevel3XParQualification r3q
								INNER JOIN ParQualification pq
									ON pq.Id = r3q.Qualification_Value
								WHERE r3q.ResultLevel3_Id = r3.Id
								FOR XML PATH (''))
							, 1, 1, '') Qualification 
						INTO #Qualification
						FROM ResultLevel3XParQualification r3q with (nolock)
						LEFT JOIN Result_Level3 r3 with (nolock)
							ON r3.Id = r3q.ResultLevel3_Id
						INNER JOIN #CollectionLevel2 C with (nolock)
							ON R3.CollectionLevel2_Id = C.ID
						LEFT JOIN ParQualification pq with (nolock)
							ON pq.Id = r3q.Qualification_Value
						GROUP BY r3.Id

                    -- Result Level 3

					SELECT 
						R3.ID
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
						,R3.WeiDefects
						,QL.Qualification
						INTO #Result_Level3
						FROM Result_Level3 R3 WITH (NOLOCK)
						INNER JOIN #CollectionLevel2 C2
							ON R3.CollectionLevel2_Id = C2.Id	
						LEFT JOIN #Qualification QL 
						ON QL.ID = R3.ID

                    CREATE INDEX IDX_Result_Level3_CollectionLevel2_ID ON #Result_Level3(CollectionLevel2_Id);
                    CREATE INDEX IDX_Result_Level3_CollectionLevel2_Lvl3_ID ON #Result_Level3(CollectionLevel2_Id,Parlevel3_Id);


                    -- CollectionLevel2XCollectionJson

                    SELECT 
						CollectionLevel2_Id
						,CollectionJson_Id as CollectionJson_Id 
						,ROW_NUMBER() OVER (PARTITION BY CollectionLevel2_Id ORDER BY CollectionJson_Id DESC) AS [ROW]
                    INTO #CollectionLevel2XCollectionJson
					FROM CollectionLevel2XCollectionJson C2CJ WITH(NOLOCK)
                    INNER JOIN #CollectionLevel2 C2
						ON C2.Id = C2CJ.CollectionLevel2_Id



                    DELETE FROM #CollectionLevel2XCollectionJson WHERE [ROW] > 1

                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionLevel2_ID ON #CollectionLevel2XCollectionJson(CollectionLevel2_Id);
                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionLevel2CollectionJson_ID ON #CollectionLevel2XCollectionJson(CollectionLevel2_Id,CollectionJson_Id);
                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionJson_ID ON #CollectionLevel2XCollectionJson(CollectionJson_Id);


                    -- CollectionJson

                    SELECT CJ.ID,CJ.AppVersion
						INTO #CollectionJson 
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
					   ,CL2HF.Value 
                    INTO #CollectionLevel2XParHeaderFieldGeral
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
							and hf.name like '%turno%'
							FOR XML PATH (''))
						, 1, 1, '') AS HeaderFieldList
						INTO #CollectionLevel2XParHeaderFieldGeralTURNO
					FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (NOLOCK)
					INNER JOIN #Collectionlevel2 CL2 (NOLOCK)
						ON CL2.Id = CL2HF.CollectionLevel2_Id
					LEFT JOIN ParHeaderFieldGeral HF (NOLOCK)
						ON CL2HF.ParHeaderFieldGeral_Id = HF.Id
					LEFT JOIN ParLevel2 L2 (NOLOCK)
						ON L2.Id = CL2.ParLevel2_Id
						WHERE HF.Name LIKE '%turno%'
					GROUP BY CL2HF.CollectionLevel2_Id

                    CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeralTURNO (CollectionLevel2_Id);


					-- Criação da Fato de Coleta x Cluster

                    SELECT 
						C2XC.Id,
						C2XC.CollectionLevel2_Id,
						C2XC.ParCluster_Id
						INTO #CollectionLevel2XCluster
						FROM CollectionLevel2XCluster C2XC WITH (NOLOCK)
						INNER JOIN #CollectionLevel2 C2 WITH (NOLOCK)
							ON C2XC.CollectionLevel2_Id = C2.Id

                    CREATE INDEX IDX_CollectionLevel2XCluster_Cluster_ID ON #CollectionLevel2XCluster (CollectionLevel2_Id);


                 -- Cubo

                 SELECT                                
                  C2.CollectionDate AS Data            
                 ,L1.Name AS Indicador                 
                 ,L2.Name AS Monitoramento             
                 ,R3.ParLevel3_Name AS Tarefa           
                 ,R3.Weight AS Peso                    
                 ,case when R3.IntervalMin = '-9999999999999.9000000000' then '' else R3.IntervalMin end  AS 'IntervaloMinimo'  
                 ,case when R3.IntervalMax = '9999999999999.9000000000' then '' else R3.IntervalMax  end AS 'IntervaloMaximo'
                  
                 ,R3.Value AS 'Lancado'                
                 ,R3.IsConform AS 'Conforme'           
                 ,R3.IsNotEvaluate AS 'NA'             
                 ,R3.WeiEvaluation AS 'AV_Peso'        
                 ,R3.WeiDefects AS 'NC_Peso'              
                 ,case 
					when isnull(R3.ValueText, '') in ('undefined','null')
						THEN '' 
					ELSE isnull(R3.ValueText, '') END 
						AS ValueText 
                 ,C2.EvaluationNumber AS 'Avaliacao'    
                 ,C2.Sample AS 'Amostra'                
                 ,ISNULL(C2.Sequential,0) AS 'Sequencial'
                 ,ISNULL(C2.Side,0) as 'Banda'          
                 ,ISNULL(HFTURNO.HeaderFieldList, '') as 'Turno'             
                 ,STR(C2.Period) as 'Periodo'           
                 ,UN.Name AS 'Unidade'                  
                 ,R3.Id AS 'ResultLevel3Id'             
                 ,US.Name as 'Auditor'                  
                 ,ISNULL(L1.hashKey, '') as 'HashKey'                      
                 ,ISNULL(HF.HeaderFieldList, '') as 'HeaderFieldList' 
                 ,C2.AddDate as AddDate
                 ,CJ.AppVersion as Platform
				 , CASE 
					WHEN CJ.AppVersion = 'Excel'  THEN '4'
					WHEN C2.AlterDate IS NOT NULL THEN '1'
					WHEN CAST(C2.AddDate as date) > CAST(C2.CollectionDate as date) THEN '2'
                    WHEN CAST(C2.AddDate as date) < CAST(C2.CollectionDate as date) THEN '3'
				   ELSE '0'
				   END
				 as Type,
                 PC.Name as Processo,
                  (SELECT top 1 PL3V.ParLevel3InputType_Id 
						FROM parlevel3value PL3V 
						WHERE 1 = 1
						 AND (isnull(PL3V.parcompany_id,un.id) = un.id ) 
						 AND (isnull(PL3V.ParLevel1_id,l1.id) = l1.id ) 
						 AND (isnull(PL3V.ParLevel2_id,l2.id) = l2.id ) 
						 AND PL3V.ParLevel3_Id = L3.Id
						 AND PL3V.IsActive = 1
				 order by PL3V.id DESC,PL3V.parcompany_id DESC, PL3V.ParLevel2_Id DESC,PL3V.ParLevel1_Id DESC) as ParLevel3InputType_Id
	            ,CASE WHEN MA.Motivo IS NULL THEN 0 ELSE 1 END AS IsLate
	            ,CASE WHEN (SELECT TOP 1 Id FROM Result_Level3_Photos RL3P WHERE RL3P.Result_Level3_Id = R3.Id) IS NOT NULL THEN 1 ELSE 0 END AS HasPhoto
	            ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'Result_Level3' AND LT.Json_Id = R3.Id) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryResult_Level3
	            ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'CollectionLevel2XParHeaderFieldGeral' AND LT.Json_Id IN (select CL2PHF_LT.ID from CollectionLevel2XParHeaderFieldGeral CL2PHF_LT where CL2PHF_LT.collectionlevel2_ID = C2.ID)) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryHeaderField
                ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'CollectionLevel2XParFamiliaProdutoXParProduto' AND LT.Json_Id IN (select CL2_PFP_PP.CollectionLevel2_Id from CollectionLevel2XParFamiliaProdutoXParProduto CL2_PFP_PP where CL2_PFP_PP.collectionlevel2_Id = C2.ID)) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryParProduto
                ,ma.Motivo as ParReason
	            ,PRT.Name as ParReasonType
                ,CL2_PFP_PP.ParFamiliaProduto_Id
                ,PP.Id as ParProduto_Id
                ,PP.Name as ParProduto
                ,ISNULL( R3.qualification, 'Sem dados' )Qualification_Group
				,CL2_PFP_PP.CollectionLevel2_Id
                 FROM #CollectionLevel2 C2 (nolock)     
                 INNER JOIN ParCompany UN (nolock)     
                 ON UN.Id = c2.UnitId                  
                 INNER JOIN #Result_Level3 R3  (nolock) 
                 ON R3.CollectionLevel2_Id = C2.Id     
                 INNER JOIN ParLevel3 L3 (nolock)      
                 ON L3.Id = R3.ParLevel3_Id            
                 INNER JOIN ParLevel2 L2 (nolock)      
                 ON L2.Id = C2.ParLevel2_Id            
                 INNER JOIN ParLevel1 L1 (nolock)      
                 ON L1.Id = C2.ParLevel1_Id         
                 INNER JOIN UserSgq US (nolock)        
                 ON C2.AuditorId = US.Id            
                 LEFT JOIN                             
                 #CollectionLevel2XParHeaderFieldGeral2 HF 
                 on c2.Id = HF.CollectionLevel2_Id
                 LEFT JOIN                             
                 #CollectionLevel2XParHeaderFieldGeralTURNO HFTURNO 
                 on c2.Id = HFTURNO.CollectionLevel2_Id
                 LEFT JOIN #CollectionLevel2XCollectionJson CLCJ
                 ON CLCJ.CollectionLevel2_Id = C2.Id
                 LEFT JOIN #CollectionJson CJ
                 ON CJ.Id = CLCJ.CollectionJson_Id
                 LEFT JOIN #CollectionLevel2XCluster C2XC
				 ON C2XC.CollectionLevel2_Id = C2.Id
				 LEFT JOIN ParCluster PC
				 ON PC.Id = C2XC.ParCluster_Id
                 LEFT JOIN CollectionLevel2XParReason CL2MA
                 ON CL2MA.CollectionLevel2_Id = C2.Id
                 LEFT JOIN ParReason MA
                 ON MA.Id = CL2MA.ParReason_Id
                 LEFT JOIN ParReasonType PRT
                 ON PRT.Id = MA.ParReasonType_Id
                 LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CL2_PFP_PP
				 ON CL2_PFP_PP.CollectionLevel2_Id = C2.Id
				 LEFT JOIN ParProduto PP 
				 ON CL2_PFP_PP.parproduto_id = PP.Id
				 AND PP.IsActive = 1
                 WHERE 1=1 
                  
                  { sqlLevel3 } 
                
                     DROP TABLE #CollectionLevel2 
                     DROP TABLE #CollectionJson
                     DROP TABLE #Result_Level3
					 DROP TABLE #CollectionLevel2XParHeaderFieldGeral 
					 DROP TABLE #CollectionLevel2XParHeaderFieldGeral2
					 DROP TABLE #CollectionLevel2XCluster
					 DROP TABLE #CollectionLevel2XCollectionJson
                     Drop TABLE #Qualification

                ";

        return query;
    }

    public string SelectUSA(DataCarrierFormulario form)
    {
        var dtInit = form._dataInicio.ToString("yyyyMMdd");
        var dtF = form._dataFim.ToString("yyyyMMdd");

        var sqlTurno = "";
        var sqlUnidade = "";
        var sqlLevel1 = "";
        var sqlLevel2 = "";
        var sqlLevel3 = "";
        var formatDate = "";

        if (form.shift > 0)
        {
            sqlTurno = "\n AND [Shift] = " + form.shift;
        }

        if (form.unitId > 0)
        {
            sqlUnidade = "\n AND UnitId = " + form.unitId;
        }

        if (form.level1Id > 0)
        {
            sqlLevel1 = "\n AND ParLevel1_id = " + form.level1Id;
        }

        if (form.level2Id > 0)
        {
            sqlLevel2 = "\n AND ParLevel2_Id = " + form.level2Id;
        }

        if (form.level3Id > 0)
        {
            sqlLevel3 = "\n AND L3.Id = " + form.level3Id;
        }

        if (GlobalConfig.Eua)
        {
            formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 101)";
        }
        else
        {
            formatDate = "CONVERT(varchar, CONVERT(DATE, CL2HF2.value, 111), 103)";
        }

        var query = $@" 


                    -- DROP TABLE #CollectionLevel2

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
                    INTO #CollectionLevel2_
                    FROM Collectionlevel2 CL2 WITH (NOLOCK)
                        WHERE 1=1
                         AND NotEvaluatedIs <> 999
                         AND Duplicated <> 999
                         AND CL2.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF }  23:59:59'
                         { sqlTurno } 
                         { sqlUnidade } 
                         { sqlLevel1 } 
                         { sqlLevel2 }

                    select c2.*, c2Group.C2ID,  case when av_desvios.shift is null then 0 else 1 end desvio  
				        INTO #CollectionLevel2
				        from #CollectionLevel2_ c2
				        left join ( 
				         select 
				         min(id) as C2ID,
				         parlevel1_id,
				         parlevel2_id,
				         Unitid,
				         cast(collectiondate as date) data,
				         EvaluationNumber,
				         Shift,
				         Period
				         from #CollectionLevel2_ 
				 
				         group by
				 
				         parlevel1_id,
				         parlevel2_id,
				         Unitid,
				         cast(collectiondate as date),
				         EvaluationNumber,
				         Shift,
				         Period
				         ) as c2Group 
				         on  c2Group.parlevel1_id = c2.parlevel1_id
				         and c2Group.parlevel2_id = c2.parlevel2_id
				         and c2Group.Unitid = c2.Unitid
				         and c2Group.data = cast(c2.collectiondate as date)
				         and c2Group.EvaluationNumber = c2.EvaluationNumber
				         and c2Group.Shift = c2.Shift
				         and c2Group.Period = c2.Period

				         left join ( 
				         select 
				         parlevel1_id,
				         parlevel2_id,
				         Unitid,
				         cast(collectiondate as date) data,
				         EvaluationNumber,
				         Shift,
				         Period
				         from #CollectionLevel2_ c2_
				         inner join (Select Adddate as adddated, Evaluation, Sample as sampled, ParLevel1_Id as p1, ParLevel2_Id as p2, ParCompany_Id, period as periodo, shift as turno, collectiondate as data  from Deviation where DeviationMessage is not null) d 
				         on (concat(substring(d.data,5,4) ,substring(d.data,1,2),substring(d.data,3,2))) = cast(c2_.collectiondate as date)
				         and d.Evaluation = c2_.EvaluationNumber
				         and d.Sampled = c2_.sample
				         and d.p1 = c2_.ParLevel1_Id
				         and d.p2 = c2_.ParLevel2_Id
				         and d.ParCompany_Id = c2_.unitid
                         and d.periodo = c2_.Period
                         and d.turno = c2_.Shift
				         group by
				 
				         parlevel1_id,
				         parlevel2_id,
				         Unitid,
				         cast(collectiondate as date),
				         EvaluationNumber,
				         Shift,
				         Period
				         ) as av_desvios 
				         on av_desvios.parlevel1_id = c2.parlevel1_id 
				         and av_desvios.parlevel2_id = c2.parlevel2_id
				         and av_desvios.Unitid = c2.Unitid
				         and cast(av_desvios.data as date) = cast(c2.collectiondate as date)
				         and av_desvios.EvaluationNumber = c2.EvaluationNumber
				         and av_desvios.Shift = c2.Shift
				         and av_desvios.Period = c2.Period
 
 
                    CREATE INDEX IDX_CollectionLevel2_ID ON #CollectionLevel2(ID);
                    CREATE INDEX IDX_CollectionLevel2_UnitId ON #CollectionLevel2(UnitId);
                    CREATE INDEX IDX_CollectionLevel2_CollectionDate ON #CollectionLevel2(CollectionDate);
                    CREATE INDEX IDX_CollectionLevel2_ParLevel1_Id ON #CollectionLevel2(ParLevel1_Id);
                    CREATE INDEX IDX_CollectionLevel2_ParLevel2_Id ON #CollectionLevel2(ParLevel2_Id);
                    CREATE INDEX IDX_CollectionLevel2_12345 ON #CollectionLevel2(ID,UnitId,CollectionDate,ParLevel1_Id,ParLevel2_Id);     
                    -- Result Level 3

					SELECT 
						R3.ID
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
						,R3.WeiDefects
						INTO #Result_Level3
						FROM Result_Level3 R3 WITH (NOLOCK)
						INNER JOIN #CollectionLevel2 C2
							ON R3.CollectionLevel2_Id = C2.Id	

                    CREATE INDEX IDX_Result_Level3_CollectionLevel2_ID ON #Result_Level3(CollectionLevel2_Id);
                    CREATE INDEX IDX_Result_Level3_CollectionLevel2_Lvl3_ID ON #Result_Level3(CollectionLevel2_Id,Parlevel3_Id);


                    -- CollectionLevel2XCollectionJson

                    SELECT 
						CollectionLevel2_Id
						,CollectionJson_Id as CollectionJson_Id 
						,ROW_NUMBER() OVER (PARTITION BY CollectionLevel2_Id ORDER BY CollectionJson_Id DESC) AS [ROW]

                    INTO #CollectionLevel2XCollectionJson
					FROM CollectionLevel2XCollectionJson C2CJ WITH(NOLOCK)

                    INNER JOIN #CollectionLevel2 C2
						ON C2.Id = C2CJ.CollectionLevel2_Id



                    DELETE FROM #CollectionLevel2XCollectionJson WHERE [ROW] > 1

                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionLevel2_ID ON #CollectionLevel2XCollectionJson(CollectionLevel2_Id);
                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionLevel2CollectionJson_ID ON #CollectionLevel2XCollectionJson(CollectionLevel2_Id,CollectionJson_Id);
                    CREATE INDEX IDX_CollectionLevel2XCollectionJson_CollectionJson_ID ON #CollectionLevel2XCollectionJson(CollectionJson_Id);


                    -- CollectionJson

                    SELECT CJ.ID,CJ.AppVersion
						INTO #CollectionJson 
						FROM CollectionJson CJ WITH (NOLOCK)
						INNER JOIN #CollectionLevel2XCollectionJson C2CJ WITH (NOLOCK)
							ON CJ.Id = C2CJ.CollectionJson_Id

                    CREATE INDEX IDX_CollectionJson_CollectionJson_ID ON #CollectionJson(ID);


                    -- Criação da Fato de Cabeçalhos
						
						SELECT 
							CL2HF.Id
							,CL2HF.CollectionLevel2_Id
							,CL2HF.ParHeaderField_Id
							,CL2HF.ParFieldType_Id
							,CL2HF.Value
						INTO #CollectionLevel2XParHeaderField
						FROM CollectionLevel2XParHeaderField CL2HF (nolock) 
						INNER JOIN #Collectionlevel2 CL2 (nolock) on CL2.id = CL2HF.CollectionLevel2_Id 

                    CREATE INDEX IDX_CollectionLevel2XParHeaderField_CollectionLevel_ID ON #CollectionLevel2XParHeaderField (CollectionLevel2_Id);

                    -- Concatenação da Fato de Cabeçalhos

					SELECT                               
						 CL2HF.CollectionLevel2_Id,        
						 STUFF(   
							(SELECT DISTINCT ', ' + CONCAT(HF.name, ': ', case 
							when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
							when CL2HF2.ParFieldType_Id = 2 then case when HF.Description = 'Produto' then cast(PRD.nCdProduto as varchar(500)) + ' - ' + PRD.cNmProduto else EQP.Nome end 
							when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar,  CL2HF2.value, 103)
							else CL2HF2.Value end)
							FROM #CollectionLevel2XParHeaderField CL2HF2 (nolock) 
							left join #collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id
							left join ParHeaderField HF (nolock)on CL2HF2.ParHeaderField_Id = HF.Id
							left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id
							left join ParMultipleValues PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
							left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2
							left join Produto PRD with(nolock) on cast(PRD.nCdProduto as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 2
							WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
							FOR XML PATH('')
							), 1, 1, '')  AS HeaderFieldList
						INTO #CollectionLevel2XParHeaderField2
						FROM #CollectionLevel2XParHeaderField CL2HF (nolock) 
						INNER join #Collectionlevel2 CL2 (nolock) on CL2.id = CL2HF.CollectionLevel2_Id 
						LEFT JOIN ParHeaderField HF (nolock) on CL2HF.ParHeaderField_Id = HF.Id 
						LEFT JOIN ParLevel2 L2 (nolock) on L2.Id = CL2.Parlevel2_id
                    GROUP BY CL2HF.CollectionLevel2_Id

                    CREATE INDEX IDX_CollectionLevel2XParHeaderField_CollectionLevel2_ID ON #CollectionLevel2XParHeaderField2 (CollectionLevel2_Id);

					-- Criação da Fato de Coleta x Cluster

                    SELECT 
						C2XC.Id,
						C2XC.CollectionLevel2_Id,
						C2XC.ParCluster_Id
						INTO #CollectionLevel2XCluster
						FROM CollectionLevel2XCluster C2XC WITH (NOLOCK)
						INNER JOIN #CollectionLevel2 C2 WITH (NOLOCK)
							ON C2XC.CollectionLevel2_Id = C2.Id

                    CREATE INDEX IDX_CollectionLevel2XCluster_Cluster_ID ON #CollectionLevel2XCluster (CollectionLevel2_Id);


                 -- Cubo

                 SELECT                                
                  C2.CollectionDate AS Data            
                 ,L1.Name AS Indicador                 
                 ,L2.Name AS Monitoramento             
                 ,R3.ParLevel3_Name AS Tarefa           
                 ,R3.Weight AS Peso                    
                 ,case when R3.IntervalMin = '-9999999999999.9000000000' then '' else R3.IntervalMin end  AS 'IntervaloMinimo'  
                 ,case when R3.IntervalMax = '9999999999999.9000000000' then '' else R3.IntervalMax  end AS 'IntervaloMaximo'
                  
                 ,R3.Value AS 'Lancado'                
                 ,R3.IsConform AS 'Conforme'           
                 ,R3.IsNotEvaluate AS 'NA'             
                 ,R3.WeiEvaluation AS 'AV_Peso'        
                 ,R3.WeiDefects AS 'NC_Peso'              
                 ,case 
					when isnull(R3.ValueText, '') in ('undefined','null')
						THEN '' 
					ELSE isnull(R3.ValueText, '') END 
						AS ValueText 
                 ,C2.EvaluationNumber AS 'Avaliacao'    
                 ,C2.Sample AS 'Amostra'                
                 ,ISNULL(C2.Sequential,0) AS 'Sequencial'
                 ,ISNULL(C2.Side,0) as 'Banda'          
                 ,STR(C2.[Shift]) as 'Turno'            
                 ,STR(C2.Period) as 'Periodo'           
                 ,UN.Name AS 'Unidade'                  
                 ,R3.Id AS 'ResultLevel3Id'             
                 ,US.Name as 'Auditor'                  
                 ,ISNULL(L1.hashKey, '') as 'HashKey'                      
                 ,ISNULL(HF.HeaderFieldList, '') as 'HeaderFieldList' 
                 ,C2.AddDate as AddDate
                 ,CJ.AppVersion as Platform
				 , CASE 
					WHEN CJ.AppVersion = 'Excel'  THEN '4'
					WHEN C2.AlterDate IS NOT NULL THEN '1'
					WHEN CAST(C2.AddDate as date) > CAST(C2.CollectionDate as date) THEN '2'
                    WHEN CAST(C2.AddDate as date) < CAST(C2.CollectionDate as date) THEN '3'
				   ELSE '0'
				   END
				 as Type,
                 PC.Name as Processo,
                  (SELECT top 1 PL3V.ParLevel3InputType_Id 
						FROM parlevel3value PL3V 
						WHERE 1 = 1
						 AND (isnull(PL3V.parcompany_id,un.id) = un.id ) 
						 AND (isnull(PL3V.ParLevel1_id,l1.id) = l1.id ) 
						 AND (isnull(PL3V.ParLevel2_id,l2.id) = l2.id ) 
						 AND PL3V.ParLevel3_Id = L3.Id
						 AND PL3V.IsActive = 1
				 order by PL3V.id DESC,PL3V.parcompany_id DESC, PL3V.ParLevel2_Id DESC,PL3V.ParLevel1_Id DESC) as ParLevel3InputType_Id
	            ,CASE WHEN MA.Motivo IS NULL THEN 0 ELSE 1 END AS IsLate
	            ,CASE WHEN (SELECT TOP 1 Id FROM Result_Level3_Photos RL3P WHERE RL3P.Result_Level3_Id = R3.Id) IS NOT NULL THEN 1 ELSE 0 END AS HasPhoto
	            ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'Result_Level3' AND LT.Json_Id = R3.Id) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryResult_Level3
	            ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'CollectionLevel2XParHeaderField' AND LT.Json_Id IN (select CL2PHF_LT.ID from CollectionLevel2XParHeaderField CL2PHF_LT where CL2PHF_LT.collectionlevel2_ID = C2.ID)) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryHeaderField
                ,ma.Motivo as ParReason
	            ,PRT.Name as ParReasonType
                
                ,C2.C2ID AS C2ID
                ,c2.desvio as DesvioAv
                ,
				(
					select TOP 10 PCL.NAME AS CRITICALLEVEL from ParLevel1XCluster P1XC
					 left join ParCriticalLevel PCL
					 ON PCL.ID = P1XC.ParCriticalLevel_Id
					 WHERE P1XC.Parlevel1_id = l1.id
					 and P1XC.ParCluster_Id = pc.id
					 and P1XC.isActive = 1
					 and P1XC.EffectiveDate = 
					 (
						select MAX(P1XC.EffectiveDate) AS CRITICALLEVEL from ParLevel1XCluster P1XC
						 left join ParCriticalLevel PCL
						 ON PCL.ID = P1XC.ParCriticalLevel_Id
						 WHERE P1XC.Parlevel1_id = l1.id
						 and P1XC.ParCluster_Id = pc.id
						 and P1XC.isActive = 1
						 and P1XC.EffectiveDate <= c2.collectiondate
					 )
					 group by P1XC.EffectiveDate, PCL.NAME
				) CriticalLevel

                 FROM #CollectionLevel2 C2 (nolock)     
                 INNER JOIN ParCompany UN (nolock)     
                 ON UN.Id = c2.UnitId                  
                 INNER JOIN #Result_Level3 R3  (nolock) 
                 ON R3.CollectionLevel2_Id = C2.Id     
                 INNER JOIN ParLevel3 L3 (nolock)      
                 ON L3.Id = R3.ParLevel3_Id            
                 INNER JOIN ParLevel2 L2 (nolock)      
                 ON L2.Id = C2.ParLevel2_Id            
                 INNER JOIN ParLevel1 L1 (nolock)      
                 ON L1.Id = C2.ParLevel1_Id         
                 INNER JOIN UserSgq US (nolock)        
                 ON C2.AuditorId = US.Id            
                 LEFT JOIN                             
                 #CollectionLevel2XParHeaderField2 HF 
                 on c2.Id = HF.CollectionLevel2_Id
                 LEFT JOIN #CollectionLevel2XCollectionJson CLCJ
                 ON CLCJ.CollectionLevel2_Id = C2.Id
                 LEFT JOIN #CollectionJson CJ
                 ON CJ.Id = CLCJ.CollectionJson_Id
                 LEFT JOIN #CollectionLevel2XCluster C2XC
				 ON C2XC.CollectionLevel2_Id = C2.Id
				 LEFT JOIN ParCluster PC
				 ON PC.Id = C2XC.ParCluster_Id
                 LEFT JOIN CollectionLevel2XParReason CL2MA
                 ON CL2MA.CollectionLevel2_Id = C2.Id
                 LEFT JOIN ParReason MA
                 ON MA.Id = CL2MA.ParReason_Id
                 LEFT JOIN ParReasonType PRT
                 ON PRT.Id = MA.ParReasonType_Id
                 WHERE 1=1 
                  
                  { sqlLevel3 } 
                    
                     DROP TABLE #CollectionLevel2_ 
                     DROP TABLE #CollectionLevel2 
                     DROP TABLE #CollectionJson
                     DROP TABLE #Result_Level3
					 DROP TABLE #CollectionLevel2XParHeaderField 
					 DROP TABLE #CollectionLevel2XParHeaderField2
					 DROP TABLE #CollectionLevel2XCluster
					 DROP TABLE #CollectionLevel2XCollectionJson

                ";

        return query;
    }

    public string SelectRH(DataCarrierFormularioNew form, string userUnits)
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
		var sqlCluster = "";


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
                sqlDepartamentoPelaHash += $@"OR Secao.Hash like '{item}|%'
                            OR Secao.Hash like '%|{item}|%'
                            OR Secao.Hash = '{item}'";
            }
            sqlDepartment = $@" AND (Secao.Id in ({string.Join(",", form.ParSecao_Ids)}) 
                             {sqlDepartamentoPelaHash})";
        }
        else if (form.ParDepartment_Ids.Length > 0)
        {
            var sqlDepartamentoPelaHash = "";
            foreach (var item in form.ParDepartment_Ids)
            {
                sqlDepartamentoPelaHash += $@"OR Centro.Hash like '{item}|%'
                            OR Centro.Hash like '%|{item}|%'
                            OR Centro.Hash = '{item}'";
            }
            sqlDepartment = $@" AND (Centro.Id in ({string.Join(",", form.ParDepartment_Ids)}) 
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

        if (form.ParCluster_Ids.Length > 0)
            sqlCluster = $" AND PC.Id IN({string.Join(",", form.ParCluster_Ids)}) --Cluster";

        if (form.ParClusterGroup_Ids.Length > 0)
			sqlClusterGroup = $" AND PC.ParClusterGroup_Id IN({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Cluster";

		if (form.ParStructure2_Ids.Length > 0)
			sqlParStructure2 = $" AND pg.ParStructureParent_Id IN({string.Join(",", form.ParStructure2_Ids)}) --Grupo de empresa";

		if (form.ParStructure3_Ids.Length > 0)
			sqlParStructure3 = $" AND pg.Id IN({string.Join(",", form.ParStructure3_Ids)}) --Regional";

		if(form.userSgqAuditor_Ids.Length > 0)
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


					-- Result Level 3 x Qualification
						SELECT
							r3.Id
						   ,STUFF((SELECT DISTINCT
									', ' + pq.Name
								FROM ResultLevel3XParQualification r3q
								INNER JOIN ParQualification pq
									ON pq.Id = r3q.Qualification_Value
								WHERE r3q.ResultLevel3_Id = r3.Id
								FOR XML PATH (''))
							, 1, 1, '') Qualification INTO #Qualification
						FROM ResultLevel3XParQualification r3q
						LEFT JOIN Result_Level3 r3
							ON r3.Id = r3q.ResultLevel3_Id
						LEFT JOIN ParQualification pq
							ON pq.Id = r3q.Qualification_Value
						GROUP BY r3.Id

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
					   ,R3.WeiDefects
                       ,QL.Qualification INTO #Result_Level3
					FROM Result_Level3 R3 WITH (NOLOCK)
					INNER JOIN #CollectionLevel2 C2
						ON R3.CollectionLevel2_Id = C2.Id
                    LEFT JOIN #Qualification QL 
						ON QL.ID = R3.ID
	

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
						 CL2HF.CollectionLevel2_Id,        
						 STUFF(   
							(SELECT DISTINCT ', ' + CONCAT(HF.name, ': ', case 
							when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
							when CL2HF2.ParFieldType_Id = 2 then case when HF.Description = 'Produto' then cast(PRD.nCdProduto as varchar(500)) + ' - ' + PRD.cNmProduto else EQP.Nome end 
							when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar,  CL2HF2.value, 103)
							else CL2HF2.Value end)
							FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (nolock) 
							left join #collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id
							left join ParHeaderFieldGeral HF (nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id
							inner JOIN ParLevelHeaderField PLHF (NOLOCK) on PLHF.Id = HF.ParLevelHeaderField_Id
							inner join ParLevelDefiniton PLD (NOLOCK) on pld.Id = PLHF.Id 
							left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id
							left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
							left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2
							left join Produto PRD with(nolock) on cast(PRD.nCdProduto as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 2
							WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
							and pld.Id = 1
							FOR XML PATH('')
							), 1, 1, '')  AS HeaderFieldList1,
							STUFF(   
							(SELECT DISTINCT ', ' + CONCAT(HF.name, ': ', case 
							when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
							when CL2HF2.ParFieldType_Id = 2 then case when HF.Description = 'Produto' then cast(PRD.nCdProduto as varchar(500)) + ' - ' + PRD.cNmProduto else EQP.Nome end 
							when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar,  CL2HF2.value, 103)
							else CL2HF2.Value end)
							FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (nolock) 
							left join #collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id
							left join ParHeaderFieldGeral HF (nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id
							inner JOIN ParLevelHeaderField PLHF (NOLOCK) on PLHF.Id = HF.ParLevelHeaderField_Id
							inner join ParLevelDefiniton PLD (NOLOCK) on pld.Id = PLHF.Id 
							left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id
							left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
							left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2
							left join Produto PRD with(nolock) on cast(PRD.nCdProduto as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 2
							WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
							and pld.Id = 2
							FOR XML PATH('')
							), 1, 1, '')  AS HeaderFieldList2
						INTO #CollectionLevel2XParHeaderFieldGeral2
						FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (nolock) 
						INNER join #Collectionlevel2 CL2 (nolock) on CL2.id = CL2HF.CollectionLevel2_Id 
						LEFT JOIN ParHeaderFieldGeral HF (nolock) on CL2HF.ParHeaderFieldGeral_Id = HF.Id 
						LEFT JOIN ParLevel2 L2 (nolock) on L2.Id = CL2.Parlevel2_id
                    GROUP BY CL2HF.CollectionLevel2_Id

                    CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeral2 (CollectionLevel2_Id);

					 -- Criação da Fato de Cabeçalhos Tarefa
						
						SELECT 
							CL2HF.Id
							,CL2HF.ResultLevel3_Id 
							,CL2HF.ParHeaderFieldGeral_Id
							,CL2HF.ParFieldType_Id
							,CL2HF.Value
						INTO #Result_Level3XParHeaderFieldGeral
						FROM #Result_Level3 R3 (nolock)
						INNER JOIN Result_Level3XParHeaderFieldGeral CL2HF (nolock) 
							ON R3.id = CL2HF.ResultLevel3_Id 

                    CREATE INDEX IDX_Result_Level3XParHeaderFieldGeral_Result_Level3_Id ON #Result_Level3XParHeaderFieldGeral (ResultLevel3_Id);


                   -- Concatenação da Fato de Cabeçalhos

					SELECT                               
						 CL2HF.ResultLevel3_Id,        
						 STUFF(   
							(SELECT DISTINCT ', ' + CONCAT(HF.name, ': ', case 
							when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
							when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar,  CL2HF2.value, 103)
							else CL2HF2.Value end)
							FROM #Result_Level3XParHeaderFieldGeral CL2HF2 (nolock) 
							left join #Result_Level3 R3(nolock) on R3.id = CL2HF2.ResultLevel3_Id
							left join ParHeaderFieldGeral HF (nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id
							LEFT JOIN ParLevel3 L3 (nolock) on L3.Id = R3.Parlevel3_id
							left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
							WHERE CL2HF2.ResultLevel3_Id = CL2HF.ResultLevel3_Id
							FOR XML PATH('')
							), 1, 1, '')  AS HeaderFieldList
						INTO #Result_Level3XParHeaderFieldGeral2
						FROM #Result_Level3XParHeaderFieldGeral CL2HF (nolock) 
						INNER join #Result_Level3 R3 (nolock) on R3.id = CL2HF.ResultLevel3_Id 
						LEFT JOIN ParHeaderFieldGeral HF (nolock) on CL2HF.ParHeaderFieldGeral_Id = HF.Id 
						LEFT JOIN ParLevel3 L3 (nolock) on L3.Id = R3.Parlevel3_id
                    GROUP BY CL2HF.ResultLevel3_Id

                    CREATE INDEX IDX_Result_Level3XParHeaderFieldGeral_Result_Level3_ID ON #Result_Level3XParHeaderFieldGeral2 (ResultLevel3_Id);
					

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
					   ,US.FullName AS 'Auditor'
					   ,ISNULL(L1.HashKey, '') AS 'HashKey'
					   ,ISNULL(HF.HeaderFieldList1, '') AS 'HeaderFieldListL1'
					   ,ISNULL(HF.HeaderFieldList2, '') AS 'HeaderFieldListL2'
					   ,ISNULL(HF3.HeaderFieldList, '') AS 'HeaderFieldListL3'
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
                        ,(SELECT TOP 1
								PL3V.IsRequired
							FROM ParLevel3Value PL3V
							WHERE 1 = 1
							AND (ISNULL(PL3V.ParCompany_Id, UN.Id) = UN.Id)
							AND (ISNULL(PL3V.ParLevel1_Id, L1.Id) = L1.Id)
							AND (ISNULL(PL3V.ParLevel2_Id, L2.Id) = L2.Id)
							AND PL3V.ParLevel3_Id = L3.Id
							AND PL3V.IsActive = 1
							ORDER BY PL3V.Id DESC, PL3V.ParCompany_Id DESC, PL3V.ParLevel2_Id DESC, PL3V.ParLevel1_Id DESC)
						AS IsRequired
                        ,(SELECT TOP 1
								PL3V.IsNCTextRequired
							FROM ParLevel3Value PL3V
							WHERE 1 = 1
							AND (ISNULL(PL3V.ParCompany_Id, UN.Id) = UN.Id)
							AND (ISNULL(PL3V.ParLevel1_Id, L1.Id) = L1.Id)
							AND (ISNULL(PL3V.ParLevel2_Id, L2.Id) = L2.Id)
							AND PL3V.ParLevel3_Id = L3.Id
							AND PL3V.IsActive = 1
							ORDER BY PL3V.Id DESC, PL3V.ParCompany_Id DESC, PL3V.ParLevel2_Id DESC, PL3V.ParLevel1_Id DESC)
						AS IsNCTextRequired
						,(SELECT TOP 1
								PL3V.IsAtiveNA
							FROM ParLevel3Value PL3V
							WHERE 1 = 1
							AND (ISNULL(PL3V.ParCompany_Id, UN.Id) = UN.Id)
							AND (ISNULL(PL3V.ParLevel1_Id, L1.Id) = L1.Id)
							AND (ISNULL(PL3V.ParLevel2_Id, L2.Id) = L2.Id)
							AND PL3V.ParLevel3_Id = L3.Id
							AND PL3V.IsActive = 1
							ORDER BY PL3V.Id DESC, PL3V.ParCompany_Id DESC, PL3V.ParLevel2_Id DESC, PL3V.ParLevel1_Id DESC)
						AS IsAtiveNA
                        ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'Result_Level3' AND LT.Json_Id = R3.Id) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryResult_Level3
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
                       ,ISNULL( R3.qualification, 'Sem dados' )Qualification_Group
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
                    LEFT JOIN  #Result_Level3XParHeaderFieldGeral2 HF3
						ON HF3.ResultLevel3_Id = R3.ID	
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
					INNER JOIN ParDepartment Secao with (NOLOCK)
						ON Secao.Id = CL2PD.ParDepartment_Id
					INNER JOIN ParDepartment Centro with (NOLOCK)
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
					{ sqlCluster}    
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
					Drop TABLE #Qualification
                    DROP TABLE #Result_Level3XParHeaderFieldGeral2					
					DROP TABLE #Result_Level3XParHeaderFieldGeral

                ";

        return query;
    }



}
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

    public System.DateTime AddDate { get; set; }
    public string _AddDate { get { return AddDate.ToShortDateString(); /*+ " " + Data.ToShortTimeString();*/ } }
    public string Platform { get; set; }
    public string Type { get; set; }
    public string Processo { get; set; }

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
							,CL2HF.ParHeaderFieldGeral_Id
							,CL2HF.ParFieldType_Id
							,CL2HF.Value
						INTO #CollectionLevel2XParHeaderFieldGeral
						FROM CollectionLevel2XParHeaderFieldGeral CL2HF (nolock) 
						INNER JOIN #Collectionlevel2 CL2 (nolock) on CL2.id = CL2HF.CollectionLevel2_Id 

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
							left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id
							left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
							left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2
							left join Produto PRD with(nolock) on cast(PRD.nCdProduto as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 2
							WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
							FOR XML PATH('')
							), 1, 1, '')  AS HeaderFieldList
						INTO #CollectionLevel2XParHeaderFieldGeral2
						FROM #CollectionLevel2XParHeaderFieldGeral CL2HF (nolock) 
						INNER join #Collectionlevel2 CL2 (nolock) on CL2.id = CL2HF.CollectionLevel2_Id 
						LEFT JOIN ParHeaderFieldGeral HF (nolock) on CL2HF.ParHeaderFieldGeral_Id = HF.Id 
						LEFT JOIN ParLevel2 L2 (nolock) on L2.Id = CL2.Parlevel2_id
                    GROUP BY CL2HF.CollectionLevel2_Id

                    CREATE INDEX IDX_CollectionLevel2XParHeaderFieldGeral_CollectionLevel2_ID ON #CollectionLevel2XParHeaderFieldGeral2 (CollectionLevel2_Id);

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
                 ,PCluster.Name as ClusterName   
                 ,R3.ParLevel3_Name AS Tarefa  
				 ,PF.Name AS Frequencia             
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
	            ,ma.Motivo as ParReason
	            ,PRT.Name as ParReasonType
				,PCargo.Name as Cargo
				,PD.Name as Departamento
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

                 LEFT JOIN CollectionLevel2XParDepartment CL2PD
                 ON CL2PD.CollectionLevel2_Id = C2.Id
                 LEFT JOIN ParDepartment PD
                 ON PD.Id = CL2PD.ParDepartment_Id
                 LEFT JOIN CollectionLevel2XParCargo CL2PC
                 ON CL2PC.CollectionLevel2_Id = C2.Id
                 LEFT JOIN ParCargo PCargo
                 ON PCargo.Id = CL2PC.ParCargo_Id

                 LEFT JOIN ParFrequency PF (nolock)        
                 ON C2.ParFrequency_Id = PF.Id    

                LEFT JOIN ParCluster PCluster
				ON C2XC.ParCluster_Id = PCluster.Id

                 WHERE 1=1 
                  { sqlDepartment }
                  { sqlCargo }
                  { sqlLevel3 } 
                
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

}
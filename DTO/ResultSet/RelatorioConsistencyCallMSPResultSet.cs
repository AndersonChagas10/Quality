using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ResultSet
{
    public class RelatorioConsistencyCallMSPResultSet
    {
        public string Product { get; set; }
        public string Flavor { get; set; }
        public string Raw_side { get; set; }
        public decimal? Batch1 { get; set; }
        public decimal? Batch2 { get; set; }
        public decimal? Tumbler_rpm { get; set; }
        public decimal? PorcWater1 { get; set; }
        public decimal? PorcWater2 { get; set; }
        public decimal? Meat_age_target { get; set; }
        public decimal? Meat_age_avg { get; set; }
        public decimal? Meat_age_min_max1 { get; set; }
        public decimal? Meat_age_min_max2 { get; set; }
        public decimal? Flats { get; set; }
        public decimal? Insides { get; set; }
        public decimal? Eyes { get; set; }
        public decimal? Tumbler_batch_size { get; set; }
        public string Product_appearance { get; set; }
        public string Seasoning_distribuition { get; set; }
        public decimal? Meat_temperature_max1 { get; set; }
        public decimal? Meat_temperature_max2 { get; set; }
        public decimal? Meat_temperature_min1 { get; set; }
        public decimal? Meat_temperature_min2 { get; set; }
        public decimal? Meat_temperature_actual1 { get; set; }
        public decimal? Meat_temperature_actual2 { get; set; }

        public decimal? Thickness_avg_max_CDCM { get; set; }
        public decimal? Thickness_avg_min_CDCM { get; set; }
        public decimal? Thickness_avg1_CDCM { get; set; }
        public decimal? Thickness_avg2_CDCM { get; set; }
        public decimal? Thickness_sample_size1_CDCM { get; set; }
        public decimal? Thickness_sample_size2_CDCM { get; set; }
        public decimal? Out_spec_target_CDCM { get; set; }
        public decimal? Porc_out_spec1_CDCM { get; set; }
        public decimal? Porc_out_spec2_CDCM { get; set; }
        public decimal? Porc_LSL1_CDCM { get; set; }
        public decimal? Porc_LSL2_CDCM { get; set; }
        public decimal? Porc_USL1_CDCM { get; set; }
        public decimal? Porc_USL2_CDCM { get; set; }

        public decimal? Thickness_avg_max_BL { get; set; }
        public decimal? Thickness_avg_min_BL { get; set; }
        public decimal? Thickness_avg1_BL { get; set; }
        public decimal? Thickness_avg2_BL { get; set; }
        public decimal? Thickness_sample_size1_BL { get; set; }
        public decimal? Thickness_sample_size2_BL { get; set; }
        public decimal? Out_spec_target_BL { get; set; }
        public decimal? Porc_out_spec1_BL { get; set; }
        public decimal? Porc_out_spec2_BL { get; set; }
        public decimal? Porc_LSL1_BL { get; set; }
        public decimal? Porc_LSL2_BL { get; set; }
        public decimal? Porc_USL1_BL { get; set; }
        public decimal? Porc_USL2_BL { get; set; }

        public decimal? Meat_weight_inside_smokehouse_target { get; set; }
        public decimal? Meat_weight_inside_smokehouse { get; set; }
        public decimal? Porc_purge_target { get; set; }
        public decimal? Porc_purge { get; set; }
        public string Cooking { get; set; }
        public decimal? Meet_requirements1 { get; set; }
        public decimal? Meet_requirements2 { get; set; }
        public decimal? Marination_time_min { get; set; }
        public decimal? Marination_time_max { get; set; }
        public decimal? Marination_time { get; set; }
        public decimal? Wait_time_avg1 { get; set; }
        public decimal? Wait_time_avg2 { get; set; }
        public decimal? Max_time1 { get; set; }
        public decimal? Max_time2 { get; set; }
        public string Time_by_sample1 { get; set; }
        public string Time_by_sample2 { get; set; }
        public decimal? Foss_used_for_pull { get; set; }
        public decimal? Foss_used_for_packing { get; set; }
        public string Cooking_time_target { get; set; }
        public string Cooking_time_avg { get; set; }
        public decimal? Standard_pull_moisture_max { get; set; }
        public decimal? Standard_pull_moisture_min { get; set; }
        public decimal? Pull_moisture_avg { get; set; }
        public decimal? Staging_room_temperature1 { get; set; }
        public decimal? Staging_room_temperature2 { get; set; }
        public decimal? Staging_room_temperature_avg1 { get; set; }
        public decimal? Staging_room_temperature_avg2 { get; set; }
        public decimal? Packing_water_activity_max { get; set; }
        public decimal? Packing_water_activity { get; set; }
        public decimal? Packing_moisture_avg_min { get; set; }
        public decimal? Packing_moisture_avg_max { get; set; }
        public decimal? Packing_moisture_avg { get; set; }
        public decimal? Reanalysis_foss_1 { get; set; }
        public decimal? Reanalysis_foss_2 { get; set; }
        public decimal? Alpenas_moisture { get; set; }
        public decimal? Packing_room_temperature_max1 { get; set; }
        public decimal? Packing_room_temperature_max2 { get; set; }
        public decimal? Packing_room_temperature1 { get; set; }
        public decimal? Packing_room_temperature2 { get; set; }
        public decimal? Yield_target_min { get; set; }
        public decimal? Yield_target { get; set; }
        public decimal? Yield1 { get; set; }
        public decimal? Yield2 { get; set; }
        public decimal? Wood_chips_target { get; set; }
        public decimal? Wood_chips { get; set; }
        public decimal? Final_product_thickness_max { get; set; }
        public decimal? Final_product_thickness_min { get; set; }
        public decimal? Final_product_thickness_avg { get; set; }
        public decimal? Thickness_sample_size { get; set; }
        public decimal? Out_of_spec_target { get; set; }
        public decimal? Porc_out_spec { get; set; }
        public decimal? Porc_lsl { get; set; }
        public decimal? Porc_usl { get; set; }
        public string Filters_on_smokehouse_exhaustion { get; set; }
        public decimal? Reprocessing_target { get; set; }
        public decimal? Rework1 { get; set; }
        public decimal? Rework2 { get; set; }
        public decimal? Cooking_flavor { get; set; }
        public decimal? Odor { get; set; }
        public decimal? Texture { get; set; }
        public decimal? Appearance { get; set; }
        public string Observations { get; set; }


        public string Select (DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");

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
                         AND CL2.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtInit }  23:59:59'
 
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


        public string SelectFlavor(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
                   -- SABOR

            SELECT
             PMV_S.Name
             ,SUM(R.WeiEvaluation) AS Amostras
             FROM CollectionLevel2 C
             INNER JOIN CollectionLevel2XParHeaderField CHF
              ON C.id = CHF.CollectionLevel2_Id
             LEFT JOIN ParMultipleValues PMV_S
              ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_S.ID AS VARCHAR(30))
              AND CHF.ParHeaderField_Id = PMV_S.ParHeaderField_Id
             INNER JOIN Result_Level3 R
              ON C.ID = R.CollectionLevel2_Id
             WHERE 1=1
             AND CONVERT(DATE,C.CollectionDate) = '{ dtInit }'
             AND ParLevel1_Id = 182 
             AND ParLevel2_Id = 670 
             AND ParLevel3_Id = 1900
             AND CHF.ParHeaderField_Name = 'SABOR'
            GROUP BY 
             PMV_S.Name
                        ";

            return query;
        }

        public string SelectProduct(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
                   -- PRODUTO

                    SELECT
                     PMV_P.Name,SUM(R.WeiEvaluation) AS Amostras
                     FROM CollectionLevel2 C
                     INNER JOIN CollectionLevel2XParHeaderField CHF
                      ON C.id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                      ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.ID = R.CollectionLevel2_Id
                     WHERE 1=1
                     AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                     AND ParLevel1_Id = 182 
                     AND ParLevel2_Id = 670 
                     AND ParLevel3_Id = 1900
                     AND CHF.ParHeaderField_Name = 'PRODUTO'
                    GROUP BY 
                     PMV_P.Name

                        ";

            return query;
        }

        public string SelectBatch1(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                SELECT
                 S.Description AS Shift_Name,SUM(R.WeiEvaluation) AS Batch
                 FROM CollectionLevel2 C
                 INNER JOIN Result_Level3 R
                  ON C.ID = R.CollectionLevel2_Id
                 INNER JOIN Shift S
                  ON C.Shift = S.ID
                 WHERE 1=1
                 and s.id = 1
                 AND CONVERT(DATE,C.CollectionDate) = '{dtInit}' 
                 AND ParLevel1_Id = 182 
                 AND ParLevel2_Id = 670 
                 AND ParLevel3_Id = 1900
                 GROUP BY 
                  S.Description
                        ";

            return query;
        }

        public string SelectBatch2(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                SELECT
                 S.Description AS Shift_Name,SUM(R.WeiEvaluation) AS Batch
                 FROM CollectionLevel2 C
                 INNER JOIN Result_Level3 R
                  ON C.ID = R.CollectionLevel2_Id
                 INNER JOIN Shift S
                  ON C.Shift = S.ID
                 WHERE 1=1
                 and s.id = 2
                 AND CONVERT(DATE,C.CollectionDate) = '{dtInit}' 
                 AND ParLevel1_Id = 182 
                 AND ParLevel2_Id = 670 
                 AND ParLevel3_Id = 1900
                 GROUP BY 
                  S.Description
                        ";

            return query;
        }

        public string SelectPorcWater(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");

            var query = $@"

                 SELECT
                 S.Description AS Shift_Name
                 ,ISNULL(NULLIF(SUM(IIF(1=1 AND ParLevel2_Id = 663 AND ParLevel3_id = 1867 
                 ,CAST(R.Value AS DECIMAL(38,8)),0)),0) / 
                 NULLIF(SUM(IIF(1=1 AND ParLevel2_Id = 664 AND ParLevel3_id = 1868  
                 ,CAST(R.Value AS DECIMAL(38,8)),0)),0) * 100,0) [PorcWater]
                 FROM CollectionLevel2 C
                 INNER JOIN CollectionLevel2XParHeaderField CHF
                  ON C.id = CHF.CollectionLevel2_Id
  
                 INNER JOIN Result_Level3 R
                  ON C.ID = R.CollectionLevel2_Id
                 INNER JOIN Shift S
                  ON C.Shift = S.ID
                 WHERE 1=1
                 AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                 AND ParLevel1_Id = 181 
                 AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
                 GROUP BY 
                  S.Description
                        ";

            return query;
        }

        public string SelectMeatAgeAVG(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

            --AVG IDADE CARNE Por TURNO / Total de CARNE

            SELECT
             AVG(IIF(1 = 1, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS MeatAgeAVG --Agua_Utilizada
             FROM CollectionLevel2 C
             INNER JOIN CollectionLevel2XParHeaderField CHF
              ON C.id = CHF.CollectionLevel2_Id
             LEFT JOIN ParMultipleValues PMV_P
              ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
              AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id

             INNER JOIN Result_Level3 R
              ON C.ID = R.CollectionLevel2_Id
             INNER JOIN Shift S
              ON C.Shift = S.ID
             WHERE 1 = 1
             AND CONVERT(DATE, C.CollectionDate)  = '{dtInit}' -- Filtro Data
            AND ParLevel1_Id = 180
             AND ParLevel2_Id = 653
             AND ParLevel3_id = 2009
             AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
                        ";

            return query;
        }

        public string SelectMeatAgeMin(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

            --MIN IDADE CARNE Por TURNO / Total de CARNE

            SELECT
             MIN(IIF(1 = 1, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS MeatAgeMin --Agua_Utilizada
             FROM CollectionLevel2 C
             INNER JOIN CollectionLevel2XParHeaderField CHF
              ON C.id = CHF.CollectionLevel2_Id
             LEFT JOIN ParMultipleValues PMV_P
              ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
              AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id

             INNER JOIN Result_Level3 R
              ON C.ID = R.CollectionLevel2_Id
             INNER JOIN Shift S
              ON C.Shift = S.ID
             WHERE 1 = 1
             AND CONVERT(DATE, C.CollectionDate)  = '{dtInit}' -- Filtro Data
            AND ParLevel1_Id = 180
             AND ParLevel2_Id = 653
             AND ParLevel3_id = 2009
             AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
                        ";

            return query;
        }

        public string SelectMeatAgeMax(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
            --MAX IDADE CARNE Por TURNO / Total de CARNE

           SELECT
             MAX(IIF(1 = 1, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS MeatAgeMax --Agua_Utilizada
             FROM CollectionLevel2 C
             INNER JOIN CollectionLevel2XParHeaderField CHF
              ON C.id = CHF.CollectionLevel2_Id
             LEFT JOIN ParMultipleValues PMV_P
              ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
              AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id

             INNER JOIN Result_Level3 R
              ON C.ID = R.CollectionLevel2_Id
             INNER JOIN Shift S
              ON C.Shift = S.ID
             WHERE 1 = 1
             AND CONVERT(DATE, C.CollectionDate)  = '{dtInit}' -- Filtro Data
            AND ParLevel1_Id = 180
             AND ParLevel2_Id = 653
             AND ParLevel3_id = 2009
             AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
                        ";

            return query;
        }

        public string SelectMeats(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                        SELECT
                        SUBSTRING(PMV_P.NAME,CHARINDEX('/',PMV_P.NAME)+2,LEN(PMV_P.NAME))  AS Meat
                        ,SUM(IIF(1=1 AND ParLevel2_Id = 653 AND ParLevel3_id = 2009 ,CAST(r.Evaluation AS DECIMAL(38,8)),0)) AS Avaliacao
                        FROM CollectionLevel2 c
                        INNER JOIN Result_Level3 r
                        ON r.CollectionLevel2_Id = c.Id
                        INNER JOIN CollectionLevel2XParHeaderField CHF
                        ON C.id = CHF.CollectionLevel2_Id
                        LEFT JOIN ParMultipleValues PMV_P
                        ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                        AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                        WHERE 1=1
                        AND ParLevel1_Id = 180
                        AND CONVERT(DATE,C.CollectionDate) =  '{dtInit}'
                        AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
                        GROUP BY PMV_P.Name
                        ";

            return query;
        }

        public string SelectTumblerBatchSize(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
            SELECT
               SUM(IIF(1 = 1 AND ParLevel2_Id = 664 AND ParLevel3_id = 1868, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS Kg --Agua_Utilizada
              FROM CollectionLevel2 C
              INNER JOIN CollectionLevel2XParHeaderField CHF
               ON C.id = CHF.CollectionLevel2_Id
              LEFT JOIN ParMultipleValues PMV_P
               ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
               AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id

              --LEFT JOIN ParMultipleValues PMV_S
              --ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_S.ID AS VARCHAR(30))
             -- AND CHF.ParHeaderField_Id = PMV_S.ParHeaderField_Id
              INNER JOIN Result_Level3 R
               ON C.ID = R.CollectionLevel2_Id
              INNER JOIN Shift S
               ON C.Shift = S.ID
              WHERE 1 = 1
              AND CONVERT(DATE, C.CollectionDate) ='{dtInit}'
            AND ParLevel1_Id = 181
              AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'

                        ";

            return query;
        }


        //*************************** Revisar *******************************
        public string SelectProductAppearance(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
            --kg Carne Por SABOR
            -- No Documento diz que Coxão Mole é Diferente de Lagarto, ao contrário do Parametrizado, que diz que são iguais(Eyes)

              SELECT
               PMV_P.NAME AS Produto,
               SUM(IIF(1 = 1 AND ParLevel2_Id = 664 AND ParLevel3_id = 1868, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS kg_Carne --Agua_Utilizada
              FROM CollectionLevel2 C
              INNER JOIN CollectionLevel2XParHeaderField CHF
               ON C.id = CHF.CollectionLevel2_Id
              LEFT JOIN ParMultipleValues PMV_P
               ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
               AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
              INNER JOIN Result_Level3 R
               ON C.ID = R.CollectionLevel2_Id
              INNER JOIN Shift S
               ON C.Shift = S.ID
              WHERE 1 = 1
              AND CONVERT(DATE, C.CollectionDate) = '{dtInit}'
            AND ParLevel1_Id = 181
              AND CHF.ParHeaderField_Name = 'SABOR'
              GROUP BY
               PMV_P.NAME
                        ";

            return query;
        }

        //*************************** Revisar *******************************
        public string SelectSeasoningDistribuition(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
                 -- Perguntas sobre a distribuição dos temperos?

                  SELECT
                   ParLevel3_Name as Tarefa,
                   SUM(R.WeiEvaluation) AS Conforme, --Agua_Utilizada
                   SUM(R.WeiDefects) AS[Não Conforme]-- Agua_Utilizada
                FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id

                  --LEFT JOIN ParMultipleValues PMV_S
                  --ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_S.ID AS VARCHAR(30))
                 -- AND CHF.ParHeaderField_Id = PMV_S.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1 = 1
                  AND CONVERT(DATE, C.CollectionDate) = '{dtInit}'
                AND ParLevel1_Id = 182
                  AND ParLevel3_Id in (1904)
                  AND CHF.ParHeaderField_Name = 'SABOR'
                  GROUP BY
                   ParLevel3_Name
                        ";

            return query;
        }

        public string SelectMeatTemperature(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

            --Temperatura

              SELECT
              'C' Tipo_Graus,
               MIN(CAST(R.Value AS DECIMAL(38, 8))) AS Temperatura_Minima, --
                MAX(CAST(R.Value AS DECIMAL(38, 8))) AS Temperatura_Maxima,  --
                 AVG(CAST(R.Value AS DECIMAL(38, 8))) AS Temperatura_Atual  --
                 FROM CollectionLevel2 C
              INNER JOIN CollectionLevel2XParHeaderField CHF
               ON C.id = CHF.CollectionLevel2_Id
              LEFT JOIN ParMultipleValues PMV_P
               ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
               AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
              INNER JOIN Result_Level3 R
               ON C.ID = R.CollectionLevel2_Id
              INNER JOIN Shift S
               ON C.Shift = S.ID
              WHERE 1 = 1
              AND CONVERT(DATE, C.CollectionDate) = '{dtInit}'
            AND ParLevel1_Id = 180
              AND ParLevel3_Id in (1851)
              AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
            UNION ALL

              SELECT
              'F' Tipo_Graus,
               ((MIN(CAST(R.Value AS DECIMAL(38, 8)))) * 9 / 5) + 32 AS Temperatura_Minima, --
                      ((MAX(CAST(R.Value AS DECIMAL(38, 8)))) * 9 / 5) + 32 AS Temperatura_Maxima,  --
                             ((AVG(CAST(R.Value AS DECIMAL(38, 8)))) * 9 / 5) + 32 AS Temperatura_Atual  --
                                   FROM CollectionLevel2 C
              INNER JOIN CollectionLevel2XParHeaderField CHF
               ON C.id = CHF.CollectionLevel2_Id
              LEFT JOIN ParMultipleValues PMV_P
               ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
               AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
              INNER JOIN Result_Level3 R
               ON C.ID = R.CollectionLevel2_Id
              INNER JOIN Shift S
               ON C.Shift = S.ID
              WHERE 1 = 1
              AND CONVERT(DATE, C.CollectionDate) = '{dtInit}'
            AND ParLevel1_Id = 180
              AND ParLevel3_Id in (1851)
              AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
                        ";

            return query;
        }

        public string SelectThicknessCDCM(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

            --Coxão Duro + Coxão Mole

             SELECT
              PMV_P.NAME Name,
               MIN(CAST(R.Value AS DECIMAL(38, 8))) AS Espessura_Minima, --
                MAX(CAST(R.Value AS DECIMAL(38, 8))) AS Espessura_Maxima,  --
                 AVG(IIF(Shift = 1, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS Espessura_T1,  --
                    AVG(IIF(Shift = 2, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS Espessura_T2,  --
                       SUM(IIF(Shift = 1, R.Evaluation, 0)) AS Av_T1,
                         SUM(IIF(Shift = 2, R.Evaluation, 0)) AS Av_T2
              FROM CollectionLevel2 C
              INNER JOIN CollectionLevel2XParHeaderField CHF
               ON C.id = CHF.CollectionLevel2_Id
              LEFT JOIN ParMultipleValues PMV_P
               ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
               AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
              INNER JOIN Result_Level3 R
               ON C.ID = R.CollectionLevel2_Id
              INNER JOIN Shift S
               ON C.Shift = S.ID
              WHERE 1 = 1
              AND CONVERT(DATE, C.CollectionDate) = '{dtInit}'
            AND ParLevel1_Id = 180
              AND ParLevel3_Id in (1848)
              AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
              GROUP BY PMV_P.NAME
                        ";

            return query;
        }

        public string SelectThicknessBL(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

            SELECT
              PMV_P.NAME Name,
               MIN(CAST(R.Value AS DECIMAL(38, 8))) AS Espessura_Minima, --
               MAX(CAST(R.Value AS DECIMAL(38, 8))) AS Espessura_Maxima,  --
               AVG(IIF(Shift = 1, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS Espessura_T1,  --
               AVG(IIF(Shift = 2, CAST(R.Value AS DECIMAL(38, 8)), 0)) AS Espessura_T2,  --
               SUM(IIF(Shift = 1, R.Evaluation, 0)) AS Av_T1,
               SUM(IIF(Shift = 2, R.Evaluation, 0)) AS Av_T2
              FROM CollectionLevel2 C
              INNER JOIN CollectionLevel2XParHeaderField CHF
               ON C.id = CHF.CollectionLevel2_Id
              LEFT JOIN ParMultipleValues PMV_P
               ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
               AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
              INNER JOIN Result_Level3 R
               ON C.ID = R.CollectionLevel2_Id
              INNER JOIN Shift S
               ON C.Shift = S.ID
              WHERE 1 = 1
              AND CONVERT(DATE, C.CollectionDate) = '{dtInit}'
            AND ParLevel1_Id = 180
              AND ParLevel3_Id in (1849, 1850)
              AND CHF.ParHeaderField_Name = 'MATÉRIA-PRIMA/ RAW MATERIAL'
              GROUP BY PMV_P.NAME
                        ";

            return query;
        }

        public string SelectMeetCanadianRequirements(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                 -- Meet Canadian Requirements: 71°C 60 min (YES/NO/NA)

                  SELECT 'C' AS Name,
                   SUM(IIF(Shift = 1,R.WeiEvaluation,0)) - sum(IIF(Shift = 1,R.WeiDefects,0)) AS Av_T1,
                   SUM(IIF(Shift = 2,R.WeiEvaluation,0)) - SUM(IIF(Shift = 2,R.WeiDefects,0))AS Av_T2
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel1_Id = 187 
                  AND ParLevel3_Id in (1947)
                  AND CHF.ParHeaderField_Name = 'SABOR'
                  UNION ALL
                SELECT 'NC',
                   SUM(IIF(Shift = 1,R.WeiDefects,0)) AS Av_T1,
                   SUM(IIF(Shift = 2,R.WeiDefects,0)) AS Av_T2
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel1_Id = 187 
                  AND ParLevel3_Id in (1947)
                  AND CHF.ParHeaderField_Name = 'SABOR'
                ";

            return query;
        }


        public string SelectWaitTime(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                -- Qual o tempo de equalização do produto?

                SELECT 
                   AVG(IIF(Shift = 1,CAST(R.Value AS DECIMAL(38,8)),0)) AS TempoMedio1,
                   AVG(IIF(Shift = 2,CAST(R.Value AS DECIMAL(38,8)),0)) AS TempoMedio2,
                   MAX(IIF(Shift = 1,CAST(R.Value AS DECIMAL(38,8)),0)) AS TempoMaximo1,
                   MAX(IIF(Shift = 2,CAST(R.Value AS DECIMAL(38,8)),0)) AS TempoMaximo2
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel1_Id = 188
                  AND ParLevel3_Id in (1956)
                  AND CHF.ParHeaderField_Name = 'SABOR'";

            return query;
        }


        public string SelectPullMoistureAVG(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"GRT3:
                 -- Qual a umidade aferida?

                  SELECT
                   MIN(CAST(R.Value AS DECIMAL(38,8))) AS MIN_Packing_Moisture ,  -- 
                   MAX(CAST(R.Value AS DECIMAL(38,8))) AS MAX_Packing_Moisture , -- 
                   AVG(CAST(R.Value AS DECIMAL(38,8))) AS AVG_Packing_Moisture  -- 
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel3_Id in (1948)
                  AND CHF.ParHeaderField_Name = 'SABOR'";

            return query;
        }

        public string SelectStagingRoomTemperature(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                  SELECT
                  'C' Tipo_Graus,
                   MIN(CAST(R.Value AS DECIMAL(38,8))) AS Temperatura_Minima, -- 
                   MAX(CAST(R.Value AS DECIMAL(38,8))) AS Temperatura_Maxima,  -- 
                   AVG(CAST(R.Value AS DECIMAL(38,8))) AS Temperatura_Media  -- 
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel1_Id = 188 
                  AND ParLevel3_Id in (1954)
                  AND CHF.ParHeaderField_Name = 'SABOR'

                UNION ALL

                  SELECT
                  'F' Tipo_Graus,
                   ((MIN(CAST(R.Value AS DECIMAL(38,8))))*9/5)+32 AS Temperatura_Minima, -- 
                   ((MAX(CAST(R.Value AS DECIMAL(38,8))))*9/5)+32 AS Temperatura_Maxima,  -- 
                   ((AVG(CAST(R.Value AS DECIMAL(38,8))))*9/5)+32 AS Temperatura_Media  -- 
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel1_Id = 188 
                  AND ParLevel3_Id in (1954)
                  AND CHF.ParHeaderField_Name = 'SABOR'";

            return query;
        }


        public string SelectPackingWaterActivity(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                 -- Qual o resultado aferido no equipamento Aqualab?


                  SELECT
  
                   AVG(CAST(R.Value AS DECIMAL(38,8))) AS AVG_Packing_Water_Activity  -- 
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
  
                  --LEFT JOIN ParMultipleValues PMV_S
                  -- ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_S.ID AS VARCHAR(30))
                  -- AND CHF.ParHeaderField_Id = PMV_S.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel1_Id = 187 
                  AND ParLevel3_Id in (1946)
                  AND CHF.ParHeaderField_Name = 'SABOR'
                ";

            return query;
        }

        public string SelectPackingMoistureAVG(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                  SELECT
                   MIN(CAST(R.Value AS DECIMAL(38,8))) AS MIN_Packing_Moisture ,  -- 
                   MAX(CAST(R.Value AS DECIMAL(38,8))) AS MAX_Packing_Moisture , -- 
                   AVG(CAST(R.Value AS DECIMAL(38,8))) AS AVG_Packing_Moisture  -- 
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
  
                  --LEFT JOIN ParMultipleValues PMV_S
                  -- ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_S.ID AS VARCHAR(30))
                  -- AND CHF.ParHeaderField_Id = PMV_S.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel1_Id = 188 
                  AND ParLevel3_Id in (1957)
                  AND CHF.ParHeaderField_Name = 'SABOR'
                ";

            return query;
        }

        public string SelectReanalysisFoss1(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
                 -- Qual a umidade aferida?

                  SELECT
                   MIN(CAST(R.Value AS DECIMAL(38,8))) AS MIN_Packing_Moisture ,  -- 
                   MAX(CAST(R.Value AS DECIMAL(38,8))) AS MAX_Packing_Moisture , -- 
                   AVG(CAST(R.Value AS DECIMAL(38,8))) AS Reanalysis  -- 
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
  
                  --LEFT JOIN ParMultipleValues PMV_S
                  -- ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_S.ID AS VARCHAR(30))
                  -- AND CHF.ParHeaderField_Id = PMV_S.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  --AND ParLevel1_Id = 188 
                  AND ParLevel3_Id in (1948)
                  AND CHF.ParHeaderField_Name = 'SABOR'";

            return query;
        }

        public string SelectReanalysisFoss2(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
                 -- Qual a umidade fria aferida?

                  SELECT
                   MIN(CAST(R.Value AS DECIMAL(38,8))) AS MIN_Packing_Moisture ,  -- 
                   MAX(CAST(R.Value AS DECIMAL(38,8))) AS MAX_Packing_Moisture , -- 
                   AVG(CAST(R.Value AS DECIMAL(38,8))) AS Reanalysis  -- 
                  FROM CollectionLevel2 C
                  INNER JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
  
                  --LEFT JOIN ParMultipleValues PMV_S
                  -- ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_S.ID AS VARCHAR(30))
                  -- AND CHF.ParHeaderField_Id = PMV_S.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                  AND ParLevel1_Id = 188 
                  AND ParLevel3_Id in (1957)
                  AND CHF.ParHeaderField_Name = 'SABOR'";

            return query;
        }

        public string SelectPackingRoomTemperture(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                SELECT 
                  MAX(CAST(R.Value AS DECIMAL(38,8))) AS Temperatura_Maxima_C,  -- 
                  Avg(CAST(R.Value AS DECIMAL(38,8))) AS Temperatura_Media_C,
                  ((MAX(CAST(R.Value AS DECIMAL(38,8))))*9/5)+32 AS Temperatura_Maxima_F,  -- 
                  ((AVG(CAST(R.Value AS DECIMAL(38,8))))*9/5)+32 AS Temperatura_Media_F  -- 
                  FROM CollectionLevel2 C
                  LEFT JOIN CollectionLevel2XParHeaderField CHF
                   ON C.id = CHF.CollectionLevel2_Id
                  LEFT JOIN ParMultipleValues PMV_P
                   ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.ID AS VARCHAR(30))
                   AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                  INNER JOIN Result_Level3 R
                   ON C.ID = R.CollectionLevel2_Id
                  INNER JOIN Shift S
                   ON C.Shift = S.ID
                  WHERE 1=1
                  AND ParLevel3_Id in (2001)
                  AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'";

            return query;
        }

        public string SelectWoodChips(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                    SELECT
                      SUM(CAST(R.Value AS DECIMAL(38, 8))) AS MAX_CHIPS
                      ,AVG(CAST(R.Value AS DECIMAL(38, 8))) AS AVG_CHIPS
                     FROM CollectionLevel2 C

                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                      ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                       AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                       INNER JOIN Shift S
                      ON C.Shift = S.Id
                     WHERE 1 = 1
                     AND ParLevel3_Id IN (1941)
                     AND CONVERT(DATE, C.CollectionDate) >= '{dtInit}'";

            return query;
        }

        public string SelectFinalProductThicknessAVG(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                    SELECT
                      AVG(CAST(R.Value AS DECIMAL(38, 8))) AS Espessura,
                      COUNT (DISTINCT c.Id) AS Amostras
                     FROM CollectionLevel2 C
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND ParLevel3_Id IN (1967)
                     AND CONVERT(DATE, C.CollectionDate) >= '{dtInit}'
                     AND 1=1 AND ParLevel2_Id = 700 
                     AND ParLevel3_id = 1967";

            return query;
        }

        //*************************** Revisar *******************************
        public string SelectRework(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                    ---- tarefa 2002

                     SELECT
                     avg(cast(nullif (r.value ,'') as decimal(38,10))) Flavor
                     FROM CollectionLevel2 C
 
                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                     ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'
                     AND c.ParLevel1_Id = 205 
                     AND c.ParLevel2_Id = 729
                     AND r.parlevel3_id = 2002




 
                    ---- tarefa 2003

                      SELECT
                     avg(cast(nullif (r.value ,'') as decimal(38,10))) Odor
                     FROM CollectionLevel2 C
 
                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                     ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'
                     AND c.ParLevel1_Id = 205 
                     AND c.ParLevel2_Id = 729
                     AND r.parlevel3_id = 2003



 
                    ---- tarefa 2004

                      SELECT
                     avg(cast(nullif (r.value ,'') as decimal(38,10))) Texture
                     FROM CollectionLevel2 C
 
                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                     ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'
                     AND c.ParLevel1_Id = 205 
                     AND c.ParLevel2_Id = 729
                     AND r.parlevel3_id = 2004



 
                    ---- tarefa 2005


                      SELECT
                     avg(cast(nullif (r.value ,'') as decimal(38,10))) Appearance
                     FROM CollectionLevel2 C
 
                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                     ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'
                     AND c.ParLevel1_Id = 205 
                     AND c.ParLevel2_Id = 729
                     AND r.parlevel3_id = 2005";

            return query;
        }

        public string SelectCookingFlavor(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                    ---- tarefa 2002

                     SELECT
                     avg(cast(nullif (r.value ,'') as decimal(38,10))) Flavor
                     FROM CollectionLevel2 C
 
                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                     ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'
                     AND c.ParLevel1_Id = 205 
                     AND c.ParLevel2_Id = 729
                     AND r.parlevel3_id = 2002
";

            return query;
        }

        public string SelectOdor(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
 
                    ---- tarefa 2003

                      SELECT
                     avg(cast(nullif (r.value ,'') as decimal(38,10))) Odor
                     FROM CollectionLevel2 C
 
                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                     ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'
                     AND c.ParLevel1_Id = 205 
                     AND c.ParLevel2_Id = 729
                     AND r.parlevel3_id = 2003

";

            return query;
        }

        public string SelectTexture(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"
                    ---- tarefa 2004

                      SELECT
                     avg(cast(nullif (r.value ,'') as decimal(38,10))) Texture
                     FROM CollectionLevel2 C
 
                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                     ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'
                     AND c.ParLevel1_Id = 205 
                     AND c.ParLevel2_Id = 729
                     AND r.parlevel3_id = 2004
";

            return query;
        }

        public string SelectAppearance(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                    ---- tarefa 2005


                      SELECT
                     avg(cast(nullif (r.value ,'') as decimal(38,10))) Appearance
                     FROM CollectionLevel2 C
 
                     LEFT JOIN CollectionLevel2XParHeaderField CHF
                      ON C.Id = CHF.CollectionLevel2_Id
                     LEFT JOIN ParMultipleValues PMV_P
                     ON CAST(CHF.Value AS VARCHAR(30)) = CAST(PMV_P.Id AS VARCHAR(30))
                      AND CHF.ParHeaderField_Id = PMV_P.ParHeaderField_Id
                     INNER JOIN Result_Level3 R
                      ON C.Id = R.CollectionLevel2_Id
                     WHERE 1 = 1
                     AND CONVERT(DATE,C.CollectionDate) >= '{dtInit}'
                     AND c.ParLevel1_Id = 205 
                     AND c.ParLevel2_Id = 729
                     AND r.parlevel3_id = 2005";

            return query;
        }

        public string SelectMarinationTime(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                    --Marination Time
                    SELECT
                    AVG(CAST(R.Value AS DECIMAL(38,8))) AS Marination_time
                    FROM CollectionLevel2 C
                    INNER JOIN Result_Level3 R
                    ON C.ID = R.CollectionLevel2_Id
                    WHERE 1=1
                    AND CONVERT(DATE,C.CollectionDate) = '{dtInit}'
                    AND ParLevel3_Id in (1908)
                    GROUP BY r.ParLevel3_Name";

            return query;
        }

        public string SelectFoss1(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                    --Foss1
                    SELECT
	                    ((SUM(R.WeiEvaluation) - SUM(R.WeiDefects)) / SUM(R.WeiEvaluation)) * 100 PorcC
                    FROM CollectionLevel2 C
                    INNER JOIN Result_Level3 R
	                    ON C.Id = R.CollectionLevel2_Id
                    WHERE 1 = 1
                    AND CONVERT(DATE, C.CollectionDate) = '{dtInit}'
                    AND ParLevel3_Id = 1999";

            return query;
        }

        public string SelectFoss2(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var query = $@"

                    --Foss2
                    SELECT
	                    ((SUM(R.WeiEvaluation) - SUM(R.WeiDefects)) / SUM(R.WeiEvaluation)) * 100 PorcC
                    FROM CollectionLevel2 C
                    INNER JOIN Result_Level3 R
	                    ON C.Id = R.CollectionLevel2_Id
                    WHERE 1 = 1
                    AND CONVERT(DATE, C.CollectionDate) = '{dtInit}'
                    AND ParLevel3_Id = 2000";

            return query;
        }

    }
}

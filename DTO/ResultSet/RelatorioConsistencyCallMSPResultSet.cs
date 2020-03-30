using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ResultSet
{
    public class RelatorioConsistencyCallMSPResultSet
    {
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
        public decimal? Thickness_avg_max { get; set; }
        public decimal? Thickness_avg_min { get; set; }
        public decimal? Thickness_avg1 { get; set; }
        public decimal? Thickness_avg2 { get; set; }
        public decimal? Thickness_sample_size1 { get; set; }
        public decimal? Thickness_sample_size2 { get; set; }
        public decimal? Out_spec_target { get; set; }
        public decimal? Porc_out_spec1 { get; set; }
        public decimal? Porc_out_spec2 { get; set; }
        public decimal? Porc_LSL1 { get; set; }
        public decimal? Porc_LSL2 { get; set; }
        public decimal? Porc_USL1 { get; set; }
        public decimal? Porc_USL2 { get; set; }
        public decimal? Meat_weight_inside_smokehouse_target { get; set; }
        public decimal? Meat_weight_inside_smokehouse { get; set; }
        public decimal? Porc_purge_target { get; set; }
        public decimal? Porc_purge { get; set; }
        public string Cooking { get; set; }
        public string Meet_requirements1 { get; set; }
        public string Meet_requirements2 { get; set; }
        public int Marination_time_min { get; set; }
        public int Marination_time_max { get; set; }
        public int Marination_time { get; set; }
        public int Wait_time_avg { get; set; }
        public string Max_time1 { get; set; }
        public string Max_time2 { get; set; }
        public string Time_by_sample1 { get; set; }
        public string Time_by_sample2 { get; set; }
        public string Foss_used_for_pull { get; set; }
        public string Foss_used_for_packing { get; set; }
        public string Cooking_time_target { get; set; }
        public string Cooking_time_avg { get; set; }
        public decimal? Standard_pull_moisture_max { get; set; }
        public decimal? Standard_pull_moisture_min { get; set; }
        public decimal? Pull_moisture_avg { get; set; }
        public decimal? Standing_room_temperature { get; set; }
        public decimal? Standing_room_temperature_avg1 { get; set; }
        public decimal? Standing_room_temperature_avg2 { get; set; }
        public decimal? Packing_water_activity_max { get; set; }
        public decimal? Packing_water_activity { get; set; }
        public decimal? Packing_moisture_avg_min { get; set; }
        public decimal? Packing_moisture_avg_max { get; set; }
        public decimal? Packing_moisture_avg { get; set; }
        public decimal? Reanalysis_foss_1 { get; set; }
        public decimal? Reanalysis_foss_2 { get; set; }
        public decimal? Alpenas_moisture { get; set; }
        public decimal? Packing_room_temperature_max { get; set; }
        public decimal? Packing_room_temperature { get; set; }
        public decimal? Yield_target_min { get; set; }
        public decimal? Yield_target { get; set; }
        public decimal? Yield { get; set; }
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
        public decimal? Filters_on_smokehouse_exhaustion { get; set; }
        public decimal? Reprocessing_target { get; set; }
        public decimal? Rework { get; set; }
        public decimal? Cooking_flavor { get; set; }
        public decimal? Odor { get; set; }
        public decimal? Texture { get; set; }
        public decimal? Appearance { get; set; }
        public string Observations { get; set; }


        public string SelectEdicaoCabecalho(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");
            
            var sqlModulo = "";
            var sqlTurno = "";
            var sqlUnidade = "";
            var sqlLevel1 = "";
            var sqlLevel2 = "";
            var sqlLevel3 = "";
            var sqlDepartment = "";
            var sqlCargo = "";
            var formatDate = "";
            var sqlSgqMonitor = "";
            var sqlParReason = "";

            #region Filtros

            if (form.ParModule_Ids.Length > 0)
            {
                sqlModulo = $"\n AND plx.ParModule_Id in ({string.Join(",", form.ParModule_Ids)})";
            }

            if (form.Shift_Ids.Length > 0)
            {
                sqlTurno = $"\n AND [Shift] in ({string.Join(",", form.Shift_Ids)})";
            }

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlUnidade = $"\n AND cl.UnitId in ({string.Join(",", form.ParCompany_Ids)})";
            }

            if (form.ParLevel1_Ids.Length > 0)
            {
                sqlLevel1 = $"\n AND cl.ParLevel1_id in ({string.Join(",", form.ParLevel1_Ids)})";
            }

            if (form.ParLevel2_Ids.Length > 0)
            {
                sqlLevel2 = $"\n AND cl.ParLevel2_Id in ({string.Join(",", form.ParLevel2_Ids)})";
            }

            if (form.ParLevel3_Ids.Length > 0)
            {
                sqlLevel3 = $"\n AND rl.ParLevel3_Id  in ({string.Join(",", form.ParLevel3_Ids)})";
            }

            if (form.userSgqMonitor_Ids.Length > 0)
            {
                sqlSgqMonitor = $"\n AND cl.AuditorId  in ({string.Join(",", form.userSgqMonitor_Ids)})";
            }

            if (form.ParReason_Ids.Length > 0)
            {
                sqlParReason = $"\n AND lt.ParReason_Id  in ({string.Join(",", form.ParReason_Ids)})";
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

            #endregion

            var query = $@"
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
								FROM CollectionLevel2XParHeaderField CL2HF2 (NOLOCK)
								LEFT JOIN CollectionLevel2 CL2 (NOLOCK)
									ON CL2.Id = CL2HF2.CollectionLevel2_Id
								LEFT JOIN ParHeaderField HF (NOLOCK)
									ON CL2HF2.ParHeaderField_Id = HF.Id
								LEFT JOIN ParLevel2 L2 (NOLOCK)
									ON L2.Id = CL2.ParLevel2_Id
								LEFT JOIN ParMultipleValues PMV (NOLOCK)
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
							, 1, 1, '') AS HeaderFieldLis INTO #CollectionLevel2XParHeaderField2
						FROM CollectionLevel2XParHeaderField CL2HF (NOLOCK)
						INNER JOIN CollectionLevel2 CL2 (NOLOCK)
							ON CL2.Id = CL2HF.CollectionLevel2_Id
						LEFT JOIN ParHeaderField HF (NOLOCK)
							ON CL2HF.ParHeaderField_Id = HF.Id
						LEFT JOIN ParLevel2 L2 (NOLOCK)
							ON L2.Id = CL2.ParLevel2_Id
						GROUP BY CL2HF.CollectionLevel2_Id

	        	DECLARE @dtinicio varchar(30), @dtfinal varchar(30)
	        	--SET @dtinicio = '2019-01-30 00:00:51'
	        	--SET @dtfinal = '2019-12-31 23:59:59'

	        	SELECT
							cl.Id ID_collectionlv2
						   ,p1.Name AS Indicador
						   ,p2.Name AS Monitoramento
						   ,cl.CollectionDate AS _Data_Coleta
						   ,lt.AddDate AS _Data_Alteracao
						   --,clhf.ParHeaderField_Name as Nome_Cabecalho
						   --,SUBSTRING(cl2hf.headerfieldlis,9,15) Resultado
						   ,CLHF.Value as Valor_Texto 
						   --,CLHF.Evaluation
						   --,CLHF.Sample
						   ,USC.Name AS Usuario_Coleta
						   ,USA.Name AS Usuario_Alteracao
						   ,lt.ParReason_Id
						   ,pr.Motivo
						   ,lt.Motivo AS DescMotivo
						   ,CASE
								WHEN lt.ParReason_Id IS NULL THEN 'Original'
								ELSE 'Editado'
							END AS 'ORIGINAL_EDITADO' 
							,cl.adddate as Data_Adicao
							,lt.Json as json

						FROM
						-- Log
						LogTrack lt

						--Edição Cabeçalho

						-- Coleta  
						INNER JOIN CollectionLevel2XParHeaderField CLHF (NOLOCK)

							ON CLHF.Id = lt.Json_Id

						LEFT JOIN CollectionLevel2 cl (NOLOCK)

							ON cl.Id = CLHF.CollectionLevel2_Id

						LEFT JOIN #CollectionLevel2XParHeaderField2 cl2hf (NOLOCK)
							ON cl2hf.CollectionLevel2_Id = cl.Id

                        LEFT JOIN ParLevel1XModule plx 
				        	ON plx.ParLevel1_Id = cl.ParLevel1_Id

						-- Parametrizacao
						LEFT JOIN ParLevel1 p1 (NOLOCK)

							ON p1.Id = cl.ParLevel1_Id

						LEFT JOIN ParLevel2 p2 (NOLOCK)

							ON p2.Id = cl.ParLevel2_Id

						LEFT JOIN ParLevel1XModule pxm
							ON pxm.ParLevel1_Id = p1.Id


						LEFT JOIN ParCompany pc
							ON pc.Id = cl.UnitId

						-- Dim Usuario

						left JOIN UserSgq USC (NOLOCK)

							ON USC.Id = cl.AuditorId

						LEFT JOIN UserSgq USA (NOLOCK)

							ON USA.Id = lt.UserSgq_Id

						-- Cabeçalho
						LEFT JOIN ParHeaderField ph (NOLOCK)

							ON ph.Id = CLHF.ParHeaderField_Id


						-- Motivos
						LEFT JOIN ParReason pr (NOLOCK)

							ON pr.Id = lt.ParReason_Id


						INNER JOIN ParMultipleValues pmv (NOLOCK)

							ON pmv.Id = ph.Id


						WHERE 1 = 1

               AND cl.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF }  23:59:59' -- Filtro Data
               AND lt.Tabela = 'CollectionLevel2XParHeaderField'-- Tabela
                         { sqlModulo }                         
                         { sqlTurno }                         
                         { sqlUnidade }
                         { sqlLevel1 } 
                         { sqlLevel2 } 
                         { sqlSgqMonitor }
                         { sqlParReason }
               

              GROUP BY cl.Id
								,p1.Name
								,p2.Name
								,cl.CollectionDate
								,lt.AddDate
								,cl2hf.headerfieldlis
								,CLHF.Value
								,CLHF.Evaluation
								,CLHF.Sample
								,USC.Name
								,USA.Name
								,lt.ParReason_Id
								,pr.Motivo
								,lt.Motivo
								,cl.adddate
								,clhf.ParHeaderField_Name
                                ,lt.Json
						ORDER BY lt.AddDate ASC


               DROP TABLE #CollectionLevel2XParHeaderField2


            ";

            return query;
        }

        public string SelectEdicaoResultado(DataCarrierFormularioNew form)
        {
            var dtInit = form.startDate.ToString("yyyyMMdd");
            var dtF = form.endDate.ToString("yyyyMMdd");

            var  sqlModulo = "";
            var sqlTurno = "";
            var sqlUnidade = "";
            var sqlLevel1 = "";
            var sqlLevel2 = "";
            var sqlLevel3 = "";
            var sqlDepartment = "";
            var sqlCargo = "";
            var formatDate = "";
            var sqlSgqMonitor = "";
            var sqlParReason = "";


            if (form.ParModule_Ids.Length > 0)
            {
                sqlModulo = $"\n AND plx.ParModule_Id in ({string.Join(",", form.ParModule_Ids)})";
            }

            if (form.Shift_Ids.Length > 0)
            {
                sqlTurno = $"\n AND [Shift] in ({string.Join(",", form.Shift_Ids)})";
            }

            if (form.ParCompany_Ids.Length > 0)
            {
                sqlUnidade = $"\n AND cl.UnitId in ({string.Join(",", form.ParCompany_Ids)})";
            }

            if (form.ParLevel1_Ids.Length > 0)
            {
                sqlLevel1 = $"\n AND cl.ParLevel1_id in ({string.Join(",", form.ParLevel1_Ids)})";
            }

            if (form.ParLevel2_Ids.Length > 0)
            {
                sqlLevel2 = $"\n AND cl.ParLevel2_Id in ({string.Join(",", form.ParLevel2_Ids)})";
            }

            if (form.ParLevel3_Ids.Length > 0)
            {
                sqlLevel3 = $"\n AND rl.ParLevel3_Id  in ({string.Join(",", form.ParLevel3_Ids)})";
            }

            if (form.userSgqMonitor_Ids.Length > 0)
            {
                sqlSgqMonitor = $"\n AND cl.AuditorId  in ({string.Join(",", form.userSgqMonitor_Ids)})";
            }

            if (form.ParReason_Ids.Length > 0)
            {
                sqlParReason = $"\n AND lt.ParReason_Id  in ({string.Join(",", form.ParReason_Ids)})";
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
               --DECLARE @dtinicio varchar(30), @dtfinal varchar(30)
	        	--SET @dtinicio = '2019-01-30 00:00:51'
	        	--SET @dtfinal = '2019-12-31 23:59:59'

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
								FROM CollectionLevel2XParHeaderField CL2HF2 (NOLOCK)
								LEFT JOIN CollectionLevel2 CL2 (NOLOCK)
									ON CL2.Id = CL2HF2.CollectionLevel2_Id
								LEFT JOIN ParHeaderField HF (NOLOCK)
									ON CL2HF2.ParHeaderField_Id = HF.Id
								LEFT JOIN ParLevel2 L2 (NOLOCK)
									ON L2.Id = CL2.ParLevel2_Id
								LEFT JOIN ParMultipleValues PMV (NOLOCK)
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
							, 1, 1, '') AS HeaderFieldLis INTO #CollectionLevel2XParHeaderField2
						FROM CollectionLevel2XParHeaderField CL2HF (NOLOCK)
						INNER JOIN CollectionLevel2 CL2 (NOLOCK)
							ON CL2.Id = CL2HF.CollectionLevel2_Id
						LEFT JOIN ParHeaderField HF (NOLOCK)
							ON CL2HF.ParHeaderField_Id = HF.Id
						LEFT JOIN ParLevel2 L2 (NOLOCK)
							ON L2.Id = CL2.ParLevel2_Id
						GROUP BY CL2HF.CollectionLevel2_Id

	        	SELECT
	        		p1.Name AS Indicador
	        	   ,p2.Name AS Monitoramento
	        	   --,p3.Name AS Tarefa
	        	   ,cl.CollectionDate as _Data_Coleta
				   ,cl.AlterDate as _Data_Alteracao
	        	   --,rl.Value as Resultado 
				   --,rl.ValueText as Valor_Texto 
	        	    ,CASE
	        		 	WHEN rl.isconform = 1 THEN 'Conforme'
	        		 	ELSE 'Nâo Conforme'
	        		 END AS Conforme
	        	   --,CASE
	        		--	WHEN rl.IsNotEvaluate = 0 THEN 'Avaliado'
	        			--ELSE 'Não Avaliado'
	        		--END AS 'AVALIADO_NAO_AVALIADO'

	        	   ,USC.Name AS Usuario_Coleta
	        	   ,USA.Name AS Usuario_Alteracao
	        	   ,lt.ParReason_Id
	        	   ,pr.Motivo
	        	   ,lt.Motivo as DescMotivo
	        	   ,CASE
                       WHEN lt.ParReason_Id IS NULL THEN 'Original'

                       ELSE 'Editado'

                   END AS 'ORIGINAL_EDITADO'
				   --,rl.WeiEvaluation AS Av_Peso
				   --,rl.IsNotEvaluate AS  Avaliado
				   --,cl.WeiDefects AS NC_Peso
				   ,cl.EvaluationNumber as Avaliacao
				   ,cl.Sample as Amostra
				   ,clxp.HeaderFieldLis AS CamposCabecalho
				   ,cl.AddDate as Data_Adicao
				   ,lt.json


                FROM
               -- Log
               LogTrack lt

               -- Coleta
               INNER JOIN Result_Level3 rl(NOLOCK)

                   ON rl.Id = lt.Json_Id

               LEFT JOIN CollectionLevel2 cl(NOLOCK)

                   ON cl.Id = rl.CollectionLevel2_Id

			    LEFT JOIN #CollectionLevel2XParHeaderField2 clxp
			    ON clxp.CollectionLevel2_Id = cl.Id

                LEFT JOIN ParLevel1XModule plx 
					ON plx.ParLevel1_Id = cl.ParLevel1_Id

               -- Parametrizacao
               LEFT JOIN ParLevel1 p1(NOLOCK)

                   ON p1.Id = cl.ParLevel1_Id

               LEFT JOIN ParLevel2 p2(NOLOCK)

                   ON p2.Id = cl.ParLevel2_Id

               LEFT JOIN ParLevel3 p3(NOLOCK)

                   ON p3.Id = rl.ParLevel3_Id

				LEFT JOIN ParCompany pc
							ON pc.Id = cl.UnitId

               -- Dim Usuario

               LEFT JOIN UserSgq USC(NOLOCK)

                   ON USC.Id = cl.AuditorId

               LEFT JOIN UserSgq USA(NOLOCK)

                   ON USA.Id = lt.UserSgq_Id

               -- Motivos
               LEFT JOIN ParReason pr(NOLOCK)

                   ON pr.Id = lt.ParReason_Id


               WHERE 1 = 1

               AND cl.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF }  23:59:59' -- Filtro Data
               AND Tabela = 'Result_Level3'-- Tabela
                         { sqlModulo }                         
                         { sqlTurno }                         
                         { sqlUnidade }                        
                         { sqlLevel1 } 
                         { sqlLevel2 }
                         { sqlLevel3 }
                         { sqlSgqMonitor }
                         { sqlParReason }

              ORDER BY cl.CollectionDate, p1.Name, p2.Name,p3.name, lt.AddDate ASC ";

            return query;
        }
    }
}

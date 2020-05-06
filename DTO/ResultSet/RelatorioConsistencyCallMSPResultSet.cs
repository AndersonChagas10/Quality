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
        public string Foss_used_for_pull { get; set; }
        public string Foss_used_for_packing { get; set; }
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

        public string SelectThickness(DataCarrierFormularioNew form)
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

    }
}

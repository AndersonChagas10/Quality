using ADOFactory;
using Dominio;
using DTO;
using Quartz;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SgqSystem.Jobs
{
    public class MandalaJob : IJob
    {
        public const int JornadaDeTrabalho = 10;
        public const int Horainicio = 8;
        public const int HoraFim = 18;

        public void Execute(IJobExecutionContext context)
        {
            // PreencherListaMandala(null);
        }

        public static void PreencherListaMandala(object stateInfo)
        {
            while (true)
            {
                if (ConfigurationManager.AppSettings["PreencherMandala"] == "on")
                {
                    Task.Run(() =>
                    {
                        PreencherListaUnidadeMandala(null);
                        Task.Delay(2000);
                        PreencherListaIndicadorMandala(null);
                        Task.Delay(2000);
                        PreencherListaMonitoramentoMandala(null);
                    });
                    Thread.Sleep(60000);
                }
                else
                {
                    GlobalConfig.MandalaUnidade = null;
                    GlobalConfig.MandalaIndicador = null;
                    GlobalConfig.MandalaMonitoramento = null;
                    Thread.Sleep(3000000);
                }
                GlobalConfig.UltimaExecucaoDoJob["PreencherMandala"] = DateTime.Now;
            }
        }

        private static void PreencherListaMonitoramentoMandala(object stateInfo)
        {
            try
            {

                var query = $@"               
                 declare @inicio datetime = DATEADD(DAY,-1,GETDATE()) 
 
                 declare @Fim datetime = DATEADD(DAY,0,GETDATE())

                 declare @horasrabalhadas float = 16
                 declare @horaInicioTrabalho float = 9

                SELECT 

		                ParCompany_id,
		                ParCompany_Name,
		                ParLevel1_id,
		                ParLevel1_name,
		                ParLevel2_id,
		                ParLevel2_Name,
		                SUM([EvaluationPlan]) AS [Avaliacoes_Planejadas],
						(SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho) as [Avaliacoes_Planejadas_Momento],
						CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) as [%_Avaliacoes_Planejadas_Momento],



		                SUM([Evaluation])     AS [Avaliacoes_Realizadas],
		                SUM([SamplePlan])	  AS [Amostras_Planejadas],
		                SUM([Sample])		  AS [Amostras_Realizadas],
		                ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 AS [Coletado],
		                CASE  
			                WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN  0.00000 AND  80.99999 THEN 'red'
			                WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN 81.00000 AND  99.99999 THEN 'yellow'
			               -- WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN 81.00000 AND 100.00000 THEN 'green'
			                WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) > 100 THEN 'green'
		                END AS [Cor]
                 FROM (

	                SELECT 
		                 Cubo.ParStructureGroup_Name
		                ,Cubo.ParStructure_Name
		                ,Cubo.ParCompany_id
		                ,Cubo.ParCompany_Name
		                ,Cubo.Data
		                ,Cubo.ANO as Year
		                ,Cubo.MES as Month
		                ,Cubo.DIA as Day
		                ,Cubo.ParConsolidationType_Id
		                ,Cubo.ParLevel1_Id
		                ,Cubo.ParLevel1_Name
		                ,Cubo.ParLevel2_id
		                ,Cubo.ParLevel2_Name
		                ,Cubo.ParFrequency_Id
		                ,Cubo.ShiftPlan
		                ,Cubo.PeriodPlan		               
                        ,MAX(CAST(Cubo.EvaluationPlan AS FLOAT)) AS EvaluationPlan
		                ,MAX(CAST(Cubo.SamplePlan AS FLOAT)) AS SamplePlan
		                ,MAX(CAST(Cubo.Evaluation AS FLOAT)) AS Evaluation
		                ,MAX(CAST(Cubo.Sample AS FLOAT)) AS [Sample]
	
	
	                 FROM (
	                SELECT -- TOP 10
	                (SELECT TOP 1 Name FROM ParStructureGroup WHERE id in (SELECT TOP 1 ParStructureGroup_Id FROM ParStructure WHERE 1=1 AND id in (SELECT ParStructure_Id FROM ParCompanyXStructure CS WHERE CS.ParCompany_id = Levantamento.ParCompany_id))) ParStructureGroup_Name,
	                (SELECT TOP 1 Name FROM ParStructure WHERE 1=1 AND id in (SELECT ParStructure_Id FROM ParCompanyXStructure CS WHERE CS.ParCompany_id = Levantamento.ParCompany_id)) ParStructure_Name,
	                ParCompany_id,
	                (SELECT TOP 1 Name FROM ParCompany C WITH (NOLOCK) WHERE 1=1 AND C.id = Levantamento.ParCompany_id ) AS ParCompany_Name,
	
	                Data,
	                YEAR(Data) ANO,
	                MONTH(Data) MES,
	                DAY(Data) DIA,
	                (SELECT TOP 1 ParConsolidationType_Id FROM ParLevel1 L1 WHERE 1=1 AND L1.id = Levantamento.ParLevel1_Id) ParConsolidationType_Id,
	                ParLevel1_Id,
	                (SELECT TOP 1 Name FROM ParLevel1 P1 WITH (NOLOCK) WHERE 1=1 AND P1.id = Levantamento.ParLevel1_Id ) AS ParLevel1_Name,
	                ParLevel2_id,
	                (SELECT TOP 1 Name FROM ParLevel2 P2 WITH (NOLOCK) WHERE 1=1 AND P2.id = Levantamento.ParLevel2_id ) AS ParLevel2_Name,
	                ParFrequency_Id,
	                ShiftPlan,
	                PeriodPlan,
	                EvaluationPlan,
	                SamplePlan,
	                Evaluation,
	                Sample
	
	                 FROM 
			                (
	
		                SELECT 
			                ORCADO.Data,
			                ISNULL((SELECT Top 1 ParLevel1_Id FROM ParLevel2Level1 P21 WITH (NOLOCK) where IsActive = 1 AND P21.ParLevel2_Id = ORCADO.ParLevel2_Id AND P21.ParCompany_Id = ORCADO.ParCompany_Id)
		                    ,(SELECT Top 1 ParLevel1_Id FROM ParLevel2Level1 P21 WITH (NOLOCK) where IsActive = 1 AND P21.ParLevel2_Id = ORCADO.ParLevel2_Id AND P21.ParCompany_Id IS NULL)) AS ParLevel1_Id,
			                ORCADO.ParLevel2_id,
			                ORCADO.ParCompany_id,
			                ORCADO.ParFrequency_Id,
			
			                ORCADO.Shift AS ShiftPlan,
			                ORCADO.Period AS PeriodPlan,
		
			                ISNULL(ORCADO.EvaluationPlan,0) AS EvaluationPlan,
			                ISNULL(ORCADO.SamplePlan,0) AS SamplePlan,
		
			                ISNULL(REAL.EvaluationNumber,0) AS Evaluation,
			                ISNULL(REAL.Sample,0) AS Sample 
		
		                FROM (
		
			                SELECT 
				                Data
				                ,ParLevel2_id
				                ,ParCompany_id
				                ,ParFrequency_Id
				                ,Period
				                ,Shift
				                ,EvaluationPlan
				                ,SamplePlan

                            FROM(
                                SELECT

                                ParLevel2_id
                                , ParCompany_id
                                , ParFrequency_Id
                                , Period
                                , Shift
                                , ISNULL((SELECT TOP 1 Number FROM ParEvaluation PE WHERE PE.ParLevel2_Id = CO.ParLevel2_id AND PE.ParCompany_Id = CO.ParCompany_Id and PE.IsActive = 1)
                                , (SELECT TOP 1 Number FROM ParEvaluation PE WHERE PE.ParLevel2_Id = CO.ParLevel2_id AND PE.ParCompany_Id IS NULL and PE.IsActive = 1)) AS EvaluationPlan
				                ,ISNULL((SELECT TOP 1 Number FROM ParSample PS WHERE PS.ParLevel2_Id = CO.ParLevel2_id AND PS.ParCompany_Id = CO.ParCompany_Id and PS.IsActive = 1)
				                ,(SELECT TOP 1 Number FROM ParSample PS WITH(NOLOCK) WHERE PS.ParLevel2_Id = CO.ParLevel2_id AND PS.ParCompany_Id IS NULL and PS.IsActive = 1)) AS SamplePlan

                                 FROM(
                                  SELECT

                                     P2.id ParLevel2_id
                                    , PC.id ParCompany_id
                                    , P2.Shift
                                    , P2.Period

                                  FROM(
                                        SELECT id, Shift, Period, ParFrequency_id

                                            FROM ParLevel2 P2 WITH(NOLOCK)

                                        CROSS JOIN
                                            (SELECT 1 Shift UNION ALL SELECT 2) Shift

                                        CROSS JOIN
                                            (SELECT 1 Period) Period

                                        WHERE ParFrequency_id = 1

                                        UNION ALL

                                        SELECT id, Shift, Period, ParFrequency_id FROM(
                                        SELECT id, 1 Period, ParFrequency_id

                                            FROM ParLevel2 P2

                                        WHERE ParFrequency_id != 1)a

                                        CROSS JOIN
                                            (SELECT 1 Shift UNION ALL SELECT 2) Shift) P2

                                    CROSS JOIN ParCompany PC WITH(NOLOCK)

                                    WHERE PC.IsActive = 1
                                    ) CO
                                LEFT JOIN ParLevel2 P2 WITH(NOLOCK)

                                    ON CO.ParLevel2_id = p2.Id


                                WHERE IsActive = 1
			                )ORCADO
                            CROSS JOIN ParCalendar Cal WITH(NOLOCK)
		                ) ORCADO
                        LEFT JOIN(
                                SELECT

                                    cast(c2.CollectionDate as date) Data,
                                    unitid ParCompany_id,
                                    ParLevel2_id,
                                    Shift,
                                    Period,
                                   MAX(CAST(EvaluationNumber AS FLOAT)) EvaluationNumber,
                                   MAX(CAST(Sample AS FLOAT)) Sample

                                    FROM CollectionLevel2 c2 WITH(NOLOCK)

                                    LEFT JOIN ParCalendar Cal WITH(NOLOCK) ON cast(c2.CollectionDate as date) = Cal.Data

                                    LEFT JOIN ParLevel1 L1 ON c2.ParLevel1_Id = L1.Id

                                    WHERE L1.IsActive = 1

                                Group by

                                    cast(c2.CollectionDate as date),
                                    ParLevel2_id,
                                    unitid,
                                    Shift,
                                    Period
                            )REAL


                        ON ORCADO.Data = REAL.Data

                         AND ORCADO.ParLevel2_id = REAL.ParLevel2_id

                         AND ORCADO.ParCompany_id = REAL.ParCompany_id

                         AND ORCADO.Shift = REAL.Shift

                         AND(ORCADO.Period = REAL.Period OR ORCADO.Period = 1)

                        WHERE 1 = 1

                         AND ORCADO.Data BETWEEN @inicio AND @Fim
		                 ) Levantamento
                          WHERE 1 = 1

                         AND ParLevel1_Id is not null
		                )CUBO
                            INNER JOIN ParLevel1 L1
                                ON CUBO.ParLevel1_id = L1.id

                                AND L1.IsActive = 1


                            INNER JOIN ParCompanyCluster CCL WITH(NOLOCK)

                                ON CCL.ParCompany_id = CUBO.ParCompany_id

                                AND CCL.Active = 1


                            LEFT JOIN ParCompanyXStructure CS WITH(NOLOCK)

                                ON CUBO.ParCompany_Id = CS.ParCompany_Id

                                AND CS.Active = 1


                            LEFT JOIN ParStructure S WITH(NOLOCK)

                                ON CS.ParStructure_id = S.id


                            LEFT JOIN ParStructureGroup SG WITH(NOLOCK)

                                ON S.ParStructureGroup_Id = SG.ID


                            WHERE 1 = 1

                        GROUP BY

                        Cubo.ParStructureGroup_Name
			                ,Cubo.ParStructure_Name
			                ,Cubo.ParCompany_id
			                ,Cubo.ParCompany_Name
			                ,Cubo.Data
			                ,Cubo.ANO
			                ,Cubo.MES
			                ,Cubo.DIA
			                ,Cubo.ParConsolidationType_Id
			                ,Cubo.ParLevel1_Id
			                ,Cubo.ParLevel1_Name
			                ,Cubo.ParLevel2_id
			                ,Cubo.ParLevel2_Name
			                ,Cubo.ParFrequency_Id
			                ,Cubo.ShiftPlan
			                ,Cubo.PeriodPlan
                            --,Cubo.EvaluationPlan
                            --,Cubo.SamplePlan
                            --,Cubo.Evaluation
                            --,Cubo.Sample
                ) CUBO
                WHERE 1 = 1
                --AND ParLevel1_Name = '(%) NC Análise Microbiológica em Produto (Desossa)'
                --AND ParLevel2_Name = 'Análise Microbiológica em Produto (Desossa)'
                Group by

                    ParCompany_Name,
	                ParCompany_id,
	                ParLevel1_id,
	                ParLevel1_Name,
	                ParLevel2_id,
	                ParLevel2_Name
                    --ORDER BY 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20";
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    GlobalConfig.MandalaMonitoramento = factory.SearchQuery<Mandala>(query).ToList();
                }

            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro no metodo Preencher Monitoramento", ex));
            }
        }

        public static void PreencherListaIndicadorMandala(object stateInfo)
        {
            try
            {
                
                //Thread.Sleep(30000);
                var query = $@"                
                 declare @inicio datetime = DATEADD(DAY,-1,GETDATE()) 
 
                 declare @Fim datetime = DATEADD(DAY,0,GETDATE())

                 declare @horasrabalhadas float = 16
                 declare @horaInicioTrabalho float = 9

                        SELECT 

		                        ParCompany_id,
		                        ParCompany_Name,
		                        ParLevel1_id,
		                        ParLevel1_name,
		                        --ParLevel2_id,
		                        --ParLevel2_Name,
		                        SUM([EvaluationPlan]) AS [Avaliacoes_Planejadas],
						        (SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho) as [Avaliacoes_Planejadas_Momento],
						        CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) as [%_Avaliacoes_Planejadas_Momento],



		                        SUM([Evaluation])     AS [Avaliacoes_Realizadas],
		                        SUM([SamplePlan])	  AS [Amostras_Planejadas],
		                        SUM([Sample])		  AS [Amostras_Realizadas],
		                        ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 AS [Coletado],
		                        CASE  
			                        WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN  0.00000 AND  80.99999 THEN 'red'
			                        WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN 81.00000 AND  99.99999 THEN 'yellow'
			                        -- WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN 81.00000 AND 100.00000 THEN 'green'
			                        WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) > 100 THEN 'green'
		                        END AS [Cor]
                            FROM (

	                        SELECT 
		                            Cubo.ParStructureGroup_Name
		                        ,Cubo.ParStructure_Name
		                        ,Cubo.ParCompany_id
		                        ,Cubo.ParCompany_Name
		                        ,Cubo.Data
		                        ,Cubo.ANO as Year
		                        ,Cubo.MES as Month
		                        ,Cubo.DIA as Day
		                        ,Cubo.ParConsolidationType_Id
		                        ,Cubo.ParLevel1_Id
		                        ,Cubo.ParLevel1_Name
		                        ,Cubo.ParLevel2_id
		                        ,Cubo.ParLevel2_Name
		                        ,Cubo.ParFrequency_Id
		                        ,Cubo.ShiftPlan
		                        ,Cubo.PeriodPlan		               
                                ,MAX(CAST(Cubo.EvaluationPlan AS FLOAT)) AS EvaluationPlan
		                        ,MAX(CAST(Cubo.SamplePlan AS FLOAT)) AS SamplePlan
		                        ,MAX(CAST(Cubo.Evaluation AS FLOAT)) AS Evaluation
		                        ,MAX(CAST(Cubo.Sample AS FLOAT)) AS [Sample]
	
	
	                            FROM (
	                        SELECT -- TOP 10
	                        (SELECT TOP 1 Name FROM ParStructureGroup WHERE id in (SELECT TOP 1 ParStructureGroup_Id FROM ParStructure WHERE 1=1 AND id in (SELECT ParStructure_Id FROM ParCompanyXStructure CS WHERE CS.ParCompany_id = Levantamento.ParCompany_id))) ParStructureGroup_Name,
	                        (SELECT TOP 1 Name FROM ParStructure WHERE 1=1 AND id in (SELECT ParStructure_Id FROM ParCompanyXStructure CS WHERE CS.ParCompany_id = Levantamento.ParCompany_id)) ParStructure_Name,
	                        ParCompany_id,
	                        (SELECT TOP 1 Name FROM ParCompany C WITH (NOLOCK) WHERE 1=1 AND C.id = Levantamento.ParCompany_id ) AS ParCompany_Name,
	
	                        Data,
	                        YEAR(Data) ANO,
	                        MONTH(Data) MES,
	                        DAY(Data) DIA,
	                        (SELECT TOP 1 ParConsolidationType_Id FROM ParLevel1 L1 WHERE 1=1 AND L1.id = Levantamento.ParLevel1_Id) ParConsolidationType_Id,
	                        ParLevel1_Id,
	                        (SELECT TOP 1 Name FROM ParLevel1 P1 WITH (NOLOCK) WHERE 1=1 AND P1.id = Levantamento.ParLevel1_Id ) AS ParLevel1_Name,
	                        ParLevel2_id,
	                        (SELECT TOP 1 Name FROM ParLevel2 P2 WITH (NOLOCK) WHERE 1=1 AND P2.id = Levantamento.ParLevel2_id ) AS ParLevel2_Name,
	                        ParFrequency_Id,
	                        ShiftPlan,
	                        PeriodPlan,
	                        EvaluationPlan,
	                        SamplePlan,
	                        Evaluation,
	                        Sample
	
	                            FROM 
			                        (
	
		                        SELECT 
			                        ORCADO.Data,
			                        ISNULL((SELECT Top 1 ParLevel1_Id FROM ParLevel2Level1 P21 WITH (NOLOCK) where IsActive = 1 AND P21.ParLevel2_Id = ORCADO.ParLevel2_Id AND P21.ParCompany_Id = ORCADO.ParCompany_Id)
		                            ,(SELECT Top 1 ParLevel1_Id FROM ParLevel2Level1 P21 WITH (NOLOCK) where IsActive = 1 AND P21.ParLevel2_Id = ORCADO.ParLevel2_Id AND P21.ParCompany_Id IS NULL)) AS ParLevel1_Id,
			                        ORCADO.ParLevel2_id,
			                        ORCADO.ParCompany_id,
			                        ORCADO.ParFrequency_Id,
			
			                        ORCADO.Shift AS ShiftPlan,
			                        ORCADO.Period AS PeriodPlan,
		
			                        ISNULL(ORCADO.EvaluationPlan,0) AS EvaluationPlan,
			                        ISNULL(ORCADO.SamplePlan,0) AS SamplePlan,
		
			                        ISNULL(REAL.EvaluationNumber,0) AS Evaluation,
			                        ISNULL(REAL.Sample,0) AS Sample 
		
		                        FROM (
		
			                        SELECT 
				                        Data
				                        ,ParLevel2_id
				                        ,ParCompany_id
				                        ,ParFrequency_Id
				                        ,Period
				                        ,Shift
				                        ,EvaluationPlan
				                        ,SamplePlan

                                    FROM(
                                        SELECT

                                        ParLevel2_id
                                        , ParCompany_id
                                        , ParFrequency_Id
                                        , Period
                                        , Shift
                                        , ISNULL((SELECT TOP 1 Number FROM ParEvaluation PE WHERE PE.ParLevel2_Id = CO.ParLevel2_id AND PE.ParCompany_Id = CO.ParCompany_Id and PE.IsActive = 1)
                                        , (SELECT TOP 1 Number FROM ParEvaluation PE WHERE PE.ParLevel2_Id = CO.ParLevel2_id AND PE.ParCompany_Id IS NULL and PE.IsActive = 1)) AS EvaluationPlan
				                        ,ISNULL((SELECT TOP 1 Number FROM ParSample PS WHERE PS.ParLevel2_Id = CO.ParLevel2_id AND PS.ParCompany_Id = CO.ParCompany_Id and PS.IsActive = 1)
				                        ,(SELECT TOP 1 Number FROM ParSample PS WITH(NOLOCK) WHERE PS.ParLevel2_Id = CO.ParLevel2_id AND PS.ParCompany_Id IS NULL and PS.IsActive = 1)) AS SamplePlan

                                            FROM(
                                            SELECT

                                                P2.id ParLevel2_id
                                            , PC.id ParCompany_id
                                            , P2.Shift
                                            , P2.Period

                                            FROM(
                                                SELECT id, Shift, Period, ParFrequency_id

                                                    FROM ParLevel2 P2 WITH(NOLOCK)

                                                CROSS JOIN
                                                    (SELECT 1 Shift UNION ALL SELECT 2) Shift

                                                CROSS JOIN
                                                    (SELECT 1 Period) Period

                                                WHERE ParFrequency_id = 1

                                                UNION ALL

                                                SELECT id, Shift, Period, ParFrequency_id FROM(
                                                SELECT id, 1 Period, ParFrequency_id

                                                    FROM ParLevel2 P2

                                                WHERE ParFrequency_id != 1)a

                                                CROSS JOIN
                                                    (SELECT 1 Shift UNION ALL SELECT 2) Shift) P2

                                            CROSS JOIN ParCompany PC WITH(NOLOCK)

                                            WHERE PC.IsActive = 1
                                            ) CO
                                        LEFT JOIN ParLevel2 P2 WITH(NOLOCK)

                                            ON CO.ParLevel2_id = p2.Id


                                        WHERE IsActive = 1
			                        )ORCADO
                                    CROSS JOIN ParCalendar Cal WITH(NOLOCK)
		                        ) ORCADO
                                LEFT JOIN(
                                        SELECT

                                            cast(c2.CollectionDate as date) Data,
                                            unitid ParCompany_id,
                                            ParLevel2_id,
                                            Shift,
                                            Period,
                                            MAX(CAST(EvaluationNumber AS FLOAT)) EvaluationNumber,
                                            MAX(CAST(Sample AS FLOAT)) Sample

                                            FROM CollectionLevel2 c2 WITH(NOLOCK)

                                            LEFT JOIN ParCalendar Cal WITH(NOLOCK) ON cast(c2.CollectionDate as date) = Cal.Data

                                            LEFT JOIN ParLevel1 L1 ON c2.ParLevel1_Id = L1.Id

                                            WHERE L1.IsActive = 1

                                        Group by

                                            cast(c2.CollectionDate as date),
                                            ParLevel2_id,
                                            unitid,
                                            Shift,
                                            Period
                                    )REAL


                                ON ORCADO.Data = REAL.Data

                                    AND ORCADO.ParLevel2_id = REAL.ParLevel2_id

                                    AND ORCADO.ParCompany_id = REAL.ParCompany_id

                                    AND ORCADO.Shift = REAL.Shift

                                    AND(ORCADO.Period = REAL.Period OR ORCADO.Period = 1)

                                WHERE 1 = 1

                                    AND ORCADO.Data BETWEEN @inicio AND @Fim
		                            ) Levantamento
                                    WHERE 1 = 1

                                    AND ParLevel1_Id is not null
		                        )CUBO
                                    INNER JOIN ParLevel1 L1
                                        ON CUBO.ParLevel1_id = L1.id

                                        AND L1.IsActive = 1


                                    INNER JOIN ParCompanyCluster CCL WITH(NOLOCK)

                                        ON CCL.ParCompany_id = CUBO.ParCompany_id

                                        AND CCL.Active = 1


                                    LEFT JOIN ParCompanyXStructure CS WITH(NOLOCK)

                                        ON CUBO.ParCompany_Id = CS.ParCompany_Id

                                        AND CS.Active = 1


                                    LEFT JOIN ParStructure S WITH(NOLOCK)

                                        ON CS.ParStructure_id = S.id


                                    LEFT JOIN ParStructureGroup SG WITH(NOLOCK)

                                        ON S.ParStructureGroup_Id = SG.ID


                                    WHERE 1 = 1

                                GROUP BY

                                Cubo.ParStructureGroup_Name
			                        ,Cubo.ParStructure_Name
			                        ,Cubo.ParCompany_id
			                        ,Cubo.ParCompany_Name
			                        ,Cubo.Data
			                        ,Cubo.ANO
			                        ,Cubo.MES
			                        ,Cubo.DIA
			                        ,Cubo.ParConsolidationType_Id
			                        ,Cubo.ParLevel1_Id
			                        ,Cubo.ParLevel1_Name
			                        ,Cubo.ParLevel2_id
			                        ,Cubo.ParLevel2_Name
			                        ,Cubo.ParFrequency_Id
			                        ,Cubo.ShiftPlan
			                        ,Cubo.PeriodPlan
                                    --,Cubo.EvaluationPlan
                                    --,Cubo.SamplePlan
                                    --,Cubo.Evaluation
                                    --,Cubo.Sample
                        ) CUBO
                        WHERE 1 = 1
                        --AND ParLevel1_Name = '(%) NC Análise Microbiológica em Produto (Desossa)'
                        --AND ParLevel2_Name = 'Análise Microbiológica em Produto (Desossa)'
                        Group by

                            ParCompany_Name,
	                        ParCompany_id,
	                        ParLevel1_id,
	                        ParLevel1_Name
	                        --ParLevel2_id,
	                        --ParLevel2_Name
                            --ORDER BY 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20

                ";
                using (Factory factory = new Factory("DefaultConnection"))
                {
                    GlobalConfig.MandalaIndicador = factory.SearchQuery<Mandala>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro no metodo PreencherIndicador", ex));
            }
        }

        public static void PreencherListaUnidadeMandala(object stateInfo)
        {
            try
            {
                var query = $@"
                 declare @inicio datetime = DATEADD(DAY,-1,GETDATE()) 
 
                 declare @Fim datetime = DATEADD(DAY,0,GETDATE())

                 declare @horasrabalhadas float = 16
                 declare @horaInicioTrabalho float = 9

                                SELECT 

		                                ParCompany_id,
		                                ParCompany_Name,
		                               -- ParLevel1_id,
		                                --ParLevel1_name,
		                                --ParLevel2_id,
		                                --ParLevel2_Name,
		                                SUM([EvaluationPlan]) AS [Avaliacoes_Planejadas],
						                (SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho) as [Avaliacoes_Planejadas_Momento],
						                CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) as [%_Avaliacoes_Planejadas_Momento],



		                                SUM([Evaluation])     AS [Avaliacoes_Realizadas],
		                                SUM([SamplePlan])	  AS [Amostras_Planejadas],
		                                SUM([Sample])		  AS [Amostras_Realizadas],
		                                ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 AS [Coletado],
		                                CASE  
			                                WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN  0.00000 AND  80.99999 THEN 'red'
			                                WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN 81.00000 AND  99.99999 THEN 'yellow'
			                               -- WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) BETWEEN 81.00000 AND 100.00000 THEN 'green'
			                                WHEN CAST(ISNULL(SUM([Evaluation])/NULLIF((SUM([EvaluationPlan])/@horasrabalhadas) * (DatePart(HOUR,GETDATE()) - @horaInicioTrabalho),0),0)*100 AS DECIMAL(10,2)) > 100 THEN 'green'
		                                END AS [Cor]
                                 FROM (

	                                SELECT 
		                                 Cubo.ParStructureGroup_Name
		                                ,Cubo.ParStructure_Name
		                                ,Cubo.ParCompany_id
		                                ,Cubo.ParCompany_Name
		                                ,Cubo.Data
		                                ,Cubo.ANO as Year
		                                ,Cubo.MES as Month
		                                ,Cubo.DIA as Day
		                                ,Cubo.ParConsolidationType_Id
		                                ,Cubo.ParLevel1_Id
		                                ,Cubo.ParLevel1_Name
		                                ,Cubo.ParLevel2_id
		                                ,Cubo.ParLevel2_Name
		                                ,Cubo.ParFrequency_Id
		                                ,Cubo.ShiftPlan
		                                ,Cubo.PeriodPlan		               
                                        ,MAX(CAST(Cubo.EvaluationPlan AS FLOAT)) AS EvaluationPlan
		                                ,MAX(CAST(Cubo.SamplePlan AS FLOAT)) AS SamplePlan
		                                ,MAX(CAST(Cubo.Evaluation AS FLOAT)) AS Evaluation
		                                ,MAX(CAST(Cubo.Sample AS FLOAT)) AS [Sample]
	
	
	                                 FROM (
	                                SELECT -- TOP 10
	                                (SELECT TOP 1 Name FROM ParStructureGroup WHERE id in (SELECT TOP 1 ParStructureGroup_Id FROM ParStructure WHERE 1=1 AND id in (SELECT ParStructure_Id FROM ParCompanyXStructure CS WHERE CS.ParCompany_id = Levantamento.ParCompany_id))) ParStructureGroup_Name,
	                                (SELECT TOP 1 Name FROM ParStructure WHERE 1=1 AND id in (SELECT ParStructure_Id FROM ParCompanyXStructure CS WHERE CS.ParCompany_id = Levantamento.ParCompany_id)) ParStructure_Name,
	                                ParCompany_id,
	                                (SELECT TOP 1 Name FROM ParCompany C WITH (NOLOCK) WHERE 1=1 AND C.id = Levantamento.ParCompany_id ) AS ParCompany_Name,
	
	                                Data,
	                                YEAR(Data) ANO,
	                                MONTH(Data) MES,
	                                DAY(Data) DIA,
	                                (SELECT TOP 1 ParConsolidationType_Id FROM ParLevel1 L1 WHERE 1=1 AND L1.id = Levantamento.ParLevel1_Id) ParConsolidationType_Id,
	                                ParLevel1_Id,
	                                (SELECT TOP 1 Name FROM ParLevel1 P1 WITH (NOLOCK) WHERE 1=1 AND P1.id = Levantamento.ParLevel1_Id ) AS ParLevel1_Name,
	                                ParLevel2_id,
	                                (SELECT TOP 1 Name FROM ParLevel2 P2 WITH (NOLOCK) WHERE 1=1 AND P2.id = Levantamento.ParLevel2_id ) AS ParLevel2_Name,
	                                ParFrequency_Id,
	                                ShiftPlan,
	                                PeriodPlan,
	                                EvaluationPlan,
	                                SamplePlan,
	                                Evaluation,
	                                Sample
	
	                                 FROM 
			                                (
	
		                                SELECT 
			                                ORCADO.Data,
			                                ISNULL((SELECT Top 1 ParLevel1_Id FROM ParLevel2Level1 P21 WITH (NOLOCK) where IsActive = 1 AND P21.ParLevel2_Id = ORCADO.ParLevel2_Id AND P21.ParCompany_Id = ORCADO.ParCompany_Id)
		                                    ,(SELECT Top 1 ParLevel1_Id FROM ParLevel2Level1 P21 WITH (NOLOCK) where IsActive = 1 AND P21.ParLevel2_Id = ORCADO.ParLevel2_Id AND P21.ParCompany_Id IS NULL)) AS ParLevel1_Id,
			                                ORCADO.ParLevel2_id,
			                                ORCADO.ParCompany_id,
			                                ORCADO.ParFrequency_Id,
			
			                                ORCADO.Shift AS ShiftPlan,
			                                ORCADO.Period AS PeriodPlan,
		
			                                ISNULL(ORCADO.EvaluationPlan,0) AS EvaluationPlan,
			                                ISNULL(ORCADO.SamplePlan,0) AS SamplePlan,
		
			                                ISNULL(REAL.EvaluationNumber,0) AS Evaluation,
			                                ISNULL(REAL.Sample,0) AS Sample 
		
		                                FROM (
		
			                                SELECT 
				                                Data
				                                ,ParLevel2_id
				                                ,ParCompany_id
				                                ,ParFrequency_Id
				                                ,Period
				                                ,Shift
				                                ,EvaluationPlan
				                                ,SamplePlan

                                            FROM(
                                                SELECT

                                                ParLevel2_id
                                                , ParCompany_id
                                                , ParFrequency_Id
                                                , Period
                                                , Shift
                                                , ISNULL((SELECT TOP 1 Number FROM ParEvaluation PE WHERE PE.ParLevel2_Id = CO.ParLevel2_id AND PE.ParCompany_Id = CO.ParCompany_Id and PE.IsActive = 1)
                                                , (SELECT TOP 1 Number FROM ParEvaluation PE WHERE PE.ParLevel2_Id = CO.ParLevel2_id AND PE.ParCompany_Id IS NULL and PE.IsActive = 1)) AS EvaluationPlan
				                                ,ISNULL((SELECT TOP 1 Number FROM ParSample PS WHERE PS.ParLevel2_Id = CO.ParLevel2_id AND PS.ParCompany_Id = CO.ParCompany_Id and PS.IsActive = 1)
				                                ,(SELECT TOP 1 Number FROM ParSample PS WITH(NOLOCK) WHERE PS.ParLevel2_Id = CO.ParLevel2_id AND PS.ParCompany_Id IS NULL and PS.IsActive = 1)) AS SamplePlan

                                                 FROM(
                                                  SELECT

                                                     P2.id ParLevel2_id
                                                    , PC.id ParCompany_id
                                                    , P2.Shift
                                                    , P2.Period

                                                  FROM(
                                                        SELECT id, Shift, Period, ParFrequency_id

                                                            FROM ParLevel2 P2 WITH(NOLOCK)

                                                        CROSS JOIN
                                                            (SELECT 1 Shift UNION ALL SELECT 2) Shift

                                                        CROSS JOIN
                                                            (SELECT 1 Period) Period

                                                        WHERE ParFrequency_id = 1

                                                        UNION ALL

                                                        SELECT id, Shift, Period, ParFrequency_id FROM(
                                                        SELECT id, 1 Period, ParFrequency_id

                                                            FROM ParLevel2 P2

                                                        WHERE ParFrequency_id != 1)a

                                                        CROSS JOIN
                                                            (SELECT 1 Shift UNION ALL SELECT 2) Shift) P2

                                                    CROSS JOIN ParCompany PC WITH(NOLOCK)

                                                    WHERE PC.IsActive = 1
                                                    ) CO
                                                LEFT JOIN ParLevel2 P2 WITH(NOLOCK)

                                                    ON CO.ParLevel2_id = p2.Id


                                                WHERE IsActive = 1
			                                )ORCADO
                                            CROSS JOIN ParCalendar Cal WITH(NOLOCK)
		                                ) ORCADO
                                        LEFT JOIN(
                                                SELECT

                                                    cast(c2.CollectionDate as date) Data,
                                                    unitid ParCompany_id,
                                                    ParLevel2_id,
                                                    Shift,
                                                    Period,
                                                   MAX(CAST(EvaluationNumber AS FLOAT)) EvaluationNumber,
                                                   MAX(CAST(Sample AS FLOAT)) Sample

                                                    FROM CollectionLevel2 c2 WITH(NOLOCK)

                                                    LEFT JOIN ParCalendar Cal WITH(NOLOCK) ON cast(c2.CollectionDate as date) = Cal.Data

                                                    LEFT JOIN ParLevel1 L1 ON c2.ParLevel1_Id = L1.Id

                                                    WHERE L1.IsActive = 1

                                                Group by

                                                    cast(c2.CollectionDate as date),
                                                    ParLevel2_id,
                                                    unitid,
                                                    Shift,
                                                    Period
                                            )REAL


                                        ON ORCADO.Data = REAL.Data

                                         AND ORCADO.ParLevel2_id = REAL.ParLevel2_id

                                         AND ORCADO.ParCompany_id = REAL.ParCompany_id

                                         AND ORCADO.Shift = REAL.Shift

                                         AND(ORCADO.Period = REAL.Period OR ORCADO.Period = 1)

                                        WHERE 1 = 1

                                         AND ORCADO.Data BETWEEN @inicio AND @Fim
		                                 ) Levantamento
                                          WHERE 1 = 1

                                         AND ParLevel1_Id is not null
		                                )CUBO
                                            INNER JOIN ParLevel1 L1
                                                ON CUBO.ParLevel1_id = L1.id

                                                AND L1.IsActive = 1


                                            INNER JOIN ParCompanyCluster CCL WITH(NOLOCK)

                                                ON CCL.ParCompany_id = CUBO.ParCompany_id

                                                AND CCL.Active = 1


                                            LEFT JOIN ParCompanyXStructure CS WITH(NOLOCK)

                                                ON CUBO.ParCompany_Id = CS.ParCompany_Id

                                                AND CS.Active = 1


                                            LEFT JOIN ParStructure S WITH(NOLOCK)

                                                ON CS.ParStructure_id = S.id


                                            LEFT JOIN ParStructureGroup SG WITH(NOLOCK)

                                                ON S.ParStructureGroup_Id = SG.ID


                                            WHERE 1 = 1

                                        GROUP BY

                                        Cubo.ParStructureGroup_Name
			                                ,Cubo.ParStructure_Name
			                                ,Cubo.ParCompany_id
			                                ,Cubo.ParCompany_Name
			                                ,Cubo.Data
			                                ,Cubo.ANO
			                                ,Cubo.MES
			                                ,Cubo.DIA
			                                ,Cubo.ParConsolidationType_Id
			                                ,Cubo.ParLevel1_Id
			                                ,Cubo.ParLevel1_Name
			                                ,Cubo.ParLevel2_id
			                                ,Cubo.ParLevel2_Name
			                                ,Cubo.ParFrequency_Id
			                                ,Cubo.ShiftPlan
			                                ,Cubo.PeriodPlan
                                            --,Cubo.EvaluationPlan
                                            --,Cubo.SamplePlan
                                            --,Cubo.Evaluation
                                            --,Cubo.Sample
                                ) CUBO
                                WHERE 1 = 1
                                --AND ParLevel1_Name = '(%) NC Análise Microbiológica em Produto (Desossa)'
                                --AND ParLevel2_Name = 'Análise Microbiológica em Produto (Desossa)'
                                Group by

                                    ParCompany_Name,
	                                ParCompany_id
	                                --ParLevel1_id,
	                                --ParLevel1_Name,
	                                --ParLevel2_id,
	                                --ParLevel2_Name
                                    --ORDER BY 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20
                ";

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    GlobalConfig.MandalaUnidade = factory.SearchQuery<Mandala>(query).ToList();
                }

            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro no metodo preencher Unidade", ex));
            }
        }
    }
}
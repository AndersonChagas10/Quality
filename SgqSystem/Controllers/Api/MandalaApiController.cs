using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Mandala")]
    public class MandalaApiController : BaseApiController
    {
        private List<JObject> Lista { get; set; }

        [HttpPost]
        [Route("MostrarIndicadorMandala")]
        public List<JObject> MostrarIndicadorMandala([FromBody] int data )
        {
            //FormularioParaRelatorioViewModel form
            var empresas = GetUserUnits(data);

            //string.Join(",", GetUserUnits(form.auditorId));

            var query = $@"
            declare @inicio datetime = '2017-06-15' -- DATEADD(DAY,-1,GETDATE()) 
            declare @Fim datetime = '2017-06-15' -- DATEADD(DAY,0,GETDATE())

            SELECT 

		            ParCompany_id,
		            ParCompany_Name,
		            ParLevel1_id,
		            ParLevel1_name,
		            --ParLevel2_id,
		            --ParLevel2_Name,
		            SUM([EvaluationPlan]) AS [Avaliacoes_Planejadas],
		            SUM([Evaluation])     AS [Avaliacoes_Realizadas],
		            SUM([SamplePlan])	  AS [Amostras_Planejadas],
		            SUM([Sample])		  AS [Amostras_Realizadas],
		            ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 AS [Coletado],
		            CASE 
			            WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN  0.00000 AND  44.99999 THEN 'red'
			            WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN 45.00000 AND  69.99999 THEN 'yellow'
			            WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN 70.00000 AND 100.00000 THEN 'green'
			            WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 > 100 THEN 'green'
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
		            ,MAX(Cubo.EvaluationPlan) AS EvaluationPlan
		            ,MAX(Cubo.SamplePlan) AS SamplePlan
		            ,MAX(Cubo.Evaluation) AS Evaluation
		            ,MAX(Cubo.Sample) AS [Sample]
	
	
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
                                MAX(EvaluationNumber) EvaluationNumber,
                                MAX(Sample) Sample

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
            AND ParCompany_Name = 'Mozarlândia'
            --AND ParLevel1_Name = '(%) NC Análise Microbiológica em Produto (Desossa)'
            --AND ParLevel2_Name = 'Análise Microbiológica em Produto (Desossa)'
            Group by

                ParCompany_Name,
	            ParCompany_id,
	            ParLevel1_id,
	            ParLevel1_Name
                --ParLevel2_id,
	            --ParLevel2_Name,
	            --ORDER BY 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20";

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                Lista = QueryNinja(dbSgq, query);
            }

            return Lista;
        }

        [HttpPost]
        [Route("MostrarMonitoramentoMandala")]
        public List<JObject> MostrarMonitoramentoMandala([FromBody] FormularioParaRelatorioViewModel form)
        {

            var query = $@"
            declare @inicio datetime = '2017-06-15' -- DATEADD(DAY,-1,GETDATE()) 
            declare @Fim datetime = '2017-06-15' -- DATEADD(DAY,0,GETDATE())

            SELECT 

		            ParCompany_id,
		            ParCompany_Name,
		            ParLevel1_id,
		            ParLevel1_name,
		            ParLevel2_id,
		            ParLevel2_Name,
		            SUM([EvaluationPlan]) AS [Avaliacoes_Planejadas],
		            SUM([Evaluation])     AS [Avaliacoes_Realizadas],
		            SUM([SamplePlan])	  AS [Amostras_Planejadas],
		            SUM([Sample])		  AS [Amostras_Realizadas],
		            ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 AS [Coletado],
		            CASE 
			            WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN  0.00000 AND  44.99999 THEN 'red'
			            WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN 45.00000 AND  69.99999 THEN 'yellow'
			            WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN 70.00000 AND 100.00000 THEN 'green'
			            WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 > 100 THEN 'green'
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
		            ,MAX(Cubo.EvaluationPlan) AS EvaluationPlan
		            ,MAX(Cubo.SamplePlan) AS SamplePlan
		            ,MAX(Cubo.Evaluation) AS Evaluation
		            ,MAX(Cubo.Sample) AS [Sample]
	
	
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
                                MAX(EvaluationNumber) EvaluationNumber,
                                MAX(Sample) Sample

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
            AND ParCompany_Name = 'Mozarlândia'
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

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                Lista = QueryNinja(dbSgq, query);
            }

            return Lista;
        }

        [HttpPost]
        [Route("MostrarTarefaMandala")]
        public List<JObject> MostrarTarefaMandala([FromBody] FormularioParaRelatorioViewModel form)
        {

            var query = $@"              
                declare @inicio datetime = '2017-06-15' -- DATEADD(DAY,-1,GETDATE()) 
                declare @Fim datetime = '2017-06-15' -- DATEADD(DAY,0,GETDATE())

                SELECT 

		                ParCompany_id,
		                ParCompany_Name,
		                --ParLevel1_id,
		                --ParLevel1_name,
		                --ParLevel2_id,
		                --ParLevel2_Name,
		                SUM([EvaluationPlan]) AS [Avaliacoes_Planejadas],
		                SUM([Evaluation])     AS [Avaliacoes_Realizadas],
		                SUM([SamplePlan])	  AS [Amostras_Planejadas],
		                SUM([Sample])		  AS [Amostras_Realizadas],
		                ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 AS [Coletado],
		                CASE 
			                WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN  0.00000 AND  44.99999 THEN 'red'
			                WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN 45.00000 AND  69.99999 THEN 'yellow'
			                WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 BETWEEN 70.00000 AND 100.00000 THEN 'green'
			                WHEN ISNULL(SUM([Evaluation])/SUM(NULLIF([EvaluationPlan],0)),0)*100 > 100 THEN 'green'
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
		                ,MAX(Cubo.EvaluationPlan) AS EvaluationPlan
		                ,MAX(Cubo.SamplePlan) AS SamplePlan
		                ,MAX(Cubo.Evaluation) AS Evaluation
		                ,MAX(Cubo.Sample) AS [Sample]
	
	
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

                                        SELECT id, Shift Period, ParFrequency_id FROM(
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
                                    MAX(EvaluationNumber) EvaluationNumber,
                                    MAX(Sample) Sample

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
                AND ParCompany_Name = 'Mozarlândia'
                --AND ParLevel1_Name = '(%) NC Análise Microbiológica em Produto (Desossa)'
                --AND ParLevel2_Name = 'Análise Microbiológica em Produto (Desossa)'
                Group by

                    ParCompany_Name,
	                ParCompany_id
                    --ParLevel1_id,
	                --ParLevel1_Name
                    --ParLevel2_id,
	                --ParLevel2_Name,
	                --ORDER BY 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20";

            using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
            {
                Lista = QueryNinja(dbSgq, query);
            }

            return Lista;
        }

        private string GetUserUnits(int User)
        {
            using (var db = new SgqDbDevEntities())
            {
                return string.Join(",", db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == User).Select(r => r.ParCompany_Id).ToList());
            }
        }
    }
}



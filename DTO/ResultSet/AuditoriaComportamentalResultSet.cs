using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ResultSet
{
    public class AuditoriaComportamentalResultSet
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


        public string GetVisaoGeral()
        {
            return $@"

 

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
                         AND CL2.CollectionDate BETWEEN '20201124 00:00' AND '20201124  23:59:59'
                         AND UnitId in (287,172,203,243,178,284,234,323,73,74,76,82,83,85,86,88,486,317,171,173,193,179,196,281,174,242,239,333,322,318,169,367,316,304,274,328,293,339,334,347,336,257,310,329,335,227,258,271,268,321,307,295,327,104,331,337,94,95,96,28,306,177,302,308,309,319,300,340,167,165,186,166,324,320,299,330,332,325,447,448,449,450,451,452,176,301,314,134,135,89,313,471,109,426,427,439,440,441,442,63,64,65,66,67,68,69,443,444,445,446,245,187,205,379,206,296,285,470,303,42,58,481,159,91,485,37,72,44,93,38,1,114,113,31,36,111,112,492,103,71,90,84,480,75,39,237,275,473,200,238,311,211,270,161,229,277,472,254,265,469,297,350,475,252,189,180,263,160,410,207,215,197,236,474,80,35,279,244,27,29,34,107,315,256,92,283,194,182,183,255,521,13,14,15,16,45,60,61,62,493,494,495,496,497,498,499,500,501,502,503,504,505,506,507,508,509,510,511,512,513,514,515,516,517,518,519,520,170,168,162,228,209,210,459,460,461,462,463,464,465,163,250,269,231,117,118,123,124,131,484,164,272,273,181,343,81,248,139,2,41,119,120,121,122,125,127,482,6,7,8,9,32,77,78,79,251,198,175,249,21,22,23,110,151,152,87,483,5,246,214,224,202,276,132,100,46,47,48,59,221,226,230,213,233,225,232,222,220,223,398,115,409,369,380,372,394,368,382,383,375,376,402,356,378,365,395,381,349,403,384,386,354,352,358,346,385,390,406,396,374,362,407,199,408,401,411,412,413,414,415,416,417,418,419,420,421,422,423,424,425,453,467,468,454,455,262,388,377,364,363,397,366,399,405,404,312,264,191,359,371,357,353,360,373,341,351,266,342,391,370,355,393,361,389,387,392,345,400,348,218,291,116,456,457,458,305,24,25,26,30,488,208,489,133,40,428,429,430,431,432,433,434,435,436,437,438,49,50,490,192,282,217,286,216,190,280,294,267,99,260,126,487,241,204,70,146,147,153,154,155,156,157,158,478,479,219,326,98,476,148,149,150,97,101,102,136,137,138,140,141,142,128,129,130,43,3,4,11,12,10,33,289,253,212,17,18,19,20,201,290,278,298,259,143,144,145,477,261,195,338,105,466,185,235,292,51,52,54,55,56,57,108,491,184,247,344,106,53,188,288,240,1)
                          
                          
                          
                         
 
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
							when CL2HF2.ParFieldType_Id = 12 then US.FullName
							else CL2HF2.Value end)
							FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2 (nolock) 
							left join #collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id
							left join ParHeaderFieldGeral HF (nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id
							inner JOIN ParLevelHeaderField PLHF (NOLOCK) on PLHF.Id = HF.ParLevelHeaderField_Id
							inner join ParLevelDefiniton PLD (NOLOCK) on pld.Id = PLHF.Id 
							left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id
							left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
							left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2
							left join UserSgq US (nolock) on  cast(US.Id as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 12
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
                        ,CASE WHEN (SELECT TOP 1 Id FROM LogTrack LT WHERE LT.Tabela = 'CollectionLevel2XParHeaderFieldGeral' AND LT.Json_Id IN (select CL2PHF_LT.ID from CollectionLevel2XParHeaderFieldGeral CL2PHF_LT where CL2PHF_LT.collectionlevel2_ID = C2.ID)) IS NOT NULL THEN 1 ELSE 0 END AS HasHistoryHeaderField               
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
					   ,US.Name as Auditor
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
        }
    }
}

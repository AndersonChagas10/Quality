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
        public string ValorDescricaoTarefa { get; set; }
        public string ClusterName { get; set; }

        public Nullable<bool> Conforme { get; set; }
        public string _Conforme { get { return Conforme.Value ? Resources.Resource.according : Resources.Resource.not_accordance; } }

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

        public string GetVisaoAcompanhamento()
        {
            return $@"

             DECLARE @DATAINICIAL DATETIME = '2020-11-19'
            DECLARE @DATAFINAL   DATETIME = '2020-11-26'

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
                AND CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                         
 
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
		            CONCAT('{{',STUFF(   
                        (SELECT DISTINCT ', ' + CONCAT('""', HF.name, '"":""', case 

                        when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name

                        when CL2HF2.ParFieldType_Id = 2 then case when HF.Description = 'Produto' then cast(PRD.nCdProduto as varchar(500)) + ' - ' + PRD.cNmProduto else EQP.Nome end

                        when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar, CL2HF2.value, 103)

                        when CL2HF2.ParFieldType_Id = 12 then US.FullName
			            else CL2HF2.Value end, '""')

                        FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2(nolock)

                        left join #collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id

                        left join ParHeaderFieldGeral HF(nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id

                        inner JOIN ParLevelHeaderField PLHF(NOLOCK) on PLHF.Id = HF.ParLevelHeaderField_Id

                        inner join ParLevelDefiniton PLD(NOLOCK) on pld.Id = PLHF.Id

                        left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id

                        left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2

                        left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2

                        left join UserSgq US(nolock) on  cast(US.Id as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 12

                        left join Produto PRD with(nolock) on cast(PRD.nCdProduto as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 2

                        WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id

                        and pld.Id = 1

                        FOR XML PATH('')
                    ),1, 1, ''),'}}')  AS HeaderFieldList1,
		            CONCAT('{{',STUFF(   
			            (SELECT DISTINCT ', ' + CONCAT('""', HF.name, '"":""', case 
			            when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
			            when CL2HF2.ParFieldType_Id = 2 then case when HF.Description = 'Produto' then cast(PRD.nCdProduto as varchar(500)) + ' - ' + PRD.cNmProduto else EQP.Nome end 
			            when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar,  CL2HF2.value, 103)
			            else CL2HF2.Value end , '""')
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
		            ), 1, 1, ''),'}}') AS HeaderFieldList2
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
		            CONCAT('{{',STUFF(   
			            (SELECT DISTINCT ', ' + CONCAT('""', HF.name, '"":""', case 
		            when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
		            when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar,  CL2HF2.value, 103)
		            else CL2HF2.Value end, '""')
		            FROM #Result_Level3XParHeaderFieldGeral CL2HF2 (nolock) 
		            left join #Result_Level3 R3(nolock) on R3.id = CL2HF2.ResultLevel3_Id
		            left join ParHeaderFieldGeral HF (nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id
		            LEFT JOIN ParLevel3 L3 (nolock) on L3.Id = R3.Parlevel3_id
		            left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
		            WHERE CL2HF2.ResultLevel3_Id = CL2HF.ResultLevel3_Id
		            FOR XML PATH('')
		            ), 1, 1, ''),'}}') AS HeaderFieldList
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

            SELECT 
	C2.CollectionDate AS Data--data coleta
	,CAST(YEAR(C2.CollectionDate) AS VARCHAR) + '-' + RIGHT('0'+CAST(DATEPART(WEEK, C2.CollectionDate) AS VARCHAR),2) AS DATAColeta
	,C2.Id as CollectionL2_Id
	,L1.Name AS Indicador--indicador
	,L2.Name AS Monitoramento--monitoramento
	,R3.ParLevel3_Name AS Tarefa--tarefa
	,PC.Name AS ClusterName--cluster
	,US.FullName AS 'Auditor' --Auditor
	,C2.EvaluationNumber AS 'Avaliacao'--AV
	,C2.Sample AS 'Amostra'--AM
	,Centro.Name as CentroCusto--departamento
	,Secao.Name AS Secao--secao
	,UN.Name AS 'Unidade'--unidade
	,PCargo.Name AS Cargo--Cargo
	,pg.Name as regional--regional
	,psg.Name as GrupoEmpresa--grupo de empresa
	,CASE
		WHEN R3.IsNotEvaluate = 1 THEN 'NA'
		WHEN R3.IsConform = 1 THEN 'C'
		WHEN R3.IsConform = 0 THEN 'NC'
	END AS 'Conforme'--Isconform
	,ISNULL(HF.HeaderFieldList1, '{{}}') AS 'HeaderFieldListL1'
	,ISNULL(HF.HeaderFieldList2, '{{}}') AS 'HeaderFieldListL2'
	,ISNULL(HF3.HeaderFieldList, '{{}}') AS 'HeaderFieldListL3'
	,CASE
		WHEN ISNULL(R3.ValueText, '') IN ('undefined', 'null') THEN ''
		ELSE ISNULL(R3.ValueText, '')
	END as 'ValorDescricaoTarefa' --campo texto com a resposta	
	,Count(*) as NUMERODECOLETAS
	into #COLETAS
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
	group by C2.CollectionDate, C2.Id, L1.Name, 
	L2.Name,R3.ParLevel3_Name,PC.Name,US.FullName,
	C2.EvaluationNumber,C2.Sample,Centro.Name,Secao.Name,
	UN.Name,PCargo.Name,pg.Name,psg.Name, R3.IsNotEvaluate,R3.IsConform, 
	HF.HeaderFieldList1,HF.HeaderFieldList2,HF3.HeaderFieldList,R3.ValueText	

----montagem da pivot de semanas--
	Begin

			DECLARE @SQLStr VARCHAR(max)
			SET @SQLStr=''
			SELECT @SQLStr = @SQLStr + '['+ a.DataColeta +'], '
			FROM
			(
				SELECT DISTINCT c2.DataColeta FROM #COLETAS c2
			) a



			SET @SQLStr = LEFT(@SQLStr,len(@SQLStr)-1)


			SET @SQLStr ='SELECT  Indicador, ClusterName,  Unidade, Auditor,Secao,GrupoEmpresa,Monitoramento,Tarefa,Conforme,ValorDescricaoTarefa,Cargo,Regional,HeaderFieldListL1,HeaderFieldListL2,HeaderFieldListL3, '
			+ @SQLStr

			+ ' FROM ( SELECT Indicador, ClusterName, Unidade, Auditor, DataColeta,Secao,GrupoEmpresa,Monitoramento,Tarefa,Conforme,ValorDescricaoTarefa,Cargo,Regional,HeaderFieldListL1,HeaderFieldListL2,HeaderFieldListL3, NUMERODECOLETAS FROM #COLETAS  
				WHERE 1=1 
				GROUP BY DataColeta, Unidade, Auditor, Indicador, ClusterName, NUMERODECOLETAS,Secao,GrupoEmpresa,Monitoramento,Tarefa,Conforme,ValorDescricaoTarefa,Cargo,Regional,HeaderFieldListL1,HeaderFieldListL2,HeaderFieldListL3 '
   
			+ '         ) sq PIVOT (sum(NUMERODECOLETAS) FOR DataColeta IN ('
			+ @SQLStr+')) AS pt ORDER BY 1'

			--PRINT @SQLStr
			EXEC(@SQLStr)

			End
                ----------------

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
                DROP TABLE #COLETAS
            ";
        }

        public string GetVisaoGeral()
        {
                        return $@"

             DECLARE @DATAINICIAL DATETIME = '2020-11-19'
            DECLARE @DATAFINAL   DATETIME = '2020-11-26'

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
                AND CL2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
                         
 
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
		            CONCAT('{{',STUFF(   
                        (SELECT DISTINCT ', ' + CONCAT('""', HF.name, '"":""', case 

                        when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name

                        when CL2HF2.ParFieldType_Id = 2 then case when HF.Description = 'Produto' then cast(PRD.nCdProduto as varchar(500)) + ' - ' + PRD.cNmProduto else EQP.Nome end

                        when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar, CL2HF2.value, 103)

                        when CL2HF2.ParFieldType_Id = 12 then US.FullName
			            else CL2HF2.Value end, '""')

                        FROM #CollectionLevel2XParHeaderFieldGeral CL2HF2(nolock)

                        left join #collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id

                        left join ParHeaderFieldGeral HF(nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id

                        inner JOIN ParLevelHeaderField PLHF(NOLOCK) on PLHF.Id = HF.ParLevelHeaderField_Id

                        inner join ParLevelDefiniton PLD(NOLOCK) on pld.Id = PLHF.Id

                        left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id

                        left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2

                        left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2

                        left join UserSgq US(nolock) on  cast(US.Id as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 12

                        left join Produto PRD with(nolock) on cast(PRD.nCdProduto as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 2

                        WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id

                        and pld.Id = 1

                        FOR XML PATH('')
                    ),1, 1, ''),'}}')  AS HeaderFieldList1,
		            CONCAT('{{',STUFF(   
			            (SELECT DISTINCT ', ' + CONCAT('""', HF.name, '"":""', case 
			            when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
			            when CL2HF2.ParFieldType_Id = 2 then case when HF.Description = 'Produto' then cast(PRD.nCdProduto as varchar(500)) + ' - ' + PRD.cNmProduto else EQP.Nome end 
			            when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar,  CL2HF2.value, 103)
			            else CL2HF2.Value end , '""')
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
		            ), 1, 1, ''),'}}') AS HeaderFieldList2
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
		            CONCAT('{{',STUFF(   
			            (SELECT DISTINCT ', ' + CONCAT('""', HF.name, '"":""', case 
		            when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
		            when CL2HF2.ParFieldType_Id = 6 then CONVERT(varchar,  CL2HF2.value, 103)
		            else CL2HF2.Value end, '""')
		            FROM #Result_Level3XParHeaderFieldGeral CL2HF2 (nolock) 
		            left join #Result_Level3 R3(nolock) on R3.id = CL2HF2.ResultLevel3_Id
		            left join ParHeaderFieldGeral HF (nolock)on CL2HF2.ParHeaderFieldGeral_Id = HF.Id
		            LEFT JOIN ParLevel3 L3 (nolock) on L3.Id = R3.Parlevel3_id
		            left join ParMultipleValuesGeral PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
		            WHERE CL2HF2.ResultLevel3_Id = CL2HF.ResultLevel3_Id
		            FOR XML PATH('')
		            ), 1, 1, ''),'}}') AS HeaderFieldList
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

            SELECT 
	            C2.CollectionDate AS Data--data coleta
                ,C2.Id as CollectionL2_Id
	            ,L1.Name AS Indicador--indicador
	            ,L2.Name AS Monitoramento--monitoramento
	            ,R3.ParLevel3_Name AS Tarefa--tarefa
	            ,PC.Name AS ClusterName--cluster
	            ,US.FullName AS 'Auditor' --Auditor
	            ,C2.EvaluationNumber AS 'Avaliacao'--AV
	            ,C2.Sample AS 'Amostra'--AM
	            ,Centro.Name as CentroCusto--departamento
	            ,Secao.Name AS Secao--secao
	            ,UN.Name AS 'Unidade'--unidade
	            ,PCargo.Name AS Cargo--Cargo
	            ,pg.Name as regional--regional
	            ,psg.Name as GrupoEmpresa--grupo de empresa
	            ,CASE
		            WHEN R3.IsNotEvaluate = 1 THEN 'NA'
		            WHEN R3.IsConform = 1 THEN 'C'
		            WHEN R3.IsConform = 0 THEN 'NC'
	            END AS 'Conforme'--Isconform
	            ,ISNULL(HF.HeaderFieldList1, '{{}}') AS 'HeaderFieldListL1'
	            ,ISNULL(HF.HeaderFieldList2, '{{}}') AS 'HeaderFieldListL2'
	            ,ISNULL(HF3.HeaderFieldList, '{{}}') AS 'HeaderFieldListL3'
	            ,CASE
		            WHEN ISNULL(R3.ValueText, '') IN ('undefined', 'null') THEN ''
		            ELSE ISNULL(R3.ValueText, '')
	            END as 'ValorDescricaoTarefa' --campo texto com a resposta
	            --trazer todos os cabeçalhos separados como colunas individuais
	            --total de avaliações
	            --total de usuarios selecionados no cabeçalho de auditor
	            --total de tarefas Conforme
	            --total de tarefas nao Conforme
	            --total de pessoas que foram selecionadas no cabeçalhos numero de pessoas observadas 

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

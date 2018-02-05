using DTO;
using System;



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

    public Nullable<bool> Conforme { get; set; }
    public string _Conforme { get { return Conforme.Value ? GetResources.getResource("according").Value.ToString() : GetResources.getResource("not_accordance").Value.ToString();} }

    public Nullable<bool> NA { get; set; }
    public string _NA { get { return NA.Value ? GetResources.getResource("unvalued").Value.ToString() : GetResources.getResource("valued").Value.ToString(); } }

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

    public string Select(DataCarrierFormulario form)
    {
        var dtInit = form._dataInicio.ToString("yyyyMMdd");
        var dtF = form._dataFim.ToString("yyyyMMdd");

        var sqlUnidade = "";
        var sqlLevel1 = "";
        var sqlLevel2 = "";
        var sqlLevel3 = "";
        var formatDate = "";

        if (form.unitId > 0)
        {
            sqlUnidade = "\n AND UN.Id = " + form.unitId;
        }

        if (form.level1Id > 0)
        {
            sqlLevel1 = "\n AND L1.Id = " + form.level1Id;
        }

        if (form.level2Id > 0)
        {
            sqlLevel2 = "\n AND C2.ParLevel2_Id = " + form.level2Id;
        }

        if (form.level3Id > 0)
        {
            sqlLevel3 = "\n AND L3.Id = " + form.level3Id;
        }

        if (GlobalConfig.Eua)
        {
            formatDate = "CONVERT(varchar, CAST(CL2HF2.Value AS datetime), 101)";
        }
        else
        {
            formatDate = "CONVERT(varchar, CAST(CL2HF2.Value AS datetime), 103)";
        }

        var query = $@" SELECT                                
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
                 ,case when isnull(R3.ValueText, '') = 'undefined' OR isnull(R3.ValueText, '') = 'null' THEN '' ELSE isnull(R3.ValueText, '') END AS ValueText 
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
					WHEN C2.AlterDate IS NOT NULL THEN 'EDITADO'
					WHEN CAST(C2.AddDate as date) <> CAST(C2.CollectionDate as date) THEN 'RETROATIVO'
				   ELSE 'NORMAL'
				   END
				 as Type
                 FROM CollectionLevel2 C2 (nolock)     
                 INNER JOIN ParCompany UN (nolock)     
                 ON UN.Id = c2.UnitId                  
                 INNER JOIN Result_Level3 R3  (nolock) 
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
                 (SELECT                               
                     CL2HF.CollectionLevel2_Id,        
                     STUFF(   
                        (SELECT DISTINCT ', ' + CONCAT(HF.name, ': ', case 
                        when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 3 then PMV.Name 
                        when CL2HF2.ParFieldType_Id = 2 then case when EQP.Nome is null then cast(PRD.nCdProduto as varchar(500)) + ' - ' + PRD.cNmProduto else EQP.Nome end 
                        when CL2HF2.ParFieldType_Id = 6 then { formatDate }
                        else CL2HF2.Value end)
                        FROM CollectionLevel2XParHeaderField CL2HF2 (nolock) 
                        left join collectionlevel2 CL2(nolock) on CL2.id = CL2HF2.CollectionLevel2_Id
                        left join ParHeaderField HF (nolock)on CL2HF2.ParHeaderField_Id = HF.Id
                        left join ParLevel2 L2(nolock) on L2.Id = CL2.Parlevel2_id
                        left join ParMultipleValues PMV(nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) and CL2HF2.ParFieldType_Id <> 2
                        left join Equipamentos EQP(nolock) on cast(EQP.Id as varchar(500)) = CL2HF2.Value and EQP.ParCompany_Id = CL2.UnitId and CL2HF2.ParFieldType_Id = 2
                        left join Produto PRD with(nolock) on cast(PRD.nCdProduto as varchar(500)) = CL2HF2.Value and CL2HF2.ParFieldType_Id = 2
                        WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id
                        FOR XML PATH('')
                        ), 1, 1, '')  AS HeaderFieldList
                    FROM CollectionLevel2XParHeaderField CL2HF (nolock) 
                    left join collectionlevel2 CL2 (nolock) on CL2.id = CL2HF.CollectionLevel2_Id 
                    left join ParHeaderField HF (nolock) on CL2HF.ParHeaderField_Id = HF.Id 
                    left join ParLevel2 L2 (nolock) on L2.Id = CL2.Parlevel2_id
                    GROUP BY CL2HF.CollectionLevel2_Id
                 	) HF 
                 on c2.Id = HF.CollectionLevel2_Id
                 LEFT JOIN CollectionLevel2XCollectionJson CLCJ
                 ON CLCJ.CollectionLevel2_Id = C2.Id
                 LEFT JOIN CollectionJson CJ
                 ON CJ.Id = CLCJ.CollectionJson_Id
                 WHERE C2.CollectionDate BETWEEN '{ dtInit } 00:00' AND '{ dtF } 23:59'
                {sqlUnidade + sqlLevel1 + sqlLevel2 + sqlLevel3 } ";

        return query;
    }
   
}
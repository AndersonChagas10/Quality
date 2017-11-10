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

        return " SELECT                                    " +
                " \n  C2.CollectionDate AS Data             " +
                " \n ,L1.Name AS Indicador                  " +
                " \n ,L2.Name AS Monitoramento              " +
                " \n ,R3.ParLevel3_Name AS Tarefa           " +
                " \n ,R3.Weight AS Peso                     " +
                " \n ,R3.IntervalMin AS 'IntervaloMinimo'   " +
                " \n ,R3.IntervalMax AS 'IntervaloMaximo'   " +
                " \n ,R3.Value AS 'Lancado'                 " +
                " \n ,R3.IsConform AS 'Conforme'            " +
                " \n ,R3.IsNotEvaluate AS 'NA'              " +
                " \n ,R3.WeiEvaluation AS 'AV_Peso'         " +
                " \n ,R3.WeiDefects AS 'NC_Peso'            " +
                " \n ,case when isnull(R3.ValueText, '') = 'undefined' OR isnull(R3.ValueText, '') = 'null' THEN '' ELSE isnull(R3.ValueText, '') END AS ValueText       " +
                " \n ,C2.EvaluationNumber AS 'Avaliacao'    " +
                " \n ,C2.Sample AS 'Amostra'                " +
                " \n ,ISNULL(C2.Sequential,0) AS 'Sequencial'         " +
                " \n ,ISNULL(C2.Side,0) as 'Banda'                    " +
                " \n ,STR(C2.[Shift]) as 'Turno'            " +
                " \n ,STR(C2.Period) as 'Periodo'           " +
                " \n ,UN.Name AS 'Unidade'                  " +
                " \n ,R3.Id AS 'ResultLevel3Id'             " +
                " \n ,US.Name as 'Auditor'                  " +
                " \n ,ISNULL(L1.hashKey, '') as 'HashKey'   " +
                " \n ,ISNULL(HF.HeaderFieldList, '') as 'HeaderFieldList'  " +
                " \n FROM CollectionLevel2 C2 (nolock)               " +
                " \n INNER JOIN ParCompany UN (nolock)               " +
                " \n ON UN.Id = c2.UnitId                   " +
                " \n INNER JOIN Result_Level3 R3  (nolock)           " +
                " \n ON R3.CollectionLevel2_Id = C2.Id      " +
                " \n INNER JOIN ParLevel3 L3 (nolock)                " +
                " \n ON L3.Id = R3.ParLevel3_Id             " +
                " \n INNER JOIN ParLevel2 L2 (nolock)                " +
                " \n ON L2.Id = C2.ParLevel2_Id             " +
                " \n INNER JOIN ParLevel1 L1 (nolock)                " +
                " \n ON L1.Id = C2.ParLevel1_Id             " +
                " \n INNER JOIN UserSgq US (nolock)                  " +
                " \n ON C2.AuditorId = US.Id                " +
                " \n LEFT JOIN                              " +
                " \n (SELECT                                " +
                " \n     CL2HF.CollectionLevel2_Id,         " +
                " \n     STUFF(                             " +
                " \n            (SELECT DISTINCT ', ' + CONCAT(HF.name, ': ', case when CL2HF2.ParFieldType_Id = 1 or CL2HF2.ParFieldType_Id = 2 or CL2HF2.ParFieldType_Id = 3 then PMV.Name " +
                " \n            when CL2HF2.ParFieldType_Id = 6 then " + formatDate + " " +
                " \n            else CL2HF2.Value end) " +
                " \n            FROM CollectionLevel2XParHeaderField CL2HF2 (nolock) " +
                " \n            left join collectionlevel2 CL2 (nolock) on CL2.id = CL2HF2.CollectionLevel2_Id " +
                " \n            left join ParHeaderField HF (nolock) on CL2HF2.ParHeaderField_Id = HF.Id " +
                " \n            left join ParLevel2 L2 (nolock) on L2.Id = CL2.Parlevel2_id " +
                " \n            left join ParMultipleValues PMV (nolock) on CL2HF2.Value = cast(PMV.Id as varchar(500)) " +
                " \n            WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id " +
                " \n            FOR XML PATH('') " +
                " \n            ), 1, 1, '')  AS HeaderFieldList " +
                " \n    FROM CollectionLevel2XParHeaderField CL2HF (nolock) " +
                " \n    left join collectionlevel2 CL2 (nolock) on CL2.id = CL2HF.CollectionLevel2_Id " +
                " \n    left join ParHeaderField HF (nolock) on CL2HF.ParHeaderField_Id = HF.Id " +
                " \n    left join ParLevel2 L2 (nolock) on L2.Id = CL2.Parlevel2_id " +
                " \n    GROUP BY CL2HF.CollectionLevel2_Id " +
                " \n 	) HF " +
                " \n on c2.Id = HF.CollectionLevel2_Id " +
                " \n WHERE C2.CollectionDate BETWEEN '" + dtInit + " 00:00' AND '" + dtF + " 23:59'" +
                sqlUnidade + sqlLevel1 + sqlLevel2 + sqlLevel3;
    }
   
}
using DTO;
using System;

public class ApontamentosDiariosResultSet
{
    public System.DateTime Data { get; set; }
    public string _Data { get { return Data.ToShortDateString() + " " + Data.ToShortTimeString(); } }

    public string Indicador { get; set; }
    public string Monitoramento { get; set; }
    public string Tarefa { get; set; }
    public Nullable<decimal> Peso { get; set; }
    public string IntervaloMinimo { get; set; }
    public string IntervaloMaximo { get; set; }
    public string Lancado { get; set; }

    public Nullable<bool> Conforme { get; set; }
    public string _Conforme { get { return Conforme.Value ? "Conforme" : "Não Conforme"; } }

    public Nullable<bool> NA { get; set; }
    public string _NA { get { return NA.Value ? "Não Avaliado" : "Avaliado"; } }

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
    public string HeaderFieldList { get; set; }

    public string Select(DataCarrierFormulario form)
    {
        var dtInit = form._dataInicio.ToString("yyyyMMdd");
        var dtF = form._dataFim.ToString("yyyyMMdd");

        var sqlUnidade = "";
        var sqlLevel1 = "";
        var sqlLevel2 = "";
        var sqlLevel3 = "";

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
                " \n ,C2.EvaluationNumber AS 'Avaliacao'    " +
                " \n ,C2.Sample AS 'Amostra'                " +
                " \n ,C2.Sequential AS 'Sequencial'         " +
                " \n ,C2.Side as 'Banda'                    " +
                " \n ,STR(C2.[Shift]) as 'Turno'            " +
                " \n ,STR(C2.Period) as 'Periodo'           " +
                " \n ,UN.Name AS 'Unidade'                  " +
                " \n ,R3.Id AS 'ResultLevel3Id'             " +
                " \n ,US.Name as 'Auditor'                  " +
                " \n ,ISNULL(L1.hashKey, '') as 'HashKey'   " +
                " \n ,ISNULL(HF.HeaderFieldList, '') as 'HeaderFieldList'  " +
                " \n FROM CollectionLevel2 C2               " +
                " \n INNER JOIN ParCompany UN               " +
                " \n ON UN.Id = c2.UnitId                   " +
                " \n INNER JOIN Result_Level3 R3            " +
                " \n ON R3.CollectionLevel2_Id = C2.Id      " +
                " \n INNER JOIN ParLevel3 L3                " +
                " \n ON L3.Id = R3.ParLevel3_Id             " +
                " \n INNER JOIN ParLevel2 L2                " +
                " \n ON L2.Id = C2.ParLevel2_Id             " +
                " \n INNER JOIN ParLevel1 L1                " +
                " \n ON L1.Id = C2.ParLevel1_Id             " +
                " \n INNER JOIN UserSgq US                  " +
                " \n ON C2.AuditorId = US.Id                " +
                " \n LEFT JOIN                              " +
                " \n (SELECT                                " +
                " \n     CL2HF.CollectionLevel2_Id,         " +
                " \n     STUFF(                             " +
                " \n            (SELECT DISTINCT ', ' + CONCAT(HF.name, ': ', CL2HF2.Value) " +
                " \n            FROM CollectionLevel2XParHeaderField CL2HF2 " +
                " \n            left join collectionlevel2 CL2 on CL2.id = CL2HF2.CollectionLevel2_Id " +
                " \n            left join ParHeaderField HF on CL2HF2.ParHeaderField_Id = HF.Id " +
                " \n            left join ParLevel2 L2 on L2.Id = CL2.Parlevel2_id " +
                " \n            WHERE CL2HF2.CollectionLevel2_Id = CL2HF.CollectionLevel2_Id " +
                " \n            FOR XML PATH('') " +
                " \n            ), 1, 1, '')  AS HeaderFieldList " +
                " \n    FROM CollectionLevel2XParHeaderField CL2HF " +
                " \n    left join collectionlevel2 CL2 on CL2.id = CL2HF.CollectionLevel2_Id " +
                " \n    left join ParHeaderField HF on CL2HF.ParHeaderField_Id = HF.Id " +
                " \n    left join ParLevel2 L2 on L2.Id = CL2.Parlevel2_id " +
                " \n    GROUP BY CL2HF.CollectionLevel2_Id " +
                " \n 	) HF " +
                " \n on c2.Id = HF.CollectionLevel2_Id " +
                " \n WHERE C2.CollectionDate BETWEEN '" + dtInit + " 00:00' AND '" + dtF + " 23:59'" +
                sqlUnidade + sqlLevel1 + sqlLevel2 + sqlLevel3;
    }

}
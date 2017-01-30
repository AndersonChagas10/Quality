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
    public Nullable<decimal> _AV_Peso { get { return AV_Peso.HasValue ? AV_Peso.Value : 0M ; } }

    public Nullable<decimal> NC_Peso { get; set; }
    public Nullable<int> Avaliacao { get; set; }
    public int Amostra { get; set; }
    public int ResultLevel3Id { get; set; }
    
    public string Unidade { get; set; }


    public string Select(DataCarrierFormulario form)
    {
        var dtInit = form._dataInicio.ToString("yyyyMMdd");
        var dtF = form._dataFim.ToString("yyyyMMdd");

        var sqlUnidade = "";
        var sqlLevel1 = "";
        var sqlLevel2 = "";

        if(form.unitId > 0)
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
                " \n ,UN.Name AS 'Unidade'                  " +
                " \n ,R3.Id AS 'ResultLevel3Id'             " +
                " \n FROM CollectionLevel2 C2               " +
                " \n LEFT JOIN ParCompany UN                " +
                " \n ON UN.Id = c2.UnitId                   " +
                " \n LEFT JOIN Result_Level3 R3             " +
                " \n ON R3.CollectionLevel2_Id = C2.Id      " +
                " \n LEFT JOIN ParLevel3 L3                 " +
                " \n ON L3.Id = R3.ParLevel3_Id             " +
                " \n LEFT JOIN ParLevel2 L2                 " +
                " \n ON L2.Id = C2.ParLevel2_Id             " +
                " \n LEFT JOIN ParLevel1 L1                 " +
                " \n ON L1.Id = C2.ParLevel1_Id             " +
                " \n WHERE C2.CollectionDate BETWEEN '" + dtInit + " 00:00' AND '" + dtF + " 23:59'" +
                sqlUnidade + sqlLevel1 + sqlLevel2;



    }
}
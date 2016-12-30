using System;

public class ScorecardResultSet
{
    public int? Cluster { get; set; }
    public string ClusterName { get; set; }

    public int? Regional { get; set; }
    public string RegionalName { get; set; }

    public int? ParCompanyId { get; set; }
    public string ParCompanyName { get; set; }

    public int? TipoIndicador { get; set; }
    public string TipoIndicadorName { get; set; }

    public int? Level1Id { get; set; }
    public string Level1Name { get; set; }

    public int? Criterio { get; set; }
    public string CriterioName { get; set; }

    public decimal? AV { get; set; }
    public decimal? NC { get; set; }

    public decimal? Pontos { get; set; }
    public decimal? Meta { get; set; }
    public decimal? Real { get; set; }
    public decimal? PontosAtingidos { get; set; }
    public decimal? Scorecard { get; set; }

    public string SelectScorecard(DateTime dtInicio, DateTime dtFim, int unidadeId)
    {

        return "\n SELECT " +

        "\n   Cluster " +
        "\n , ClusterName " +
        "\n , Regional " +
        "\n , RegionalName " +
        "\n , ParCompanyId " +
        "\n , ParCompanyName " +
        "\n , TipoIndicador " +
        "\n , TipoIndicadorName " +
        "\n , Level1Id " +
        "\n , Level1Name " +
        "\n , Criterio " +
        "\n , CriterioName " +
        "\n , AV " +
        "\n , NC " +
        "\n , Pontos " +
        "\n , Meta " +
        "\n ,ROUND(Real, 2) as Real " +
        "\n ,ROUND(CASE WHEN Scorecard < 70 THEN 0 ELSE (CASE WHEN Scorecard > 100 THEN 100 ELSE Scorecard END /100 ) * Pontos  END , 2) AS PontosAtingidos " +
        "\n ,CASE WHEN Scorecard > 100 THEN 100 ELSE ROUND(Scorecard,2) END AS Scorecard " +

        "\n FROM " +
        "\n ( " +
            "\n SELECT " +


            "\n   Cluster " +
            "\n , ClusterName " +
            "\n , Regional " +
            "\n , RegionalName " +
            "\n , ParCompanyId " +
            "\n , ParCompanyName " +
            "\n , TipoIndicador " +
            "\n , TipoIndicadorName " +
            "\n , Level1Id " +
            "\n , Level1Name " +
            "\n , Criterio " +
            "\n , CriterioName " +
            "\n , AV " +
            "\n , NC " +
            "\n , Pontos " +
            "\n , Meta " +
            "\n , ROUND(Real, 2) as Real " +
            "\n , NULL AS PontosAtingidos " +
            "\n , CASE WHEN TipoIndicador = 1 THEN CASE WHEN Real = 0 THEN 100 ELSE Meta / Real * 100 END WHEN TipoIndicador = 2 THEN Real / Meta * 100 END AS Scorecard " +


            "\n FROM " +

                "\n ( " +
                "\n SELECT " +


                "\n   CL.Id AS Cluster " +
                "\n , CL.Name AS ClusterName " +
                "\n , S.Id AS Regional " +
                "\n , S.Name AS RegionalName " +
                "\n , CL1.UnitId AS ParCompanyId " +
                "\n , C.Name AS ParCompanyName " +
                "\n , CASE WHEN L1.IsRuleConformity = 0 THEN 1 ELSE 2 END             AS TipoIndicador " +
                "\n , CASE WHEN L1.IsRuleConformity = 0 THEN 'Menor' ELSE 'Maior' END AS TipoIndicadorName " +
                "\n , L1.Id AS Level1Id " +
                "\n , L1.Name AS Level1Name " +
                "\n , CRL.Id AS Criterio " +
                "\n , CRL.Name AS CriterioName " +
                "\n , CASE WHEN CT.Id = 1 THEN SUM(CL1.WeiEvaluation) WHEN CT.Id = 3 THEN SUM(CL1.EvaluatedResult) END AS AV " +
                "\n , CASE WHEN CT.Id = 1 THEN SUM(CL1.WeiDefects) WHEN CT.Id = 3 THEN SUM(CL1.DefectsResult) END      AS NC " +
                "\n , L1C.Points AS Pontos " +
                "\n , G.PercentValue AS Meta " +
                "\n , CASE " +

                   "\n  WHEN CT.Id = 1 THEN SUM(CL1.WeiDefects) / SUM(CL1.WeiEvaluation) " +

                   "\n  WHEN CT.Id = 3 THEN " +

                    "\n CASE WHEN SUM(CL1.EvaluatedResult) = 0 THEN 0 ELSE (CAST(SUM(CL1.DefectsResult) AS DECIMAL) / CAST(SUM(CL1.EvaluatedResult) AS DECIMAL)) END " +

                "\n  END * 100 AS Real " +



                "\n FROM ParLevel1 L1 " +

                "\n LEFT JOIN ConsolidationLevel1 CL1 " +

                "\n ON L1.Id = CL1.ParLevel1_Id " +

                "\n LEFT JOIN ParCompany C " +

                "\n ON C.Id = CL1.UnitId " +

                "\n LEFT JOIN ParCompanyXStructure CS " +

                "\n ON CS.ParCompany_Id = C.Id " +

                "\n LEFT JOIN ParStructure S " +

                "\n ON S.Id = CS.ParStructure_Id " +

                "\n LEFT JOIN ParStructureGroup SG " +

                "\n ON SG.Id = S.ParStructureGroup_Id " +

                "\n LEFT JOIN ParCompanyCluster CCL " +

                "\n ON CCL.ParCompany_Id = C.Id " +

                "\n LEFT JOIN ParCluster CL " +

                "\n ON CL.Id = CCL.ParCluster_Id " +

                "\n LEFT JOIN ParConsolidationType CT " +

                "\n ON CT.Id = L1.ParConsolidationType_Id " +

                "\n LEFT JOIN ParLevel1XCluster L1C " +

                "\n ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id " +

                "\n LEFT JOIN ParCriticalLevel CRL " +

                "\n ON L1C.ParCriticalLevel_Id = CRL.Id " +

                "\n LEFT JOIN ParGoal G " +

                "\n ON(G.ParCompany_Id = C.Id OR G.ParCompany_Id IS NULL) AND G.ParLevel1_Id = L1.Id " +

                "\n WHERE(ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59') " +

                "\n AND(C.Id = " + unidadeId + ") " +

                "\n GROUP BY " +

                "\n  CL.Id " +

                "\n ,CL.Name " +

                "\n ,S.Id " +

                "\n ,S.Name " +

                "\n ,CL1.UnitId " +

                "\n ,C.Name " +

                "\n ,L1.IsRuleConformity " +

                "\n ,L1.Id " +

                "\n ,L1.Name " +

                "\n ,CRL.Id " +

                "\n ,CRL.Name " +

                "\n ,L1C.Points " +

                "\n ,G.PercentValue " +

                "\n ,CT.Id " +

            "\n ) AS Score " +
        "\n ) Real " +

            "\n                                                                                                                                                                              " +
            "\n  UNION ALL                                                                                                                                                                   " +
            "\n                                                                                                                                                                              " +
            "\n                                                                                                                                                                              " +
            "\n  SELECT                                                                                                                                                                      " +
            "\n  --*                                                                                                                                                                         " +
            "\n      CL.Id                      AS Cluster                                                                                                                                   " +
            "\n  , CL.Name AS ClusterName                                                                                                                                                    " +
            "\n  , S.Id AS Regional                                                                                                                                                          " +
            "\n  , S.Name AS RegionalName                                                                                                                                                    " +
            "\n  , C.Id AS ParCompanyId                                                                                                                                                      " +
            "\n  , C.Name AS ParCompanyName                                                                                                                                                  " +
            "\n                                                                                                                                                                              " +
            "\n  , CASE WHEN L1.IsRuleConformity = 0 THEN 1 ELSE 2 END AS TipoIndicador                                                                                                      " +
            "\n  , CASE WHEN L1.IsRuleConformity = 0 THEN 'Menor' ELSE 'Maior' END AS TipoIndicadorName                                                                                      " +
            "\n                                                                                                                                                                              " +
            "\n  , L1.Id AS Level1Id                                                                                                                                                         " +
            "\n  , L1.Name AS Level1Name                                                                                                                                                     " +
            "\n                                                                                                                                                                              " +
            "\n  , CRL.Id AS Criterio                                                                                                                                                        " +
            "\n  , CRL.Name AS CriterioName                                                                                                                                                  " +
            "\n                                                                                                                                                                              " +
            "\n  ,0							AS AV                                                                                                                                            " +
            "\n                                                                                                                                                                              " +
            "\n  ,0							AS NC                                                                                                                                            " +
            "\n                                                                                                                                                                              " +
            "\n                                                                                                                                                                              " +
            "\n  , L1C.Points AS Pontos                                                                                                                                                      " +
            "\n  , G.PercentValue AS Meta                                                                                                                                                    " +
            "\n  ,0						    AS Real                                                                                                                                          " +
            "\n                                                                                                                                                                              " +
            "\n  ,0							AS PontosAtingidos                                                                                                                               " +
            "\n  ,0							AS Scorecard                                                                                                                                     " +
            "\n                                                                                                                                                                              " +
            "\n  FROM ParLevel1 L1                                                                                                                                                           " +
            "\n  LEFT JOIN ParCompany C                                                                                                                                                      " +
            "\n  ON C.Id = 1                                                                                                                                                                 " +
            "\n  LEFT JOIN ParCompanyXStructure CS                                                                                                                                           " +
            "\n  ON CS.ParCompany_Id = C.Id                                                                                                                                                  " +
            "\n  LEFT JOIN ParStructure S                                                                                                                                                    " +
            "\n  ON S.Id = CS.ParStructure_Id                                                                                                                                                " +
            "\n  LEFT JOIN ParStructureGroup SG                                                                                                                                              " +
            "\n  ON SG.Id = S.ParStructureGroup_Id                                                                                                                                           " +
            "\n  LEFT JOIN ParCompanyCluster CCL                                                                                                                                             " +
            "\n  ON CCL.ParCompany_Id = C.Id                                                                                                                                                 " +
            "\n  LEFT JOIN ParCluster CL                                                                                                                                                     " +
            "\n  ON CL.Id = CCL.ParCluster_Id                                                                                                                                                " +
            "\n  LEFT JOIN ParConsolidationType CT                                                                                                                                           " +
            "\n  ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                       " +
            "\n  LEFT JOIN ParLevel1XCluster L1C                                                                                                                                             " +
            "\n  ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id                                                                                                                   " +
            "\n  LEFT JOIN ParCriticalLevel CRL                                                                                                                                              " +
            "\n  ON L1C.ParCriticalLevel_Id = CRL.Id                                                                                                                                         " +
            "\n  LEFT JOIN ParGoal G                                                                                                                                                         " +
            "\n  ON (G.ParCompany_Id = C.Id OR G.ParCompany_Id IS NULL) AND G.ParLevel1_Id = L1.Id                                                                                           " +
            "\n  WHERE C.Id = " + unidadeId + "                                                                                                                                              " +
            "\n  AND L1.Id NOT IN (SELECT CCC.ParLevel1_Id FROM ConsolidationLevel1 CCC WHERE CCC.UnitId = 1                                                                                 " +
            "\n  AND CCC.ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59')                                             ";
    }

}
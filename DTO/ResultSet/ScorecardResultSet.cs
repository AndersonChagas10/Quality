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

        return "SELECT " +
            "\n -- *                                                                                                                                                                       " +
            "\n     CL.Id                      AS Cluster                                                                                                                                   " +
            "\n , CL.Name AS ClusterName                                                                                                                                                    " +
            "\n ,S.Id AS Regional                                                                                                                                                           " +
            "\n ,S.Name AS RegionalName                                                                                                                                                     " +
            "\n ,CL1.UnitId AS ParCompanyId                                                                                                                                                 " +
            "\n ,C.Name AS ParCompanyName                                                                                                                                                   " +
            "\n                                                                                                                                                                             " +
            "\n ,CASE WHEN L1.IsRuleConformity = 0 THEN 1 ELSE 2 END AS TipoIndicador                                                                                                       " +
            "\n ,CASE WHEN L1.IsRuleConformity = 0 THEN 'Menor' ELSE 'Maior' END AS TipoIndicadorName                                                                                       " +
            "\n                                                                                                                                                                             " +
            "\n ,L1.Id AS Level1Id                                                                                                                                                          " +
            "\n ,L1.Name AS Level1Name                                                                                                                                                      " +
            "\n                                                                                                                                                                             " +
            "\n ,CRL.Id AS Criterio                                                                                                                                                         " +
            "\n ,CRL.Name AS CriterioName                                                                                                                                                   " +
            "\n                                                                                                                                                                             " +
            "\n ,CASE                                                                                                                                                                       " +
            "\n     WHEN CT.Id = 1 THEN CL1.WeiEvaluation                                                                                                                                   " +
            "\n                                                                                                                                                                             " +
            "\n     WHEN CT.Id = 2 THEN CL1.EvaluatedResult                                                                                                                                 " +
            "\n     END                                        AS AV                                                                                                                        " +
            "\n                                                                                                                                                                             " +
            "\n , CASE                                                                                                                                                                      " +
            "\n     WHEN CT.Id = 1 THEN CL1.WeiDefects                                                                                                                                      " +
            "\n     WHEN CT.Id = 2 THEN CL1.DefectsResult                                                                                                                                   " +
            "\n     END                                        AS NC                                                                                                                        " +
            "\n                                                                                                                                                                             " +
            "\n                                                                                                                                                                             " +
            "\n , L1C.Points AS Pontos                                                                                                                                                      " +
            "\n , G.PercentValue AS Meta                                                                                                                                                    " +
            "\n , CASE                                                                                                                                                                      " +
            "\n     WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation* 100                                                                                                             " +
            "\n                                                                                                                                                                             " +
            "\n     WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult* 100                                                                                                        " +
            "\n     END AS Real                                                                                                                                                             " +
            "\n                                                                                                                                                                             " +
            "\n , CASE                                                                                                                                                                      " +
            "\n         WHEN (                                                                                                                                                              " +
            "\n                                                                                                                                                                             " +
            "\n                                                                                                                                                                             " +
            "\n                 CASE WHEN (                                                                                                                                                 " +
            "\n                                                                                                                                                                             " +
            "\n                 CASE                                                                                                                                                        " +
            "\n                         WHEN L1.IsRuleConformity = 0                                                                                                                        " +
            "\n                                                                                                                                                                             " +
            "\n                         THEN G.PercentValue / ( CASE                                                                                                                        " +
            "\n                                                 WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                      " +
            "\n                                                 WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                 " +
            "\n                                                 END)                                                                                                                        " +
            "\n                                                                                                                                                                             " +
            "\n                         WHEN L1.IsRuleConformity = 1                                                                                                                        " +
            "\n 				        THEN ( CASE                                                                                                                                          " +
            "\n                                 WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                                      " +
            "\n                                 WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                                 " +
            "\n                                 END) / G.PercentValue                                                                                                                       " +
            "\n                 END                                                                                                                                                         " +
            "\n 		        ) > 100 THEN 100 ELSE                                                                                                                                        " +
            "\n                     CASE                                                                                                                                                    " +
            "\n                                                                                                                                                                             " +
            "\n                         WHEN L1.IsRuleConformity = 0                                                                                                                        " +
            "\n 				        THEN G.PercentValue / ( CASE                                                                                                                         " +
            "\n                                                 WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                      " +
            "\n                                                 WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                 " +
            "\n                                                 END)                                                                                                                        " +
            "\n                                                                                                                                                                             " +
            "\n                         WHEN L1.IsRuleConformity = 1                                                                                                                        " +
            "\n 				        THEN ( CASE                                                                                                                                          " +
            "\n                                 WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                                      " +
            "\n                                 WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                                 " +
            "\n                                 END) / G.PercentValue                                                                                                                       " +
            "\n                 END                                                                                                                                                         " +
            "\n                                                                                                                                                                             " +
            "\n                 END                                                                                                                                                         " +
            "\n 		                                                                                                                                                                     " +
            "\n 		    ) < 70 THEN 0                                                                                                                                                    " +
            "\n                                                                                                                                                                             " +
            "\n         ELSE(                                                                                                                                                               " +
            "\n                                                                                                                                                                             " +
            "\n             CASE WHEN (                                                                                                                                                     " +
            "\n                                                                                                                                                                             " +
            "\n                 CASE                                                                                                                                                        " +
            "\n                         WHEN L1.IsRuleConformity = 0                                                                                                                        " +
            "\n                                                                                                                                                                             " +
            "\n                         THEN G.PercentValue / ( CASE                                                                                                                        " +
            "\n                                                 WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                      " +
            "\n                                                 WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                 " +
            "\n                                                 END)                                                                                                                        " +
            "\n                                                                                                                                                                             " +
            "\n                         WHEN L1.IsRuleConformity = 1                                                                                                                        " +
            "\n 				        THEN ( CASE                                                                                                                                          " +
            "\n                                 WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                                      " +
            "\n                                 WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                                 " +
            "\n                                 END) / G.PercentValue                                                                                                                       " +
            "\n                 END                                                                                                                                                         " +
            "\n 		        ) > 100 THEN 100 ELSE                                                                                                                                        " +
            "\n                     CASE                                                                                                                                                    " +
            "\n                                                                                                                                                                             " +
            "\n                         WHEN L1.IsRuleConformity = 0                                                                                                                        " +
            "\n 				        THEN G.PercentValue / ( CASE                                                                                                                         " +
            "\n                                                 WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                      " +
            "\n                                                 WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                 " +
            "\n                                                 END)                                                                                                                        " +
            "\n                                                                                                                                                                             " +
            "\n                         WHEN L1.IsRuleConformity = 1                                                                                                                        " +
            "\n 				        THEN ( CASE                                                                                                                                          " +
            "\n                                 WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                                      " +
            "\n                                 WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                                 " +
            "\n                                 END) / G.PercentValue                                                                                                                       " +
            "\n                 END                                                                                                                                                         " +
            "\n                                                                                                                                                                             " +
            "\n                 END                                                                                                                                                         " +
            "\n                                                                                                                                                                             " +
            "\n         )	/100 * L1C.Points                                                                                                                                                " +
            "\n     END AS PontosAtingidos  --multiplicação dos pontos* o scorecard.Se scorecard < 70 - pontos = 0, senão fica conta.                                                       " +
            "\n , CASE WHEN (                                                                                                                                                               " +
            "\n                                                                                                                                                                             " +
            "\n     CASE                                                                                                                                                                    " +
            "\n             WHEN L1.IsRuleConformity = 0                                                                                                                                    " +
            "\n                                                                                                                                                                             " +
            "\n             THEN G.PercentValue / ( CASE                                                                                                                                    " +
            "\n                                     WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                                  " +
            "\n                                     WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                             " +
            "\n                                     END)                                                                                                                                    " +
            "\n                                                                                                                                                                             " +
            "\n             WHEN L1.IsRuleConformity = 1                                                                                                                                    " +
            "\n 	        THEN ( CASE                                                                                                                                                      " +
            "\n                     WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                                                  " +
            "\n                     WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                                             " +
            "\n                     END) / G.PercentValue                                                                                                                                   " +
            "\n     END                                                                                                                                                                     " +
            "\n     ) > 100 THEN 100 ELSE                                                                                                                                                   " +
            "\n         CASE                                                                                                                                                                " +
            "\n                                                                                                                                                                             " +
            "\n             WHEN L1.IsRuleConformity = 0                                                                                                                                    " +
            "\n 	        THEN G.PercentValue / ( CASE                                                                                                                                     " +
            "\n                                     WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                                  " +
            "\n                                     WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                             " +
            "\n                                     END)                                                                                                                                    " +
            "\n                                                                                                                                                                             " +
            "\n             WHEN L1.IsRuleConformity = 1                                                                                                                                    " +
            "\n 	        THEN ( CASE                                                                                                                                                      " +
            "\n                     WHEN CT.Id = 1 THEN CL1.WeiDefects / CL1.WeiEvaluation                                                                                                  " +
            "\n                     WHEN CT.Id = 2 THEN CL1.EvaluatedResult / CL1.DefectsResult                                                                                             " +
            "\n                     END) / G.PercentValue                                                                                                                                   " +
            "\n     END                                                                                                                                                                     " +
            "\n     END                                                                                                                                                                     " +
            "\n     AS Scorecard  --se for meta de NC, ele sempre será a meta dividido sobre o real, com, no máximo 100%. Se for meta C, real / meta                                        " +
            "\n                                                                                                                                                                             " +
            "\n                                                                                                                                                                             " +
            "\n  FROM ParLevel1 L1                                                                                                                                                           " +
            "\n  LEFT JOIN ConsolidationLevel1 CL1                                                                                                                                           " +
            "\n  ON L1.Id = CL1.ParLevel1_Id                                                                                                                                                 " +
            "\n  LEFT JOIN ParCompany C                                                                                                                                                      " +
            "\n  ON C.Id = CL1.UnitId                                                                                                                                                        " +
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
            "\n  WHERE (ConsolidationDate BETWEEN '20161227 00:00' AND '20161228 00:00')                                                                                                     " +
            "\n  AND(C.Id = 1)                                                                                                                                                               " +
            "\n                                                                                                                                                                             " +
            "\n                                                                                                                                                                             " +
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
            "\n  WHERE C.Id = 1                                                                                                                                                              " +
            "\n  AND L1.Id NOT IN (SELECT CCC.ParLevel1_Id FROM ConsolidationLevel1 CCC WHERE CCC.UnitId = 1 AND CCC.ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 00:00')        ";
    }

}
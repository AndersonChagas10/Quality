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
    public decimal? PontosIndicador { get; set; }
    public decimal? Meta { get; set; }
    public decimal? Real { get; set; }
    public decimal? PontosAtingidos { get; set; }
    public decimal? PontosAtingidosIndicador { get; set; }
    public decimal? Scorecard { get; set; }

    public string TipoScore { get; set; }

    public string getSQLScorecard(DateTime dtInicio, DateTime dtFim, int unidadeId)
    {
        string sql = "";

        sql = "\n SELECT " +

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
        "\n , ROUND(Meta,2) AS Meta" +
        "\n , ROUND(Real,2) as Real " +
        "\n , CASE WHEN Scorecard < 70 THEN 0 ELSE (CASE WHEN Scorecard > 100 THEN 100 ELSE Scorecard END /100 ) * Pontos  END AS PontosAtingidos " +
        "\n , ROUND(CASE WHEN Scorecard > 100 THEN 100 ELSE Scorecard END,2) AS Scorecard " +
        "\n , TipoScore " + 

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
            "\n , CASE WHEN TipoIndicadorName = 'Maior' THEN AV - NC ELSE NC END AS NC " +
            "\n , Pontos " +
            "\n , Meta " +
            "\n , CASE WHEN TipoIndicadorName = 'Maior' THEN AV - NC ELSE NC END / AV * 100 as Real " +
            "\n , NULL AS PontosAtingidos " +
            "\n , CASE WHEN TipoIndicador = 1 THEN CASE WHEN (CASE WHEN TipoIndicadorName = 'Maior' THEN AV - NC ELSE NC END / AV * 100) = 0 THEN 100 ELSE Meta / (CASE WHEN TipoIndicadorName = 'Maior' THEN AV - NC ELSE NC END / AV * 100) * 100 END WHEN TipoIndicador = 2 THEN (CASE WHEN TipoIndicadorName = 'Maior' THEN AV - NC ELSE NC END / AV * 100) / Meta * 100 END AS Scorecard " +
            "\n , TipoScore " +

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
                "\n , CASE WHEN L1.HashKey = 1 THEN " +


                "\n          ((SELECT sum(Amostras) * 2 as AV FROM VolumePcc1b WHERE ParCompany_id = " + unidadeId + " and Data BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59')" +
                "\n          -" +
                "\n          (" +
                "\n              @RESS --SELECT COUNT(1) FROM" +
                "\n              --(" +
                "\n              --SELECT C2.ID, CASE WHEN COUNT(1) = sum(CAST(C3.IsNotEvaluate AS INT)) THEN 'NA' ELSE 'A' END NA FROM CollectionLevel2 C2" +
                "\n              --LEFT JOIN Result_Level3 C3" +
                "\n              --ON C3.CollectionLevel2_Id = C2.Id" +
                "\n              --WHERE C2.CollectionDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59'" +
                "\n              --AND C2.ParLevel1_Id = L1.Id" +
                "\n              --AND C2.UnitId = " + unidadeId + "" +
                "\n              --GROUP BY C2.ID" +
                "\n              --) NA" +
                "\n              --WHERE NA = 'NA'" +
                "\n          )) ELSE" +


                "\n  CASE WHEN CT.Id IN (1,2) THEN SUM(CL1.WeiEvaluation) WHEN CT.Id = 3 THEN SUM(CL1.EvaluatedResult) END END AS AV " +
                "\n , CASE WHEN CT.Id IN (1,2) THEN SUM(CL1.WeiDefects) WHEN CT.Id = 3 THEN SUM(CL1.DefectsResult) END      AS NC " +
                "\n , L1C.Points AS Pontos " +
                
                //"\n     SELECT TOP 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END FROM ParGoal GG WHERE GG.ParLevel1_Id = L1.Id AND (GG.ParCompany_Id = c.ID OR GG.ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC" +

                "\n  , (( " +
                "\n      CASE WHEN(SELECT COUNT(1) FROM ParGoal WHERE AddDate <= '" + dtFim.ToString("yyyyMMdd") + " 23:59' AND ParLevel1_Id = L1.Id) > 0 THEN " +
                "\n          ( " +
                "\n          SELECT top 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END PercentValue " +
                "\n          --, Adddate, ParCompany_Id, ParLevel1_Id " +
                "\n          FROM ParGoal G " +
                "\n          where ParLevel1_Id = L1.Id " +
                "\n          and(ParCompany_Id is null or ParCompany_Id = C.Id) " +
                "\n          and AddDate <= '" + dtFim.ToString("yyyyMMdd") + " 23:59' " +
                "\n          group by ParCompany_Id, ParLevel1_Id, PercentValue, Adddate " +
                "\n          order by Adddate desc " +
                "\n          ) " +
                "\n      ELSE " +
                "\n          ( " +
                "\n            SELECT top 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END PercentValue " +
                "\n            --, Adddate, ParCompany_Id, ParLevel1_Id " +
                "\n            FROM ParGoal G " +
                "\n            where ParLevel1_Id = L1.Id " +
                "\n            and(ParCompany_Id is null or ParCompany_Id = C.Id) " +
                "\n            group by ParCompany_Id, ParLevel1_Id, PercentValue, Adddate " +
                "\n            order by Adddate ASC " +
                "\n           ) " +
                "\n      END " +
                "\n     )) AS META " +

                "\n , CASE WHEN L1.HashKey = 1 THEN " +
                "\n           CAST(SUM(CL1.DefectsResult) AS DECIMAL) / " +
                "\n          ((SELECT sum(Amostras) * 2 as AV FROM VolumePcc1b WHERE ParCompany_id = " + unidadeId + " and Data BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59')" +
                "\n          -" +
                "\n          (" +
                "\n              @RESS --SELECT COUNT(1) FROM" +
                "\n              --(" +
                "\n              --SELECT C2.ID, CASE WHEN COUNT(1) = sum(CAST(C3.IsNotEvaluate AS INT)) THEN 'NA' ELSE 'A' END NA FROM CollectionLevel2 C2" +
                "\n              --LEFT JOIN Result_Level3 C3" +
                "\n              --ON C3.CollectionLevel2_Id = C2.Id" +
                "\n              --WHERE C2.CollectionDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59'" +
                "\n              --AND C2.ParLevel1_Id = L1.Id" +
                "\n              --AND C2.UnitId = " + unidadeId + "" +
                "\n              --GROUP BY C2.ID" +
                "\n              --) NA" +
                "\n              --WHERE NA = 'NA'" +
                "\n          )) ELSE" +

                "\n   CASE " +

                   "\n  WHEN CT.Id IN (1,2) THEN " +

                    "\n  CASE WHEN SUM(CL1.WeiEvaluation) = 0 THEN 0 " +
                    "\n  WHEN SUM(CL1.WeiDefects) = 0 THEN 1 " +
                    "\n  ELSE SUM(CL1.WeiDefects) / SUM(CL1.WeiEvaluation) END " +
                    "\n  WHEN CT.Id = 3 THEN " +
                    "\n  CASE WHEN SUM(CL1.EvaluatedResult) = 0 THEN 0 " +
                    "\n  WHEN SUM(CL1.DefectsResult) = 0 THEN 1 " +
                    "\n  ELSE(CAST(SUM(CL1.DefectsResult) AS DECIMAL) / CAST(SUM(CL1.EvaluatedResult) AS DECIMAL)) END " +
                    "\n  END END * 100 AS Real " +

                
                "\n ,ST.Name AS TipoScore " +

                "\n FROM ParLevel1 L1 " +

                "\n LEFT JOIN ConsolidationLevel1 CL1 " +

                "\n ON L1.Id = CL1.ParLevel1_Id " +

                "\n LEFT JOIN ParScoreType ST " +

                "\n ON ST.Id = L1.ParScoreType_Id " +

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

                "\n --LEFT JOIN (SELECT TOP 1 * FROM ParGoal WHERE ParCompany_Id = " + unidadeId + " OR ParCompany_Id IS NULL ORDER BY ParCompany_Id DESC) G " +

                "\n --ON(G.ParCompany_Id = C.Id OR G.ParCompany_Id IS NULL) AND G.ParLevel1_Id = L1.Id " +

                "\n WHERE(ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59') " +
                "\n --AND(L1.IsActive <> 0) " +
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
                "\n ,ST.Name " +
                "\n --,G.PercentValue " +

                "\n ,CT.Id " +

                "\n ,L1.HashKey " +

                "\n ,C.Id " +

            "\n ) AS Score " +
        "\n ) Real " +


            "\n                                                                                                                                                                              " +
            "\n  UNION ALL                                                                                                                                                                   " +
            "\n                                                                                                                                                                              " +
            "\n                                                                                                                                                                              " +

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
        "\n , ROUND(Meta,2) AS Meta " +
        "\n , ROUND(Real,2) as Real " +
        "\n , CASE WHEN Scorecard < 70 THEN 0 ELSE (CASE WHEN Scorecard > 100 THEN 100 ELSE Scorecard END /100 ) * Pontos  END AS PontosAtingidos " +
        "\n , ROUND(CASE WHEN Scorecard > 100 THEN 100 ELSE Scorecard END,2) AS Scorecard " +
        "\n , TipoScore " +
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
            "\n , CASE WHEN TipoIndicadorName = 'Maior' THEN NC ELSE AV - NC END AS NC " +
            "\n , Pontos " +
            "\n , Meta " +
            "\n , CASE WHEN AV = 0 THEN 0 WHEN AV - NC = 0 THEN 100 ELSE CASE WHEN TipoIndicadorName = 'Maior' THEN NC ELSE AV - NC END / AV * 100 END as Real " +
            "\n , NULL AS PontosAtingidos " +
            "\n , CASE WHEN AV = 0 THEN 0 WHEN AV - NC = 0 THEN 100 ELSE CASE WHEN TipoIndicador = 1 THEN CASE WHEN (CASE WHEN TipoIndicadorName = 'Maior' THEN NC ELSE AV - NC END / AV * 100) = 0 THEN 100 ELSE Meta / (CASE WHEN TipoIndicadorName = 'Maior' THEN NC ELSE AV - NC END / AV * 100) * 100 END WHEN TipoIndicador = 2 THEN (CASE WHEN TipoIndicadorName = 'Maior' THEN NC ELSE AV - NC END / AV * 100) / Meta * 100 END END AS Scorecard " +
            "\n , TipoScore " +

            "\n FROM " +

                "\n ( " +

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
            "\n  , ( SELECT COUNT(PCC.semana) AS AV FROM  " +
            "\n  (  " +
            "\n      SELECT  " +
            "\n      CONVERT(VARCHAR, DATEPART(year, C1.ConsolidationDate)) + '-' + CONVERT(VARCHAR, DATEPART(week, C1.ConsolidationDate)) as Semana  " +
            "\n      , C1.ParLevel1_Id  " +
            "\n      , C1.UnitId AS ParCompany_Id  " +
            "\n      , 1 as Verificado  " +

            "\n      FROM ConsolidationLevel1 C1  " +

            "\n      WHERE C1.ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59'  " +

            "\n      AND C1.ParLevel1_Id = 3  " +
            "\n      AND C1.ParLevel1_Id <> 0 " +
            "\n      AND C1.UnitId = 1  " +

            "\n      GROUP BY CONVERT(VARCHAR, DATEPART(year, C1.ConsolidationDate)) + '-' + CONVERT(VARCHAR, DATEPART(week, C1.ConsolidationDate))  " +
            "\n      , C1.ParLevel1_Id  " +
            "\n      , C1.UnitId  " +
            "\n  ) PCC  " +
            "\n  LEFT JOIN  " +
            "\n  (  " +
            "\n      SELECT  " +

            "\n      CONVERT(VARCHAR, DATEPART(year, C1.ConsolidationDate)) + '-' + CONVERT(VARCHAR, DATEPART(week, C1.ConsolidationDate)) as Semana  " +
            "\n      , C1.ParLevel1_Id  " +
            "\n      , C1.UnitId AS ParCompany_Id  " +
            "\n      , 1 as Verificado  " +

            "\n      FROM ConsolidationLevel1 C1  " +

            "\n      WHERE C1.ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59'  " +

            "\n      AND C1.ParLevel1_Id = 24  " +
            "\n      AND C1.ParLevel1_Id <> 0 " +
            "\n      AND C1.UnitId = 1  " +

            "\n      GROUP BY CONVERT(VARCHAR, DATEPART(year, C1.ConsolidationDate)) + '-' + CONVERT(VARCHAR, DATEPART(week, C1.ConsolidationDate))  " +
            "\n      , C1.ParLevel1_Id  " +
            "\n      , C1.UnitId  " +
            "\n  ) VT  " +
            "\n  ON PCC.Semana = VT.Semana ) AS AV " +

            "\n  , ( SELECT COUNT(vt.semana)AS NC FROM  " +
            "\n  (  " +
            "\n      SELECT  " +
            "\n      CONVERT(VARCHAR, DATEPART(year, C1.ConsolidationDate)) + '-' + CONVERT(VARCHAR, DATEPART(week, C1.ConsolidationDate)) as Semana  " +
            "\n      , C1.ParLevel1_Id  " +
            "\n      , C1.UnitId AS ParCompany_Id  " +
            "\n      , 1 as Verificado  " +

            "\n      FROM ConsolidationLevel1 C1  " +

            "\n      WHERE C1.ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59'  " +

            "\n      AND C1.ParLevel1_Id = 3  " +

            "\n      AND C1.UnitId = 1  " +

            "\n      GROUP BY CONVERT(VARCHAR, DATEPART(year, C1.ConsolidationDate)) + '-' + CONVERT(VARCHAR, DATEPART(week, C1.ConsolidationDate))  " +
            "\n      , C1.ParLevel1_Id  " +
            "\n      , C1.UnitId  " +
            "\n  ) PCC  " +
            "\n  LEFT JOIN  " +
            "\n  (  " +
            "\n      SELECT  " +

            "\n      CONVERT(VARCHAR, DATEPART(year, C1.ConsolidationDate)) + '-' + CONVERT(VARCHAR, DATEPART(week, C1.ConsolidationDate)) as Semana  " +
            "\n      , C1.ParLevel1_Id  " +
            "\n      , C1.UnitId AS ParCompany_Id  " +
            "\n      , 1 as Verificado  " +

            "\n      FROM ConsolidationLevel1 C1  " +

            "\n      WHERE C1.ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59'  " +

            "\n      AND C1.ParLevel1_Id = 24  " +
            "\n      AND C1.ParLevel1_Id <> 0 " +
            "\n      AND C1.UnitId = 1  " +

            "\n      GROUP BY CONVERT(VARCHAR, DATEPART(year, C1.ConsolidationDate)) + '-' + CONVERT(VARCHAR, DATEPART(week, C1.ConsolidationDate))  " +
            "\n      , C1.ParLevel1_Id  " +
            "\n      , C1.UnitId  " +
            "\n  ) VT  " +
            "\n  ON PCC.Semana = VT.Semana ) AS NC " +




            "\n                                                                                                                                                                              " +
            "\n  , L1C.Points AS Pontos                                                                                                                                                      " +
         
            //"\n     SELECT TOP 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END FROM ParGoal GG WHERE GG.ParLevel1_Id = L1.Id AND (GG.ParCompany_Id = c.ID OR GG.ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC" +

            "\n  , (( " +
            "\n      CASE WHEN(SELECT COUNT(1) FROM ParGoal WHERE AddDate <= '" + dtFim.ToString("yyyyMMdd") + " 23:59' AND ParLevel1_Id = L1.Id) > 0 THEN " +
            "\n          ( " +
            "\n          SELECT top 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END PercentValue " +
            "\n          --, Adddate, ParCompany_Id, ParLevel1_Id " +
            "\n          FROM ParGoal G " +
            "\n          where ParLevel1_Id = L1.Id " +
            "\n          and(ParCompany_Id is null or ParCompany_Id = C.Id) " +
            "\n          and AddDate <= '" + dtFim.ToString("yyyyMMdd") + " 23:59' " +
            "\n          group by ParCompany_Id, ParLevel1_Id, PercentValue, Adddate " +
            "\n          order by Adddate desc " +
            "\n          ) " +
            "\n      ELSE " +
            "\n          ( " +
            "\n            SELECT top 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END PercentValue " +
            "\n            --, Adddate, ParCompany_Id, ParLevel1_Id " +
            "\n            FROM ParGoal G " +
            "\n            where ParLevel1_Id = L1.Id " +
            "\n            and(ParCompany_Id is null or ParCompany_Id = C.Id) " +
            "\n            group by ParCompany_Id, ParLevel1_Id, PercentValue, Adddate " +
            "\n            order by Adddate ASC " +
            "\n           ) " +
            "\n      END " +
            "\n     )) AS META " +
            "\n  ,0 AS Real                                                                                                                                          " +
            "\n                                                                                                                                                                              " +
            "\n  ,0	AS PontosAtingidos                                                                                                                               " +
            "\n  ,0	AS Scorecard                                                                                                                                     " +
            "\n  ,ST.Name AS TipoScore                                                                                                                                                                            " +
            "\n  FROM ParLevel1 L1                                                                                                                                                           " +
            
            "\n LEFT JOIN ParScoreType ST " +
            "\n ON ST.Id = L1.ParScoreType_Id " +

            "\n  LEFT JOIN ParCompany C                                                                                                                                                      " +
            "\n  ON C.Id = " + unidadeId + "                                                                                                                                                                 " +
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
            "\n  --LEFT JOIN (SELECT TOP 1 * FROM ParGoal WHERE ParCompany_Id = " + unidadeId + " OR ParCompany_Id IS NULL ORDER BY ParCompany_Id DESC) G                                                                                                                                                         " +
            "\n  --ON (G.ParCompany_Id = C.Id OR G.ParCompany_Id IS NULL) AND G.ParLevel1_Id = L1.Id                                                                                           " +
            "\n  WHERE C.Id = " + unidadeId + "                                                                                                                                              " +
            "\n  AND L1.Id = 25 " +
            "\n  AND L1.IsActive <> 0 " +
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
            "\n  ,0	AS AV                                                                                                                                            " +
            "\n                                                                                                                                                                              " +
            "\n  ,0	AS NC                                                                                                                                            " +
            "\n                                                                                                                                                                              " +
            "\n                                                                                                                                                                              " +
            "\n  , L1C.Points AS Pontos                                                                                                                                                      " +

            //"\n     SELECT TOP 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END FROM ParGoal GG WHERE GG.ParLevel1_Id = L1.Id AND (GG.ParCompany_Id = c.ID OR GG.ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC" +

            "\n  , ROUND( " +
            "\n      CASE WHEN(SELECT COUNT(1) FROM ParGoal WHERE AddDate <= '" + dtFim.ToString("yyyyMMdd") + " 23:59' AND ParLevel1_Id = L1.Id) > 0 THEN " +
            "\n          ( " +
            "\n          SELECT top 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END PercentValue " +
            "\n          --, Adddate, ParCompany_Id, ParLevel1_Id " +
            "\n          FROM ParGoal G " +
            "\n          where ParLevel1_Id = L1.Id " +
            "\n          and(ParCompany_Id is null or ParCompany_Id = C.Id) " +
            "\n          and AddDate <= '" + dtFim.ToString("yyyyMMdd") + " 23:59' " +
            "\n          group by ParCompany_Id, ParLevel1_Id, PercentValue, Adddate " +
            "\n          order by Adddate desc " +
            "\n          ) " +
            "\n      ELSE " +
            "\n          ( " +
            "\n            SELECT top 1 CASE WHEN PercentValue IS NULL THEN 0 ELSE PercentValue END PercentValue " +
            "\n            --, Adddate, ParCompany_Id, ParLevel1_Id " +
            "\n            FROM ParGoal G " +
            "\n            where ParLevel1_Id = L1.Id " +
            "\n            and(ParCompany_Id is null or ParCompany_Id = C.Id) " +
            "\n            group by ParCompany_Id, ParLevel1_Id, PercentValue, Adddate " +
            "\n            order by Adddate ASC " +
            "\n           ) " +
            "\n      END " +
            "\n    ,2) AS META " +

            "\n  ,0 AS Real                                                                                                                                          " +
            "\n                                                                                                                                                                              " +
            "\n  ,0	AS PontosAtingidos                                                                                                                               " +
            "\n  ,0	AS Scorecard                                                                                                                                     " +
            "\n  ,ST.Name AS TipoScore                                                                                                                                                                            " +
            "\n  FROM ParLevel1 L1                                                                                                                                                           " +

            "\n LEFT JOIN ParScoreType ST " +
            "\n ON ST.Id = L1.ParScoreType_Id " +

            "\n  LEFT JOIN ParCompany C                                                                                                                                                      " +
            "\n  ON C.Id = " + unidadeId + "                                                                                                                                                                 " +
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
            "\n  --LEFT JOIN (SELECT TOP 1 * FROM ParGoal WHERE ParCompany_Id = " + unidadeId + " OR ParCompany_Id IS NULL ORDER BY ParCompany_Id DESC) G                                                                                                                                                         " +
            "\n  --ON (G.ParCompany_Id = C.Id OR G.ParCompany_Id IS NULL) AND G.ParLevel1_Id = L1.Id                                                                                           " +
            "\n  WHERE C.Id = " + unidadeId + " " +
            "\n  AND L1.Id <> 25 " +
            "\n  AND L1.AddDate <= '" + dtFim.ToString("yyyyMMdd") + " 23:59'" +
            "\n  AND L1.IsActive <> 0 " +
            "\n  AND L1.Id NOT IN (SELECT CCC.ParLevel1_Id FROM ConsolidationLevel1 CCC WHERE CCC.UnitId = " + unidadeId + "                                                                 " +
            "\n  AND CCC.ConsolidationDate BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59')                                             ";

        return sql;
    }

    public string SelectScorecard(DateTime dtInicio, DateTime dtFim, int unidadeId)
    {

        DateTime _dtIni = dtInicio;
        DateTime _dtFim = dtFim;

        DateTime _novaDataIni = _dtIni;
        DateTime _novaDataFim = _dtIni;

        int numMeses = (12 * (_dtFim.Year - _dtIni.Year) + _dtFim.Month - _dtIni.Month) + 1;

        var sql =

        "\n DECLARE @RESS INT " +

        "\n SELECT " +
        "\n       @RESS =  " +

        "\n         COUNT(1) " +
        "\n         FROM " +
        "\n         ( " +
        "\n         SELECT " +
        "\n         COUNT(1) AS NA " +
        "\n         FROM CollectionLevel2 C2 " +
        "\n         LEFT JOIN Result_Level3 C3 " +
        "\n         ON C3.CollectionLevel2_Id = C2.Id " +
        "\n         WHERE convert(date, C2.CollectionDate) BETWEEN '" + dtInicio.ToString("yyyyMMdd") + " 00:00' AND '" + dtFim.ToString("yyyyMMdd") + " 23:59'" +
        "\n         AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1) " +
        "\n         AND C2.UnitId = " + unidadeId + " " +
        "\n         AND IsNotEvaluate = 1 " +
        "\n         GROUP BY C2.ID " +
        "\n         ) NA " +
        "\n         WHERE NA = 2 " +

     

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
        "\n , SUM(AV) AS AV " +
        "\n , SUM(NC) AS NC " +
        "\n , SUM(Pontos) AS Pontos " +
        "\n , AVG(Pontos) AS PontosIndicador " +
        "\n , Meta " +
        "\n , CASE WHEN SUM(AV) = 0 THEN 0 ELSE ROUND(SUM(NC) / SUM(AV) * 100,2) END Real " +
        "\n , SUM(PontosAtingidos) AS PontosAtingidos " +
        //"\n , CASE WHEN TipoIndicador = 1 THEN (CASE WHEN SUM(AV) = 0 OR Meta = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END AS Scorecard " +

        

        "\n , CASE WHEN " +
        //ini Scorecard 

        "\n ( " +
        "\n CASE WHEN (" +
        "\n  CASE WHEN TipoIndicadorName = 'Maior' THEN CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END " +
        "\n  ELSE CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 0 ELSE Meta / (SUM(NC) / SUM(AV) * 100) * 100 END END  " +
        "\n ) > 100 THEN 100 ELSE " +
        "\n  CASE WHEN TipoIndicadorName = 'Maior' THEN CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END " +
        "\n  ELSE CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 0 ELSE Meta / (SUM(NC) / SUM(AV) * 100) * 100 END END  " +
        "\n END " +
        "\n ) " +

        //fim Scorecard 
        "\n < 70 THEN 0 ELSE (CASE WHEN " +
        //ini Scorecard 

        "\n ( " +
        "\n CASE WHEN (" +
        "\n  CASE WHEN TipoIndicadorName = 'Maior' THEN CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END " +
        "\n  ELSE CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 0 ELSE Meta / (SUM(NC) / SUM(AV) * 100) * 100 END END  " +
        "\n ) > 100 THEN 100 ELSE " +
        "\n  CASE WHEN TipoIndicadorName = 'Maior' THEN CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END " +
        "\n  ELSE CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 0 ELSE Meta / (SUM(NC) / SUM(AV) * 100) * 100 END END  " +
        "\n END " +
        "\n ) " +

        //fim Scorecard 
        "\n > 100 THEN 100 ELSE " +
        //ini Scorecard 

        "\n ( " +
        "\n CASE WHEN (" +
        "\n  CASE WHEN TipoIndicadorName = 'Maior' THEN CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END " +
        "\n  ELSE CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 0 ELSE Meta / (SUM(NC) / SUM(AV) * 100) * 100 END END  " +
        "\n ) > 100 THEN 100 ELSE " +
        "\n  CASE WHEN TipoIndicadorName = 'Maior' THEN CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END " +
        "\n  ELSE CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 0 ELSE Meta / (SUM(NC) / SUM(AV) * 100) * 100 END END  " +
        "\n END " +
        "\n ) " +

        //fim Scorecard  
        "\n END /100 ) * AVG(Pontos)  END AS PontosAtingidosIndicador " +

        "\n , ROUND(CASE WHEN (" +
        "\n  CASE WHEN TipoIndicadorName = 'Maior' THEN CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END " +
        "\n  ELSE CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 0 ELSE Meta / (SUM(NC) / SUM(AV) * 100) * 100 END END  " +
        "\n ) > 100 THEN 100 ELSE " +
        "\n  CASE WHEN TipoIndicadorName = 'Maior' THEN CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 100 ELSE (SUM(NC) / SUM(AV) * 100) / Meta * 100 END " +
        "\n  ELSE CASE WHEN SUM(AV) = 0 OR (SUM(NC) / SUM(AV) * 100) = 0 THEN 0 ELSE Meta / (SUM(NC) / SUM(AV) * 100) * 100 END END  " +
        "\n END, 2) AS Scorecard " +
        "\n , TipoScore " +
        "\n FROM ( \n ";

        for (int i = 0; i < numMeses; i++)
        {

            if (i > 0)
            {
                sql += "\n UNION ALL \n";

                _novaDataIni = new DateTime(_novaDataIni.AddMonths(1).Year, _novaDataIni.AddMonths(1).Month, 1);

            }

            _novaDataFim = new DateTime(_novaDataIni.Year, _novaDataIni.Month, DateTime.DaysInMonth(_novaDataIni.Year, _novaDataIni.Month));

            if(i == numMeses - 1)
            {
                _novaDataFim = _dtFim;
            }
            
            sql += getSQLScorecard(_novaDataIni, _novaDataFim, unidadeId);
        }

        sql += " ) A " +

        "\n GROUP BY " +
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
        "\n , TipoScore " +
        //"\n , AV " +
        //"\n , NC " +
        //"\n , Pontos " +
        "\n , Meta " +
        //"\n , Real " +
        //"\n , PontosAtingidos " +
        //"\n , Scorecard " +
        "";

        
        return sql;

    }

}
using Dominio;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/RelDiario")]
    public class RelatorioDiarioApiController : ApiController
    {
        private PanelResulPanel _mock { get; set; }

        public PanelResulPanel _listGrafico1Level1 { get; set; }

        public RelatorioDiarioApiController()
        {
            _listGrafico1Level1 = new PanelResulPanel();
            CriaMockGrafico1Level1();
            //CriaMockLevel1();
        }

        [HttpPost]
        [Route("Grafico1")]
        public PanelResulPanel GetGrafico1Level1([FromBody] FormularioParaRelatorioViewModel form)
        {
            var queryGrafico1 = "" +
                "\n SELECT " + 
                "\n  level1_Id " +
                "\n ,Level1Name " +
                "\n ,Unidade_Id " +
                "\n ,Unidade " +
                "\n ,ProcentagemNc " +
                "\n ,Meta " +
                "\n ,NC " +
                "\n ,Av " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n     * " +
                "\n     , CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC / AV * 100 END AS ProcentagemNc " +
                "\n     , CASE WHEN CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC / AV * 100 END > META THEN 1 ELSE 0 END RELATORIO_DIARIO " +
                "\n     FROM " +
                "\n     ( " +
                "\n         SELECT " +
                "\n          IND.Id         AS level1_Id " +
                "\n         , IND.Name       AS Level1Name " +
                "\n         , UNI.Id         AS Unidade_Id " +
                "\n         , UNI.Name       AS Unidade " +
                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
                "\n         ELSE 0 " +
                "\n        END AS Av " +
                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult " +
                "\n         ELSE 0 " +

                "\n         END AS NC " +
                "\n         , (SELECT TOP 1 PercentValue FROM ParGoal WHERE ParLevel1_Id = CL1.ParLevel1_Id AND(ParCompany_Id = CL1.UnitId OR ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC) AS Meta " +
                "\n         FROM ConsolidationLevel1 CL1 " +
                "\n         INNER JOIN ParLevel1 IND " +
                "\n         ON IND.Id = CL1.ParLevel1_Id " +
                "\n         INNER JOIN ParCompany UNI " +
                "\n         ON UNI.Id = CL1.UnitId " +
                "\n         WHERE CL1.ConsolidationDate = '20170124' " +
                "\n         AND CL1.UnitId = 1 " +
                "\n     ) S1 " +
                "\n ) S2 " +
                "\n WHERE RELATORIO_DIARIO = 1 " +
                "\n ORDER BY 5 DESC";

            var queryGraficoTendencia = "" +
                "\n SELECT " +
                "\n  level1_Id " +
                "\n ,Level1Name " +
                "\n ,'Tendência do Indicador ' + Level1Name AS Level2Name " +
                "\n ,Unidade_Id " +
                "\n ,Unidade " +
                "\n ,ProcentagemNc " +
                "\n ,Meta " +
                "\n ,NC " +
                "\n ,Av " +
                "\n ,Data AS _Data " +
                "\n FROM " +
                "\n ( " +
                "\n 	SELECT  " +
                "\n 	* " +
                "\n 	,CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC/AV * 100 END AS ProcentagemNc " +
                "\n 	,CASE WHEN CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC/AV * 100 END > META THEN 1 ELSE 0 END RELATORIO_DIARIO " +
                "\n 	FROM " +
                "\n 	( " +
                "\n 		SELECT " +
                "\n 		 IND.Id			AS level1_Id " +
                "\n 		,IND.Name		AS Level1Name " +
                "\n 		,UNI.Id			AS Unidade_Id " +
                "\n 		,UNI.Name		AS Unidade " +
                "\n 		,CASE  " +
                "\n 		WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation " +
                "\n 		WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
                "\n 		WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
                "\n 		ELSE 0 " +
                "\n 		END AS Av " +
                "\n 		,CASE  " +
                "\n 		WHEN IND.ParConsolidationType_Id = 1 THEN WeiDefects " +
                "\n 		WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects " +
                "\n 		WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult " +
                "\n 		ELSE 0 " +
                "\n 		END AS NC " +
                "\n 		, (SELECT TOP 1 PercentValue FROM ParGoal WHERE ParLevel1_Id = CL1.ParLevel1_Id AND (ParCompany_Id = CL1.UnitId OR ParCompany_Id IS NULL) ORDER BY ParCompany_Id DESC ) AS Meta " +
                "\n 		,CL1.ConsolidationDate as Data " +
                "\n 		FROM ConsolidationLevel1 CL1 " +
                "\n 		INNER JOIN ParLevel1 IND " +
                "\n 		ON IND.Id = CL1.ParLevel1_Id " +
                "\n 		INNER JOIN ParCompany UNI " +
                "\n 		ON UNI.Id = CL1.UnitId " +
                "\n 		WHERE CL1.ConsolidationDate = '20170124' " +
                "\n    		AND CL1.UnitId = 1 " +
                 "\n	) S1 " +
                "\n ) S2 " +
                "\n WHERE RELATORIO_DIARIO = 1 ";

            var queryGrafico3 = "" +
                "\n SELECT " +
                "\n  " +
                "\n  level1_Id " +
                "\n ,Level1Name " +
                "\n ,level2_Id " +
                "\n ,Level2Name " +
                "\n ,Unidade_Id " +
                "\n ,Unidade " +
                "\n ,Av " +
                "\n ,NC " +
                "\n FROM " +
                "\n ( " +
                "\n 	SELECT " +
                "\n 	 MON.Id			AS level2_Id " +
                "\n 	,'Level 2 ' + MON.Name		AS Level2Name " +
                "\n 	,IND.Id AS level1_Id " +
                "\n 	,IND.Name AS Level1Name " +
                "\n 	,UNI.Id			AS Unidade_Id " +
                "\n 	,UNI.Name		AS Unidade " +
                "\n 	,CASE  " +
                "\n 	WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation " +
                "\n 	WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n 	WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult " +
                "\n 	ELSE 0 " +
                "\n 	END AS Av " +
                "\n 	,CASE  " +
                "\n 	WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects " +
                "\n 	WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n 	WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult " +
                "\n 	ELSE 0 " +
                "\n 	END AS NC " +
                "\n 	FROM ConsolidationLevel2 CL2 " +
                "\n 	INNER JOIN ConsolidationLevel1 CL1 " +
                "\n 	ON CL1.Id = CL2.ConsolidationLevel1_Id " +
                "\n 	INNER JOIN ParLevel1 IND " +
                "\n 	ON IND.Id = CL1.ParLevel1_Id " +
                "\n 	INNER JOIN ParLevel2 MON " +
                "\n 	ON MON.Id = CL2.ParLevel2_Id " +
                "\n 	INNER JOIN ParCompany UNI " +
                "\n 	ON UNI.Id = CL1.UnitId " +
                "\n 	WHERE CL2.ConsolidationDate = '20170124' " +
                "\n 	AND CL2.UnitId = 1 " +
                "\n 	AND CL1.ParLevel1_Id IN (1, 22, 11) " +
                "\n ) S1 " +
                "\n ORDER BY 8 DESC";

            var queryGrafico4 = "" +
                "\n SELECT " +
                "\n  " +
                "\n  IND.Id AS level1_Id " +
                "\n ,IND.Name AS Level1Name " +
                "\n ,MON.Id AS level2_Id " +
                "\n ,MON.Name AS Level2Name " +
                "\n ,R3.ParLevel3_Id AS level3_Id " +
                "\n ,R3.ParLevel3_Name AS Level3Name " +
                "\n ,UNI.Name AS Unidade " +
                "\n ,UNI.Id AS Unidade_Id " +
                "\n ,SUM(R3.WeiDefects) AS NC " +
                "\n FROM Result_Level3 R3 " +
                "\n INNER JOIN CollectionLevel2 C2 " +
                "\n ON C2.Id = R3.CollectionLevel2_Id " +
                "\n INNER JOIN ConsolidationLevel2 CL2 " +
                "\n ON CL2.Id = C2.ConsolidationLevel2_Id " +
                "\n INNER JOIN ConsolidationLevel1 CL1 " +
                "\n ON CL1.Id = CL2.ConsolidationLevel1_Id " +
                "\n INNER JOIN ParCompany UNI " +
                "\n ON UNI.Id = CL1.UnitId " +
                "\n INNER JOIN ParLevel1 IND  " +
                "\n ON IND.Id = CL1.ParLevel1_Id " +
                "\n INNER JOIN ParLevel2 MON " +
                "\n ON MON.Id = CL2.ParLevel2_Id " +
                "\n WHERE IND.Id IN (1, 22, 11) " +
                "\n /* and MON.Id = 1 */" +
                "\n and UNI.Id = 1 " +
                "\n GROUP BY " +
                "\n  IND.Id " +
                "\n ,IND.Name " +
                "\n ,MON.Id " +
                "\n ,MON.Name " +
                "\n ,R3.ParLevel3_Id " +
                "\n ,R3.ParLevel3_Name " +
                "\n ,UNI.Name " +
                "\n ,UNI.Id " +
                "\n ORDER BY 9 DESC";

            var queryGraficoTarefasAcumuladas = "" +
                "\n SELECT " +
                "\n  " +
                "\n  IND.Id AS level1_Id " +
                "\n ,IND.Name AS Level1Name " +
                "\n ,IND.Id AS level2_Id " +
                "\n ,IND.Name AS Level2Name " +
                "\n ,R3.ParLevel3_Id AS level3_Id " +
                "\n ,R3.ParLevel3_Name AS Level3Name " +
                "\n ,UNI.Name AS Unidade " +
                "\n ,UNI.Id AS Unidade_Id " +
                "\n ,SUM(R3.WeiDefects) AS NC " +
                "\n FROM Result_Level3 R3 " +
                "\n INNER JOIN CollectionLevel2 C2 " +
                "\n ON C2.Id = R3.CollectionLevel2_Id " +
                "\n INNER JOIN ConsolidationLevel2 CL2 " +
                "\n ON CL2.Id = C2.ConsolidationLevel2_Id " +
                "\n INNER JOIN ConsolidationLevel1 CL1 " +
                "\n ON CL1.Id = CL2.ConsolidationLevel1_Id " +
                "\n INNER JOIN ParCompany UNI " +
                "\n ON UNI.Id = CL1.UnitId " +
                "\n INNER JOIN ParLevel1 IND  " +
                "\n ON IND.Id = CL1.ParLevel1_Id " +
                "\n INNER JOIN ParLevel2 MON " +
                "\n ON MON.Id = CL2.ParLevel2_Id " +
                "\n WHERE IND.Id IN (1, 22, 11) " +
                "\n /* and MON.Id = 1 */" +
                "\n and UNI.Id = 1 " +
                "\n GROUP BY " +
                "\n  IND.Id " +
                "\n ,IND.Name " +
                "\n ,R3.ParLevel3_Id " +
                "\n ,R3.ParLevel3_Name " +
                "\n ,UNI.Name " +
                "\n ,UNI.Id " +
                "\n ORDER BY 9 DESC";






            using (var db = new SgqDbDevEntities())
            {
                _listGrafico1Level1.listResultSetLevel1 = new List<RelDiarioResultSet>();
                _listGrafico1Level1.listResultSetLevel1 = db.Database.SqlQuery<RelDiarioResultSet>(queryGrafico1).ToList();
                _listGrafico1Level1.listResultSetTendencia = db.Database.SqlQuery<RelDiarioResultSet>(queryGraficoTendencia).ToList();
                _listGrafico1Level1.listResultSetLevel2 = db.Database.SqlQuery<RelDiarioResultSet>(queryGrafico3).ToList();
                _listGrafico1Level1.listResultSetTarefaPorIndicador = db.Database.SqlQuery<RelDiarioResultSet>(queryGraficoTarefasAcumuladas).ToList();
                _listGrafico1Level1.listResultSetLevel3 = db.Database.SqlQuery<RelDiarioResultSet>(queryGrafico4).ToList();

            }

            return _listGrafico1Level1;
        }

        private void CriaMockGrafico1Level1()
        {
            var firstDayOfLastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(1);

            _mock = new PanelResulPanel();

            _mock.listResultSetLevel1 = new List<RelDiarioResultSet>();
            _mock.listResultSetTendencia = new List<RelDiarioResultSet>();
            _mock.listResultSetLevel2 = new List<RelDiarioResultSet>();
            _mock.listResultSetTarefaPorIndicador = new List<RelDiarioResultSet>();
            _mock.listResultSetLevel3 = new List<RelDiarioResultSet>();

            /*Query 1 para Indicador*/
            _mock.listResultSetLevel1.Add(new RelDiarioResultSet()
            {
                level1_Id = 1,
                Level1Name = "TINGIR",
                Unidade_Id = 1,
                Unidade = "Lins",
                ProcentagemNc = 66M,
                Meta = 5M,
                NC = 2M,
                Av = 3M
            });
            _mock.listResultSetLevel1.Add(new RelDiarioResultSet()
            {
                level1_Id = 2,
                Level1Name = "SECAR",
                Unidade_Id = 1,
                Unidade = "Lins",
                ProcentagemNc = 0M,
                Meta = 10M,
                NC = 0M,
                Av = 3M
            });

            var counter = 0;
            foreach (var i in _mock.listResultSetLevel1)
            {
                /*Query2 Para Tendencia*/
                for (int j = 0; j < 30; j++)
                {
                    if (j == 5)
                    {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            ProcentagemNc = 50M + counter,
                            NC = 4M + counter,
                            Av = 12M + counter,
                            Meta = 5M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            Data = DateTime.UtcNow.AddMonths(-1).AddDays(4).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                        });
                    }
                    else if (j == 16)
                    {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            ProcentagemNc = 76M + counter,
                            NC = 8M + counter,
                            Av = 12M + counter,
                            Meta = 5M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                            //;
                        });
                    }
                    else if (j == 28)
                    {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            ProcentagemNc = 20M + counter,
                            NC = 1M + counter,
                            Av = 12M + counter,
                            Meta = 5M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                        });
                    }
                    //else {
                    //    _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                    //    {
                    //        level1_Id = i.level1_Id,
                    //        Level1Name = i.Level1Name,
                    //        Level2Name = "Tendência Level2 " + counter,
                    //        //ProcentagemNc = 0M + counter,
                    //        //NC = 0M + counter,
                    //        //Av = 0M + counter,
                    //        //Meta = 0M,
                    //        Unidade = i.Unidade,
                    //        Unidade_Id = i.Unidade_Id,
                    //        Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                    //    });
                    //}


                }

                /*Query 3 para Monitoramentos do Indicador*/
                _mock.listResultSetLevel2.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter,
                    Level1Name = i.Level1Name,
                    Level2Name = "Level2 - 1" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });
                _mock.listResultSetLevel2.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter + 1,
                    Level1Name = i.Level1Name,
                    Level2Name = "Level2 - 2" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });


                /*Query 4 para Tarefas Acumuladas do Indicador*/
                _mock.listResultSetTarefaPorIndicador.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter,
                    level3_Id = counter + 12,
                    Level1Name = i.Level1Name,
                    Level2Name = "Tarefas Acumuladas " + counter,
                    Level3Name = "Tarefas Acumuladas Level3 Senhor Amado" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });

                counter += 5;
            }

            counter = 0;
            foreach (var i in _mock.listResultSetLevel1)
            {
                var counter2 = 10;
                foreach (var ii in _mock.listResultSetLevel2.Where(r => r.level1_Id == i.level1_Id))
                {
                    /*Query 5 para Tarefas dos Monitoramentos dos indicadores*/
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2,
                        Level3Name = "Level3 - 1" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2 + 1,
                        Level3Name = "Level3 - 2" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2 + 2,
                        Level3Name = "Level3 - 3" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    counter2 += counter;
                }
            }

        }
    }

    public class RelDiarioResultSet
    {
        public int level1_Id { get; set; }
        public string Level1Name { get; set; }
        public int level2_Id { get; set; }
        public string Level2Name { get; set; }
        public int level3_Id { get; set; }
        public string Level3Name { get; set; }

        public int Unidade_Id { get; set; }
        public string Unidade { get; set; }

        public decimal Meta { get; set; }
        public decimal ProcentagemNc { get; set; }
        public decimal Av { get; set; }
        public decimal Av_Peso { get; set; }
        public decimal NC { get; set; }
        public decimal NC_Peso { get; set; }
        public double Data { get; internal set; }
        public DateTime _Data { get; set; }
    }

    public class PanelResulPanel
    {
        public List<RelDiarioResultSet> listResultSetLevel1 { get; set; }
        public List<RelDiarioResultSet> listResultSetTendencia { get; set; }
        public List<RelDiarioResultSet> listResultSetLevel2 { get; set; }
        public List<RelDiarioResultSet> listResultSetTarefaPorIndicador { get; set; }
        public List<RelDiarioResultSet> listResultSetLevel3 { get; set; }
    }
}


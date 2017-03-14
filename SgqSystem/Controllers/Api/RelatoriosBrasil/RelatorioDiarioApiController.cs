using Dominio;
using DTO.Helpers;
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
        #region Mock
        private PanelResulPanel _mock { get; set; }
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
        #endregion

        private PanelResulPanel _todosOsGraficos { get; set; }

        public RelatorioDiarioApiController()
        {
            _todosOsGraficos = new PanelResulPanel();
            CriaMockGrafico1Level1();
            //CriaMockLevel1();
        }

        [HttpPost]
        [Route("Grafico1")]
        public PanelResulPanel GetGrafico1Level1([FromBody] FormularioParaRelatorioViewModel form)
        {
            using (var db = new SgqDbDevEntities())
            {

                /*Estas 2 primeiras queryes são independentes*/
                var queryIndicadores = PanelResulPanel.QueryIndicadores(form);
                var queryTendencia = PanelResulPanel.QueryGraficoTendencia(form);

                _todosOsGraficos.listResultSetLevel1 = db.Database.SqlQuery<RelDiarioResultSet>(queryIndicadores).ToList();
                _todosOsGraficos.listResultSetTendencia = db.Database.SqlQuery<RelDiarioResultSet>(queryTendencia).ToList();
                if(_todosOsGraficos.listResultSetLevel1.Count() == 0)
                    return _todosOsGraficos;

                var indicadores = _todosOsGraficos._idLevel1QueryIndicadores;
                /*Estas 3 demais queryes dependem do resultado da queryIndicadores*/
                var queryGrafico3 = PanelResulPanel.QueryGrafico3(form, indicadores);
                var queryGraficoTarefasAcumuladas = PanelResulPanel.QueryGraficoTarefasAcumuladas(form, indicadores);
                var queryGrafico4 = PanelResulPanel.QueryGrafico4(form, indicadores);

                _todosOsGraficos.listResultSetLevel2 = db.Database.SqlQuery<RelDiarioResultSet>(queryGrafico3).ToList();
                _todosOsGraficos.listResultSetTarefaPorIndicador = db.Database.SqlQuery<RelDiarioResultSet>(queryGraficoTarefasAcumuladas).ToList();
                _todosOsGraficos.listResultSetLevel3 = db.Database.SqlQuery<RelDiarioResultSet>(queryGrafico4).ToList();
            }

            return _todosOsGraficos;
        }

    }

    public class PanelResulPanel
    {
        public List<RelDiarioResultSet> listResultSetLevel1 { get; set; }
        public List<RelDiarioResultSet> listResultSetTendencia { get; set; }
        public List<RelDiarioResultSet> listResultSetLevel2 { get; set; }
        public List<RelDiarioResultSet> listResultSetTarefaPorIndicador { get; set; }
        public List<RelDiarioResultSet> listResultSetLevel3 { get; set; }

        public string _idLevel1QueryIndicadores
        {
            get
            {
                var filtro = string.Empty;

                if (listResultSetLevel1.IsNotNull())
                    if (listResultSetLevel1.Count() > 0)
                    {
                        listResultSetLevel1.ForEach(r => filtro += r.level1_Id + ",");
                        filtro = filtro.Remove(filtro.Length - 1);
                    }

                return filtro;
            }
        }

        internal static string QueryIndicadores(FormularioParaRelatorioViewModel form)
        {
            var queryGrafico1 = "" +


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
                "\n         WHERE convert(date, C2.CollectionDate) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n         AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1) " +
                "\n         AND C2.UnitId = " + form.unitId + " " +
                "\n         AND IsNotEvaluate = 1 " +
                "\n         GROUP BY C2.ID " +
                "\n         ) NA " +
                "\n         WHERE NA = 2 " +


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

               "\n , CASE WHEN IND.HashKey = 1 THEN " +


                "\n          ((SELECT sum(Amostras) * 2 as AV FROM VolumePcc1b WHERE ParCompany_id = " + form.unitId + " and CONVERT(Date, Data) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "')" +
                "\n          -" +
                "\n          (" +
                "\n              SELECT @RESS --COUNT(1) FROM" +
                "\n              --(" +
                "\n              --SELECT C2.ID, CASE WHEN COUNT(1) = sum(CAST(C3.IsNotEvaluate AS INT)) THEN 'NA' ELSE 'A' END NA FROM CollectionLevel2 C2" +
                "\n              --LEFT JOIN Result_Level3 C3" +
                "\n              --ON C3.CollectionLevel2_Id = C2.Id" +
                "\n              --WHERE CONVERT(Date,C2.CollectionDate) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n              --AND C2.ParLevel1_Id = IND.Id" +
                "\n              --AND C2.UnitId = " + form.unitId + "" +
                "\n              --GROUP BY C2.ID" +
                "\n              --) NA" +
                "\n              --WHERE NA = 'NA'" +
                "\n          )) ELSE" +

               "\n         CASE " +
               "\n         WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation " +
               "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
               "\n         WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
               "\n         ELSE 0 " +
               "\n        END END AS Av " +
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
               "\n         WHERE CL1.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
               "\n         AND CL1.UnitId = " + form.unitId +
               "\n     ) S1 " +
               "\n ) S2 " +
               "\n WHERE RELATORIO_DIARIO = 1 " +
               "\n ORDER BY 5 DESC";

            return queryGrafico1;
        }

        internal static string QueryGraficoTendencia(FormularioParaRelatorioViewModel form)
        {
            var queryGraficoTendencia = "" +

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
                "\n         WHERE convert(date, C2.CollectionDate) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n         AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1) " +
                "\n         AND C2.UnitId = " + form.unitId + " " +
                "\n         AND IsNotEvaluate = 1 " +
                "\n         GROUP BY C2.ID " +
                "\n         ) NA " +
                "\n         WHERE NA = 2 " +

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
                "\n , CASE WHEN IND.HashKey = 1 THEN " +


                "\n          ((SELECT sum(Amostras) * 2 as AV FROM VolumePcc1b WHERE ParCompany_id = " + form.unitId + " and CONVERT(Date, Data) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "')" +
                "\n          -" +
                "\n          (" +
                "\n              SELECT @RESS --COUNT(1) FROM" +
                "\n              --(" +
                "\n              --SELECT C2.ID, CASE WHEN COUNT(1) = sum(CAST(C3.IsNotEvaluate AS INT)) THEN 'NA' ELSE 'A' END NA FROM CollectionLevel2 C2" +
                "\n              --LEFT JOIN Result_Level3 C3" +
                "\n              --ON C3.CollectionLevel2_Id = C2.Id" +
                "\n              --WHERE CONVERT(Date,C2.CollectionDate) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n              --AND C2.ParLevel1_Id = IND.Id" +
                "\n              --AND C2.UnitId = " + form.unitId + "" +
                "\n              --GROUP BY C2.ID" +
                "\n              --) NA" +
                "\n              --WHERE NA = 'NA'" +
                "\n          )) ELSE" +
               "\n 		CASE  " +
               "\n 		WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation " +
               "\n 		WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
               "\n 		WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
               "\n 		ELSE 0 " +
               "\n 		END END AS Av " +
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
               "\n 		WHERE CL1.ConsolidationDate BETWEEN '" + new DateTime(form._dataInicio.AddMonths(-1).Year, form._dataInicio.AddMonths(-1).Month, 1).ToString("yyyyMMdd") + "' AND '" + form._dataFimSQL + "'" +
               "\n    		AND CL1.UnitId = " + form.unitId +
                "\n	) S1 " +
               "\n ) S2 " +
               "\n WHERE RELATORIO_DIARIO = 1 ";

            return queryGraficoTendencia;
        }

        internal static string QueryGrafico3(FormularioParaRelatorioViewModel form, string indicadores)
        {
            var queryGrafico3 = "" +

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
                "\n         WHERE convert(date, C2.CollectionDate) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n         AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1) " +
                "\n         AND C2.UnitId = " + form.unitId + " " +
                "\n         AND IsNotEvaluate = 1 " +
                "\n         GROUP BY C2.ID " +
                "\n         ) NA " +
                "\n         WHERE NA = 2 " +

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
                 "\n , CASE WHEN IND.HashKey = 1 THEN " +


                "\n          ((SELECT sum(Amostras) * 2 as AV FROM VolumePcc1b WHERE ParCompany_id = " + form.unitId + " and CONVERT(Date, Data) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "')" +
                "\n          -" +
                "\n          (" +
                "\n              SELECT @RESS --COUNT(1) FROM" +
                "\n              --(" +
                "\n              --SELECT C2.ID, CASE WHEN COUNT(1) = sum(CAST(C3.IsNotEvaluate AS INT)) THEN 'NA' ELSE 'A' END NA FROM CollectionLevel2 C2" +
                "\n              --LEFT JOIN Result_Level3 C3" +
                "\n              --ON C3.CollectionLevel2_Id = C2.Id" +
                "\n              --WHERE CONVERT(Date,C2.CollectionDate) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n              --AND C2.ParLevel1_Id = IND.Id" +
                "\n              --AND C2.UnitId = " + form.unitId + "" +
                "\n              --GROUP BY C2.ID" +
                "\n              --) NA" +
                "\n              --WHERE NA = 'NA'" +
                "\n          )) ELSE" +
                "\n 	CASE  " +
                "\n 	WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation " +
                "\n 	WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n 	WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult " +
                "\n 	ELSE 0 " +
                "\n 	END END AS Av " +
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
                "\n 	WHERE CL2.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n 	AND CL2.UnitId = " + form.unitId +
                "\n 	--AND CL1.ParLevel1_Id IN (" + indicadores + ") " + //
                "\n ) S1 " +
                "\n ORDER BY 8 DESC";

            return queryGrafico3;
        }

        internal static string QueryGraficoTarefasAcumuladas(FormularioParaRelatorioViewModel form, string indicadores)
        {
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
             "\n WHERE 1 = 1 --IND.Id IN (" + indicadores + ") " +
             "\n /* and MON.Id = 1 */" +
             "\n and UNI.Id = " + form.unitId +
             "\n and CL2.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
             "\n GROUP BY " +
             "\n  IND.Id " +
             "\n ,IND.Name " +
             "\n ,R3.ParLevel3_Id " +
             "\n ,R3.ParLevel3_Name " +
             "\n ,UNI.Name " +
             "\n ,UNI.Id " +
             "\n HAVING SUM(R3.WeiDefects) > 0" +
             "\n ORDER BY 9 DESC";

            return queryGraficoTarefasAcumuladas;
        }

        internal static string QueryGrafico4(FormularioParaRelatorioViewModel form, string indicadores)
        {

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
                "\n WHERE 1 = 1 --IND.Id IN (" + indicadores + ") " + //
                "\n /* and MON.Id = 1 */" +
                "\n and UNI.Id = " + form.unitId +
                "\n and CL2.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n GROUP BY " +
                "\n  IND.Id " +
                "\n ,IND.Name " +
                "\n ,MON.Id " +
                "\n ,MON.Name " +
                "\n ,R3.ParLevel3_Id " +
                "\n ,R3.ParLevel3_Name " +
                "\n ,UNI.Name " +
                "\n ,UNI.Id " +
                "\n HAVING SUM(R3.WeiDefects) > 0" +
                "\n ORDER BY 9 DESC";

            return queryGrafico4;
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

  
}


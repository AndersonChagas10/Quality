using Dominio;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/NaoConformidade")]
    public class NaoConformidadeApiController : ApiController
    {
        private List<NaoConformidadeResultsSet> _mock { get; set; }
        private List<NaoConformidadeResultsSet> _list { get; set; }

        [HttpPost]
        [Route("GraficoUnidades")]
        public List<NaoConformidadeResultsSet> GraficoUnidades([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Relatorio_Nao_Conformidade");

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'                                                                                                                                                                                                                    " +

                 "\n DECLARE @VOLUMEPCC int                                                  " +
                
                "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                "\n UNIDADE INT NULL, " +
                "\n INDICADOR INT NULL, " +
                "\n AM INT NULL, " +
                "\n DEF_AM INT NULL " +
                "\n ) " +
                
                "\n INSERT INTO #AMOSTRATIPO4 " +
                
                "\n SELECT " +
                "\n  UNIDADE, INDICADOR, " +
                "\n COUNT(1) AM " +
                "\n ,SUM(DEF_AM) DEF_AM " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n     cast(C2.CollectionDate as DATE) AS DATA " +
                "\n     , C.Id AS UNIDADE " +
                "\n     , C2.ParLevel1_Id AS INDICADOR " +
                "\n     , C2.EvaluationNumber AS AV " +
                "\n     , C2.Sample AS AM " +
                "\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                "\n     FROM CollectionLevel2 C2 (nolock) " +
                "\n     INNER JOIN ParLevel1 L1 (nolock) " +
                "\n     ON L1.Id = C2.ParLevel1_Id " +

                "\n     INNER JOIN ParCompany C (nolock) " +
                "\n     ON C.Id = C2.UnitId " +
                "\n     where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n     and C2.NotEvaluatedIs = 0 " +
                "\n     and C2.Duplicated = 0 " +
                "\n     and L1.ParConsolidationType_Id = 4 " +
                "\n     group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) " +
                "\n ) TAB " +
                "\n GROUP BY UNIDADE, INDICADOR " +
                

                "\n SELECT " +

                "\n Unidade as UnidadeName" +
                "\n ,CONVERT(varchar(153), Unidade_Id) as Unidade_Id" +
                "\n ,ProcentagemNc as [proc] " +

                "\n ,nc" +
                "\n ,av " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n       Unidade  " +
                "\n     , Unidade_Id " +
                "\n     , sum(avSemPeso) as av " +
                "\n     , sum(ncSemPeso) as nc " +
                "\n     , CASE WHEN sum(AV) IS NULL OR sum(AV) = 0 THEN 0 ELSE sum(NC) / sum(AV) * 100 END AS ProcentagemNc " +

                "\n     FROM " +
                "\n     ( " +
                "\n         SELECT " +
                "\n          IND.Id         AS level1_Id " +
                "\n         , IND.Name       AS Level1Name " +
                "\n         , UNI.Id         AS Unidade_Id " +
                "\n         , UNI.Name       AS Unidade " +
                "\n         , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = UNI.Id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL) " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN WeiEvaluation " +

                "\n         ELSE 0 " +
                "\n        END AS Av " +

                "\n       , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = UNI.Id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL) " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN EvaluateTotal " +


                "\n         ELSE 0 " +
                "\n        END AS AvSemPeso " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN TotalLevel3WithDefects " +

                "\n         ELSE 0 " +

                "\n         END AS NC " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN TotalLevel3WithDefects " +

                "\n         ELSE 0 " +

                "\n         END AS NCSemPeso " +


                               "\n  ,                                                                                                                                                                                                                                                                  " +
               "\n  CASE                                                                                                                                                                                                                                                               " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     ELSE                                                                                                                                                                                                                                                            " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      " +
               "\n  END                                                                                                                                                                                                                                                                " +
               "\n  AS Meta                                                                                                                                                                                                                                                            " +



                "\n         FROM ConsolidationLevel1 CL1 (nolock) " +
                "\n         INNER JOIN ParLevel1 IND (nolock) " +
                "\n         ON IND.Id = CL1.ParLevel1_Id " +
                "\n         INNER JOIN ParCompany UNI  (nolock)" +
                "\n         ON UNI.Id = CL1.UnitId " +
                "\n         INNER JOIN ParCompanyXUserSgq CU(nolock) "+
                "\n         ON CU.UserSgq_Id = " + form.auditorId + " and CU.ParCompany_Id = UNI.Id " + 
                "\n         LEFT JOIN #AMOSTRATIPO4 A4 (nolock) " +
                "\n         ON A4.UNIDADE = UNI.Id " +
                "\n         AND A4.INDICADOR = IND.ID " +
                "\n         WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n         -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) " +

                "\n     ) S1 " +

                "\n     GROUP BY Unidade, Unidade_Id " +

                "\n ) S2 " +
                "\n WHERE nc > 0 " +
                "\n ORDER BY 3 DESC" +
                "\n  DROP TABLE #AMOSTRATIPO4 " ;

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoIndicadorDepartamento")]
        public List<NaoConformidadeResultsSet> GraficoIndicadorDepartamento([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoNcPorUnidadeIndicador();

            //    public string Indicador_Id { get; set; }
            //public string IndicadorName { get; set; }
            //public string Unidade_Id { get; set; }
            //public string UnidadeName { get; set; }
            //public string Monitoramento_Id { get; set; }
            //public string MonitoramentoName { get; set; }
            //public string Tarefa_Id { get; set; }
            //public string TarefaName { get; set; }
            //public decimal Nc { get; set; }
            //public decimal Av { get; set; }
            //public decimal Meta { get; set; }
            //public decimal Proc { get; internal set; }

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'                                                                                                                                                                                                                    " +

                "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.unitName + "'" +

                "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                "\n UNIDADE INT NULL, " +
                "\n INDICADOR INT NULL, " +
                "\n AM INT NULL, " +
                "\n DEF_AM INT NULL " +
                "\n ) " +

                "\n INSERT INTO #AMOSTRATIPO4 " +

                "\n SELECT " +
                "\n  UNIDADE, INDICADOR, " +
                "\n COUNT(1) AM " +
                "\n ,SUM(DEF_AM) DEF_AM " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n     cast(C2.CollectionDate as DATE) AS DATA " +
                "\n     , C.Id AS UNIDADE " +
                "\n     , C2.ParLevel1_Id AS INDICADOR " +
                "\n     , C2.EvaluationNumber AS AV " +
                "\n     , C2.Sample AS AM " +
                "\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                "\n     FROM CollectionLevel2 C2  (nolock)" +
                "\n     INNER JOIN ParLevel1 L1  (nolock)" +
                "\n     ON L1.Id = C2.ParLevel1_Id " +

                "\n     INNER JOIN ParCompany C  (nolock)" +
                "\n     ON C.Id = C2.UnitId " +
                "\n     where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n     and C2.NotEvaluatedIs = 0 " +
                "\n     and C2.Duplicated = 0 " +
                "\n     and L1.ParConsolidationType_Id = 4 " +
                "\n     group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) " +
                "\n ) TAB " +
                "\n GROUP BY UNIDADE, INDICADOR " +

                "\n --------------------------------                                                                                                                     " +
                "\n                                                                                                                                                      " +
                "\n  SELECT TOP 1 @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  DECLARE @NAPCC INT                                                                                                                                  " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  SELECT                                                                                                                                              " +
                "\n         @NAPCC =                                                                                                                                     " +
                "\n           COUNT(1)                                                                                                                                   " +
                "\n           FROM                                                                                                                                       " +
                "\n      (                                                                                                                                               " +
                "\n               SELECT                                                                                                                                 " +
                "\n               COUNT(1) AS NA                                                                                                                         " +
                "\n               FROM CollectionLevel2 C2(nolock)                                                                                                       " +
                "\n               LEFT JOIN Result_Level3 C3(nolock)                                                                                                     " +
                "\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                      " +
                "\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                             " +
                "\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                               " +
                "\n               AND C2.UnitId = @ParCompany_Id                                                                                                         " +
                "\n               AND IsNotEvaluate = 1                                                                                                                  " +
                "\n               GROUP BY C2.ID                                                                                                                         " +
                "\n           ) NA                                                                                                                                       " +
                "\n           WHERE NA = 2                                                                                                                               " +
                "\n  --------------------------------                                                                                                                    " +



                "\n SELECT " +

                "\n  CONVERT(varchar(153), Unidade) as UnidadeName" +
                "\n ,CONVERT(varchar(153), Unidade_Id) as Unidade_Id" +
                "\n ,CONVERT(varchar(153), level1_Id) as Indicador_Id" +
                "\n ,CONVERT(varchar(153), Level1Name) as IndicadorName" +

                
                "\n ,ProcentagemNc as [proc] " +

               "\n ,(case when IsRuleConformity = 1 THEN (100 - META) ELSE Meta END) AS Meta  " +
               "\n ,NC " +
               "\n ,Av " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n       Unidade  " +
                "\n     , IsRuleConformity " +
                "\n     , Unidade_Id " +
                "\n     , Level1Name " +
                "\n     , level1_Id " +

                "\n     , sum(avSemPeso) as av " +
                "\n     , sum(ncSemPeso) as nc " +
                "\n     , CASE WHEN sum(AV) IS NULL OR sum(AV) = 0 THEN 0 ELSE sum(NC) / sum(AV) * 100 END AS ProcentagemNc " +
                "\n     , max(Meta) as Meta" +

                "\n     FROM " +
                "\n     ( " +
                "\n         SELECT " +
                "\n          IND.Id         AS level1_Id " +
                "\n         , IND.IsRuleConformity " +
                "\n         , IND.Name       AS Level1Name " +
                "\n         , UNI.Id         AS Unidade_Id " +
                "\n         , UNI.Name       AS Unidade " +
                "\n         , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation " +


                "\n         ELSE 0 " +
                "\n        END AS Av " +

                "\n       , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal " +
                "\n         ELSE 0 " +
                "\n        END AS AvSemPeso " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NC " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NCSemPeso " +
               "\n  ,                                                                                                                                                                                                                                                                  " +
               "\n  CASE                                                                                                                                                                                                                                                               " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     ELSE                                                                                                                                                                                                                                                            " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      " +
               "\n  END                                                                                                                                                                                                                                                                " +
               "\n  AS Meta                                                                                                                                                                                                                                                            " +
                "\n         FROM ConsolidationLevel1 CL1  (nolock)" +
                "\n         INNER JOIN ParLevel1 IND  (nolock)" +
                "\n         ON IND.Id = CL1.ParLevel1_Id " +
                
                "\n         INNER JOIN ConsolidationLevel2 CL2 with (nolock) " +
                "\n         ON CL2.ConsolidationLevel1_id = CL1.Id " +
                "\n         INNER JOIN ParLevel2 L2 with (nolock) " +
                "\n         ON CL2.ParLevel2_id = L2.Id " +
                "\n         INNER JOIN ParDepartment D with (nolock) " +
                "\n         ON L2.ParDepartment_Id = D.Id " +


                "\n         INNER JOIN ParCompany UNI  (nolock)" +
                "\n         ON UNI.Id = CL1.UnitId " +
                "\n         LEFT JOIN #AMOSTRATIPO4 A4  (nolock)" +
                "\n         ON A4.UNIDADE = UNI.Id " +
                "\n         AND A4.INDICADOR = IND.ID " +
                "\n         WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n         AND UNI.Name = '" + form.unitName + "'" +
                "\n         -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) " +

                "\n         AND D.Id = 2 " +

                "\n     ) S1 " +

                "\n     GROUP BY Unidade, Unidade_Id, Level1Name, level1_Id, IsRuleConformity  " +

                "\n ) S2 " +
                "\n WHERE nc > 0 " +
                "\n ORDER BY 5 DESC" +

                "\n  DROP TABLE #AMOSTRATIPO4 ";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoIndicador")]
        public List<NaoConformidadeResultsSet> GraficoIndicador([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoNcPorUnidadeIndicador();

            //    public string Indicador_Id { get; set; }
            //public string IndicadorName { get; set; }
            //public string Unidade_Id { get; set; }
            //public string UnidadeName { get; set; }
            //public string Monitoramento_Id { get; set; }
            //public string MonitoramentoName { get; set; }
            //public string Tarefa_Id { get; set; }
            //public string TarefaName { get; set; }
            //public decimal Nc { get; set; }
            //public decimal Av { get; set; }
            //public decimal Meta { get; set; }
            //public decimal Proc { get; internal set; }

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'                                                                                                                                                                                                                    " +

                "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.unitName + "'" +

                "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                "\n UNIDADE INT NULL, " +
                "\n INDICADOR INT NULL, " +
                "\n AM INT NULL, " +
                "\n DEF_AM INT NULL " +
                "\n ) " +

                "\n INSERT INTO #AMOSTRATIPO4 " +

                "\n SELECT " +
                "\n  UNIDADE, INDICADOR, " +
                "\n COUNT(1) AM " +
                "\n ,SUM(DEF_AM) DEF_AM " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n     cast(C2.CollectionDate as DATE) AS DATA " +
                "\n     , C.Id AS UNIDADE " +
                "\n     , C2.ParLevel1_Id AS INDICADOR " +
                "\n     , C2.EvaluationNumber AS AV " +
                "\n     , C2.Sample AS AM " +
                "\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                "\n     FROM CollectionLevel2 C2  (nolock)" +
                "\n     INNER JOIN ParLevel1 L1  (nolock)" +
                "\n     ON L1.Id = C2.ParLevel1_Id " +

                "\n     INNER JOIN ParCompany C  (nolock)" +
                "\n     ON C.Id = C2.UnitId " +
                "\n     where cast(C2.CollectionDate as DATE) BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n     and C2.NotEvaluatedIs = 0 " +
                "\n     and C2.Duplicated = 0 " +
                "\n     and L1.ParConsolidationType_Id = 4 " +
                "\n     group by C.Id, ParLevel1_Id, EvaluationNumber, Sample, cast(CollectionDate as DATE) " +
                "\n ) TAB " +
                "\n GROUP BY UNIDADE, INDICADOR " +

                "\n --------------------------------                                                                                                                     " +
                "\n                                                                                                                                                      " +
                "\n  SELECT TOP 1 @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  DECLARE @NAPCC INT                                                                                                                                  " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  SELECT                                                                                                                                              " +
                "\n         @NAPCC =                                                                                                                                     " +
                "\n           COUNT(1)                                                                                                                                   " +
                "\n           FROM                                                                                                                                       " +
                "\n      (                                                                                                                                               " +
                "\n               SELECT                                                                                                                                 " +
                "\n               COUNT(1) AS NA                                                                                                                         " +
                "\n               FROM CollectionLevel2 C2(nolock)                                                                                                       " +
                "\n               LEFT JOIN Result_Level3 C3(nolock)                                                                                                     " +
                "\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                      " +
                "\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                             " +
                "\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                               " +
                "\n               AND C2.UnitId = @ParCompany_Id                                                                                                         " +
                "\n               AND IsNotEvaluate = 1                                                                                                                  " +
                "\n               GROUP BY C2.ID                                                                                                                         " +
                "\n           ) NA                                                                                                                                       " +
                "\n           WHERE NA = 2                                                                                                                               " +
                "\n  --------------------------------                                                                                                                    " +



                "\n SELECT " +

                "\n  CONVERT(varchar(153), Unidade) as UnidadeName" +
                "\n ,CONVERT(varchar(153), Unidade_Id) as Unidade_Id" +
                "\n ,CONVERT(varchar(153), level1_Id) as Indicador_Id" +
                "\n ,CONVERT(varchar(153), Level1Name) as IndicadorName" +


                "\n ,ProcentagemNc as [proc] " +

               "\n ,(case when IsRuleConformity = 1 THEN (100 - META) ELSE Meta END) AS Meta  " +
               "\n ,NC " +
               "\n ,Av " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n       Unidade  " +
                "\n     , IsRuleConformity " +
                "\n     , Unidade_Id " +
                "\n     , Level1Name " +
                "\n     , level1_Id " +

                "\n     , sum(avSemPeso) as av " +
                "\n     , sum(ncSemPeso) as nc " +
                "\n     , CASE WHEN sum(AV) IS NULL OR sum(AV) = 0 THEN 0 ELSE sum(NC) / sum(AV) * 100 END AS ProcentagemNc " +
                "\n     , max(Meta) as Meta" +

                "\n     FROM " +
                "\n     ( " +
                "\n         SELECT " +
                "\n          IND.Id         AS level1_Id " +
                "\n         , IND.IsRuleConformity " +
                "\n         , IND.Name       AS Level1Name " +
                "\n         , UNI.Id         AS Unidade_Id " +
                "\n         , UNI.Name       AS Unidade " +
                "\n         , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN WeiEvaluation " +


                "\n         ELSE 0 " +
                "\n        END AS Av " +

                "\n       , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN EvaluateTotal " +
                "\n         ELSE 0 " +
                "\n        END AS AvSemPeso " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NC " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 3 THEN DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 4 THEN A4.DEF_AM" +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NCSemPeso " +
               "\n  ,                                                                                                                                                                                                                                                                  " +
               "\n  CASE                                                                                                                                                                                                                                                               " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) AND G.AddDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, AddDate DESC)                                         " +
               "\n                                                                                                                                                                                                                                                                     " +
               "\n     ELSE                                                                                                                                                                                                                                                            " +
               "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G WHERE G.ParLevel1_id = CL1.ParLevel1_Id AND(G.ParCompany_id = CL1.UnitId OR G.ParCompany_id IS NULL) ORDER BY G.ParCompany_Id DESC, AddDate ASC)                                                                      " +
               "\n  END                                                                                                                                                                                                                                                                " +
               "\n  AS Meta                                                                                                                                                                                                                                                            " +
                "\n         FROM ConsolidationLevel1 CL1  (nolock)" +
                "\n         INNER JOIN ParLevel1 IND  (nolock)" +
                "\n         ON IND.Id = CL1.ParLevel1_Id " +
                "\n         INNER JOIN ParCompany UNI  (nolock)" +
                "\n         ON UNI.Id = CL1.UnitId " +
                "\n         LEFT JOIN #AMOSTRATIPO4 A4  (nolock)" +
                "\n         ON A4.UNIDADE = UNI.Id " +
                "\n         AND A4.INDICADOR = IND.ID " +
                "\n         WHERE CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n         AND UNI.Name = '" + form.unitName + "'" +
                "\n         -- AND (TotalLevel3WithDefects > 0 AND TotalLevel3WithDefects IS NOT NULL) " +

                "\n     ) S1 " +

                "\n     GROUP BY Unidade, Unidade_Id, Level1Name, level1_Id, IsRuleConformity  " +

                "\n ) S2 " +
                "\n WHERE nc > 0 " +
                "\n ORDER BY 5 DESC" +

                "\n  DROP TABLE #AMOSTRATIPO4 ";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoMonitoramento")]
        public List<NaoConformidadeResultsSet> GraficoMonitoramento([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoMonitoramento();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +

                "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.unitName + "'" +

                "\n --------------------------------                                                                                                                     " +
                "\n                                                                                                                                                      " +
                "\n  SELECT TOP 1 @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  DECLARE @NAPCC INT                                                                                                                                  " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  SELECT                                                                                                                                              " +
                "\n         @NAPCC =                                                                                                                                     " +
                "\n           COUNT(1)                                                                                                                                   " +
                "\n           FROM                                                                                                                                       " +
                "\n      (                                                                                                                                               " +
                "\n               SELECT                                                                                                                                 " +
                "\n               COUNT(1) AS NA                                                                                                                         " +
                "\n               FROM CollectionLevel2 C2(nolock)                                                                                                       " +
                "\n               LEFT JOIN Result_Level3 C3(nolock)                                                                                                     " +
                "\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                      " +
                "\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                             " +
                "\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                               " +
                "\n               AND C2.UnitId = @ParCompany_Id                                                                                                         " +
                "\n               AND IsNotEvaluate = 1                                                                                                                  " +
                "\n               GROUP BY C2.ID                                                                                                                         " +
                "\n           ) NA                                                                                                                                       " +
                "\n           WHERE NA = 2                                                                                                                               " +
                "\n  --------------------------------                                                                                                                    " +

               "\n SELECT " +
               "\n  " +
               "\n  --level1_Id " +
               "\n  --,Level1Name " +
               "\n  --,level2_Id " +
               "\n Level2Name AS MonitoramentoName " +
               "\n --,Unidade_Id " +
               "\n --,Unidade " +

               "\n     , sum(avSemPeso) as av " +
                "\n     , sum(ncSemPeso) as nc " +
                "\n     , CASE WHEN sum(AV) IS NULL OR sum(AV) = 0 THEN 0 ELSE sum(NC) / sum(AV) * 100 END AS [Proc] " +

               "\n FROM " +
               "\n ( " +
               "\n 	SELECT " +
               "\n 	 MON.Id			AS level2_Id " +
               "\n 	,MON.Name		AS Level2Name " +
               "\n 	,IND.Id AS level1_Id " +
               "\n 	,IND.Name AS Level1Name " +
               "\n 	,UNI.Id			AS Unidade_Id " +
               "\n 	,UNI.Name		AS Unidade " +


                               "\n         , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC/2 - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id in (3,4) THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.WeiEvaluation " +
                "\n         ELSE 0 " +
                "\n        END AS Av " +

                "\n       , CASE " +
                "\n         WHEN IND.HashKey = 1 THEN @VOLUMEPCC/2 - @NAPCC " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiEvaluation " +
                "\n         WHEN IND.ParConsolidationType_Id in (3,4) THEN CL2.EvaluatedResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.EvaluateTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.EvaluateTotal " +
                "\n         ELSE 0 " +
                "\n        END AS AvSemPeso " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id in (3,4) THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.WeiDefects " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NC " +

                "\n         , CASE " +
                "\n         WHEN IND.ParConsolidationType_Id = 1 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 2 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id in (3,4) THEN CL2.DefectsResult " +
                "\n         WHEN IND.ParConsolidationType_Id = 5 THEN CL2.DefectsTotal " +
                "\n         WHEN IND.ParConsolidationType_Id = 6 THEN CL2.TotalLevel3WithDefects " +
                "\n         ELSE 0 " +

                "\n         END AS NCSemPeso " +




               "\n 	FROM ConsolidationLevel2 CL2  (nolock)" +
               "\n 	INNER JOIN ConsolidationLevel1 CL1  (nolock)" +
               "\n 	ON CL1.Id = CL2.ConsolidationLevel1_Id " +
               "\n 	INNER JOIN ParLevel1 IND  (nolock)" +
               "\n 	ON IND.Id = CL1.ParLevel1_Id " +
               "\n 	INNER JOIN ParLevel2 MON  (nolock)" +
               "\n 	ON MON.Id = CL2.ParLevel2_Id " +
               "\n 	INNER JOIN ParCompany UNI (nolock) " +
               "\n 	ON UNI.Id = CL1.UnitId " +
               "\n 	WHERE CL2.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL" +
               "\n 	AND (UNI.Name = '" + form.unitName + "' OR UNI.Initials = '" + form.unitName + "')" +
               "\n 	AND IND.Name = '" + form.level1Name + "' " + //
               
               "\n ) S1 " +
               

            "\n  GROUP BY Level2Name " +
            "\n  HAVING sum(NC) > 0 " +
            "\n  ORDER BY 4 DESC ";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefa")]
        public List<NaoConformidadeResultsSet> GraficoTarefa([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoTarefas();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

                    //Av = av + i,
                    //Nc = nc + i,
                    //Proc = proc + i,
                    //TarefaName = tarefaName + i.ToString()

            var query = "" +

                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +

                 "\n DECLARE @VOLUMEPCC int                                                  " +
                "\n DECLARE @ParCompany_id INT                                              " +

                "\n SELECT @ParCompany_id = ID FROM PARCOMPANY WHERE NAME = '" + form.unitName + "'" +

                "\n --------------------------------                                                                                                                     " +
                "\n                                                                                                                                                      " +
                "\n  SELECT TOP 1 @VOLUMEPCC = SUM(Quartos) FROM VolumePcc1b  (nolock) WHERE ParCompany_id = @ParCompany_id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  DECLARE @NAPCC INT                                                                                                                                  " +
                "\n                                                                                                                                                      " +
                "\n                                                                                                                                                      " +
                "\n  SELECT                                                                                                                                              " +
                "\n         @NAPCC =                                                                                                                                     " +
                "\n           COUNT(1)                                                                                                                                   " +
                "\n           FROM                                                                                                                                       " +
                "\n      (                                                                                                                                               " +
                "\n               SELECT                                                                                                                                 " +
                "\n               COUNT(1) AS NA                                                                                                                         " +
                "\n               FROM CollectionLevel2 C2(nolock)                                                                                                       " +
                "\n               LEFT JOIN Result_Level3 C3(nolock)                                                                                                     " +
                "\n               ON C3.CollectionLevel2_Id = C2.Id                                                                                                      " +
                "\n               WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                             " +
                "\n               AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1)                                                               " +
                "\n               AND C2.UnitId = @ParCompany_Id                                                                                                         " +
                "\n               AND IsNotEvaluate = 1                                                                                                                  " +
                "\n               GROUP BY C2.ID                                                                                                                         " +
                "\n           ) NA                                                                                                                                       " +
                "\n           WHERE NA = 2                                                                                                                               " +
                "\n  --------------------------------                                                                                                                    " +

                "\n SELECT " +
                "\n TarefaName, NcSemPeso as Nc, AvSemPeso as Av, [Proc] FROM (" +
                "\n SELECT " +
                         "\n  " +
                    //     "\n  IND.Id AS level1_Id " +
                    //     "\n ,IND.Name AS Level1Name " +
                    //     "\n ,IND.Id AS level2_Id " +
                    //     "\n ,IND.Name AS Level2Name " +
                    //     "\n ,R3.ParLevel3_Id AS level3_Id " +
                         "\n R3.ParLevel3_Name AS TarefaName " +
                    //     "\n ,UNI.Name AS Unidade " +
                    //     "\n ,UNI.Id AS Unidade_Id " +
                         "\n ,SUM(R3.WeiDefects) AS Nc " +
                         "\n ,CASE WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiDefects) ELSE SUM(R3.Defects) END AS NcSemPeso  " +
                         "\n ,CASE " +
                         "\n WHEN IND.HashKey = 1 THEN @VOLUMEPCC/2 - @NAPCC " +
                         "\n ELSE SUM(R3.WeiEvaluation) END AS Av " +
                         "\n ,CASE " +
                         "\n WHEN IND.HashKey = 1 THEN @VOLUMEPCC/2 - @NAPCC " +
                         "\n WHEN IND.ParConsolidationType_Id = 2 THEN SUM(r3.WeiEvaluation) " +
                         "\n ELSE SUM(R3.Evaluation) END AS AvSemPeso " + 
                         "\n ,SUM(R3.WeiDefects) / " +
                         "\n CASE " +
                         "\n WHEN IND.HashKey = 1 THEN(SELECT TOP 1 SUM(Quartos) / 2 FROM VolumePcc1b (nolock) WHERE ParCompany_id = UNI.Id AND Data BETWEEN @DATAINICIAL AND @DATAFINAL) " +
                         "\n ELSE SUM(R3.WeiEvaluation) END * 100 AS [Proc] " +
                         "\n FROM Result_Level3 R3 (nolock) " +
                         "\n INNER JOIN CollectionLevel2 C2  (nolock)" +
                         "\n ON C2.Id = R3.CollectionLevel2_Id " +
                         "\n INNER JOIN ConsolidationLevel2 CL2  (nolock)" +
                         "\n ON CL2.Id = C2.ConsolidationLevel2_Id " +
                         "\n INNER JOIN ConsolidationLevel1 CL1  (nolock)" +
                         "\n ON CL1.Id = CL2.ConsolidationLevel1_Id " +
                         "\n INNER JOIN ParCompany UNI  (nolock)" +
                         "\n ON UNI.Id = C2.UnitId " +
                         "\n INNER JOIN ParLevel1 IND   (nolock)" +
                         "\n ON IND.Id = C2.ParLevel1_Id " +
                         "\n INNER JOIN ParLevel2 MON  (nolock)" +
                         "\n ON MON.Id = C2.ParLevel2_Id " +
                         "\n WHERE IND.Name = '" + form.level1Name + "' " +
                         "\n    and MON.Name = '" + form.level2Name + "' " +
                         "\n 	AND (UNI.Name = '" + form.unitName + "' OR UNI.Initials = '" + form.unitName + "')" +
                         "\n    AND R3.IsNotEvaluate = 0 " +
                         "\n 	AND CL2.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                         "\n GROUP BY " +
                         "\n  IND.Id " +
                         "\n ,IND.Name " +
                         "\n ,R3.ParLevel3_Id " +
                         "\n ,R3.ParLevel3_Name " +
                         "\n ,UNI.Name " +
                         "\n ,UNI.Id " +
                          "\n ,ind.hashKey " +
                          "\n ,ind.ParConsolidationType_Id " +

                         "\n HAVING SUM(R3.WeiDefects) > 0" +
                         "\n ) TAB ORDER BY 4 DESC";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefasAcumulada")]
        public List<NaoConformidadeResultsSet> GraficoTarefasAcumulada([FromBody] FormularioParaRelatorioViewModel form)
        {
            //_list = CriaMockGraficoTarefasAcumuladas();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            var query = "SELECT " +
                        "\n  " +
                        //     "\n  IND.Id AS level1_Id " +
                        //     "\n ,IND.Name AS Level1Name " +
                        //     "\n ,IND.Id AS level2_Id " +
                        //     "\n ,IND.Name AS Level2Name " +
                        //     "\n ,R3.ParLevel3_Id AS level3_Id " +
                        "\n R3.ParLevel3_Name AS TarefaName " +
                        //     "\n ,UNI.Name AS Unidade " +
                        //     "\n ,UNI.Id AS Unidade_Id " +
                        "\n ,SUM(R3.WeiDefects) AS Nc " +
                        "\n ,SUM(R3.WeiEvaluation) AS Av " +
                        "\n ,SUM(R3.WeiDefects) / SUM(R3.WeiEvaluation) * 100 AS [Proc] " +
                        "\n FROM Result_Level3 R3  (nolock)" +
                        "\n INNER JOIN CollectionLevel2 C2  (nolock)" +
                        "\n ON C2.Id = R3.CollectionLevel2_Id " +
                        "\n INNER JOIN ConsolidationLevel2 CL2  (nolock)" +
                        "\n ON CL2.Id = C2.ConsolidationLevel2_Id " +
                        "\n INNER JOIN ConsolidationLevel1 CL1  (nolock)" +
                        "\n ON CL1.Id = CL2.ConsolidationLevel1_Id " +
                        "\n INNER JOIN ParCompany UNI  (nolock)" +
                        "\n ON UNI.Id = CL1.UnitId " +
                        "\n INNER JOIN ParLevel1 IND   (nolock)" +
                        "\n ON IND.Id = CL1.ParLevel1_Id " +
                        "\n INNER JOIN ParLevel2 MON  (nolock)" +
                        "\n ON MON.Id = CL2.ParLevel2_Id " +
                        "\n WHERE IND.Name ='" + form.level1Name + "' " +
                        "\n /* and MON.Id = 1 */" +
                        "\n 	AND UNI.Name = '" + form.unitName + "'" +
                        "\n 	AND CL2.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                        "\n GROUP BY " +
                        "\n  IND.Id " +
                        "\n ,IND.Name " +
                        "\n ,R3.ParLevel3_Id " +
                        "\n ,R3.ParLevel3_Name " +
                        "\n ,UNI.Name " +
                        "\n ,UNI.Id " +
                        "\n HAVING SUM(R3.WeiDefects) > 0" +
                        "\n ORDER BY 4 DESC";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoUnidades()
        {

            #region Props
            var nc = 10;
            var av = 10;
            var proc = 20;
            var unidade = "Unidade"; 
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 30; i++)
            {
                list.Add(new NaoConformidadeResultsSet() { Av = av + i , Nc = nc + i, Proc = proc + i, UnidadeName = unidade + i.ToString() });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoNcPorUnidadeIndicador()
        {
            #region Props
            var nc = 10;
            var av = 10;
            var proc = 20;
            var Meta = 2;
            var indicadorName = "Indicador1"; 
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeResultsSet() {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    Meta = Meta + i -5,
                    IndicadorName = indicadorName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoMonitoramento()
        {
            //var Meta = 2;
            
            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var monitoramento = "Monitoramento";
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    MonitoramentoName = monitoramento + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoTarefas()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var tarefaName = "Tarefa";
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 90; i++)
            {
                list.Add(new NaoConformidadeResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    TarefaName = tarefaName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoTarefasAcumuladas()
        {
            //var Meta = 2;

            #region Prop
            var av = 10;
            var nc = 10;
            var proc = 20;
            var tarefaName = "TarefaAcumulada";
            #endregion

            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 90; i++)
            {
                list.Add(new NaoConformidadeResultsSet()
                {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    //Meta = Meta + i - 5,
                    TarefaName = tarefaName + i.ToString()
                });
                i += 10;
            }
            return list;
        }

    }
}

public class NaoConformidadeResultsSet
{

    internal string Select(DateTime _dataInicio, DateTime _dataFim, int unitId)
    {
        return "";
    }

    public string Indicador_Id { get; set; }
    public string IndicadorName { get; set; }
    public string Unidade_Id { get; set; }
    public string UnidadeName { get; set; }
    public string Monitoramento_Id { get; set; }
    public string MonitoramentoName { get; set; }
    public string Tarefa_Id { get; set; }
    public string TarefaName { get; set; }
    public decimal Nc { get; set; }
    public decimal Av { get; set; }
    public decimal Meta { get; set; }
    public decimal Proc { get; internal set; }
}
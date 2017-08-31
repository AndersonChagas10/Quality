using ADOFactory;
using Dominio;
using SGQDBContext;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Scorecard")]
    public class ScorecardController : ApiController
    {
        private List<ScorecardResultSet> _mock { get; set; }
        private List<ScorecardResultSet> _list { get; set; }

        public decimal[] SelectPontosScorecard(DateTime dtInicio, DateTime dtFim, int unidadeId, int tipo, int clusterSelected_Id) //Se 0, tras pontos , se 1, tras tudo                                                                                                                                                                                               
        {

            decimal[] pontosTotais = { 0, 0 };
            decimal pontosAtingidos = 0;
            decimal pontosDisputados = 0;

            DateTime _dtIni = dtInicio;
            DateTime _dtFim = dtFim;

            DateTime _novaDataIni = _dtIni;
            DateTime _novaDataFim = _dtIni;



            int numMeses = (12 * (_dtFim.Year - _dtIni.Year) + _dtFim.Month - _dtIni.Month) + 1;

            var sql = "";

            for (int i = 0; i < numMeses; i++)
            {
                if (i > 0)
                {
                    _novaDataIni = new DateTime(_novaDataIni.AddMonths(1).Year, _novaDataIni.AddMonths(1).Month, 1);
                }

                _novaDataFim = new DateTime(_novaDataIni.Year, _novaDataIni.Month, DateTime.DaysInMonth(_novaDataIni.Year, _novaDataIni.Month));

                if (i == numMeses - 1)
                {
                    _novaDataFim = _dtFim;
                }

                sql = new ScorecardResultSet().SelectScorecardCompleto(_novaDataIni, _novaDataFim, unidadeId, 0, clusterSelected_Id);

                using (var db = new SgqDbDevEntities())
                {

                    _list = db.Database.SqlQuery<ScorecardResultSet>(sql).ToList();

                    foreach (var r in _list.ToList())
                    {
                        pontosDisputados = r.PontosAtingidosIndicador.Value;

                        pontosAtingidos = r.PontosAtingidos.Value;
                    }
                }

                pontosTotais[0] += pontosDisputados;
                pontosTotais[1] += pontosAtingidos;

            }

            return pontosTotais;
        }

        [HttpPost]
        [Route("GetScorecard")]
        public List<ScorecardResultSet> GetScorecard([FromBody] FormularioParaRelatorioViewModel form)
        {
            //CriaMock();            

            CommonLog.SaveReport(form, "Report_Scorecard");

            decimal[] pontosTotais = SelectPontosScorecard(form._dataInicio, form._dataFim, form.unitId, 0, form.clusterSelected_Id);

            var query = new ScorecardResultSet().SelectScorecardCompleto(form._dataInicio, form._dataFim, form.unitId, 1, form.clusterSelected_Id);
            using (var db = new SgqDbDevEntities())
            {

                _list = db.Database.SqlQuery<ScorecardResultSet>(query).ToList();
                var total = new ScorecardResultSet() { Level1Name = "Total:", PontosIndicador = 784, Scorecard = 0, PontosAtingidos = 0 };

                var totalPontosDisputados = 0.0M;
                var totalPontosAtingidos = 0.0M;
                var totalScorecard = 0.0M;

                var pontosDisputados = 0.0M;
                var pontosAtingidos = 0.0M;

                foreach (var i in _list.ToList())
                {
                    pontosDisputados = 0;

                    if (i.PontosIndicador != null)
                        pontosDisputados = i.PontosIndicador.Value;
                    else
                        _list.Remove(i);

                    if (i.AV > 0)
                    {
                        totalPontosDisputados += pontosDisputados;

                        pontosAtingidos = 0;

                        if (i.PontosAtingidos != null)
                            pontosAtingidos = i.PontosAtingidos.Value;

                        totalPontosAtingidos += pontosAtingidos;
                    }
                }

                try
                {
                    totalScorecard = totalPontosDisputados == 0 ? 0 : Math.Round((totalPontosAtingidos / totalPontosDisputados * 100), 2);
                }
                catch (DivideByZeroException)
                {
                    totalScorecard = 0;
                }

                //_list.Add(new ScorecardResultSet() { Level1Name = "Total:", PontosIndicador = totalPontosDisputados, Scorecard = totalScorecard, PontosAtingidosIndicador = totalPontosAtingidos });

                try
                {
                    _list.Add(new ScorecardResultSet() { Level1Name = "Total:", PontosIndicador = pontosTotais[0], Scorecard = (pontosTotais[0] == 0 ? 0 : Math.Round((pontosTotais[1] / pontosTotais[0]) * 100, 2)), PontosAtingidos = totalPontosAtingidos });
                }
                catch (DivideByZeroException)
                {
                    _list.Add(new ScorecardResultSet() { Level1Name = "Total:", PontosIndicador = pontosTotais[0], Scorecard = Math.Round(0.00M, 2), PontosAtingidos = totalPontosAtingidos });
                }

            }

            return _list;
        }

        private void CriaMock()
        {
            _mock = new List<ScorecardResultSet>();
            _mock.Add(new ScorecardResultSet()
            {
                Cluster = 1,
                ClusterName = "Cluster 1",
                Regional = 1,
                RegionalName = "Reg1",
                ParCompanyId = 1,
                ParCompanyName = "Comp1",
                TipoIndicador = 1,
                TipoIndicadorName = "tipo Ind1",
                Level1Id = 1,
                Level1Name = "Level11",
                Criterio = 1,
                CriterioName = "Maior",
                AV = 3,
                NC = 2,
                Pontos = 3.2M,
                Meta = 4.1M,
                Real = 5.6M,
                PontosAtingidos = 50,
                Scorecard = 80
            });

            _mock.Add(new ScorecardResultSet()
            {
                Cluster = 2,
                ClusterName = "Cluster 2",
                Regional = 2,
                RegionalName = "Reg2",
                ParCompanyId = 2,
                ParCompanyName = "Comp2",
                TipoIndicador = 2,
                TipoIndicadorName = "tipo Ind2",
                Level1Id = 2,
                Level1Name = "Level22",
                Criterio = 2,
                CriterioName = "Menor",
                AV = 3,
                NC = 2,
                Pontos = 3.2M,
                Meta = 4.1M,
                Real = 5.6M,
                PontosAtingidos = 50,
                Scorecard = 100
            });

            _mock.Add(new ScorecardResultSet()
            {
                Cluster = 3,
                ClusterName = "Cluster 3",
                Regional = 3,
                RegionalName = "Reg3",
                ParCompanyId = 3,
                ParCompanyName = "Comp3",
                TipoIndicador = 3,
                TipoIndicadorName = "tipo Ind3",
                Level1Id = 3,
                Level1Name = "Level33",
                Criterio = 3,
                CriterioName = "Maior",
                AV = 3,
                NC = 2,
                Pontos = 3.2M,
                Meta = 4.1M,
                Real = 5.6M,
                PontosAtingidos = 50,
                Scorecard = 10
            });
        }

        //MIGRAÇÃO ESTADOS UNIDOS

        /// <summary>
        /// Retorna Objeto Dinamico com dados da query.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="query"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("Migracao/{unidade}/{ip}/{banco}/{ipProd}/{bancoProd}")]
        public void MigracaoParte1(string unidade, string ip, string banco, string ipProd, string bancoProd)
        {
            
            #region parte01

            List<ResultadoUmaColuna> collectionlevel2List = new List<ResultadoUmaColuna>();

            //inserir o ambiente do cliente a ser copiado
            //using (var db = new Factory(ip, banco, "wordpass14t", "grjuser"))
            using (var db = new Factory(ip, banco, "betsy1", "sa"))
            {
                var collectionlevel2 = "" +

                    "\n SELECT-- top 10 " +
                    "\n 'INSERT INTO CollectionLevel2 ([Key],ConsolidationLevel2_Id,ParLevel1_Id,ParLevel2_Id,UnitId,AuditorId,[Shift],Period,Phase,ReauditIs,ReauditNumber,CollectionDate,StartPhaseDate,EvaluationNumber,[Sample],AddDate,AlterDate,ConsecutiveFailureIs,ConsecutiveFailureTotal,NotEvaluatedIs,Duplicated,HaveCorrectiveAction,HaveReaudit,HavePhase,Completed,ParFrequency_Id,AlertLevel,Sequential,Side,WeiEvaluation,Defects,WeiDefects,TotalLevel3WithDefects,TotalLevel3Evaluation,LastEvaluationAlert,EvaluatedResult,DefectsResult,IsEmptyLevel3,LastLevel2Alert,ReauditLevel,StartPhaseEvaluation) VALUES (' + " +
                    "\n  '''' + ISNULL(CAST('" + unidade + "' AS VARCHAR(500)) + '-' + CAST(c2.Id AS VARCHAR(500)),'NULL') +''',' + --KEY                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          " +
                    "\n + '1' + ', ' + --ConsolidationLevel2_Id                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           " +
                    "\n + '(SELECT TOP 1 Id FROM ParLevel1 L1 WHERE L1.[Description] = ''' + CAST(Level01Id as VARCHAR) + '''), ' + --Level01Id                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           " +
                    "\n + '(SELECT TOP 1 Id FROM ParLevel2 L2 WHERE L2.[Description] = ''' + CAST(Level02Id as VARCHAR) + '''), ' + --Level02Id                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           " +
                    "\n + '(SELECT TOP 1 Id FROM ParCompany C WHERE C.[Description] = ''' + CAST([UnitId] as VARCHAR) +'''), ' + --UnitId                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 " +
                    "\n + '(SELECT TOP 1 Id FROM UserSGQ U WHERE U.[Name] = ''' + CAST(UU.Nome as VARCHAR) + '''), ' + --AuditorId                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        " +
                    "\n + ISNULL(CAST([Shift]  AS VARCHAR),'NULL') +', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                " +
                    "\n +ISNULL(CAST([Period]  AS VARCHAR),'NULL') +', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                " +
                    "\n +ISNULL(CAST([Phase]  AS VARCHAR),'NULL') +', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 " +
                    "\n +ISNULL(CAST([ReauditIs]  AS VARCHAR),'NULL') +', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             " +
                    "\n +ISNULL(CAST([ReauditNumber]  AS VARCHAR),'NULL') +', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         " +
                    "\n +CASE WHEN CollectionDate IS NULL THEN 'NULL' ELSE '''' + CAST(CollectionDate  AS VARCHAR) + '''' END + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    " +
                    "\n +CASE WHEN StartPhaseDate IS NULL THEN 'NULL' ELSE '''' + CAST(StartPhaseDate  AS VARCHAR) + '''' END + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    " +
                    "\n +ISNULL(CAST(EvaluationNumber  AS VARCHAR), 'NULL') + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      " +
                    "\n +ISNULL(CAST([Sample]  AS VARCHAR),'NULL') +', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                " +
                    "\n +CASE WHEN c2.AddDate IS NULL THEN 'NULL' ELSE '''' + CAST(c2.AddDate  AS VARCHAR) + '''' END + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            " +
                    "\n +CASE WHEN c2.AlterDate IS NULL THEN 'NULL' ELSE '''' + CAST(c2.AlterDate  AS VARCHAR) + '''' END + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        " +
                    "\n +ISNULL(CAST(ConsecutiveFailureIs  AS VARCHAR), 'NULL') + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  " +
                    "\n +ISNULL(CAST(ConsecutiveFailureTotal  AS VARCHAR), 'NULL') + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               " +
                    "\n +ISNULL(CAST(c2.NotEvaluatedIs  AS VARCHAR), 'NULL') + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     " +
                    "\n +ISNULL(CAST(c2.Duplicated  AS VARCHAR), 'NULL') + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         " +
                    "\n +ISNULL(CAST(c2.HaveCorrectiveAction  AS VARCHAR), 'NULL') + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               " +
                    "\n +ISNULL(CAST(c2.HaveReaudit  AS VARCHAR), 'NULL') + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        " +
                    "\n +ISNULL(CAST(c2.HavePhase  AS VARCHAR), 'NULL') + ', ' +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          " +
                    "\n +'0' + ', ' + --Completed                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         " +
                    "\n + '1' + ', ' + --ParFrequency_Id                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  " +
                    "\n + '0' + ', ' + --AlertLevel                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       " +
                    "\n + '0' + ', ' + --Sequential                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       " +
                    "\n + '0' + ', ' + --Side                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             " +
                    "\n + '0' + ', ' + --WeiEvaluation                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    " +
                    "\n + '0' + ', ' + --Defects                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          " +
                    "\n + '0' + ', ' + --WeiDefects--ResultLevel03_Teste.WeiDefects                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       " +
                    "\n + '0' + ', ' + --TotalLevel3WithDefects--ResultLevel03_Teste.WeiDefects TotalLevel3WithDefects                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    " +
                    "\n +'0' + ', ' + --TotalLevel3Evaluation--ResultLevel03_Teste.WeiEvaluation TotalLevel3Evaluation                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    " +
                    "\n +'0' + ', ' + --LastEvaluationAlert                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               " +
                    "\n + '1' + ', ' + --EvaluatedResult                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  " +
                    "\n + '0' + ', ' + --DefectsResult--case when WeiDefects> 0 then 1 else 0 end DefectsResult                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           " +
                    "\n +'0' + ', ' + --IsEmptyLevel3                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     " +
                    "\n + '0' + ', ' + --LastLevel2Alert                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  " +
                    "\n + '0' + ', ' + --ReauditLevel                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     " +
                    "\n + 'NULL' + ')'--  A                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               " +
                    "\n AS retorno FROM CollectionLevel02 c2 WITH(NOLOCK)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            " +
                    "\n  INNER JOIN (SELECT ID COD, NAME NOME FROM UserSgq WITH(NOLOCK)) UU                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               " +
                    "\n ON UU.COD = C2.[AuditorId]                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        " +
                "\n -- WHERE c2.addDate between '20170110' and '20170115' ";

                dynamic resultado = db.QueryNinjaADO(collectionlevel2);

                for (var i = 0; i < resultado.Count; i++)
                {
                    try
                    {
                        ResultadoUmaColuna item = new ResultadoUmaColuna();
                        item.retorno = resultado[i].retorno;
                        collectionlevel2List.Add(item);

                    }
                    catch (Exception)
                    {


                    }
                }

            }

            #endregion

            #region parte02

            //inserir dados do clinete que receberá os dados
            //using (var db = new Factory(ipProd, bancoProd, "wordpass14t", "grjuser"))
            using (var db = new Factory(ipProd, bancoProd, "betsy1", "sa"))
            {

                foreach (var i in collectionlevel2List)
                {
                    try
                    {
                        db.ExecuteSql(i.retorno.ToString());

                    }
                    catch (Exception)
                    {


                    }
                }


            }

            #endregion

            #region parte03            

            List<ResultadoUmaColuna> result_level3List = new List<ResultadoUmaColuna>();
            //inserir o ambiente do cliente a ser copiado
            //using (var db = new Factory(ip, banco, "wordpass14t", "grjuser"))
            using (var db = new Factory(ip, banco, "betsy1", "sa"))
            {

                var result_level3 = "" +

                    "\n SELECT-- TOP 10                                                                                                                                                                                                                                     " +
                    "\n 'INSERT INTO [Result_Level3] (CollectionLevel2_Id,ParLevel3_Id,ParLevel3_Name,[Weight],IntervalMin,IntervalMax,Value,ValueText,IsConform,IsNotEvaluate,Defects,PunishmentValue,WeiEvaluation,Evaluation,WeiDefects,CT4Eva3,Sampling) VALUES (' +    " +
                    "\n '(SELECT TOP 1 Id FROM CollectionLevel2 WHERE [Key] = ''" + unidade + "-' + CAST(CollectionLevel02.Id AS VARCHAR) + '''), ' + --CollectionLevel2_Id                                                                                                               " +
                    "\n '(SELECT TOP 1 Id FROM ParLevel3 L3 WHERE L3.[Description] = ''' + CAST(Level03Id as VARCHAR) + '''), ' + --ParLevel3_Id                                                                                                                            " +
                    "\n '(SELECT TOP 1 Name FROM ParLevel3 L3 WHERE L3.[Description] = ''' + CAST(Level03Id as VARCHAR) + '''), ' + --ParLevel3_Name                                                                                                                        " +
                    "\n '1' + ',' + -- [Weight]                                                                                                                                                                                                                             " +
                    "\n '0' + ',' + -- [IntervalMin]                                                                                                                                                                                                                        " +
                    "\n '0' + ',' + -- [IntervalMax]                                                                                                                                                                                                                        " +
                    "\n CASE WHEN Value IS NULL THEN 'NULL' ELSE CAST(Value  AS VARCHAR)  END + ', ' +                                                                                                                                                                      " +
                    "\n CASE WHEN ValueText IS NULL THEN 'NULL' ELSE '''' + CAST(ValueText  AS VARCHAR) + ''''  END + ', ' +                                                                                                                                                " +
                    "\n '' + CASE WHEN(CASE WHEN VALUE > 0 THEN 1 ELSE 0 END) > 0 THEN '0' ELSE '1' END + ' ,' + --IsConform /*Alterado após gerar Script*/                                                                                                                 " +
                    "\n CASE WHEN CollectionLevel02.NotEvaluatedIs IS NULL THEN 'NULL' ELSE CAST(CollectionLevel02.NotEvaluatedIs  AS VARCHAR) END + ', ' + --AS IsNotEvaluate                                                                                              " +
                    "\n '' + CASE WHEN VALUE > 0 THEN '1' ELSE '0' END + ' ,' + --Defects                                                                                                                                                                                   " +
                    "\n '0' + ' ,' + --PunishmentValue                                                                                                                                                                                                                      " +
                    "\n '1 * 1' + ' ,' + --WeiEvaluation                                                                                                                                                                                                                    " +
                    "\n '1' + ' ,' + --Evaluation                                                                                                                                                                                                                           " +
                    "\n '1 * (0 + ' + CASE WHEN VALUE > 0 THEN '1' ELSE '0' END + ')' + ' ,' + --AS WeiDefects                                                                                                                                                              " +
                    "\n 'NULL' + ' ,' + --CT4Eva3                                                                                                                                                                                                                           " +
                    "\n 'NULL' + ')'-- Sampling                                                                                                                                                                                                                             " +
                    "\n    as retorno                                                                                                                                                                                                                                                 " +
                    "\n  FROM CollectionLevel03                                                                                                                                                                                                                             " +
                    "\n  LEFT JOIN CollectionLevel02 on CollectionLevel03.CollectionLevel02Id = CollectionLevel02.Id                                                                                                                                                        " +
                    "\n LEFT JOIN Level03                                                                                                                                                                                                                                   " +
                    "\n     ON CollectionLevel03.Level03Id = Level03.Id                                                                                                                                                                                                     " +
                    "\n LEFT JOIN ParLevel3                                                                                                                                                                                                                                 " +
                    "\n     ON CASE WHEN ParLevel3.[Description] = 'Other' then '18' ELSE ParLevel3.Description END = CAST(Level03.Id AS VARCHAR(500))                                                                                                                      " +
                "\n -- WHERE CollectionLevel03.addDate between '20170110' and '20170115' ";

                dynamic resultado = db.QueryNinjaADO(result_level3);

                for (var i = 0; i < resultado.Count; i++)
                {
                    try
                    {
                        ResultadoUmaColuna item = new ResultadoUmaColuna();
                        item.retorno = resultado[i].retorno;
                        result_level3List.Add(item);

                    }
                    catch (Exception)
                    {


                    }
                }

            }

            #endregion

            #region parte04
            //inserir dados do cliente que receberá os dados
            //using (var db = new Factory(ipProd, bancoProd, "wordpass14t", "grjuser"))
            using (var db = new Factory(ipProd, bancoProd, "betsy1", "sa"))
            {

                foreach (var i in result_level3List)
                {
                    try
                    {
                        db.ExecuteSql(i.retorno.ToString());
                    }
                    catch (Exception)
                    {

                    }

                }


            }

            #endregion

            #region parte05
            var etapa1 = "" +

"\n                 DECLARE CollectionL2 CURSOR FOR                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n select                                                                                                                                                                              " +
"\n ParLevel1_Id,                                                                                                                                                                       " +
"\n ParLevel2_Id,                                                                                                                                                                       " +
"\n UnitId,                                                                                                                                                                             " +
"\n CAST(CollectionDate AS DATE) DATA                                                                                                                                                   " +
"\n from CollectionLevel2                                                                                                                                                               " +
"\n -- WHERE ID = 470293                                                                                                                                                                " +
"\n WHERE 1 = 1                                                                                                                                                                         " +
"\n and [Key] like '" + unidade + "-%'                                                                                                                                          " +
"\n GROUP BY ParLevel1_Id, ParLevel2_Id, UnitId,  CAST(CollectionDate AS DATE)                                                                                                          " +
"\n                                                                                                                                                                                     " +
"\n ;                                                                                                                                                                                   " +
"\n             OPEN CollectionL2;                                                                                                                                                      " +
"\n                                                                                                                                                                                     " +
"\n             DECLARE @UNIDADEID INT                                                                                                                                                  " +
"\n             DECLARE @DATACOLETA DATE                                                                                                                                                " +
"\n DECLARE @LEVEL1ID INT                                                                                                                                                               " +
"\n DECLARE @LEVEL2ID INT                                                                                                                                                               " +
"\n                                                                                                                                                                                     " +
"\n DECLARE @CONSOLIDATIONLEVEL1ID INT                                                                                                                                                  " +
"\n DECLARE @CONSOLIDATIONLEVEL2ID INT                                                                                                                                                  " +
"\n                                                                                                                                                                                     " +
"\n FETCH NEXT FROM CollectionL2 INTO @LEVEL1ID, @LEVEL2ID, @UNIDADEID, @DATACOLETA                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n WHILE @@FETCH_STATUS = 0                                                                                                                                                            " +
"\n    BEGIN                                                                                                                                                                            " +
"\n      FETCH NEXT FROM CollectionL2 INTO @LEVEL1ID, @LEVEL2ID, @UNIDADEID, @DATACOLETA                                                                                                " +
"\n                                                                                                                                                                                     " +
"\n       SELECT @CONSOLIDATIONLEVEL1ID = ID FROM ConsolidationLevel1 WHERE UnitId = @UNIDADEID AND ParLevel1_Id = @LEVEL1ID AND CONVERT(date, ConsolidationDate) = @DATACOLETA         " +
"\n                                                                                                                                                                                     " +
"\n        -- DECLARE @CONSOLIDATIONLEVEL1ID INT                                                                                                                                        " +
"\n                                                                                                                                                                                     " +
"\n        -- SELECT @CONSOLIDATIONLEVEL1ID = ID FROM ConsolidationLevel1 WHERE UnitId = 55 AND ParLevel1_Id = 310 AND CONVERT(date, ConsolidationDate) = '20170710'                    " +
"\n                                                                                                                                                                                     " +
"\n        -- SELECT @CONSOLIDATIONLEVEL1ID                                                                                                                                             " +
"\n                                                                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n        IF @CONSOLIDATIONLEVEL1ID IS NULL OR @CONSOLIDATIONLEVEL1ID = 1                                                                                                              " +
"\n                                                                                                                                                                                     " +
"\n        BEGIN                                                                                                                                                                        " +
"\n                                                                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n          INSERT ConsolidationLevel1([UnitId],[DepartmentId],[ParLevel1_Id],[AddDate],[AlterDate],[ConsolidationDate])                                                               " +
"\n 		VALUES(@UNIDADEID, 1, @LEVEL1ID, GetDate(), null, @DATACOLETA)                                                                                                              " +
"\n                                                                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n         SELECT @CONSOLIDATIONLEVEL1ID = @@IDENTITY                                                                                                                                  " +
"\n                                                                                                                                                                                     " +
"\n       END                                                                                                                                                                           " +
"\n                                                                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n       SELECT @CONSOLIDATIONLEVEL2ID = Id FROM ConsolidationLevel2 WHERE ConsolidationLevel1_Id = @CONSOLIDATIONLEVEL1ID AND ParLevel2_Id = @LEVEL2ID AND UnitId = @UNIDADEID        " +
"\n                                                                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n       IF @CONSOLIDATIONLEVEL2ID IS NULL  OR @CONSOLIDATIONLEVEL1ID = 1                                                                                                              " +
"\n                                                                                                                                                                                     " +
"\n       BEGIN                                                                                                                                                                         " +
"\n                                                                                                                                                                                     " +
"\n         INSERT ConsolidationLevel2([ConsolidationLevel1_Id], [ParLevel2_Id], [UnitId], [AddDate], [AlterDate], [ConsolidationDate])                                                 " +
"\n                                                                                                                                                                                     " +
"\n         VALUES(@CONSOLIDATIONLEVEL1ID, @LEVEL2ID, @UNIDADEID, GETDATE(), NULL, @DATACOLETA )                                                                                        " +
"\n                                                                                                                                                                                     " +
"\n 		SELECT @CONSOLIDATIONLEVEL2ID = @@IDENTITY                                                                                                                                  " +
"\n                                                                                                                                                                                     " +
"\n       END                                                                                                                                                                           " +
"\n                                                                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n       SET @CONSOLIDATIONLEVEL1ID = NULL                                                                                                                                             " +
"\n                                                                                                                                                                                     " +
"\n       SET @CONSOLIDATIONLEVEL2ID = NULL                                                                                                                                             " +
"\n                                                                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n                                                                                                                                                                                     " +
"\n    END;                                                                                                                                                                             " +
"\n CLOSE CollectionL2;                                                                                                                                                                 " +
"\n         DEALLOCATE CollectionL2;                                                                                                                                                    ";


            //using (var db = new Factory(ipProd, bancoProd, "wordpass14t", "grjuser"))
            using (var db = new Factory(ipProd, bancoProd, "betsy1", "sa"))
            {
                try
                {

                    db.ExecuteSql(etapa1);
                }
                catch (Exception)
                {


                }
            }

            #endregion

            #region parte06

            var etapa2 = "" +

"\n                 DECLARE Consulta01 CURSOR FOR                   " +
"\n                                                                 " +
"\n SELECT                                                          " +
"\n c2.Id, CS2.id                                                   " +
"\n FROM CollectionLevel2 C2                                        " +
"\n inner JOIN ConsolidationLevel2 CS2                              " +
"\n ON CS2.ConsolidationDate = CAST(C2.CollectionDate AS DATE)      " +
"\n AND CS2.UnitId = C2.UnitId                                      " +
"\n AND CS2.ParLevel2_Id = C2.ParLevel2_Id                          " +
"\n INNER JOIN ConsolidationLevel1 CS1                              " +
"\n on CS1.Id = CS2.ConsolidationLevel1_Id                          " +
"\n and CS1.ParLevel1_Id = C2.ParLevel1_Id                          " +
"\n WHERE and [Key] like '" + unidade + "-%'                   " +
"\n --AND CAST(C2.CollectionDate AS DATE) >= '20170301'             " +
"\n and CS2.Id = 1                                               " +
"\n                                                                 " +
"\n ;                                                               " +
"\n             OPEN Consulta01;                                    " +
"\n                                                                 " +
"\n                                                                 " +
"\n             DECLARE @ConsolidationLevel2_Id INT                 " +
"\n             DECLARE @Id INT                                     " +
"\n                                                                 " +
"\n FETCH NEXT FROM Consulta01 INTO                                 " +
"\n @Id,                                                            " +
"\n @ConsolidationLevel2_Id                                         " +
"\n                                                                 " +
"\n WHILE @@FETCH_STATUS = 0                                        " +
"\n    BEGIN                                                        " +
"\n       FETCH NEXT FROM Consulta01 INTO                           " +
"\n                                                                 " +
"\n         @Id ,                                                   " +
"\n 		@ConsolidationLevel2_Id                                 " +
"\n                                                                 " +
"\n         UPDATE CollectionLevel2 SET                             " +
"\n                                                                 " +
"\n         ConsolidationLevel2_Id = @ConsolidationLevel2_Id        " +
"\n                                                                 " +
"\n         WHERE ID = @Id                                          " +
"\n                                                                 " +
"\n                                                                 " +
"\n                                                                 " +
"\n    END;                                                         " +
"\n             CLOSE Consulta01;                                   " +
"\n             DEALLOCATE Consulta01;                              ";

            //using (var db = new Factory(ipProd, bancoProd, "wordpass14t", "grjuser"))
            using (var db = new Factory(ipProd, bancoProd, "betsy1", "sa"))
            {
                try
                {
                    db.ExecuteSql(etapa2);

                }
                catch (Exception)
                {


                }
            }

            //--------------------------------------------------------------------------------

            #endregion

            #region Parte07
            Services.SyncServices s = new Services.SyncServices();

            //using (var db = new Factory(ipProd, bancoProd, "wordpass14t", "grjuser"))
            using (var db = new Factory(ipProd, bancoProd, "betsy1", "sa"))
            {

                ResultadoUmaColuna list = new ResultadoUmaColuna();

                dynamic resultado = db.QueryNinjaADO("select id as retorno from collectionLevel2 where [Key] like '" + unidade + "-%'");

                for (var i = 0; i < resultado.Count; i++)
                {
                    try
                    {
                        ResultadoUmaColuna item = new ResultadoUmaColuna();
                        item.retorno = resultado[i].retorno;
                        s.ReconsolidationToLevel3(item.retorno.ToString());
                        System.Console.Write("Reconsolidando: " + item.retorno.ToString());

                    }
                    catch (Exception)
                    {


                    }
                }

            }

            #endregion

        }


    }



}

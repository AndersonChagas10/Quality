using Dominio;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //_list = CriaMockGraficoUnidades();

        var query = "" +
                "\n SELECT " +

                "\n Unidade as UnidadeName" +
                "\n ,CONVERT(varchar, Unidade_Id) as Unidade_Id" +
                "\n ,ProcentagemNc as [proc] " +

                "\n ,nc" +
                "\n ,av " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n       Unidade  " +
                "\n     , Unidade_Id " +
                "\n     , sum(av) as av " +
                "\n     , sum(nc) as nc " +
                "\n     , CASE WHEN sum(AV) IS NULL OR sum(AV) = 0 THEN 0 ELSE sum(NC) / sum(AV) * 100 END AS ProcentagemNc " +

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
                "\n         WHERE CL1.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
             
                "\n     ) S1 " +

                "\n     GROUP BY Unidade, Unidade_Id " +

                "\n ) S2 " +
                "\n WHERE nc > 0 " +
                "\n ORDER BY 1 DESC";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;
        }


        [HttpPost]
        [Route("GraficoIndicador/{unidade}")]
        public List<NaoConformidadeResultsSet> GraficoIndicador(string unidade, [FromBody] FormularioParaRelatorioViewModel form)
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
                "\n SELECT " +

                "\n  CONVERT(varchar, Unidade) as UnidadeName" +
                "\n ,CONVERT(varchar, Unidade_Id) as Unidade_Id" +
                "\n ,CONVERT(varchar, level1_Id) as Indicador_Id" +
                "\n ,CONVERT(varchar, Level1Name) as IndicadorName" +

                
                "\n ,ProcentagemNc as [proc] " +

                "\n ,nc" +
                "\n ,av " +
                "\n FROM " +
                "\n ( " +
                "\n     SELECT " +
                "\n       Unidade  " +
                "\n     , Unidade_Id " +
                "\n     , Level1Name " +
                "\n     , level1_Id " +
                "\n     , sum(av) as av " +
                "\n     , sum(nc) as nc " +
                "\n     , CASE WHEN sum(AV) IS NULL OR sum(AV) = 0 THEN 0 ELSE sum(NC) / sum(AV) * 100 END AS ProcentagemNc " +

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
                "\n         WHERE CL1.ConsolidationDate BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "'" +
                "\n         AND UNI.Name = '" + unidade + "'" +

                "\n     ) S1 " +

                "\n     GROUP BY Unidade, Unidade_Id, Level1Name, level1_Id  " +

                "\n ) S2 " +
                "\n WHERE nc > 0 " +
                "\n ORDER BY 1 DESC";

            using (var db = new SgqDbDevEntities())
            {
                _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            }

            return _list;

        }

        [HttpPost]
        [Route("GraficoMonitoramento/{indicador}")]
        public List<NaoConformidadeResultsSet> GraficoMonitoramento(string indicador)
        {
            _list = CriaMockGraficoMonitoramento();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //using (var db = new SgqDbDevEntities())
            //{
            //    _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            //}

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefa/{indicador}")]
        public List<NaoConformidadeResultsSet> GraficoTarefa(string indicador)
        {
            _list = CriaMockGraficoTarefas();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //using (var db = new SgqDbDevEntities())
            //{
            //    _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            //}

            return _list;
        }

        [HttpPost]
        [Route("GraficoTarefasAcumulada/{indicador}")]
        public List<NaoConformidadeResultsSet> GraficoTarefasAcumulada(string indicador)
        {
            _list = CriaMockGraficoTarefasAcumuladas();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //using (var db = new SgqDbDevEntities())
            //{
            //    _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            //}

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
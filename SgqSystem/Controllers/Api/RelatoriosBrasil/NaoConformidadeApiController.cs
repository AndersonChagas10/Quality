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
            _list = CriaMockGraficoUnidades();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //using (var db = new SgqDbDevEntities())
            //{
            //    _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            //}

            return _list;
        }

        [HttpPost]
        [Route("GraficoIndicador/{unidadeName}")]
        public List<NaoConformidadeResultsSet> GraficoIndicador(string unidadeName)
        {
            _list = CriaMockGraficoNcPorUnidadeIndicador();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //using (var db = new SgqDbDevEntities())
            //{
            //    _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            //}

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

    public string IndicadorName { get; set; }
    public string UnidadeName { get; set; }
    public string MonitoramentoName { get; set; }
    public string TarefaName { get; set; }
    public decimal Nc { get; set; }
    public decimal Av { get; set; }
    public decimal Meta { get; set; }
    public int Proc { get; internal set; }
}
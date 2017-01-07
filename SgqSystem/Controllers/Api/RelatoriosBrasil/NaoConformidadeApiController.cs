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
        [Route("GraficoNcPorUnidadeIndicador/{unidadeName}")]
        public List<NaoConformidadeResultsSet> GraficoNcPorUnidadeIndicador(string unidadeName)
        {
            _list = CriaMockGraficoNcPorUnidadeIndicador();

            //var query = new NaoConformidadeResultsSet().Select(form._dataInicio, form._dataFim, form.unitId);

            //using (var db = new SgqDbDevEntities())
            //{
            //    _list = db.Database.SqlQuery<NaoConformidadeResultsSet>(query).ToList();
            //}

            return _list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoUnidades()
        {
            var nc = 10;
            var av = 10;
            var proc = 20;
            var unidade = "Unidade";
            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 30; i++)
            {
                list.Add(new NaoConformidadeResultsSet() { Av = av + i , Nc = nc + i, Proc = proc + i, IndicadorName = unidade + i.ToString() });
                i += 10;
            }
            return list;
        }

        internal List<NaoConformidadeResultsSet> CriaMockGraficoNcPorUnidadeIndicador()
        {
            var nc = 10;
            var av = 10;
            var proc = 20;
            var Meta = 2;
            var unidade = "Unidade";
            var list = new List<NaoConformidadeResultsSet>();
            for (int i = 0; i < 60; i++)
            {
                list.Add(new NaoConformidadeResultsSet() {
                    Av = av + i,
                    Nc = nc + i,
                    Proc = proc + i,
                    Meta = Meta + i -5,
                    IndicadorName = unidade + i.ToString()
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
    public decimal Nc { get; set; }
    public decimal Av { get; set; }
    public decimal Meta { get; set; }
    public int Proc { get; internal set; }
}
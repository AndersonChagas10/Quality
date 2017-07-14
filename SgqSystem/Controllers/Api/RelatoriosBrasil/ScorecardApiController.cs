using Dominio;
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

        public decimal[] SelectPontosScorecard(DateTime dtInicio, DateTime dtFim, int unidadeId, int tipo) //Se 0, tras pontos , se 1, tras tudo                                                                                                                                                                                               
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

                sql = new ScorecardResultSet().SelectScorecardCompleto(_novaDataIni, _novaDataFim, unidadeId, 0);

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

            decimal[] pontosTotais = SelectPontosScorecard(form._dataInicio, form._dataFim, form.unitId, 0);

            var query = new ScorecardResultSet().SelectScorecardCompleto(form._dataInicio, form._dataFim, form.unitId, 1);
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

                        if(i.PontosAtingidos != null)
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
                    _list.Add(new ScorecardResultSet() { Level1Name = "Total:", PontosIndicador = pontosTotais[0], Scorecard =  Math.Round(0.00M, 2), PontosAtingidos = totalPontosAtingidos });
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

    }

    

}

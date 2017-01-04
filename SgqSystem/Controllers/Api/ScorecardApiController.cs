using Dominio;
using SgqSystem.ViewModels;
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

        [HttpPost]
        [Route("GetScorecard")]
        public List<ScorecardResultSet> GetScorecard([FromBody] FormularioParaRelatorioViewModel form)
        {
            //CriaMock();
            var query = new ScorecardResultSet().SelectScorecard(form._dataInicio, form._dataFim, form.unitId);
            using (var db = new SgqDbDevEntities())
            {

                _list = db.Database.SqlQuery<ScorecardResultSet>(query).ToList();
                var total = new ScorecardResultSet() { Level1Name = "Total:", Pontos = 0, Scorecard = 0 };

                foreach (var i in _list)
                {
                    if (i.AV > 0)
                    {
                        total.Pontos += i.PontosAtingidos - i.Pontos;

                        if (total.Scorecard >= 100)
                            total.Scorecard = 100;
                        else
                            total.Scorecard += i.Scorecard;
                    }
                }
                _list.Add(total);
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

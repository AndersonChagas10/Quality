using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Scorecard")]
    public class ScorecardController : ApiController
    {
        public List<MockScorecard> _mock;

        [HttpPost]
        [Route("GetScorecardMock")]
        public List<MockScorecard> GetScorecardMock([FromBody] FormularioParaRelatorioViewModel form)
        {
            CriaMock();
            return _mock;
        }

        //public void GetScorecardMock()
        //{
        //    CriaMock();
        //    return _mock;
        //}

        private void CriaMock()
        {
            _mock = new List<MockScorecard>();
            _mock.Add(new MockScorecard()
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

            _mock.Add(new MockScorecard()
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

            _mock.Add(new MockScorecard()
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

    public class MockScorecard
    {
        public int Cluster { get; set; }
        public string ClusterName { get; set; }

        public int Regional { get; set; }
        public string RegionalName { get; set; }

        public int ParCompanyId { get; set; }
        public string ParCompanyName { get; set; }

        public int TipoIndicador { get; set; }
        public string TipoIndicadorName { get; set; }

        public int Level1Id { get; set; }
        public string Level1Name { get; set; }

        public int Criterio { get; set; }
        public string CriterioName { get; set; }

        public decimal AV { get; set; }
        public decimal NC { get; set; }

        public decimal Pontos { get; set; }
        public decimal Meta { get; set; }
        public decimal Real { get; set; }
        public decimal PontosAtingidos { get; set; }
        public decimal Scorecard { get; set; }
    }
}

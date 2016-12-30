using Dominio;
using SgqSystem.ViewModels;
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
        private List<RelDiarioGrafico1Result> _mock { get; set; }
        private List<RelDiarioGrafico1Result> _listGrafico1Level1 { get; set; }

        [HttpPost]
        [Route("Grafico1")]
        public List<RelDiarioGrafico1Result> GetGrafico1Level1([FromBody] FormularioParaRelatorioViewModel form)
        {
            CriaMockGrafico1Level1();
            //var query = "";
            //using (var db = new SgqDbDevEntities())
            //{
            //    _listGrafico1Level1 = db.Database.SqlQuery<RelDiarioGrafico1Result>(query).ToList();
            //}

            return _mock;
        }

        private void CriaMockGrafico1Level1()
        {
            _mock = new List<RelDiarioGrafico1Result>();
            _mock.Add(new RelDiarioGrafico1Result()
            {
                Level1Name = "Level1 teste 1",
                NCProc = 90.8M,
                Meta = 8.9M,
                TotalNC = 13.74M,
                TotalAv = 12.34M
            });
            _mock.Add(new RelDiarioGrafico1Result()
            {
                Level1Name = "Level1 teste 2",
                NCProc = 130.8M,
                Meta = 5M,
                TotalNC = 8M,
                TotalAv = 12M
            });
            _mock.Add(new RelDiarioGrafico1Result()
            {
                Level1Name = "Level1 teste 3",
                NCProc = 20.8M,
                Meta = 8.99M,
                TotalNC = 13M,
                TotalAv = 12M
            });
            _mock.Add(new RelDiarioGrafico1Result()
            {
                Level1Name = "Level1 teste 4",
                NCProc = 2.8M,
                Meta = 3.9M,
                TotalNC = 103.74M,
                TotalAv = 210.34M
            });
           
        }
    }

    public class RelDiarioGrafico1Result
    {
        public RelDiarioGrafico1Result()
        {
        }

        public string Level1Name { get; set; }
        public decimal Meta { get; set; }
        public decimal NCProc { get; set; }
        public decimal TotalAv { get; set; }
        public decimal TotalNC { get; set; }
    }
}

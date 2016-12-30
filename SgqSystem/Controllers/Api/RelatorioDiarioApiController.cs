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
        private PanelResulPanel _mockPanelResultevel2Level3 { get; set; }

        public RelatorioDiarioApiController() {
            CriaMockGrafico1Level1();
            CriaMockLevel1();
        }

        [HttpPost]
        [Route("Grafico1")]
        public List<RelDiarioGrafico1Result> GetGrafico1Level1([FromBody] FormularioParaRelatorioViewModel form)
        {
            //var query = "";
            //using (var db = new SgqDbDevEntities())
            //{
            //    _listGrafico1Level1 = db.Database.SqlQuery<RelDiarioGrafico1Result>(query).ToList();
            //}

            return _mock;
        }

        [HttpPost]
        [Route("GetPanelResultSet")]
        public PanelResulPanel GetPanelResultSet([FromBody] FormularioParaRelatorioViewModel form)
        {
            //var query = "";
            //using (var db = new SgqDbDevEntities())
            //{
            //    _listGrafico1Level1 = db.Database.SqlQuery<RelDiarioGrafico1Result>(query).ToList();
            //}
            //var mockRetorno = _mockPanelResultevel2Level3;
            //mockRetorno.listResultSetLevel2 = _mockPanelResultevel2Level3.listResultSetLevel2.Where(r => r.level1_Id == form.level01Id).ToList();
            //mockRetorno.listResultSetLevel3 = _mockPanelResultevel2Level3.listResultSetLevel3.Where(r => r.level2_Id == form.level02Id && r.level1_Id == form.level01Id).ToList();
            return _mockPanelResultevel2Level3;
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
                TotalAv = 12.34M,
                Id = 1
            });
            _mock.Add(new RelDiarioGrafico1Result()
            {
                Level1Name = "Level1 teste 2",
                NCProc = 130.8M,
                Meta = 5M,
                TotalNC = 8M,
                TotalAv = 12M,
                Id = 2
            });
            //_mock.Add(new RelDiarioGrafico1Result()
            //{
            //    Level1Name = "Level1 teste 3",
            //    NCProc = 20.8M,
            //    Meta = 8.99M,
            //    TotalNC = 13M,
            //    TotalAv = 12M,
            //    Id = 3
            //});
            //_mock.Add(new RelDiarioGrafico1Result()
            //{
            //    Level1Name = "Level1 teste 4",
            //    NCProc = 2.8M,
            //    Meta = 3.9M,
            //    TotalNC = 103.74M,
            //    TotalAv = 210.34M,
            //    Id = 4
            //});

        }

        private void CriaMockLevel1()
        {
            _mockPanelResultevel2Level3 = new PanelResulPanel();
            _mockPanelResultevel2Level3.listResultSetLevel2 = new List<PanelResultevel2Level3>();
            _mockPanelResultevel2Level3.listResultSetLevel3PorLevel1 = new List<PanelResultevel2Level3>();
            _mockPanelResultevel2Level3.listResultSetLevel3 = new List<PanelResultevel2Level3>();

            /*Level 2*/
            _mockPanelResultevel2Level3.listResultSetLevel2.Add(new PanelResultevel2Level3()
            {
                Name = "Level2 teste 1",
                NC = 13.74M,
                Av = 12.34M,
                Id = 12,
                level1_Id = 1

            });
            _mockPanelResultevel2Level3.listResultSetLevel2.Add(new PanelResultevel2Level3()
            {
                Name = "Level2 teste 2",
                NC = 60M,
                Av = 80M,
                Id = 23,
                level1_Id = 1
            });
            _mockPanelResultevel2Level3.listResultSetLevel2.Add(new PanelResultevel2Level3()
            {
                Name = "Level2 teste 3",
                NC = 40M,
                Av = 49M,
                Id = 34,
                level1_Id = 2
            });
            _mockPanelResultevel2Level3.listResultSetLevel2.Add(new PanelResultevel2Level3()
            {
                Name = "Level2 teste 4",
                NC = 103.74M,
                Av = 210.34M,
                Id = 45,
                level1_Id = 2

            });

            /*Level 3*/
            _mockPanelResultevel2Level3.listResultSetLevel3.Add(new PanelResultevel2Level3()
            {
                Name = "Level3 teste 1",
                NC = 13.74M,
                Av = 12.34M,
                Id = 112,
                level1_Id = 1,
                level2_Id = 12
            });
            _mockPanelResultevel2Level3.listResultSetLevel3.Add(new PanelResultevel2Level3()
            {
                Name = "Level3 teste 2",
                NC = 60M,
                Av = 80M,
                Id = 123,
                level1_Id = 2,
                level2_Id = 12
            });

            _mockPanelResultevel2Level3.listResultSetLevel3.Add(new PanelResultevel2Level3()
            {
                Name = "Level3 teste 3",
                NC = 40M,
                Av = 49M,
                Id = 134,
                level1_Id = 3,
                level2_Id = 23
            });
            _mockPanelResultevel2Level3.listResultSetLevel3.Add(new PanelResultevel2Level3()
            {
                Name = "Level3 teste 4",
                NC = 103.74M,
                Av = 210.34M,
                Id = 145,
                level1_Id = 4,
                level2_Id = 23
            });

            _mockPanelResultevel2Level3.listResultSetLevel3.Add(new PanelResultevel2Level3()
            {
                Name = "Level3 teste 3",
                NC = 40M,
                Av = 49M,
                Id = 134,
                level1_Id = 3,
                level2_Id = 34
            });
            _mockPanelResultevel2Level3.listResultSetLevel3.Add(new PanelResultevel2Level3()
            {
                Name = "Level3 teste 4",
                NC = 103.74M,
                Av = 210.34M,
                Id = 145,
                level1_Id = 4,
                level2_Id = 34
            });

            _mockPanelResultevel2Level3.listResultSetLevel3.Add(new PanelResultevel2Level3()
            {
                Name = "Level3 teste 3",
                NC = 40M,
                Av = 49M,
                Id = 134,
                level1_Id = 3,
                level2_Id = 45
            });
            _mockPanelResultevel2Level3.listResultSetLevel3.Add(new PanelResultevel2Level3()
            {
                Name = "Level3 teste 4",
                NC = 103.74M,
                Av = 210.34M,
                Id = 145,
                level1_Id = 4,
                level2_Id = 45
            });
        }
    }

    public class RelDiarioGrafico1Result
    {
        public int Id { get; internal set; }
        public string Level1Name { get; set; }
        public decimal Meta { get; set; }
        public decimal NCProc { get; set; }
        public decimal TotalAv { get; set; }
        public decimal TotalNC { get; set; }
    }

    public class PanelResulPanel
    {
        public List<PanelResultevel2Level3> listResultSetLevel2 { get; set; }
        public List<PanelResultevel2Level3> listResultSetLevel3PorLevel1 { get; set; }
        public List<PanelResultevel2Level3> listResultSetLevel3 { get; set; }
    }

    public class PanelResultevel2Level3
    {
        internal int level1_Id;
        internal int level2_Id;

        public string Name { get; set; }
        public decimal Av { get; set; }
        public decimal NC { get; set; }
        public int Id { get; internal set; }
    }

}

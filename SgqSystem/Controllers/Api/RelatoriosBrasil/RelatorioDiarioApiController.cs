using Dominio;
using SgqSystem.ViewModels;
using System;
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
        private PanelResulPanel _mock { get; set; }

        public RelatorioDiarioApiController()
        {
            CriaMockGrafico1Level1();
            //CriaMockLevel1();
        }

        [HttpPost]
        [Route("Grafico1")]
        public PanelResulPanel GetGrafico1Level1([FromBody] FormularioParaRelatorioViewModel form)
        {
            //var query = "";
            //using (var db = new SgqDbDevEntities())
            //{
            //    _listGrafico1Level1 = db.Database.SqlQuery<RelDiarioGrafico1Result>(query).ToList();
            //}

            return _mock;
        }



        private void CriaMockGrafico1Level1()
        {
            _mock = new PanelResulPanel();

            _mock.listResultSetLevel1 = new List<RelDiarioResultSet>();
            _mock.listResultSetTendencia = new List<RelDiarioResultSet>();
            _mock.listResultSetLevel2 = new List<RelDiarioResultSet>();
            _mock.listResultSetTarefaPorIndicador = new List<RelDiarioResultSet>();
            _mock.listResultSetLevel3 = new List<RelDiarioResultSet>();

            /*Query 1 para Indicador*/
            _mock.listResultSetLevel1.Add(new RelDiarioResultSet()
            {
                level1_Id = 1,
                Level1Name = "Level1 - 1",
                Unidade_Id = 1,
                Unidade = "Lins",
                ProcentagemNc = 90.8M,
                Meta = 8.9M,
                NC = 13.74M,
                Av = 12.34M
            });
            _mock.listResultSetLevel1.Add(new RelDiarioResultSet()
            {
                level1_Id = 2,
                Level1Name = "Level1 - 2",
                Unidade_Id = 1,
                Unidade = "Lins",
                ProcentagemNc = 130.8M,
                Meta = 5M,
                NC = 8M,
                Av = 12M
            });

            var counter = 0;
            foreach (var i in _mock.listResultSetLevel1)
            {
                /*Query2 Para Tendencia*/
                for (int j = 0; j < 30; j++)
                {
                    if (j > 5 && j < 10)
                    {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            ProcentagemNc = 50M + counter,
                            NC = 4M + counter,
                            Av = 12M + counter,
                            Meta = 5M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                        });
                    }
                    else if (j > 16 && j < 23)
                    {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            ProcentagemNc = 50M + counter,
                            NC = 4M + counter,
                            Av = 12M + counter,
                            Meta = 5M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                            //;
                        });
                    }
                    else if (j < 28 && j > 26)
                    {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            ProcentagemNc = 50M + counter,
                            NC = 4M + counter,
                            Av = 12M + counter,
                            Meta = 5M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                        });
                    }
                    else {
                        _mock.listResultSetTendencia.Add(new RelDiarioResultSet()
                        {
                            level1_Id = i.level1_Id,
                            Level1Name = i.Level1Name,
                            Level2Name = "Tendência Level2 " + counter,
                            //ProcentagemNc = 0M + counter,
                            //NC = 0M + counter,
                            //Av = 0M + counter,
                            //Meta = 0M,
                            Unidade = i.Unidade,
                            Unidade_Id = i.Unidade_Id,
                            Data = DateTime.UtcNow.AddDays(j).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                        });
                    }
                }

                /*Query 3 para Monitoramentos do Indicador*/
                _mock.listResultSetLevel2.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter,
                    Level1Name = i.Level1Name,
                    Level2Name = "Level2 - 1" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });
                _mock.listResultSetLevel2.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter + 1,
                    Level1Name = i.Level1Name,
                    Level2Name = "Level2 - 2" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });


                /*Query 4 para Tarefas Acumuladas do Indicador*/
                _mock.listResultSetTarefaPorIndicador.Add(new RelDiarioResultSet()
                {
                    level1_Id = i.level1_Id,
                    level2_Id = counter,
                    level3_Id = counter + 12,
                    Level1Name = i.Level1Name,
                    Level2Name = "Tarefas Acumuladas " + counter,
                    Level3Name = "Tarefas Acumuladas Level3 Senhor Amado" + counter,
                    Unidade = i.Unidade,
                    Unidade_Id = i.Unidade_Id,
                    //ProcentagemNc = 50M + counter,
                    NC = 4M + counter,
                    Av = 12M + counter,
                });

                counter += 5;
            }

            counter = 0;
            foreach (var i in _mock.listResultSetLevel1)
            {
                var counter2 = 10;
                foreach (var ii in _mock.listResultSetLevel2.Where(r => r.level1_Id == i.level1_Id))
                {
                    /*Query 5 para Tarefas dos Monitoramentos dos indicadores*/
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2,
                        Level3Name = "Level3 - 1" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2 + 1,
                        Level3Name = "Level3 - 2" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    _mock.listResultSetLevel3.Add(new RelDiarioResultSet()
                    {
                        level1_Id = ii.level1_Id,
                        Level1Name = ii.Level1Name,
                        level2_Id = ii.level2_Id,
                        Level2Name = ii.Level2Name,
                        level3_Id = counter2 + 2,
                        Level3Name = "Level3 - 3" + counter2,
                        Unidade = i.Unidade,
                        Unidade_Id = i.Unidade_Id,
                        //ProcentagemNc = 50M + counter2,
                        NC = 4M + counter2,
                        //Av = 12M + counter2,
                    });
                    counter2 += counter;
                }
            }

        }
    }

    public class RelDiarioResultSet
    {
        public int level1_Id { get; set; }
        public string Level1Name { get; set; }
        public int level2_Id { get; set; }
        public string Level2Name { get; set; }
        public int level3_Id { get; set; }
        public string Level3Name { get; set; }

        public int Unidade_Id { get; set; }
        public string Unidade { get; set; }

        public decimal Meta { get; set; }
        public decimal ProcentagemNc { get; set; }
        public decimal Av { get; set; }
        public decimal NC { get; set; }
        public double Data { get; internal set; }
    }

    public class PanelResulPanel
    {
        public List<RelDiarioResultSet> listResultSetLevel1 { get; set; }
        public List<RelDiarioResultSet> listResultSetTendencia { get; set; }
        public List<RelDiarioResultSet> listResultSetLevel2 { get; set; }
        public List<RelDiarioResultSet> listResultSetTarefaPorIndicador { get; set; }
        public List<RelDiarioResultSet> listResultSetLevel3 { get; set; }
    }
}


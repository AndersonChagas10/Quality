using DTO.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/VisaoGeralDaArea")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VisaoGeralDaAreaApiController : ApiController
    {

        private List<VisaoGeralDaAreaResultSet> _mock { get; set; }

        [HttpPost]
        [Route("Grafico1")]
        public List<VisaoGeralDaAreaResultSet> Grafico1([FromBody] FormularioParaRelatorioViewModel form)
        {
            CriaMockG1();
            return _mock;
        }

        [HttpPost]
        [Route("Grafico2/{regId}")]
        public List<VisaoGeralDaAreaResultSet> Grafico2([FromBody] FormularioParaRelatorioViewModel form, int regId)
        {
            CriaMockG2();
            return _mock;
        }

        [HttpPost]
        [Route("Grafico3/{regId}")]
        public List<VisaoGeralDaAreaResultSet> Grafico3([FromBody] FormularioParaRelatorioViewModel form, int regId)
        {
            CriaMockG3(form);
            return _mock;
        }

        [HttpPost]
        [Route("Grafico4/{regId}")]
        public List<VisaoGeralDaAreaResultSet> Grafico4([FromBody] FormularioParaRelatorioViewModel form, int regId)
        {
            CriaMockG4();
            return _mock;
        }

        [HttpPost]
        [Route("Grafico5/{regId}")]
        public List<VisaoGeralDaAreaResultSet> Grafico5([FromBody] FormularioParaRelatorioViewModel form, int regId)
        {
            CriaMockG5();
            return _mock;
        }

        /// <summary>
        /// A query para o grafico1 da Visão geral da área SGQ deve retornar:
        ///   
        ///   Coluna            |   Tipagem
        ///                     |
        ///   regId             |   Int
        ///   regName           |   string
        ///   scorecardJbs      |   decimal
        ///   scorecardJbsReg   |   decimal
        ///   
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG1()
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                regId = 1,
                regName = "reg1",
                scorecardJbs = 80M,
                scorecardJbsReg = 26.5M
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                regId = 2,
                regName = "reg2",
                scorecardJbs = 80M,
                scorecardJbsReg = 66.2M
            });
        }

        /// <summary>
        /// A query para o grafico2 da Visão geral da área SGQ deve retornar:
        /// 
        ///    Coluna            |   Tipagem
        ///                      |
        ///    companySigla      |   string
        ///    companyScorecard  |   decimal
        ///    scorecardJbs      |   decimal
        ///    scorecardJbsReg   |   decimal
        /// 
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG2()
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                companySigla = "Com",
                companyScorecard = 90M,
                scorecardJbs = 80M,
                scorecardJbsReg = 66.2M
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                companySigla = "Pan",
                companyScorecard = 70M,
                scorecardJbs = 80M,
                scorecardJbsReg = 66.2M
            });

        }

        /// <summary>
        /// A query para o grafico3 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      date           |   datetime
        ///      
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        /// <param name="form"></param>
        private void CriaMockG3(FormularioParaRelatorioViewModel form)
        {
            var primeiroDiaMesAnterior = Guard.PrimeiroDiaMesAnterior(form._dataInicio);
            var proximoDomingo = Guard.GetNextWeekday(form._dataFim, DayOfWeek.Sunday);

            _mock = new List<VisaoGeralDaAreaResultSet>();
            
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
               nc = 10M,
               procentagemNc = 90M,
               date = proximoDomingo.AddDays(-8)
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 50M,
                av = 50M,
                procentagemNc = 40M,
                date = proximoDomingo.AddDays(-9)
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 20M,
                av = 150M,
                procentagemNc = 50M,
                date = proximoDomingo.AddDays(-18)
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 90M,
                av = 200M,
                procentagemNc = 90M,
                date = proximoDomingo.AddDays(-15)
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 120M,
                av = 75M,
                procentagemNc = 20M,
                date = proximoDomingo.AddDays(-22)
            });

            for (DateTime i = primeiroDiaMesAnterior;  i < proximoDomingo; i = i.AddDays(1))
            {
                if (_mock.FirstOrDefault(r => r.date == i) == null)
                {
                    _mock.Add(new VisaoGeralDaAreaResultSet()
                    {
                        nc = 0M,
                        av = 0M,
                        procentagemNc = 0M,
                        date = i
                    });

                    //_mock.Add(new VisaoGeralDaAreaResultSet());

                }
            }
            _mock = _mock.OrderBy(r => r.date).ToList();
          
        }

        /// <summary>
        /// A query para o grafico4 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      av             |   decimal
        ///      level1Name     |   string
        ///      level2Name     |   string
        ///      level1Id       |   int
        ///      level2Id       |   int
        ///      
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG4()
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 10M,
                procentagemNc = 50M,
                av = 20M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level1Id = 1,
                level2Id = 1,
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 50M,
                procentagemNc = 33.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name2",
                level1Id = 1,
                level2Id = 2,
            });

        }

        /// <summary>
        /// A query para o grafico5 da Visão geral da área SGQ deve retornar:
        /// 
        ///      Coluna         |   Tipagem
        ///                     |
        ///      nc             |   decimal
        ///      procentagemNc  |   decimal
        ///      av             |   decimal
        ///      level1Name     |   string
        ///      level2Name     |   string
        ///      level3Name     |   string
        ///      level1Id       |   int
        ///      level2Id       |   int
        ///      level3Id       |   int
        /// 
        /// Objeto a ser utilizado: VisaoGeralDaAreaResultSet
        /// </summary>
        private void CriaMockG5()
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 10M,
                procentagemNc = 50M,
                av = 20M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name1",
                level1Id = 1,
                level2Id = 1,
                level3Id = 1,
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 50M,
                procentagemNc = 33.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name2",
                level1Id = 1,
                level2Id = 1,
                level3Id = 2,
            });
            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
                nc = 102M,
                procentagemNc = 90.3M,
                av = 120M,
                level1Name = "level1 Name1",
                level2Name = "level2 Name1",
                level3Name = "level3 Name3",
                level1Id = 1,
                level2Id = 1,
                level3Id = 3,
            });

        }
    }

    public class VisaoGeralDaAreaResultSet
    {
        public DateTime date { get; set; }
        public string _date { get { return date.ToString("dd/MM/yyyy"); } }

        public decimal scorecard { get; set; }
        public int regId { get; set; }
        public string regName { get; set; }

        public string level1Name { get; set; }
        public string level2Name { get; set; }
        public string level3Name { get; set; }
        public int level1Id { get; set; }
        public int level2Id { get; set; }
        public int level3Id { get; set; }

        public decimal scorecardJbs { get; set; }
        public decimal scorecardJbsReg { get; set; }
        public string companySigla { get; set; }
        public decimal companyScorecard { get; set; }
        public decimal procentagemNc { get; set; }
        public decimal nc { get; set; }
        public decimal av { get; set; }
    }
}

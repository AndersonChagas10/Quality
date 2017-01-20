using DTO.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public List<VisaoGeralDaAreaResultSet> Grafico2(int regId)
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
            foreach (var i in _mock)
            {
            }

        }

        private void CriaMockG4()
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
             
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
               
            });

        }

        private void CriaMockG5()
        {
            _mock = new List<VisaoGeralDaAreaResultSet>();

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
              
            });

            _mock.Add(new VisaoGeralDaAreaResultSet()
            {
             
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
        public decimal scorecardJbs { get; set; }
        public decimal scorecardJbsReg { get; set; }
        public string companySigla { get; set; }
        public decimal companyScorecard { get; set; }
        public decimal procentagemNc { get; set; }
        public decimal nc { get; set; }
        public decimal av { get; set; }
    }
}

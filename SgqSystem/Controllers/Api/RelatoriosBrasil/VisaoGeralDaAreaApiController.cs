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
        [Route("Grafico2")]
        public List<VisaoGeralDaAreaResultSet> Grafico2([FromBody] FormularioParaRelatorioViewModel form)
        {
            CriaMockG2();
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
    }

    public class VisaoGeralDaAreaResultSet
    {
        public int regId { get; set; }
        public string regName { get; set; }
        public decimal scorecardJbs { get; set; }
        public decimal scorecardJbsReg { get; set; }
        
            
    }
}

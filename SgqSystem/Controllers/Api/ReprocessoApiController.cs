using ADOFactory;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Reprocesso")]
    public class ReprocessoApiController : ApiController
    {


        public class ParReprocessoHeaderOP
        {
            public int nCdOrdemProducao { get; set; }
            public int nCdEmpresa { get; set; }
            public int dLancamento { get; set; }
            public DateTime nCdUsuario { get; set; }
            public String cCdRastreabilidade { get; set; }
            public int nCdHabilitacao { get; set; }
        }

        public class ParReprocessoCertificadosSaidaOP
        {
            public int nCdOrdemProducao { get; set; }
            public int nCdCertificacao { get; set; }
        }

        public class ParReprocessoSaidaOP
        {
            public int nCdOrdemProducao { get; set; }
            public int iItem { get; set; }
            public double nCdProduto { get; set; }
            public int iQtdeValor { get; set; }
            public String cQtdeTipo { get; set; }
            public int nCdLocalEstoque { get; set; }
        }

        public class ParReprocessoEntradaOP
        {
            public int nCdOrdemProducao { get; set; }
            public DateTime dProducao { get; set; }
            public DateTime dEmbalagem { get; set; }
            public DateTime dValidade { get; set; }
            public int nCdLocalEstoque { get; set; }
            public String cCdOrgaoRegulador { get; set; }
            public String cCdRastreabilidade { get; set; }
            public int iVolume { get; set; }
            public double nPesoLiquido { get; set; }
        }

        [Route("Get")]
        [HttpGet]
        public void Get(int ParCompany_Id)
        {
            Factory factory = new Factory("DbContextSgqEUA");

            var t1 = factory.SearchQuery<ParReprocessoHeaderOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoHeaderOP") + " 489");
            var t2 = factory.SearchQuery<ParReprocessoCertificadosSaidaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoCertificadosSaidaOP"));
            var t3 = factory.SearchQuery<ParReprocessoSaidaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoSaidaOP") + " 489");
            var t4 = factory.SearchQuery<ParReprocessoEntradaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoEntradaOP") + " 489");

        }
    }
}

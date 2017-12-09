using ADOFactory;
using Dominio;
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
        public class RetrocessoReturn
        {
           public List<ParReprocessoHeaderOP> parReprocessoHeaderOPs { get; set; }
           public List<ParReprocessoCertificadosSaidaOP> parReprocessoCertificadosSaidaOP { get; set; }
           public List<ParReprocessoSaidaOP> parReprocessoSaidaOPs { get; set; }
           public List<ParReprocessoEntradaOP> parReprocessoEntradaOPs { get; set; }
        }

        public class ParReprocessoHeaderOP
        {
            public int nCdOrdemProducao { get; set; }
            public int nCdEmpresa { get; set; }
            public DateTime dLancamento { get; set; }
            public int nCdUsuario { get; set; }
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
            public Produto produto { get; set; }
        }

        public class Produto
        {

        }

        [Route("Get/{ParCompany_Id}")]
        [HttpGet]
        public RetrocessoReturn Get(int ParCompany_Id)
        {
            Factory factory = new Factory("DbContextSgqEUA");
            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities();

            

            var parCompany = sgqDbDevEntities.ParCompany.FirstOrDefault(r => r.Id == ParCompany_Id);

            if (parCompany != null)
            {
                RetrocessoReturn retrocessoReturn = new RetrocessoReturn();

                retrocessoReturn.parReprocessoHeaderOPs = factory.SearchQuery<ParReprocessoHeaderOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoHeaderOP") + " "+ parCompany.CompanyNumber);
                retrocessoReturn.parReprocessoCertificadosSaidaOP = factory.SearchQuery<ParReprocessoCertificadosSaidaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoCertificadosSaidaOP"));
                retrocessoReturn.parReprocessoSaidaOPs = factory.SearchQuery<ParReprocessoSaidaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoSaidaOP"));
                retrocessoReturn.parReprocessoEntradaOPs = factory.SearchQuery<ParReprocessoEntradaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoEntradaOP"));

                return retrocessoReturn;
            }

            return null;

        }
    }
}

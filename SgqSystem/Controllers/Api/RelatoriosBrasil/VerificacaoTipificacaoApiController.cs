using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/VerificacaoTipificacao")]
    public class VerificacaoTipificacaoApiController : ApiController
    {
        [HttpPost]
        [Route("Get")]
        public dynamic getTabela3(JObject model)
        {
            var db = new SgqDbDevEntities();

            var query = "select vp.[Id] " +
            "\n , Convert(varchar(10), vp.[AddDate], 103) as 'Data de Adição' " +
            "\n ,Convert(varchar(10), vp.[AlterDate], 103) as 'Data de Alteração' " +
            "\n ,Convert(varchar(10), CollectionDate, 103) as 'Data da Coleta' " +
            "\n ,[Sequencial]  " +
            "\n ,[Banda]  " +
            "\n ,ParCompany.Name as 'Unidade' " +
            "\n ,UserSgq.Name as 'Nome' " +
            "\n ,[cSgCaracteristica]  " +
            "\n ,[GRT_nCdCaracteristicaTipificacao]  " +
            "\n ,[JBS_nCdCaracteristicaTipificacao]  " +
            "\n ,[ResultadoComparacaoGRT_JBS]  " +
            "\n ,[cIdentificadorTipificacao]  " +
            "\n ,[cNmCaracteristica]  " +
            "\n ,[Key] " +
            "\n FROM VerificacaoTipificacaoV2 vp " +
            "\n INNER JOIN ParCompany on ParCompany.Id = vp.[ParCompany_Id] " +
            "\n INNER JOIN UserSgq on vp.UserSgq_Id = vp.UserSgq_Id";

            var retorno = GenericTable.QueryNinja(db, query);

            return retorno;
        }
    }
}

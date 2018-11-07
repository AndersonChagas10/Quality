using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Integracao
{
    [RoutePrefix("api/Integracao")]
    public class IntegracaoApiController : BaseApiController
    {
        [HttpGet]
        [Route("ConsultarCodigoBarra/{unidadeId}/{busca}")]
        public IHttpActionResult ConsultarCodigoBarra(int unidadeId, string busca)
        {
            var userName = "UserGQualidade";
            var pass = "grJsoluco3s";

            var query = "";

            try
            {
                using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
                {
                    var company = dbSgq.ParCompany.FirstOrDefault(r => r.Id == unidadeId);

                    using (var factory = new Factory(company.IPServer, company.DBServer, pass, userName))
                    {
                        query = $@"EXEC FBES_GRTInfoVolume '{busca}'";

                        return Ok(factory.QueryNinjaADO(query));
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToClient());
            }

            return null;
        }

        [HttpGet]
        [Route("ConsultarPedidoEntrada/{unidadeId}/{busca}")]
        public IHttpActionResult ConsultarPedidoEntrada(int unidadeId, string busca)
        {
            var userName = "UserGQualidade";
            var pass = "grJsoluco3s";

            var query = "";

            try
            {
                using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
                {
                    var company = dbSgq.ParCompany.FirstOrDefault(r => r.Id == unidadeId);

                    using (var factory = new Factory(company.IPServer, company.DBServer, pass, userName))
                    {
                        query = $@"EXEC COPD_GRTPedidoEntrada {busca}";

                        return Ok(factory.QueryNinjaADO(query));
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToClient());
            }

            return null;
        }
    }
}

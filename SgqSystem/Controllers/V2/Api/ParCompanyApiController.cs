using Dominio;
using SgqServiceBusiness.Controllers.RH;
using SgqSystem.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api")]
    public class ParCompanyApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParCompany
        [Route("parCompany")]
        public IHttpActionResult GetParCompany(int userSgq_Id)
        {
            VerifyIfIsAuthorized();
            InicioRequisicao();

            List<ParCompany> listaUnidadesVinculadasUsuario = new List<ParCompany>();

            ParCompanyBusiness business = new ParCompanyBusiness();
            listaUnidadesVinculadasUsuario = business.GetListaParCompany(userSgq_Id);

            return Ok(listaUnidadesVinculadasUsuario);
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

        }
    }
}

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
    public class ParClusterApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParCluster
        [Route("parCluster")]
        public IHttpActionResult GetParCluster(int parClusterGroupId, int parCompany_Id)
        {
            InicioRequisicao();
            var listaCluster = new List<ParCluster>();
            db.Configuration.LazyLoadingEnabled = false;

            ParClusterBusiness business = new ParClusterBusiness();
            listaCluster = business.GetListaParCluster(parCompany_Id, parClusterGroupId);
            return Ok(listaCluster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

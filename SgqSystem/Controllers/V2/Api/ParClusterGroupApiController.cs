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
    public class ParClusterGroupApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParCluster
        [Route("parClusterGroup")]
        public IHttpActionResult GetParClusterGroup(int parCompany_Id)
        {
            InicioRequisicao();
            var listaClusterGroup = new List<ParClusterGroup>();

            ParClusterGroupBusiness business = new ParClusterGroupBusiness();
            listaClusterGroup = business.GetListaParClusterGroup(parCompany_Id);
            
            return Ok(listaClusterGroup);
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

using Dominio;
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
        public IHttpActionResult GetParClusterGroup()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.ParClusterGroup.Where(x => x.IsActive).ToList());
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

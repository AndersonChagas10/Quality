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
    public class ParClusterApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParCluster
        [Route("parCluster")]
        public IHttpActionResult GetParCluster(int parClusterGroupId)
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.ParCluster.Where(x => x.IsActive && x.ParClusterGroup_Id == parClusterGroupId).ToList());
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

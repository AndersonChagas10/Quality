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
        public IHttpActionResult GetParCluster(int parClusterGroupId, int parCompany_Id)
        {
            var listaCluster = new List<ParCluster>();
            db.Configuration.LazyLoadingEnabled = false;

            var listaParVinculoPesoClusterIds = db.ParVinculoPeso.Where(x => x.ParCompany_Id == parCompany_Id && x.IsActive == true).Select(y => y.ParCluster_Id).Distinct().ToList();

            foreach (var item in listaParVinculoPesoClusterIds)
            {
                var clusterFiltrado = new ParCluster();
                if (item != null)
                    clusterFiltrado = db.ParCluster.Where(x => x.Id == item.Value && x.ParClusterGroup_Id == parClusterGroupId).FirstOrDefault();
                    if(clusterFiltrado != null)
                        listaCluster.Add(clusterFiltrado);
            }

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

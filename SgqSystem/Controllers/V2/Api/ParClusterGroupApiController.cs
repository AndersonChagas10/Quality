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
        public IHttpActionResult GetParClusterGroup(int parCompany_Id)
        {
            var listaClusterGroup = new List<ParClusterGroup>();
            var listaCluster = new List<ParCluster>();
            using (db)
            {
                db.Configuration.LazyLoadingEnabled = false;
                var listaParVinculoPesoClusterIds = db.ParVinculoPeso.Where(x => x.ParCompany_Id == parCompany_Id && x.IsActive == true).Select(y => y.ParCluster_Id).Distinct().ToList();

                foreach (var item in listaParVinculoPesoClusterIds)
                {
                    if(item != null)
                        listaCluster.Add(db.ParCluster.Where(x => x.Id == item.Value).FirstOrDefault());
                }

                foreach (var item in listaCluster)
                {
                    listaClusterGroup.Add(db.ParClusterGroup.Where(x => x.Id == item.ParClusterGroup_Id).FirstOrDefault());
                }
            }
            return Ok(listaClusterGroup.Distinct());
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

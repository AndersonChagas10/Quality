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
    public class ParCompanyApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParCompany
        [Route("parCompany")]
        public IHttpActionResult GetParCompany(int userSgq_Id)
        {
            using (db)
            {
                db.Configuration.LazyLoadingEnabled = false;

                var listaUnidadesVinculadasUsuario = db.ParCompanyXUserSgq
                    .Where(x => x.UserSgq_Id == userSgq_Id)
                    .Join(db.ParCompany, x => x.ParCompany_Id, pc => pc.Id, (x, pc) => new { ParCompanyXUserSgq = x, ParCompany = pc })
                    .Where(x => x.ParCompanyXUserSgq.ParCompany_Id == x.ParCompany.Id)
                    .ToList();

                return Ok(listaUnidadesVinculadasUsuario);
            }
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

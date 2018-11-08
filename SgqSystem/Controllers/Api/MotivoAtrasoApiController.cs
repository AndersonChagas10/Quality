using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/MotivoAtraso")]
    public class MotivoAtrasoApiController : BaseApiController
    {

        [HttpGet]
        [Route("Get")]
        public List<MotivoAtraso> Get()
        {
            using (var db = new SgqDbDevEntities())
            {
                return db.MotivoAtraso.Where(r => r.IsActive).ToList();
            }
        }

    }
}

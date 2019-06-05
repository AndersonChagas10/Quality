using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Http.Cors;

namespace SgqService.Controllers.Api
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/ParReason")]
    public class ParReasonApiController : BaseApiController
    {

        [HttpGet]
        [Route("Get")]
        public List<ParReason> Get()
        {
            using (var db = new SgqDbDevEntities())
            {
                return db.ParReason.Where(r => r.IsActive).ToList();
            }
        }

    }
}

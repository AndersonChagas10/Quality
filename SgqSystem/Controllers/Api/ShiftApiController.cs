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
    [RoutePrefix("api/Shift")]
    public class ShiftApiController : ApiController
    {
        [HttpGet]
        [Route("Get")]
        public List<Shift> Get()
        {
            using (var db = new SgqDbDevEntities())
            {
                return db.Shift.ToList();
            }
        }
    }
}

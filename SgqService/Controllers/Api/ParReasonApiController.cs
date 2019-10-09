using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqService.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/ParReason")]
    public class ParReasonApiController : BaseApiController
    {
        SgqServiceBusiness.Api.ParReasonApiController business;
        public ParReasonApiController()
        {
            business = new SgqServiceBusiness.Api.ParReasonApiController();
        }

        [HttpGet]
        [Route("Get")]
        public List<ParReason> Get()
        {
            return business.Get();
        }
    }
}

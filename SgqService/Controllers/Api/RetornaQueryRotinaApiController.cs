using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqService.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqService.Controllers.Api
{

    [HandleApi()]
    [RoutePrefix("api/RetornaQueryRotinaApi")]
    public class RetornaQueryRotinaApiController : BaseApiController
    {
        private SgqServiceBusiness.Controllers.Api.RetornaQueryRotinaApiController business;

        public RetornaQueryRotinaApiController()
        {
            business = new SgqServiceBusiness.Controllers.Api.RetornaQueryRotinaApiController();
        }

        [HttpPost]
        [Route("RetornaQueryRotina")]
        public Object RetornaQueryRotina(JToken body)
        {
            return business.RetornaQueryRotina(body);
        }
    }
}

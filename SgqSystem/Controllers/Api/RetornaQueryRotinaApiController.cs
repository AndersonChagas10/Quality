using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Handlres;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
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
            VerifyIfIsAuthorized();
            return business.RetornaQueryRotina(body);
        }
    }

}

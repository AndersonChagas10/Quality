using Dominio.Entities;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UrlsUtilController : ApiController
    {
        [Route("api/GetUrls")]
        public void GetUrls()
        {

            string storeLink = Url.Route("SalvarListaColeta", null);
            var teste = "aaa";
        }

    }
}

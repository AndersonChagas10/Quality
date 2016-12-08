using Dominio;
using SgqSystem.Handlres;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("GlobalConfigApi")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GlobalConfigApiController : ApiController
    {

        [HttpPost]
        [Route("AlterGC/{Id}")]
        public string AlterGC(int Id)
        {
            return GlobalConfig.AlteraGc(Id);
        }

        [HttpPost]
        [Route("CheckGC")]
        public string CheckGC()
        {
            return GlobalConfig.CheckGC();
        }

    }
}

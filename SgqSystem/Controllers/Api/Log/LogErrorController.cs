using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Log
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/LogError")]
    public class LogErrorController : ApiController
    {
        [Route("Registrar")]
        [HttpPost]
        public void Registrar([FromBody] Dominio.LogError error)
        {
            error.AddDate = System.DateTime.Now;
            LogSystem.LogErrorBusiness.SaveLogError(error);
        }
    }
}
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/NewSync")]
    public class VerificacaoTipificacaoController : ApiController
    {
        //[HttpGet]
        //[Route("AddVinculoL3L2/{idLevel2}/{idLevel3}/{peso}/{groupLevel2}")]
        //public ParLevel3Level2DTO AddVinculoL3L2(int idLevel2, int idLevel3, decimal peso, int? groupLevel2 = 0)
        //{
        //    return _paramdDomain.AddVinculoL3L2(idLevel2, idLevel3, peso, groupLevel2);
        //    //return paramsViewModel;
        //}
        //[HttpGet]
        //public HttpResponseMessage test([FromBody] string test)
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, "SGQ");
        //}

        //[Route("GetCorrectiveAction")]
        //[HttpPost]
        //public GenericReturn<List<CorrectiveActionDTO>> GetCorrectiveAction([FromBody]FormularioParaRelatorioViewModel model)
        //{
        //    return _correctiveActionAppService.GetCorrectiveAction(model);
        //}
    }
}

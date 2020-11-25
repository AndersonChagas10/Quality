using ADOFactory;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Relatorios.RH
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/AuditoriaComportamental")]
    public class AuditoriaComportamentalApiController : BaseApiController
    {

        [HttpPost]
        [Route("GetAuditoriaComportamental")]
        public List<ApontamentosDiariosResultSet> GetApontamentosDiariosRH([FromBody] DataCarrierFormularioNew form)
        {

            //var query = new ApontamentosDiariosResultSet().SelectRH(form, GetUserUnitsIds(form.ShowUserCompanies));

            //using (Factory factory = new Factory("DefaultConnection"))
            //{
            //    if (query != "")
            //        _list = factory.SearchQuery<ApontamentosDiariosResultSet>(query).ToList();

            //    return _list;
            //}
            return null;
        }

    }
}

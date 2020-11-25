using ADOFactory;
using DTO;
using DTO.ResultSet;
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
        public List<AuditoriaComportamentalResultSet> GetApontamentosDiariosRH([FromBody] DataCarrierFormularioNew form)
        {
            var _list = new List<AuditoriaComportamentalResultSet>();

            var query = new AuditoriaComportamentalResultSet().GetVisaoGeral();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                if (query != "")
                    _list = factory.SearchQuery<AuditoriaComportamentalResultSet>(query).ToList();

                return _list;
            }
        }

    }
}

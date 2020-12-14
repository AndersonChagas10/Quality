using ADOFactory;
using DTO;
using DTO.ResultSet;
using Newtonsoft.Json.Linq;
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
        public List<JObject> GetApontamentosDiariosRH([FromBody] DataCarrierFormularioNew form)
        {
            var _list = new List<JObject>();

            var query = new AuditoriaComportamentalResultSet().GetVisaoGeral(form, GetUserUnitsIds(form.ShowUserCompanies));

            using (Factory factory = new Factory("DefaultConnection"))
            {
                if (query != "")
                    _list = factory.QueryNinjaADO(query);

                return _list;
            }
        }

        [HttpPost]
        [Route("GetAuditoriaComportamentalUnidade")]
        public List<JObject> GetAuditoriaComportamentalUnidade([FromBody] DataCarrierFormularioNew form)
        {
            var _list = new List<JObject>();

            var query = new AuditoriaComportamentalResultSet().GetVisaoUnidade(form, GetUserUnitsIds(form.ShowUserCompanies));

            using (Factory factory = new Factory("DefaultConnection"))
            {
                if (query != "")
                    _list = factory.QueryNinjaADO(query);

                return _list;
            }
        }

        [HttpPost]
        [Route("GetAuditoriaComportamentalAcompanhamento")]
        public List<JObject> GetAuditoriaComportamentalAcompanhamento([FromBody] DataCarrierFormularioNew form)
        {
            var _list = new List<JObject>();

            var query = new AuditoriaComportamentalResultSet().GetVisaoAcompanhamento(form, GetUserUnitsIds(form.ShowUserCompanies));

            using (Factory factory = new Factory("DefaultConnection"))
            {
                if (query != "")
                    _list = factory.QueryNinjaADO(query);

                return _list;
            }
        }

    }
}

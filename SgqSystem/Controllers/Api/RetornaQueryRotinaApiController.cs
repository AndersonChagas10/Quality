using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Handlres;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{

    [HandleApi()]
    [RoutePrefix("api/RetornaQueryRotinaApi")]
    public class RetornaQueryRotinaApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("RetornaQueryRotina")]
        public Object RetornaQueryRotina(JToken body)
        {
            var service = new SyncServices();
            var retornoRotinaNinja = new Object();

            Rotina myDeserializedObjList = (Rotina)Newtonsoft.Json.JsonConvert.DeserializeObject(body.ToString(), typeof(Rotina));

            var retorno = service.RetornaQueryRotina(myDeserializedObjList.IdRotina, myDeserializedObjList.Params);
            if (!string.IsNullOrEmpty(retorno))
            {
                using (Factory factory = new Factory("defaultconnection"))
                {
                    retornoRotinaNinja = QueryNinja(db, retorno).FirstOrDefault();
                }
            }

            return retornoRotinaNinja;

        }

        class Rotina
        {
            public string IdRotina { get; set; }
            public List<Dictionary<string, string>> Params { get; set; }
        }
    }
}

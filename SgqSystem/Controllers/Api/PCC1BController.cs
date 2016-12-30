using Dominio;
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

    public partial class TestePCC1B
    {
        public int Sequential { get; set; }
        public int Side { get; set; }

    };

    [HandleApi()]
    [RoutePrefix("api/PCC1B")]
    public class PCC1BController : ApiController
    {
        private List<TestePCC1B> _list { get; set; }

        [HttpPost]
        [Route("Next")]
        public List<TestePCC1B> Save(Object dataAtual)
        {

            using (var db = new SGQ_GlobalEntities())
            {

                string query = "SELECT Max(Sequential) AS Sequencial, Max(Side) AS Side from CollectionLevel2 " +
                    "WHERE ParLevel1_Id=3 and ParLevel2_Id in (66,67) AND UnitId=1 AND CollectionDate ";
                    //"BETWEEN '"+ dataAtual.ToString("yyyyMMdd") + " 00:00:00' AND '"+ dataAtual.ToString("yyyyMMdd") + " 23:59:59'";

                _list = db.Database.SqlQuery<TestePCC1B>(query).ToList();
            }
            
            return _list;
        }
    }
}

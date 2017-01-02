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

    public partial class _PCC1B
    {
        public int Sequential { get; set; }
        public int Side { get; set; }
    };

    public partial class _Receive
    {
        public String Data { get; set; }
        public int Unit { get; set; }
        public int ParLevel2 { get; set; }
    }

    [HandleApi()]
    [RoutePrefix("api/PCC1B")]
    public class PCC1BController : ApiController
    {
        private _PCC1B pcc1b { get; set; }

        [HttpPost]
        [Route("Next")]
        public _PCC1B Next(_Receive receive)
        {

            List<_PCC1B> _list = new List<_PCC1B>();

            using (var db = new SgqDbDevEntities())
            {

                string query = "SELECT IsNull(Max(Sequential), 0) AS Sequential, 0 AS Side from CollectionLevel2 " +
                    "WHERE ParLevel1_Id=3 and ParLevel2_Id = "+ receive.ParLevel2 + " AND UnitId="+receive.Unit+" AND CollectionDate "+
                    "BETWEEN '"+ receive.Data + " 00:00:00' AND '"+ receive.Data + " 23:59:59'";

                _list = db.Database.SqlQuery<_PCC1B>(query).ToList();

                if (_list.Count > 0)
                {
                    pcc1b = _list[0];

                    query = "SELECT IsNull(Max(Sequential), 0) AS Sequential, IsNull(Max(Side), 0) AS Side from CollectionLevel2 " +
                    "WHERE ParLevel1_Id=3 and ParLevel2_Id = " + receive.ParLevel2 + " AND UnitId=" + receive.Unit + " AND CollectionDate " +
                    "BETWEEN '" + receive.Data + " 00:00:00' AND '" + receive.Data + " 23:59:59' AND Sequential = "+pcc1b.Sequential;

                    _list = db.Database.SqlQuery<_PCC1B>(query).ToList();

                    pcc1b = _list[0];
                }
            }

            return pcc1b;
        }
    }
}

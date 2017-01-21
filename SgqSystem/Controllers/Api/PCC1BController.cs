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

                string querySequencial =   
                        "SELECT                                                                                             " +
                        "\n IsNull(Max(Sequential), 0) AS Sequential,                                                       " +
                        "\n 0 AS Side from CollectionLevel2                                                                 " +
                        "\n WHERE ParLevel1_Id=3 and ParLevel2_Id = "+ receive.ParLevel2 + " AND                            " +
                        "\n UnitId="+receive.Unit+" AND                                                                     " +
                        "\n CollectionDate BETWEEN '" + receive.Data + " 00:00:00' AND '"+ receive.Data + " 23:59:59'       ";

                _list = db.Database.SqlQuery<_PCC1B>(querySequencial).ToList();

                if (_list.Count > 0)
                {
                    pcc1b = _list[0];

                    string queryBanda = 
                        "SELECT                                                                                             " +
                        "\n IsNull(Max(Sequential), 0) AS Sequential,                                                       " +
                        "\n IsNull(Max(Side), 0) AS Side from CollectionLevel2                                              " +
                        "\n WHERE ParLevel1_Id=3 and ParLevel2_Id = " + receive.ParLevel2 + "                               " +
                        "\n AND UnitId=" + receive.Unit + " AND CollectionDate                                              " +
                        "\n BETWEEN '" + receive.Data + " 00:00:00' AND '" + receive.Data + " 23:59:59'                     "+
                        "\n AND Sequential = " + pcc1b.Sequential+"                                                         ";

                    _list = db.Database.SqlQuery<_PCC1B>(queryBanda).ToList();

                    pcc1b = _list[0];
                }
            }

            return pcc1b;
        }
    }
}

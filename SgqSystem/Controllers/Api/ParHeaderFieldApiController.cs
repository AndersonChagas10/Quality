using Dominio;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/ParHeader")]
    public class ParHeaderFieldApiController : ApiController
    {
        public partial class CollectionHeaderField
        {
            public int ParLevel1_Id { get; set; }
            public int ParLevel2_Id { get; set; }
            public int Period { get; set; }
            public int Shift { get; set; }
            public int ParHeaderField_Id { get; set; }
            public int Evaluation { get; set; }
            public int Sample { get; set; }
            public string Value { get; set; }
        }

        [HttpGet]
        [Route("GetList/{unitId}/{date}")]
        public IEnumerable<CollectionHeaderField> GetListCollectionHeaderField(int UnitId, String Date)
        {
            var TransformedDate = CommonDate.TransformDateFormatToAnother(
                                                    Date, "MMddyyyy", "yyyy-MM-dd");

            var SelectQuery =
                "SELECT                                                                        " +
                "C.ParLevel1_Id AS ParLevel1_Id,                                               " +
                "C.ParLevel2_Id AS ParLevel2_Id,                                               " +
                "C.Period AS Period,                                                           " +
                "C.Shift AS Shift,                                                             " +
                "CP.ParHeaderField_Id AS ParHeaderField_Id,                                    " +
                "CP.Value AS Value,                                                            " +
                "C.EvaluationNumber AS Evaluation,                                             " +
                "C.Sample AS Sample                                                            " +
                "FROM CollectionLevel2XParHeaderField CP                                       " +
                "LEFT JOIN CollectionLevel2 C ON C.Id = CP.CollectionLevel2_Id                 " +
                "LEFT JOIN ParHeaderField PH ON CP.ParHeaderField_Id = PH.Id                   " +
                "AND PH.LinkNumberEvaluetion = 1                                               " +
                "WHERE C.UnitId = " + UnitId + " AND                                           " +
                "C.CollectionDate BETWEEN '" + TransformedDate + " 00:00' AND                  " +
                "'" + TransformedDate + " 23:59'";

            using (var context = new SgqDbDevEntities())
            {
                return context.Database.SqlQuery<CollectionHeaderField>(SelectQuery).ToList();
            }

        }
    }
}

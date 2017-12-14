using Dominio;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public partial class ParMultipleValuesXParCompany
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
        [Route("GetCollectionLevel2XHeaderField/{unitId}/{date}")]
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
                "FROM CollectionLevel2XParHeaderField CP (NOLOCK)                              " +
                "LEFT JOIN CollectionLevel2 C (NOLOCK) ON C.Id = CP.CollectionLevel2_Id        " +
                "LEFT JOIN ParHeaderField PH (NOLOCK) ON CP.ParHeaderField_Id = PH.Id          " +
                "AND PH.LinkNumberEvaluetion = 1                                               " +
                "WHERE C.UnitId = " + UnitId + " AND                                           " +
                "C.CollectionDate BETWEEN '" + TransformedDate + " 00:00' AND                  " +
                "'" + TransformedDate + " 23:59'";

            using (var context = new SgqDbDevEntities())
            {
                return context.Database.SqlQuery<CollectionHeaderField>(SelectQuery).ToList();
            }

        }

        [HttpGet]
        [Route("GetListParMultipleValuesXParCompany/{unitId}")]
        public IEnumerable<ParMultipleValuesXParCompany> GetListParMultipleValuesXParCompany(int UnitId, String Date)
        {
            var SelectQuery =
                @"SELECT 
                PP.ParCompany_Id, 
                PP.ParMultipleValues_Id, 
                PP.HashKey, 
                PP.ParHeaderField_Id, 
                PP.Parent_ParMultipleValues_Id, 
                P.Name 
                FROM ParMultipleValuesXParCompany PP 
                LEFT JOIN ParMultipleValues P on P.Id = PP.ParMultipleValues_Id
                WHERE PP.IsActive = 1 and ParCompany_Id = " +  UnitId ;

            using (var context = new SgqDbDevEntities())
            {
                return context.Database.SqlQuery<ParMultipleValuesXParCompany>(SelectQuery).ToList();
            }

        }
    }
}

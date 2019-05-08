using ADOFactory;
using Dominio;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/ParHeader")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ParHeaderFieldApiController : BaseApiController
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
            VerifyIfIsAuthorized();

            var TransformedDate = CommonDate.TransformDateFormatToAnother(
                                                    Date, "MMddyyyy", "yyyy-MM-dd");

            var sql =
                "SELECT                                                                        " +
                "IIF(CC.ParCluster_Id is null, C.ParLevel1_Id, CONCAT(CC.ParCluster_Id,'98789', C.ParLevel1_Id)) AS ParLevel1_Id," +
                "IIF(CC.ParCluster_Id is null, C.ParLevel2_Id, CONCAT(CC.ParCluster_Id,'98789', C.ParLevel2_Id)) AS ParLevel2_Id," +
                "C.Period AS Period,                                                           " +
                "C.Shift AS Shift,                                                             " +
                "CP.ParHeaderField_Id AS ParHeaderField_Id,                                    " +
                "CP.Value AS Value,                                                            " +
                "C.EvaluationNumber AS Evaluation,                                             " +
                "C.Sample AS Sample                                                            " +
                "FROM CollectionLevel2XParHeaderField CP (NOLOCK)                              " +
                "LEFT JOIN CollectionLevel2 C (NOLOCK) ON C.Id = CP.CollectionLevel2_Id        " +
                "LEFT JOIN CollectionLevel2XCluster CC (NOLOCK) ON CC.CollectionLevel2_Id = C.Id " +
                "LEFT JOIN ParHeaderField PH (NOLOCK) ON CP.ParHeaderField_Id = PH.Id          " +
                "AND PH.LinkNumberEvaluetion = 1                                               " +
                "WHERE C.UnitId = " + UnitId + " AND                                           " +
                "C.CollectionDate BETWEEN '" + TransformedDate + " 00:00' AND                  " +
                "'" + TransformedDate + " 23:59:59'";

            List<CollectionHeaderField> Lista1 = new List<CollectionHeaderField>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                Lista1 = factory.SearchQuery<CollectionHeaderField>(sql);
            }


            return Lista1;
        }

        [HttpGet]
        [Route("GetListParMultipleValuesXParCompany/{unitId}/{level1_id}")]
        public IEnumerable<ParMultipleValuesXParCompany> GetListParMultipleValuesXParCompany(int UnitId, string level1_id)
        {
            VerifyIfIsAuthorized();
            return GetListParMultipleValuesXParCompany(UnitId);

        }

        [HttpGet]
        [Route("GetListParMultipleValuesXParCompany/{unitId}")]
        public IEnumerable<ParMultipleValuesXParCompany> GetListParMultipleValuesXParCompany(int UnitId)
        {
            VerifyIfIsAuthorized();

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
                WHERE PP.IsActive = 1 and ParCompany_Id = " + UnitId;

            using (Factory factory = new Factory("DefaultConnection"))
            {
                return factory.SearchQuery<ParMultipleValuesXParCompany>(SelectQuery).ToList();
            }

        }
    }
}

using ADOFactory;
using Dominio;
using ServiceModel;
using SgqServiceBusiness.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SgqServiceBusiness.Api
{
    public class ParHeaderFieldApiController
    {

        public IEnumerable<CollectionHeaderField> GetListCollectionHeaderField(int UnitId, String Date)
        {
            var TransformedDate = CommonDate.TransformDateFormatToAnother(
                                                    Date, "MMddyyyy", "yyyy-MM-dd");

            var sql = $@"SELECT Iif(CC.parcluster_id IS NULL, Cast(C.parlevel1_id AS BIGINT), 
              Concat(CC.parcluster_id, '98789', C.parlevel1_id)) AS ParLevel1_Id 
       , 
       Iif(CC.parcluster_id IS NULL, Cast(C.parlevel2_id AS BIGINT), 
       Concat(CC.parcluster_id, '98789', C.parlevel2_id))        AS ParLevel2_Id 
       , 
       C.period                                                  AS 
       Period, 
       C.shift                                                   AS Shift, 
       CP.parheaderfield_id                                      AS 
       ParHeaderField_Id, 
       CP.value                                                  AS Value, 
       C.evaluationnumber                                        AS Evaluation, 
       C.sample                                                  AS Sample 
FROM  collectionlevel2 C (nolock) 
       CROSS APPLY (select parheaderfield_id, value 
						from 
							(select top 200000 parheaderfield_id, value, collectionlevel2_id 
							from collectionlevel2xparheaderfield  (nolock)
							order by id desc) TEMPCP
						WHERE C.id = collectionlevel2_id ) CP
       LEFT JOIN collectionlevel2xcluster CC (nolock) 
              ON CC.collectionlevel2_id = C.id 
       LEFT JOIN parheaderfield PH (nolock) 
              ON CP.parheaderfield_id = PH.id 
AND PH.LinkNumberEvaluetion = 1                             
WHERE C.UnitId = { UnitId } AND                         
C.CollectionDate BETWEEN '{ TransformedDate } 00:00' AND
'{ TransformedDate } 23:59:59'";

            List<CollectionHeaderField> Lista1 = new List<CollectionHeaderField>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                Lista1 = factory.SearchQuery<CollectionHeaderField>(sql);
            }


            return Lista1;
        }

        public IEnumerable<ParMultipleValuesXParCompany> GetListParMultipleValuesXParCompany(int UnitId, string level1_id)
        {
            return GetListParMultipleValuesXParCompany(UnitId);

        }

        public IEnumerable<ParMultipleValuesXParCompany> GetListParMultipleValuesXParCompany(int UnitId)
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
                WHERE PP.IsActive = 1 and ParCompany_Id = " + UnitId;

            using (Factory factory = new Factory("DefaultConnection"))
            {
                return factory.SearchQuery<ParMultipleValuesXParCompany>(SelectQuery).ToList();
            }
        }
    }
}

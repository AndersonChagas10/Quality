using ADOFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Services.SyncModel
{
    public class ParLevel3XParDepartmentSyncModel
    {
        public int Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int ParLevel3_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public int ParDepartment_Id { get; set; }
        public string ParDepartment_Name { get; set; }

        public ParLevel3XParDepartmentSyncModel()
        {
        }

        public IEnumerable<ParLevel3XParDepartmentSyncModel> GetList(int parCompany_Id)
        {

            //string sql = $@"SELECT pl3pd.ParDepartment_Id, 
            //    pd.Name as ParDepartment_Name,  
            //    pl3pd.ParLevel1_Id,
            //    pl3pd.ParLevel2_Id,
            //    pl3pd.ParLevel3_Id,
            //    pl3pd.ParCompany_Id
            //    FROM ParLevel3XParDepartment pl3pd
            //    LEFT JOIN ParDepartment pd ON pd.Id = pl3pd.ParDepartment_Id
            //    WHERE pl3pd.IsActive = 1 AND pl3pd.ParCompany_Id = {parCompany_Id}
            //    GROUP BY pl3pd.ParDepartment_Id, 
            //    pd.Name,  
            //    pl3pd.ParLevel1_Id,
            //    pl3pd.ParLevel2_Id,
            //    pl3pd.ParLevel3_Id,
            //    pl3pd.ParCompany_Id";

            string sql = $@"SELECT DISTINCT
                       	pl3pd.ParDepartment_Id
                          ,pd.Name AS ParDepartment_Name
                          ,pl3pd.ParLevel1_Id
                          ,pl3pd.ParLevel2_Id
                          ,pl3pd.ParLevel3_Id
                          ,{ parCompany_Id } as ParCompany_Id
                       FROM ParLevel3XParDepartment pl3pd
                       LEFT JOIN ParDepartment pd
                       	ON pd.Id = pl3pd.ParDepartment_Id
                       WHERE pl3pd.IsActive = 1
                       AND (pl3pd.ParCompany_Id = 8
                       OR pl3pd.ParCompany_Id IS NULL)
                       GROUP BY pl3pd.ParDepartment_Id
                       		,pd.Name
                       		,pl3pd.ParLevel1_Id
                       		,pl3pd.ParLevel2_Id
                       		,pl3pd.ParLevel3_Id
                       		,pl3pd.ParCompany_Id";

            List<ParLevel3XParDepartmentSyncModel> listParLevel3XParDepartmentSyncModel = new List<ParLevel3XParDepartmentSyncModel>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                listParLevel3XParDepartmentSyncModel = factory.SearchQuery<ParLevel3XParDepartmentSyncModel>(sql);
            }

            return listParLevel3XParDepartmentSyncModel;
        }

        public string ToHtml(int parCompany_Id)
        {
            return $@"<script>
                    var listaParLevel3XParDepartment = " + System.Web.Helpers.Json.Encode(this.GetList(parCompany_Id)) + @";
                    </script> ";
        }
    }
}
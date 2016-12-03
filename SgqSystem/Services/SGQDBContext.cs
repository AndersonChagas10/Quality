using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace SGQDBContext
{
    public partial class ParLevel1
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public int ParCriticalLevel_Id { get; set; }
        public string ParCriticalLevel_Name { get; set; }
        public bool HasSaveLevel2 { get; set; }
        public bool HasNoApplicableLevel2 { get; set; }
        public ParLevel1()
        {

        }

        //public List<ParLevel1> getList()
        //{

        //    string sql = " SELECT P1.Id, P1.Name, CL.Id AS ParCriticalLevel_Id, CL.Name AS ParCriticalLevel_Name      " +
        //                 " FROM ParLevel1 P1                                                                                          " +
        //                 " INNER JOIN ParLevel1XCluster P1C                                                                           " +
        //                 " ON P1C.ParLevel1_Id = P1.Id                                                                                " +
        //                 " INNER JOIN ParCluster C                                                                                    " +
        //                 " ON C.Id = P1C.ParCluster_Id                                                                                " +
        //                 " INNER JOIN ParCompanyCluster CC                                                                            " +
        //                 " ON CC.ParCluster_Id = P1C.ParCluster_Id                                                                    " +
        //                 " INNER JOIN ParCriticalLevel CL                                                                             " +
        //                 " ON CL.Id = P1C.ParCriticalLevel_Id                                                                         " +
        //                 " WHERE CC.ParCompany_Id = 1                                                                                 " +
        //                 " order by CL.Name                                                                                           ";

        //    string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        //    using (SqlConnection connection = new SqlConnection(conexao))
        //    {
        //        using (SqlCommand command = new SqlCommand(sql, connection))
        //        {
        //            connection.Open();
        //            using (SqlDataReader r = command.ExecuteReader())
        //            {
        //                var parLevel1List = new List<ParLevel1>();
        //                while (r.Read())
        //                {
        //                    var parLevel1 = new ParLevel1();
        //                    parLevel1.Id = Convert.ToInt32(r[0]);
        //                    parLevel1.Name = r[1].ToString();
        //                    parLevel1.ParCriticalLevel_Id = Convert.ToInt32(r[2]);
        //                    parLevel1.ParCriticalLevel_Name = r[3].ToString();

        //                    parLevel1List.Add(parLevel1);
        //                }
        //                return parLevel1List;
        //            }
        //        }
        //    }
        //}

        public IEnumerable<ParLevel1> getParLevel1ParCriticalLevelList(string ParCompany_Id)
        {

            SqlConnection db = new SqlConnection(conexao);
            string sql = " SELECT P1.Id, P1.Name, CL.Id AS ParCriticalLevel_Id, CL.Name AS ParCriticalLevel_Name, P1.HasSaveLevel2 AS HasSaveLevel2,  " +
                         " P1.HasNoApplicableLevel2 AS HasNoApplicableLevel2                                                                          " +
                         " FROM ParLevel1 P1                                                                                                          " +
                         " INNER JOIN ParLevel1XCluster P1C                                                                                           " +
                         " ON P1C.ParLevel1_Id = P1.Id                                                                                                " +
                         " INNER JOIN ParCluster C                                                                                                    " +
                         " ON C.Id = P1C.ParCluster_Id                                                                                                " +
                         " INNER JOIN ParCompanyCluster CC                                                                                            " +
                         " ON CC.ParCluster_Id = P1C.ParCluster_Id                                                                                    " +
                         " INNER JOIN ParCriticalLevel CL                                                                                             " +
                         " ON CL.Id = P1C.ParCriticalLevel_Id                                                                                         " +
                         " WHERE CC.ParCompany_Id = '" + ParCompany_Id + "'                                                                           " +
                         " ORDER BY CL.Name                                                                                                           ";

            //var parLevel1List = (List<ParLevel1>)db.Query<ParLevel1>(sql);

            var parLevel1List = db.Query<ParLevel1>(sql);

            return parLevel1List;
        }
    }

}
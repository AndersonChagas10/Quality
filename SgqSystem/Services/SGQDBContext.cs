using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System;

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


        public IEnumerable<ParLevel1> getParLevel1ParCriticalLevelList(int ParCompany_Id)
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
                         " AND P1.IsActive = 1                                                                                                        " +
                         " ORDER BY CL.Name                                                                                                           ";

            //var parLevel1List = (List<ParLevel1>)db.Query<ParLevel1>(sql);

            var parLevel1List = db.Query<ParLevel1>(sql);

            return parLevel1List;
        }
    }
    public partial class ParLevel2
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
        public int Id { get; set; }
        public string Name { get; set; }
        //public int? Evaluate { get; set; }
        //public int? Sample { get; set; }
        //public int? ParCompany_Id_Evaluate { get; set; }
        //public int? ParCompany_Id_Sample { get; set; }
        public ParLevel2()
        {

        }
        public IEnumerable<ParLevel2> getLevel2ByIdLevel1(int ParLevel1_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name                        " +
                         "FROM ParLevel3Level2 P32                                     " +
                         "INNER JOIN ParLevel3Level2Level1 P321                        " +
                         "ON P321.ParLevel3Level2_Id = P32.Id                          " +
                         "INNER JOIN ParLevel2 PL2                                     " +
                         "ON PL2.Id = P32.ParLevel2_Id                                 " +
                         "WHERE P321.ParLevel1_Id = '" + ParLevel1_Id + "'             " +
                         " AND PL2.IsActive = 1                                        " +          
                         "GROUP BY PL2.Id, PL2.Name                                    ";

            var parLevel2List = db.Query<ParLevel2>(sql);

            return parLevel2List;


        }
    }
    public partial class ParLevel2Evaluate
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name {get;set;}
        public int Evaluate { get; set; }
        //public int? ParCompany_Id { get; set; }

        public IEnumerable<ParLevel2Evaluate> getEvaluate(ParLevel1 ParLevel1, int? ParCompany_Id)
        {

            SqlConnection db = new SqlConnection(conexao);
            string queryCompany = null;
            if(ParCompany_Id > 0)
            {
                queryCompany = " AND PE.ParCompany_Id = '" + ParCompany_Id + "'";
            }

            string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name, PE.Number AS Evaluate                " +
                         "FROM                                                                        " +
                         "ParLevel3Level2 P32                                                         " +
                         "INNER JOIN ParLevel3Level2Level1 P321                                       " +
                         "ON P321.ParLevel3Level2_Id = P32.Id                                         " +
                         "INNER JOIN ParLevel2 PL2                                                    " +
                         "ON PL2.Id = P32.ParLevel2_Id                                                " +
                         "INNER JOIN ParEvaluation PE                                                 " +
                         "ON PE.ParLevel2_Id = PL2.Id                                                 " +
                         "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id  +"'                            " + 
                         queryCompany                                                                   +
                         "GROUP BY PL2.Id, PL2.Name, PE.Number                                        ";

            var parEvaluate = db.Query<ParLevel2Evaluate>(sql);

            return parEvaluate;
        }



    }
    public partial class ParLevel2Sample
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Sample { get; set; }
        //public int? ParCompany_Id { get; set; }

        public IEnumerable<ParLevel2Sample> getSample(ParLevel1 ParLevel1, int? ParCompany_Id)
        {

            SqlConnection db = new SqlConnection(conexao);
            string queryCompany = null;
            if (ParCompany_Id > 0)
            {
                queryCompany = " AND PS.ParCompany_Id = '" + ParCompany_Id + "'";
            }

            string sql = "SELECT PL2.Id AS Id, PL2.Name AS Name, PS.Number AS Sample FROM  " +
                         "ParLevel3Level2 P32                                              " +
                         "INNER JOIN ParLevel3Level2Level1 P321                            " +
                         "ON P321.ParLevel3Level2_Id = P32.Id                              " +
                         "INNER JOIN ParLevel2 PL2                                         " +
                         "ON PL2.Id = P32.ParLevel2_Id                                     " +
                         "INNER JOIN ParSample PS                                          " +
                         "ON PS.ParLevel2_Id = PL2.Id                                      " +
                         "WHERE P321.ParLevel1_Id = '" + ParLevel1.Id + "'                 " +
                         queryCompany                                                        +
                         "GROUP BY PL2.Id, PL2.Name, PS.Number, PS.ParCompany_Id           ";

            var parSample = db.Query<ParLevel2Sample>(sql);

            return parSample;
        }



    }
    public partial class ParLevel3
    {
        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

        public int Id { get; set; }
        public string Name { get; set; }
        public int ParLevel3Group_Id { get; set; }
        public string ParLevel3Group_Name { get; set; }
        public int ParLevel3InputType_Id { get; set; }
        public string ParLevel3InputType_Name { get; set; }
        public int ParLevel3BoolFalse_Id { get; set; }
        public string ParLevel3BoolFalse_Name { get; set; }
        public int ParLevel3BoolTrue_Id { get; set; }
        public string ParLevel3BoolTrue_Name { get; set; }
        public decimal IntervalMin { get; set; }
        public decimal IntervalMax { get; set; }
        public string ParMeasurementUnit_Name { get; set; }
        public decimal Weight { get; set; }

        public IEnumerable<ParLevel3> getList()
        {
            SqlConnection db = new SqlConnection(conexao);
            string sql = "SELECT Id, Name FROM ParLevel3";
            var parLevel3List = db.Query<ParLevel3>(sql);

            return parLevel3List;

        }
        public IEnumerable<ParLevel3> getLevel3ByLevel2(int ParLevel2_Id)
        {
            SqlConnection db = new SqlConnection(conexao);

            string sql = "SELECT L3.Id AS Id, L3.Name AS Name, L3G.Id AS ParLevel3Group_Id, L3G.Name AS ParLevel3Group_Name, L3IT.Id AS ParLevel3InputType_Id, L3IT.Name AS ParLevel3InputType_Name, L3V.ParLevel3BoolFalse_Id AS ParLevel3BoolFalse_Id, L3BF.Name AS ParLevel3BoolFalse_Name, L3V.ParLevel3BoolTrue_Id AS ParLevel3BoolTrue_Id, L3BT.Name AS ParLevel3BoolTrue_Name, " +
                         "L3V.IntervalMin AS IntervalMin, L3V.IntervalMax AS IntervalMax, MU.Name AS ParMeasurementUnit_Name, L32.Weight AS Weight                                                                                                                                                                                                                                    " +
                         "FROM ParLevel3 L3                                                                                                                                                                                                                                                                                                                                           " +
                         "INNER JOIN ParLevel3Value L3V                                                                                                                                                                                                                                                                                                                               " +
                         "        ON L3V.ParLevel3_Id = L3.Id                                                                                                                                                                                                                                                                                                                         " +
                         "INNER JOIN ParLevel3InputType L3IT                                                                                                                                                                                                                                                                                                                          " +
                         "        ON L3IT.Id = L3V.ParLevel3InputType_Id                                                                                                                                                                                                                                                                                                              " +
                         "LEFT JOIN ParLevel3BoolFalse L3BF                                                                                                                                                                                                                                                                                                                           " +
                         "        ON L3BF.Id = L3V.ParLevel3BoolFalse_Id                                                                                                                                                                                                                                                                                                              " +
                         "LEFT JOIN ParLevel3BoolTrue L3BT                                                                                                                                                                                                                                                                                                                            " +
                         "        ON L3BT.Id = L3V.ParLevel3BoolTrue_Id                                                                                                                                                                                                                                                                                                               " +
                         "LEFT JOIN ParMeasurementUnit MU                                                                                                                                                                                                                                                                                                                             " +
                         "        ON MU.Id = L3V.ParMeasurementUnit_Id                                                                                                                                                                                                                                                                                                                " +
                         "LEFT JOIN ParLevel3Level2 L32                                                                                                                                                                                                                                                                                                                               " +
                         "        ON L32.ParLevel3_Id = L3.Id                                                                                                                                                                                                                                                                                                                         " +
                         "LEFT JOIN ParLevel3Group L3G                                                                                                                                                                                                                                                                                                                                " +
                         "        ON L3G.Id = L32.ParLevel3Group_Id                                                                                                                                                                                                                                                                                                                   " +
                         "INNER JOIN ParLevel2 L2                                                                                                                                                                                                                                                                                                                                     " +
                         "        ON L2.Id = L32.ParLevel2_Id                                                                                                                                                                                                                                                                                                                         " +
                         "                                                                                                                                                                                                                                                                                                                                                            " +
                         "WHERE  L3.IsActive = 1                                                                                                                                                                                                                                                                                                                                                     " +
                         " AND L2.Id = '" +  ParLevel2_Id + "'                                                                                                                                                                                                                                                                                                                             ";

            var parLevel3List = db.Query<ParLevel3>(sql);

            return parLevel3List;
        }                                                                                                                                                                                                                                                                                                                                                                          
    }
 
    public partial class Level2Result
    {
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int Unit_Id { get; set; }
        public int Shift { get; set; }
        public int Period { get; set; }

        public DateTime CollectionDate { get; set; }

        public int EvaluateLast { get; set; }
        public int SampleLast { get; set; }

        public IEnumerable<Level2Result> getList(string UnidadeId)
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;

            SqlConnection db = new SqlConnection(conexao);
            string sql = "SELECT                                                           " +
                        "   ParLevel1_Id                                                   " +
                        "    , ParLevel2_Id                                                " +
                        "    , UnitId      AS Unit_Id                                      " +
                        "    , Shift                                                       " +
                        "    , Period                                                      " +
                        "    , CollectionDate                                              " +
                        "    ,max(EvaluationNumber) as EvaluateLast                        " +
                        "    ,max(Sample) as SampleLast                                    " +
                        "   FROM                                                           " +
                        "   (                                                              " +
                        "    SELECT                                                        " +
                        "                                                                  " +
                        "    ParLevel1_Id                                                  " +
                        "    , ParLevel2_Id                                                " +
                        "    , UnitId                                                      " +
                        "    , Shift                                                       " +
                        "    , Period                                                      " +
                        "    , convert(date, CollectionDate) as CollectionDate             " +
                        "    , EvaluationNumber                                            " +
                        "    , max(Sample) as Sample                                       " +
                        "                                                                  " +
                        "    FROM CollectionLevel2                                         " +
                        "                                                                  " +
                        "    where UnitId = '" + UnidadeId + "'                            " +
                        "                                                                  " +
                        "    group by                                                      " +
                        "    ParLevel1_Id                                                  " +
                        "    , ParLevel2_Id                                                " +
                        "    , UnitId                                                      " +
                        "    , Shift                                                       " +
                        "    , Period                                                      " +
                        "    , convert(date, CollectionDate)                               " +
                        "    , EvaluationNumber                                            " +
                        "   ) ultimas_amostras                                             " +
                        "                                                                  " +
                        "   group by                                                       " +
                        "   ParLevel1_Id                                                   " +
                        "   , ParLevel2_Id                                                 " +
                        "   , UnitId                                                       " +
                        "   , Shift                                                        " +
                        "   , Period                                                       " +
                        "   , CollectionDate                                               ";
           
            var Level2ResultList = db.Query<Level2Result>(sql);

            return Level2ResultList;

        }
    }   
}
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.NewSync
{
    [HandleApi()]
    [RoutePrefix("api/NewSync")]
    public class NewSyncController : ApiController
    {
    

        [HttpGet]
        [Route("Sync2")]
        public string Sync2()
        {
            string sql = "SELECT * FROM CollectionLevel02";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                int id = Convert.ToInt32(r[0]);

                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw;
            }
          
            return null;
        }
        public HttpResponseMessage test([FromBody] string test)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "SGQ");
        }
        [HttpGet]
        [Route("Insert2")]
        public string Insert2()
        {
            string sql = "SELECT * FROM CollectionLevel02";
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        int i = (int)command.ExecuteScalar();
                        if (i > 0)
                        {
                           
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }
        [HttpPost]
        //[Route("InsertJson/{obj}/{collectionDate}/{level01Id}/{level02Id}/{unitId}/{period}/{shift}/{device}/{version}/{ambient}")]
        //public string InsertJson(string obj, string collectionDate,string level01Id, string level02Id ,string unitId ,string period ,string shift ,string device ,string version ,string ambient)
        [Route("InsertJson")]
        //public string GenericReturn<CollectionJson> InsertJson([FromBody] SyncViewModel objToSync)
        public string InsertJson(string obj)
        {

            return null;
           //string obj, string collectionDate, string level01id, string unit, string period, string shift, string device, string version
           // string obj = "123";
            //collectionDate = Convert.ToDateTime(collectionDate).ToString("yyyy-MM-dd HH:mm:ss");
            //string level01id = "1";
            //string unit = "1";
           // string period = "1";
            //string shift = "1";
           // string device = "123";
            //string version = "2";
           
        }
        [HttpGet]
        [Route("ProcessJson")]
        public string ProcessJson()
        {
            string device = "123";

            string sql = "SELECT ID, ObjectJson FROM CollectionJson WHERE [Device_Id] = " + device + " AND [IsFullSaved] = 0";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                string Id = r[0].ToString();
                                string obj = r[1].ToString();
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        [HttpGet]
        [Route("InsertConsoliDationLevel01")]
        public int InsertConsoliDationLevel01()
        {

            //string obj, string collectionDate, string level01id, string unit, string period, string shift, string device, string version
            string unit = "1";
            string departament = "1";
            string level01Id = "1";
            string collectionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            string sql = "INSERT ConsolidationLevel01 ([UnitId],[DepartmentId],[Level01Id],[AddDate],[AlterDate],[ConsolidationDate]) VALUES ('" + unit + "','" + departament + "','" + level01Id + "', GetDate(),null, CONVERT(DATE, '" + collectionDate + "')) SELECT @@IDENTITY AS 'Identity'";


            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = command.ExecuteScalar();
                       
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return 0;
        }
        [HttpGet]
        [Route("InsertConsoliDationLevel02")]

        public int InsertConsoliDationLevel02()
        {

            //string obj, string collectionDate, string level01id, string unit, string period, string shift, string device, string version
            string level01Consolidation = "23856";
            string level02Id = "40";
            string unitId = "1";
            string collectionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            string sql = "INSERT ConsolidationLevel02 ([Level01ConsolidationId], [Level02Id], [UnitId], [AddDate], [AlterDate], [ConsolidationDate]) VALUES ('" + level01Consolidation + "', '" + level02Id + "', '" + unitId + "', GetDate(), null, GetDate()) SELECT @@IDENTITY AS 'Identity'";


            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = command.ExecuteScalar();

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return 0;
        }
        [HttpGet]
        [Route("InsertCollectionLevel02")]

        public int InsertCollectionLevel02()
        {

            //string obj, string collectionDate, string level01id, string unit, string period, string shift, string device, string version
            string consolidationLevel02 = "37157";
            string level01Id = "3";
            string level02Id = "40";
            string unitId = "1";
            string auditorId = "1";
            string shift = "1";
            string period = "1";
            string phase = "1";
            string reaudit = "0";
            string reauditNumber = "0";
            string CollectionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //atualizar no banco isso
            string startphase = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss");
            string evaluationNumber = "1";
            string sampleNumber = "1";
            string cattlytypeId = "1";
            string chainspeedy = "1";
            string ConsecutiveFailureIs = "0";
            string ConsecutiveFailureTotal = "0";
            string LotNumber = "1";
            string Mudscore = "1";
            string NotEvaluatedIs = "0";
            string Duplicated = "0";


            string collectionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            string sql =  "INSERT INTO CollectionLevel02 ([ConsolidationLevel02Id],[Level01Id],[Level02Id],[UnitId],[AuditorId],[Shift],[Period],[Phase],[ReauditIs],[ReauditNumber],[CollectionDate],[StartPhaseDate],[EvaluationNumber],[Sample],[AddDate],[AlterDate],[CattleTypeId],[Chainspeed],[ConsecutiveFailureIs],[ConsecutiveFailureTotal],[LotNumber],[Mudscore],[NotEvaluatedIs],[Duplicated]) " +
                          "VALUES" +
                          "('" +  consolidationLevel02+ "','" + level01Id + "','" + level02Id + "','" + unitId + "','" + auditorId + "','" + shift + "','" + period + "','" + phase + "','" +reaudit + "','" + reauditNumber + "', CONVERT(DATE, '" + CollectionDate + "'), CONVERT(DATE, '" + startphase + "'),'" + evaluationNumber + "','"  + sampleNumber +  "',GETDATE(),NULL,'" + cattlytypeId + "','" + chainspeedy + "','" + ConsecutiveFailureIs + "','" + ConsecutiveFailureTotal + "','" + LotNumber + "','" + Mudscore + "','" + NotEvaluatedIs + "','" + Duplicated + "')";


            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = command.ExecuteScalar();

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return 0;
        }
        [HttpGet]
        [Route("InsertCollectionLevel03")]

        public int InsertCollectionLevel03()
        {

            //string obj, string collectionDate, string level01id, string unit, string period, string shift, string device, string version

            string CollectionLevel02Id = "42485";
            string Level03Id = "2";
            string ConformedIs = "1";
            string Value = "0";
            string ValueText = "";
            string Duplicated = "0";


            string sql = "INSERT INTO CollectionLevel03 ([CollectionLevel02Id],[Level03Id],[AddDate],[AlterDate],[ConformedIs],[Value],[ValueText],[Duplicated]) " +
                         "VALUES " +
                         "('" + CollectionLevel02Id + "','" + Level03Id + "',GETDATE(),NULL,'" + ConformedIs + "','" + Value  + "','" + ValueText + "','" + Duplicated + "')";




            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = command.ExecuteScalar();

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return 0;
        }

    }
}

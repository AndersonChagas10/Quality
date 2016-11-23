using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Web.Helpers;
using SgqSystem.Handlres;

namespace SgqSystem.Services
{
    /// <summary>
    /// Summary description for SynService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SynService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        private DateTime dateCollectConvert(string collectionDate)
        {
            string[] data = collectionDate.Split('/');

            //verificar o tipo de data quando for no brasil
            string ano = data[2].Substring(0, 4);
            string mes = data[0];
            string dia = data[1];

            string hora = data[2].Substring(4, 10);
            string[] horaArray = hora.Split(':');


            DateTime newData = new DateTime(
                                            year: Convert.ToInt32(ano),
                                            month: Convert.ToInt32(mes),
                                            day: Convert.ToInt32(dia),
                                            hour: Convert.ToInt32(horaArray[0]),
                                            minute: Convert.ToInt32(horaArray[1]),
                                            second: Convert.ToInt32(horaArray[2]));

            return newData;
        }

        [WebMethod]
        public string InsertJson(string obj, string deviceName, string deviceMac)
        {
            //string key = "123";
            if(string.IsNullOrEmpty(obj))
            {
                return null;
            }
            string key = "124e4343";
            obj = obj.Replace("</level02><level02>", "@").Replace("<level02>", "");
            string[] arrayObj = obj.Split('@');
            string sql = null;

            for (int i = 0; i < arrayObj.Length; i++)
            {
                string[] result = arrayObj[i].Split(';');
                string level01Id = result[0];

                string level01DataCollect = result[1];
                DateTime level01CollectData = dateCollectConvert(level01DataCollect);
                level01DataCollect = level01CollectData.ToString("yyyy-MM-dd HH:mm:ss");



                string level02Id = result[2];
                string level02DataCollect = result[3];

                DateTime level02CollectData = dateCollectConvert(level02DataCollect);
                level02DataCollect = level02CollectData.ToString("yyyy-MM-dd HH:mm:ss");

                string unidadeId = result[4];
                string period = result[5];
                string shift = result[6];
                string versao = result[7];
                string ambiente = result[8];

                string level03Result = result[9];
                level03Result = HttpUtility.UrlDecode(level03Result, System.Text.Encoding.Default);

                sql += "INSERT INTO [dbo].[CollectionJson] " +
                       "([Unit_Id],[Shift],[Period],[level01_Id],[Level01CollectionDate],[level02_Id],[Level02CollectionDate],[ObjectJson],[Device_Id],[AppVersion],[Ambient],[IsFullSaved],[Device_Mac],[AddDate],[AlterDate],[Key],[TTP]) " +
                       "VALUES " +
                       "('" + unidadeId + "','" + shift + "','" + period + "','" + level01Id + "',CAST(N'" + level01DataCollect + "' AS DateTime),'" + level02Id + "',CAST(N'" + level02DataCollect + "' AS DateTime),'" + level03Result + "','" + deviceName + "','" + versao + "','" + ambiente + "',0,'" + deviceMac + "',GETDATE(),NULL,'" + key + "',NULL) ";
                       //"([AddDate],[AlterDate],[ObjectJson],[Key],[CollectionDate],[IsFullSaved],[level01_Id],[level02_Id],[Unit_Id],[Period],[Shift],[Device_Id],[Device_Mac],[AppVersion],[Ambient],[TTP])" +
                       //"VALUES " +
                       //"(GetDate(),null,'" + level03Result + "','" + key + "',CONVERT(DATE, '" + level02Collect + "'),0,'" + level01Id + "', '" + level02Id + "','" + unidadeId + "','" + period + "','" + shift + "','" + deviceName + "', '" + deviceMac + "', '" + ambiente + "','" + versao + "', null) ";

            }
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = command.ExecuteScalar();
                        if (i != null)
                        {

                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                    return ex.Message;
            }
            return null;
        }

        [WebMethod]
        public string ProcessJson(string device)
        {


            string sql = "SELECT [level01_Id], [Level01CollectionDate], [level02_Id], [Level02CollectionDate], [Unit_Id],[Period], [Shift], [AppVersion], [Ambient], [Device_Id], [Device_Mac] , [Key], [ObjectJson]  FROM CollectionJson WHERE [Device_Id] = " + device + " AND [IsFullSaved] = 0";

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
                                string level01 = r[0].ToString();
                                string level01CollectionDate = r[1].ToString();
                                level01CollectionDate = Convert.ToDateTime(level01CollectionDate).ToString("yyyy-MM-dd HH:mm:ss");

                                string level02 = r[2].ToString();
                                string level02CollectionDate = r[3].ToString();
                                level02CollectionDate = Convert.ToDateTime(level02CollectionDate).ToString("yyyy-MM-dd HH:mm:ss");


                                string unitId = r[4].ToString();
                                if(unitId == "0")
                                {
                                    unitId = "1";
                                }


                                string period = r[5].ToString();
                                string shift = r[6].ToString();

                                string appVersion = r[7].ToString();
                                string ambiente = r[8].ToString();

                                string deviceName = r[9].ToString();
                                string deviceMac = r[10].ToString();
                                string key = r[11].ToString();
                                string objson = r[12].ToString();

                               int ConsolidationLevel01Id = InsertConsoliDationLevel01(unitId, level01, level01CollectionDate);
                               int ConsolidationLevel02Id = InsertConsoliDationLevel02(ConsolidationLevel01Id.ToString(), level02, unitId, level02CollectionDate);

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




        [WebMethod]
        public int InsertConsoliDationLevel01(string unitId, string level01Id, string collectionDate, string departmentId="1")
        {

            int CollectionLevel01Id = GetLevel01Consolidation(unitId, level01Id, collectionDate);
            if(CollectionLevel01Id > 0)
            {
                return CollectionLevel01Id;
            }

            string sql = "INSERT ConsolidationLevel01 ([UnitId],[DepartmentId],[Level01Id],[AddDate],[AlterDate],[ConsolidationDate]) VALUES ('" + unitId + "','" + departmentId + "','" + level01Id + "', GetDate(),null, CONVERT(DATE, '" + collectionDate + "')) " +
                         "SELECT @@IDENTITY AS 'Identity'";


            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = command.ExecuteScalar();

                        return Convert.ToInt32(i);
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


        [WebMethod]
        public int GetLevel01Consolidation(string unitId, string level01Id, string collectionDate)
        {

            collectionDate = Convert.ToDateTime(collectionDate).ToString("yyyy-MM-dd");

            string sql = "SELECT Id FROM ConsolidationLevel01 WHERE UnitId = '" + unitId + "' AND Level01Id= '" + level01Id + "' AND CONVERT(date, ConsolidationDate) = '" + collectionDate + "'";

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
                            if (r.Read())
                            {

                                return Convert.ToInt32(r[0]);
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
            return 0;
        }


        [WebMethod]
        public int InsertConsoliDationLevel02(string Level01ConsolidationId, string Level02Id, string unitId, string collectionDate)
        {

            int CollectionLevel02Id = GetLevel02Consolidation(Level01ConsolidationId, Level02Id);
            if (CollectionLevel02Id > 0)
            {
                return CollectionLevel02Id;
            }

            string sql = "INSERT ConsolidationLevel02 ([Level01ConsolidationId], [Level02Id], [UnitId], [AddDate], [AlterDate], [ConsolidationDate]) VALUES  " +
                         "('" + Level01ConsolidationId  + "', '" + Level02Id + "', '" + unitId + "', GETDATE(), NULL, CAST(N'" + collectionDate + "' AS DateTime)) " +
                         "SELECT @@IDENTITY AS 'Identity'";


            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = command.ExecuteScalar();

                        return Convert.ToInt32(i);
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


        [WebMethod]
        public int GetLevel02Consolidation(string Level01ConsolidationId, string Level02Id)
        {

           

            string sql = "SELECT Id FROM ConsolidationLevel02 WHERE Level01ConsolidationId = '" + Level01ConsolidationId + "' AND Level02Id= '" + Level02Id + "'";

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
                            if (r.Read())
                            {

                                return Convert.ToInt32(r[0]);
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
            return 0;
        }

    }
}

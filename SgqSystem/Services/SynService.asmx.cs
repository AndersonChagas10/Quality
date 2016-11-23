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

        #region Json
        [WebMethod]
        public string InsertJson(string obj, string deviceName, string deviceMac)
        {
            //string key = "123";
            if (string.IsNullOrEmpty(obj))
            {
                return null;
            }
            string key = "124e4343";
            obj = obj.Replace("</level02><level02>", "@").Replace("<level02>", "").Replace("</level02>", "");
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
                        else
                        {
                            return "erro json";
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


            string sql = "SELECT [level01_Id], [Level01CollectionDate], [level02_Id], [Level02CollectionDate], [Unit_Id],[Period], [Shift], [AppVersion], [Ambient], [Device_Id], [Device_Mac] , [Key], [ObjectJson], [Id]  FROM CollectionJson WHERE [Device_Id] = " + device + " AND [IsFullSaved] = 0";

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
                                string Id = r[13].ToString();
                                string level01 = r[0].ToString();
                                string level01CollectionDate = r[1].ToString();
                                level01CollectionDate = Convert.ToDateTime(level01CollectionDate).ToString("yyyy-MM-dd HH:mm:ss");

                                string level02 = r[2].ToString();
                                string level02CollectionDate = r[3].ToString();
                                level02CollectionDate = Convert.ToDateTime(level02CollectionDate).ToString("yyyy-MM-dd HH:mm:ss");


                                string unitId = r[4].ToString();
                                if (unitId == "0")
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

                                string AuditorId = "1";
                                string Phase = "1";
                                string Reaudit = "1";
                                string StartPhase = null;
                                string Evaluation = "1";
                                string Sample = "1";
                                string CattleType = "123";
                                string ChainSpeed = "13";
                                string ConsecutiveFailureTotal = "0";
                                string ConsecuticeFalireIs = "0";
                                string LotNumber = "1";
                                string MudScore = "1";
                                string NotEvaluateIs = "1";
                                string Duplicated = "0";


                                int ConsolidationLevel01Id = InsertConsoliDationLevel01(unitId, level01, level01CollectionDate);
                                if(ConsolidationLevel01Id == 0)
                                {
                                    return "erro consolidation level01";
                                }
                                int ConsolidationLevel02Id = InsertConsoliDationLevel02(ConsolidationLevel01Id.ToString(), level02, unitId, level02CollectionDate);
                                if(ConsolidationLevel02Id == 0)
                                {
                                    return  "Erro Consolidation Level02";
                                }

                                int CollectionLevel02Id = InsertCollectionLevel02(ConsolidationLevel02Id.ToString(), level01, level02, unitId, AuditorId, shift, period, Phase, Reaudit, Reaudit, level02CollectionDate,
                                                                                  StartPhase, Evaluation, Sample, CattleType, ChainSpeed, ConsecuticeFalireIs, ConsecutiveFailureTotal, LotNumber, MudScore, NotEvaluateIs, Duplicated);

                                if(CollectionLevel02Id == 0)
                                {
                                    return "erro Collection level02";
                                }


                                int CollectionLevel03Id = InsertCollectionLevel03(CollectionLevel02Id.ToString(), level02, objson, AuditorId, Duplicated);
                                if(CollectionLevel03Id == 0)
                                {
                                    return "Erro Level03";
                                }
                                int jsonUpdate = updateJson(Id);
                                if(jsonUpdate  == 0)
                                {
                                    return "Erro Json";
                                }
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

        public int updateJson(string JsonId)
        {



            string sql = "UPDATE CollectionJson SET IsFullSaved=1 WHERE ID='" + JsonId + "'";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteNonQuery());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
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

        #endregion

        #region Consolidation Level01
        [WebMethod]
        public int InsertConsoliDationLevel01(string unitId, string level01Id, string collectionDate, string departmentId = "1")
        {

            int CollectionLevel01Id = GetLevel01Consolidation(unitId, level01Id, collectionDate);
            if (CollectionLevel01Id > 0)
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
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
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

        #endregion

        #region Consolidation Level02
        [WebMethod]
        public int InsertConsoliDationLevel02(string Level01ConsolidationId, string Level02Id, string unitId, string collectionDate)
        {

            int CollectionLevel02Id = GetLevel02Consolidation(Level01ConsolidationId, Level02Id);
            if (CollectionLevel02Id > 0)
            {
                return CollectionLevel02Id;
            }

            string sql = "INSERT ConsolidationLevel02 ([Level01ConsolidationId], [Level02Id], [UnitId], [AddDate], [AlterDate], [ConsolidationDate]) VALUES  " +
                         "('" + Level01ConsolidationId + "', '" + Level02Id + "', '" + unitId + "', GETDATE(), NULL, CAST(N'" + collectionDate + "' AS DateTime)) " +
                         "SELECT @@IDENTITY AS 'Identity'";


            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        if (i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
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

        #endregion

        #region Collection Level02
        public int InsertCollectionLevel02(string ConsolidationLevel02Id, string Level01Id, string Level02Id, string UnitId, string AuditorId, string Shift, string Period, string Phase, string Reaudit, string ReauditNumber, string CollectionDate,
                                           string StartPhase, string Evaluation, string Sample, string CatteType, string ChainSpeed, string ConsecuticeFalireIs, string ConsecutiveFailureTotal, string LotNumber, string MudScore, string NotEvaluateIs, string Duplicated)
        {


            if (string.IsNullOrEmpty(StartPhase))
            {
                StartPhase = "'0001-01-01 00:00:00'";
            }
            else
            {
                DateTime dataPhase = dateCollectConvert(StartPhase);

                StartPhase = "CAST(N'" + dataPhase.ToString("yyyy-MM-dd 00:00:00") + "' AS DateTime)";
            }

            string collectionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            string sql = "INSERT INTO CollectionLevel02 ([ConsolidationLevel02Id],[Level01Id],[Level02Id],[UnitId],[AuditorId],[Shift],[Period],[Phase],[ReauditIs],[ReauditNumber],[CollectionDate],[StartPhaseDate],[EvaluationNumber],[Sample],[AddDate],[AlterDate],[CattleTypeId],[Chainspeed],[ConsecutiveFailureIs],[ConsecutiveFailureTotal],[LotNumber],[Mudscore],[NotEvaluatedIs],[Duplicated]) " +
                          "VALUES" +
                          "('" + ConsolidationLevel02Id + "','" + Level01Id + "','" + Level02Id + "','" + UnitId + "','" + AuditorId + "','" + Shift + "','" + Period + "','" + Phase + "','" + Reaudit + "','" + ReauditNumber + "', CAST(N'" + CollectionDate + "' AS DateTime), " + StartPhase + ",'" + Evaluation + "','" + Sample + "',GETDATE(),NULL,'" + CatteType + "','" + ChainSpeed + "','" + ConsecuticeFalireIs + "','" + ConsecutiveFailureTotal + "','" + LotNumber + "','" + MudScore + "','" + NotEvaluateIs + "','" + Duplicated + "') " +
                          "SELECT @@IDENTITY AS 'Identity'";


            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        if(i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
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

        #endregion

        #region Collection Level03
        public int InsertCollectionLevel03(string CollectionLevel02Id, string level02, string level03Results, string auditorId, string duplicated)
        {

            //string obj, string collectionDate, string level01id, string unit, string period, string shift, string device, string version

            level03Results = level03Results.Replace("</level03><level03>", "@").Replace("<level03>", "").Replace("</level03>", "");

            string[] arrayResults = level03Results.Split('@');
            //"trocar o virgula do value text";

            string sql = null;

            for (int i = 0; i < arrayResults.Length; i++)
            {

                var result = arrayResults[i].Split(',');

                string Level03Id = result[0];
                string value = result[2];
                if(string.IsNullOrEmpty(value) || value == "null")
                {
                    value = "0";
                }
                string conform = result[3];
                string valueText = result[6];
                if(string.IsNullOrEmpty(valueText))
                {
                    valueText = "undefined";
                }

                sql +=   "INSERT INTO CollectionLevel03 ([CollectionLevel02Id],[Level03Id],[AddDate],[AlterDate],[ConformedIs],[Value],[ValueText],[Duplicated]) " +
                         "VALUES " +
                         "('" + CollectionLevel02Id + "','" + Level03Id + "',GETDATE(),NULL,'" + conform + "','" + value + "','" + valueText + "','" + duplicated + "') ";

            }
            sql += "SELECT @@IDENTITY AS 'Identity'";

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DbContextSgqEUA"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        var i = Convert.ToInt32(command.ExecuteScalar());
                        if(i > 0)
                        {
                            return i;
                        }
                        else
                        {
                            return 0;
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

        #endregion  
    }
}

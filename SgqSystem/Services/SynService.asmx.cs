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

            string hora = data[2].Substring(4, (data[2].Length - 4));
            hora = hora.Trim();
            if(hora.Length == 5)
            {
                hora += ":00";
            }
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
        public string InsertJson(string ObjResultJSon, string deviceId, string deviceMac)
        {
            //string key = "123";
            if (string.IsNullOrEmpty(ObjResultJSon))
            {
                return null;
            }
            string key = "124e4343";
            ObjResultJSon = ObjResultJSon.Replace("</level02><level02>", "@").Replace("<level02>", "").Replace("</level02>", "");
            string[] arrayObj = ObjResultJSon.Split('@');
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



                string auditorId = result[7];

                string reaudit = result[9];
                if(reaudit == "true")
                {
                    reaudit = "1";
                }
                else
                {
                    reaudit = "0";
                }

                string evaluate = result[11];
                string sample = result[12];

                string versao = result[20];
                string ambiente = result[21];

                string phase = result[8];
                string startphasedate = result[10];
                string cattletype = result[13];
                string chainspeed = result[14];
                string lotnumber = result[15];
                string mudscore = result[16];
                string consecutivefailurelevel = result[17];
                string consecutivefailuretotal = result[18];
                string notavaliable = result[19];


                string baisedUnbaised = result[27];
                if (baisedUnbaised != "0")
                {
                    cattletype = baisedUnbaised;
                }

                string level02HeaderJSon = phase;
                       level02HeaderJSon += ";" + startphasedate;
                       level02HeaderJSon += ";" + cattletype;
                       level02HeaderJSon += ";" + chainspeed;
                       level02HeaderJSon += ";" + lotnumber;
                       level02HeaderJSon += ";" + mudscore;
                       level02HeaderJSon += ";" + consecutivefailurelevel;
                       level02HeaderJSon += ";" + consecutivefailuretotal;
                       level02HeaderJSon += ";" + notavaliable;

                string level03ResultJson = result[22];
                level03ResultJson = HttpUtility.UrlDecode(level03ResultJson, System.Text.Encoding.Default);
                string correctiveActionJson =  result[23];
                string haveReaduit = result[24];
                
                if(haveReaduit == "undefined")
                {
                    haveReaduit = "0";
                }
                else
                {
                    haveReaduit = "1";
                }

                string haveCorrectiveAction = result[25];
                if(haveCorrectiveAction == "undefined")
                {
                    haveCorrectiveAction = "0";
                }
                else
                {
                    haveCorrectiveAction = "1";
                }
                string reauditNumber = result[26];
                if(reauditNumber == "undefined")
                {
                    reauditNumber = "0";
                }


                sql += "INSERT INTO [dbo].[CollectionJson] " +
                       "([Unit_Id],[Shift],[Period],[level01_Id],[Level01CollectionDate],[level02_Id],[Evaluate],[Sample],[AuditorId],[Level02CollectionDate],[Level02HeaderJson],[Level03ResultJSon],[CorrectiveActionJson],[Reaudit],[ReauditNumber],[haveReaudit],[haveCorrectiveAction],[Device_Id],[AppVersion],[Ambient],[IsProcessed],[Device_Mac],[AddDate],[AlterDate],[Key],[TTP]) " +
                       "VALUES " +
                       "('" + unidadeId + "','" + shift + "','" + period + "','" + level01Id + "',CAST(N'" + level01DataCollect + "' AS DateTime),'" + level02Id + "','" + evaluate + "','" + sample + "', '" + auditorId + "',CAST(N'" + level02DataCollect + "' AS DateTime),'" + level02HeaderJSon + "','" + level03ResultJson + "', '" + correctiveActionJson + "', '" + reaudit + "', '" + reauditNumber + "', '" + haveReaduit + "','" + haveCorrectiveAction   + "' ,'" + deviceId + "','" + versao + "','" + ambiente + "',0,'" + deviceMac + "',GETDATE(),NULL,'" + key + "',NULL) ";
                //"([AddDate],[AlterDate],[ObjectJson],[Key],[CollectionDate],[IsProcessed],[level01_Id],[level02_Id],[Unit_Id],[Period],[Shift],[Device_Id],[Device_Mac],[AppVersion],[Ambient],[TTP])" +
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
                        var i = command.ExecuteNonQuery();
                        if (i > 0)
                        {
                            return null;
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

            if(string.IsNullOrEmpty(device))
            {
                return "informe o device";
            }
            if(device == "web")
            {
                device = null;
            }
            else
            {
                device = "[Device_Id] = '" + device + "' AND";
            }

            string sql = "SELECT [level01_Id], [Level01CollectionDate], [level02_Id], [Level02CollectionDate], [Unit_Id],[Period], [Shift], [AppVersion], [Ambient], [Device_Id], [Device_Mac] , [Key], [Level03ResultJSon], [Id], [Level02HeaderJson], [Evaluate],[Sample],[AuditorId], [Reaudit], [CorrectiveActionJson],[haveReaudit],[haveCorrectiveAction],[ReauditNumber]  FROM CollectionJson WHERE " + device + " [IsProcessed] = 0";

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

                                string Level02HeaderJson = r[14].ToString();

                                string[] arrayHeader = Level02HeaderJson.Split(';');
                                //[Evaluate],[Sample],[AuditorId], [Reaudit] 



                                string Phase = arrayHeader[0];
                                string AuditorId = r[17].ToString();
                                string Reaudit = r[18].ToString();
                                if(Reaudit == "False")
                                {
                                    Reaudit = "0";
                                }
                                else
                                {
                                    Reaudit = "1";
                                }
                                string StartPhase = arrayHeader[1];
                                string Evaluation = r[15].ToString();
                                string Sample = r[16].ToString();
                                
                                string CattleType = arrayHeader[2];
                                if(CattleType == "undefined" || string.IsNullOrEmpty(CattleType))
                                {
                                    CattleType = "0";
                                }
                                string ChainSpeed = arrayHeader[3];
                                if(ChainSpeed == "undefined" || string.IsNullOrEmpty(ChainSpeed))
                                {
                                    ChainSpeed = "0";
                                }
                                string ConsecuticeFalireIs = arrayHeader[6];
                                if(ConsecuticeFalireIs == "undefined" || ConsecuticeFalireIs == "null" || string.IsNullOrEmpty(ConsecuticeFalireIs))
                                {
                                    ConsecuticeFalireIs = "0";
                                }
                                else
                                {
                                    ConsecuticeFalireIs = "1";
                                }
                                string ConsecutiveFailureTotal = arrayHeader[7];
                                if(ConsecutiveFailureTotal == "undefined" || ConsecuticeFalireIs == "null" || string.IsNullOrEmpty(ConsecutiveFailureTotal))
                                {
                                    ConsecutiveFailureTotal = "0";
                                }
                                string LotNumber = arrayHeader[4];
                                if(LotNumber == "undefined" || string.IsNullOrEmpty(LotNumber))
                                {
                                    LotNumber = "0";
                                }
                                string MudScore = arrayHeader[5];
                                if(MudScore == "undefined" || string.IsNullOrEmpty(MudScore))
                                {
                                    MudScore = "0";
                                }
                                string NotEvaluateIs = arrayHeader[8];
                                if(NotEvaluateIs == "false")
                                {
                                    NotEvaluateIs = "0";
                                }
                                else
                                {
                                    NotEvaluateIs = "1";
                                }
                                string Duplicated = "0";

                                string correctiveActionJson = r[19].ToString();
                                string haveReaudit = r[20].ToString();
                                string haveCorrectiveACtion = r[21].ToString();
                                string reauditNumber = r[22].ToString();

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

                                int CollectionLevel02Id = InsertCollectionLevel02(ConsolidationLevel02Id.ToString(), level01, level02, unitId, AuditorId, shift, period, Phase, Reaudit, reauditNumber, level02CollectionDate,
                                                                                  StartPhase, Evaluation, Sample, CattleType, ChainSpeed, ConsecuticeFalireIs, ConsecutiveFailureTotal, LotNumber, MudScore, NotEvaluateIs, Duplicated, haveReaudit, haveCorrectiveACtion);

                                if(CollectionLevel02Id == 0)
                                {
                                    return "erro Collection level02";
                                }


                                int CollectionLevel03Id = InsertCollectionLevel03(CollectionLevel02Id.ToString(), level02, objson, AuditorId, Duplicated);
                                if(CollectionLevel03Id == 0)
                                {
                                    return "Erro Level03";
                                }

                                if(!string.IsNullOrEmpty(correctiveActionJson))
                                {
                                    correctiveActionJson = correctiveActionJson.Replace("<correctiveaction>", "").Replace("</correctiveaction>", "");

                                    string[] arrayCorrectiveAction = correctiveActionJson.Split(',');

                                    string slaugthersignature = arrayCorrectiveAction[0];
                                    string techinicalsignature = arrayCorrectiveAction[1];
                                    string datetimeslaughter = arrayCorrectiveAction[2];
                                    string datetimetechinical = arrayCorrectiveAction[3];
                                    string auditstarttime = arrayCorrectiveAction[4];
                                    string datecorrectiveaction = arrayCorrectiveAction[5];

                                    string descriptionFailure = arrayCorrectiveAction[6];
                                    descriptionFailure = HttpUtility.UrlDecode(descriptionFailure, System.Text.Encoding.Default);

                                    string immediateCorrectiveAction = arrayCorrectiveAction[7];
                                    immediateCorrectiveAction = HttpUtility.UrlDecode(immediateCorrectiveAction, System.Text.Encoding.Default);

                                    string productDisposition = arrayCorrectiveAction[8];
                                    productDisposition = HttpUtility.UrlDecode(productDisposition, System.Text.Encoding.Default);

                                    string preventativeMeasure = arrayCorrectiveAction[9];
                                    preventativeMeasure = HttpUtility.UrlDecode(preventativeMeasure, System.Text.Encoding.Default);

                                    int CorrectiveActionId = correctiveActionInsert(AuditorId, CollectionLevel02Id.ToString(), slaugthersignature, techinicalsignature, datetimeslaughter, 
                                                                                   datetimetechinical, datecorrectiveaction, auditstarttime, descriptionFailure, immediateCorrectiveAction,
                                                                                   productDisposition, preventativeMeasure);

                                    if(CorrectiveActionId == 0)
                                    {
                                        return "erro CorrectiveAction";
                                    }

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



            string sql = "UPDATE CollectionJson SET IsProcessed=1 WHERE ID='" + JsonId + "'";

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
                                           string StartPhase, string Evaluation, string Sample, string CatteType, string ChainSpeed, string ConsecuticeFalireIs,
                                           string ConsecutiveFailureTotal, string LotNumber, string MudScore, string NotEvaluateIs, string Duplicated, string haveReaudit, string haveCorrectiveAction)
        {


            if (string.IsNullOrEmpty(StartPhase) || StartPhase == "null" || StartPhase == "undefined")
            {
                StartPhase = "'0001-01-01 00:00:00'";
            }
            else
            {
                DateTime dataPhase = dateCollectConvert(StartPhase);

                StartPhase = "CAST(N'" + dataPhase.ToString("yyyy-MM-dd 00:00:00") + "' AS DateTime)";
            }

            string collectionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            string sql = "INSERT INTO CollectionLevel02 ([ConsolidationLevel02Id],[Level01Id],[Level02Id],[UnitId],[AuditorId],[Shift],[Period],[Phase],[ReauditIs],[ReauditNumber],[CollectionDate],[StartPhaseDate],[EvaluationNumber],[Sample],[AddDate],[AlterDate],[CattleTypeId],[Chainspeed],[ConsecutiveFailureIs],[ConsecutiveFailureTotal],[LotNumber],[Mudscore],[NotEvaluatedIs],[Duplicated],[HaveReaudit], [HaveCorrectiveAction]) " +
                          "VALUES" +
                          "('" + ConsolidationLevel02Id + "','" + Level01Id + "','" + Level02Id + "','" + UnitId + "','" + AuditorId + "','" + Shift + "','" + Period + "','" + Phase + "','" + Reaudit + "','" + ReauditNumber + "', CAST(N'" + CollectionDate + "' AS DateTime), " + StartPhase + ",'" + Evaluation + "','" + Sample + "',GETDATE(),NULL,'" + CatteType + "','" + ChainSpeed + "','" + ConsecuticeFalireIs + "','" + ConsecutiveFailureTotal + "','" + LotNumber + "','" + MudScore + "','" + NotEvaluateIs + "','" + Duplicated + "', '" + haveReaudit + "', '" + haveCorrectiveAction + "') " +
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
                if(string.IsNullOrEmpty(value) || value == "null" || value == "undefined")
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

        #region Corrective Action
        public int correctiveActionInsert(string AuditorId, string CollectionLevel02Id, string SlaughterId, string TechinicalId, string DateTimeSlaughter, string DateTimeTechinical, string DateCorrectiveAction, string AuditStartTime, string DescriptionFailure, string ImmediateCorrectiveAction, string ProductDisposition, string PreventativeMeasure)
        {

            DateTime SlaughterDateTime = dateCollectConvert(DateTimeSlaughter);
            DateTimeSlaughter = SlaughterDateTime.ToString("yyyy-MM-dd HH:mm:ss");


            DateTime TechinicalDateTime = dateCollectConvert(DateTimeTechinical);
            DateTimeTechinical = TechinicalDateTime.ToString("yyyy-MM-dd HH:mm:ss");



            DateTime CorrectiveActionDate = dateCollectConvert(DateCorrectiveAction);
            DateCorrectiveAction = CorrectiveActionDate.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime StartTimeAudit = dateCollectConvert(AuditStartTime);
            AuditStartTime = StartTimeAudit.ToString("yyyy-MM-dd HH:mm:ss");

            string sql = "INSERT INTO CorrectiveAction ([AuditorId],[CollectionLevel02Id],[SlaughterId],[TechinicalId],[DateTimeSlaughter],[DateTimeTechinical],[AddDate],[AlterDate],[DateCorrectiveAction],[AuditStartTime],[DescriptionFailure],[ImmediateCorrectiveAction],[ProductDisposition],[PreventativeMeasure]) " +
                         "VALUES " +
                         "('" + AuditorId + "','" + CollectionLevel02Id + "','" + SlaughterId + "','" + TechinicalId + "',CAST(N'" + DateTimeSlaughter + "' AS DateTime),CAST(N'" + DateTimeTechinical + "' AS DateTime),GETDATE(),NULL,CAST(N'" + DateCorrectiveAction + "' AS DateTime),CAST(N'" + AuditStartTime + "' AS DateTime),'" + DescriptionFailure + "','" + ImmediateCorrectiveAction + "','" + ProductDisposition + "','" + PreventativeMeasure + "')";
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Web.Helpers;

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

        //public string InsertJson(string obj, string collectionDate, string level01Id, string level02Id, string unitId, string period, string shift, string device, string version, string ambient = null)
        public string InsertJson(string obj, string deviceName, string deviceMac)
        {
            //string key = "123";
            if(string.IsNullOrEmpty(obj))
            {
                return null;
            }
            string key = "124e4343";
            //dynamic coleta = Json.Decode(json);
            //DateTime data = dateCollectConvert(collectionDate);

            //collectionDate = data.ToString("yyyy-MM-dd HH:mm:ss");

            obj = obj.Replace("</level02><level02>", "@").Replace("<level02>", "");
            string[] arrayObj = obj.Split('@');

            string sql = null;

            for (int i = 0; i < arrayObj.Length; i++)
            {
                string[] result = arrayObj[i].Split(';');

                string level01Id = result[0];
                string level02Id = result[1];

                DateTime data = dateCollectConvert(result[2]);

                string level02Collect = data.ToString("yyyy-MM-dd HH:mm:ss");
                string unidadeId = result[3];
                string period = result[4];
                string shift = result[5];
               // string device = result[6];
                string versao = result[6];
                string ambiente = result[7];
                string level03Result = HttpUtility.UrlDecode(result[8], System.Text.Encoding.Default);

                sql += "INSERT INTO [dbo].[CollectionJson] " +
                       "([AddDate],[AlterDate],[ObjectJson],[Key],[CollectionDate],[IsFullSaved],[level01_Id],[level02_Id],[Unit_Id],[Period],[Shift],[Device_Id],[Device_Mac],[AppVersion],[Ambient],[TTP])" +
                       "VALUES " +
                       "(GetDate(),null,'" + level03Result + "','" + key + "',CONVERT(DATE, '" + level02Collect + "'),0,'" + level01Id + "', '" + level02Id + "','" + unidadeId + "','" + period + "','" + shift + "','" + deviceName + "', '" + deviceMac + "', '" + ambiente + "','" + versao + "', null) ";


                //for (int i2 = 0; i2 < result.Length; i2++)
                //{
                //}
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
    }
}

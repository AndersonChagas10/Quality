using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/ImportacaoExcel")]
    [HandleApi()]
    public class ImportacaoExcelApiController : ApiController
    {

        [HttpPost]
        [Route("SalvarExcel")]
        public HttpStatusCode SalvarExcel([FromBody] List<JObject> dados)
        {
            //ver com leo para subir a branche do gabriel( SyncServicee = insertJson nao esta funcionando)
            try
            {
                foreach (var item in dados)
                {
                    var av = item["MATÉRIA PRIMA"].ToString();
                    var parlevel1_id = item["INDICADOR"].ToString();
                    var parlevel2_id = "306";
                    var parlevel3_id = "1142";
                    var cluster_id = "3";
                    var versionApp = "2.0.50";
                    var NC = "258";
                    var empresa = "JBS";
                    var initials = item["UNI"].ToString();
                    var data = convertStringToDatetimeNull(item["DATA"].ToString(), "dd/MM/yyyy HH:mm:ss");
                    string collectionDate = data.Value.ToString("MM/dd/yyyy HH:mm:ss");
                    string collectionDateMesDiaAno = data.Value.ToString("MM/dd/yyyy HH:mm:ss");
                    string parCompany_Id = db.ParCompany.FirstOrDefault(r => r.Initials == initials).Id.ToString();
                    av = parlevel1_id == "43" ? "1" : av;

                    string collectionJson = "<level02>";
                    collectionJson += cluster_id + "|" + parlevel1_id; //[0]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += collectionDate; //[1]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += cluster_id + "|" + parlevel2_id; //[2]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += collectionDate; //[3]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += parCompany_Id; //[4]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[5]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[6]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[7]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[8]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[9]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += collectionDateMesDiaAno; //[10]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[11]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[12]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += ""; //[13]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[14]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[15]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += ""; //[16]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "undefined"; //[17]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "undefined"; //[18]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[19]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += versionApp; //[20]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += empresa; //[21]
                    collectionJson += ";"; //SEPARADOR LEVEL2

                    // Início do Level03

                    collectionJson += "<level03>";
                    collectionJson += parlevel3_id; //level3 1
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += collectionDate; //level3 2
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "0"; //level3 2???
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "true"; //level3 3
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "1"; //level3 6
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "null"; //level3 7
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "undefined"; //level3 8
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "undefined"; //level3 9
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "0"; //level3 10
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += ""; //level3 11
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "0"; //level3 12
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "0"; //level3 13
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "0"; //level3 14
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += av; //level3 15
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += NC; //level3 16
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "0"; //level3 17
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "0"; //level3 17??
                    collectionJson += ","; //SEPARADOR LEVEL3
                    collectionJson += "0"; //level3 17??
                    collectionJson += "</level03>";

                    // Final do Level03

                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += ""; //[23]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[24]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[25]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[26]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "null"; //[27]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "null"; //[28]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[29]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "undefined"; //[30]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[31]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "false"; //[32]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "undefined"; //[33]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[35]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[36]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[37]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[38]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[39]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[40]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[41]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[42]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[43]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "1"; //[44]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[45]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[46]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[47]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += "0"; //[48]
                    collectionJson += ";"; //SEPARADOR LEVEL2
                    collectionJson += cluster_id; //[49]
                    collectionJson += "</level02>";

                   // var teste = item.Last.ToString();
                   // new SgqSystem.Services.SyncServices().InsertJson(item.ToString(), "5", "1", false);

                    new SgqSystem.Services.SyncServices().InsertJson(collectionJson, "1", "1", false);

                }
                return HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public SgqDbDevEntities db = new SgqDbDevEntities();

        public DateTime? convertStringToDatetimeNull(string date, string format)
        {
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }

            if (date.Length == 10)
            {
                date += " 00:00:00";
            }
            DateTime datetime = DateTime.MinValue;

            if (DateTime.TryParseExact(
                    date,
                    format,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out datetime) || DateTime.TryParseExact(
                    date,
                    format.Replace("/", "."),
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out datetime))
            {
                if (datetime > DateTime.MinValue)
                {
                    return datetime;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}

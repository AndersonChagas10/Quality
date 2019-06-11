using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using static SgqSystem.Controllers.Api.SyncServiceApiController;

namespace SgqSystem.Controllers.Api.Public
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Public")]
    public class PublicApiController : BaseApiController
    {

        [HttpPost]
        [Route("Teste")]
        public string Teste()
        {
            return "Teste";
        }

        public SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("CriarCollectJson/{parlevel1_id}/{parlevel2_id}/{parlevel3_id}/{cluster_id}/{versionApp}/{empresa}/{ano}/{mes}/{dia}/{initials}/{av}/{NC}")]
        public void CriarCollectJson(string parlevel1_id, string parlevel2_id, string parlevel3_id, string cluster_id, string versionApp, string empresa, int ano, int mes, int dia, string initials, string av, string NC)           
        {

            /*
            parlevel1_id = "42";
            parlevel2_id = "306";
            parlevel3_id = "1142";
            cluster_id = "3";
            versionApp = "2.0.50";
            empresa = "JBS";
            initials = "CGR";
            av = "448030";
            NC = "258";
            */
            DateTime date = new DateTime(ano,mes,dia); //ano,mes,dia
            string collectionDate = date.Month.ToString().PadLeft(2, '0') + "/" + date.Day.ToString().PadLeft(2, '0') + "/" + date.Year.ToString() + " 00:00:00";
            string collectionDateMesDiaAno = date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0') + date.Year.ToString(); 
            string parCompany_Id = db.ParCompany.FirstOrDefault(r => r.Initials == initials).Id.ToString();

            av = parlevel1_id == "42" ? av : av;

            string sample = parlevel1_id == "42" ? av : "1";


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
            collectionJson += sample; //[12]
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
            collectionJson += parlevel3_id; //level3 0 ID LEVEL 3
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += collectionDate; //level3 1 DATA
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += NC; //level3 2 VALUE
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += Convert.ToInt32(NC) > 0 ? "false" : "true"; //level3 3 CONFORME
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "1"; //level3 4 ??
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "null"; //level3 5
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "undefined"; //level3 6 VALUE TEXT
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "undefined"; //level3 7 ID
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "1"; //level3 8 PESO
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += ""; //level3 9 NAME
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "0"; //level3 10 INTERVALO MINIMO
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "0"; //level3 11 INTERVALO MAXIMO
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "0"; //level3 12 ISNOTEVALUATE
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "0"; //level3 13 PUNICAO
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += NC; //level3 14 DEFECTS
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += av; //level3 15 WEIAV
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += NC; //level3 16 WEIDF
            collectionJson += ","; //SEPARADOR LEVEL3
            collectionJson += "false"; //level3 17 
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

            new SyncServiceApiController().InsertJson(new InsertJsonClass()
            {
                ObjResultJSon = collectionJson,
                deviceId = "1",
                deviceMac = "1",
                autoSend = false
            });

        }
    }
}

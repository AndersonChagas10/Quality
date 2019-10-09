using System.Collections.Generic;

namespace Dominio
{
    public class Coleta
    {
        public string ParCluster_Id { get; set; }
        public string ParLevel1_Id { get; set; }
        public string Level01DataCollect { get; set; }
        public string ParLevel2_Id { get; set; }
        public string Level02DataCollect { get; set; }
        public string UnidadeId { get; set; }
        public string Period { get; set; } = "0";
        public string Shift { get; set; } = "0";
        public string AuditorId { get; set; } = "1";
        public string Phase { get; set; } = "0";
        public string Reaudit { get; set; } = "0";
        public string Startphasedate { get; set; }
        public string Evaluate { get; set; } = "0";
        public string Sample { get; set; } = "0";
        public string Level02HeaderJSon { get; set; }
        public string Isemptylevel3 { get; set; }
        public string Hassampletotal { get; set; }
        public string Mudscore { get; set; }
        public string Consecutivefailurelevel { get; set; }
        public string Consecutivefailuretotal { get; set; }
        public string Notavaliable { get; set; }
        public string VersaoApp { get; set; } = "0";
        public string Ambiente { get; set; } = "0";
        public string CorrectiveActionJson { get; set; }
        public string HaveReaudit { get; set; } = "0";
        public string HaveCorrectiveAction { get; set; }
        public string ReauditNumber { get; set; } = "0";
        public string AlertLevel { get; set; } = "0";
        public string Completed { get; set; }
        public string HavePhases { get; set; } = "0";
        public string CollectionLevel02Id { get; set; } = "0";
        public string CorrectiveActionCompleted { get; set; }
        public string CompleteReaudit { get; set; }
        public string HashKey { get; set; } = "0";
        public string Weievaluation { get; set; }
        public string Weidefects { get; set; }
        public string Defects { get; set; }
        public string Totallevel3withdefects { get; set; } = "0";
        public string TotalLevel2Evaluation { get; set; } = "0";
        public string Avaliacaoultimoalerta { get; set; } = "0";
        public string Evaluatedresult { get; set; } = "0";
        public string Defectsresult { get; set; } = "0";
        public string Sequential { get; set; } = "0";
        public string Side { get; set; } = "0";
        public string Monitoramentoultimoalerta { get; set; } = "0";
        public string Reauditlevel { get; set; } = "0";
        public string Startphaseevaluation { get; set; } = "0";
        public string Endphaseevaluation { get; set; } = "0";
        public string Cluster { get; set; }
        public string Reprocesso { get; set; } = "0";

        public List<ColetaTarefa> ColetaTarefa { get; set; }
        public List<ColetaCabecalho> ColetaCabecalho { get; set; }

        public override string ToString()
        {

            string collectionJson = "<level02>";
            collectionJson += ParCluster_Id + "98789" + ParLevel1_Id; //[0]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Level01DataCollect; //[1]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += ParCluster_Id + "98789" + ParLevel2_Id; //[2]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Level02DataCollect; //[3]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += UnidadeId; //[4]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Period; //[5]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Shift; //[6]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += AuditorId; //[7]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Phase; //[8]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Reaudit; //[9]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Startphasedate; //[10]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Evaluate; //[11]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Sample; //[12]
            collectionJson += ";"; //SEPARADOR LEVEL2

            collectionJson += Dominio.ColetaCabecalho.pessego(ColetaCabecalho);

            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Isemptylevel3; //[14]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Hassampletotal; //[15]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Mudscore; //[16]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Consecutivefailurelevel; //[17]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Consecutivefailuretotal; //[18]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Notavaliable; //[19]   // VER onde preenche esse cara ficou faltando valor nele
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += VersaoApp; //[20]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Ambiente; //[21]
            collectionJson += ";"; //SEPARADOR LEVEL2


            collectionJson += Dominio.ColetaTarefa.pessego(ColetaTarefa);

            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += HaveReaudit; //[24]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += HaveCorrectiveAction; //[25]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += ReauditNumber; //[26]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += AlertLevel; //[27]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Completed; //[28]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += HavePhases; //[29]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += CollectionLevel02Id; //[30]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += CorrectiveActionCompleted; //[31]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += CompleteReaudit; //[32]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += HashKey; //[33]
            collectionJson += ";"; //SEPARADOR LEVEL2
            //[34]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Weievaluation; //[35]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Weidefects; //[36]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Defects; //[37]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Totallevel3withdefects; //[38]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += TotalLevel2Evaluation; //[39]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Avaliacaoultimoalerta; //[40]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Evaluatedresult; //[41]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Defectsresult; //[42]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Sequential; //[43]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Side; //[44]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Monitoramentoultimoalerta; //[45]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Reauditlevel; //[46]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Startphaseevaluation; //[47]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Endphaseevaluation; //[48]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Cluster; //[49]
            collectionJson += ";"; //SEPARADOR LEVEL2
            collectionJson += Reprocesso; //[50] 

            collectionJson += "</level02>";

            return collectionJson;
        }
    }


}

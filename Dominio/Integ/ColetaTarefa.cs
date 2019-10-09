using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ColetaTarefa
    {
        public string Level03Id { get; set; } //10
        public string CollectionDate { get; set; } // 01/29/2019
        public string ValueConform { get; set; } // 0
        public string Conform { get; set; } // false
        public string Pos4 { get; set; }
        public string Pos5 { get; set; }
        public string ValueText { get; set; } // null
        public string Id { get; set; } //undefined
        public string Weight { get; set; } // 1.0000
        public string Name { get; set; } //""
        public string IntervalMin { get; set; } //0.0000
        public string IntervalMax { get; set; } //0.0000
        public string IsnotEvaluate { get; set; } // false
        public string PunishimentValue { get; set; } // 1
        public string Defects { get; set; } // 1
        public string WeiEvaluation { get; set; } // 1
        public string WeiDefects { get; set; } // 2
        public string HasPhoto { get; set; }

        public static string pessego(List<ColetaTarefa> coletaTarefa)
        {
            if (coletaTarefa == null) return "";
            string collectionJson = "";
            foreach (var item in coletaTarefa)
            {
                collectionJson += "<level03>";
                collectionJson += item.Level03Id; //level3 0 ID LEVEL 3
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.CollectionDate; //level3 1 DATA
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.ValueConform; //level3 2 VALUE
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.Conform; //level3 3 CONFORME
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += "1"; //level3 4 ??
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += "null"; //level3 5
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.ValueText; //level3 6 VALUE TEXT
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.Id; //level3 7 ID
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.Weight; //level3 8 PESO
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.Name; //level3 9 NAME
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.IntervalMin; //level3 10 INTERVALO MINIMO
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.IntervalMax; //level3 11 INTERVALO MAXIMO
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.IsnotEvaluate; //level3 12 ISNOTEVALUATE
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.PunishimentValue; //level3 13 PUNICAO
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.Defects; //level3 14 DEFECTS
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.WeiEvaluation; //level3 15 WEIAV
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.WeiDefects; //level3 16 WEIDF
                collectionJson += ","; //SEPARADOR LEVEL3
                collectionJson += item.HasPhoto; //level3 17 
                collectionJson += "</level03>";
            }
            return collectionJson += ";";
        }
    }
}

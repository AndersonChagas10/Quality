using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ColetaCabecalho
    {

        public string ParHeaderField_Id { get; set; }
        public string ParFieldType_Id { get; set; }
        public string Value { get; set; }
        public string Evaluation { get; set; }
        public string Sample { get; set; }


        public static string pessego(List<ColetaCabecalho> coletaCabecalho)
        {
            if (coletaCabecalho == null) return "";
            string collectionJson = "";
            foreach (var item in coletaCabecalho)
            {
                collectionJson += "<header>";
                collectionJson += item.ParHeaderField_Id;
                collectionJson += ",";
                collectionJson += item.ParFieldType_Id;
                collectionJson += ",";
                collectionJson += item.Value;
                collectionJson += ",";
                collectionJson += item.Sample;
                collectionJson += "</header>";
            }
            return collectionJson += ";";
        }
    }
}

using System.Collections.Generic;

namespace DTO.ResultSet
{

    public class TabelaDinamicaResultados
    {
        public List<Trs> trsMeio { get; set; }

        public List<Ths> trsCabecalho1 { get; set; }
        public List<Ths> trsCabecalho2 { get; set; }
        public List<Ths> trsCabecalho3 { get; set; }

        public List<Trs> footer { get; set; }

        public string CallBackTableBody { get; set; }
        public string CallBackTableEsquerda { get; set; }
        public string CallBackTableTituloColunas { get; set; }
        public string Title { get; set; }

    }

    public class Trs
    {
        public string name { get; set; }
        public int coolspan { get; set; }

        public List<Tds> tdsEsquerda { get; set; }
        public List<Tds> tdsDireita { get; set; }
    }

    public class Tds
    {
        public string valor { get; set; }
        public string click { get; set; }
        public int coolspan { get; set; }
    }

    public class Ths
    {
        public string name { get; set; }
        public int coolspan { get; set; }
        public List<Ths> tds { get; set; }
    }

    //public class Footers
    //{
    //    public decimal valor { get; set; }
    //}

}

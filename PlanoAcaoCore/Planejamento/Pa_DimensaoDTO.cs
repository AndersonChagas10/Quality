using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Dimensao : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Dimensao> Listar()
        {
            var query = "SELECT * FROM Pa_Dimensao";
            return ListarGenerico<Pa_Dimensao>(query);
        }

        public static Pa_Dimensao Get(int Id)
        {
            var query = "SELECT * FROM Pa_Dimensao WHERE Id = " + Id;
            return GetGenerico<Pa_Dimensao>(query);
        }
    }
}

using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Missao : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Missao> Listar()
        {
            var query = "SELECT * FROM Pa_Missao";
            return ListarGenerico<Pa_Missao>(query);
        }

        public static Pa_Missao Get(int Id)
        {
            var query = "SELECT * FROM Pa_Missao WHERE Id = " + Id;
            return GetGenerico<Pa_Missao>(query);
        }
    }
}

using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_GrupoCausa : Pa_BaseObject
    {
        public string GrupoCausa { get; set; }
        public static List<Pa_GrupoCausa> Listar()
        {
            var query = "SELECT * FROM Pa_GrupoCausa";
            return ListarGenerico<Pa_GrupoCausa>(query);
        }

        public static Pa_GrupoCausa Get(int Id)
        {
            var query = "SELECT * FROM Pa_GrupoCausa WHERE Id = " + Id;
            return GetGenerico<Pa_GrupoCausa>(query);
        }
    }
}

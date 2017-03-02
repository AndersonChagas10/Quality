using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Visao : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Visao> Listar()
        {
            var query = "SELECT * FROM Pa_Visao";
            return ListarGenerico<Pa_Visao>(query);
        }

        public static Pa_Visao Get(int Id)
        {
            var query = "SELECT * FROM Pa_Visao WHERE Id = " + Id;
            return GetGenerico<Pa_Visao>(query);
        }
    }
}


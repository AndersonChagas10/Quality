using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Objetivo : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Objetivo> Listar()
        {
            var query = "SELECT * FROM Pa_Objetivo";
            return ListarGenerico<Pa_Objetivo>(query);
        }

        public static Pa_Objetivo Get(int Id)
        {
            var query = "SELECT * FROM Pa_Objetivo WHERE Id = " + Id;
            return GetGenerico<Pa_Objetivo>(query);
        }
    }
}

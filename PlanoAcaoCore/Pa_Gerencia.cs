using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Gerencia : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Gerencia> Listar()
        {
            var query = "SELECT * FROM Pa_Gerencia";
            return ListarGenerico<Pa_Gerencia>(query);
        }

        public static Pa_Gerencia Get(int Id)
        {
            var query = "SELECT * FROM Pa_Gerencia WHERE Id = " + Id;
            return GetGenerico<Pa_Gerencia>(query);
        }
    }
}

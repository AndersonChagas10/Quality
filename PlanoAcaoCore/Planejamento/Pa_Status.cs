using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Status : Pa_BaseObject
    {
        public string Name { get; set; }

        public static List<Pa_Status> Listar()
        {
            var query = "SELECT * FROM Pa_Status";
            return ListarGenerico<Pa_Status>(query);
        }

        public static Pa_Status Get(int Id)
        {
            var query = "SELECT * FROM Pa_Status WHERE Id = " + Id;
            return GetGenerico<Pa_Status>(query);
        }
    }
}

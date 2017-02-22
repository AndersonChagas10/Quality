using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Quem : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Quem> Listar()
        {
            var query = "SELECT * FROM Pa_Quem";
            return ListarGenerico<Pa_Quem>(query);
        }

        public static Pa_Quem Get(int Id)
        {
            var query = "SELECT * FROM Pa_Quem WHERE Id = " + Id;
            return GetGenerico<Pa_Quem>(query);
        }
    }
}

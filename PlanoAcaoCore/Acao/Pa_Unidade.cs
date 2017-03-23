using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Unidade : Pa_BaseObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public static List<Pa_Unidade> Listar()
        {
            var query = "SELECT * FROM Pa_Unidade";
            return ListarGenerico<Pa_Unidade>(query);
        }

        public static Pa_Unidade Get(int Id)
        {
            var query = "SELECT * FROM Pa_Unidade WHERE Id = " + Id;
            return GetGenerico<Pa_Unidade>(query);
        }
    }
}

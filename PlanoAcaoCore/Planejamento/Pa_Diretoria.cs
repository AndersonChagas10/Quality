using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Diretoria : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Diretoria> Listar()
        {
            var query = "SELECT * FROM Pa_Diretoria";
            return ListarGenerico<Pa_Diretoria>(query);
        }

        public static Pa_Diretoria Get(int Id)
        {
            var query = "SELECT * FROM Pa_Diretoria WHERE Id = " + Id;
            return GetGenerico<Pa_Diretoria>(query);
        }
    }
}

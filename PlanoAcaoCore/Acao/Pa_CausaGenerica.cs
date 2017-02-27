using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_CausaGenerica : Pa_BaseObject
    {
        public string CausaGenerica { get; set; }
        public static List<Pa_CausaGenerica> Listar()
        {
            var query = "SELECT * FROM Pa_CausaGenerica";
            return ListarGenerico<Pa_CausaGenerica>(query);
        }

        public static Pa_CausaGenerica Get(int Id)
        {
            var query = "SELECT * FROM Pa_CausaGenerica WHERE Id = " + Id;
            return GetGenerico<Pa_CausaGenerica>(query);
        }
    }
}

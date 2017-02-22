using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_ContramedidaGenerica : Pa_BaseObject
    {
        public string ContramedidaGenerica { get; set; }
        public static List<Pa_ContramedidaGenerica> Listar()
        {
            var query = "SELECT * FROM Pa_ContramedidaGenerica";
            return ListarGenerico<Pa_ContramedidaGenerica>(query);
        }

        public static Pa_ContramedidaGenerica Get(int Id)
        {
            var query = "SELECT * FROM Pa_ContramedidaGenerica WHERE Id = " + Id;
            return GetGenerico<Pa_ContramedidaGenerica>(query);
        }
    }
}

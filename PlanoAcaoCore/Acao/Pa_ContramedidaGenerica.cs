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

        public static Pa_ContramedidaGenerica Get(int id)
        {
            var query = "SELECT * FROM Pa_ContramedidaGenerica WHERE Id = " + id;
            return GetGenerico<Pa_ContramedidaGenerica>(query);
        }

        public static List<Pa_ContramedidaGenerica> ContramedidaGenericaPorGrupoCausa(int id)
        {
            var query = "select * from Pa_ContramedidaGenerica WHERE CausaGenerica = " + id;
            return ListarGenerico<Pa_ContramedidaGenerica>(query);
        }

    }
}

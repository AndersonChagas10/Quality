using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_IndicadorSgqAcao : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_IndicadorSgqAcao> Listar()
        {
            var query = "SELECT * FROM Pa_IndicadorSgqAcao";
            return ListarGenerico<Pa_IndicadorSgqAcao>(query);
        }

        public static Pa_IndicadorSgqAcao Get(int Id)
        {
            var query = "SELECT * FROM Pa_IndicadorSgqAcao WHERE Id = " + Id;
            return GetGenerico<Pa_IndicadorSgqAcao>(query);
        }

    }
}

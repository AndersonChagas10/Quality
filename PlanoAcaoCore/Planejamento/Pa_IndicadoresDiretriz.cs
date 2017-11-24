using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_IndicadoresDiretriz : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_IndicadoresDiretriz> Listar()
        {
            var query = "SELECT * FROM Pa_IndicadoresDiretriz WHERE IsActive = 1";
            return ListarGenerico<Pa_IndicadoresDiretriz>(query);
        }

        public static Pa_IndicadoresDiretriz Get(int Id)
        {
            var query = "SELECT * FROM Pa_IndicadoresDiretriz WHERE Id = " + Id;
            return GetGenerico<Pa_IndicadoresDiretriz>(query);
        }

        public static List<Pa_IndicadoresDiretriz> GetIndicadoresDiretrizXObjetivo(int id)
        {
            var query = "SELECT * FROM Pa_IndicadoresDiretriz WHERE IsActive = 1 AND pa_objetivo_id = " + id;
            return ListarGenerico<Pa_IndicadoresDiretriz>(query);
        }
    }
}


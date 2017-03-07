using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_IndicadoresDeProjeto : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_IndicadoresDeProjeto> Listar()
        {
            var query = "SELECT * FROM Pa_IndicadoresDeProjeto";
            return ListarGenerico<Pa_IndicadoresDeProjeto>(query);
        }

        public static Pa_IndicadoresDeProjeto Get(int Id)
        {
            var query = "SELECT * FROM Pa_IndicadoresDeProjeto WHERE Id = " + Id;
            return GetGenerico<Pa_IndicadoresDeProjeto>(query);
        }

        public static List<Pa_IndicadoresDeProjeto> GetIndicadoresProjetoXiniciativa(int id)
        {
            var query = "SELECT * FROM Pa_IndicadoresDeProjeto WHERE Pa_Iniciativa_Id = " + id;
            return ListarGenerico<Pa_IndicadoresDeProjeto>(query);
        }
    }
}


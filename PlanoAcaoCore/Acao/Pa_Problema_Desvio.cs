using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Problema_Desvio : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Problema_Desvio> Listar()
        {
            var query = "SELECT TOP 1000 MIN(id) AS Id , Name FROM Pa_Problema_Desvio GROUP BY name";
            return ListarGenerico<Pa_Problema_Desvio>(query);
        }

        public static Pa_Problema_Desvio Get(int Id)
        {
            var query = "SELECT * FROM Pa_Problema_Desvio WHERE Id = " + Id;
            return GetGenerico<Pa_Problema_Desvio>(query);
        }
        
    }
}
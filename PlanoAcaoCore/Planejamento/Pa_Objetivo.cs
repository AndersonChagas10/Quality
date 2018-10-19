﻿using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Objetivo : Pa_BaseObject
    {
        public string Name { get; set; }
        public bool IsPriority { get; set; }

        public static List<Pa_Objetivo> Listar()
        {
            var query = "SELECT * FROM Pa_Objetivo WHERE IsActive = 1";
            return ListarGenerico<Pa_Objetivo>(query);
        }

        public static Pa_Objetivo Get(int Id)
        {
            var query = "SELECT * FROM Pa_Objetivo WHERE Id = " + Id;
            return GetGenerico<Pa_Objetivo>(query);
        }

        public static List<Pa_Objetivo> GetObjetivoXDimensao(int id)
        {
            var query = "SELECT * FROM Pa_Objetivo WHERE IsActive = 1 AND pa_dimensao_id = " + id;
            return ListarGenerico<Pa_Objetivo>(query);
        }

        
    }
}

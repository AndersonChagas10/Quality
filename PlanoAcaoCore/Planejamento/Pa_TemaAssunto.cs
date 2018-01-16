﻿using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_TemaAssunto : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_TemaAssunto> Listar()
        {
            var query = "SELECT * FROM Pa_TemaAssunto WHERE IsActive = 1";
            return ListarGenerico<Pa_TemaAssunto>(query);
        }

        public static Pa_TemaAssunto Get(int Id)
        {
            var query = "SELECT * FROM Pa_TemaAssunto WHERE IsActive = 1 AND Id = " + Id;
            return GetGenerico<Pa_TemaAssunto>(query);
        }
    }
}

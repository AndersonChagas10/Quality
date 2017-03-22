using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_IndicadorSgqAcao : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Qualquer_Name> Listar(string campo)
        {
            var query = "SELECT * FROM " + campo + "";
            return ListarGenerico<Pa_Qualquer_Name>(query);
        }

        public static Pa_Qualquer_Name Get(int Id, string campo)
        {
            var query = "SELECT * FROM " + campo + " WHERE Id = " + Id;
            return GetGenerico<Pa_Qualquer_Name>(query);
        }

    }
}

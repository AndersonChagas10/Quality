using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Qualquer_Name : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Qualquer_Name> Listar(string campo)
        {
            switch (campo)
            {
                case "[Status]":
                    campo = "Pa_Status";
                    break;
                case "q.Id":
                    campo = "Pa_Quem";
                    break;
                case "Pa_IndicadorSgqAcao_Id":
                    campo = "Pa_IndicadorSgqAcao";
                    break;
                default:
                    break;
            }
            var query = "SELECT * FROM "+ campo +"";
            return ListarGenerico<Pa_Qualquer_Name>(query);
        }

        public static Pa_Qualquer_Name Get(int Id, string campo)
        {
            campo = campo.Replace("_id", "");
            var query = "SELECT * FROM "+ campo +" WHERE Id = " + Id;
            return GetGenerico<Pa_Qualquer_Name>(query);
        }
        
    }
}

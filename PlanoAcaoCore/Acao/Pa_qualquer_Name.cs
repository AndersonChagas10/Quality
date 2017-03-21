using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Qualquer_Name : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Qualquer_Name> Listar(string campo)
        {
            //ser somente para tabelas com id e name
            var tabela = "";

            switch (campo)
            {
                case "[Status]":
                    tabela = "Pa_Status";
                    break;
                case "q.Id":
                    tabela = "Pa_Quem";
                    break;
                case "Pa_IndicadorSgqAcao_Id":
                    tabela = "Pa_IndicadorSgqAcao";
                    break;
                default:
                    break;
            }
            var query = "SELECT * FROM "+ tabela + "";
            return ListarGenerico<Pa_Qualquer_Name>(query);
        }

        public static Pa_Qualquer_Name Get(int Id, string campo)
        {
            //ser somente para tabelas com id e name
            var tabela = "";

            switch (campo)
            {
                case "[Status]":
                    tabela = "Pa_Status";
                    break;
                case "q.Id":
                    tabela = "Pa_Quem";
                    break;
                case "Pa_IndicadorSgqAcao_Id":
                    tabela = "Pa_IndicadorSgqAcao";
                    break;
                default:
                    break;
            }

            var query = "SELECT * FROM "+ tabela +" WHERE Id = " + Id;
            return GetGenerico<Pa_Qualquer_Name>(query);
        }
        
    }
}

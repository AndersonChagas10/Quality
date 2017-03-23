using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoAcaoCore.Acao
{
    public class Pa_Acompanhamento : Pa_BaseObject
    {

        public string Name { get; set; }
        public static List<Pa_Acompanhamento> Listar()
        {
            var query = "SELECT * FROM Pa_Acompanhamento";
            return ListarGenerico<Pa_Acompanhamento>(query);
        }

        public static Pa_Acompanhamento Get(int Id)
        {
            var query = "SELECT * FROM Pa_Acompanhamento WHERE Id = " + Id;
            return GetGenerico<Pa_Acompanhamento>(query);
        }

    }
}

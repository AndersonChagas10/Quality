using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Departamento : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Departamento> Listar()
        {
            var query = "SELECT * FROM Pa_Departamento";
            return ListarGenerico<Pa_Departamento>(query);
        }

        public static Pa_Departamento Get(int Id)
        {
            var query = "SELECT * FROM Pa_Departamento WHERE Id = " + Id;
            return GetGenerico<Pa_Departamento>(query);
        }
    }
}

using System.Collections.Generic;


namespace PlanoAcaoCore
{
    public class Pa_TemaProjeto : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_TemaProjeto> Listar()
        {
            var query = "SELECT * FROM Pa_TemaProjeto WHERE IsActive = 1";
            return ListarGenerico<Pa_TemaProjeto>(query);
        }

        public static Pa_TemaProjeto Get(int Id)
        {
            var query = "SELECT * FROM Pa_TemaProjeto WHERE IsActive = 1 AND Id = " + Id;
            return GetGenerico<Pa_TemaProjeto>(query);
        }
    }
}

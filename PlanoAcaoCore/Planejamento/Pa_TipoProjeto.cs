using System.Collections.Generic;


namespace PlanoAcaoCore
{
    public class Pa_TipoProjeto : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_TipoProjeto> Listar()
        {
            var query = "SELECT * FROM Pa_TipoProjeto WHERE IsActive = 1";
            return ListarGenerico<Pa_TipoProjeto>(query);
        }

        public static Pa_TipoProjeto Get(int Id)
        {
            var query = "SELECT * FROM Pa_TipoProjeto WHERE IsActive = 1 AND Id = " + Id;
            return GetGenerico<Pa_TipoProjeto>(query);
        }
    }
}

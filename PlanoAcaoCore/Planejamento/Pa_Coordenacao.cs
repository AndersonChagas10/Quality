using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Coordenacao : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Coordenacao> Listar()
        {
            var query = "SELECT * FROM Pa_Coordenacao";
            return ListarGenerico<Pa_Coordenacao>(query);
        }

        public static Pa_Coordenacao Get(int Id)
        {
            var query = "SELECT * FROM Pa_Coordenacao WHERE Id = " + Id;
            return GetGenerico<Pa_Coordenacao>(query);
        }

        public static List<Pa_Coordenacao> GetCoordenacaoByGerencia(int Id)
        {
            var query = "SELECT * FROM Pa_Coordenacao WHERE GERENCIA_ID = " + Id;
            return ListarGenerico<Pa_Coordenacao>(query);
        }
        

    }
}

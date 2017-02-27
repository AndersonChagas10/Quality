using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Quem : Pa_BaseObject
    {
        public string Name { get; set; }
        public static List<Pa_Quem> Listar()
        {
            var query = "SELECT * FROM Pa_Quem";
            return ListarGenerico<Pa_Quem>(query);
        }

        public static Pa_Quem Get(int Id)
        {
            var query = "SELECT * FROM Pa_Quem WHERE Id = " + Id;
            return GetGenerico<Pa_Quem>(query);
        }

        internal static List<Pa_Quem> GetQuemXAcao(int IdAcao)
        {
            var query = "SELECT Q.* FROM Pa_AcaoXQuem AQ INNER JOIN Pa_Quem Q on q.Id = AQ.Quem_Id where AQ.Acao_Id =" + IdAcao;
            return ListarGenerico<Pa_Quem>(query);
        }
    }
}

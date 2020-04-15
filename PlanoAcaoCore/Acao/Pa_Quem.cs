using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoAcaoCore
{
    public class Pa_Quem : Pa_BaseObject
    {
        public string Name { get; set; }

        public string _FullName { get; set; }

        public string _FullNameConcatenado
        {
            get
            {
                try
                {
                    return _FullName + " - " + Name.Substring(0, 3);
                }
                catch
                {
                    return null;
                } 
            } 
        }

        public int? UserSgq_Id { get; set; }

        public static List<Pa_Quem> Listar()
        {
            var query = $@"SELECT * FROM Pa_Quem";

            return ListarGenerico<Pa_Quem>(query);
        }

        public static List<Pa_Quem> ListarFTA()
        {
            var query = $@"SELECT
                        	paq.Id
                           ,usgq.Name
                           ,usgq.FullName as _FullName
                           ,paq.UserSgq_Id
                        FROM Pa_Quem paq
                        LEFT JOIN UserSgq usgq ON usgq.Id = paq.UserSgq_Id";

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

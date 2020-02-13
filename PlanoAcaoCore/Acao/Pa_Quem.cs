using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Quem : Pa_BaseObject
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string FullNameConcatenado
        {
            get
            {
                try
                {
                    return FullName + " - " + Name.Substring(0, 3);
                }
                catch
                {
                    return null;
                } 
            } 
        }
        public static List<Pa_Quem> Listar()
        {
            var query = $@"select paq.Id 
                         ,paq.Name
                         ,usgq.FullName
                         FROM Pa_Quem paq  
                         inner join UserSgq usgq
                         on paq.Name = usgq.Name 
                         where paq.Id = usgq.Id";
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

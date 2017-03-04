using System.Collections.Generic;
using System.Data.SqlClient;

namespace PlanoAcaoCore
{
    public class Pa_AcaoXQuem : Pa_BaseObject, ICrudPa<Pa_CausaMedidasXAcao>
    {
        public int Acao_Id { get; set; }
        public int Quem_Id { get; set; }

        internal static IEnumerable<Pa_AcaoXQuem> Get(int idAcao)
        {
            var query = "SELECT AQ.* FROM Pa_AcaoXQuem AQ INNER JOIN Pa_Quem Q on q.Id = AQ.Quem_Id where AQ.Acao_Id =" + idAcao;
            return ListarGenerico<Pa_AcaoXQuem>(query);
        }

        public void AddOrUpdate()
        {
            IsValid();
            string query;
            if (Id > 0)
            {
                query = "UPDATE [dbo].[Pa_AcaoXQuem]            " +
                   "\n    SET [Acao_Id] = @Acao_Id              " +
                   "\n       ,[Quem_Id] = @Quem_Id              " +
                   "\n  WHERE Id = @Id SELECT CAST(@Id AS int)  ";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Acao_Id", Acao_Id);
                cmd.Parameters.AddWithValue("@Quem_Id", Quem_Id);
                cmd.Parameters.AddWithValue("@Id", Id);

                Id = Salvar(cmd);
            }
            else
            {
                query = "INSERT INTO [dbo].[Pa_AcaoXQuem]           " +
                  "\n        ([Acao_Id]                               " +
                  "\n        ,[Quem_Id])                              " +
                  "\n  VALUES                                         " +
                  "\n        (@Acao_Id                             " +
                  "\n        ,@Quem_Id)SELECT CAST(scope_identity() AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Acao_Id", Acao_Id);
                cmd.Parameters.AddWithValue("@Quem_Id", Quem_Id);

                Id = Salvar(cmd);
            }

        }

        public void IsValid()
        {
            //throw new NotImplementedException();
        }
    }
}

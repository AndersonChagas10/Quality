using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace PlanoAcaoCore
{
    public class Pa_CausaMedidasXAcao : Pa_BaseObject, ICrudPa<Pa_CausaMedidasXAcao>
    {

        [Display(Name = "Causa generica")]
        public int CausaGenerica_Id { get; set; }
        public string CausaGenerica { get; set; }

        [Display(Name = "Causa especifica")]
        public int CausaEspecifica_Id { get; set; }
        public string CausaEspecifica { get; set; }

        [Display(Name = "Contramedida generica")]
        public int ContramedidaGenerica_Id { get; set; }
        public string ContramedidaGenerica { get; set; }

        [Display(Name = "Contramedida especifica")]
        public int ContramedidaEspecifica_Id { get; set; }
        public string ContramedidaEspecifica { get; set; }

        [Display(Name = "Grupo causa")]
        public int GrupoCausa_Id { get; set; }
        public string GrupoCausa { get; set; }

        [Display(Name = "Acao")]
        public int Acao_Id { get; set; }

        public void AddOrUpdate()
        {
            IsValid();
            string query;
            if (Id > 0)
            {
                query = "UPDATE [dbo].[Pa_AcaoXQuem]                                  " +
                   "\n    SET [CausaGenerica_Id] = @CausaGenerica_Id                    " +
                   "\n       ,[CausaEspecifica_Id] = @CausaEspecifica_Id                " +
                   "\n       ,[ContramedidaGenerica_Id] = @ContramedidaGenerica_Id      " +
                   "\n       ,[ContramedidaEspecifica_Id] = @ContramedidaEspecifica_Id  " +
                   "\n       ,[GrupoCausa_Id] = @GrupoCausa_Id                          " +
                   "\n       ,[Acao_Id] = @Acao_Id                                      " +
                   "\n  WHERE Id = @Id ";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@CausaGenerica_Id", CausaGenerica_Id);
                cmd.Parameters.AddWithValue("@CausaEspecifica_Id", CausaEspecifica_Id);
                cmd.Parameters.AddWithValue("@ContramedidaGenerica_Id", ContramedidaGenerica_Id);
                cmd.Parameters.AddWithValue("@ContramedidaEspecifica_Id", ContramedidaEspecifica_Id);
                cmd.Parameters.AddWithValue("@GrupoCausa_Id", GrupoCausa_Id);
                cmd.Parameters.AddWithValue("@Acao_Id", Acao_Id);

                Id = Salvar(cmd);
            }
            else
            {
                query = "INSERT INTO[dbo].[Pa_CausaMedidaXAcao]     " +
              "\n    ([CausaGenerica_Id]                                 " +
              "\n    ,[CausaEspecifica_Id]                               " +
              "\n    ,[ContramedidaGenerica_Id]                          " +
              "\n    ,[ContramedidaEspecifica_Id]                        " +
              "\n    ,[GrupoCausa_Id]                                    " +
              "\n    ,[Acao_Id])                                         " +
              "\n VALUES                                                 " +
              "\n    (@CausaGenerica_Id                                  " +
              "\n    ,@CausaEspecifica_Id                                " +
              "\n    ,@ContramedidaGenerica_Id                           " +
              "\n    ,@ContramedidaEspecifica_Id                         " +
              "\n    ,@GrupoCausa_Id                                     " +
              "\n    ,@Acao_Id)SELECT CAST(scope_identity() AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@CausaGenerica_Id", CausaGenerica_Id);
                cmd.Parameters.AddWithValue("@CausaEspecifica_Id", CausaEspecifica_Id);
                cmd.Parameters.AddWithValue("@ContramedidaGenerica_Id", ContramedidaGenerica_Id);
                cmd.Parameters.AddWithValue("@ContramedidaEspecifica_Id", ContramedidaEspecifica_Id);
                cmd.Parameters.AddWithValue("@GrupoCausa_Id", GrupoCausa_Id);
                cmd.Parameters.AddWithValue("@Acao_Id", Acao_Id);

                Id = Salvar(cmd);
            }
        }

        public void IsValid()
        {
            //throw new NotImplementedException();
        }
    }
}

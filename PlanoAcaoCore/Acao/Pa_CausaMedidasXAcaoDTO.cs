using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace PlanoAcaoCore
{
    public class Pa_CausaMedidasXAcao : Pa_BaseObject, ICrudPa<Pa_CausaMedidasXAcao>
    {

        [Display(Name = "Causa generica")]
        public int? CausaGenerica_Id { get; set; }
        public string CausaGenerica { get; set; }

        [Display(Name = "Causa especifica")]
        public int? CausaEspecifica_Id { get; set; }
        public string CausaEspecifica { get; set; }

        [Display(Name = "Contramedida generica")]
        public int? ContramedidaGenerica_Id { get; set; }
        public string ContramedidaGenerica { get; set; }

        [Display(Name = "Contramedida especifica")]
        public int? ContramedidaEspecifica_Id { get; set; }
        public string ContramedidaEspecifica { get; set; }

        [Display(Name = "Grupo causa")]
        public int? GrupoCausa_Id { get; set; }
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
                   "\n    SET  ";
                if (CausaEspecifica_Id != null)
                {
                    query += "\n [CausaGenerica_Id] = @CausaGenerica_Id                    ";
                }
                if (CausaEspecifica_Id != null)
                {
                    query += "\n       ,[CausaEspecifica_Id] = @CausaEspecifica_Id                ";
                }
                if (ContramedidaGenerica_Id != null)
                {
                    query += "\n       ,[ContramedidaGenerica_Id] = @ContramedidaGenerica_Id                ";
                }
                if (ContramedidaEspecifica_Id != null)
                {
                    query += "\n       ,[ContramedidaEspecifica_Id] = @ContramedidaEspecifica_Id                ";
                }
                if (GrupoCausa_Id != null)
                {
                    query += "\n       ,[GrupoCausa_Id] = @GrupoCausa_Id                ";
                }

                query += "\n  WHERE Id = @Id ";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                if (CausaGenerica_Id != null)
                    cmd.Parameters.AddWithValue("@CausaGenerica_Id", CausaGenerica_Id);
                if (CausaEspecifica_Id != null)
                    cmd.Parameters.AddWithValue("@CausaEspecifica_Id", CausaEspecifica_Id);
                if (ContramedidaGenerica_Id != null)
                    cmd.Parameters.AddWithValue("@ContramedidaGenerica_Id", ContramedidaGenerica_Id);
                if (ContramedidaEspecifica_Id != null)
                    cmd.Parameters.AddWithValue("@ContramedidaEspecifica_Id", ContramedidaEspecifica_Id);
                if (GrupoCausa_Id != null)
                    cmd.Parameters.AddWithValue("@GrupoCausa_Id", GrupoCausa_Id);

                cmd.Parameters.AddWithValue("@Acao_Id", Acao_Id);
                cmd.Parameters.AddWithValue("@Id", Id);

                Id = Salvar(cmd);
            }
            else
            {
                query = "INSERT INTO[dbo].[Pa_CausaMedidaXAcao]  (";
                if (CausaGenerica_Id != null)
                    query += "\n    [CausaGenerica_Id],";
                if (CausaEspecifica_Id != null)
                    query += "\n    [CausaEspecifica_Id],";
                if (ContramedidaGenerica_Id != null)
                    query += "\n    [ContramedidaGenerica_Id],";
                if (ContramedidaEspecifica_Id != null)
                    query += "\n    [ContramedidaEspecifica_Id],";
                if (GrupoCausa_Id != null)
                    query += "\n    [GrupoCausa_Id],";

                query += "\n    [Acao_Id])" +
                "\n VALUES                                                 " +
                "\n    (";

                if (CausaGenerica_Id != null)
                    query += "\n    @CausaGenerica_Id,";
                if (CausaEspecifica_Id != null)
                    query += "\n    @CausaEspecifica_Id,";
                if (ContramedidaGenerica_Id != null)
                    query += "\n    @ContramedidaGenerica_Id,";
                if (ContramedidaEspecifica_Id != null)
                    query += "\n    @ContramedidaEspecifica_Id,";
                if (GrupoCausa_Id != null)
                    query += "\n    @GrupoCausa_Id,";

                query += "\n    @Acao_Id)SELECT CAST(scope_identity() AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                if (CausaGenerica_Id != null)
                    cmd.Parameters.AddWithValue("@CausaGenerica_Id", CausaGenerica_Id);
                if (CausaEspecifica_Id != null)
                    cmd.Parameters.AddWithValue("@CausaEspecifica_Id", CausaEspecifica_Id);
                if (ContramedidaGenerica_Id != null)
                    cmd.Parameters.AddWithValue("@ContramedidaGenerica_Id", ContramedidaGenerica_Id);
                if (ContramedidaEspecifica_Id != null)
                    cmd.Parameters.AddWithValue("@ContramedidaEspecifica_Id", ContramedidaEspecifica_Id);
                if (GrupoCausa_Id != null)
                    cmd.Parameters.AddWithValue("@GrupoCausa_Id", GrupoCausa_Id);

                cmd.Parameters.AddWithValue("@Acao_Id", Acao_Id);

                Id = Salvar(cmd);
            }
        }

        public void IsValid()
        {
            //throw new NotImplementedException();
        }

        //public static List<Pa_Acao> Listar()
        //{
        //    var query = "SELECT ACAO.* ,                                                    " +
        //                "\n STA.Name as StatusName,                                         " +
        //                "\n UN.Name as Unidade,                                             " +
        //                "\n DPT.Name as Departamento                                       " +
        //                "\n FROM pa_acao ACAO                                               " +
        //                "\n LEFT JOIN Pa_Unidade UN ON UN.Id = ACAO.Unidade_Id              " +
        //                "\n LEFT JOIN Pa_Departamento DPT ON DPT.Id = ACAO.Departamento_Id  " +
        //                "\n LEFT JOIN Pa_Status STA ON STA.Id = ACAO.[Status];              ";
        //    var retorno = ListarGenerico<Pa_Acao>(query);
        //    foreach (var i in retorno)
        //    {
        //        i._Quem = Pa_Quem.GetQuemXAcao(i.Id).Select(r => r.Name).ToList();
        //    }
        //    return retorno;
        //}

        public static Pa_CausaMedidasXAcao Get(int Id)
        {
            var query = "select *                                                                                   "+ 
                        "\n from Pa_CausaMedidaXAcao CMA                                                            "+
                        "\n left join Pa_CausaGenerica CG on CMA.CausaGEnerica_Id = CG.Id                           "+
                        "\n left join Pa_CausaEspecifica CE on CMA.CausaEspecifica_Id = CE.Id                       "+
                        "\n left join Pa_ContramedidaGenerica CMG on CMA.ContramedidaGenerica_Id = CMG.Id           "+
                        "\n left join Pa_ContramedidaEspecifica CME on CMA.ContramedidaEspecifica_Id = CME.Id       "+
                        "\n left join Pa_GrupoCausa GC on CMA.GrupoCausa_Id = GC.Id                                 "+
                        "\n where CMA.Acao_Id = " + Id;

            return GetGenerico<Pa_CausaMedidasXAcao>(query);
        }
    }
}

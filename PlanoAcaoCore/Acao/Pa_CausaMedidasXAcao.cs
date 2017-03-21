using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace PlanoAcaoCore
{
    public class Pa_CausaMedidasXAcao : Pa_BaseObject, ICrudPa<Pa_CausaMedidasXAcao>
    {



        [Display(Name = "Causa generica")]
        public int? CausaGenerica_Id { get; set; }
        public string CausaGenerica
        {
            get
            {
                var retorno = string.Empty;
                if (Acao_Id.GetValueOrDefault() > 0 && CausaGenerica_Id.GetValueOrDefault() > 0)
                {
                    retorno = Pa_CausaGenerica.Get(CausaGenerica_Id.GetValueOrDefault())?.CausaGenerica;
                }
                return retorno;
            }
        }

        [Display(Name = "Causa especifica")]
        public string _CausaEspecifica { get; set; }// ID no DB
        public int? CausaEspecifica_Id { get; set; }// Usada para SAVE/UPDATE
        public string CausaEspecifica
        {
            get
            {
                var retorno = string.Empty;
                if (Acao_Id.GetValueOrDefault() > 0 && CausaEspecifica_Id.GetValueOrDefault() > 0)
                {
                    retorno = Pa_CausaEspecifica.Get(CausaEspecifica_Id.GetValueOrDefault())?.Text;
                }
                return retorno;
            }
        }//Valor em String ndo DB

        [Display(Name = "Contramedida generica")]
        public int? ContramedidaGenerica_Id { get; set; } // ID no DB
        public string _ContramedidaEspecifica { get; set; } // Usada para SAVE/UPDATE
        public string ContramedidaGenerica
        {
            get
            {
                var retorno = string.Empty;
                if (Acao_Id.GetValueOrDefault() > 0 && ContramedidaGenerica_Id.GetValueOrDefault() > 0)
                {
                    retorno = Pa_ContramedidaGenerica.Get(ContramedidaGenerica_Id.GetValueOrDefault())?.ContramedidaGenerica;
                }
                return retorno;
            }
        } //Valor em String ndo DB

        [Display(Name = "Contramedida especifica")]
        public int? ContramedidaEspecifica_Id { get; set; }
        public string ContramedidaEspecifica
        {
            get
            {
                var retorno = string.Empty;
                if (Acao_Id.GetValueOrDefault() > 0 && ContramedidaEspecifica_Id.GetValueOrDefault() > 0)
                {
                    retorno = Pa_ContramedidaEspecifica.Get(ContramedidaEspecifica_Id.GetValueOrDefault())?.Text;
                }
                return retorno;
            }
        }

        [Display(Name = "Grupo causa")]
        public int? GrupoCausa_Id { get; set; }
        public string GrupoCausa
        {
            get
            {
                var retorno = string.Empty;
                if (Acao_Id.GetValueOrDefault() > 0 && GrupoCausa_Id.GetValueOrDefault() > 0)
                {
                    retorno = Pa_GrupoCausa.Get(GrupoCausa_Id.GetValueOrDefault())?.GrupoCausa;
                }
                return retorno;
            }
        }

        [Display(Name = "Acao")]
        public int? Acao_Id { get; set; }

        public void AddOrUpdate()
        {
            IsValid();
            string query;
            if (Id > 0)
            {
                query = "UPDATE [dbo].[Pa_CausaMedidaXAcao]  SET ";
                if (CausaGenerica_Id != null)
                    query += "\n    CausaGenerica_Id = @CausaGenerica_Id,";
                if (CausaEspecifica_Id != null)
                    query += "\n    CausaEspecifica_Id = @CausaEspecifica_Id,";
                if (ContramedidaGenerica_Id != null)
                    query += "\n    ContramedidaGenerica_Id = @ContramedidaGenerica_Id,";
                if (ContramedidaEspecifica_Id != null)
                    query += "\n    ContramedidaEspecifica_Id = @ContramedidaEspecifica_Id,";
                if (GrupoCausa_Id != null)
                    query += "\n    GrupoCausa_Id = @GrupoCausa_Id,";

                query = query.TrimEnd(',');

                query += "\n  WHERE Id = @Id SELECT CAST(@Id AS int)";

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

                cmd.Parameters.AddWithValue("@Id", Id);

                Salvar(cmd);
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

        internal static Pa_CausaMedidasXAcao GetByAcaoId(int id)
        {

            var query3 = "select * from Pa_CausaMedidaXAcao where Acao_Id = " + id;
            var CausaMedidaXAcoes = GetGenerico<Pa_CausaMedidasXAcao>(query3);

            //if (CausaMedidaXAcoes != null)
            //{
            //    if (CausaMedidaXAcoes.GrupoCausa_Id.GetValueOrDefault() > 0)
            //    {
            //        var query = "select * from Pa_GrupoCausa where Id = " + id;
            //        var GruposCausas = ListarGenerico<Pa_GrupoCausa>(query);
            //        CausaMedidaXAcoes.GrupoCausa = GruposCausas.FirstOrDefault(r => r.Id == CausaMedidaXAcoes.GrupoCausa_Id)?.GrupoCausa;
            //    }

            //    if (CausaMedidaXAcoes.ContramedidaGenerica_Id.GetValueOrDefault() > 0)
            //    {
            //        var query1 = "select * from Pa_ContramedidaGenerica where Id = " + id;
            //        var ContramedidasGenericas = ListarGenerico<Pa_ContramedidaGenerica>(query1);
            //        CausaMedidaXAcoes.ContramedidaGenerica = ContramedidasGenericas.FirstOrDefault(r => r.Id == CausaMedidaXAcoes.ContramedidaGenerica_Id)?.ContramedidaGenerica;
            //    }

            //    if (CausaMedidaXAcoes.CausaGenerica_Id.GetValueOrDefault() > 0)
            //    {
            //        var query2 = "select * from Pa_CausaGenerica where Id = " + id;
            //        var CausasGenericas = ListarGenerico<Pa_CausaGenerica>(query2);
            //        CausaMedidaXAcoes.CausaGenerica = CausasGenericas.FirstOrDefault(r => r.Id == CausaMedidaXAcoes.CausaGenerica_Id)?.CausaGenerica;
            //    }

            //    if (CausaMedidaXAcoes.CausaEspecifica_Id.GetValueOrDefault() > 0)
            //    {
            //        var query4 = "select * from Pa_CausaEspecifica where Id = " + id;
            //        var causaEspecifica = ListarGenerico<Pa_CausaEspecifica>(query4);
            //        CausaMedidaXAcoes.CausaEspecifica = causaEspecifica.FirstOrDefault(r => r.Id == CausaMedidaXAcoes.CausaEspecifica_Id)?.Text;
            //    }

            //    if (CausaMedidaXAcoes.ContramedidaEspecifica_Id.GetValueOrDefault() > 0)
            //    {
            //        var query5 = "select * from Pa_ContramedidaEspecifica where Id = " + id;
            //        var contramedidaEspecifica = ListarGenerico<Pa_ContramedidaEspecifica>(query5);
            //        CausaMedidaXAcoes.ContramedidaEspecifica = contramedidaEspecifica.FirstOrDefault(r => r.Id == CausaMedidaXAcoes.ContramedidaEspecifica_Id)?.Text;
            //    }

            //}

            return CausaMedidaXAcoes;
        }

        public void IsValid()
        {
            //if (string.IsNullOrEmpty(_CausaEspecifica))
            //    message += "\n Causa Específica,";

            //if (string.IsNullOrEmpty(_ContramedidaEspecifica))
            //    message += "\n Contramedida Específica,";

            //VerificaMensagemCamposObrigatorios(message);
            //throw new NotImplementedException();
        }

        public static Pa_CausaMedidasXAcao Get(int Id)
        {
            var query = "select *                                                                                   " +
                        "\n from Pa_CausaMedidaXAcao CMA                                                            " +
                        "\n left join Pa_CausaGenerica CG on CMA.CausaGEnerica_Id = CG.Id                           " +
                        "\n left join Pa_CausaEspecifica CE on CMA.CausaEspecifica_Id = CE.Id                       " +
                        "\n left join Pa_ContramedidaGenerica CMG on CMA.ContramedidaGenerica_Id = CMG.Id           " +
                        "\n left join Pa_ContramedidaEspecifica CME on CMA.ContramedidaEspecifica_Id = CME.Id       " +
                        "\n left join Pa_GrupoCausa GC on CMA.GrupoCausa_Id = GC.Id                                 " +
                        "\n where CMA.Acao_Id = " + Id;

            return GetGenerico<Pa_CausaMedidasXAcao>(query);
        }
    }
}

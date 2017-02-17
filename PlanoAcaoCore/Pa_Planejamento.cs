using ADOFactory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_Planejamento : Pa_BaseObject, ICrudPa<Pa_Planejamento>
    {
       public int Diretoria_Id { get; set; }
       public int Gerencia_Id { get; set; }
       public int Coordenacao_Id { get; set; }
       public int Missao_Id { get; set; }
       public int Visao_Id { get; set; }
       public int TemaAssunto_Id { get; set; }
       public int Indicadores_Id { get; set; }
       public int Iniciativa_Id { get; set; }
       public int ObjetivoGerencial_Id { get; set; }
       public string Dimensao { get; set; }
       public string Objetivo { get; set; }
       public decimal ValorDe { get; set; }
       public decimal ValorPara { get; set; }
       public DateTime DataInicio { get; set; }
       public DateTime DataFim { get; set; }

        public void IsValid()
        {
            //Name = Guard.CheckStringFullSimple(Name);
        }

        public static List<Pa_Planejamento> Listar()
        {
            List<Pa_Planejamento> listReturn;
            var query = "SELECT * FROM Pa_Planejamento";
            using (var db = new Factory(""))
                listReturn = db.SearchQuery<Pa_Planejamento>(query);

            return listReturn;
        }

        public static Pa_Planejamento Get(int Id)
        {
            var query = "SELECT * FROM Pa_Planejamento WHERE Id = "  + Id;
            return GetGenerico<Pa_Planejamento>(query);
        }

        public void AddOrUpdate()
        {
            IsValid();
            string query;
            if (Id > 0)
            {
                query =" UPDATE[dbo].[Pa_Planejamento]                          "+
                       "\n SET                                                  "+
                       "\n    ,[AlterDate] = @AlterDate                         "+
                       "\n    ,[Diretoria_Id] = @Diretoria_Id                   "+
                       "\n    ,[Gerencia_Id] = @Gerencia_Id                     "+
                       "\n    ,[Coordenacao_Id] = @Coordenacao_Id               "+
                       "\n    ,[Missao_Id] = @Missao_Id                         "+
                       "\n    ,[Visao_Id] = @Visao_Id                           "+
                       "\n    ,[TemaAssunto_Id] = @TemaAssunto_Id               "+
                       "\n    ,[Indicadores_Id] = @Indicadores_Id               "+
                       "\n    ,[Iniciativa_Id] = @Iniciativa_Id                 "+
                       "\n    ,[ObjetivoGerencial_Id] = @ObjetivoGerencial_Id   "+
                       "\n    ,[Dimensao] = @Dimensao                           "+
                       "\n    ,[Objetivo] = @Objetivo                           "+
                       "\n    ,[ValorDe] = @ValorDe                             "+
                       "\n    ,[ValorPara] = @ValorPara                         "+
                       "\n    ,[DataInicio] = @DataInicio                       "+
                       "\n    ,[DataFim] = @DataFim                             "+
                       "\n WHERE Id = @Id                                       ";


                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Diretoria_Id", Diretoria_Id);
                cmd.Parameters.AddWithValue("@Gerencia_Id", Gerencia_Id);
                cmd.Parameters.AddWithValue("@Coordenacao_Id", Coordenacao_Id);
                cmd.Parameters.AddWithValue("@Missao_Id", Missao_Id);
                cmd.Parameters.AddWithValue("@Visao_Id", Visao_Id);
                cmd.Parameters.AddWithValue("@TemaAssunto_Id", TemaAssunto_Id);
                cmd.Parameters.AddWithValue("@Indicadores_Id", Indicadores_Id);
                cmd.Parameters.AddWithValue("@Iniciativa_Id", Iniciativa_Id);
                cmd.Parameters.AddWithValue("@ObjetivoGerencial_Id", ObjetivoGerencial_Id);
                cmd.Parameters.AddWithValue("@Dimensao", Dimensao);
                cmd.Parameters.AddWithValue("@Objetivo", Objetivo);
                cmd.Parameters.AddWithValue("@ValorDe", ValorDe);
                cmd.Parameters.AddWithValue("@ValorPara", ValorPara);
                cmd.Parameters.AddWithValue("@DataInicio", DataInicio);
                cmd.Parameters.AddWithValue("@DataFim", DataFim);

                Update(cmd);
            }
            else
            {
                query = "INSERT INTO [dbo].[Pa_Planejamento]            " +
                        //"\n       ([AddDate]                          " +
                        //"\n       ,[AlterDate]                        " +
                        "\n       ([Diretoria_Id]                       " +
                        "\n       ,[Gerencia_Id]                        " +
                        "\n       ,[Coordenacao_Id]                     " +
                        "\n       ,[Missao_Id]                          " +
                        "\n       ,[Visao_Id]                           " +
                        "\n       ,[TemaAssunto_Id]                     " +
                        "\n       ,[Indicadores_Id]                     " +
                        "\n       ,[Iniciativa_Id]                      " +
                        "\n       ,[ObjetivoGerencial_Id]               " +
                        "\n       ,[Dimensao]                           " +
                        "\n       ,[Objetivo]                           " +
                        "\n       ,[ValorDe]                            " +
                        "\n       ,[ValorPara]                          " +
                        "\n       ,[DataInicio]                         " +
                        "\n       ,[DataFim])                           " +
                        //"\n       ,[Order])                           " +
                        "\n VALUES                                      " +
                        //"\n       (<AddDate, datetime2(7),>           " +
                        //"\n       ,<AlterDate, datetime2(7),>         " +
                        "\n       (@Diretoria_Id                        " +
                        "\n       ,@Gerencia_Id                         " +
                        "\n       ,@Coordenacao_Id                      " +
                        "\n       ,@Missao_Id                           " +
                        "\n       ,@Visao_Id                            " +
                        "\n       ,@TemaAssunto_Id                      " +
                        "\n       ,@Indicadores_Id                      " +
                        "\n       ,@Iniciativa_Id                       " +
                        "\n       ,@ObjetivoGerencial_Id                " +
                        "\n       ,@Dimensao                            " +
                        "\n       ,@Objetivo                            " +
                        "\n       ,@ValorDe                             " +
                        "\n       ,@ValorPara                           " +
                        "\n       ,@DataInicio                          " +
                        "\n       ,@DataFim)                            " +
                        "\n       SELECT CAST(scope_identity() AS int)   ";
                        //"\n       ,@Order )                             ";


                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Diretoria_Id", Diretoria_Id);
                cmd.Parameters.AddWithValue("@Gerencia_Id", Gerencia_Id);
                cmd.Parameters.AddWithValue("@Coordenacao_Id", Coordenacao_Id);
                cmd.Parameters.AddWithValue("@Missao_Id", Missao_Id);
                cmd.Parameters.AddWithValue("@Visao_Id", Visao_Id);
                cmd.Parameters.AddWithValue("@TemaAssunto_Id", TemaAssunto_Id);
                cmd.Parameters.AddWithValue("@Indicadores_Id", Indicadores_Id);
                cmd.Parameters.AddWithValue("@Iniciativa_Id", Iniciativa_Id);
                cmd.Parameters.AddWithValue("@ObjetivoGerencial_Id", ObjetivoGerencial_Id);
                cmd.Parameters.AddWithValue("@Dimensao", Dimensao);
                cmd.Parameters.AddWithValue("@Objetivo", Objetivo);
                cmd.Parameters.AddWithValue("@ValorDe", ValorDe);
                cmd.Parameters.AddWithValue("@ValorPara", ValorPara);
                cmd.Parameters.AddWithValue("@DataInicio", DataInicio);
                cmd.Parameters.AddWithValue("@DataFim", DataFim);

                Id = Salvar(cmd);
            }
        }

      
    }
}

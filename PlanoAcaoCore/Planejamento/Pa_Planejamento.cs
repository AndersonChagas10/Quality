using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_Planejamento : Pa_BaseObject, ICrudPa<Pa_Planejamento>
    {
        [Display(Name = "Diretoria")]
        public int Diretoria_Id { get; set; }
        public string Diretoria { get; set; }

        [Display(Name = "Gerência")]
        public int Gerencia_Id { get; set; }
        public string Gerencia { get; set; }

        [Display(Name = "Coordenação")]
        public int Coordenacao_Id { get; set; }
        public string Coordenacao { get; set; }

        [Display(Name = "Missão")]
        public int Missao_Id { get; set; }
        public string Missao { get; set; }

        [Display(Name = "Visão")]
        public int Visao_Id { get; set; }
        public string Visao { get; set; }

        [Display(Name = "Tema / Assunto")]
        public int TemaAssunto_Id { get; set; }
        public string TemaAssunto { get; set; }

        [Display(Name = "Indicadores diretriz")]
        public int IndicadoresDiretriz_Id { get; set; }
        public string IndicadoresDiretriz { get; set; }


        [Display(Name = "Indicadores de projeto")]
        public int IndicadoresDeProjeto_Id { get; set; }
        public string IndicadoresDeProjeto { get; set; }

        [Display(Name = "Iniciativa")]
        public int Iniciativa_Id { get; set; }
        public string Iniciativa { get; set; }

        [Display(Name = "Objetivos Gerenciais")]
        public int ObjetivoGerencial_Id { get; set; }
        public string ObjetivoGerencial { get; set; }

        [Display(Name = "Dimensão")]
        public int Dimensao_Id { get; set; }
        public string Dimensao { get; set; }

        [Display(Name = "Objetivo")]
        public int Objetivo_Id { get; set; }
        public string Objetivo { get; set; }

        [Display(Name = "Valor de")]
        public decimal ValorDe { get; set; }

        [Display(Name = "Valor para")]
        public decimal ValorPara { get; set; }

        [Display(Name = "Data inicio")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "_DataInicio")]
        public string _DataInicio { get; set; }

        [Display(Name = "Data fim")]
        public DateTime DataFim { get; set; }

        [Display(Name = "_DataFim")]
        public string _DataFim { get; set; }

        public Pa_Acao Acao { get; set; }



        public void IsValid()
        {
            //Name = Guard.CheckStringFullSimple(Name);
        }

        private static string query
        {
            get
            {
                return "SELECT Pl.*,                                                                         " +
                    "\n INI.Name AS Inciativa,                                                               " +
                    "\n DIR.Name AS Diretoria,                                                               " +
                    "\n GER.Name AS Gerencia,                                                                " +
                    "\n CORD.Name AS Coordenacao,                                                            " +
                    "\n MISS.Name AS Missao,                                                                 " +
                    "\n VIS.Name AS Visao,                                                                   " +
                    "\n TEM.Name AS TemaAssunto,                                                             " +
                    "\n INDIC.Name AS IndicadoresDiretriz,                                                   " +
                    "\n INDICProj.Name AS IndicadoresDeProjeto,                                              " +
                    "\n OBJT.Name AS ObjetivoGerencial,                                                      " +
                    "\n OBJ.Name AS Dimensao,                                                                " +
                    "\n INICI.Name AS Iniciativa,                                                            " +
                    "\n DIME.Name AS Objetivo                                                                " +
                    "\n  FROM Pa_planejamento Pl                                                             " +
                    "\n LEFT JOIN Pa_Iniciativa INI on INI.Id = Pl.Iniciativa_Id                             " +
                    "\n LEFT JOIN Pa_Diretoria DIR on DIR.Id = Pl.Diretoria_Id                               " +
                    "\n LEFT JOIN Pa_Gerencia GER on Pl.Gerencia_Id = GER.Id                                 " +
                    "\n LEFT JOIN Pa_Coordenacao CORD on CORD.Id = Pl.Coordenacao_Id                         " +
                    "\n LEFT JOIN Pa_Missao MISS on MISS.Id = Pl.Missao_Id                                   " +
                    "\n LEFT JOIN Pa_Visao VIS on VIS.Id = Pl.Visao_Id                                       " +
                    "\n LEFT JOIN Pa_TemaAssunto TEM on TEM.Id = Pl.TemaAssunto_Id                           " +
                    "\n LEFT JOIN Pa_IndicadoresDiretriz INDIC on INDIC.Id = Pl.IndicadoresDiretriz_Id       " +
                    "\n LEFT JOIN Pa_IndicadoresDeProjeto INDICProj on INDIC.Id = Pl.IndicadoresDeProjeto_Id " +
                    "\n LEFT JOIN Pa_Iniciativa INICI on INICI.Id = Pl.Iniciativa_Id                         " +
                    "\n LEFT JOIN Pa_ObjetivoGeral OBJT on OBJT.Id = Pl.ObjetivoGerencial_Id                 " +
                    "\n LEFT JOIN Pa_Objetivo OBJ on OBJ.Id = Pl.Objetivo_Id                                 " +
                    "\n LEFT JOIN Pa_Dimensao DIME on DIME.Id = Pl.Dimensao_Id                               ";

            }
        }
        public static List<Pa_Planejamento> Listar()
        {
           
            var results = ListarGenerico<Pa_Planejamento>(query);

            foreach (var i in results)
            {
                i._DataFim = i.DataFim.ToShortDateString() + " " + i.DataFim.ToShortTimeString();
                i._DataInicio = i.DataInicio.ToShortDateString() + " " + i.DataInicio.ToShortTimeString();
            }

            return results;
        }

        public static Pa_Planejamento Get(int Id)
        {
            return GetGenerico<Pa_Planejamento>(query + "WHERE Pl.Id = " + Id);
        }

        public static List<Pa_Planejamento> GetPlanejamentoAcao()
        {
            var retorno = new List<Pa_Planejamento>();
            var planejamentos = Listar();
            var acoes = Pa_Acao.Listar();

            foreach (var i in planejamentos)
            {
                var acoesTmp = acoes.Where(r => r.Panejamento_Id == i.Id);
                if (acoesTmp.Count() > 0)
                {
                    foreach (var k in acoesTmp)
                    {
                        var planTemp = new Pa_Planejamento();
                        foreach (var pt in planTemp.GetType().GetProperties())
                            pt.SetValue(planTemp, i.GetType().GetProperty(pt.Name).GetValue(i));

                        planTemp.Acao = k;
                        retorno.Add(planTemp);
                    }
                }
                else
                {
                    i.Acao = new Pa_Acao() { CausaMedidasXAcao = new Pa_CausaMedidasXAcao()};
                        
                    retorno.Add(i);
                }
            }

            return retorno;
        }
        public void AddOrUpdateteste()
        {

        }
        public void AddOrUpdate()
        {
            IsValid();
            string query;
            if (Id > 0)
            {
                query = " UPDATE[dbo].[Pa_Planejamento]                          " +
                       "\n SET                                                  " +
                       "\n    ,[AlterDate] = @AlterDate                         " +
                       "\n    ,[Diretoria_Id] = @Diretoria_Id                   " +
                       "\n    ,[Gerencia_Id] = @Gerencia_Id                     " +
                       "\n    ,[Coordenacao_Id] = @Coordenacao_Id               " +
                       "\n    ,[Missao_Id] = @Missao_Id                         " +
                       "\n    ,[Visao_Id] = @Visao_Id                           " +
                       "\n    ,[TemaAssunto_Id] = @TemaAssunto_Id               " +
                       "\n    ,[IndicadoresDeProjeto_Id] = @IndicadoresDeProjeto_Id               " +
                       "\n    ,[IndicadoresDiretriz_Id] = @IndicadoresDiretriz_Id               " +
                       "\n    ,[Iniciativa_Id] = @Iniciativa_Id                 " +
                       "\n    ,[ObjetivoGerencial_Id] = @ObjetivoGerencial_Id   " +
                       "\n    ,[Dimensao_Id] = @Dimensao_Id                           " +
                       "\n    ,[Objetivo_Id] = @Objetivo_Id                           " +
                       "\n    ,[ValorDe] = @ValorDe                             " +
                       "\n    ,[ValorPara] = @ValorPara                         " +
                       "\n    ,[DataInicio] = @DataInicio                       " +
                       "\n    ,[DataFim] = @DataFim                             " +
                       "\n WHERE Id = @Id                                       ";


                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Diretoria_Id", Diretoria_Id);
                cmd.Parameters.AddWithValue("@Gerencia_Id", Gerencia_Id);
                cmd.Parameters.AddWithValue("@Coordenacao_Id", Coordenacao_Id);
                cmd.Parameters.AddWithValue("@Missao_Id", Missao_Id);
                cmd.Parameters.AddWithValue("@Visao_Id", Visao_Id);
                cmd.Parameters.AddWithValue("@TemaAssunto_Id", TemaAssunto_Id);
                cmd.Parameters.AddWithValue("@IndicadoresDeProjeto_Id", IndicadoresDeProjeto_Id);
                cmd.Parameters.AddWithValue("@IndicadoresDiretriz_Id", IndicadoresDiretriz_Id);
                cmd.Parameters.AddWithValue("@Iniciativa_Id", Iniciativa_Id);
                cmd.Parameters.AddWithValue("@ObjetivoGerencial_Id", ObjetivoGerencial_Id);
                cmd.Parameters.AddWithValue("@Dimensao_Id", Dimensao_Id);
                cmd.Parameters.AddWithValue("@Objetivo_Id", Objetivo_Id);
                cmd.Parameters.AddWithValue("@ValorDe", ValorDe);
                cmd.Parameters.AddWithValue("@ValorPara", ValorPara);
                cmd.Parameters.AddWithValue("@DataInicio", DataInicio);
                cmd.Parameters.AddWithValue("@DataFim", DataFim);

                Update(cmd);
            }
            else
            {
                query = "INSERT INTO [dbo].[Pa_Planejamento]            " +
                        "\n       ([Diretoria_Id]                       " +
                        "\n       ,[Gerencia_Id]                        " +
                        "\n       ,[Coordenacao_Id]                     " +
                        "\n       ,[Missao_Id]                          " +
                        "\n       ,[Visao_Id]                           " +
                        "\n       ,[TemaAssunto_Id]                     " +
                        "\n       ,[IndicadoresDeProjeto_Id]            " +
                        "\n       ,[IndicadoresDiretriz_Id]             " +
                        "\n       ,[Iniciativa_Id]                      " +
                        "\n       ,[ObjetivoGerencial_Id]               " +
                        "\n       ,[Dimensao_Id]                        " +
                        "\n       ,[Objetivo_Id]                        " +
                        "\n       ,[ValorDe]                            " +
                        "\n       ,[ValorPara]                          " +
                        "\n       ,[DataInicio]                         " +
                        "\n       ,[DataFim])                           " +
                        "\n VALUES                                      " +
                        "\n       (@Diretoria_Id                        " +
                        "\n       ,@Gerencia_Id                         " +
                        "\n       ,@Coordenacao_Id                      " +
                        "\n       ,@Missao_Id                           " +
                        "\n       ,@Visao_Id                            " +
                        "\n       ,@TemaAssunto_Id                      " +
                        "\n       ,@IndicadoresDeProjeto_Id             " +
                        "\n       ,@IndicadoresDiretriz_Id              " +
                        "\n       ,@Iniciativa_Id                       " +
                        "\n       ,@ObjetivoGerencial_Id                " +
                        "\n       ,@Dimensao_Id                         " +
                        "\n       ,@Objetivo_Id                         " +
                        "\n       ,@ValorDe                             " +
                        "\n       ,@ValorPara                           " +
                        "\n       ,@DataInicio                          " +
                        "\n       ,@DataFim)                            " +
                        "\n       SELECT CAST(scope_identity() AS int)  ";


                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Diretoria_Id", Diretoria_Id);
                cmd.Parameters.AddWithValue("@Gerencia_Id", Gerencia_Id);
                cmd.Parameters.AddWithValue("@Coordenacao_Id", Coordenacao_Id);
                cmd.Parameters.AddWithValue("@Missao_Id", Missao_Id);
                cmd.Parameters.AddWithValue("@Visao_Id", Visao_Id);
                cmd.Parameters.AddWithValue("@TemaAssunto_Id", TemaAssunto_Id);
                cmd.Parameters.AddWithValue("@IndicadoresDeProjeto_Id", IndicadoresDeProjeto_Id);
                cmd.Parameters.AddWithValue("@IndicadoresDiretriz_Id", IndicadoresDiretriz_Id);
                cmd.Parameters.AddWithValue("@Iniciativa_Id", Iniciativa_Id);
                cmd.Parameters.AddWithValue("@ObjetivoGerencial_Id", ObjetivoGerencial_Id);
                cmd.Parameters.AddWithValue("@Dimensao_Id", Dimensao_Id);
                cmd.Parameters.AddWithValue("@Objetivo_Id", Objetivo_Id);
                cmd.Parameters.AddWithValue("@ValorDe", ValorDe);
                cmd.Parameters.AddWithValue("@ValorPara", ValorPara);
                cmd.Parameters.AddWithValue("@DataInicio", DataInicio);
                cmd.Parameters.AddWithValue("@DataFim", DataFim);

                Id = Salvar(cmd);
            }
        }

    }

}

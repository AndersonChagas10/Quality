using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace PlanoAcaoCore
{
    public class Pa_Acao : Pa_BaseObject, ICrudPa<Pa_Acao>
    {

        [Display(Name = "Unidade")]
        public int Unidade_Id { get; set; }
        public string Unidade { get; set; }

        [Display(Name = "Departamento")]
        public int Departamento_Id { get; set; }
        public string Departamento { get; set; }

        [Display(Name = "Quando início")]
        public int Pa_CausaMedidasXAcao_Id { get; set; }

        [Display(Name = "Quando início")]
        public DateTime QuandoInicio { get; set; }
       
        public string _QuandoInicio { get; set; }

        [Display(Name = "Duracao dias")]
        public int DuracaoDias { get; set; }

        [Display(Name = "Quando fim")]
        public DateTime QuandoFim { get; set; }
        public string _QuandoFim { get; set; }
        //{
        //    get
        //    {
        //        if (QuandoFim == null)
        //            if (!string.IsNullOrEmpty(_QuandoFim))
        //                return Guard.ParseDateToSqlV2(_QuandoFim);

        //        return QuandoFim;
        //    }
        //    set
        //    {
        //        QuandoFim = value;
        //    }
        //}

        [Display(Name = "Como pontos importantes")]
        public string ComoPontosimportantes { get; set; }
        [Display(Name = "Predecessora")]
        public int Predecessora_Id { get; set; }
        [Display(Name = "pra que")]
        public string PraQue { get; set; }
        [Display(Name = "Quanto custa")]
        public decimal QuantoCusta { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        public string StatusName { get; set; }
       
        [Display(Name = "Planejamento")]
        public int Panejamento_Id { get; set; }

        //public List<Pa_CausaMedidasXAcao> CausaMedidasXAcao { get; set; }
        //public List<Pa_AcaoXQuem> AcaoXQuem { get; set; }
        public Pa_CausaMedidasXAcao CausaMedidasXAcao { get; set; }
        public Pa_AcaoXQuem AcaoXQuem { get; set; }
        public string _Quem { get; set; }
        public string _Prazo { get; set; }

        public void IsValid()
        {
            //Name = Guard.CheckStringFullSimple(Name);
        }

      

        public static List<Pa_Acao> Listar()
        {                                                                                   
            var query = "SELECT ACAO.* ,                                                    "+
                        "\n STA.Name as StatusName,                                         "+
                        "\n UN.Name as Unidade,                                             "+
                        "\n DPT.Name as Departamento,                                       "+
                        "\n AXQ.*,                                                          "+
                        "\n CMA.*                                                           "+
                        "\n FROM pa_acao ACAO                                               "+
                        "\n LEFT JOIN Pa_CausaMedidaXAcao CMA ON CMA.Acao_Id = ACAO.Id      "+
                        "\n LEFT JOIN Pa_Unidade UN ON UN.Id = ACAO.Unidade_Id              "+
                        "\n LEFT JOIN Pa_Departamento DPT ON DPT.Id = ACAO.Departamento_Id  "+
                        "\n LEFT JOIN Pa_AcaoXQuem AXQ ON AXQ.Acao_Id = ACAO.Id             "+
                        "\n LEFT JOIN Pa_Status STA ON STA.Id = ACAO.[Status];              ";
            return ListarGenerico<Pa_Acao>(query);
        }

        public static Pa_Acao Get(int Id)
        {
            var query = "SELECT ACAO.* ,                                                    " +
                        "\n STA.Name as StatusName,                                         " +
                        "\n UN.Name as Unidade,                                             " +
                        "\n DPT.Name as Departamento,                                       " +
                        "\n AXQ.*,                                                          " +
                        "\n CMA.*                                                           " +
                        "\n FROM pa_acao ACAO                                               " +
                        "\n LEFT JOIN Pa_CausaMedidaXAcao CMA ON CMA.Acao_Id = ACAO.Id      " +
                        "\n LEFT JOIN Pa_Unidade UN ON UN.Id = ACAO.Unidade_Id              " +
                        "\n LEFT JOIN Pa_Departamento DPT ON DPT.Id = ACAO.Departamento_Id  " +
                        "\n LEFT JOIN Pa_AcaoXQuem AXQ ON AXQ.Acao_Id = ACAO.Id             " +
                        "\n LEFT JOIN Pa_Status STA ON STA.Id = ACAO.[Status] WHERE Id = " + Id;

            return GetGenerico<Pa_Acao>(query);
        }

        public void AddOrUpdate()
        {
            IsValid();
            string query;
            if (Id > 0)
            {
                query = "UPDATE [dbo].[Pa_Acao]                                 " +
                   "\n    SET [Unidade_Id] = @Unidade_Id                          " +
                   "\n       ,[Departamento_Id] = @Departamento_Id                " +
                   "\n       ,[QuandoInicio] = @QuandoInicio                      " +
                   "\n       ,[DuracaoDias] = @DuracaoDias                        " +
                   "\n       ,[QuandoFim] = @QuandoFim                            " +
                   "\n       ,[ComoPontosimportantes] = @ComoPontosimportantes    " +
                   "\n       ,[Predecessora_Id] = @Predecessora_Id                " +
                   "\n       ,[PraQue] = @PraQue                                  " +
                   "\n       ,[QuantoCusta] = @QuantoCusta                        " +
                   "\n       ,[Status] = @Status                                  " +
                   "\n       ,[Panejamento_Id] = @Panejamento_Id                  " +
                   "\n  WHERE Id = @Id ";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Unidade_Id", Unidade_Id);
                cmd.Parameters.AddWithValue("@Departamento_Id", Departamento_Id);
                cmd.Parameters.AddWithValue("@QuandoInicio", Guard.ParseDateToSqlV2(_QuandoInicio));
                cmd.Parameters.AddWithValue("@DuracaoDias", DuracaoDias);
                cmd.Parameters.AddWithValue("@QuandoFim", Guard.ParseDateToSqlV2(_QuandoFim));
                cmd.Parameters.AddWithValue("@ComoPontosimportantes", ComoPontosimportantes);
                cmd.Parameters.AddWithValue("@Predecessora_Id", Predecessora_Id);
                cmd.Parameters.AddWithValue("@PraQue", PraQue);
                cmd.Parameters.AddWithValue("@QuantoCusta", QuantoCusta);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Panejamento_Id", Panejamento_Id);
                cmd.Parameters.AddWithValue("@Id", Id);

                Salvar(cmd);
            }
            else
            {

                //foreach (var i in CausaMedidasXAcao)
                //    i.IsValid();

                //foreach (var j in AcaoXQuem)
                //    j.IsValid();
                CausaMedidasXAcao.IsValid();
                AcaoXQuem.IsValid();

                query = "INSERT INTO [dbo].[Pa_Acao]" +
              "\n        ([Unidade_Id]                            " +
              "\n        ,[Departamento_Id]                       " +
              "\n        ,[QuandoInicio]                          " +
              "\n        ,[DuracaoDias]                           " +
              "\n        ,[QuandoFim]                             " +
              "\n        ,[ComoPontosimportantes]                 " +
              "\n        ,[Predecessora_Id]                       " +
              "\n        ,[PraQue]                                " +
              "\n        ,[QuantoCusta]                           " +
              "\n        ,[Status]                                " +
              "\n        ,[Panejamento_Id])                       " +
              "\n  VALUES                                         " +
              "\n        (@Unidade_Id                             " +
              "\n        ,@Departamento_Id                        " +
              "\n        ,@QuandoInicio                           " +
              "\n        ,@DuracaoDias                            " +
              "\n        ,@QuandoFim                              " +
              "\n        ,@ComoPontosimportantes                  " +
              "\n        ,@Predecessora_Id                        " +
              "\n        ,@PraQue                                 " +
              "\n        ,@QuantoCusta                            " +
              "\n        ,@Status                                 " +
              "\n        ,@Panejamento_Id)SELECT CAST(scope_identity() AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@Unidade_Id", Unidade_Id);
                cmd.Parameters.AddWithValue("@Departamento_Id", Departamento_Id);
                cmd.Parameters.AddWithValue("@QuandoInicio", Guard.ParseDateToSqlV2(_QuandoInicio));
                cmd.Parameters.AddWithValue("@DuracaoDias", DuracaoDias);
                cmd.Parameters.AddWithValue("@QuandoFim", Guard.ParseDateToSqlV2(_QuandoFim));
                cmd.Parameters.AddWithValue("@ComoPontosimportantes", ComoPontosimportantes);
                cmd.Parameters.AddWithValue("@Predecessora_Id", Predecessora_Id);
                cmd.Parameters.AddWithValue("@PraQue", PraQue);
                cmd.Parameters.AddWithValue("@QuantoCusta", QuantoCusta);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Panejamento_Id", Panejamento_Id);

                Id = Salvar(cmd);

                CausaMedidasXAcao.Acao_Id = Id;
                CausaMedidasXAcao.AddOrUpdate();
                AcaoXQuem.Acao_Id = Id;
                AcaoXQuem.AddOrUpdate();

                //foreach (var i in CausaMedidasXAcao)
                //{
                //    i.Acao_Id = Id;
                //    i.AddOrUpdate();
                //}

                //foreach (var j in AcaoXQuem)
                //{
                //    j.Acao_Id = Id;
                //    j.AddOrUpdate();
                //}

            }
        }
    }
}

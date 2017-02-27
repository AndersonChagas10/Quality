using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_Acao : Pa_BaseObject, ICrudPa<Pa_Acao>
    {

        [Display(Name = "Unidade")]
        public int? Unidade_Id { get; set; }
        public string Unidade { get; set; }

        [Display(Name = "Departamento")]
        public int? Departamento_Id { get; set; }
        public string Departamento { get; set; }

        [Display(Name = "Quando início")]
        public int? Pa_CausaMedidasXAcao_Id { get; set; }

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
        public int? Predecessora_Id { get; set; }
        [Display(Name = "Pra que")]
        public string PraQue { get; set; }
        [Display(Name = "Quanto custa")]
        public decimal QuantoCusta { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        public string StatusName { get; set; }

        [Display(Name = "Planejamento")]
        public int Panejamento_Id { get; set; }

        //public List<Pa_CausaMedidasXAcao> CausaMedidasXAcao { get; set; }
        public List<Pa_AcaoXQuem> AcaoXQuem { get; set; }
        public List<string> _Quem { get; set; }

        public Pa_CausaMedidasXAcao CausaMedidasXAcao { get; set; }
        //public Pa_AcaoXQuem AcaoXQuem { get; set; }

        public string _CorPrazo { get; set; }

        public string _Prazo
        {
            get
            {
                if (!string.IsNullOrEmpty(StatusName))
                    if (StatusName.Contains("Concluído"))
                    {
                        _CorPrazo = "rgb(126, 194, 253)";
                        return "Finalizado";
                    }

                var agora = DateTime.Now;
                if (QuandoFim > agora)
                {
                    _CorPrazo = "rgb(144, 238, 144);";
                    return string.Format("Faltam {0} dias.", Math.Round((QuandoFim - agora).TotalDays));
                }
                else if (QuandoFim < agora)
                {
                    _CorPrazo = "rgb(250, 128, 114);";
                    return string.Format("-{0} Dias", Math.Round((QuandoFim - agora).TotalDays));
                }


                return string.Empty;
            }
        }

        public void IsValid()
        {
            //Name = Guard.CheckStringFullSimple(Name);
        }

        private static string query
        {
            get
            {
                return "SELECT ACAO.* ,                                                    " +
                        "\n STA.Name as StatusName,                                         " +
                        "\n UN.Name as Unidade,                                             " +
                        "\n DPT.Name as Departamento                                       " +
                        "\n FROM pa_acao ACAO                                               " +
                        "\n LEFT JOIN Pa_Unidade UN ON UN.Id = ACAO.Unidade_Id              " +
                        "\n LEFT JOIN Pa_Departamento DPT ON DPT.Id = ACAO.Departamento_Id  " +
                        "\n LEFT JOIN Pa_Status STA ON STA.Id = ACAO.[Status]              ";


            }
        }

        public static List<Pa_Acao> Listar()
        {
         
            var retorno = ListarGenerico<Pa_Acao>(query);

            foreach (var i in retorno)
            {
                i._Quem = Pa_Quem.GetQuemXAcao(i.Id).Select(r => r.Name).ToList();
                i.CausaMedidasXAcao = Pa_CausaMedidasXAcao.GetByAcaoId(i.Id);
                i._QuandoInicio = i.QuandoInicio.ToShortDateString() + " " + i.QuandoInicio.ToShortTimeString();
                i._QuandoFim = i.QuandoFim.ToShortDateString() + " " + i.QuandoFim.ToShortTimeString();
            }

            return retorno;
        }

        public static Pa_Acao Get(int Id)
        {
            return GetGenerico<Pa_Acao>(query + " WHERE ACAO.Id = " + Id);
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

                foreach (var j in AcaoXQuem)
                    j.IsValid();
                CausaMedidasXAcao.IsValid();
                //AcaoXQuem.IsValid();

                query = "INSERT INTO [dbo].[Pa_Acao]" +
              "\n       ([QuandoInicio]                          " +
              "\n        ,[DuracaoDias]                           " +
              "\n        ,[QuandoFim]                             " +
              "\n        ,[ComoPontosimportantes]                 " +
              "\n        ,[PraQue]                                " +
              "\n        ,[QuantoCusta]                           " +
              "\n        ,[Status]                                " +
              "\n        ,[Panejamento_Id])                       " +
              "\n  VALUES                                         " +
              "\n        (@QuandoInicio                           " +
              "\n        ,@DuracaoDias                            " +
              "\n        ,@QuandoFim                              " +
              "\n        ,@ComoPontosimportantes                  " +
              "\n        ,@PraQue                                 " +
              "\n        ,@QuantoCusta                            " +
              "\n        ,@Status                                 " +
              "\n        ,@Panejamento_Id)SELECT CAST(scope_identity() AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@QuandoInicio", QuandoInicio);
                cmd.Parameters.AddWithValue("@DuracaoDias", DuracaoDias);
                cmd.Parameters.AddWithValue("@QuandoFim", QuandoFim);
                cmd.Parameters.AddWithValue("@ComoPontosimportantes", ComoPontosimportantes);
                cmd.Parameters.AddWithValue("@PraQue", PraQue);
                cmd.Parameters.AddWithValue("@QuantoCusta", QuantoCusta);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Panejamento_Id", Panejamento_Id);

                Id = Salvar(cmd);

                var causaEsp = new Pa_CausaEspecifica() { Text = CausaMedidasXAcao.CausaEspecifica };
                var contramedidaEsp = new Pa_ContramedidaEspecifica() { Text = CausaMedidasXAcao.ContramedidaEspecifica };
                causaEsp.AddOrUpdate();
                contramedidaEsp.AddOrUpdate();

                CausaMedidasXAcao.CausaEspecifica_Id = causaEsp.Id;
                CausaMedidasXAcao.ContramedidaEspecifica_Id = contramedidaEsp.Id;

                CausaMedidasXAcao.Acao_Id = Id;
                CausaMedidasXAcao.AddOrUpdate();



                //AcaoXQuem.Acao_Id = Id;
                //AcaoXQuem.AddOrUpdate();

                //foreach (var i in CausaMedidasXAcao)
                //{
                //    i.Acao_Id = Id;
                //    i.AddOrUpdate();
                //}

                foreach (var j in AcaoXQuem)
                {
                    j.Acao_Id = Id;
                    j.AddOrUpdate();
                }

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_Acao : Pa_BaseObject, ICrudPa<Pa_Acao>
    {
        [Display(Name = "Problema ou Desvio")]
        public int? Pa_Problema_Desvio_Id { get; set; }
        public Pa_Problema_Desvio _Pa_Problema_Desvio_Id
        {
            get
            {
                if (Pa_Problema_Desvio_Id > 0)
                    return Pa_Problema_Desvio.Get(Pa_Problema_Desvio_Id.GetValueOrDefault());
                else
                    return new Pa_Problema_Desvio();
            }
        }

        //[Display(Name = "Indicador SGQ ou Ação")]
        [Display(Name = "Indicador Operacional")]
        public int? Pa_IndicadorSgqAcao_Id { get; set; }
        public Pa_IndicadorSgqAcao _Pa_IndicadorSgqAcao
        {
            get
            {
                if (Pa_IndicadorSgqAcao_Id > 0)
                    return Pa_IndicadorSgqAcao.Get(Pa_IndicadorSgqAcao_Id.GetValueOrDefault());
                else
                    return new Pa_IndicadorSgqAcao();
            }
        }



        [Display(Name = "Unidade")]
        public int? Unidade_Id { get; set; }
        public string Unidade { get; set; }

        [Display(Name = "Departamento")]
        public int? Departamento_Id { get; set; }
        public string Departamento { get; set; }

        public int? Pa_CausaMedidasXAcao_Id { get; set; }

        [Display(Name = "Duracao dias")]
        public int DuracaoDias { get; set; }

        [Display(Name = "Como pontos importantes")]
        public string ComoPontosimportantes { get; set; }

        [Display(Name = "Predecessora")]
        public int? Predecessora_Id { get; set; }

        [Display(Name = "Pra que")]
        public string PraQue { get; set; }

        [Display(Name = "Quanto custa")]
        public decimal QuantoCusta { get; set; }
        public string _QuantoCusta { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

        public string StatusName { get; set; }

        [Display(Name = "Planejamento")]
        public int Panejamento_Id { get; set; }

        [Display(Name = "Quando início")]
        public DateTime QuandoInicio { get; set; }
        public string _QuandoInicio { get; set; }

        [Display(Name = "Quando fim")]
        public DateTime QuandoFim { get; set; }
        public string _QuandoFim { get; set; }

        public List<Pa_AcaoXQuem> AcaoXQuem { get; set; }
        public List<string> _Quem { get; set; }
        public List<Pa_Quem> _QuemObj { get; set; }

        public Pa_CausaMedidasXAcao CausaMedidasXAcao { get; set; }

        public string _Prazo
        {
            get
            {
                if (QuandoFim == DateTime.MinValue)
                    return "-";

                if (!string.IsNullOrEmpty(StatusName))
                    if (StatusName.Contains("Concluido") || StatusName.Contains("Concluído") || StatusName.Contains("Cancelado"))
                        return "Finalizado";

                var agora = DateTime.Now;
                if (QuandoFim > agora)
                    return string.Format("Faltam {0} dias.", Math.Round((QuandoFim - agora).TotalDays));
                else if (QuandoFim < agora)
                    return string.Format("{0} Dias", Math.Round((QuandoFim - agora).TotalDays));

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
                return "SELECT TOP 200 ACAO.* ,                                                     " +
                        "\n STA.Name as StatusName,                                         " +
                        "\n UN.Name as Unidade,                                             " +
                        "\n DPT.Name as Departamento                                        " +
                        "\n FROM pa_acao ACAO                                               " +
                        "\n LEFT JOIN Pa_Unidade UN ON UN.Id = ACAO.Unidade_Id              " +
                        "\n LEFT JOIN Pa_Departamento DPT ON DPT.Id = ACAO.Departamento_Id  " +
                        "\n LEFT JOIN Pa_Status STA ON STA.Id = ACAO.[Status]               ";

            }
        }

        public static List<Pa_Acao> Listar()
        {
            var retorno = ListarGenerico<Pa_Acao>(query);

            foreach (var i in retorno)
            {
                i._Quem = Pa_Quem.GetQuemXAcao(i.Id).Select(r => r.Name).ToList();
                i._QuemObj = Pa_Quem.GetQuemXAcao(i.Id).ToList();
                i.AcaoXQuem = Pa_AcaoXQuem.Get(i.Id).ToList();
                i.CausaMedidasXAcao = Pa_CausaMedidasXAcao.GetByAcaoId(i.Id);
                i._QuandoInicio = i.QuandoInicio.ToShortDateString() + " " + i.QuandoInicio.ToShortTimeString();
                i._QuandoFim = i.QuandoFim.ToShortDateString() + " " + i.QuandoFim.ToShortTimeString();
            }

            return retorno;
        }

        public static Pa_Acao Get(int Id)
        {
            var retorno = GetGenerico<Pa_Acao>(query + " WHERE ACAO.Id = " + Id);

            retorno._Quem = Pa_Quem.GetQuemXAcao(retorno.Id).Select(r => r.Name).ToList();
            retorno.AcaoXQuem = Pa_AcaoXQuem.Get(retorno.Id).ToList();
            retorno._QuemObj = Pa_Quem.GetQuemXAcao(retorno.Id).ToList();
            retorno.CausaMedidasXAcao = Pa_CausaMedidasXAcao.GetByAcaoId(retorno.Id);
            retorno._QuandoInicio = retorno.QuandoInicio.ToShortDateString() + " " + retorno.QuandoInicio.ToShortTimeString();
            retorno._QuandoFim = retorno.QuandoFim.ToShortDateString() + " " + retorno.QuandoFim.ToShortTimeString();

            return retorno;
        }

        public void AddOrUpdate()
        {
            IsValid();
            CausaMedidasXAcao.IsValid();

            foreach (var j in AcaoXQuem)
                j.IsValid();

            string query;

            if (Id > 0)
            {

                query = "UPDATE [dbo].[Pa_Acao]                                             " +
                   "\n    SET [QuandoInicio] = @QuandoInicio                                " +
                   "\n       ,[DuracaoDias] = @DuracaoDias                                  " +
                   "\n       ,[QuandoFim] = @QuandoFim                                      " +
                   "\n       ,[ComoPontosimportantes] = @ComoPontosimportantes              " +
                   "\n       ,[QuantoCusta] = @QuantoCusta                                  " +
                   "\n       ,[Status] = @Status                                            " +
                   "\n       ,[Pa_Problema_Desvio_Id] = @Pa_Problema_Desvio_Id    " +
                   "\n       ,[Pa_IndicadorSgqAcao_Id] = @Pa_IndicadorSgqAcao_Id            " +
                   "\n       ,[PraQue] = @PraQue                                            " +
                   "\n       ,[Panejamento_Id] = @Panejamento_Id                            " +
                   "\n  WHERE Id = @Id                                                      ";

                query += " SELECT CAST(1 AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@QuandoInicio", QuandoInicio);
                cmd.Parameters.AddWithValue("@DuracaoDias", DuracaoDias);
                cmd.Parameters.AddWithValue("@QuandoFim", QuandoFim);
                cmd.Parameters.AddWithValue("@ComoPontosimportantes", ComoPontosimportantes);
                cmd.Parameters.AddWithValue("@QuantoCusta", QuantoCusta);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@PraQue", PraQue);
                cmd.Parameters.AddWithValue("@Panejamento_Id", Panejamento_Id);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@Pa_Problema_Desvio_Id", Pa_Problema_Desvio_Id);
                cmd.Parameters.AddWithValue("@Pa_IndicadorSgqAcao_Id", Pa_IndicadorSgqAcao_Id);

                Salvar(cmd);

                var causaEsp = new Pa_CausaEspecifica()
                {
                    Id = CausaMedidasXAcao.CausaEspecifica_Id.GetValueOrDefault(),
                    Text = CausaMedidasXAcao.CausaEspecifica
                };

                var contramedidaEsp = new Pa_ContramedidaEspecifica()
                {
                    Id = CausaMedidasXAcao.ContramedidaEspecifica_Id.GetValueOrDefault(),
                    Text = CausaMedidasXAcao.ContramedidaEspecifica
                };

                causaEsp.AddOrUpdate();
                contramedidaEsp.AddOrUpdate();

                CausaMedidasXAcao.AddOrUpdate();

                foreach (var j in AcaoXQuem)
                {
                    j.Acao_Id = Id;
                    j.AddOrUpdate();
                }

            }
            else
            {

                query = "INSERT INTO [dbo].[Pa_Acao]               " +
                        "\n       ([QuandoInicio]                  " +
                        "\n        ,[DuracaoDias]                  " +
                        "\n        ,[QuandoFim]                    " +
                        "\n        ,[ComoPontosimportantes]        " +
                        "\n        ,[PraQue]                       " +
                        "\n        ,[QuantoCusta]                  " +
                        "\n        ,[Status]                       " +
                        "\n        ,[Pa_Problema_Desvio_Id]   " +
                        "\n        ,[Pa_IndicadorSgqAcao_Id]       " +
                        "\n        ,[Panejamento_Id])              " +
                        "\n  VALUES                                " +
                        "\n        (@QuandoInicio                  " +
                        "\n        ,@DuracaoDias                   " +
                        "\n        ,@QuandoFim                     " +
                        "\n        ,@ComoPontosimportantes         " +
                        "\n        ,@PraQue                        " +
                        "\n        ,@QuantoCusta                   " +
                        "\n        ,@Status                        " +
                        "\n        ,@Pa_Problema_Desvio_Id    " +
                        "\n        ,@Pa_IndicadorSgqAcao_Id        " +
                        "\n        ,@Panejamento_Id);              ";

                query += "SELECT CAST(scope_identity() AS int)";

                SqlCommand cmd;
                cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@QuandoInicio", QuandoInicio);
                cmd.Parameters.AddWithValue("@DuracaoDias", DuracaoDias);
                cmd.Parameters.AddWithValue("@QuandoFim", QuandoFim);
                cmd.Parameters.AddWithValue("@ComoPontosimportantes", ComoPontosimportantes);
                cmd.Parameters.AddWithValue("@PraQue", PraQue);
                cmd.Parameters.AddWithValue("@QuantoCusta", QuantoCusta);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Pa_Problema_Desvio_Id", Pa_Problema_Desvio_Id);
                cmd.Parameters.AddWithValue("@Pa_IndicadorSgqAcao_Id", Pa_IndicadorSgqAcao_Id);
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

                foreach (var j in AcaoXQuem)
                {
                    j.Acao_Id = Id;
                    j.AddOrUpdate();
                }

            }
        }
    }
}

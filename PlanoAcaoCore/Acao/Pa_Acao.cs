﻿using DTO.Helpers;
using Helper;
using PlanoAcaoCore.Acao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_Acao : Pa_BaseObject
    {

        #region SGQ

        [Display(Name = "Indicador")]
        public int? Level1Id { get; set; }
        public string _Level1 { get; set; }

        [Display(Name = "Monitoramento")]
        public int? Level2Id { get; set; }
        public string _Level2 { get; set; }

        [Display(Name = "Tarefa")]
        public int? Level3Id { get; set; }
        public string _Level3 { get; set; }

        #endregion

        [Display(Name = "Problema ou Desvio")]
        public int? Pa_Problema_Desvio_Id { get; set; }

        //[Display(Name = "Indicador SGQ ou Ação")]
        [Display(Name = "Indicador Operacional")]
        public int? Pa_IndicadorSgqAcao_Id { get; set; }

        [Display(Name = "Unidade")]
        public int? Unidade_Id { get; set; }
        public string _Unidade { get; set; }

        [Display(Name = "Departamento")]
        public int? Departamento_Id { get; set; }

        //[Display(Name = "Duracao dias")]
        //public int DuracaoDias { get; set; }

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

        public string _StatusName { get; set; }

        [Display(Name = "Planejamento")]
        public int Panejamento_Id { get; set; }

        [Display(Name = "Quando início")]
        public DateTime QuandoInicio { get; set; }
        public string _QuandoInicio { get; set; }

        [Display(Name = "Quando fim")]
        public DateTime QuandoFim { get; set; }
        public string _QuandoFim { get; set; }

        [Display(Name = "Quem")]
        public int Quem_Id { get; set; }
        public string _Quem { get; set; }

        [Display(Name = "Causa Generica")]
        public int CausaGenerica_Id { get; set; }
        public string _CausaGenerica { get; set; }

        [Display(Name = "Contramedida Genérica")]
        public int ContramedidaGenerica_Id { get; set; }
        public string _ContramedidaGenerica { get; set; }

        [Display(Name = "Grupo Causa")]
        public int GrupoCausa_Id { get; set; }
        public string _GrupoCausa { get; set; }

        [Display(Name = "Causa Especifica")]
        public string CausaEspecifica { get; set; }

        [Display(Name = "Causa Generica")]
        public string ContramedidaEspecifica { get; set; }

        public int TipoIndicador { get; set; }

        //public List<Pa_AcaoXQuem> AcaoXQuem { get; set; }

        //public List<string> _Quem { get; set; }

        //public List<Pa_Quem> _QuemObj { get; set; }

        public List<Pa_Acompanhamento> _Acompanhamento
        {
            get
            {
                var RenanConvulsao = new List<Pa_Acompanhamento>();

                if (Id > 0)
                    RenanConvulsao = Pa_Acompanhamento.GetByAcaoId(Id);

                return RenanConvulsao;
            }
        }
      
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

        public string _Prazo
        {
            get
            {
                if (QuandoFim == DateTime.MinValue)
                    return "-";

                if (!string.IsNullOrEmpty(_StatusName))
                    if (_StatusName.Contains("Concluido") || _StatusName.Contains("Concluído") || _StatusName.Contains("Cancelado"))
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

            if (Id <= 0)
            {
                Pa_Status status = Pa_Status.Listar().FirstOrDefault(r => r.Name.Equals("Em Andamento"));

                Status = status.Id;
                //StatusName = status.Name;
            }
            else
            {
                var old = Pa_Acao.Get(Id);
                Panejamento_Id = old.Panejamento_Id;
            }

            //if (Pa_IndicadorSgqAcao_Id <= 0)
            //    message += "\n Indicador Operacional,";

            //if (Pa_Problema_Desvio_Id <= 0 || Pa_Problema_Desvio_Id == null)
            //    message += "\n Problema ou Desvio,";

            //if (AcaoXQuem != null)
            //{
            //    foreach (var i in AcaoXQuem)
            //        if (i.Quem_Id <= 0)
            //            message = "\n Quem,";
            //}
            //else
            //    message = "\n Quem,";

            //if (CausaMedidasXAcao == null)
            //    CausaMedidasXAcao = new Pa_CausaMedidasXAcao();

            //if (CausaMedidasXAcao.CausaGenerica_Id <= 0)
            //    message += "\n Causa generica,";

            //if (CausaMedidasXAcao.GrupoCausa_Id <= 0)
            //    message += "\n Grupo causa,";

            //if (CausaMedidasXAcao.ContramedidaGenerica_Id <= 0)
            //    message += "\n Contramedida generica,";

            //if (string.IsNullOrEmpty(CausaMedidasXAcao._CausaEspecifica))
            //    message += "\n Causa Específica,";

            //if (string.IsNullOrEmpty(CausaMedidasXAcao._ContramedidaEspecifica))
            //    message += "\n Contramedida Específica,";

            //if (string.IsNullOrEmpty(_QuandoInicio))
            //    message += "\n Quando início,";

            //if (string.IsNullOrEmpty(_QuandoFim))
            //    message += "\n Quando fim,";

            //if (string.IsNullOrEmpty(ComoPontosimportantes))
            //    message += "\n Como pontos importantes,";

            //if (string.IsNullOrEmpty(PraQue))
            //    message += "\n Pra que,";

            //if (string.IsNullOrEmpty(_QuantoCusta))
            //    message += "\n Quanto custa,";

            //if (Status <= 0)
            //    message += "\n Status,";

            VerificaMensagemCamposObrigatorios(message);

            #region Prepara obj para DB

            //throw new Exception("treste");
            if (_QuantoCusta != null)
                QuantoCusta = NumericExtensions.CustomParseDecimal(_QuantoCusta).GetValueOrDefault();

            if (_QuandoInicio != null)
                QuandoInicio = Guard.ParseDateToSqlV2(_QuandoInicio);
            else
                QuandoInicio = DateTime.Now;

            if (_QuandoFim != null)
                QuandoFim = Guard.ParseDateToSqlV2(_QuandoFim);
            else
                QuandoFim = DateTime.Now;


            #endregion

        }

        private static string query
        {
            get
            {
                return " \n SELECT TOP 200 ACAO.* ,                                                           " +
                        " \n STA.Name as _StatusName,                                                           " +
                        " \n UN.Name as _Unidade,                                                               " +
                        " \n DPT.Name as _Departamento,                                                         " +
                        " \n Q.Name as _Quem,                                                                  " +
                        " \n CG.CausaGenerica as _CausaGenerica,                                               " +
                        " \n CMG.ContramedidaGenerica as _ContramedidaGenerica,                                " +
                        " \n GC.GrupoCausa as _GrupoCausa                                                      " +
                        " \n FROM pa_acao ACAO                                                                 " +
                        " \n LEFT JOIN Pa_Unidade UN ON UN.Id = ACAO.Unidade_Id                                " +
                        " \n LEFT JOIN Pa_Quem Q ON Q.Id = ACAO.Quem_Id                                        " +
                        " \n LEFT JOIN Pa_CausaGenerica CG ON CG.Id = ACAO.CausaGenerica_Id                    " +
                        " \n LEFT JOIN Pa_ContramedidaGenerica CMG ON CMG.Id = ACAO.ContramedidaGenerica_Id    " +
                        " \n LEFT JOIN Pa_GrupoCausa GC ON GC.Id = ACAO.GrupoCausa_Id                          " +
                        " \n LEFT JOIN Pa_Departamento DPT ON DPT.Id = ACAO.Departamento_Id                    " +
                        " \n LEFT JOIN Pa_Status STA ON STA.Id = ACAO.[Status]";

            }
        }

        public static List<Pa_Acao> Listar()
        {
            var retorno = ListarGenerico<Pa_Acao>(query);

            foreach (var i in retorno)
            {
                //i._Quem = Pa_Quem.GetQuemXAcao(i.Id).Select(r => r.Name).ToList();
                //i._QuemObj = Pa_Quem.GetQuemXAcao(i.Id).ToList();
                //i.AcaoXQuem = Pa_AcaoXQuem.Get(i.Id).ToList();
                //i.CausaMedidasXAcao = Pa_CausaMedidasXAcao.GetByAcaoId(i.Id);
                i._QuandoInicio = i.QuandoInicio.ToShortDateString() + " " + i.QuandoInicio.ToShortTimeString();
                i._QuandoFim = i.QuandoFim.ToShortDateString() + " " + i.QuandoFim.ToShortTimeString();
            }

            return retorno;
        }

        public static Pa_Acao Get(int Id)
        {
            var retorno = GetGenerico<Pa_Acao>(query + " AND ACAO.Id = " + Id);

            //retorno._Quem = Pa_Quem.GetQuemXAcao(retorno.Id).Select(r => r.Name).ToList();
            //retorno.AcaoXQuem = Pa_AcaoXQuem.Get(retorno.Id).ToList();
            //retorno._QuemObj = Pa_Quem.GetQuemXAcao(retorno.Id).ToList();
            //retorno.CausaMedidasXAcao = Pa_CausaMedidasXAcao.GetByAcaoId(retorno.Id);
            retorno._QuandoInicio = retorno.QuandoInicio.ToShortDateString() + " " + retorno.QuandoInicio.ToShortTimeString();
            retorno._QuandoFim = retorno.QuandoFim.ToShortDateString() + " " + retorno.QuandoFim.ToShortTimeString();

            return retorno;
        }

        //public Pa_CausaMedidasXAcao CausaMedidasXAcao { get; set; }

    }
}

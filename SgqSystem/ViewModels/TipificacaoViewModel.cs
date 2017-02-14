﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.ViewModels
{
    public class TipificacaoViewModel
    {
        public int EmpresaId { get; set; }
        public int DepartamentoId { get; set; }
        public int TarefaId { get; set; }
        public int MonitoramentoId { get; set; }
        public int ProdutoId { get; set; }
        public int UnidadeId { get; set; }
        public int OperacaoId { get; set; }
        public VTVerificacaoTipificacaoDTO VerificacaoTipificacao { get; set; }
        public List<VTVerificacaoTipificacaoResultadosDTO> VerificacaoTipificacaoResultados { get; set; }
    }

    public class VTVerificacaoTipificacaoDTO
    {
        public int Id { get; set; }
        public int Sequencial { get; set; }
        public byte Banda { get; set; }
        public string dataTipificacao { get; set; }
        public DateTime DataHora { get; set; }
        public int UnidadeId { get; set; }
        public string Chave { get; set; }
    }

    public class VTVerificacaoTipificacaoResultadosDTO
    {
        public int Id { get; set; }
        public VTVerificacaoTipificacaoDTO VerificacaoTipificacao { get; set; }
        public MonitoramentoDTO Monitoramento { get; set; }
        public CaracteristicaTipificacaoDTO CaracteristicaTipificacao { get; set; }
    }

    public class VTVerificacaoTipificacaoTarefaIntegracaoDTO
    {
        public MonitoramentoDTO Monitoramento { get; set; }
        public CaracteristicaTipificacaoDTO CaracteristicaTipificacao { get; set; }
    }

    public class CaracteristicaTipificacaoDTO
    {
        public decimal nCdCaracteristica { get; set; }
        public string cNmCaracteristica { get; set; }
        public string cNrCaracteristica { get; set; }
        public string cSgCaracteristica { get; set; }
        public string cIdentificador { get; set; }
        public List<CaracteristicaTipificacaoDTO> CaracteristicasTipificacao { get; set; }
    }

    public class AreasParticipantesDTO
    {
        public decimal nCdCaracteristica { get; set; }
        public string cNmCaracteristica { get; set; }
        public string cNrCaracteristica { get; set; }
        public string cSgCaracteristica { get; set; }
        public string cIdentificador { get; set; }
        public List<AreasParticipantesDTO> AreasParticipantes { get; set; }
    }

    public class MonitoramentoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int UsuarioInsercao { get; set; }
        public System.DateTime DataInsercao { get; set; }
        public Nullable<int> UsuarioAlteracao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        public string Frequencia { get; set; }
        public Nullable<System.DateTime> Vigencia { get; set; }
        public string SiglaContusao { get; set; }
        public string SiglaFalhaOperacional { get; set; }
    }
}
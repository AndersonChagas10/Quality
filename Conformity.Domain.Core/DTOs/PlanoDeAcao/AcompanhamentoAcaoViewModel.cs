﻿using Conformity.Domain.Core.Entities.PlanoDeAcao;
using System;
using System.Collections.Generic;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;

namespace Conformity.Domain.Core.DTOs
{
    public class AcompanhamentoAcaoViewModel
    {
        public int Id { get; set; }
        public string Observacao { get; set; }
        public DateTime DataRegistro { get; set; }
        public string DataRegistroFormatada { get { return DataRegistro.ToString("dd/MM/yyyy HH:mm"); } }
        public List<NotificarViewModel> ListaNotificar { get; set; }
        public int Status { get; set; }
        public string Responsavel { get; set; }
        public string StatusName { get { return Enum.GetName(typeof(EAcaoStatus), Status); } }
        public EvidenciaXAttributeViewModel Evidencia1 { get; set; }
        public EvidenciaXAttributeViewModel Evidencia2 { get; set; }
    }
}

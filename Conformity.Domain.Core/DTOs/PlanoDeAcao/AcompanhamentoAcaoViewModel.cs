using System;
using System.Collections.Generic;

namespace Conformity.Domain.Core.DTOs
{
    public class AcompanhamentoAcaoViewModel
    {
        public string Observacao { get; set; }
        public DateTime DataRegistro { get; set; }
        public string DataRegistroFormatada { get { return DataRegistro.ToString("dd/MM/yyyy HH:mm"); } }
        public List<NotificarViewModel> ListaNotificar { get; set; }
        public Dominio.Enums.Enums.EAcaoStatus Status { get; set; }
        public string Responsavel { get; set; }
        public string StatusName { get { return Enum.GetName(typeof(Dominio.Enums.Enums.EAcaoStatus), Status); } }
    }
}

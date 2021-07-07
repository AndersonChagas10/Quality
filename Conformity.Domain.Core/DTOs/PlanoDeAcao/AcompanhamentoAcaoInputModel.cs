using System;
using System.Collections.Generic;

namespace Conformity.Domain.Core.DTOs
{
    public class AcompanhamentoAcaoInputModel
    {
        public string Observacao { get; set; }
        public DateTime DataRegistro { get { return DateTime.Now; } }
        public List<NotificarViewModel> ListaNotificar { get; set; }
        public Dominio.Enums.Enums.EAcaoStatus Status { get; set; }
    }
}

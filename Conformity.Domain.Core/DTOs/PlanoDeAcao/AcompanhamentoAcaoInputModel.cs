using Conformity.Domain.Core.Entities.PlanoDeAcao;
using System;
using System.Collections.Generic;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;

namespace Conformity.Domain.Core.DTOs
{
    public class AcompanhamentoAcaoInputModel
    {
        public string Observacao { get; set; }
        public DateTime DataRegistro { get { return DateTime.Now; } }
        public List<NotificarViewModel> ListaNotificar { get; set; }
        public EAcaoStatus Status { get; set; }
        public ICollection<AcompanhamentoAcaoXAttributes> ListaEvidencias { get; set; }
    }
}

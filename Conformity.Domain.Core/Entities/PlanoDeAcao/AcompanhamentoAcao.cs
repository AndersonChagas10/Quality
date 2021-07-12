using Conformity.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("PA.AcompanhamentoAcao")]
    public partial class AcompanhamentoAcao : BaseModel, IEntity
    {
        public int Acao_Id { get; set; }
        public int UserSgq_Id { get; set; }
        public string Observacao { get; set; }
        public DateTime DataRegistro { get; set; } = DateTime.Now;
        public ICollection<AcompanhamentoAcaoXNotificar> ListaNotificar { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey("UserSgq_Id")]
        public virtual UserSgq UserSgq { get; set; }
    }
}

using Dominio.AcaoRH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("PA.AcompanhamentoAcao")]
    public class AcompanhamentoAcao
    {
        public int Id { get; set; }
        public int Acao_Id { get; set; }
        public int UserSgq_Id { get; set; }
        public string Observacao { get; set; }
        public DateTime DataRegistro { get; set; } = DateTime.Now;
        public ICollection<AcompanhamentoAcaoXNotificar> ListaNotificar { get; set; }
        public Dominio.Enums.Enums.EAcaoStatus Status { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey("UserSgq_Id")]
        public virtual UserSgq UserSgq { get; set; }
    }
}

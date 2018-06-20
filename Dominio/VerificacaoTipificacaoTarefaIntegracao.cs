namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VerificacaoTipificacaoTarefaIntegracao")]
    public partial class VerificacaoTipificacaoTarefaIntegracao
    {
        public int Id { get; set; }

        public int TarefaId { get; set; }

        public int CaracteristicaTipificacaoId { get; set; }

        public virtual Monitoramentos Monitoramentos { get; set; }
    }
}

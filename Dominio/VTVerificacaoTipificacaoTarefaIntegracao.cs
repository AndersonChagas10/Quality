namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VTVerificacaoTipificacaoTarefaIntegracao")]
    public partial class VTVerificacaoTipificacaoTarefaIntegracao
    {
        public int Id { get; set; }

        public int TarefaId { get; set; }

        public int CaracteristicaTipificacaoId { get; set; }
    }
}

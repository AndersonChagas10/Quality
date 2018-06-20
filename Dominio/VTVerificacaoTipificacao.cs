namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VTVerificacaoTipificacao")]
    public partial class VTVerificacaoTipificacao
    {
        public int Id { get; set; }

        public int Sequencial { get; set; }

        public byte Banda { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataHora { get; set; }

        public int UnidadeId { get; set; }

        public string Chave { get; set; }

        public bool? Status { get; set; }

        public int? EvaluationNumber { get; set; }

        public int? Sample { get; set; }
    }
}

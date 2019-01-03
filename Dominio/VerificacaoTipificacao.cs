namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VerificacaoTipificacao")]
    public partial class VerificacaoTipificacao
    {
        public int Id { get; set; }

        public int Sequencial { get; set; }

        public byte Banda { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataHora { get; set; }

        public int UnidadeId { get; set; }

        public string Chave { get; set; }

        public bool? Status { get; set; }

        [ForeignKey("UnidadeId")]
        public virtual Unidades Unidades { get; set; }
    }
}

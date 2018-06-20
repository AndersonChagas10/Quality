namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ControleMetaTolerancia")]
    public partial class ControleMetaTolerancia
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UnidadeId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DepartamentoId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OperacaoId { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime Data { get; set; }

        public decimal? ProximaMetaTolerancia { get; set; }

        public decimal? UltimoNumeroNC { get; set; }

        public decimal? TotalNC { get; set; }
    }
}

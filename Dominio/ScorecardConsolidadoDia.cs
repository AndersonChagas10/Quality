namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScorecardConsolidadoDia")]
    public partial class ScorecardConsolidadoDia
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Unidade { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Operacao { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime Data { get; set; }

        public decimal? TotalAvaliado { get; set; }

        public decimal? TotalForaPadrao { get; set; }

        public decimal? Meta { get; set; }

        public decimal? Pontos { get; set; }
    }
}

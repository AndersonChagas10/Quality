namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ScorecardJBS_V
    {
        public int? Cluster { get; set; }

        [StringLength(155)]
        public string ClusterName { get; set; }

        public int? Regional { get; set; }

        [StringLength(155)]
        public string RegionalName { get; set; }

        public int? ParCompanyId { get; set; }

        [StringLength(155)]
        public string ParCompanyName { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TipoIndicador { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string TipoIndicadorName { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Level1Id { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(155)]
        public string Level1Name { get; set; }

        public int? Criterio { get; set; }

        [StringLength(155)]
        public string CriterioName { get; set; }

        public decimal? AV { get; set; }

        public decimal? NC { get; set; }

        public decimal? Pontos { get; set; }

        public decimal? Meta { get; set; }

        public decimal? Real { get; set; }

        public decimal? PontosAtingidos { get; set; }

        public decimal? Scorecard { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParGoalScorecard")]
    public partial class ParGoalScorecard
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public decimal PercentValueMid { get; set; }

        public decimal PercentValueHigh { get; set; }

        public DateTime? InitDate { get; set; }
    }
}

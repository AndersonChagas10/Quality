namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParGoalScorecard")]
    public partial class ParGoalScorecard : BaseModel
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public decimal PercentValueMid { get; set; }

        public decimal PercentValueHigh { get; set; }

        public DateTime? InitDate { get; set; }
    }
}

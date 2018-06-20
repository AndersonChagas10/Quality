namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParGoal")]
    public partial class ParGoal
    {
        public int Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public decimal PercentValue { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidoApartirDe { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

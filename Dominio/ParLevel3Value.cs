namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel3Value
    {
        public int Id { get; set; }

        public int ParLevel3_Id { get; set; }

        public int ParLevel3InputType_Id { get; set; }

        public int? ParLevel3BoolFalse_Id { get; set; }

        public int? ParLevel3BoolTrue_Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? ParMeasurementUnit_Id { get; set; }

        public bool? AcceptableValueBetween { get; set; }

        public decimal? IntervalMin { get; set; }

        public decimal? IntervalMax { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        [StringLength(100)]
        public string DynamicValue { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }

        public virtual ParLevel2 ParLevel2 { get; set; }

        public virtual ParLevel3 ParLevel3 { get; set; }

        public virtual ParLevel3BoolFalse ParLevel3BoolFalse { get; set; }

        public virtual ParLevel3BoolTrue ParLevel3BoolTrue { get; set; }

        public virtual ParLevel3InputType ParLevel3InputType { get; set; }

        public virtual ParMeasurementUnit ParMeasurementUnit { get; set; }
    }
}

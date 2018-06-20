namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParRelapse")]
    public partial class ParRelapse
    {
        public int Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int? ParLevel3_Id { get; set; }

        public int ParFrequency_Id { get; set; }

        public int NcNumber { get; set; }

        public int EffectiveLength { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public virtual ParFrequency ParFrequency { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }

        public virtual ParLevel2 ParLevel2 { get; set; }

        public virtual ParLevel3 ParLevel3 { get; set; }
    }
}

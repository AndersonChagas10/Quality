namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParRelapse")]
    public partial class ParRelapse : BaseModel
    {
        public int Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int? ParLevel3_Id { get; set; }

        public int ParFrequency_Id { get; set; }

        public int NcNumber { get; set; }

        public int EffectiveLength { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParFrequency_Id")]
        public virtual ParFrequency ParFrequency { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }
    }
}

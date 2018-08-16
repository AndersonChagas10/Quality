namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel1XModule
    {
        public int Id { get; set; }

        
        public int ParLevel1_Id { get; set; }

    
        public int ParModule_Id { get; set; }

        public decimal Points { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EffectiveDateStart { get; set; }

        //public DateTime? EffectiveDateEnd { get; set; }

        [ForeignKey("ParModule_Id")]
        public virtual ParModule ParModule { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

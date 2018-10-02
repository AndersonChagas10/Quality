namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel1XCluster
    {
        public int Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParCluster_Id { get; set; }

        public decimal Points { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public int? ParCriticalLevel_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidoApartirDe { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public virtual ParCluster ParCluster { get; set; }

        public virtual ParCriticalLevel ParCriticalLevel { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

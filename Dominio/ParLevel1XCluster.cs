namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel1XCluster : BaseModel
    {
        public int Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParCluster_Id { get; set; }

        public decimal Points { get; set; }

        public bool IsActive { get; set; }

        public int? ParCriticalLevel_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidoApartirDe { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [ForeignKey("ParCluster_Id")]
        public virtual ParCluster ParCluster { get; set; }

        [ForeignKey("ParCriticalLevel_Id")]
        public virtual ParCriticalLevel ParCriticalLevel { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

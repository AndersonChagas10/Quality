namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParClusterXModule")]
    public partial class ParClusterXModule : BaseModel
    {
        public int Id { get; set; }

        public int ParCluster_Id { get; set; }

        public int ParModule_Id { get; set; }

        public decimal Points { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [ForeignKey("ParCluster_Id")]
        public virtual ParCluster ParCluster { get; set; }

        [ForeignKey("ParModule_Id")]
        public virtual ParModule ParModule { get; set; }
    }
}

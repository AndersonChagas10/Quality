namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel1XModule : BaseModel
    {
        public int Id { get; set; }

        [NotMapped]
        [DisplayName("Indicador")]
        public IEnumerable<int> ParLevel1_IdHelper { get; set; }

        [DisplayName("Indicador")]
        public int ParLevel1_Id { get; set; }

        [DisplayName("Módulo")]
        public int ParModule_Id { get; set; }

        public decimal Points { get; set; }

        public int? ParCluster_Id { get; set; }
        

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [DisplayName("Está ativo")]
        public bool IsActive { get; set; }

        [DisplayName("Data Efetiva de início")]
        public DateTime? EffectiveDateStart { get; set; }

        [DisplayName("Data Efetiva de término")]
        public DateTime? EffectiveDateEnd { get; set; } 

        [ForeignKey("ParModule_Id")]
        public virtual ParModule ParModule { get; set; }

        [ForeignKey("ParCluster_Id")]
        public virtual ParCluster ParCluster { get; set; }

        [NotMapped]
        public virtual List<ParLevel1> ParLevel1Helper { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

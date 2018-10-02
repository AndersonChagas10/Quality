namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConsolidationLevel2
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsolidationLevel2()
        {
            CollectionLevel2 = new HashSet<CollectionLevel2>();
        }

        public int Id { get; set; }

        public int ConsolidationLevel1_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int UnitId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public int? AlertLevel { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ConsolidationDate { get; set; }

        public decimal? WeiEvaluation { get; set; }

        public decimal? EvaluateTotal { get; set; }

        public decimal? DefectsTotal { get; set; }

        public decimal? WeiDefects { get; set; }

        public int? TotalLevel3Evaluation { get; set; }

        public int? TotalLevel3WithDefects { get; set; }

        public int? LastEvaluationAlert { get; set; }

        public int? EvaluatedResult { get; set; }

        public int? DefectsResult { get; set; }

        public int? LastLevel2Alert { get; set; }

        public int? ReauditIs { get; set; }

        public int? ReauditNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel2> CollectionLevel2 { get; set; }

        public virtual ConsolidationLevel1 ConsolidationLevel1 { get; set; }

        public virtual ParLevel2 ParLevel2 { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConsolidationLevel1 : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsolidationLevel1()
        {
            ConsolidationLevel2 = new HashSet<ConsolidationLevel2>();
        }

        public int Id { get; set; }

        public int UnitId { get; set; }

        public int DepartmentId { get; set; }

        public int ParLevel1_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ConsolidationDate { get; set; }

        public decimal? Evaluation { get; set; }

        public int? AtualAlert { get; set; }

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

        public int? Shift { get; set; }

        public int? Period { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsolidationLevel2> ConsolidationLevel2 { get; set; }
    }
}

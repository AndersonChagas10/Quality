namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CollectionLevel2 : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CollectionLevel2()
        {
            CollectionLevel2XParHeaderField = new HashSet<CollectionLevel2XParHeaderField>();
            CorrectiveAction = new HashSet<CorrectiveAction>();
            Result_Level3 = new HashSet<Result_Level3>();
        }

        public int Id { get; set; }

        public int? ConsolidationLevel2_Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int UnitId { get; set; }

        public int AuditorId { get; set; }

        public int Shift { get; set; }

        public int Period { get; set; }

        public int Phase { get; set; }

        public bool ReauditIs { get; set; }

        public int ReauditNumber { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CollectionDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartPhaseDate { get; set; }

        public int? EvaluationNumber { get; set; }

        public int Sample { get; set; }

        public bool ConsecutiveFailureIs { get; set; }

        public int ConsecutiveFailureTotal { get; set; }

        public bool NotEvaluatedIs { get; set; }

        public bool Duplicated { get; set; }

        public bool? HaveCorrectiveAction { get; set; }

        public bool? HaveReaudit { get; set; }

        public bool? HavePhase { get; set; }

        public bool? Completed { get; set; }

        public int? ParFrequency_Id { get; set; }

        public int? AlertLevel { get; set; }

        public int? Sequential { get; set; }

        public int? Side { get; set; }

        public decimal? WeiEvaluation { get; set; }

        public decimal? Defects { get; set; }

        public decimal? WeiDefects { get; set; }

        public int? TotalLevel3WithDefects { get; set; }

        public int? TotalLevel3Evaluation { get; set; }

        public int? LastEvaluationAlert { get; set; }

        public int? EvaluatedResult { get; set; }

        public int? DefectsResult { get; set; }

        public bool? IsEmptyLevel3 { get; set; }

        [StringLength(50)]
        public string Key { get; set; }

        public int? LastLevel2Alert { get; set; }

        public int? ReauditLevel { get; set; }

        public int? StartPhaseEvaluation { get; set; }

        public int? CounterDonePhase { get; set; }

        public int? EndPhaseEvaluation { get; set; }

        public int? ParDepartment_Id { get; set; }

        public int? ParCargo_Id { get; set; }

        public int? ParCluster_Id { get; set; }

        [ForeignKey("ConsolidationLevel2_Id")]
        public virtual ConsolidationLevel2 ConsolidationLevel2 { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("AuditorId")]
        public virtual UserSgq UserSgq { get; set; }

        public virtual ICollection<CollectionLevel2XParHeaderField> CollectionLevel2XParHeaderField { get; set; }

        public virtual ICollection<CorrectiveAction> CorrectiveAction { get; set; }

        public virtual ICollection<Result_Level3> Result_Level3 { get; set; }
    }
}

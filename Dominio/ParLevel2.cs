namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel2 : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParLevel2()
        {
            CollectionLevel2 = new HashSet<CollectionLevel2>();
            ConsolidationLevel2 = new HashSet<ConsolidationLevel2>();
            ParCounterXLocal = new HashSet<ParCounterXLocal>();
            ParEvaluation = new HashSet<ParEvaluation>();
            ParLevel3EvaluationSample = new HashSet<ParLevel3EvaluationSample>();
            ParLevel3Value = new HashSet<ParLevel3Value>();
            ParLevel2Level1 = new HashSet<ParLevel2Level1>();
            ParLevel2ControlCompany = new HashSet<ParLevel2ControlCompany>();
            ParLevel3Group = new HashSet<ParLevel3Group>();
            ParLevel3Level2 = new HashSet<ParLevel3Level2>();
            ParNotConformityRuleXLevel = new HashSet<ParNotConformityRuleXLevel>();
            ParRelapse = new HashSet<ParRelapse>();
            ParSample = new HashSet<ParSample>();
        }

        public int Id { get; set; }

        public int? ParFrequency_Id { get; set; }

        public int ParDepartment_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Monitoramento")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool IsEmptyLevel3 { get; set; }

        public bool HasShowLevel03 { get; set; }

        public bool HasGroupLevel3 { get; set; }

        public bool IsActive { get; set; }

        public bool? HasSampleTotal { get; set; }

        public bool HasTakePhoto { get; set; }

        public virtual ICollection<CollectionLevel2> CollectionLevel2 { get; set; }

        public virtual ICollection<ConsolidationLevel2> ConsolidationLevel2 { get; set; }

        public virtual ICollection<ParCounterXLocal> ParCounterXLocal { get; set; }

        [ForeignKey("ParDepartment_Id")]
        public virtual ParDepartment ParDepartment { get; set; }

        public virtual ICollection<ParEvaluation> ParEvaluation { get; set; }

        [ForeignKey("ParFrequency_Id")]
        public virtual ParFrequency ParFrequency { get; set; }

        public virtual ICollection<ParLevel3EvaluationSample> ParLevel3EvaluationSample { get; set; }

        public virtual ICollection<ParLevel3Value> ParLevel3Value { get; set; }

        public virtual ICollection<ParLevel2Level1> ParLevel2Level1 { get; set; }

        public virtual ICollection<ParLevel2ControlCompany> ParLevel2ControlCompany { get; set; }

        public virtual ICollection<ParLevel3Group> ParLevel3Group { get; set; }

        public virtual ICollection<ParLevel3Level2> ParLevel3Level2 { get; set; }

        public virtual ICollection<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }

        public virtual ICollection<ParRelapse> ParRelapse { get; set; }

        public virtual ICollection<ParSample> ParSample { get; set; }
    }
}

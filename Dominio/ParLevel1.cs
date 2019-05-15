namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
     
    public partial class ParLevel1 : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParLevel1()
        {
            CollectionLevel2 = new HashSet<CollectionLevel2>();
            ConsolidationLevel1 = new HashSet<ConsolidationLevel1>();
            Defect = new HashSet<Defect>();
            ParCounterXLocal = new HashSet<ParCounterXLocal>();
            ParGoal = new HashSet<ParGoal>();
            ParLevel3EvaluationSample = new HashSet<ParLevel3EvaluationSample>();
            ParLevel3Value = new HashSet<ParLevel3Value>();
            VolumeCepDesossa = new HashSet<VolumeCepDesossa>();
            VolumeCepRecortes = new HashSet<VolumeCepRecortes>();
            VolumePcc1b = new HashSet<VolumePcc1b>();
            VolumeVacuoGRD = new HashSet<VolumeVacuoGRD>();
            ParLevel2Level1 = new HashSet<ParLevel2Level1>();
            ParLevel1XCluster = new HashSet<ParLevel1XCluster>();
            ParLevel1XHeaderField = new HashSet<ParLevel1XHeaderField>();
            ParLevel2ControlCompany = new HashSet<ParLevel2ControlCompany>();
            ParLevel3Level2Level1 = new HashSet<ParLevel3Level2Level1>();
            ParNotConformityRuleXLevel = new HashSet<ParNotConformityRuleXLevel>();
            ParRelapse = new HashSet<ParRelapse>();
        }

        public int Id { get; set; }

        public int ParConsolidationType_Id { get; set; }

        public int? ParFrequency_Id { get; set; }

        [NotMapped]
        public string ParFrequencyDescription { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Indicador")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool HasSaveLevel2 { get; set; }

        public bool HasNoApplicableLevel2 { get; set; }

        public bool HasGroupLevel2 { get; set; }

        public bool HasAlert { get; set; }

        public bool? IsSpecific { get; set; }

        public bool IsSpecificHeaderField { get; set; }

        public bool IsSpecificNumberEvaluetion { get; set; }

        public bool IsSpecificNumberSample { get; set; }

        public bool IsSpecificLevel3 { get; set; }

        public bool? IsSpecificGoal { get; set; }

        public bool IsRuleConformity { get; set; }

        public bool IsFixedEvaluetionNumber { get; set; }

        public bool IsLimitedEvaluetionNumber { get; set; }

        public bool IsActive { get; set; }

        public int? Level2Number { get; set; }

        public int? hashKey { get; set; }

        public bool? haveRealTimeConsolidation { get; set; }

        public int? RealTimeConsolitationUpdate { get; set; }

        public bool? IsPartialSave { get; set; }

        public bool? HasCompleteEvaluation { get; set; }

        public int? ParScoreType_Id { get; set; }

        public bool? IsChildren { get; set; }

        public int? ParLevel1Origin_Id { get; set; }

        public bool? PointsDestiny { get; set; }

        public int? ParLevel1Destiny_Id { get; set; }

        public bool EditLevel2 { get; set; }

        public bool HasTakePhoto { get; set; }

        public bool? AllowAddLevel3 { get; set; }

        public bool? AllowEditPatternLevel3Task { get; set; }

        public bool? AllowEditWeightOnLevel3 { get; set; }

        public bool? IsRecravacao { get; set; }

        public int? ParGroupLevel1_Id { get; set; }

        public bool? ShowInTablet { get; set; }

        public bool? ShowScorecard { get; set; }

        public virtual ICollection<CollectionLevel2> CollectionLevel2 { get; set; }

        public virtual ICollection<ConsolidationLevel1> ConsolidationLevel1 { get; set; }

        public virtual ICollection<Defect> Defect { get; set; }

        [ForeignKey("ParConsolidationType_Id")]
        public virtual ParConsolidationType ParConsolidationType { get; set; }

        public virtual ICollection<ParCounterXLocal> ParCounterXLocal { get; set; }

        [NotMapped]
        public List<RotinaIntegracao> RotinaIntegracao { get; set; }
        
        public virtual ICollection<ParLevel1XRotinaIntegracao> ParLevel1XRotinaIntegracao { get; set; }

        [ForeignKey("ParFrequency_Id")]
        public virtual ParFrequency ParFrequency { get; set; }

        public virtual ICollection<ParGoal> ParGoal { get; set; }

        public virtual ICollection<ParLevel3EvaluationSample> ParLevel3EvaluationSample { get; set; }

        public virtual ICollection<ParLevel3Value> ParLevel3Value { get; set; }

        public virtual ICollection<VolumeCepDesossa> VolumeCepDesossa { get; set; }

        public virtual ICollection<VolumeCepRecortes> VolumeCepRecortes { get; set; }

        public virtual ICollection<VolumePcc1b> VolumePcc1b { get; set; }

        public virtual ICollection<VolumeVacuoGRD> VolumeVacuoGRD { get; set; }

        public virtual ICollection<ParLevel2Level1> ParLevel2Level1 { get; set; }

        [ForeignKey("ParScoreType_Id")]
        public virtual ParScoreType ParScoreType { get; set; }

        public virtual ICollection<ParLevel1XCluster> ParLevel1XCluster { get; set; }

        public virtual ICollection<ParLevel1XHeaderField> ParLevel1XHeaderField { get; set; }

        public virtual ICollection<ParLevel2ControlCompany> ParLevel2ControlCompany { get; set; }

        public virtual ICollection<ParLevel3Level2Level1> ParLevel3Level2Level1 { get; set; }

        public virtual ICollection<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }

        public virtual ICollection<ParRelapse> ParRelapse { get; set; }
    }
}

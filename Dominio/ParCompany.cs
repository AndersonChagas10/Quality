namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParCompany")]
    public partial class ParCompany : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParCompany()
        {
            ConsolidationLevel1 = new HashSet<ConsolidationLevel1>();
            Defect = new HashSet<Defect>();
            ParLevel3EvaluationSample = new HashSet<ParLevel3EvaluationSample>();
            VolumeCepDesossa = new HashSet<VolumeCepDesossa>();
            VolumeCepRecortes = new HashSet<VolumeCepRecortes>();
            VolumePcc1b = new HashSet<VolumePcc1b>();
            VolumeVacuoGRD = new HashSet<VolumeVacuoGRD>();
            ParCompanyCluster = new HashSet<ParCompanyCluster>();
            ParCompanyXStructure = new HashSet<ParCompanyXStructure>();
            ParCompanyXUserSgq = new HashSet<ParCompanyXUserSgq>();
            ParEvaluation = new HashSet<ParEvaluation>();
            ParGoal = new HashSet<ParGoal>();
            ParLevel2ControlCompany = new HashSet<ParLevel2ControlCompany>();
            ParLevel3Level2 = new HashSet<ParLevel3Level2>();
            ParLevel3Value = new HashSet<ParLevel3Value>();
            ParNotConformityRuleXLevel = new HashSet<ParNotConformityRuleXLevel>();
            ParSample = new HashSet<ParSample>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Empresa")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [StringLength(50)]
        public string Initials { get; set; }

        [StringLength(155)]
        public string SIF { get; set; }

        public int? CompanyNumber { get; set; }

        [StringLength(155)]
        public string IPServer { get; set; }

        [StringLength(155)]
        public string DBServer { get; set; }

        [NotMapped]
        public string Identification { get; set; }

        public decimal? IntegrationId { get; set; }

        public int? ParCompany_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsolidationLevel1> ConsolidationLevel1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Defect> Defect { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3EvaluationSample> ParLevel3EvaluationSample { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumeCepDesossa> VolumeCepDesossa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumeCepRecortes> VolumeCepRecortes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumePcc1b> VolumePcc1b { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumeVacuoGRD> VolumeVacuoGRD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCompanyCluster> ParCompanyCluster { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCompanyXStructure> ParCompanyXStructure { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCompanyXUserSgq> ParCompanyXUserSgq { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParEvaluation> ParEvaluation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParGoal> ParGoal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel2ControlCompany> ParLevel2ControlCompany { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Level2> ParLevel3Level2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Value> ParLevel3Value { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParSample> ParSample { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParLevel1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParLevel1()
        {
            this.CollectionLevel2 = new HashSet<CollectionLevel2>();
            this.ConsolidationLevel1 = new HashSet<ConsolidationLevel1>();
            this.Defect = new HashSet<Defect>();
            this.ParCounterXLocal = new HashSet<ParCounterXLocal>();
            this.ParGoal = new HashSet<ParGoal>();
            this.VolumeCepDesossa = new HashSet<VolumeCepDesossa>();
            this.VolumeCepRecortes = new HashSet<VolumeCepRecortes>();
            this.VolumePcc1b = new HashSet<VolumePcc1b>();
            this.VolumeVacuoGRD = new HashSet<VolumeVacuoGRD>();
            this.ParLevel2Level1 = new HashSet<ParLevel2Level1>();
            this.ParLevel1XCluster = new HashSet<ParLevel1XCluster>();
            this.ParLevel1XHeaderField = new HashSet<ParLevel1XHeaderField>();
            this.ParLevel2ControlCompany = new HashSet<ParLevel2ControlCompany>();
            this.ParLevel3Level2Level1 = new HashSet<ParLevel3Level2Level1>();
            this.ParNotConformityRuleXLevel = new HashSet<ParNotConformityRuleXLevel>();
            this.ParRelapse = new HashSet<ParRelapse>();
        }
    
        public int Id { get; set; }
        public int ParConsolidationType_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasSaveLevel2 { get; set; }
        public bool HasNoApplicableLevel2 { get; set; }
        public bool HasGroupLevel2 { get; set; }
        public bool HasAlert { get; set; }
        public Nullable<bool> IsSpecific { get; set; }
        public bool IsSpecificHeaderField { get; set; }
        public bool IsSpecificNumberEvaluetion { get; set; }
        public bool IsSpecificNumberSample { get; set; }
        public bool IsSpecificLevel3 { get; set; }
        public Nullable<bool> IsSpecificGoal { get; set; }
        public bool IsRuleConformity { get; set; }
        public bool IsFixedEvaluetionNumber { get; set; }
        public bool IsLimitedEvaluetionNumber { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Level2Number { get; set; }
        public Nullable<int> hashKey { get; set; }
        public Nullable<bool> haveRealTimeConsolidation { get; set; }
        public Nullable<int> RealTimeConsolitationUpdate { get; set; }
        public Nullable<bool> IsPartialSave { get; set; }
        public Nullable<bool> HasCompleteEvaluation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel2> CollectionLevel2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsolidationLevel1> ConsolidationLevel1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Defect> Defect { get; set; }
        public virtual ParConsolidationType ParConsolidationType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCounterXLocal> ParCounterXLocal { get; set; }
        public virtual ParFrequency ParFrequency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParGoal> ParGoal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumeCepDesossa> VolumeCepDesossa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumeCepRecortes> VolumeCepRecortes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumePcc1b> VolumePcc1b { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumeVacuoGRD> VolumeVacuoGRD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel2Level1> ParLevel2Level1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel1XCluster> ParLevel1XCluster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel1XHeaderField> ParLevel1XHeaderField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel2ControlCompany> ParLevel2ControlCompany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Level2Level1> ParLevel3Level2Level1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParRelapse> ParRelapse { get; set; }
    }
}

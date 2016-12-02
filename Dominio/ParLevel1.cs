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
            this.ParCounterXLocal = new HashSet<ParCounterXLocal>();
            this.ParLevel1XCluster = new HashSet<ParLevel1XCluster>();
            this.ParLevel1XHeaderField = new HashSet<ParLevel1XHeaderField>();
            this.ParLevel3Level2Level1 = new HashSet<ParLevel3Level2Level1>();
            this.ParNotConformityRuleXLevel = new HashSet<ParNotConformityRuleXLevel>();
            this.ParRelapse = new HashSet<ParRelapse>();
            this.ParGoal = new HashSet<ParGoal>();
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
    
        public virtual ParConsolidationType ParConsolidationType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCounterXLocal> ParCounterXLocal { get; set; }
        public virtual ParFrequency ParFrequency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel1XCluster> ParLevel1XCluster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel1XHeaderField> ParLevel1XHeaderField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Level2Level1> ParLevel3Level2Level1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParRelapse> ParRelapse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParGoal> ParGoal { get; set; }
    }
}

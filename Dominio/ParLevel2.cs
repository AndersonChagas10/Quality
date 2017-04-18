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
    
    public partial class ParLevel2
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParLevel2()
        {
            this.ParEvaluation = new HashSet<ParEvaluation>();
            this.ParLevel2Level1 = new HashSet<ParLevel2Level1>();
            this.ParLevel2ControlCompany = new HashSet<ParLevel2ControlCompany>();
            this.ParLevel3Group = new HashSet<ParLevel3Group>();
            this.ParLevel3Level2 = new HashSet<ParLevel3Level2>();
            this.ParNotConformityRuleXLevel = new HashSet<ParNotConformityRuleXLevel>();
            this.ParRelapse = new HashSet<ParRelapse>();
            this.ParSample = new HashSet<ParSample>();
            this.CollectionLevel2 = new HashSet<CollectionLevel2>();
            this.ConsolidationLevel2 = new HashSet<ConsolidationLevel2>();
            this.ParCounterXLocal = new HashSet<ParCounterXLocal>();
        }
    
        public int Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public int ParDepartment_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEmptyLevel3 { get; set; }
        public bool HasShowLevel03 { get; set; }
        public bool HasGroupLevel3 { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> HasSampleTotal { get; set; }
    
        public virtual ParDepartment ParDepartment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParEvaluation> ParEvaluation { get; set; }
        public virtual ParFrequency ParFrequency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel2Level1> ParLevel2Level1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel2ControlCompany> ParLevel2ControlCompany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Group> ParLevel3Group { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Level2> ParLevel3Level2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParRelapse> ParRelapse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParSample> ParSample { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel2> CollectionLevel2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsolidationLevel2> ConsolidationLevel2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCounterXLocal> ParCounterXLocal { get; set; }
    }
}

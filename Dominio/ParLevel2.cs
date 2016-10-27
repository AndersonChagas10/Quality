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
            this.ParCounterXLocal = new HashSet<ParCounterXLocal>();
            this.ParEvaluation = new HashSet<ParEvaluation>();
            this.ParLevel3Group = new HashSet<ParLevel3Group>();
            this.ParNotConformityRuleXLevel = new HashSet<ParNotConformityRuleXLevel>();
            this.ParRelapse = new HashSet<ParRelapse>();
            this.ParSample = new HashSet<ParSample>();
        }
    
        public int Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public int ParDepartment_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEmptyLevel3 { get; set; }
        public Nullable<bool> HasShowLevel03 { get; set; }
        public bool HasGroupLevel3 { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCounterXLocal> ParCounterXLocal { get; set; }
        public virtual ParDepartment ParDepartment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParEvaluation> ParEvaluation { get; set; }
        public virtual ParFrequency ParFrequency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Group> ParLevel3Group { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParRelapse> ParRelapse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParSample> ParSample { get; set; }
    }
}

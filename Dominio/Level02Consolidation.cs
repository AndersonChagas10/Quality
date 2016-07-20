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
    
    public partial class Level02Consolidation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Level02Consolidation()
        {
            this.DataCollection = new HashSet<DataCollection>();
            this.Level03Consolidation = new HashSet<Level03Consolidation>();
        }
    
        public int Id { get; set; }
        public int Level02Id { get; set; }
        public int Level01ConsolidationId { get; set; }
        public System.DateTime DateConsolidation { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public decimal TotalLevel03 { get; set; }
        public decimal TotalLevel03Weight { get; set; }
        public decimal TotalEvaluated { get; set; }
        public decimal TotalEvaluatedWeight { get; set; }
        public decimal TotalEvaluatedShared { get; set; }
        public decimal TotalEvaluatedSharedWeight { get; set; }
        public decimal TotalNotConform { get; set; }
        public decimal TotalNotConformWeight { get; set; }
        public decimal TotalNotConformShared { get; set; }
        public decimal TotalNotConformSharedWeight { get; set; }
        public bool Shared { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DataCollection> DataCollection { get; set; }
        public virtual Level01Consolidation Level01Consolidation { get; set; }
        public virtual Level02 Level02 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Level03Consolidation> Level03Consolidation { get; set; }
    }
}

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
    
    public partial class ConsolidationLevel2
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsolidationLevel2()
        {
            this.CollectionLevel2 = new HashSet<CollectionLevel2>();
        }
    
        public int Id { get; set; }
        public int ConsolidationLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int UnitId { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public Nullable<int> AlertLevel { get; set; }
        public Nullable<System.DateTime> ConsolidationDate { get; set; }
        public Nullable<decimal> WeiEvaluation { get; set; }
        public Nullable<decimal> EvaluateTotal { get; set; }
        public Nullable<decimal> DefectsTotal { get; set; }
        public Nullable<decimal> WeiDefects { get; set; }
        public Nullable<int> TotalLevel3Evaluation { get; set; }
        public Nullable<int> TotalLevel3WithDefects { get; set; }
        public Nullable<int> LastEvaluationAlert { get; set; }
        public Nullable<int> EvaluatedResult { get; set; }
        public Nullable<int> DefectsResult { get; set; }
        public string LastLevel2Alert { get; set; }
    
        public virtual ParCompany ParCompany { get; set; }
        public virtual ParLevel2 ParLevel2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel2> CollectionLevel2 { get; set; }
    }
}

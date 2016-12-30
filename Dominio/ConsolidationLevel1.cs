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
    
    public partial class ConsolidationLevel1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsolidationLevel1()
        {
            this.ConsolidationLevel2 = new HashSet<ConsolidationLevel2>();
        }
    
        public int Id { get; set; }
        public int UnitId { get; set; }
        public int DepartmentId { get; set; }
        public int ParLevel1_Id { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public System.DateTime ConsolidationDate { get; set; }
        public Nullable<decimal> Evaluation { get; set; }
        public Nullable<int> AtualAlert { get; set; }
        public Nullable<decimal> WeiEvaluation { get; set; }
        public Nullable<decimal> EvaluateTotal { get; set; }
        public Nullable<decimal> DefectsTotal { get; set; }
        public Nullable<decimal> WeiDefects { get; set; }
        public Nullable<int> TotalLevel3Evaluation { get; set; }
        public Nullable<int> TotalLevel3WithDefects { get; set; }
        public Nullable<int> LastEvaluationAlert { get; set; }
        public Nullable<int> EvaluatedResult { get; set; }
        public Nullable<int> DefectsResult { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Unit Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsolidationLevel2> ConsolidationLevel2 { get; set; }
        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

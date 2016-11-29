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
    
    public partial class CollectionLevel02
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CollectionLevel02()
        {
            this.CollectionLevel03 = new HashSet<CollectionLevel03>();
            this.CorrectiveAction = new HashSet<CorrectiveAction>();
        }
    
        public int Id { get; set; }
        public int ConsolidationLevel02Id { get; set; }
        public int Level01Id { get; set; }
        public int Level02Id { get; set; }
        public int UnitId { get; set; }
        public int AuditorId { get; set; }
        public int Shift { get; set; }
        public int Period { get; set; }
        public int Phase { get; set; }
        public bool ReauditIs { get; set; }
        public int ReauditNumber { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public System.DateTime StartPhaseDate { get; set; }
        public Nullable<int> EvaluationNumber { get; set; }
        public int Sample { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public int CattleTypeId { get; set; }
        public decimal Chainspeed { get; set; }
        public bool ConsecutiveFailureIs { get; set; }
        public int ConsecutiveFailureTotal { get; set; }
        public decimal LotNumber { get; set; }
        public decimal Mudscore { get; set; }
        public bool NotEvaluatedIs { get; set; }
        public bool Duplicated { get; set; }
        public Nullable<bool> HaveCorrectiveAction { get; set; }
        public Nullable<bool> HaveReaudit { get; set; }
        public Nullable<bool> HavePhase { get; set; }
        public Nullable<bool> Completed { get; set; }
    
        public virtual ConsolidationLevel02 ConsolidationLevel02 { get; set; }
        public virtual Level01 Level01 { get; set; }
        public virtual Level02 Level02 { get; set; }
        public virtual UserSgq UserSgq { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel03> CollectionLevel03 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CorrectiveAction> CorrectiveAction { get; set; }
    }
}

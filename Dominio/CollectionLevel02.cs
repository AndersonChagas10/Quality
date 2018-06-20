namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CollectionLevel02
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CollectionLevel02()
        {
            CollectionLevel03 = new HashSet<CollectionLevel03>();
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

        [Column(TypeName = "datetime2")]
        public DateTime CollectionDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartPhaseDate { get; set; }

        public int? EvaluationNumber { get; set; }

        public int Sample { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public int CattleTypeId { get; set; }

        public decimal Chainspeed { get; set; }

        public bool ConsecutiveFailureIs { get; set; }

        public int ConsecutiveFailureTotal { get; set; }

        public decimal LotNumber { get; set; }

        public decimal Mudscore { get; set; }

        public bool NotEvaluatedIs { get; set; }

        public bool Duplicated { get; set; }

        public bool? HaveCorrectiveAction { get; set; }

        public bool? HaveReaudit { get; set; }

        public bool? HavePhase { get; set; }

        public bool? Completed { get; set; }

        public virtual ConsolidationLevel02 ConsolidationLevel02 { get; set; }

        public virtual Level01 Level01 { get; set; }

        public virtual Level02 Level02 { get; set; }

        public virtual UserSgq UserSgq { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel03> CollectionLevel03 { get; set; }
    }
}

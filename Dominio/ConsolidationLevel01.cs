namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConsolidationLevel01
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsolidationLevel01()
        {
            ConsolidationLevel02 = new HashSet<ConsolidationLevel02>();
        }

        public int Id { get; set; }

        public int UnitId { get; set; }

        public int DepartmentId { get; set; }

        public int Level01Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ConsolidationDate { get; set; }

        public virtual Department Department { get; set; }

        public virtual Level01 Level01 { get; set; }

        public virtual Unit Unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsolidationLevel02> ConsolidationLevel02 { get; set; }
    }
}

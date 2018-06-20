namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConsolidationLevel02
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsolidationLevel02()
        {
            CollectionLevel02 = new HashSet<CollectionLevel02>();
        }

        public int Id { get; set; }

        public int Level01ConsolidationId { get; set; }

        public int Level02Id { get; set; }

        public int UnitId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ConsolidationDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel02> CollectionLevel02 { get; set; }

        public virtual ConsolidationLevel01 ConsolidationLevel01 { get; set; }

        public virtual Level02 Level02 { get; set; }
    }
}

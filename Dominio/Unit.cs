namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Unit")]
    public partial class Unit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unit()
        {
            ConsolidationLevel01 = new HashSet<ConsolidationLevel01>();
            UnitUser = new HashSet<UnitUser>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        public int? Number { get; set; }

        public string Code { get; set; }

        public string Ip { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsolidationLevel01> ConsolidationLevel01 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitUser> UnitUser { get; set; }
    }
}

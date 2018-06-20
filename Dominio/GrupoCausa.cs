namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrupoCausa")]
    public partial class GrupoCausa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GrupoCausa()
        {
            fa_GrupoCausa = new HashSet<fa_GrupoCausa>();
        }

        public int Id { get; set; }

        [Column("GrupoCausa")]
        [StringLength(100)]
        public string GrupoCausa1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_GrupoCausa> fa_GrupoCausa { get; set; }
    }
}

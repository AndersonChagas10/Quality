namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CausaGenerica")]
    public partial class CausaGenerica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CausaGenerica()
        {
            fa_CausaGenerica = new HashSet<fa_CausaGenerica>();
        }

        public int Id { get; set; }

        [Column("CausaGenerica")]
        [StringLength(100)]
        public string CausaGenerica1 { get; set; }

        public int? GrupoCausa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_CausaGenerica> fa_CausaGenerica { get; set; }
    }
}

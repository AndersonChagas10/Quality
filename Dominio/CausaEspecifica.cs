namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CausaEspecifica")]
    public partial class CausaEspecifica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CausaEspecifica()
        {
            fa_CausaEspecifica = new HashSet<fa_CausaEspecifica>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_CausaEspecifica> fa_CausaEspecifica { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContramedidaGenerica")]
    public partial class ContramedidaGenerica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContramedidaGenerica()
        {
            fa_ContramedidaGenerica = new HashSet<fa_ContramedidaGenerica>();
        }

        public int Id { get; set; }

        [Column("ContramedidaGenerica")]
        [StringLength(100)]
        public string ContramedidaGenerica1 { get; set; }

        public int? CausaGenerica { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_ContramedidaGenerica> fa_ContramedidaGenerica { get; set; }
    }
}

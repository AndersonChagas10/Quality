namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContramedidaEspecifica")]
    public partial class ContramedidaEspecifica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContramedidaEspecifica()
        {
            fa_ContramedidaEspecifica = new HashSet<fa_ContramedidaEspecifica>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_ContramedidaEspecifica> fa_ContramedidaEspecifica { get; set; }
    }
}

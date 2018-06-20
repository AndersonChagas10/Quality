namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanoDeAcaoQuemQuandoComo")]
    public partial class PlanoDeAcaoQuemQuandoComo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanoDeAcaoQuemQuandoComo()
        {
            fa_PlanoDeAcaoQuemQuandoComo = new HashSet<fa_PlanoDeAcaoQuemQuandoComo>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Quem { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Quando { get; set; }

        [Required]
        public string Como { get; set; }

        public int? IdUsuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_PlanoDeAcaoQuemQuandoComo> fa_PlanoDeAcaoQuemQuandoComo { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}

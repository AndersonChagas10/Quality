namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UnidadesMedidas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnidadesMedidas()
        {
            PadraoMonitoramentos = new HashSet<PadraoMonitoramentos>();
            PadraoMonitoramentos1 = new HashSet<PadraoMonitoramentos>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Sigla { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoMonitoramentos> PadraoMonitoramentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoMonitoramentos> PadraoMonitoramentos1 { get; set; }
    }
}

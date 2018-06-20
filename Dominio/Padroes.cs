namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Padroes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Padroes()
        {
            PadraoMonitoramentos = new HashSet<PadraoMonitoramentos>();
            PadraoTolerancias = new HashSet<PadraoTolerancias>();
            PadraoTolerancias1 = new HashSet<PadraoTolerancias>();
        }

        public int Id { get; set; }

        [StringLength(20)]
        public string Minimo { get; set; }

        [StringLength(20)]
        public string Maximo { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoMonitoramentos> PadraoMonitoramentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoTolerancias> PadraoTolerancias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoTolerancias> PadraoTolerancias1 { get; set; }
    }
}

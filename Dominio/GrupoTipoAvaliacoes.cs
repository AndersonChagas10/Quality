namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GrupoTipoAvaliacoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GrupoTipoAvaliacoes()
        {
            GrupoTipoAvaliacaoMonitoramentos = new HashSet<GrupoTipoAvaliacaoMonitoramentos>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        public int Positivo { get; set; }

        public int Negativo { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoTipoAvaliacaoMonitoramentos> GrupoTipoAvaliacaoMonitoramentos { get; set; }

        public virtual TipoAvaliacoes TipoAvaliacoes { get; set; }

        public virtual TipoAvaliacoes TipoAvaliacoes1 { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TipoAvaliacoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoAvaliacoes()
        {
            GrupoTipoAvaliacoes = new HashSet<GrupoTipoAvaliacoes>();
            GrupoTipoAvaliacoes1 = new HashSet<GrupoTipoAvaliacoes>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

        public int Valor { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoTipoAvaliacoes> GrupoTipoAvaliacoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoTipoAvaliacoes> GrupoTipoAvaliacoes1 { get; set; }
    }
}

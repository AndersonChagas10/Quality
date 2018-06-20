namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrupoCabecalho")]
    public partial class GrupoCabecalho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GrupoCabecalho()
        {
            Campo = new HashSet<Campo>();
            VinculoCampoCabecalho = new HashSet<VinculoCampoCabecalho>();
        }

        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public int? IdGrupoCabecalhoPai { get; set; }

        public int? IdProjeto { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Campo> Campo { get; set; }

        public virtual Projeto Projeto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoCabecalho> VinculoCampoCabecalho { get; set; }
    }
}

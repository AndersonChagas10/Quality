namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrupoProjeto")]
    public partial class GrupoProjeto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GrupoProjeto()
        {
            Projeto = new HashSet<Projeto>();
        }

        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public int? Sequencia { get; set; }

        public int? IdEmpresa { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }

        public virtual Unidades Unidades { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Projeto> Projeto { get; set; }
    }
}

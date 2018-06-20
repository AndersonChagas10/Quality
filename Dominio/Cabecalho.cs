namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cabecalho")]
    public partial class Cabecalho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cabecalho()
        {
            TarefaPA = new HashSet<TarefaPA>();
            VinculoCampoCabecalho = new HashSet<VinculoCampoCabecalho>();
        }

        public int Id { get; set; }

        public int IdProjeto { get; set; }

        public int? IdParticipanteCriador { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }

        public virtual Usuarios Usuarios { get; set; }

        public virtual Projeto Projeto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaPA> TarefaPA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoCabecalho> VinculoCampoCabecalho { get; set; }
    }
}

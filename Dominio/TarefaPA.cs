namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TarefaPA")]
    public partial class TarefaPA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TarefaPA()
        {
            AcompanhamentoTarefa = new HashSet<AcompanhamentoTarefa>();
            VinculoCampoTarefa = new HashSet<VinculoCampoTarefa>();
        }

        public int Id { get; set; }

        public int IdProjeto { get; set; }

        public int? IdParticipanteCriador { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }

        public int? IdCabecalho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcompanhamentoTarefa> AcompanhamentoTarefa { get; set; }

        public virtual Cabecalho Cabecalho { get; set; }

        public virtual Projeto Projeto { get; set; }

        public virtual Usuarios Usuarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoTarefa> VinculoCampoTarefa { get; set; }
    }
}

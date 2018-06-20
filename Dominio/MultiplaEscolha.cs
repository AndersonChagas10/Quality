namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MultiplaEscolha")]
    public partial class MultiplaEscolha
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MultiplaEscolha()
        {
            VinculoCampoCabecalho = new HashSet<VinculoCampoCabecalho>();
            VinculoCampoTarefa = new HashSet<VinculoCampoTarefa>();
            VinculoParticipanteMultiplaEscolha = new HashSet<VinculoParticipanteMultiplaEscolha>();
        }

        public int Id { get; set; }

        public int IdCampo { get; set; }

        public string Nome { get; set; }

        public int? IdTabelaExterna { get; set; }

        public string Cor { get; set; }

        public string NomeTabelaExterna { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }

        public int? IdMultiplaEscolhaPai { get; set; }

        public virtual Campo Campo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoCabecalho> VinculoCampoCabecalho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoTarefa> VinculoCampoTarefa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoParticipanteMultiplaEscolha> VinculoParticipanteMultiplaEscolha { get; set; }
    }
}

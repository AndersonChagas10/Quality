namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Campo")]
    public partial class Campo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Campo()
        {
            MultiplaEscolha = new HashSet<MultiplaEscolha>();
            VinculoCampoCabecalho = new HashSet<VinculoCampoCabecalho>();
            VinculoCampoTarefa = new HashSet<VinculoCampoTarefa>();
        }

        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Tipo { get; set; }

        public bool Agrupador { get; set; }

        public int? Sequencia { get; set; }

        public bool Obrigatorio { get; set; }

        public bool Ativo { get; set; }

        public int IdProjeto { get; set; }

        public bool Predefinido { get; set; }

        public bool Modificavel { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataAlteracao { get; set; }

        public int? IdCampoPai { get; set; }

        public bool? FixadoEsquerda { get; set; }

        public bool? ExibirTabela { get; set; }

        public bool? Cabecalho { get; set; }

        public int? IdGrupoCabecalho { get; set; }

        public virtual GrupoCabecalho GrupoCabecalho { get; set; }

        public virtual Projeto Projeto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MultiplaEscolha> MultiplaEscolha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoCabecalho> VinculoCampoCabecalho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoTarefa> VinculoCampoTarefa { get; set; }
    }
}

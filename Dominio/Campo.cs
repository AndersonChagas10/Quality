//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    
    public partial class Campo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Campo()
        {
            this.MultiplaEscolha = new HashSet<MultiplaEscolha>();
            this.VinculoCampoCabecalho = new HashSet<VinculoCampoCabecalho>();
            this.VinculoCampoTarefa = new HashSet<VinculoCampoTarefa>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public bool Agrupador { get; set; }
        public Nullable<int> Sequencia { get; set; }
        public bool Obrigatorio { get; set; }
        public bool Ativo { get; set; }
        public int IdProjeto { get; set; }
        public bool Predefinido { get; set; }
        public bool Modificavel { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        public Nullable<int> IdCampoPai { get; set; }
        public Nullable<bool> FixadoEsquerda { get; set; }
        public Nullable<bool> ExibirTabela { get; set; }
        public Nullable<bool> Cabecalho { get; set; }
        public Nullable<int> IdGrupoCabecalho { get; set; }
    
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

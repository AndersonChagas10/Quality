//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SgqSystem.PlanoAcao.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TarefaPA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TarefaPA()
        {
            this.AcompanhamentoTarefa = new HashSet<AcompanhamentoTarefa>();
            this.VinculoCampoTarefa = new HashSet<VinculoCampoTarefa>();
        }
    
        public int Id { get; set; }
        public int IdProjeto { get; set; }
        public Nullable<int> IdParticipanteCriador { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public Nullable<int> IdCabecalho { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcompanhamentoTarefa> AcompanhamentoTarefa { get; set; }
        public virtual Campo Cabecalho { get; set; }
        public virtual Projeto Projeto { get; set; }
        public virtual Usuarios Usuarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoTarefa> VinculoCampoTarefa { get; set; }
    }
}

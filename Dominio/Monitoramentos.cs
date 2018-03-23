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
    
    public partial class Monitoramentos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Monitoramentos()
        {
            this.TarefaMonitoramentos = new HashSet<TarefaMonitoramentos>();
            this.VerificacaoTipificacaoTarefaIntegracao = new HashSet<VerificacaoTipificacaoTarefaIntegracao>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
        public int UsuarioInsercao { get; set; }
        public System.DateTime DataInsercao { get; set; }
        public Nullable<int> UsuarioAlteracao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        public string Frequencia { get; set; }
        public Nullable<System.DateTime> Vigencia { get; set; }
        public string SiglaContusao { get; set; }
        public string SiglaFalhaOperacional { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaMonitoramentos> TarefaMonitoramentos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VerificacaoTipificacaoTarefaIntegracao> VerificacaoTipificacaoTarefaIntegracao { get; set; }
    }
}

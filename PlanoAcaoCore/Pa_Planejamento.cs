//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlanoAcaoCore
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pa_Planejamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pa_Planejamento()
        {
            this.Pa_Acao = new HashSet<Pa_Acao>();
        }
    
        public int Id { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public Nullable<int> Diretoria_Id { get; set; }
        public Nullable<int> Gerencia_Id { get; set; }
        public Nullable<int> Coordenacao_Id { get; set; }
        public Nullable<int> Missao_Id { get; set; }
        public Nullable<int> Visao_Id { get; set; }
        public Nullable<int> TemaAssunto_Id { get; set; }
        public Nullable<int> Indicadores_Id { get; set; }
        public Nullable<int> Iniciativa_Id { get; set; }
        public Nullable<int> ObjetivoGerencial_Id { get; set; }
        public string Dimensao { get; set; }
        public string Objetivo { get; set; }
        public decimal ValorDe { get; set; }
        public decimal ValorPara { get; set; }
        public Nullable<System.DateTime> DataInicio { get; set; }
        public Nullable<System.DateTime> DataFim { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<int> Dimensao_Id { get; set; }
        public Nullable<int> Objetivo_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pa_Acao> Pa_Acao { get; set; }
    }
}

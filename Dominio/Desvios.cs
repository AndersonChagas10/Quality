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
    
    public partial class Desvios
    {
        public int Id { get; set; }
        public System.DateTime DataHora { get; set; }
        public int UnidadeId { get; set; }
        public string Unidade { get; set; }
        public int DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int OperacaoId { get; set; }
        public string Operacao { get; set; }
        public int NumeroAvaliacao { get; set; }
        public int Desvio { get; set; }
        public Nullable<int> MailItemId { get; set; }
        public Nullable<decimal> Meta { get; set; }
        public Nullable<decimal> Real { get; set; }
        public Nullable<int> TarefaId { get; set; }
        public Nullable<int> NumeroAmostra { get; set; }
        public Nullable<int> AlertaEmitido { get; set; }
    }
}

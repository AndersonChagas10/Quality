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
    
    public partial class ApontamentosDiarios_Result
    {
        public System.DateTime Data { get; set; }
        public string Indicador { get; set; }
        public string Monitoramento { get; set; }
        public string Tarefa { get; set; }
        public Nullable<decimal> Peso { get; set; }
        public string IntervaloMinimo { get; set; }
        public string IntervaloMaximo { get; set; }
        public string Lancado { get; set; }
        public Nullable<bool> Conforme { get; set; }
        public Nullable<bool> NA { get; set; }
        public Nullable<decimal> AV_Peso { get; set; }
        public Nullable<decimal> NC_Peso { get; set; }
        public Nullable<int> Avaliacao { get; set; }
        public int Amostra { get; set; }
    }
}

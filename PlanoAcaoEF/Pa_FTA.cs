//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pa_FTA
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public string MetaFTA { get; set; }
        public string PercentualNCFTA { get; set; }
        public string ReincidenciaDesvioFTA { get; set; }
        public Nullable<int> Supervisor_Id { get; set; }
        public Nullable<System.DateTime> DataInicioFTA { get; set; }
        public Nullable<System.DateTime> DataFimFTA { get; set; }
        public Nullable<int> Order { get; set; }
    }
}

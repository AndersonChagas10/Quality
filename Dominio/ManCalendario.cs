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
    
    public partial class ManCalendario
    {
        public int id { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public Nullable<bool> Feriado { get; set; }
        public Nullable<bool> DiaUtil { get; set; }
        public Nullable<int> NrDiaSemana { get; set; }
        public Nullable<int> NrSemanaMes { get; set; }
        public Nullable<int> NrSemanaAno { get; set; }
        public Nullable<int> Sabado { get; set; }
        public Nullable<int> Domingo { get; set; }
    }
}

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
    
    public partial class Pcc1b
    {
        public int Id { get; set; }
        public Nullable<int> Indicador { get; set; }
        public Nullable<int> Unidade { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public string Departamento { get; set; }
        public Nullable<int> VolumeAnimais { get; set; }
        public Nullable<int> Quartos { get; set; }
        public Nullable<decimal> Meta { get; set; }
        public Nullable<float> ToleranciaDia { get; set; }
        public Nullable<float> Nivel11 { get; set; }
        public Nullable<float> Nivel12 { get; set; }
        public Nullable<float> Nivel13 { get; set; }
        public Nullable<int> Avaliacoes { get; set; }
        public Nullable<int> Amostras { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public Nullable<int> ParCompany_id { get; set; }
        public Nullable<int> ParLevel1_id { get; set; }
    
        public virtual ParLevel1 ParLevel1 { get; set; }
        public virtual ParCompany ParCompany { get; set; }
    }
}

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
    
    public partial class ParLevel3Value
    {
        public int Id { get; set; }
        public int ParLevel3_Id { get; set; }
        public int ParLevel3InputType_Id { get; set; }
        public Nullable<int> ParLevel3BoolFalse_Id { get; set; }
        public Nullable<int> ParLevel3BoolTrue_Id { get; set; }
        public Nullable<int> ParCompany_Id { get; set; }
        public Nullable<int> ParMeasurementUnit_Id { get; set; }
        public Nullable<bool> AcceptableValueBetween { get; set; }
        public Nullable<decimal> IntervalMin { get; set; }
        public Nullable<decimal> IntervalMax { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ParLevel3 ParLevel3 { get; set; }
        public virtual ParLevel3BoolFalse ParLevel3BoolFalse { get; set; }
        public virtual ParLevel3BoolTrue ParLevel3BoolTrue { get; set; }
        public virtual ParLevel3InputType ParLevel3InputType { get; set; }
        public virtual ParMeasurementUnit ParMeasurementUnit { get; set; }
        public virtual ParCompany ParCompany { get; set; }
    }
}

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
    
    public partial class manDataCollectIT1
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> instantDatetime { get; set; }
        public Nullable<System.DateTime> referenceDatetime { get; set; }
        public Nullable<int> userSGQ_id { get; set; }
        public Nullable<int> parCompany_id { get; set; }
        public Nullable<int> parFrequency_id { get; set; }
        public Nullable<int> shift { get; set; }
        public string dataType { get; set; }
        public Nullable<decimal> amountData { get; set; }
        public Nullable<int> parMeasurementUnit_Id { get; set; }
        public string comments { get; set; }
        public System.DateTime addDate { get; set; }
        public Nullable<System.DateTime> alterDate { get; set; }
        public bool isActive { get; set; }
    }
}

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
    
    public partial class ParLevel2ControlCompany
    {
        public int Id { get; set; }
        public Nullable<int> ParCompany_Id { get; set; }
        public Nullable<int> ParLevel1_Id { get; set; }
        public Nullable<int> ParLevel2_Id { get; set; }
        public Nullable<System.DateTime> InitDate { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual ParCompany ParCompany { get; set; }
        public virtual ParLevel2 ParLevel2 { get; set; }
        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

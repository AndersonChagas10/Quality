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
    
    public partial class ParEvaluation
    {
        public int Id { get; set; }
        public Nullable<int> ParCompany_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int Number { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ParLevel1_Id { get; set; }
    
        public virtual ParLevel2 ParLevel2 { get; set; }
        public virtual ParCompany ParCompany { get; set; }
    }
}

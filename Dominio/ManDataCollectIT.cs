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
    
    public partial class ManDataCollectIT
    {
        public int Id { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public Nullable<System.DateTime> ReferenceDatetime { get; set; }
        public Nullable<int> UserSGQ_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public int DimManutencaoColetaITs_id { get; set; }
        public decimal AmountData { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }
    
        public virtual DimManutencaoColetaITs DimManutencaoColetaITs { get; set; }
        public virtual ParCompany ParCompany { get; set; }
    }
}

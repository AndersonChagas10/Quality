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
    
    public partial class ParCompanyXUserSgq
    {
        public int Id { get; set; }
        public int UserSgq_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public string Role { get; set; }
    
        public virtual UserSgq UserSgq { get; set; }
        public virtual ParCompany ParCompany { get; set; }
    }
}

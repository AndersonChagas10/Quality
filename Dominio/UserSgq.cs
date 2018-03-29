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
    
    public partial class UserSgq
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserSgq()
        {
            this.CorrectiveAction = new HashSet<CorrectiveAction>();
            this.CorrectiveAction1 = new HashSet<CorrectiveAction>();
            this.CorrectiveAction2 = new HashSet<CorrectiveAction>();
            this.ParCompanyXUserSgq = new HashSet<ParCompanyXUserSgq>();
            this.UnitUser = new HashSet<UnitUser>();
            this.CollectionLevel2 = new HashSet<CollectionLevel2>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> AcessDate { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<int> ParCompany_Id { get; set; }
        public Nullable<System.DateTime> PasswordDate { get; set; }
        public bool UseActiveDirectory { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CorrectiveAction> CorrectiveAction { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CorrectiveAction> CorrectiveAction1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CorrectiveAction> CorrectiveAction2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParCompanyXUserSgq> ParCompanyXUserSgq { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitUser> UnitUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel2> CollectionLevel2 { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserSgq")]
    public partial class UserSgq
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserSgq()
        {
            CollectionLevel2 = new HashSet<CollectionLevel2>();
            CorrectiveAction = new HashSet<CorrectiveAction>();
            CorrectiveAction1 = new HashSet<CorrectiveAction>();
            CorrectiveAction2 = new HashSet<CorrectiveAction>();
            ParCompanyXUserSgq = new HashSet<ParCompanyXUserSgq>();
            UnitUser = new HashSet<UnitUser>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime? AcessDate { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        public string Role { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int? ParCompany_Id { get; set; }

        public DateTime? PasswordDate { get; set; }

        public bool UseActiveDirectory { get; set; }

        public bool? IsActive { get; set; }

        public bool? ShowAllUnits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectionLevel2> CollectionLevel2 { get; set; }

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
    }
}

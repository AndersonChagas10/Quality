namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserSgq")]
    public partial class UserSgq : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserSgq()
        {
            CollectionLevel2 = new HashSet<CollectionLevel2>();
            ParCompanyXUserSgq = new HashSet<ParCompanyXUserSgq>();
            UnitUser = new HashSet<UnitUser>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; }

        public DateTime? AcessDate { get; set; }

        public string Role { get; set; }

        [Required(AllowEmptyStrings = true)]
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
        public virtual ICollection<ParCompanyXUserSgq> ParCompanyXUserSgq { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitUser> UnitUser { get; set; }
    }
}

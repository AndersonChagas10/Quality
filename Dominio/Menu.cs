namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            MenuXRoles = new HashSet<MenuXRoles>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Class { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int GroupMenu_Id { get; set; }

        public bool IsActive { get; set; }

        public virtual GroupMenu GroupMenu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuXRoles> MenuXRoles { get; set; }
    }
}

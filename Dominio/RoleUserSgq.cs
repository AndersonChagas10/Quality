namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleUserSgq")]
    public partial class RoleUserSgq : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoleUserSgq()
        {
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool? FazColeta { get; set; }

        public bool? IsCorporativo { get; set; }

        public bool? IsNegocio { get; set; }

        public bool? IsRegional { get; set; }
    }
}

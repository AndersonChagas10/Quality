namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleUserSgqXItemMenu")]
    public partial class RoleUserSgqXItemMenu
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AlterDate { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int ItemMenu_Id { get; set; }

        public int RoleUserSgq_Id { get; set; }

        public bool? IsActive { get; set; }
    }
}

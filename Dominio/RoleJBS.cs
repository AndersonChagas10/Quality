namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoleJBS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ScreenComponent_Id { get; set; }

        [StringLength(10)]
        public string Role { get; set; }

        public int? Id { get; set; }
    }
}

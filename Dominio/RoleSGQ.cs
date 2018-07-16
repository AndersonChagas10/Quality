namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleSGQ")]
    public partial class RoleSGQ
    {
        public int Id { get; set; }

        public int ScreenComponent_Id { get; set; }

        [StringLength(10)]
        public string Role { get; set; }
    }
}

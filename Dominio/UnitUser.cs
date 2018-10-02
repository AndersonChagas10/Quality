namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnitUser")]
    public partial class UnitUser
    {
        public int Id { get; set; }

        public int UserSgqId { get; set; }

        public int UnitId { get; set; }

        public string Role { get; set; }

        public virtual Unit Unit { get; set; }

        public virtual UserSgq UserSgq { get; set; }
    }
}

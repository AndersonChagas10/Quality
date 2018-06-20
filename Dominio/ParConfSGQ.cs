namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParConfSGQ")]
    public partial class ParConfSGQ
    {
        public int Id { get; set; }

        public bool? HaveUnitLogin { get; set; }

        public bool? HaveShitLogin { get; set; }
    }
}

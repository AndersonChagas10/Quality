namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangFire.AggregatedCounter")]
    public partial class AggregatedCounter1
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        public long Value { get; set; }

        public DateTime? ExpireAt { get; set; }
    }
}

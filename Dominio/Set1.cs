namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangFire.Set")]
    public partial class Set1
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        public double Score { get; set; }

        [Required]
        [StringLength(256)]
        public string Value { get; set; }

        public DateTime? ExpireAt { get; set; }
    }
}

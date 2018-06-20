namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hash")]
    public partial class Hash
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        [Required]
        [StringLength(100)]
        public string Field { get; set; }

        public string Value { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ExpireAt { get; set; }
    }
}

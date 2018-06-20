namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Terceiro")]
    public partial class Terceiro
    {
        [Key]
        public decimal nCdTerceiro { get; set; }

        [Required]
        [StringLength(50)]
        public string cNmTerceiro { get; set; }
    }
}

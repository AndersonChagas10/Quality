namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MigrationHistory")]
    public partial class MigrationHistory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(155)]
        public string Name { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }
    }
}

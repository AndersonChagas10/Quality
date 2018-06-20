namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JobQueue")]
    public partial class JobQueue
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        [Required]
        [StringLength(50)]
        public string Queue { get; set; }

        public DateTime? FetchedAt { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("State")]
    public partial class State
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Data { get; set; }

        public virtual Job Job { get; set; }
    }
}

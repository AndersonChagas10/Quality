namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JobParameter")]
    public partial class JobParameter
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public string Value { get; set; }

        public virtual Job Job { get; set; }
    }
}

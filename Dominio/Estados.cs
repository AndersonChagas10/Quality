namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Estados
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }
    }
}

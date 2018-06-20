namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScreenComponent")]
    public partial class ScreenComponent
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string HashKey { get; set; }

        [Key]
        [Column(Order = 2)]
        public string Component { get; set; }

        public int? Type { get; set; }
    }
}

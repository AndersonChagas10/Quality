namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParGroupParLevel1Type
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool IsActive { get; set; }
    }
}

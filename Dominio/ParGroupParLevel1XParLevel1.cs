namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParGroupParLevel1XParLevel1
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParGroupParLevel1_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool IsActive { get; set; }
    }
}

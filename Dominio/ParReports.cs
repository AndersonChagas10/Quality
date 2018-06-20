namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParReports
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        public string groupReport { get; set; }

        [Key]
        [Column(Order = 1)]
        public string name { get; set; }

        [Key]
        [Column(Order = 2)]
        public string query { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool IsActive { get; set; }
    }
}

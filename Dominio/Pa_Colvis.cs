namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_Colvis
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [StringLength(255)]
        public string ColVisShow { get; set; }

        [StringLength(255)]
        public string ColVisHide { get; set; }

        public int Pa_Quem_Id { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel3Level2Level1
    {
        public int Id { get; set; }

        public int ParLevel3Level2_Id { get; set; }

        public int ParLevel1_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool Active { get; set; }

        public int? ParCompany_Id { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }

        public virtual ParLevel3Level2 ParLevel3Level2 { get; set; }
    }
}

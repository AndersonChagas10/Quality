namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel2ControlCompany
    {
        public int Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InitDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool? IsActive { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }

        public virtual ParLevel2 ParLevel2 { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParCounterXLocal")]
    public partial class ParCounterXLocal
    {
        public int Id { get; set; }

        public int ParLocal_Id { get; set; }

        public int ParCounter_Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int? ParLevel3_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public virtual ParCounter ParCounter { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }

        public virtual ParLevel2 ParLevel2 { get; set; }

        public virtual ParLevel3 ParLevel3 { get; set; }

        public virtual ParLocal ParLocal { get; set; }
    }
}

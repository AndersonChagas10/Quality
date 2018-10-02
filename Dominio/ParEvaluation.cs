namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParEvaluation")]
    public partial class ParEvaluation
    {
        public int Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int Number { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParCluster_Id { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel2 ParLevel2 { get; set; }
    }
}

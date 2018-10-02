namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel3EvaluationSample
    {
        public int Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int ParLevel3_Id { get; set; }

        public decimal? SampleNumber { get; set; }

        public decimal? EvaluationNumber { get; set; }

        [StringLength(30)]
        public string EvaluationInterval { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        public bool? IsActive { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }

        public virtual ParLevel2 ParLevel2 { get; set; }

        public virtual ParLevel3 ParLevel3 { get; set; }
    }
}

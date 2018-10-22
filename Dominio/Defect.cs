namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Defect")]
    public partial class Defect : BaseModel
    {
        public int Id { get; set; }

        public int ParCompany_Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public decimal Defects { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public bool Active { get; set; }

        public int? Evaluations { get; set; }

        public int? CurrentEvaluation { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

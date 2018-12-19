namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParGoal")]
    public partial class ParGoal : BaseModel
    {
        public int Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public decimal PercentValue { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidoApartirDe { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

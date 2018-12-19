namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParNotConformityRuleXLevel")]
    public partial class ParNotConformityRuleXLevel : BaseModel
    {
        public int Id { get; set; }

        public int ParNotConformityRule_Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public decimal? Value { get; set; }

        public int Level { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int? ParLevel3_Id { get; set; }

        public bool? IsReaudit { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParNotConformityRule_Id")]
        public virtual ParNotConformityRule ParNotConformityRule { get; set; }
    }
}

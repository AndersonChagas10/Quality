namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParEvaluation")]
    public partial class ParEvaluation : BaseModel
    {
        public int Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int Number { get; set; }

        public int? Sample { get; set; }

        public bool IsActive { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParCluster_Id { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }
    }
}

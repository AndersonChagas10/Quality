namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParAlert : BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column("ParDepartment_Id")]
        public int? ParSecao_Ids { get; set; }

        [Column("ParCargo_Id")]
        public int? ParCargo_Ids { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int? ParLevel3_Id { get; set; }

        [Column("ParCompany_Id")]
        public int? ParCompany_Ids { get; set; }

        public int ParAlertType_Id { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsCollectAlert { get; set; } = true;

        public bool HasCorrectiveAction { get; set; } = true;

        [ForeignKey("ParSecao_Ids")]
        public virtual ParDepartment ParDepartment { get; set; }

        [ForeignKey("ParCargo_Ids")]
        public virtual ParCargo ParCargo { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParCompany_Ids")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParAlertType_Id")]
        public virtual ParAlertType ParAlertType { get; set; }

        public virtual List<ParAlertXUser> ParAlertXUsers { get; set; }
    }
}

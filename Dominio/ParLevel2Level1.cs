namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel2Level1 : BaseModel
    {
        public int Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }
    }
}

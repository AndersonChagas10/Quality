namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParCounterXLocal")]
    public partial class ParCounterXLocal : BaseModel
    {
        public int Id { get; set; }

        public int ParLocal_Id { get; set; }

        public int ParCounter_Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int? ParLevel3_Id { get; set; }

        public bool IsActive { get; set; }
        
        [ForeignKey("ParCounter_Id")]
        public virtual ParCounter ParCounter { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParLocal_Id")]
        public virtual ParLocal ParLocal { get; set; }
    }
}

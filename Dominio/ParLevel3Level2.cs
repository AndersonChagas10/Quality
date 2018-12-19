namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel3Level2 : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParLevel3Level2()
        {
            ParLevel3Level2Level1 = new HashSet<ParLevel3Level2Level1>();
        }

        public int Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int ParLevel3_Id { get; set; }

        public int? ParLevel3Group_Id { get; set; }

        public decimal Weight { get; set; }

        public bool IsActive { get; set; }

        public int? ParCompany_Id { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParLevel3Group_Id")]
        public virtual ParLevel3Group ParLevel3Group { get; set; }

        public virtual ICollection<ParLevel3Level2Level1> ParLevel3Level2Level1 { get; set; }
    }
}

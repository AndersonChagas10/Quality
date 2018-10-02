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

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel2 ParLevel2 { get; set; }

        public virtual ParLevel3 ParLevel3 { get; set; }

        public virtual ParLevel3Group ParLevel3Group { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel3Level2Level1> ParLevel3Level2Level1 { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Unit")] 
    public partial class Unit : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unit()
        {
            UnitUser = new HashSet<UnitUser>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        public int? Number { get; set; }

        public string Code { get; set; }

        public string Ip { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitUser> UnitUser { get; set; }
    }
}

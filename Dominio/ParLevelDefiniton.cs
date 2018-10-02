namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParLevelDefiniton")]
    public partial class ParLevelDefiniton : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParLevelDefiniton()
        {
            ParHeaderField = new HashSet<ParHeaderField>();
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParHeaderField> ParHeaderField { get; set; }
    }
}

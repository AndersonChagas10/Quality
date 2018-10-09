namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParNotConformityRule")]
    public partial class ParNotConformityRule : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParNotConformityRule()
        {
            ParNotConformityRuleXLevel = new HashSet<ParNotConformityRuleXLevel>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Sufix { get; set; }

        public bool IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParNotConformityRuleXLevel> ParNotConformityRuleXLevel { get; set; }
    }
}

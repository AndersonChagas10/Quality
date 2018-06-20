namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParStructureGroup")]
    public partial class ParStructureGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParStructureGroup()
        {
            ParStructure = new HashSet<ParStructure>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(155)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int ParStructureGroupParent_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool Active { get; set; }

        public bool? IsParentCompany { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParStructure> ParStructure { get; set; }
    }
}

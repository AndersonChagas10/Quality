namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParStructureGroup")]
    public partial class ParStructureGroup : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParStructureGroup()
        {
            ParStructure = new HashSet<ParStructure>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("Descrição")]
        public string Description { get; set; }

        [DisplayName("É filho de")]
        public int ParStructureGroupParent_Id { get; set; }

        public bool Active { get; set; }

        public bool? IsParentCompany { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParStructure> ParStructure { get; set; }

        [NotMapped]
        public ParStructureGroup ParStructureGroupHelper { get; set; }
    }
}

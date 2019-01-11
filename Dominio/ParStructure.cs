namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParStructure")]
    public partial class ParStructure : BaseModel
    {

        public int Id { get; set; }

        public int ParStructureGroup_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public int ParStructureParent_Id { get; set; }

        public bool Active { get; set; }

        [ForeignKey("ParStructureGroup_Id")]
        public virtual ParStructureGroup ParStructureGroup { get; set; }
    }
}

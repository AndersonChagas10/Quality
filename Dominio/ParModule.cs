namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParModule")]
    public partial class ParModule : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParModule()
        {
            ParClusterXModule = new HashSet<ParClusterXModule>();
            ParModuleXModuleChild = new HashSet<ParModuleXModule>();
            ParModuleXModuleParent = new HashSet<ParModuleXModule>();
        }

        public int Id { get; set; }

        //[Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        //[Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public virtual ICollection<ParClusterXModule> ParClusterXModule { get; set; }

        [NotMapped]
        public virtual ICollection<ParModuleXModule> ParModuleXModuleChild { get; set; }

        [NotMapped]
        public virtual ICollection<ParModuleXModule> ParModuleXModuleParent { get; set; }
    }
}

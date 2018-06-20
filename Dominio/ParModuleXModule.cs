namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParModuleXModule")]
    public partial class ParModuleXModule
    {
        public int Id { get; set; }

        public int ParModuleParent_Id { get; set; }

        public int ParModuleChild_Id { get; set; }

        public virtual ParModule ParModule { get; set; }

        public virtual ParModule ParModule1 { get; set; }
    }
}

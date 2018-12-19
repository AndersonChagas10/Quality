namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParCompanyXStructure")]
    public partial class ParCompanyXStructure : BaseModel
    {
        public int Id { get; set; }

        public int ParStructure_Id { get; set; }

        public int ParCompany_Id { get; set; }

        public bool Active { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParStructure_Id")]
        public virtual ParStructure ParStructure { get; set; }
    }
}

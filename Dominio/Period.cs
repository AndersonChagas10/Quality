namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Period")]
    public partial class Period : BaseModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(50)]
        public string Description { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CollectionHtml")]
    public partial class CollectionHtml : BaseModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Html { get; set; }

        public int Period { get; set; }

        public int Shift { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CollectionDate { get; set; }

        public int UnitId { get; set; }
    }
}

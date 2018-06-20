namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CollectionLevel03
    {
        public int Id { get; set; }

        public int CollectionLevel02Id { get; set; }

        public int? Level03Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool ConformedIs { get; set; }

        public decimal Value { get; set; }

        [Required]
        public string ValueText { get; set; }

        public bool? Duplicated { get; set; }

        public virtual CollectionLevel02 CollectionLevel02 { get; set; }

        public virtual Level03 Level03 { get; set; }
    }
}

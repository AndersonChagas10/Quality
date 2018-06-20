namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ItemMenu")]
    public partial class ItemMenu
    {
        public int Id { get; set; }

        public int? ItemMenu_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Icon { get; set; }

        [StringLength(255)]
        public string Url { get; set; }

        [StringLength(255)]
        public string Resource { get; set; }

        public bool? IsActive { get; set; }
    }
}

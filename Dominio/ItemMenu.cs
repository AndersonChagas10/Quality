namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ItemMenu")]
    public partial class ItemMenu : BaseModel
    {
        public int Id { get; set; }

        public int? ItemMenu_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(255)]
        public string Name { get; set; } = "";

        [StringLength(255)]
        public string Icon { get; set; } = "";

        [StringLength(255)]
        public string Url { get; set; } = "";

        [StringLength(255)]
        public string Resource { get; set; } = "";

        public int? PDCAMenuItem { get; set; }

        public bool? IsActive { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BkpCollection")]
    public partial class BkpCollection : BaseModel
    {
        public int Id { get; set; }

        public int User_Id { get; set; }

        public int Shift { get; set; }

        public int Period { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Html { get; set; }

        public string Error { get; set; }

        public string Json { get; set; }
    }
}

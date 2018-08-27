namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogJson")]
    public partial class LogJson
    {
        public Int64 Id { get; set; }

        [Column(TypeName = "text")]
        [Required(AllowEmptyStrings = true)]
        public string result { get; set; }

        [Column(TypeName = "text")]
        [Required(AllowEmptyStrings = true)]
        public string log { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        public string Device_Id { get; set; }

        public string AppVersion { get; set; }

        [StringLength(50)]
        public string callback { get; set; }
    }
}

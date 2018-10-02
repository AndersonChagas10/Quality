namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CollectionJson")]
    public partial class CollectionJson : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CollectionJson()
        {
        }

        public int Id { get; set; }

        public int? Unit_Id { get; set; }

        public int? Shift { get; set; }

        public int? Period { get; set; }

        public int? level01_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Level01CollectionDate { get; set; }

        public int? level02_Id { get; set; }

        public int? Evaluate { get; set; }

        public int? Sample { get; set; }

        public int? AuditorId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Level02CollectionDate { get; set; }

        [Column(TypeName = "text")]
        public string Level02HeaderJson { get; set; }

        [Column(TypeName = "text")]
        [Required(AllowEmptyStrings = true)]
        public string Level03ResultJSon { get; set; }

        [Column(TypeName = "text")]
        public string CorrectiveActionJson { get; set; }

        public bool Reaudit { get; set; }

        public int? ReauditNumber { get; set; }

        public bool? haveReaudit { get; set; }

        public bool? haveCorrectiveAction { get; set; }

        public string Device_Id { get; set; }

        public string AppVersion { get; set; }

        [StringLength(50)]
        public string Ambient { get; set; }

        public bool IsProcessed { get; set; }

        public string Device_Mac { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Key { get; set; }

        public string TTP { get; set; }

        public int? ReauditLevel { get; set; }

    }
}

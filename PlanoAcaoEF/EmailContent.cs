namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmailContent")]
    public partial class EmailContent
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Required]
        [StringLength(450)]
        public string To { get; set; }

        public string Body { get; set; }

        [StringLength(15)]
        public string SendStatus { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SendDate { get; set; }

        [StringLength(15)]
        public string Project { get; set; }

        public bool? IsBodyHtml { get; set; }

        [StringLength(35)]
        public string From { get; set; }

        [StringLength(450)]
        public string Subject { get; set; }
    }
}

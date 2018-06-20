namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SgqConfig")]
    public partial class SgqConfig
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ActiveIn { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool recoveryPassAvaliable { get; set; }

        public string urlPreffixAppColleta { get; set; }

        public string urlAppColleta { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool mockLoginEUA { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool MailSSL { get; set; }

        public string MailFrom { get; set; }

        public string MailPass { get; set; }

        public string MailSmtp { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MailPort { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool MockEmail { get; set; }
    }
}

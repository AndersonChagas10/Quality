namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CorrectiveAction")]
    public partial class CorrectiveAction
    {
        public int Id { get; set; }

        public int AuditorId { get; set; }

        public int CollectionLevel02Id { get; set; }

        public int? SlaughterId { get; set; }

        public int? TechinicalId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateTimeSlaughter { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateTimeTechinical { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCorrectiveAction { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AuditStartTime { get; set; }

        public string DescriptionFailure { get; set; }

        public string ImmediateCorrectiveAction { get; set; }

        public string ProductDisposition { get; set; }

        public string PreventativeMeasure { get; set; }

        public int? CollectionLevel2_Id { get; set; }

        public bool? MailProcessed { get; set; }

        public int? EmailContent_Id { get; set; }

        public virtual CollectionLevel2 CollectionLevel2 { get; set; }

        public virtual EmailContent EmailContent { get; set; }

        public virtual UserSgq UserSgq { get; set; }

        public virtual UserSgq UserSgq1 { get; set; }

        public virtual UserSgq UserSgq2 { get; set; }
    }
}

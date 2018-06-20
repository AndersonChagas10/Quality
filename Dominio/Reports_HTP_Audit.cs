namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reports_HTP_Audit
    {
        [Column("Plant Number")]
        public int? Plant_Number { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Shift { get; set; }

        [Column("Start Date", TypeName = "datetime2")]
        public DateTime? Start_Date { get; set; }

        [Column("Start Time")]
        [StringLength(10)]
        public string Start_Time { get; set; }

        [Column("Job Id")]
        public string Job_Id { get; set; }

        public string Auditor { get; set; }

        [Key]
        [Column("Audit/ReAudit", Order = 1)]
        [StringLength(8)]
        public string Audit_ReAudit { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Period { get; set; }

        [Key]
        [Column("Baised/Unbiased", Order = 3)]
        [StringLength(8)]
        public string Baised_Unbiased { get; set; }

        [Column("Job Date", TypeName = "date")]
        public DateTime? Job_Date { get; set; }

        [Column("Job Time")]
        [StringLength(10)]
        public string Job_Time { get; set; }

        [Key]
        [Column("Starting Phase", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Starting_Phase { get; set; }

        [Column("Core Practice HTP Deviation")]
        [StringLength(3)]
        public string Core_Practice_HTP_Deviation { get; set; }

        [Column("Other Deviaton")]
        [StringLength(3)]
        public string Other_Deviaton { get; set; }

        [Column("Ending Phase")]
        public int? Ending_Phase { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(8)]
        public string Reaudit { get; set; }

        [Column("End Date", TypeName = "date")]
        public DateTime? End_Date { get; set; }

        [Column("End Time")]
        [StringLength(10)]
        public string End_Time { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
    }
}

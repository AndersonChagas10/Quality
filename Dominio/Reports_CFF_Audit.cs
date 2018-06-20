namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reports_CFF_Audit
    {
        [Column("Plant Number")]
        public int? Plant_Number { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Shift { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Period { get; set; }

        public int? Set { get; set; }

        [Column("Start Date", TypeName = "datetime2")]
        public DateTime? Start_Date { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Side { get; set; }

        [Column("Audit Area")]
        public string Audit_Area { get; set; }

        [Column("Set Start Date", TypeName = "date")]
        public DateTime? Set_Start_Date { get; set; }

        [Column("Set Start Time")]
        [StringLength(5)]
        public string Set_Start_Time { get; set; }

        [Column("Set End Date", TypeName = "date")]
        public DateTime? Set_End_Date { get; set; }

        [Column("Set End Time")]
        [StringLength(5)]
        public string Set_End_Time { get; set; }

        public string Auditor { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string Reaudit { get; set; }

        public decimal? Defects { get; set; }

        public decimal? Cut { get; set; }

        [Column("Fold/Flap")]
        public decimal? Fold_Flap { get; set; }

        public decimal? Puncture { get; set; }

        [Key]
        [Column("Defect Counter", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Defect_Counter { get; set; }
    }
}

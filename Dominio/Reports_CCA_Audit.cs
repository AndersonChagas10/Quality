namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reports_CCA_Audit
    {
        [Column("Plant Number")]
        public int? Plant_Number { get; set; }

        [Column("Audit Area")]
        public string Audit_Area { get; set; }

        [Column("Set Start Date", TypeName = "date")]
        public DateTime? Set_Start_Date { get; set; }

        [Column("Set Start Time")]
        [StringLength(5)]
        public string Set_Start_Time { get; set; }

        [Key]
        [Column("Audit Period", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Audit_Period { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Shift { get; set; }

        [Key]
        [Column("Audit/Reaudit", Order = 2)]
        [StringLength(8)]
        public string Audit_Reaudit { get; set; }

        public decimal? Specks { get; set; }

        public decimal? Dressing { get; set; }

        [Column("Single Hairs")]
        public decimal? Single_Hairs { get; set; }

        public decimal? Clusters { get; set; }

        public decimal? Hide { get; set; }

        public decimal? Defects { get; set; }

        [Key]
        [Column("No of Def", Order = 3)]
        [StringLength(3)]
        public string No_of_Def { get; set; }

        [Key]
        [Column("Cattle Type", Order = 4)]
        [StringLength(5)]
        public string Cattle_Type { get; set; }

        [Key]
        [Column("Chain Speed", Order = 5)]
        public decimal Chain_Speed { get; set; }

        [Key]
        [Column("Lot #", Order = 6)]
        public decimal Lot__ { get; set; }

        [Key]
        [Column("Mud Score", Order = 7)]
        public decimal Mud_Score { get; set; }

        [Column("Auditor Name Global")]
        public string Auditor_Name_Global { get; set; }

        [Column("Set End Date", TypeName = "date")]
        public DateTime? Set_End_Date { get; set; }

        [Column("Set End Time")]
        [StringLength(5)]
        public string Set_End_Time { get; set; }
    }
}

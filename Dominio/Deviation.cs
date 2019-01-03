namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Deviation")]
    public partial class Deviation
    {
        public int Id { get; set; }

        public int ParCompany_Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public decimal Evaluation { get; set; }

        public decimal Sample { get; set; }

        public int AlertNumber { get; set; }

        public decimal Defects { get; set; }

        public DateTime DeviationDate { get; set; }

        public DateTime AddDate { get; set; }

        public bool? sendMail { get; set; }

        public string DeviationMessage { get; set; }

        public string Status { get; set; }

        public string SendDate { get; set; }

        public int? EmailContent_Id { get; set; }

        [ForeignKey("EmailContent_Id")]
        public virtual EmailContent EmailContent { get; set; }
    }
}

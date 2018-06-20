namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("manDataCollectIT")]
    public partial class manDataCollectIT
    {
        public int Id { get; set; }

        public DateTime? instantDatetime { get; set; }

        public DateTime? referenceDatetime { get; set; }

        public int? userSGQ_id { get; set; }

        public int? parCompany_id { get; set; }

        public int? parFrequency_id { get; set; }

        public int? shift { get; set; }

        [StringLength(110)]
        public string dataType { get; set; }

        public decimal? amountData { get; set; }

        public int? parMeasurementUnit_Id { get; set; }

        [StringLength(800)]
        public string comments { get; set; }

        public DateTime addDate { get; set; }

        public DateTime? alterDate { get; set; }

        public bool isActive { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Result_Level3_Photos
    {
        public int ID { get; set; }

        public int? Result_Level3_Id { get; set; }

        [Column(TypeName = "text")]
        public string Photo_Thumbnaills { get; set; }

        [Column(TypeName = "text")]
        public string Photo { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}

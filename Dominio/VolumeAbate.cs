namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VolumeAbate")]
    public partial class VolumeAbate
    {
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public int Volume { get; set; }

        public int UnidadeId { get; set; }
    }
}

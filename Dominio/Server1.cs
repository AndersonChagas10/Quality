namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangFire.Server")]
    public partial class Server1
    {
        [StringLength(100)]
        public string Id { get; set; }

        public string Data { get; set; }

        public DateTime LastHeartbeat { get; set; }
    }
}

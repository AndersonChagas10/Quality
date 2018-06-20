namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConsolidationLevel1XCluster
    {
        public int Id { get; set; }

        public int? ConsolidationLevel1_Id { get; set; }

        public int? ParCluster_Id { get; set; }
    }
}

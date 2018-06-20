namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConsolidationLevel2XCluster
    {
        public int Id { get; set; }

        public int? ConsolidationLevel2_Id { get; set; }

        public int? ParCluster_Id { get; set; }
    }
}

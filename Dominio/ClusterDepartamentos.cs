namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClusterDepartamentos
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Cluster { get; set; }

        [StringLength(100)]
        public string Departamento { get; set; }
    }
}

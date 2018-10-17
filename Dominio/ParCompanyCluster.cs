namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParCompanyCluster")]
    public partial class ParCompanyCluster : BaseModel
    {
        public int Id { get; set; }

        public int ParCompany_Id { get; set; }

        public int ParCluster_Id { get; set; }

        public bool Active { get; set; }

        public virtual ParCluster ParCluster { get; set; }

        public virtual ParCompany ParCompany { get; set; }
    }
}

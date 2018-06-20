namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CollectionLevel2XCollectionJson
    {
        public int ID { get; set; }

        public int CollectionLevel2_Id { get; set; }

        public int CollectionJson_Id { get; set; }

        public virtual CollectionJson CollectionJson { get; set; }

        public virtual CollectionLevel2 CollectionLevel2 { get; set; }
    }
}

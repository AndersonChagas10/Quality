namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CollectionLevel2XParHeaderField
    {
        public int Id { get; set; }

        public int CollectionLevel2_Id { get; set; }

        public int ParHeaderField_Id { get; set; }

        [Required]
        public string ParHeaderField_Name { get; set; }

        public int ParFieldType_Id { get; set; }

        [Required]
        public string Value { get; set; }

        public int? Evaluation { get; set; }

        public int? Sample { get; set; }

        public virtual CollectionLevel2 CollectionLevel2 { get; set; }

        public virtual ParFieldType ParFieldType { get; set; }

        public virtual ParHeaderField ParHeaderField { get; set; }
    }
}

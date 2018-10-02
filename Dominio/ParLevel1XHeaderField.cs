namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel1XHeaderField
    {
        public int Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParHeaderField_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public bool? IsRequired { get; set; }

        public bool? DefaultSelected { get; set; }

        [StringLength(100)]
        public string HeaderFieldGroup { get; set; }

        public virtual ParHeaderField ParHeaderField { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}

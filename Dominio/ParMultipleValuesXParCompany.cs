namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParMultipleValuesXParCompany")]
    public partial class ParMultipleValuesXParCompany
    {
        public int Id { get; set; }

        public int? ParMultipleValues_Id { get; set; }

        public int? Parent_ParMultipleValues_Id { get; set; }

        public int ParCompany_Id { get; set; }

        [StringLength(255)]
        public string HashKey { get; set; }

        public bool IsActive { get; set; }

        public int ParLevel1_Id { get; set; }

        public int? ParHeaderField_Id { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParHeaderField ParHeaderField { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }

        public virtual ParMultipleValues ParMultipleValues { get; set; }

        public virtual ParMultipleValues ParMultipleValues1 { get; set; }
    }
}

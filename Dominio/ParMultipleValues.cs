namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParMultipleValues : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParMultipleValues()
        {
        }

        public int Id { get; set; }

        public int ParHeaderField_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public decimal PunishmentValue { get; set; }

        public bool Conformity { get; set; }

        public bool IsActive { get; set; }

        public bool? IsDefaultOption { get; set; }

        public virtual ParHeaderField ParHeaderField { get; set; }
    }
}

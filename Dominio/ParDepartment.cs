namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParDepartment")]
    public partial class ParDepartment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParDepartment()
        {
            ParLevel2 = new HashSet<ParLevel2>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool Active { get; set; }
       
        public string Hash { get; set; }

        [DisplayName("É filho de")]
        public int? Parent_Id { get; set; }

        [ForeignKey("Parent_Id")]
        public virtual ParDepartment ParDepartmentPai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParLevel2> ParLevel2 { get; set; }
    }
}
